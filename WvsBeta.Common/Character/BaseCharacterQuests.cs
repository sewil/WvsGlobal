using System;
using System.Collections.Generic;
using System.Linq;
using WvsBeta.Common.DataProviders;
using WvsBeta.Common.Objects;

namespace WvsBeta.Common.Character
{
    public class BaseCharacterQuests
    {
        public Dictionary<int, QuestData> Quests { get; } = new Dictionary<int, QuestData>();
        public virtual void SaveQuests() { throw new NotImplementedException(); }
        public virtual bool LoadQuests() { throw new NotImplementedException(); }
        public virtual bool AddNewQuest(int QuestID, string Data = "") { throw new NotImplementedException(); }
        public bool HasQuest(int QuestID)
        {
            return Quests.ContainsKey(QuestID);
        }
        // Used by scripts because they can't use out
        public QuestData GetQuest(int QuestID)
        {
            if (Quests.ContainsKey(QuestID)) return Quests[QuestID];
            else return null;
        }
        public Dictionary<int, QuestData> GetWZExistingQuests()
        {
            return Quests.Where((i) => BaseDataProvider.Quests.ContainsKey(i.Key)).ToDictionary(x => x.Key, x => x.Value);
        }
    }
}
