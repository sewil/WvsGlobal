using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using WvsBeta.Common.DataProviders;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Objects;
using WvsBeta.Game.GameObjects;

namespace WvsBeta.Game
{
    public class CharacterQuests
    {
        private GameCharacter Character { get; set; }

        public Dictionary<short, QuestData> Quests { get; } = new Dictionary<short, QuestData>();
        public bool HasQuest(short questID)
        {
            return Quests.ContainsKey(questID);
        }
        public bool HasQuestState(short questID, QuestState state)
        {
            return Quests.TryGetValue(questID, out QuestData quest) && quest.State == state;
        }
        public Dictionary<short, QuestData> GetQuests(bool wzFilter = true)
        {
            IEnumerable<KeyValuePair<short, QuestData>> quests = Quests;
            if (wzFilter)
            {
                quests = quests.Where((i) => DataProvider.Quests.ContainsKey(i.Key));
            }
            return quests
                .OrderBy(quest => quest.Key)
                .ToDictionary(x => x.Key, x => x.Value)
            ;
        }
        public CharacterQuests(GameCharacter character)
        {
            Character = character;
        }

        public void SaveQuests()
        {
            int charid = Character.ID;

            Server.Instance.CharacterDatabase.RunTransaction((cmd) =>
            {
                cmd.Parameters.AddWithValue("@charid", charid);

                cmd.CommandText = "DELETE FROM character_quests WHERE charid = @charid";
                cmd.ExecuteNonQuery();

                if (Quests.Count > 0)
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = "INSERT INTO character_quests (charid, questid, data, state, endtime) VALUES ";
                    cmd.CommandText += string.Join(", ", Quests.Select(kvp =>
                    {
                        return "(" +
                               charid + ", " +
                               kvp.Key + ", " +
                               "'" + MySqlHelper.EscapeString(kvp.Value.Data) + "', " +
                               (byte)kvp.Value.State + ", " +
                               kvp.Value.EndTime + " " +
                               ")";
                    }));
                    cmd.ExecuteNonQuery();
                }
            }, Program.MainForm.LogAppend);
        }

        public bool LoadQuests()
        {
            using (var data = (MySqlDataReader)Server.Instance.CharacterDatabase.RunQuery(
                    "SELECT * FROM character_quests WHERE charid = @charid",
                    "@charid", Character.ID
            ))
            {
                while (data.Read())
                {
                    var qd = new QuestData(data, Character);
                    Quests[qd.QuestID] = qd;
                }
            }
            return true;
        }
        public bool StartQuest(QuestData quest)
        {
            if (quest.HasExpired()) return false;
            QuestPacket.SendQuestUpdateData(Character, quest.QuestID, quest.Data);
            return true;
        }
        public bool RepeatQuest(QuestData quest)
        {
            if (!quest.CanRepeat()) return false;
            quest.State = QuestState.InProgress;
            quest.Data = "";
            return StartQuest(quest);
        }
        public bool StartQuest(short questID, string data = "")
        {
            if (Quests.TryGetValue(questID, out QuestData eQuest)) return RepeatQuest(eQuest);
            WZQuestData wzQuest = null;
            if (GameDataProvider.Quests.ContainsKey(questID)) wzQuest = GameDataProvider.Quests[questID];
            if (wzQuest?.Stages[QuestStage.Start].Check.HasExpired() == true) return false;
            return AddNewQuest(questID, wzQuest, data);
        }

        private bool AddNewQuest(short questID, WZQuestData wzQuest, string data = "")
        {
            var quest = new QuestData(Character, wzQuest, questID, data);
            Quests.Add(questID, quest);
            return StartQuest(quest);
        }

        public void UpdateMobKillCount(int mobID)
        {
            var quests = Quests.Where(qd => qd.Value.Check != null && qd.Value.State == QuestState.InProgress && qd.Value.Check.Mobs.ContainsKey(mobID)).Select(i => i.Value);
            foreach (var quest in quests)
            {
                quest.MobKilled(mobID);
            }
        }

        public string GetQuestData(short questID)
        {
            return Quests.ContainsKey(questID) ? Quests[questID].Data : "";
        }

        public void ItemCheck(int itemID)
        {
            var quests = Quests.Where(qd => qd.Value.Check != null && !qd.Value.ClearedItems && qd.Value.Check.Items.ContainsKey(itemID) && qd.Value.State == QuestState.InProgress).Select(i => i.Value);
            foreach (var qd in quests)
            {
                qd.CheckItemsCleared();
            }
        }

        public void AppendQuestData(short questID, string pData, bool pSendPacket = true)
        {
            SetQuestData(questID, GetQuestData(questID) + pData, pSendPacket);
        }


        public void SetQuestData(short questID, string pData, bool pSendPacket = true)
        {
            if (!HasQuest(questID)) return;

            Quests[questID].Data = pData;
            if (pSendPacket) QuestPacket.SendQuestUpdateData(Character, questID, pData);
        }

        public void SetQuestComplete(short questID, bool pSendPacket = true)
        {
            if (!HasQuest(questID)) return;

            var quest = Quests[questID];
            quest.State = QuestState.Completed;
            quest.EndTime = MasterThread.CurrentTime;
            if (pSendPacket) QuestPacket.SendCompleteQuest(Character, quest);
        }

        public void RemoveQuest(short qid, bool sendPacket = true)
        {
            if (!HasQuest(qid)) return;
            Quests.Remove(qid);
            if (sendPacket) QuestPacket.SendRemoveQuest(Character, qid);
        }

        #region Script helpers
        /// <summary>
        /// Get quest data
        /// </summary>
        /// <param name="questID"></param>
        /// <returns></returns>
        public string Get(short questID)
        {
            if (!HasQuest(questID)) return "";
            return Quests[questID].Data;
        }
        /// <summary>
        /// Set quest data
        /// </summary>
        /// <param name="questID"></param>
        /// <param name="data"></param>
        public void Set(short questID, string data)
        {
            if (Quests.TryGetValue(questID, out QuestData quest))
            {
                quest.State = QuestState.InProgress;
                SetQuestData(questID, data);
            }
            else
            {
                StartQuest(questID, data);
            }
        }
        public void SetComplete(short questID)
        {
            SetQuestComplete(questID);
        }
        public byte GetState(short questID)
        {
            if (!HasQuest(questID)) return (byte)QuestState.Available;
            return (byte)Quests[questID].State;
        }

        public void Remove(short questID)
        {
            Quests.Remove(questID);
        }
        #endregion
    }
}
