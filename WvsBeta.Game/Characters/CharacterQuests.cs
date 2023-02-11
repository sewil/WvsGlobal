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

            if (Quests.Count > 0)
            {
                query += "INSERT INTO character_quests (id, charid, questid, data, state, endtime) VALUES ";
                query += string.Join(", ", Quests.Select(kvp =>
                {
                    return "(" +
                           kvp.Value.ID + ", " +
                           charid + ", " +
                           kvp.Key + ", " +
                           "'" + MySqlHelper.EscapeString(kvp.Value.Data) + "', " +
                           (byte)kvp.Value.State + ", " +
                           kvp.Value.EndTime + " " +
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
                        Data = data.GetString("data"),
                        State = (QuestState)data.GetByte("state"),
                        EndTime = data.GetInt64("endtime")
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
            return true;
        }
        public override bool AddNewQuest(short questID, string data = "")
        {
            if (Quests.ContainsKey(questID))
                return false;

            Server.Instance.CharacterDatabase.RunQuery("INSERT INTO character_quests (id, charid, questid, data, state, endtime) VALUES (NULL, " + Character.ID.ToString() + ", " + questID + ", '" + MySqlHelper.EscapeString(data) + "', 1, 0)");
            int ID = Server.Instance.CharacterDatabase.GetLastInsertId();

            var quest = new QuestData
            {
                ID = ID,
                Data = data,
                QuestID = questID,
                State = QuestState.InProgress
            };
            Quests[questID] = quest;

            //if (DataProvider.Quests.TryGetValue(questID, out WZQuestData wzq) && wzq.Mobs.Count > 0)
            //{
            //    QuestMobs.AddRange(wzq.Mobs.Select(m => new QuestMobData {
            //        QuestDataId = ID,
            //        MobID = m.MobID,
            //        Needed = m.ReqKills
            //    }));
            //}

            QuestPacket.SendQuestUpdateData(Character, questID, data);
            return true;
        }

        public void UpdateMobKillCount(int questID, int mobID)
        {
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

        public void SetQuestComplete(short questID, bool pSendPacket = true)
        {
            if (!HasQuest(questID)) return;

            var quest = Quests[questID];
            quest.State = QuestState.Completed;
            quest.EndTime = MasterThread.UnixTime;
            if (pSendPacket) QuestPacket.SendCompleteQuest(Character, questID, quest.FileTime);
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
                SetQuestData(questID, data);
            }
            else
            {
                AddNewQuest(questID, data);
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
        #endregion
    }
}
