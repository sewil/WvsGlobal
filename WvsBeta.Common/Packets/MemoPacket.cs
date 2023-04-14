using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using WvsBeta.Common.Extensions;
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
            if (memos.Count > byte.MaxValue) throw new ArgumentException("Too many memos!");
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

        public static void SendMemo(ConnectionSession session, int charid, string recipientName, string message, long? giftCashId, bool onlyOffline)
        {
            SendMemo(session, charid, null, recipientName, message, giftCashId, onlyOffline);
        }
        public static void SendMemo(ConnectionSession session, int charid, int recipientId, string message, long? giftCashId, bool onlyOffline)
        {
            SendMemo(session, charid, recipientId, null, message, giftCashId, onlyOffline);
        }
        private static void SendMemo(ConnectionSession session, int charid, int? recipientId, string recipientName, string message, long? giftCashId, bool onlyOffline)
        {
            var pw = new Packet(ISClientMessages.MemoAdd);
            pw.WriteInt(charid);
            pw.WriteBool(recipientId != null);
            if (recipientId != null) pw.WriteInt(recipientId.Value);
            else pw.WriteString(recipientName);
            pw.WriteString(message);
            pw.WriteBool(giftCashId != null);
            if (giftCashId != null) pw.WriteLong(giftCashId.Value);
            pw.WriteBool(onlyOffline);
            session.SendPacket(pw);
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
        public long? GiftCashId { get; }

        public Memo(MySqlDataReader reader)
        {
            ID = reader.GetInt32("id");
            From = reader.GetInt32("from");
            To = reader.GetInt32("to");
            Message = reader.GetString("message");
            FromName = reader.GetString("fromname");
            Sent = reader.GetInt64("sent");
            Read = reader.GetInt64("read");
            GiftCashId = reader.GetNullableFieldValue<long>("gift_cashid");
        }
        public Memo(long ctime, int fromId, string fromName, int to, string message, MySQL_Connection conn, long? giftCashId = null)
        {
            int _insertid = 0;
            From = fromId;
            To = to;
            Message = message;
            FromName = fromName;
            Sent = ctime;
            Read = 0;
            GiftCashId = giftCashId;
            
            conn.RunTransaction((cmd) =>
            {
                cmd.CommandText = "INSERT INTO memos (`from`, `to`, `message`, `sent`, `read`, `fromname`, `gift_cashid`) VALUES (@from, @to, @message, @sent, @read, @fromname, @cashid)";
                cmd.Parameters.AddWithValue("@from", From);
                cmd.Parameters.AddWithValue("@to", To);
                cmd.Parameters.AddWithValue("@message", Message);
                cmd.Parameters.AddWithValue("@sent", Sent);
                cmd.Parameters.AddWithValue("@read", Read);
                cmd.Parameters.AddWithValue("@fromname", FromName);
                cmd.Parameters.AddWithValue("@cashid", GiftCashId);
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
