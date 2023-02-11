using System;
using System.Collections.Generic;
using System.Linq;
using WvsBeta.Common.DataProviders;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Objects;

namespace WvsBeta.Common.Character
{
    public class BaseCharacterQuests
    {
        public Dictionary<short, QuestData> Quests { get; } = new Dictionary<short, QuestData>();

        public virtual void SaveQuests() { throw new NotImplementedException(); }
        public virtual bool LoadQuests() { throw new NotImplementedException(); }
        public virtual bool AddNewQuest(short questID, string data = "") { throw new NotImplementedException(); }
        public bool HasQuest(short questID)
        {
            return Quests.ContainsKey(questID);
        }
        public Dictionary<short, QuestData> GetQuests(bool wzFilter = true)
        {
            if (wzFilter) return Quests.Where((i) => BaseDataProvider.Quests.ContainsKey(i.Key)).ToDictionary(x => x.Key, x => x.Value);
            else return Quests;
        }
    }
}
