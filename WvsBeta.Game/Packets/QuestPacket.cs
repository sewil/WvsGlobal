using System.Collections.Generic;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Game
{
    public class QuestPacket : Packet
    {
        private QuestPacket(short questID, QuestState state) : base(ServerMessages.MESSAGE)
        {
            WriteByte((byte)MessageType.QuestRecord);
            WriteShort(questID);
            WriteByte((byte)state);
        }

        public static void SendRemoveQuest(GameCharacter chr, short questID)
        {
            var pw = new QuestPacket(questID, QuestState.NotStarted);
            chr.SendPacket(pw);
        }
        public static void SendQuestUpdateData(GameCharacter chr, short questID, string data)
        {
            var pw = new QuestPacket(questID, QuestState.Started);
            pw.WriteString(data);
            chr.SendPacket(pw);
        }

        public static void SendCompleteQuest(GameCharacter chr, short questID, long filetime)
        {
            var pw = new QuestPacket(questID, QuestState.Completed);
            pw.WriteLong(filetime);
            chr.SendPacket(pw);
        }

        public static void SendGainItemChat(GameCharacter chr, params KeyValuePair<int, int>[] pItems)
        {
            Packet pw = new Packet(ServerMessages.PLAYER_EFFECT);
            pw.WriteByte(0x03);
            pw.WriteByte((byte)pItems.Length);
            foreach (var kvp in pItems)
            {
                pw.WriteInt(kvp.Key);
                pw.WriteInt(kvp.Value);
            }
            chr.SendPacket(pw);
        }
    }
}
