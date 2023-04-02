using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;
using WvsBeta.Common.Extensions;
using WvsBeta.Common.Packets;
using WvsBeta.Common.Sessions;
using WvsBeta.Database;

namespace WvsBeta.Center.Handlers
{
    public static class MemoHandler
    {
        static CenterServer S => CenterServer.Instance;
        static MySQL_Connection Db => S.CharacterDatabase;
        public static void Load()
        {
            var memos = CenterServer.Instance.Memos;
            using (var reader = CenterServer.Instance.CharacterDatabase.RunQuery("SELECT * FROM memos WHERE `read` = 0 ORDER BY `to`") as MySqlDataReader)
            {
                while (reader.Read())
                {
                    var memo = new Memo(reader);
                    if (!memos.ContainsKey(memo.To)) memos.Add(memo.To, new List<Memo>());
                    memos[memo.To].Add(memo);
                }
            }
        }

        public static void SaveChr(Character chr)
        {
            if (!CenterServer.Instance.Memos.TryGetValue(chr.ID, out var memos)) return;
            CenterServer.Instance.CharacterDatabase.RunTransaction((cmd) =>
            {
                cmd.CommandText = "DELETE FROM memos WHERE (`from` = @cid OR `to` = @cid) AND `read` = 0";
                cmd.Parameters.AddWithValue("@cid", chr.ID);
                cmd.ExecuteNonQuery();
                foreach (var memo in memos.ToList())
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = "INSERT INTO memos (`from`, `to`, `message`, `sent`, `read`, `fromname`) VALUES (@from, @to, @message, @sent, @read, @fromname)";
                    cmd.Parameters.AddWithValue("@from", memo.From);
                    cmd.Parameters.AddWithValue("@to", memo.To);
                    cmd.Parameters.AddWithValue("@message", memo.Message);
                    cmd.Parameters.AddWithValue("@sent", memo.Sent);
                    cmd.Parameters.AddWithValue("@read", memo.Read);
                    cmd.Parameters.AddWithValue("@fromname", memo.FromName);
                    cmd.ExecuteNonQuery();
                    if (memo.Read > 0) memos.Remove(memo);
                }
            });
        }

        public static void SendMemos(Character chr)
        {
            if (!CenterServer.Instance.Memos.TryGetValue(chr.ID, out IList<Memo> memos)) return;
            var unreadMemos = memos.Where(m => m.Read == 0).ToList();
            if (unreadMemos.Count == 0) return;

            var pw = MemoPacket.MemoSend(unreadMemos);
            chr.SendPacket(pw);
        }
        public static void HandleAddMemo(int fromId, string name, string message, bool isNote)
        {
            long ctime = MasterThread.FileTime;
            var from = S.FindCharacter(fromId);
            if (from == null) return;

            int toId;
            using (var reader = Db.RunQuery("SELECT id FROM CHARACTERS WHERE `name` = @name", "@name", name) as MySqlDataReader)
            {
                if (!reader.Read())
                {
                    from.SendPacket(MemoPacket.MemoError(MemoSendError.WrongIGN));
                    return;
                }
                toId = reader.GetInt32("id");
            }
            if (isNote && S.FindCharacter(toId, true) != null)
            {
                from.SendPacket(MemoPacket.MemoError(MemoSendError.CharAlreadyOnline));
                return;
            }

            var memo = new Memo(ctime, from.ID, from.Name, toId, message, CenterServer.Instance.CharacterDatabase);
            CenterServer.Instance.Memos.SafeAdd(from.ID, memo);

            from.SendPacket(MemoPacket.MemoSuccess());
        }
        public static void HandleMemoRead(int cid, int memoid)
        {
            if (!CenterServer.Instance.Memos.TryGetValue(cid, out var memos)) return;
            var memo = memos.FirstOrDefault(m => m.ID == memoid);
            if (memo == null) return;
            memo.Read = MasterThread.FileTime;
        }
    }
}
