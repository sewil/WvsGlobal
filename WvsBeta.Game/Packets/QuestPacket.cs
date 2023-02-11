using System;
using System.Collections.Generic;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Objects;
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
            var pw = new QuestPacket(questID, QuestState.Available);
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
        public static void ShowQuestActionResultError(GameCharacter chr, QuestActionResult result)
        {
            ShowQuestActionResult(chr, result, 0, 0);
        }
        public static void ShowQuestActionResult(GameCharacter chr, QuestActionResult result, int npcID, short currentQuest, params short[] nextQuests)
        {
            var pw = new Packet(ServerMessages.QUEST_ACTION_RESULT);

            pw.WriteByte((byte)result);
            pw.WriteShort(currentQuest); // Send 0 to ignore
            if (result == QuestActionResult.Success)
            {
                pw.WriteInt(npcID);
                for (int i = 0; i < nextQuests.Length; i++)
                {
                    pw.WriteShort(nextQuests[i]); // Next quest
                }
                pw.WriteShort(0);
            }
            chr.SendPacket(pw);
        }
        public enum QuestActionResult
        {
            // < 4 = Nothing happens (0xFF > 9 so it goto END;)
            // 4 = Complete with npc window
            // 5 = The quest has ended due to an unknown error.
            // 6 = Item inventory is full
            // 7 = You have not enough mesos.
            // 8 = Failed to retrieve the equipped item. Please unequip it first.
            // 9 = You cannot carry more than one for this item.
            Success = 4,
            UnknownError = 5,
            InventoryFull = 6,
            NotEnoughMesos = 7,
            FailedRetrieveEquippedItem = 8,
            CannotCarryMoreThanOne = 9
        }
        class QuestException:Exception
        {
            public QuestActionResult Result { get; private set; }
            public QuestException(QuestActionResult result)
            {
                Result = result;
            }
        }
        public static void HandleQuestCheck(GameCharacter chr, WZQuestCheck check)
        {
            foreach (var item in check.Items) { if (!chr.Inventory.CanExchangeItem(item.ItemID, (short)-item.Count)) throw new QuestException(QuestActionResult.FailedRetrieveEquippedItem); }
            if (check.Mesos > 0 && !chr.Inventory.CanExchangeMesos(-check.Mesos)) throw new QuestException(QuestActionResult.NotEnoughMesos);
            if (check.LvMin > 0 && chr.Level < check.LvMin) throw new QuestException(QuestActionResult.UnknownError);
            if (check.LvMax > 0 && chr.Level > check.LvMax) throw new QuestException(QuestActionResult.UnknownError);

            foreach (var qt in check.Quests)
            {
                if (qt.NewState == QuestState.Completed)
                {
                    chr.Quests.SetComplete(qt.QuestID);
                }
            }
        }
        public static void HandleQuestAct(GameCharacter chr, WZQuestAct act)
        {
            foreach (var item in act.Items) { if (!chr.Inventory.CanExchangeItem(item.ItemID, item.Count)) throw new QuestException(QuestActionResult.InventoryFull); }
            if (act.Mesos > 0 && !chr.Inventory.CanExchangeMesos(act.Mesos)) throw new QuestException(QuestActionResult.NotEnoughMesos);
            if (act.Exp > 0 && chr.Level == 200) throw new QuestException(QuestActionResult.UnknownError);

            if (act.Items.Count > 0) { foreach (var item in act.Items) { chr.Inventory.ExchangeItem(item.ItemID, item.Count); } }
            if (act.Mesos > 0)
            {
                chr.Inventory.ExchangeMesos(act.Mesos);
                chr.SendPacket(MessagePacket.MesosChanged(act.Mesos));
            }
            if (act.Exp > 0) chr.AddEXP(act.Exp, false, true);

            if (act.Stage.Stage == QuestStage.Start)
            {
                chr.Quests.AddNewQuest(act.Stage.Quest.QuestID);
            }
            else if (act.Stage.Stage == QuestStage.Complete)
            {
                if (act.NextQuest > 0)
                {
                    if (!DataProvider.Quests.TryGetValue(act.NextQuest, out WZQuestData nextQuest)) throw new QuestException(QuestActionResult.UnknownError);
                    ShowQuestActionResult(chr, QuestActionResult.Success, nextQuest.OnStart.Check.NpcID, 0, act.NextQuest);
                }
                else
                {
                    chr.Quests.SetComplete(act.Stage.Quest.QuestID);
                    ShowQuestActionResult(chr, QuestActionResult.Success, act.Stage.Check.NpcID, act.Stage.Quest.QuestID);
                }
            }
        }
        public static void HandleQuestStage(GameCharacter chr, WZQuestStage stage)
        {
            try
            {
                HandleQuestCheck(chr, stage.Check);
                HandleQuestAct(chr, stage.Act);
            }
            catch (QuestException e)
            {
                ShowQuestActionResultError(chr, e.Result);
            }
        }
        public static void HandleAction(GameCharacter chr, Packet packet)
        {
            byte type = packet.ReadByte(); // 0 = lost item, 1 = start, 2 = complete, 3 = forfeit
            short qid = packet.ReadShort();
            int npcID = 0;
            NPCData npc;
            if (type != 3 && type != 0)
            {
                npcID = packet.ReadInt();
                if (!DataProvider.NPCs.TryGetValue(npcID, out npc))
                {
                    Program.MainForm.LogAppend("Npc " + npcID + " not found!");
                    ShowQuestActionResultError(chr, QuestActionResult.UnknownError);
                    return;
                }
            }
            switch (type)
            {
                case 0:
                    {
                        // lost item [42] [00] [E9 03] [01 00 00 00] [1B 82 3D 00]
                        int amount = packet.ReadInt();
                        int itemid = packet.ReadInt();
                        if (!chr.Inventory.TryExchangeItem(itemid, (short)amount))
                        {
                            ShowQuestActionResultError(chr, QuestActionResult.InventoryFull);
                        }
                        break;
                    }
                case 1:
                    {
                        //start quest 42 01 E8 03 35 08 00 00
                        if (!DataProvider.Quests.TryGetValue(qid, out WZQuestData qd))
                        {
                            ShowQuestActionResult(chr, QuestActionResult.UnknownError, npcID, qid);
                            return;
                        }
                        HandleQuestStage(chr, qd.OnStart);
                    }
                    break;
                case 2:
                    {
                        // complete quest  42 02 E8 03 34 08 00 00 FF FF FF FF
                        packet.ReadInt();

                        if (!DataProvider.Quests.TryGetValue(qid, out WZQuestData qd))
                        {
                            ShowQuestActionResult(chr, QuestActionResult.UnknownError, npcID, qid);
                            return;
                        }
                        HandleQuestStage(chr, qd.OnComplete);
                    }
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
