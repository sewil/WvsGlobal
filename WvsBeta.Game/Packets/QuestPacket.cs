using System.Collections.Generic;
using System.Security.Cryptography;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Sessions;
using WvsBeta.Game.Packets;

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
            var pw = new QuestPacket(questID, QuestState.InProgress);
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
        public static void QuestClear(GameCharacter chr, short qid)
        {
            var pw = new Packet(ServerMessages.QUEST_CLEAR);
            pw.WriteShort(qid);
            chr.SendPacket(pw);
        }
        public static void HandleAction(GameCharacter chr, Packet packet)
        {
            byte type = packet.ReadByte(); // 1 = start, 2 = complete, 3 = forfeit
            short qid = packet.ReadShort();
            int npcid = 0;
            NPCData npc;
            if (type != 3)
            {
                npcid = packet.ReadInt();
                if (!DataProvider.NPCs.TryGetValue(npcid, out npc))
                {
                    Program.MainForm.LogAppend("Npc " + npcid + " not found!");
                    return;
                }
            }
            switch (type)
            {
                case 1:
                    //start quest 42 01 E8 03 35 08 00 00
                    chr.Quests.AddNewQuest(qid);
                    break;
                case 2:
                    // open quest  42 02 E8 03 34 08 00 00 FF FF FF FF
                    int status = packet.ReadInt();
                    //Packet pw = new Packet(ServerMessages.SCRIPT_MESSAGE);
                    //pw.WriteByte(0x04);
                    //pw.WriteInt(npcid);
                    //pw.WriteByte(0);
                    //pw.WriteString("");
                    //pw.WriteBool(false);
                    //pw.WriteBool(true);

                    //chr.SendPacket(pw);
                    InventoryOperationPacket.NoChange(chr);
                    break;
                case 3: // Forfeit
                        // 42 03 EB 03
                    QuestPacket.SendRemoveQuest(chr, qid);
                    break;
                default:
                    break;
            }
        }
    }
}
