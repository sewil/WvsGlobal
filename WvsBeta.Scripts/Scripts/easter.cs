using System;
using System.IO.Ports;
using WvsBeta.Common;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Objects;
using WvsBeta.Game;
using WvsBeta.Game.Packets;
using WvsBeta.Game.Scripting;

namespace WvsBeta.Scripts
{
    [Script("easter")]
    class Easter : INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            if(target.Level < 8)
            {
                self.Say("Sup. I'm Roy, but my boys call me 'The Mad Bunny'. I hate Easter and I'm going to bring it down this year. I don't think you can help us out right now, cuz you seem weak. Go get yourself stronger, and maybe we'll talk business. ");
                return;
            }
            var inventory = target.Inventory;
            var cTime = MasterThread.CurrentDate;
            var esTime = cTime.CompareTo(new DateTime(cTime.Year, 3, 11, 0, 0, 0));
            var eeTime = cTime.CompareTo(new DateTime(cTime.Year, 4, 2, 23, 59, 59));
            if (esTime < 0)
            {
                self.Say("Quit buggin me. When Easter comes along, I'll have plenty for you to do...");
                return;
            }
            else if (eeTime > 0)
            {
                self.Say("Sup. Easter is over, but I still don't like it one bit, and now the people are aware of my plans. That's alright, though; my gang will do our best to bring it down next year, too. I'll be counting on you to help us take away more Easter Eggs from others. Peace..");
                return;
            }

            var qr = target.Quests;
            var val = qr.Get(8876);
            if (val == "end") // if a user already finished the quest
            {
                int nRet = self.AskYesNo("Hey, nice to see you again! How have you been? So, have you found more #b#t4031284##k for me?");
                if (nRet == 0)
                {
                    self.Say("Oh really? That's alright. I'll be here for a bit, so if you find more #b#t4031284##k, you know where to find me.");
                }
                else
                {
                    self.Say("Sweet. That sounds like good news. I'll be here waiting.");
                    qr.Set(8876, "ing");
                }
            }
            else if (val == "ing")
            {
                if (inventory.GetItemAmount(Inventory.Etc, 4031284) >= 1)
                {
                    int nRet = self.AskYesNo("Wow! You found the Golden Egg? That's the rarest of the Easter Eggs! What are you going to do with it? If you give it to me, I'll give you some EXP! Now, I'm not used to giving out EXP, so I can't guarantee how much you're going to get. But life's a gamble, right? Otherwise, I've got a deal with all the store owners, and they'll give you some good money for it. (These EXP points may level you up as much as 'almost' two levels, depending on your level.) ");
                    if (nRet != 0)
                    {
                        self.Say("Good deal! Hope you get some good EXP!");
                        inventory.TakeItem(4031284, 1);
                        var exp = 0;
                        var rn1 = Rand32.NextBetween(1, 10000);
                        if (1 <= rn1 && rn1 <= 5000) exp = 100;
                        if (5000 < rn1 && rn1 <= 8500) exp = 1000;
                        if (8500 < rn1 && rn1 <= 9999) exp = 10000;
                        if (9999 < rn1 && rn1 <= 10000) exp = 100000;
                        target.AddEXP(exp, MessageAppearType.SideWhite);
                        self.Say("I'm giving you " + exp + " EXP. Peace out!");
                        qr.Set(8876, "end");
                    }
                }
                else
                {
                    self.Say("You don't have the Golden Egg!! Man... if you ever find one, then please see me, alright?");
                }
            }
            else
            {
                int nRet = self.AskYesNo("I see, so does that mean you can help us out by giving us the Golden Egg?");
                if (nRet != 0)
                {
                    //user accepts the quest
                    self.Say("Okay... Good luck!!!");
                    qr.Set(8876, "ing");
                }
                else //user doesn't accept the quest
                {
                    self.Say("You just wasted my breath. Get lost!");
                }
            }
        }
    }
}
