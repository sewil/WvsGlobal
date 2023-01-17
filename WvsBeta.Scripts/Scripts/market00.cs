using System.ComponentModel;
using WvsBeta.Game;
using WvsBeta.Game.Scripting;

namespace WvsBeta.Scripts.Scripts
{
    internal class Market00 : IPortalScript
    {
        public void Run(IPortalHost host, GameCharacter target)
        {
            var quest = ScriptStandard.GetQuestData(target, ScriptStandard.ScriptQuest.Portal);
            var val = quest.Data;

            if (val == "0") target.ChangeMap(100000100, "in04");
            else if (val == "1") target.ChangeMap(220000000, "st00");
            else if (val == "2") target.ChangeMap(211000100, "st00");
            else if (val == "3") target.ChangeMap(230000000, "st00");
            else if (val == "4") target.ChangeMap(102000000, "st00");
            else if (val == "5") target.ChangeMap(221000000, "st00");
            else if (val == "6") target.ChangeMap(200000000, "st00");
            else if (val == "7") target.ChangeMap(801000300, "st00");
            else if (val == "8") target.ChangeMap(240000000, "st00");
            else if (val == "9") target.ChangeMap(250000000, "st00");
            else if (val == "10") target.ChangeMap(251000000, "st00");
            else if (val == "11") target.ChangeMap(600000000, "st00");
            else target.ChangeMap(102000000, "st00");
            quest.Data = "-1";
        }
    }
}
