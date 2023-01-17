using WvsBeta.Game;
using WvsBeta.Game.Scripting;

namespace WvsBeta.Scripts.Scripts
{
    internal class Market09 : IPortalScript
    {
        public void Run(IPortalHost host, GameCharacter target)
        {
            var quest = ScriptStandard.GetQuestData(target, ScriptStandard.ScriptQuest.Portal);
            quest.Data = "8";
            target.ChangeMap(910000000, "st00");
        }
    }
}
