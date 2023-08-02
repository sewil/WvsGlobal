using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;
using WvsBeta.Common.Packets;
using WvsBeta.Database;

namespace WvsBeta.Center
{
    public class CharacterMemos
    {
        private readonly CenterCharacter chr;
        public IList<Memo> Memos { get; } = new List<Memo>();
        static CenterServer S => CenterServer.Instance;
        static MySQL_Connection Db => S.CharacterDatabase;

        private CharacterMemos(CenterCharacter chr)
        {
            this.chr = chr;
        }

        public static void Load(CenterCharacter chr)
        {
            chr.Memos = new CharacterMemos(chr);
            chr.Memos.Load();
        }

        public void Load()
        {
            using (var reader = CenterServer.Instance.CharacterDatabase.RunQuery(
                "SELECT * FROM memos WHERE `read` = 0 AND `to` = @cid",
                "@cid", chr.ID
            ) as MySqlDataReader)
            {
                while (reader.Read())
                {
                    Memo memo = new Memo(reader);
                    Memos.Add(memo);
                }
            }
        }

        public void Save()
        {
            CenterServer.Instance.CharacterDatabase.RunTransaction((cmd) =>
            {
                cmd.CommandText = "DELETE FROM memos WHERE `to` = @cid AND `read` = 0";
                cmd.Parameters.AddWithValue("@cid", chr.ID);
                cmd.ExecuteNonQuery();
                foreach (var memo in Memos.ToList())
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = "INSERT INTO memos (`from`, `to`, `message`, `sent`, `read`, `fromname`, `gift_cashid`) VALUES (@from, @to, @message, @sent, @read, @fromname, @giftcashid)";
                    cmd.Parameters.AddWithValue("@from", memo.From);
                    cmd.Parameters.AddWithValue("@to", memo.To);
                    cmd.Parameters.AddWithValue("@message", memo.Message);
                    cmd.Parameters.AddWithValue("@sent", memo.Sent);
                    cmd.Parameters.AddWithValue("@read", memo.Read);
                    cmd.Parameters.AddWithValue("@fromname", memo.FromName);
                    cmd.Parameters.AddWithValue("@giftcashid", memo.GiftCashId);
                    cmd.ExecuteNonQuery();
                    if (memo.Read > 0) Memos.Remove(memo);
                }
            });
        }
        public void SendMemos()
        {
            if (Memos.Count == 0) return;
            var unreadMemos = Memos.Where(m => m.Read == 0).Take(byte.MaxValue).ToList();
            if (unreadMemos.Count == 0) return;
            var pw = MemoPacket.MemoSend(Memos);
            chr.SendPacket(pw);
        }
        public static void HandleAddMemo(int fromId, string recipientName, string message, bool isNote, long? giftCashId)
        {
            var from = S.FindCharacter(fromId);
            if (from == null) return;
            int toId;
            using (var reader = Db.RunQuery("SELECT id FROM CHARACTERS WHERE `name` = @name", "@name", recipientName) as MySqlDataReader)
            {
                if (!reader.Read())
                {
                    from.SendPacket(MemoPacket.MemoError(MemoSendError.WrongIGN));
                    return;
                }
                toId = reader.GetInt32("id");
            }
            HandleAddMemo(from, toId, message, isNote, giftCashId);
        }
        public static void HandleAddMemo(int fromId, int toId, string message, bool onlyOffline, long? giftCashId)
        {
            var from = S.FindCharacter(fromId);
            if (from == null) return;
            HandleAddMemo(from, toId, message, onlyOffline, giftCashId);
        }
        public static void HandleAddMemo(CenterCharacter from, int toId, string message, bool onlyOffline, long? giftCashId)
        {
            long ctime = MasterThread.CurrentTime;
            CenterCharacter to = S.FindCharacter(toId, true);

            if (onlyOffline && to != null)
            {
                from.SendPacket(MemoPacket.MemoError(MemoSendError.CharAlreadyOnline));
                return;
            }

            var memo = new Memo(ctime, from.ID, from.Name, toId, message, CenterServer.Instance.CharacterDatabase, giftCashId);
            if (to != null)
            {
                to.Memos.Memos.Add(memo);
                if (!to.isCCing && !to.InCashShop) to.Memos.SendMemos();
            }

            from.SendPacket(MemoPacket.MemoSuccess());
        }
        public void HandleMemoRead(int[] memos)
        {
            long ctime = MasterThread.CurrentTime;
            foreach (int memo in memos)
            {
                Memo cmemo = Memos.FirstOrDefault(m => m.ID == memo);
                if (cmemo == null) continue;
                cmemo.Read = ctime;
            }
        }
    }
}
