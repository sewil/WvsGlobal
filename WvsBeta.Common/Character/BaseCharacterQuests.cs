using System;
using System.Collections.Generic;
using WvsBeta.Common.Objects;

namespace WvsBeta.Common.Character
{
    public class BaseCharacterQuests
    {
        public Dictionary<int, QuestData> Quests { get; } = new Dictionary<int, QuestData>();
        public virtual void SaveQuests() { throw new NotImplementedException(); }
        public virtual bool LoadQuests() { throw new NotImplementedException(); }

        public bool HasQuest(int QuestID)
        {
            return Quests.ContainsKey(QuestID);
        }
    }
}
