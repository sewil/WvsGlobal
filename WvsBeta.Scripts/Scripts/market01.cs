using WvsBeta.Game;
using WvsBeta.Game.Scripting;

namespace WvsBeta.Scripts.Scripts
{
    internal class Market01 : IPortalScript
    {
        public void Run(IPortalHost host, GameCharacter target)
        {
            var quest = ScriptStandard.GetQuestData(target, ScriptStandard.ScriptQuest.Portal);
            quest.Data = "0";
            target.ChangeMap(910000000, "st00");
        }
    }
}
