using WvsBeta.Common.Enums;
using WvsBeta.Common.Extensions;
using WvsBeta.Game;
using WvsBeta.Game.Scripting;

namespace WvsBeta.Scripts.Scripts
{
    // send users from florina beach to towns
    [Script("florina1")]
    class florina1 : INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            var qr = target.QuestRecord;
            var val1 = qr.Get(9005);
            int mCode, nRet;

            if (val1 == "0") { mCode = 104000000; }
            else if (val1 == "1") { mCode = 200000000; }
            else { mCode = 220000000; }

            var v1 = self.AskMenu("Do you wish to leave #b#m110000000##k? If you like, I can take you back to #b#m" + mCode + "##k.\r\n\r\n#b#L0# #bI'd like to go back now.");
            if (v1 == 0)
            {
                nRet = self.AskYesNo("Are you sure you want to go back to #b#m?" + mCode + "##k? Alright, we'll have to hurry. Do you want to return to #m" + mCode + "# now?");
                if (nRet == 0) self.Say("You must have some business to attend to here. It's not a bad idea to take a little break from #m" + mCode + "#. Look at me, I'm going to end up living here. Hahaha... Well, anyway, talk to me when you want to go back.");
                else
                {
                    if (val1 == "0") target.ChangeMap(104000000, "set00");
                    else if (val1 == "1") target.ChangeMap(200000000, "");
                    else target.ChangeMap(220000000, "");
                }
            }
        }
    }
    // Send users to Florina Beach
    [Script("florina2")]
    class florina2 : INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            var field = self.Field;
            var qr = target.QuestRecord;
            var inven = target.Inventory;
            int fee;
            string qrVal;

            // Lith Harbor
            if (field.ID == MapIds.LithHarbor)
            {
                fee = 1500;
                qrVal = "0";
            }
            // Orbis
            else if (field.ID == MapIds.Orbis)
            {
                fee = 1500;
                qrVal = "1";
            }
            // Ludibrium
            else
            {
                fee = 1200;
                qrVal = "2";
            }

            int option = self.AskMenu("Ever heard of the beach with a wonderful view of the ocean, called #b#m110000000##k? I can take you there right now for #b1,500 mesos#k, or if you have #b#t4031134##k with you, I'll take you there for free.",
                "I'll pay #b1,500 mesos#k.",
                "I have #b#t4031134##k.",
                "What is #b#t4031134##k?"
            );

            string goDeny = "I suppose you have some business to attend to here still. You must be tired from all the traveling and hunting. Go get some rest, and if you change your mind, come back and talk to me.";

            if (option == 0)
            {
                var nRet = self.AskYesNo($"So, you want to pay #b{fee.Culture()}#k and go to #m{MapIds.FlorinaBeach}#? Alright, but be aware that you may encounter some monsters there, so make sure to be on your guard. Alright, would you like to go to #m110000000# right now?");
                if (nRet == 0) self.Say(goDeny);
                else
                {
                    var ret = target.IncMoney(-1500, 1);
                    if (ret == 0) self.Say("I think you're out of mesos. There are many ways to earn money, you know... Selling your armor... Defeating monsters... Doing quests... You know what I'm talking about.");
                    else
                    {
                        qr.Set(9005, qrVal);
                        target.ChangeMap(110000000, "st00");
                    }
                }
            }
            else if (option == 1)
            {
                var nRet = self.AskYesNo($"So, you have #b#t4031134##k? It gives you unlimited access to #m{MapIds.FlorinaBeach}#, right?! Be aware though that you may encounter some monsters there, so make sure to be on your guard. Alright, would you like to go to #m{MapIds.FlorinaBeach}# right now?");
                if (nRet == 0) self.Say(goDeny);
                else
                {
                    if (inven.ItemCount(4031134) >= 1)
                    {
                        qr.Set(9005, qrVal);
                        target.ChangeMap(110000000, "st00");
                    }
                    else self.Say("Um... Are you sure you have #b#t4031134##k on you? Make sure, because I can't find it for you.");
                }
            }
            else if (option == 2)
            {
                self.Say("Guessing you're curious about #b#t4031134##k. Haha, that's quite understandable. #t4031134# is an item that, as long as you have it, can get you to #m110000000# for free. It's such a rare item that even we had to buy them, but unfortunately I lost mine a few weeks ago during my precious summer vacation.");
                self.Say("I came back without it and I feel terrible not having it anymore. I hope someone found it and put it in a safe place. Well, that's my story, and who knows? You might find it and put it to good use. If you have any questions, feel free to ask.");
            }
        }
    }
}
