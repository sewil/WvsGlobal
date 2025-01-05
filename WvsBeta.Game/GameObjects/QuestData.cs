using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Objects;
using WvsBeta.Database;

namespace WvsBeta.Game.GameObjects
{
    public class QuestData
    {
        public GameCharacter Character { get; }
        public short QuestID { get; }
        public string Data { get; set; }
        public QuestState State { get; set; }
        public bool ClearedItems { get; set; }
        public bool ClearedMobs { get; private set; }
        public WZQuestCheck Check { get; private set; }
        public long EndTime { get; set; }
        private WZQuestData wzQuest;
        private void SetWzQuestData()
        {
            if (GameDataProvider.Quests.TryGetValue(QuestID, out wzQuest))
            {
                Check = wzQuest.Stages[QuestStage.Complete].Check;
                if (Check.Mobs.Count == 0) ClearedMobs = true;
                if (Check.Items.Count == 0) ClearedItems = true;
            }
        }
        public QuestData(MySqlDataReader reader, GameCharacter character)
        {
            Character = character;
            QuestID = reader.GetInt16("questid");
            Data = reader.GetString("data");
            State = (QuestState)reader.GetByte("state");
            EndTime = reader.GetInt64("endtime");
            SetWzQuestData();
            if (State == QuestState.InProgress)
            {
                CheckItemsCleared();
                CheckMobsCleared();
            }
            else
            {
                ClearedMobs = true;
                ClearedItems = true;
            }
        }

        public QuestData(GameCharacter character, WZQuestData wzQuest, short questId, string data = "")
        {
            Character = character;
            QuestID = questId;
            State = QuestState.InProgress;
            Data = data;
            SetWzQuestData();
            if (Check?.Mobs.Count > 0 && Data == "")
            {
                Data = "".PadLeft(Check.Mobs.Count * 3, '0');
            }
        }

        public bool CanRepeat()
        {
            if (wzQuest == null || State != QuestState.Completed || EndTime == 0) return false;
            
            int intervalMins = wzQuest.Stages[QuestStage.Start].Check.IntervalMins;
            if (intervalMins <= 0) return false;

            long cTime = MasterThread.CurrentTime;
            long eTime = EndTime + intervalMins*60000;
            return eTime <= cTime;
        }
        public bool HasExpired()
        {
            if (wzQuest == null) return false;
            return wzQuest.Stages[QuestStage.Start].Check.HasExpired();
        }
        private int GetMobKills(int mobidx)
        {
            int stridx = mobidx * 3;
            if (stridx > Data.Length - 1) return 0;
            if (!int.TryParse(Data.Substring(stridx, 3), out int count)) return 0;
            return count;
        }
        public void UpdateMobKills(int mobidx, int kills)
        {
            int stridx = mobidx * 3;
            if (stridx > Data.Length - 1) return;
            string sKills = kills.ToString().PadLeft(3, '0');
            Data = Data.Remove(stridx, 3).Insert(stridx, sKills);
        }
        public bool MobKilled(int mobid)
        {
            if (Check == null || ClearedMobs || !Check.Mobs.TryGetValue(mobid, out QuestMob questMob)) return false;
            bool prevClearedMobs = ClearedMobs;
            int mobidx = questMob.ID;
            int mobkills = GetMobKills(mobidx);
            int newmobkills = mobkills + 1;
            if (mobkills < questMob.Count)
            {
                UpdateMobKills(mobidx, newmobkills);
            }
            if (newmobkills >= questMob.Count)
            {
                CheckMobsCleared();
            }
            QuestPacket.SendQuestUpdateData(Character, QuestID, Data);
            if (((!prevClearedMobs && ClearedMobs) && ClearedItems))
            {
                QuestPacket.QuestClear(Character, QuestID);
            }
            return true;
        }
        private void CheckMobsCleared()
        {
            if (Check == null || ClearedMobs || Check.Mobs.Count == 0) return;
            for (int i = 0; i < Data.Length; i+=3)
            {
                int count = int.Parse(Data.Substring(i, 3));
                int idx = i / 3;
                QuestMob questMob = Check.Mobs.Values.FirstOrDefault(m => m.ID == idx);
                if (questMob == null || count < questMob.Count) return;
            }
            ClearedMobs = true;
        }
        public void CheckItemsCleared()
        {
            if (Check == null || ClearedItems) return;
            foreach (var item in Check.Items.Select(i => i.Value))
            {
                if (!Character.Inventory.HasItemAmount(item.ItemID, item.Amount)) return;
            }
            ClearedItems = true;
        }
    }
}
