using WvsBeta.Common.Objects;
using WvsBeta.Game;

namespace WvsBeta.Scripts.Scripts
{
    public class ScriptStandard
    {
        public static QuestData GetQuestData(GameCharacter target, int questID, string defaultData = "")
        {
            if (!target.Quests.HasQuest(questID))
                target.Quests.AddNewQuest(questID, defaultData);
            var quest = target.Quests.Quests[questID];
            return quest;
        }
    }
}
