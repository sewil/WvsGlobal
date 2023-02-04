using WvsBeta.Common;
using WvsBeta.Game;
using WvsBeta.Game.Packets;
using WvsBeta.Game.Scripting;

namespace WvsBeta.Scripts.Scripts
{
    class Market
    {
        public static readonly short QUEST_ID = 7600;
    }
    [Script("market00")]
    class Market00 : Market, IPortalScript
    {
        public void Run(IPortalHost host, GameCharacter target)
        {
            var qr = target.Quests;
            var val = qr.Get(QUEST_ID);

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
        }
    }
    [Script("market01")]
    class Market01 : Market, IPortalScript
    {
        public void Run(IPortalHost host, GameCharacter target)
        {
            var qr = target.Quests;
            qr.Set(QUEST_ID, "0");
            target.SendSound(Constants.Sounds.Portal);
            target.ChangeMap(910000000, "st00");
        }
    }
    [Script("market02")]
    class Market02 : Market, IPortalScript
    {
        public void Run(IPortalHost host, GameCharacter target)
        {
            var qr = target.Quests;
            qr.Set(QUEST_ID, "1");
            target.SendSound(Constants.Sounds.Portal);
            target.ChangeMap(910000000, "st00");
        }
    }
    [Script("market03")]
    class Market03 : Market, IPortalScript
    {
        public void Run(IPortalHost host, GameCharacter target)
        {
            var qr = target.Quests;
            qr.Set(QUEST_ID, "2");
            target.SendSound(Constants.Sounds.Portal);
            target.ChangeMap(910000000, "st00");
        }
    }
    [Script("market04")]
    class Market04 : Market, IPortalScript
    {
        public void Run(IPortalHost host, GameCharacter target)
        {
            var qr = target.Quests;
            qr.Set(QUEST_ID, "3");
            target.SendSound(Constants.Sounds.Portal);
            target.ChangeMap(910000000, "st00");
        }
    }
    [Script("market05")]
    class Market05 : Market, IPortalScript
    {
        public void Run(IPortalHost host, GameCharacter target)
        {
            var qr = target.Quests;
            qr.Set(QUEST_ID, "4");
            target.SendSound(Constants.Sounds.Portal);
            target.ChangeMap(910000000, "st00");
        }
    }
    [Script("market06")]
    class Market06 : Market, IPortalScript
    {
        public void Run(IPortalHost host, GameCharacter target)
        {
            var qr = target.Quests;
            qr.Set(QUEST_ID, "5");
            target.SendSound(Constants.Sounds.Portal);
            target.ChangeMap(910000000, "st00");
        }
    }
    [Script("market07")]
    class Market07 : Market, IPortalScript
    {
        public void Run(IPortalHost host, GameCharacter target)
        {
            var qr = target.Quests;
            qr.Set(QUEST_ID, "6");
            target.SendSound(Constants.Sounds.Portal);
            target.ChangeMap(910000000, "st00");
        }
    }
    [Script("market08")]
    class Market08 : Market, IPortalScript
    {
        public void Run(IPortalHost host, GameCharacter target)
        {
            var qr = target.Quests;
            qr.Set(QUEST_ID, "7");
            target.SendSound(Constants.Sounds.Portal);
            target.ChangeMap(910000000, "st00");
        }
    }
    [Script("market09")]
    class Market09 : Market, IPortalScript
    {
        public void Run(IPortalHost host, GameCharacter target)
        {
            var qr = target.Quests;
            qr.Set(QUEST_ID, "8");
            target.SendSound(Constants.Sounds.Portal);
            target.ChangeMap(910000000, "st00");
        }
    }
}
