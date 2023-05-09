using System.Linq;
using WvsBeta.Common;
using WvsBeta.Game;
using WvsBeta.Game.Scripting;

namespace WvsBeta.Scripts.Scripts
{
    public static class ossyria0
    {
        static INpcHost self;
        static GameCharacter target;
        public static void replaceItem(int itemCode)
        {
            int nNewItemID, nNewItemNum, rnum;
            var inventory = target.Inventory;

            var nRet1 = self.AskYesNo("Let's see, you want to exchange #b100 #t" + itemCode + "#s#k for something of mine, right? Before the exchange, make sure you have a free slot in your use and etc. inventory. So, do you really want to make the exchange?");
            if (nRet1 == 0) self.Say("Hmmm... it wouldn't be too bad for you. If you come to me at the right time, I can get you some good items. Anyway, when you're ready to make the trade, come see me.");
            else
            {
                // hard horn
                if (itemCode == 4000073)
                {
                    rnum = Rand32.NextBetween(1, 6);
                    if (rnum == 1)
                    {
                        nNewItemID = 2000001;
                        nNewItemNum = 20;
                    }
                    else if (rnum == 2)
                    {
                        nNewItemID = 2000003;
                        nNewItemNum = 15;
                    }
                    else if (rnum == 3)
                    {
                        nNewItemID = 2020001;
                        nNewItemNum = 15;
                    }
                    else if (rnum == 4)
                    {
                        nNewItemID = 2010004;
                        nNewItemNum = 10;
                    }
                    else if (rnum == 5)
                    {
                        nNewItemID = 4003001;
                        nNewItemNum = 15;
                    }
                    else
                    {
                        nNewItemID = 2030000;
                        nNewItemNum = 15;
                    }
                }
                // Star Pixie's Piece of Star or Flying Eye's Wings
                else if (itemCode == 4000059 || itemCode == 4000076)
                {
                    rnum = Rand32.NextBetween(1, 100);
                    if (rnum <= 25)
                    {
                        nNewItemID = 2000001;
                        nNewItemNum = 30;
                    }
                    else if (rnum <= 50)
                    {
                        nNewItemID = 2000003;
                        nNewItemNum = 20;
                    }
                    else if (rnum <= 75)
                    {
                        nNewItemID = 2010001;
                        nNewItemNum = 40;
                    }
                    else if (rnum <= 99)
                    {
                        nNewItemID = 4003001;
                        nNewItemNum = 20;
                    }
                    else
                    {
                        nNewItemID = 2040002;
                        nNewItemNum = 1;
                    }
                }
                // Nependeath's Seed
                else if (itemCode == 4000058)
                {
                    rnum = Rand32.NextBetween(1, 100);
                    if (rnum <= 25)
                    {
                        nNewItemID = 2000002;
                        nNewItemNum = 15;
                    }
                    else if (rnum <= 50)
                    {
                        nNewItemID = 2000003;
                        nNewItemNum = 25;
                    }
                    else if (rnum <= 75)
                    {
                        nNewItemID = 2010004;
                        nNewItemNum = 15;
                    }
                    else if (rnum <= 99)
                    {
                        nNewItemID = 4003001;
                        nNewItemNum = 30;
                    }
                    else
                    {
                        nNewItemID = 2040302;
                        nNewItemNum = 1;
                    }
                }
                // Jr. Bulldog's Tooth
                else if (itemCode == 4000078)
                {
                    rnum = Rand32.NextBetween(1, 100);
                    if (rnum <= 20)
                    {
                        nNewItemID = 2000002;
                        nNewItemNum = 15;
                    }
                    else if (rnum <= 40)
                    {
                        nNewItemID = 2000003;
                        nNewItemNum = 25;
                    }
                    else if (rnum <= 60)
                    {
                        nNewItemID = 2010004;
                        nNewItemNum = 15;
                    }
                    else if (rnum <= 80)
                    {
                        nNewItemID = 4003001;
                        nNewItemNum = 30;
                    }
                    else if (rnum <= 99)
                    {
                        nNewItemID = 2050004;
                        nNewItemNum = 15;
                    }
                    else
                    {
                        nNewItemID = 2040302;
                        nNewItemNum = 1;
                    }
                }
                // Lunar Pixie's Piece of Moon
                else if (itemCode == 4000060)
                {
                    rnum = Rand32.NextBetween(1, 100);
                    if (rnum <= 25)
                    {
                        nNewItemID = 2000002;
                        nNewItemNum = 25;
                    }
                    else if (rnum <= 50)
                    {
                        nNewItemID = 2000006;
                        nNewItemNum = 10;
                    }
                    else if (rnum <= 75)
                    {
                        nNewItemID = 2022000;
                        nNewItemNum = 5;
                    }
                    else if (rnum <= 99)
                    {
                        nNewItemID = 4000030;
                        nNewItemNum = 15;
                    }
                    else
                    {
                        nNewItemID = 2040902;
                        nNewItemNum = 1;
                    }
                }
                // Jr. Yeti's Skin or Dark Nependeath's Seed
                else if (itemCode == 4000048 || itemCode == 4000062)
                {
                    rnum = Rand32.NextBetween(1, 100);
                    if (rnum <= 25)
                    {
                        nNewItemID = 2000002;
                        nNewItemNum = 30;
                    }
                    else if (rnum <= 50)
                    {
                        nNewItemID = 2000006;
                        nNewItemNum = 15;
                    }
                    else if (rnum <= 75)
                    {
                        nNewItemID = 2020000;
                        nNewItemNum = 20;
                    }
                    else if (rnum <= 99)
                    {
                        nNewItemID = 4003000;
                        nNewItemNum = 5;
                    }
                    else
                    {
                        nNewItemID = 2040402;
                        nNewItemNum = 1;
                    }
                }
                // Fireball's Flame
                else if (itemCode == 4000081)
                {
                    rnum = Rand32.NextBetween(1, 100);
                    if (rnum <= 15)
                    {
                        nNewItemID = 2000006;
                        nNewItemNum = 25;
                    }
                    else if (rnum <= 30)
                    {
                        nNewItemID = 2020006;
                        nNewItemNum = 25;
                    }
                    else if (rnum <= 45)
                    {
                        nNewItemID = 4010004;
                        nNewItemNum = 8;
                    }
                    else if (rnum <= 60)
                    {
                        nNewItemID = 4010005;
                        nNewItemNum = 8;
                    }
                    else if (rnum <= 75)
                    {
                        nNewItemID = 4010006;
                        nNewItemNum = 3;
                    }
                    else if (rnum <= 90)
                    {
                        nNewItemID = 4020008;
                        nNewItemNum = 3;
                    }
                    else if (rnum <= 95)
                    {
                        nNewItemID = 4020007;
                        nNewItemNum = 2;
                    }
                    else
                    {
                        nNewItemID = 2040705;
                        nNewItemNum = 1;
                    }
                }
                // Luster Pixie's Piece of Sun
                else if (itemCode == 4000061)
                {
                    rnum = Rand32.NextBetween(1, 100);
                    if (rnum <= 25)
                    {
                        nNewItemID = 2000002;
                        nNewItemNum = 30;
                    }
                    else if (rnum <= 50)
                    {
                        nNewItemID = 2000006;
                        nNewItemNum = 15;
                    }
                    else if (rnum <= 75)
                    {
                        nNewItemID = 2020000;
                        nNewItemNum = 20;
                    }
                    else if (rnum <= 99)
                    {
                        nNewItemID = 4003000;
                        nNewItemNum = 5;
                    }
                    else
                    {
                        nNewItemID = 2041016;
                        nNewItemNum = 1;
                    }
                }
                // Cellion's Tail or Lioner's Tail or Grupin's Tail
                else if (itemCode == 4000070 || itemCode == 4000071 || itemCode == 4000072)
                {
                    rnum = Rand32.NextBetween(1, 100);
                    if (rnum <= 25)
                    {
                        nNewItemID = 2000002;
                        nNewItemNum = 30;
                    }
                    else if (rnum <= 50)
                    {
                        nNewItemID = 2000006;
                        nNewItemNum = 15;
                    }
                    else if (rnum <= 75)
                    {
                        nNewItemID = 2020000;
                        nNewItemNum = 20;
                    }
                    else if (rnum <= 99)
                    {
                        nNewItemID = 4003000;
                        nNewItemNum = 5;
                    }
                    else
                    {
                        nNewItemID = 2041005;
                        nNewItemNum = 1;
                    }
                }
                // Hector's Tail 
                else if (itemCode == 4000051)
                {
                    rnum = Rand32.NextBetween(1, 100);
                    if (rnum <= 35)
                    {
                        nNewItemID = 2002004;
                        nNewItemNum = 15;
                    }
                    else if (rnum <= 70)
                    {
                        nNewItemID = 2002005;
                        nNewItemNum = 15;
                    }
                    else if (rnum <= 97)
                    {
                        nNewItemID = 2002003;
                        nNewItemNum = 10;
                    }
                    else if (rnum <= 99)
                    {
                        nNewItemID = 4001005;
                        nNewItemNum = 1;
                    }
                    else
                    {
                        nNewItemID = 2040502;
                        nNewItemNum = 1;
                    }
                }
                // Dark Jr. Yeti's Skin
                else if (itemCode == 4000055)
                {
                    rnum = Rand32.NextBetween(1, 100);
                    if (rnum <= 35)
                    {
                        nNewItemID = 2022001;
                        nNewItemNum = 30;
                    }
                    else if (rnum <= 70)
                    {
                        nNewItemID = 2020006;
                        nNewItemNum = 15;
                    }
                    else if (rnum <= 97)
                    {
                        nNewItemID = 2020005;
                        nNewItemNum = 30;
                    }
                    else if (rnum <= 99)
                    {
                        nNewItemID = 4003003;
                        nNewItemNum = 1;
                    }
                    else
                    {
                        nNewItemID = 2040505;
                        nNewItemNum = 1;
                    }
                }
                // Zombie's Lost Tooth
                else if (itemCode == 4000069)
                {
                    rnum = Rand32.NextBetween(1, 100);
                    if (rnum <= 25)
                    {
                        nNewItemID = 2050004;
                        nNewItemNum = 20;
                    }
                    else if (rnum <= 50)
                    {
                        nNewItemID = 2000006;
                        nNewItemNum = 20;
                    }
                    else if (rnum <= 75)
                    {
                        nNewItemID = 2020006;
                        nNewItemNum = 15;
                    }
                    else if (rnum <= 98)
                    {
                        nNewItemID = 2020005;
                        nNewItemNum = 30;
                    }
                    else if (rnum <= 99)
                    {
                        nNewItemID = 4003003;
                        nNewItemNum = 1;
                    }
                    else
                    {
                        nNewItemID = 2041002;
                        nNewItemNum = 1;
                    }
                }
                // White Pang's Tail
                else if (itemCode == 4000052)
                {
                    rnum = Rand32.NextBetween(1, 100);
                    if (rnum <= 40)
                    {
                        nNewItemID = 2000006;
                        nNewItemNum = 20;
                    }
                    else if (rnum <= 60)
                    {
                        nNewItemID = 4010003;
                        nNewItemNum = 7;
                    }
                    else if (rnum <= 80)
                    {
                        nNewItemID = 4010004;
                        nNewItemNum = 7;
                    }
                    else if (rnum <= 97)
                    {
                        nNewItemID = 4010005;
                        nNewItemNum = 7;
                    }
                    else if (rnum <= 99)
                    {
                        nNewItemID = 4003002;
                        nNewItemNum = 1;
                    }
                    else
                    {
                        nNewItemID = 2040602;
                        nNewItemNum = 1;
                    }
                }
                // Pepe's Beek
                else if (itemCode == 4000050)
                {
                    rnum = Rand32.NextBetween(1, 100);
                    if (rnum <= 30)
                    {
                        nNewItemID = 2000006;
                        nNewItemNum = 20;
                    }
                    else if (rnum <= 45)
                    {
                        nNewItemID = 4010000;
                        nNewItemNum = 7;
                    }
                    else if (rnum <= 60)
                    {
                        nNewItemID = 4010001;
                        nNewItemNum = 7;
                    }
                    else if (rnum <= 79)
                    {
                        nNewItemID = 4010002;
                        nNewItemNum = 7;
                    }
                    else if (rnum <= 99)
                    {
                        nNewItemID = 4010006;
                        nNewItemNum = 2;
                    }
                    else
                    {
                        nNewItemID = 2040702;
                        nNewItemNum = 1;
                    }
                }
                // Dark Pepe's Beek
                else if (itemCode == 4000057)
                {
                    rnum = Rand32.NextBetween(1, 100);
                    if (rnum <= 30)
                    {
                        nNewItemID = 2000006;
                        nNewItemNum = 20;
                    }
                    else if (rnum <= 50)
                    {
                        nNewItemID = 4010004;
                        nNewItemNum = 7;
                    }
                    else if (rnum <= 62)
                    {
                        nNewItemID = 4010005;
                        nNewItemNum = 7;
                    }
                    else if (rnum <= 74)
                    {
                        nNewItemID = 4010006;
                        nNewItemNum = 3;
                    }
                    else if (rnum <= 86)
                    {
                        nNewItemID = 4020008;
                        nNewItemNum = 2;
                    }
                    else if (rnum <= 99)
                    {
                        nNewItemID = 4020007;
                        nNewItemNum = 2;
                    }
                    else
                    {
                        nNewItemID = 2040705;
                        nNewItemNum = 1;
                    }
                }
                // Yeti's Horn
                else if (itemCode == 4000049)
                {
                    rnum = Rand32.NextBetween(1, 100);
                    if (rnum <= 30)
                    {
                        nNewItemID = 2000006;
                        nNewItemNum = 25;
                    }
                    else if (rnum <= 50)
                    {
                        nNewItemID = 4020000;
                        nNewItemNum = 7;
                    }
                    else if (rnum <= 65)
                    {
                        nNewItemID = 4020001;
                        nNewItemNum = 7;
                    }
                    else if (rnum <= 85)
                    {
                        nNewItemID = 4020002;
                        nNewItemNum = 3;
                    }
                    else if (rnum <= 99)
                    {
                        nNewItemID = 4020007;
                        nNewItemNum = 2;
                    }
                    else
                    {
                        nNewItemID = 2040708;
                        nNewItemNum = 1;
                    }
                }
                // Dark Yeti's Horn
                else if (itemCode == 4000056)
                {
                    rnum = Rand32.NextBetween(1, 100);
                    if (rnum <= 30)
                    {
                        nNewItemID = 2000006;
                        nNewItemNum = 25;
                    }
                    else if (rnum <= 50)
                    {
                        nNewItemID = 4020003;
                        nNewItemNum = 7;
                    }
                    else if (rnum <= 65)
                    {
                        nNewItemID = 4020004;
                        nNewItemNum = 7;
                    }
                    else if (rnum <= 85)
                    {
                        nNewItemID = 4020005;
                        nNewItemNum = 7;
                    }
                    else if (rnum <= 99)
                    {
                        nNewItemID = 4020008;
                        nNewItemNum = 2;
                    }
                    else
                    {
                        nNewItemID = 2040802;
                        nNewItemNum = 1;
                    }
                }
                // Bulldog's Tooth
                else if (itemCode == 4000079)
                {
                    rnum = Rand32.NextBetween(1, 100);
                    if (rnum <= 15)
                    {
                        nNewItemID = 2000006;
                        nNewItemNum = 25;
                    }
                    else if (rnum <= 30)
                    {
                        nNewItemID = 2022001;
                        nNewItemNum = 35;
                    }
                    else if (rnum <= 45)
                    {
                        nNewItemID = 4020000;
                        nNewItemNum = 8;
                    }
                    else if (rnum <= 60)
                    {
                        nNewItemID = 4020001;
                        nNewItemNum = 8;
                    }
                    else if (rnum <= 75)
                    {
                        nNewItemID = 4020002;
                        nNewItemNum = 8;
                    }
                    else if (rnum <= 90)
                    {
                        nNewItemID = 4020007;
                        nNewItemNum = 2;
                    }
                    else if (rnum <= 99)
                    {
                        nNewItemID = 2050004;
                        nNewItemNum = 30;
                    }
                    else
                    {
                        nNewItemID = 2041023;
                        nNewItemNum = 1;
                    }
                }
                // Werewolf's Toenail
                else if (itemCode == 4000053)
                {
                    rnum = Rand32.NextBetween(1, 100);
                    if (rnum <= 37)
                    {
                        nNewItemID = 2000006;
                        nNewItemNum = 30;
                    }
                    else if (rnum <= 57)
                    {
                        nNewItemID = 4020006;
                        nNewItemNum = 7;
                    }
                    else if (rnum <= 77)
                    {
                        nNewItemID = 4020007;
                        nNewItemNum = 2;
                    }
                    else if (rnum <= 97)
                    {
                        nNewItemID = 4020008;
                        nNewItemNum = 2;
                    }
                    else if (rnum <= 99)
                    {
                        nNewItemID = 2070010;
                        nNewItemNum = 1;
                    }
                    else
                    {
                        nNewItemID = 2040805;
                        nNewItemNum = 1;
                    }
                }
                // Lycanthrope's Toenail
                else if (itemCode == 4000054)
                {
                    rnum = Rand32.NextBetween(1, 100);
                    if (rnum <= 37)
                    {
                        nNewItemID = 2000006;
                        nNewItemNum = 30;
                    }
                    else if (rnum <= 57)
                    {
                        nNewItemID = 4020006;
                        nNewItemNum = 7;
                    }
                    else if (rnum <= 77)
                    {
                        nNewItemID = 4020007;
                        nNewItemNum = 2;
                    }
                    else if (rnum <= 97)
                    {
                        nNewItemID = 4020008;
                        nNewItemNum = 2;
                    }
                    else if (rnum <= 99)
                    {
                        nNewItemID = 2070010;
                        nNewItemNum = 1;
                    }
                    else
                    {
                        nNewItemID = 2041020;
                        nNewItemNum = 1;
                    }
                }
                // Firedawg's necklace
                else if (itemCode == 4000080)
                {
                    rnum = Rand32.NextBetween(1, 100);
                    if (rnum <= 37)
                    {
                        nNewItemID = 2000006;
                        nNewItemNum = 35;
                    }
                    else if (rnum <= 57)
                    {
                        nNewItemID = 4020006;
                        nNewItemNum = 9;
                    }
                    else if (rnum <= 77)
                    {
                        nNewItemID = 4020007;
                        nNewItemNum = 4;
                    }
                    else if (rnum <= 97)
                    {
                        nNewItemID = 4020008;
                        nNewItemNum = 4;
                    }
                    else if (rnum <= 99)
                    {
                        nNewItemID = 2070011;
                        nNewItemNum = 1;
                    }
                    else
                    {
                        nNewItemID = 2041008;
                        nNewItemNum = 1;
                    }
                }
                else
                {
                    nNewItemID = 0;
                    nNewItemNum = 0;
                }

                if ((inventory.SlotCount(2) > inventory.HoldCount(2)) && (inventory.SlotCount(4) > inventory.HoldCount(4)))
                {
                    var ret = inventory.Exchange(0, itemCode, -100, nNewItemID, nNewItemNum);
                    if (ret == 0) self.Say("Hmmm... Are you sure you collected #b100 #t" + itemCode + "#s#k? If so, please check if your inventory is full.");
                    else
                    {
                        target.IncEXP(500, 0);
                        self.Say("For your #b100 #t" + itemCode + "#s#k, I'll give you #b" + nNewItemNum + " #t" + nNewItemID + "#(s)#k. What do you think? Did you like the item I gave in return? I'm supposed to be here for a while, so if you gather more items, I'm always willing to trade...");
                    }
                }
                else self.Say("Looks like your etc. inventory is full. You need free space to trade with me! Please make some room and then talk to me again...");
            }
        }
        // The NPC that warps the character to the different boarding platform
        [Script("getAboard")]
        class getAboard : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                int[] stations = new int[]
                {
                    200000110,
                    200000120
                };
                int menuIdx = self.AskMenu("Where would you like to go?", stations.Select(i => $"#m{i + 1}#").ToArray());
                int map = stations[menuIdx];
                target.ChangeMap(map, "west00");
            }
        }
        [Script("goDungeon")]
        class goDungeon : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                self.Say("Hey, looks like you want to go a lot further from here. There, however, you'll find monsters on all sides, aggressive, dangerous, and even if you think you're ready, be careful. A long time ago, some brave heroes from our town went to eliminate those who threatened it, but they never returned...");

                if (target.Level >= 50)
                {
                    var nRet = self.AskYesNo("If you're thinking of entering, I suggest you change your mind. But if you really want to enter... Only those strong enough to stay alive inside will be allowed. I don't want to see anyone else die. Let's see... Hmm...! You look quite strong. Okay, do you wish to enter?");
                    if (nRet == 0) self.Say("Even though your level is high, it's difficult to get in there. But if you change your mind, talk to me. After all, my duty is to protect this place.");
                    else if (nRet == 1) target.ChangeMap(211040300, "under00");
                }
                else self.Say("If you're thinking of entering, I suggest you change your mind. But if you really want to enter... Only those strong enough to stay alive inside will be allowed. I don't want to see anyone else die. Let's see... Hmmm... you haven't reached level 50 yet. I can't let you in, forget it.");
            }
        }
        // Sergeant Charlie
        [Script("carlie")]
        class carlie : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                ossyria0.self = self;
                ossyria0.target = target;
                self.Say("Hey, do you have a moment? Well, my job is to collect items here and sell them elsewhere, but lately the monsters have been getting more hostile and it's been really hard to get good items... What do you think? Want to do business with me?");
                var nRet = self.AskYesNo("The deal is simple. You get something I need and I'll get you something you need. The problem is that I deal with a lot of people. So the items I have to offer may change each time we meet. What do you think? Still interested?");
                if (nRet == 0) self.Say("Hmmm... it won't be too bad for you. Find me at the right time and you can get much better items. But let me know when you change your mind.");
                else
                {
                    var v = self.AskMenu("Alright! First you need to select the item you wish to exchange. The better the item, the greater the chance of receiving a better item in return.\r\n#b#L0# 100 #t4000073#s#l\r\n#L1# 100 #t4000059#s#l\r\n#L2# 100 #t4000076#s#l\r\n#L3# 100 #t4000058#s#l\r\n#L4# 100 #t4000078#s#l\r\n#L5# 100 #t4000060#s#l\r\n#L6# 100 #t4000062#s#l\r\n#L7# 100 #t4000048#s#l\r\n#L8# 100 #t4000081#s#l\r\n#L9# 100 #t4000061#s#l\r\n#L10# 100 #t4000070#s#l\r\n#L11# 100 #t4000071#s#l\r\n#L12# 100 #t4000072#s#l\r\n#L13# 100 #t4000051#s#l\r\n#L14# 100 #t4000055#s#l\r\n#L15# 100 #t4000069#s#l\r\n#L16# 100 #t4000052#s#l\r\n#L17# 100 #t4000050#s#l\r\n#L18# 100 #t4000057#s#l\r\n#L19# 100 #t4000049#s#l\r\n#L20# 100 #t4000056#s#l\r\n#L21# 100 #t4000079#s#l\r\n#L22# 100 #t4000053#s#l\r\n#L23# 100 #t4000054#s#l\r\n#L24# 100 #t4000080#s#l");
                    if (v == 0) replaceItem(4000073);
                    else if (v == 1) replaceItem(4000059);
                    else if (v == 2) replaceItem(4000076);
                    else if (v == 3) replaceItem(4000058);
                    else if (v == 4) replaceItem(4000078);
                    else if (v == 5) replaceItem(4000060);
                    else if (v == 6) replaceItem(4000062);
                    else if (v == 7) replaceItem(4000048);
                    else if (v == 8) replaceItem(4000081);
                    else if (v == 9) replaceItem(4000061);
                    else if (v == 10) replaceItem(4000070);
                    else if (v == 11) replaceItem(4000071);
                    else if (v == 12) replaceItem(4000072);
                    else if (v == 13) replaceItem(4000051);
                    else if (v == 14) replaceItem(4000055);
                    else if (v == 15) replaceItem(4000069);
                    else if (v == 16) replaceItem(4000052);
                    else if (v == 17) replaceItem(4000050);
                    else if (v == 18) replaceItem(4000057);
                    else if (v == 19) replaceItem(4000049);
                    else if (v == 20) replaceItem(4000056);
                    else if (v == 21) replaceItem(4000079);
                    else if (v == 22) replaceItem(4000053);
                    else if (v == 23) replaceItem(4000054);
                    else if (v == 24) replaceItem(4000080);
                }
            }
        }
    }
}
