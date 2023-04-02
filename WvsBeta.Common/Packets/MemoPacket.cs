using K4os.Compression.LZ4.Internal;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WvsBeta.Common.Sessions;
using WvsBeta.Database;

namespace WvsBeta.Common.Packets
{
    public enum MemoResult : byte
    {
        Receive = 0x01,
        SendSuccess = 0x02,
        SendError = 0x03
    }
    public enum MemoSendError : byte
    {
        CharAlreadyOnline = 0x00,
        WrongIGN = 0x01,
        ReceiverInboxFull = 0x02
    }
    public class MemoPacket
    {
        public static Packet MemoSend(IList<Memo> memos)
        {
            Packet pw = new Packet(ServerMessages.MEMO_ACTION);
            pw.WriteByte((byte)MemoResult.Receive);
            pw.WriteByte((byte)memos.Count);
            foreach (Memo memo in memos.ToList())
            {
                memo.Encode(pw);
            }
            return pw;
        }
        public static Packet MemoSuccess()
        {
            Packet pw = new Packet(ServerMessages.MEMO_ACTION);
            pw.WriteByte((byte)MemoResult.SendSuccess);
            return pw;
        }

        public static Packet MemoError(MemoSendError err)
        {
            Packet pw = new Packet(ServerMessages.MEMO_ACTION);
            pw.WriteByte((byte)MemoResult.SendError);
            pw.WriteByte((byte)err);
            return pw;
        }
    }

    public class Memo
    {
        public int ID { get; }
        public int From { get; }
        public int To { get; }
        public string Message { get; }
        public string FromName { get; }
        public long Sent { get; }
        public long Read { get; set; }

        public Memo(MySqlDataReader reader)
        {
            ID = reader.GetInt32("id");
            From = reader.GetInt32("from");
            To = reader.GetInt32("to");
            Message = reader.GetString("message");
            FromName = reader.GetString("fromname");
            Sent = reader.GetInt64("sent");
            Read = reader.GetInt64("read");
        }
        public Memo(long ctime, int fromId, string fromName, int to, string message, MySQL_Connection conn)
        {
            int _insertid = 0;
            From = fromId;
            To = to;
            Message = message;
            FromName = fromName;
            Sent = ctime;
            Read = 0;
            
            conn.RunTransaction((cmd) =>
            {
                cmd.CommandText = "INSERT INTO memos (`from`, `to`, `message`, `sent`, `read`, `fromname`) VALUES (@from, @to, @message, @sent, @read, @fromname)";
                cmd.Parameters.AddWithValue("@from", From);
                cmd.Parameters.AddWithValue("@to", To);
                cmd.Parameters.AddWithValue("@message", Message);
                cmd.Parameters.AddWithValue("@sent", Sent);
                cmd.Parameters.AddWithValue("@read", Read);
                cmd.Parameters.AddWithValue("@fromname", FromName);
                cmd.ExecuteNonQuery();
                _insertid = (int)cmd.LastInsertedId;
            });
            ID = _insertid;
        }
        public void Encode(Packet pw)
        {
            pw.WriteInt(ID);
            pw.WriteString(FromName);
            pw.WriteString(Message);
            pw.WriteLong(Sent);
        }
    }
}
