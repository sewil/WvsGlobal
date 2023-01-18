using log4net;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WvsBeta.Common;
using WvsBeta.Common.Character;
using WvsBeta.Common.Sessions;


namespace WvsBeta.Shop
{
    public class Character : CharacterBase
    {
        private static ILog _characterLog = LogManager.GetLogger("CharacterLog");
        public static ILog CashLog = LogManager.GetLogger("CashLog");

        public int UserID { get; set; }
        public string UserName { get; set; }
        public long PetCashId { get; set; }
        public int DoB { get; set; }

        public override BaseCharacterInventory Inventory { get; set; }
        public CharacterCashLocker Locker { get; private set; }
        public int[] Wishlist { get; } = new int[10];

        public Player Player { get; set; }

        public enum TransactionType
        {
            MaplePoints,
            NX
        }

        public Dictionary<TransactionType, List<(string reason, int amount)>> BoughtItems { get; } = new Dictionary<TransactionType, List<(string, int)>>();

        public Character(int CharacterID)
        {
            CharacterStat.ID = CharacterID;
        }


        public void SendPacket(Packet pw)
        {
            Player?.Socket?.SendPacket(pw);
        }

        public void Save()
        {
            Server.Instance.CharacterDatabase.RunTransaction(comm =>
            {
                comm.CommandText = "DELETE FROM character_wishlist WHERE charid = " + CharacterStat.ID;
                comm.ExecuteNonQuery();

                var wishlistQuery = new StringBuilder();

                wishlistQuery.Append("INSERT INTO character_wishlist VALUES ");
                wishlistQuery.Append(string.Join(", ", Wishlist.Select(serial => "(" + CharacterStat.ID + ", " + serial + ")")));

                comm.CommandText = wishlistQuery.ToString();
                comm.ExecuteNonQuery();

            }, Program.MainForm.LogAppend);


            Inventory.SaveInventory();
            Inventory.SaveCashItems(Locker);

            SaveSales();
        }

        public enum LoadFailReasons
        {
            None,
            UnknownCharacter,
            NotFromPreviousIP,
            UserAlreadyOnline,
            TransitionTimeout
        }

        public LoadFailReasons Load(string IP)
        {
            _characterLog.Debug($"Loading character {CharacterStat.ID} from IP {IP}...");

            using (var data = (MySqlDataReader)Server.Instance.CharacterDatabase.RunQuery(
                    "SELECT characters.*, users.char_delete_password, users.admin, users.username AS uname FROM characters LEFT JOIN users ON users.id = characters.userid WHERE characters.id = " +
                    CharacterStat.ID))
            {
                if (!data.Read())
                {
                    _characterLog.Debug("Loading failed: unknown character.");
                    return LoadFailReasons.UnknownCharacter;
                }


                UserID = data.GetInt32("userid");
                GMLevel = data.GetByte("admin");
                UserName = data.GetString("uname");
                PetCashId = data.GetInt64("pet_cash_id");
                DoB = data.GetInt32("char_delete_password");
                
                CharacterStat.LoadFromReader(data);
            }

            
            BoughtItems[TransactionType.MaplePoints] = new List<(string, int)>();
            BoughtItems[TransactionType.NX] = new List<(string, int)>();

            Inventory = new CharacterInventory(this);
            Inventory.LoadInventory();

            Locker = new CharacterCashLocker(this);
            Locker.Load();

            using (var data = (MySqlDataReader)Server.Instance.CharacterDatabase.RunQuery("SELECT serial FROM character_wishlist WHERE charid = @charid LIMIT 10", "@charid", CharacterStat.ID))
            {
                int i = 0;
                while (data.Read())
                {
                    Wishlist[i++] = data.GetInt32(0);
                }
            }
            //using (var data = (MySqlDataReader)Server.Instance.CharacterDatabase.RunQuery("SELECT skillid FROM skills WHERE charid = @charid", "@charid", CharacterStat.ID))
            //{
            //    while (data.Read())
            //    {
            //        ((CharacterInventory)Skills).Add(data.GetInt32(0));
            //    }
            //}

            RedisBackend.Instance.SetPlayerOnline(
                UserID,
                Server.Instance.GetOnlineId()
            );

            _characterLog.Debug("Loaded!");

            return LoadFailReasons.None;
        }


        public void SetupLogging()
        {
            ThreadContext.Properties["UserID"] = UserID;
            ThreadContext.Properties["CharacterID"] = CharacterStat.ID;
            ThreadContext.Properties["CharacterName"] = CharacterStat.Name;
        }

        public static void RemoveLogging()
        {
            ThreadContext.Properties.Remove("UserID");
            ThreadContext.Properties.Remove("CharacterID");
            ThreadContext.Properties.Remove("CharacterName");
        }

        public (int nx, int maplepoints) GetCashStatus()
        {
            using (var data = Server.Instance.CharacterDatabase.RunQuery(@"
SELECT 
    SUM(IF(pointtype = 'maplepoints', amount, 0)) AS mp_sum, 
    SUM(IF(pointtype = 'nx', amount, 0)) AS nx_sum
FROM user_point_transactions
WHERE userid = @userid",
                "@userid", UserID
            ) as MySqlDataReader)
            {
                data.Read();

                var nxSum = data.IsDBNull(0) ? 0 : data.GetInt32("nx_sum");
                var mpSum = data.IsDBNull(1) ? 0 : data.GetInt32("mp_sum");

                return (
                    nxSum - BoughtItems[TransactionType.NX].Sum(x => x.amount),
                    mpSum - BoughtItems[TransactionType.MaplePoints].Sum(x => x.amount)
                );
            }
        }

        public void AddSale(string message, int amount, TransactionType paidWith)
        {
            CashLog.Info(message);
            BoughtItems[paidWith].Add((message, amount));
        }

        public void SaveSales()
        {
            if (BoughtItems.Select(x => x.Value.Count).Sum() == 0) return;

            Server.Instance.CharacterDatabase.RunTransaction(x =>
            {
                var sb = new StringBuilder();

                sb.AppendLine("INSERT INTO user_point_transactions VALUES ");

                var start = true;
                foreach (var boughtItem in BoughtItems)
                {

                    var boughtType = "";
                    switch (boughtItem.Key)
                    {
                        case TransactionType.MaplePoints: boughtType = "maplepoints"; break;
                        case TransactionType.NX: boughtType = "nx"; break;
                    }

                    foreach (var record in boughtItem.Value)
                    {
                        if (!start) sb.Append(',');
                        start = false;

                        sb.AppendFormat("(NULL, {0}, {1}, NOW(), '{2}', '{3}')\r\n",
                            UserID,
                            -record.amount,
                            MySqlHelper.EscapeString(record.reason),
                            boughtType
                        );
                    }
                }

                x.CommandText = sb.ToString();
                x.ExecuteNonQuery();
            });

            // Remove all sales
            foreach (var keyValuePair in BoughtItems)
            {
                keyValuePair.Value.Clear();
            }
        }
    }
}
