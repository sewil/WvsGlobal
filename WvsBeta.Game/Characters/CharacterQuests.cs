using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using WvsBeta.Common;
using WvsBeta.Common.Character;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Objects;

namespace WvsBeta.Game
{

    public class QuestMobData
    {
        public int QuestDataId { get; set; }
        public int MobID { get; set; }
        public int Killed { get; set; }
        public int Needed { get; set; }
    }

    public class CharacterQuests : BaseCharacterQuests
    {
        private GameCharacter Character { get; set; }
        public List<QuestMobData> QuestMobs { get; } = new List<QuestMobData>();

        public CharacterQuests(GameCharacter character)
        {
            Character = character;
        }

        public override void SaveQuests()
        {
            int charid = Character.ID;
            string query = "";

            query = "DELETE mobs.* FROM character_quest_mobs mobs LEFT JOIN character_quests quests ON mobs.id = quests.id WHERE quests.charid = " + charid + "; ";
            query += "DELETE FROM character_quests WHERE charid = " + charid + "; ";
            query += "DELETE FROM completed_quests WHERE charid = " + charid + "; ";

            if (Quests.Count > 0)
            {
                query += "INSERT INTO character_quests (id, charid, questid, data) VALUES ";
                query += string.Join(", ", Quests.Select(kvp =>
                {
                    return "(" +
                           kvp.Value.ID + ", " +
                           charid + ", " +
                           kvp.Key + ", " +
                           "'" + MySqlHelper.EscapeString(kvp.Value.Data) + "'" +
                           ")";
                }));
                query += ";";

                if (QuestMobs.Count > 0)
                {
                    query += "INSERT INTO character_quest_mobs (id, mobid, killed, needed) VALUES ";
                    query += string.Join(", ", QuestMobs.Select(kvp => "(" +
                                                                       kvp.QuestDataId + ", " +
                                                                       kvp.MobID + ", " +
                                                                       kvp.Killed + ", " +
                                                                       kvp.Needed + " " +
                                                                       ")"));
                    query += ";";
                }
            }

            if (CompletedQuests.Count > 0)
            {
                query += "INSERT INTO completed_quests (charid, questid, endtime) VALUES ";
                query += string.Join(", ", CompletedQuests.Select(kvp =>
                {
                    return "(" +
                           charid + ", " +
                           kvp.Key + ", " +
                           kvp.Value.EndTime + " " +
                           ")";
                }));
                query += ";";
            }

            Server.Instance.CharacterDatabase.RunQuery(query);
        }

        public override bool LoadQuests()
        {
            using (var data = (MySqlDataReader)Server.Instance.CharacterDatabase.RunQuery(
                    "SELECT * FROM character_quests WHERE charid = @charid",
                    "@charid", Character.ID
            ))
            {
                while (data.Read())
                {
                    var qd = new QuestData()
                    {
                        ID = data.GetInt32("id"),
                        QuestID = data.GetInt16("questid"),
                        Data = data.GetString("data")
                    };
                    Quests[qd.QuestID] = qd;
                }
            }

            if (Quests.Count > 0)
            {
                using (var mdr = (MySqlDataReader)Server.Instance.CharacterDatabase.RunQuery(
                    "SELECT * FROM character_quest_mobs WHERE id IN (" + string.Join(",", Quests.Keys.ToArray()) + ")"
                ))
                {
                    while (mdr.Read())
                    {
                        var questDataId = mdr.GetInt32("id");
                        var qmd = new QuestMobData()
                        {
                            QuestDataId = questDataId,
                            MobID = mdr.GetInt32("mobid"),
                            Killed = mdr.GetInt32("killed"),
                            Needed = mdr.GetInt32("needed")
                        };
                        QuestMobs.Add(qmd);
                    }
                }
            }

            using (var data = (MySqlDataReader)Server.Instance.CharacterDatabase.RunQuery(
                    "SELECT * FROM completed_quests WHERE charid = @charid",
                    "@charid", Character.ID
            ))
            {
                while (data.Read())
                {
                    var qd = new CompletedQuest(data.GetInt16("questid"), data.GetInt64("endtime"));
                    CompletedQuests[qd.QuestID] = qd;
                }
            }
            return true;
        }
        public override bool AddNewQuest(short questID, string data = "")
        {
            if (Quests.ContainsKey(questID))
                return false;

            Server.Instance.CharacterDatabase.RunQuery("INSERT INTO character_quests (id, charid, questid, data) VALUES (NULL, " + Character.ID.ToString() + ", " + questID + ", '" + MySqlHelper.EscapeString(data) + "')");
            int ID = Server.Instance.CharacterDatabase.GetLastInsertId();

            var quest = new QuestData
            {
                ID = ID,
                Data = data,
                QuestID = questID
            };
            Quests[questID] = quest;
            QuestPacket.SendQuestUpdateData(Character, questID, data);
            return true;
        }
        public bool AddCompletedQuest(short questID)
        {
            if (CompletedQuests.ContainsKey(questID))
                return false;

            long unix = MasterThread.UnixTime;

            Server.Instance.CharacterDatabase.RunQuery("DELETE FROM character_quests WHERE charid = " + Character.ID + " AND questid = " + questID);
            Server.Instance.CharacterDatabase.RunQuery("INSERT INTO completed_quests (charid, questid, endtime) VALUES (" + Character.ID.ToString() + ", " + questID + ", " + unix + ")");

            var quest = new CompletedQuest(questID, unix);
            Quests.Remove(questID);
            CompletedQuests[questID] = quest;
            QuestPacket.SendCompleteQuest(Character, questID, quest.FileTime);
            return true;
        }
        public void AddOrSetQuestMob(short questID, int mobID, int needed)
        {
            if (!Quests.TryGetValue(questID, out var qd)) return;

            var questDataId = qd.ID;

            var mobData = QuestMobs.FirstOrDefault(x => x.QuestDataId == questDataId && x.MobID == mobID);

            if (mobData == null)
            {
                QuestMobs.Add(new QuestMobData
                {
                    Killed = 0,
                    Needed = needed,
                    MobID = mobID,
                    QuestDataId = questDataId
                });
            }
            else
            {
                mobData.Needed = needed;
            }
        }

        public bool HasQuestMob(short questID, int MobID)
        {
            if (!Quests.TryGetValue(questID, out var qd)) return false;

            var questDataId = qd.ID;
            return QuestMobs.Exists(x => x.QuestDataId == questDataId && x.MobID == MobID);
        }

        public string GetQuestData(short questID)
        {
            return Quests.ContainsKey(questID) ? Quests[questID].Data : "";
        }

        public bool ItemCheck(int ItemID)
        {
            return false;
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
            if (HasQuest(questID))
            {
                Quests[questID].Data = data;
            }
            else
            {
                AddNewQuest(questID, data);
            }
        }
        public void SetComplete(short questID)
        {
            AddCompletedQuest(questID);
        }
        public QuestState GetState(short questID)
        {
            if (HasQuest(questID)) return QuestState.Started;
            else if (HasCompletedQuest(questID)) return QuestState.Completed;
            else return QuestState.NotStarted;
        }
        #endregion
    }
}
