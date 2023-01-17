using WvsBeta.Common.Objects;
using WvsBeta.Game;

namespace WvsBeta.Scripts.Scripts
{
    public class ScriptStandard
    {
        public enum ScriptQuest : short
        {
            Portal = 7600,
            Easter = 8876
        }
        public static QuestData GetQuestData(GameCharacter target, ScriptQuest scriptQuest)
        {
            int questID = (int)scriptQuest;
            if (!target.Quests.HasQuest(questID))
                target.Quests.AddNewQuest(questID);
            var quest = target.Quests.Quests[questID];
            return quest;
        }
        public static void GoTown()
        {

        }
    }
}
