using WvsBeta.Game;
using WvsBeta.Game.Scripting;

namespace WvsBeta.Scripts.Scripts
{
    class Market
    {
        public static readonly int QUEST_ID = 7600;
    }
    [Script("market00")]
    class Market00 : Market, IPortalScript
    {
        public void Run(IPortalHost host, GameCharacter target)
        {
            var quest = ScriptStandard.GetQuestData(target, QUEST_ID);
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
    [Script("market01")]
    class Market01 : Market, IPortalScript
    {
        public void Run(IPortalHost host, GameCharacter target)
        {
            var quest = ScriptStandard.GetQuestData(target, QUEST_ID);
            quest.Data = "0";
            target.ChangeMap(910000000, "st00");
        }
    }
    [Script("market02")]
    class Market02 : Market, IPortalScript
    {
        public void Run(IPortalHost host, GameCharacter target)
        {
            var quest = ScriptStandard.GetQuestData(target, QUEST_ID);
            quest.Data = "1";
            target.ChangeMap(910000000, "st00");
        }
    }
    [Script("market03")]
    class Market03 : Market, IPortalScript
    {
        public void Run(IPortalHost host, GameCharacter target)
        {
            var quest = ScriptStandard.GetQuestData(target, QUEST_ID);
            quest.Data = "2";
            target.ChangeMap(910000000, "st00");
        }
    }
    [Script("market04")]
    class Market04 : Market, IPortalScript
    {
        public void Run(IPortalHost host, GameCharacter target)
        {
            var quest = ScriptStandard.GetQuestData(target, QUEST_ID);
            quest.Data = "4";
            target.ChangeMap(910000000, "st00");
        }
    }
    [Script("market05")]
    class Market05 : Market, IPortalScript
    {
        public void Run(IPortalHost host, GameCharacter target)
        {
            var quest = ScriptStandard.GetQuestData(target, QUEST_ID);
            quest.Data = "3";
            target.ChangeMap(910000000, "st00");
        }
    }
    [Script("market06")]
    class Market06 : Market, IPortalScript
    {
        public void Run(IPortalHost host, GameCharacter target)
        {
            var quest = ScriptStandard.GetQuestData(target, QUEST_ID);
            quest.Data = "5";
            target.ChangeMap(910000000, "st00");
        }
    }
    [Script("market07")]
    class Market07 : Market, IPortalScript
    {
        public void Run(IPortalHost host, GameCharacter target)
        {
            var quest = ScriptStandard.GetQuestData(target, QUEST_ID);
            quest.Data = "6";
            target.ChangeMap(910000000, "st00");
        }
    }
    [Script("market08")]
    class Market08 : Market, IPortalScript
    {
        public void Run(IPortalHost host, GameCharacter target)
        {
            var quest = ScriptStandard.GetQuestData(target, QUEST_ID);
            quest.Data = "7";
            target.ChangeMap(910000000, "st00");
        }
    }
    [Script("market09")]
    class Market09 : Market, IPortalScript
    {
        public void Run(IPortalHost host, GameCharacter target)
        {
            var quest = ScriptStandard.GetQuestData(target, QUEST_ID);
            quest.Data = "8";
            target.ChangeMap(910000000, "st00");
        }
    }
}
