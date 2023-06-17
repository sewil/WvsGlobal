using System;
using System.Reflection;
using WvsBeta.Common;
using WvsBeta.Game;
using WvsBeta.Game.Scripting;

namespace WvsBeta.Scripts.Scripts
{
    public static class make4
    {
        static INpcHost self;
        static GameCharacter target;
        public static void chat_message6(int index, string makeItem, string needItem, int reqLevel, string itemOption)
        {
            var inventory = target.Inventory;
            int ret = 0;

            var nRet = self.AskYesNo("To upgrade the " + makeItem + ", you will need the following materials. The Level Requirement will be " + reqLevel + ", and the basic option will be #r" + itemOption + "#k. Please make sure not to use an already upgraded item for the upgrade. What do you think? Do you want to continue with the upgrade?\r\n\r\n#b" + needItem);
            if (nRet == 0) self.Say("Don't have the materials? All good. Come back after you have all the materials needed to make the item...");
            else
            {
                // warrior glove upgrade
                if (index == 0) ret = inventory.Exchange(-18000, 1082007, -1, 4011001, -1, 1082005, 1);
                else if (index == 1) ret = inventory.Exchange(-22500, 1082007, -1, 4011005, -2, 1082006, 1);
                else if (index == 2) ret = inventory.Exchange(-27000, 1082008, -1, 4021006, -3, 1082035, 1);
                else if (index == 3) ret = inventory.Exchange(-36000, 1082008, -1, 4021008, -1, 1082036, 1);
                else if (index == 4) ret = inventory.Exchange(-40500, 1082023, -1, 4011003, -4, 1082024, 1);
                else if (index == 5) ret = inventory.Exchange(-45000, 1082023, -1, 4021008, -2, 1082025, 1);
                else if (index == 6) ret = inventory.Exchange(-49500, 1082009, -1, 4011002, -5, 1082010, 1);
                else if (index == 7) ret = inventory.Exchange(-54000, 1082009, -1, 4011006, -4, 1082011, 1);
                // magician glove upgrade
                else if (index == 100) ret = inventory.Exchange(-31500, 1082051, -1, 4021005, -3, 1082052, 1);
                else if (index == 101) ret = inventory.Exchange(-36000, 1082051, -1, 4021008, -1, 1082053, 1);
                else if (index == 102) ret = inventory.Exchange(-36000, 1082054, -1, 4021005, -3, 1082055, 1);
                else if (index == 103) ret = inventory.Exchange(-40500, 1082054, -1, 4021008, -1, 1082056, 1);
                else if (index == 104) ret = inventory.Exchange(-40500, 1082062, -1, 4021002, -4, 1082063, 1);
                else if (index == 105) ret = inventory.Exchange(-45000, 1082062, -1, 4021008, -2, 1082064, 1);
                else if (index == 106) ret = inventory.Exchange(-49500, 1082081, -1, 4021002, -5, 1082082, 1);
                else if (index == 107) ret = inventory.Exchange(-54000, 1082081, -1, 4021008, -3, 1082080, 1);
                // bowman glove upgrade
                else if (index == 200) ret = inventory.Exchange(-13500, 1082048, -1, 4021003, -3, 1082049, 1);
                else if (index == 201) ret = inventory.Exchange(-18000, 1082048, -1, 4021008, -1, 1082050, 1);
                else if (index == 202) ret = inventory.Exchange(-19800, 1082068, -1, 4011002, -4, 1082069, 1);
                else if (index == 203) ret = inventory.Exchange(-22500, 1082068, -1, 4011006, -2, 1082070, 1);
                else if (index == 204) ret = inventory.Exchange(-27000, 1082071, -1, 4011006, -4, 1082072, 1);
                else if (index == 205) ret = inventory.Exchange(-36000, 1082071, -1, 4021008, -2, 1082073, 1);
                else if (index == 206) ret = inventory.Exchange(-49500, 1082084, -1, 4021000, -5, 4011000, -1, 1082085, 1);
                else if (index == 207) ret = inventory.Exchange(-54000, 1082084, -1, 4021008, -2, 4011006, -2, 1082083, 1);
                // thief glove upgrade
                else if (index == 300) ret = inventory.Exchange(-13500, 1082042, -1, 4011004, -2, 1082043, 1);
                else if (index == 301) ret = inventory.Exchange(-17000, 1082042, -1, 4011006, -1, 1082044, 1);
                else if (index == 302) ret = inventory.Exchange(-19800, 1082046, -1, 4011005, -3, 1082047, 1);
                else if (index == 303) ret = inventory.Exchange(-22500, 1082046, -1, 4011006, -2, 1082045, 1);
                else if (index == 304) ret = inventory.Exchange(-40500, 1082075, -1, 4011006, -4, 1082076, 1);
                else if (index == 305) ret = inventory.Exchange(-45000, 1082075, -1, 4021008, -2, 1082074, 1);
                else if (index == 306) ret = inventory.Exchange(-49500, 1082065, -1, 4021000, -5, 1082067, 1);
                else if (index == 307) ret = inventory.Exchange(-54000, 1082065, -1, 4011006, -2, 4021008, -1, 1082066, 1);

                if (ret == 0) self.Say("Please make sure you have all the items you need, and whether your equipment inventory is full or not.");
                else self.Say("Here! Take the " + makeItem + "! Look how beautiful it is... the impeccable stitching, the beautiful craftsmanship... isn't this item beautifully made? Please check back anytime!~");
            }
        }
        public static void chat_message5(int index, string makeItem, string needItem, int reqLevel)
        {
            var inventory = target.Inventory;
            int ret = 0;

            var nRet = self.AskYesNo("To make a " + makeItem + ", you will need the following materials. The Level Requirement for this item is " + reqLevel + ", so make sure it's something you really need. Want me to do it?\r\n\r\n#b" + needItem);
            if (nRet == 0) self.Say("Don't have the materials? All good. Come back after you have all the materials needed to make the item...");
            else
            {
                // creating warrior glove
                if (index == 0) ret = inventory.Exchange(-18000, 4011000, -3, 4011001, -2, 4003000, -15, 1082007, 1);
                else if (index == 1) ret = inventory.Exchange(-27000, 4011001, -4, 4000021, -30, 4003000, -30, 1082008, 1);
                else if (index == 2) ret = inventory.Exchange(-36000, 4011001, -5, 4000021, -50, 4003000, -40, 1082023, 1);
                else if (index == 3) ret = inventory.Exchange(-45000, 4011001, -3, 4021007, -2, 4000030, -30, 4003000, -45, 1082009, 1);
                // creating magician glove
                else if (index == 100) ret = inventory.Exchange(-22500, 4000021, -60, 4021006, -1, 4021000, -2, 1082051, 1);
                else if (index == 101) ret = inventory.Exchange(-27000, 4000021, -70, 4011006, -1, 4011001, -3, 4021000, -2, 1082054, 1);
                else if (index == 102) ret = inventory.Exchange(-36000, 4000021, -80, 4021000, -3, 4021006, -3, 4003000, -30, 1082062, 1);
                else if (index == 103) ret = inventory.Exchange(-45000, 4021000, -3, 4011006, -2, 4000030, -35, 4003000, -40, 1082081, 1);
                // creating bowman glove
                else if (index == 200) ret = inventory.Exchange(-18000, 4000021, -50, 4021001, -1, 4011006, -2, 1082048, 1);
                else if (index == 201) ret = inventory.Exchange(-27000, 4000021, -60, 4011001, -3, 4011000, -1, 4003000, -15, 1082068, 1);
                else if (index == 202) ret = inventory.Exchange(-36000, 4000021, -80, 4021002, -3, 4011001, -3, 4021000, -1, 4003000, -25, 1082071, 1);
                else if (index == 203) ret = inventory.Exchange(-45000, 4011004, -3, 4021002, -2, 4011006, -1, 4000030, -40, 4003000, -35, 1082084, 1);
                // creating thief glove
                else if (index == 300) ret = inventory.Exchange(-22500, 4011001, -2, 4000021, -50, 4003000, -10, 1082042, 1);
                else if (index == 301) ret = inventory.Exchange(-27000, 4011001, -3, 4011000, -1, 4000021, -60, 4003000, -15, 1082046, 1);
                else if (index == 302) ret = inventory.Exchange(-36000, 4000021, -80, 4021000, -3, 4000101, -100, 4003000, -30, 1082075, 1);
                else if (index == 303) ret = inventory.Exchange(-45000, 4021005, -3, 4021008, -1, 4000030, -40, 4003000, -30, 1082065, 1);

                if (ret == 0) self.Say("Please make sure you have all the items you need, and whether your equipment inventory is full or not.");
                else self.Say("Here! Take the " + makeItem + "! Look how beautiful it is... the impeccable stitching, the beautiful craftsmanship... isn't this item beautifully made? Please check back anytime!");
            }
        }
        public static void chat_message4(int index, string makeItem, string needItem, int reqLevel, string itemOption)
        {
            var inventory = target.Inventory;
            int ret = 0;

            var nRet = self.AskYesNo("To upgrade " + makeItem + " with the stimulator, I will need the following items. The Level Requirement is " + reqLevel + ", and the basic option is " + itemOption + ". Please make sure not to use an upgraded item as upgrade material. If the stimulator is used, the item option can increase, but #bit can also come out worse than it was before, as well as carry a 10% failure chance#k, so please be careful. So what do you think? Do you want to upgrade the item?\r\n\r\n#b" + needItem);
            if (nRet == 0) self.Say("I understand. When using the stimulator, you run the risk of not being able to craft the item, and thus you may still waste materials in the process. I'd be careful too, but if you change your mind, come back anytime.");
            else
            {
                var sProp = Rand32.NextBetween(1, 10);
                if (sProp == 1)
                {
                    // warrior glove upgrade - failed
                    if (index == 0) ret = inventory.Exchange(0, 4130000, -1, 1082007, -1, 4011001, -1);
                    else if (index == 1) ret = inventory.Exchange(0, 4130000, -1, 1082007, -1, 4011005, -2);
                    else if (index == 2) ret = inventory.Exchange(0, 4130000, -1, 1082008, -1, 4021006, -3);
                    else if (index == 3) ret = inventory.Exchange(0, 4130000, -1, 1082008, -1, 4021008, -1);
                    else if (index == 4) ret = inventory.Exchange(0, 4130000, -1, 1082023, -1, 4011003, -4);
                    else if (index == 5) ret = inventory.Exchange(0, 4130000, -1, 1082023, -1, 4021008, -2);
                    else if (index == 6) ret = inventory.Exchange(0, 4130000, -1, 1082009, -1, 4011002, -5);
                    else if (index == 7) ret = inventory.Exchange(0, 4130000, -1, 1082009, -1, 4011006, -4);
                    // magician glove upgrade - failed
                    else if (index == 100) ret = inventory.Exchange(0, 4130000, -1, 1082051, -1, 4021005, -3);
                    else if (index == 101) ret = inventory.Exchange(0, 4130000, -1, 1082051, -1, 4021008, -1);
                    else if (index == 102) ret = inventory.Exchange(0, 4130000, -1, 1082054, -1, 4021005, -3);
                    else if (index == 103) ret = inventory.Exchange(0, 4130000, -1, 1082054, -1, 4021008, -1);
                    else if (index == 104) ret = inventory.Exchange(0, 4130000, -1, 1082062, -1, 4021002, -4);
                    else if (index == 105) ret = inventory.Exchange(0, 4130000, -1, 1082062, -1, 4021008, -2);
                    else if (index == 106) ret = inventory.Exchange(0, 4130000, -1, 1082081, -1, 4021002, -5);
                    else if (index == 107) ret = inventory.Exchange(0, 4130000, -1, 1082081, -1, 4021008, -3);
                    // bowman glove upgrade - failed
                    else if (index == 200) ret = inventory.Exchange(0, 4130000, -1, 1082048, -1, 4021003, -3);
                    else if (index == 201) ret = inventory.Exchange(0, 4130000, -1, 1082048, -1, 4021008, -1);
                    else if (index == 202) ret = inventory.Exchange(0, 4130000, -1, 1082068, -1, 4011002, -4);
                    else if (index == 203) ret = inventory.Exchange(0, 4130000, -1, 1082068, -1, 4011006, -2);
                    else if (index == 204) ret = inventory.Exchange(0, 4130000, -1, 1082071, -1, 4011006, -4);
                    else if (index == 205) ret = inventory.Exchange(0, 4130000, -1, 1082071, -1, 4021008, -2);
                    else if (index == 206) ret = inventory.Exchange(0, 4130000, -1, 1082084, -1, 4021000, -5, 4011000, -1);
                    else if (index == 207) ret = inventory.Exchange(0, 4130000, -1, 1082084, -1, 4021008, -2, 4011006, -2);
                    // thief glove upgrade - failed
                    else if (index == 300) ret = inventory.Exchange(0, 4130000, -1, 1082042, -1, 4011004, -2);
                    else if (index == 301) ret = inventory.Exchange(0, 4130000, -1, 1082042, -1, 4011006, -1);
                    else if (index == 302) ret = inventory.Exchange(0, 4130000, -1, 1082046, -1, 4011005, -3);
                    else if (index == 303) ret = inventory.Exchange(0, 4130000, -1, 1082046, -1, 4011006, -2);
                    else if (index == 304) ret = inventory.Exchange(0, 4130000, -1, 1082075, -1, 4011006, -4);
                    else if (index == 305) ret = inventory.Exchange(0, 4130000, -1, 1082075, -1, 4021008, -2);
                    else if (index == 306) ret = inventory.Exchange(0, 4130000, -1, 1082065, -1, 4021000, -5);
                    else if (index == 307) ret = inventory.Exchange(0, 4130000, -1, 1082065, -1, 4011006, -2);

                    if (ret == 0) self.Say("Please make sure you have all the items you need, and whether your equipment inventory is full or not.");
                    else self.Say("Damn... I must have put on too much of the stimulator... All items are gone now... I'm sorry. I warned you about the possibility of this happening, I hope you understand.");
                }
                else
                {
                    // warrior glove upgrade - success
                    if (index == 0) ret = inventory.ExchangeEx(-20000, "4130000", -1, "1082007", -1, "4011001", -1, "1082005,Variation:1", 1);
                    else if (index == 1) ret = inventory.ExchangeEx(-25000, "4130000", -1, "1082007", -1, "4011005", -2, "1082006,Variation:1", 1);
                    else if (index == 2) ret = inventory.ExchangeEx(-30000, "4130000", -1, "1082008", -1, "4021006", -3, "1082035,Variation:1", 1);
                    else if (index == 3) ret = inventory.ExchangeEx(-40000, "4130000", -1, "1082008", -1, "4021008", -1, "1082036,Variation:1", 1);
                    else if (index == 4) ret = inventory.ExchangeEx(-45000, "4130000", -1, "1082023", -1, "4011003", -4, "1082024,Variation:1", 1);
                    else if (index == 5) ret = inventory.ExchangeEx(-50000, "4130000", -1, "1082023", -1, "4021008", -2, "1082025,Variation:1", 1);
                    else if (index == 6) ret = inventory.ExchangeEx(-55000, "4130000", -1, "1082009", -1, "4011002", -5, "1082010,Variation:1", 1);
                    else if (index == 7) ret = inventory.ExchangeEx(-60000, "4130000", -1, "1082009", -1, "4011006", -4, "1082011,Variation:1", 1);
                    // magician glove upgrade - success
                    else if (index == 100) ret = inventory.ExchangeEx(-35000, "4130000", -1, "1082051", -1, "4021005", -3, "1082052,Variation:1", 1);
                    else if (index == 101) ret = inventory.ExchangeEx(-40000, "4130000", -1, "1082051", -1, "4021008", -1, "1082053,Variation:1", 1);
                    else if (index == 102) ret = inventory.ExchangeEx(-40000, "4130000", -1, "1082054", -1, "4021005", -3, "1082055,Variation:1", 1);
                    else if (index == 103) ret = inventory.ExchangeEx(-45000, "4130000", -1, "1082054", -1, "4021008", -1, "1082056,Variation:1", 1);
                    else if (index == 104) ret = inventory.ExchangeEx(-45000, "4130000", -1, "1082062", -1, "4021002", -4, "1082063,Variation:1", 1);
                    else if (index == 105) ret = inventory.ExchangeEx(-50000, "4130000", -1, "1082062", -1, "4021008", -2, "1082064,Variation:1", 1);
                    else if (index == 106) ret = inventory.ExchangeEx(-55000, "4130000", -1, "1082081", -1, "4021002", -5, "1082082,Variation:1", 1);
                    else if (index == 107) ret = inventory.ExchangeEx(-60000, "4130000", -1, "1082081", -1, "4021008", -3, "1082080,Variation:1", 1);
                    // bowman glove upgrade - success
                    else if (index == 200) ret = inventory.ExchangeEx(-15000, "4130000", -1, "1082048", -1, "4021003", -3, "1082049,Variation:1", 1);
                    else if (index == 201) ret = inventory.ExchangeEx(-20000, "4130000", -1, "1082048", -1, "4021008", -1, "1082050,Variation:1", 1);
                    else if (index == 202) ret = inventory.ExchangeEx(-22000, "4130000", -1, "1082068", -1, "4011002", -4, "1082069,Variation:1", 1);
                    else if (index == 203) ret = inventory.ExchangeEx(-25000, "4130000", -1, "1082068", -1, "4011006", -2, "1082070,Variation:1", 1);
                    else if (index == 204) ret = inventory.ExchangeEx(-30000, "4130000", -1, "1082071", -1, "4011006", -4, "1082072,Variation:1", 1);
                    else if (index == 205) ret = inventory.ExchangeEx(-40000, "4130000", -1, "1082071", -1, "4021008", -2, "1082073,Variation:1", 1);
                    else if (index == 206) ret = inventory.ExchangeEx(-55000, "4130000", -1, "1082084", -1, "4021000", -5, "4011000", -1, "1082085,Variation:1", 1);
                    else if (index == 207) ret = inventory.ExchangeEx(-60000, "4130000", -1, "1082084", -1, "4021008", -2, "4011006", -2, "1082083,Variation:1", 1);
                    // thief glove upgrade - success
                    else if (index == 300) ret = inventory.ExchangeEx(-15000, "4130000", -1, "1082042", -1, "4011004", -2, "1082043,Variation:1", 1);
                    else if (index == 301) ret = inventory.ExchangeEx(-20000, "4130000", -1, "1082042", -1, "4011006", -1, "1082044,Variation:1", 1);
                    else if (index == 302) ret = inventory.ExchangeEx(-22000, "4130000", -1, "1082046", -1, "4011005", -3, "1082047,Variation:1", 1);
                    else if (index == 303) ret = inventory.ExchangeEx(-25000, "4130000", -1, "1082046", -1, "4011006", -2, "1082045,Variation:1", 1);
                    else if (index == 304) ret = inventory.ExchangeEx(-45000, "4130000", -1, "1082075", -1, "4011006", -4, "1082076,Variation:1", 1);
                    else if (index == 305) ret = inventory.ExchangeEx(-50000, "4130000", -1, "1082075", -1, "4021008", -2, "1082074,Variation:1", 1);
                    else if (index == 306) ret = inventory.ExchangeEx(-55000, "4130000", -1, "1082065", -1, "4021000", -5, "1082067,Variation:1", 1);
                    else if (index == 307) ret = inventory.ExchangeEx(-60000, "4130000", -1, "1082065", -1, "4011006", -2, "4021008", -1, "1082066,Variation:1", 1);

                    if (ret == 0) self.Say("Please make sure you have all the items you need, and whether your equipment inventory is full or not.");
                    else self.Say("Here! Take the " + makeItem + "! Everything worked out, and this glove looks much prettier than a regular glove. You're welcome back anytime!");
                }
            }
        }
        public static void chat_message3(int index, string makeItem, string needItem, int unitNum, string itemOption)
        {
            var inventory = target.Inventory;
            int nRet, ret = 0;

            if (itemOption == "") nRet = self.AskYesNo("To make #b" + unitNum + " " + makeItem + "(s)#k, I will need the following materials. If you have the materials, it will be much better for you to craft the item here than to buy it in the store. Are you sure you want to do it?\r\n\r\n#b" + needItem);
            else nRet = self.AskYesNo("You want to make #b" + unitNum + " " + makeItem + "(s)#k?\r\n Give me\r\n#b" + needItem + "#k\r\nand I can create it for you. By the way, this arrow has the item option of #b" + itemOption + "#k. It's a special type of arrow, so if you have the materials, why not craft some? Wanna do it?");
            if (nRet == 0) self.Say("Don't have the materials? You can get materials by eliminating nearby monsters! Go fight them!");
            else
            {
                if (index == 1) ret = inventory.Exchange(0, 4003001, -1, 4003004, -1, 2060000, 1000);
                else if (index == 2) ret = inventory.Exchange(0, 4003001, -1, 4003004, -1, 2061000, 1000);
                else if (index == 3) ret = inventory.Exchange(0, 4011000, -1, 4003001, -3, 4003004, -10, 2060001, 900);
                else if (index == 4) ret = inventory.Exchange(0, 4011000, -1, 4003001, -3, 4003004, -10, 2061001, 900);
                else if (index == 5) ret = inventory.Exchange(0, 4011001, -1, 4003001, -5, 4003005, -15, 2060002, 800);
                else if (index == 6) ret = inventory.Exchange(0, 4011001, -1, 4003001, -5, 4003005, -15, 2061002, 800);
            }
        }
        public static void chat_message2(int index, string makeItem, string needItem, int needNumber, int itemNumber)
        {
            var inventory = target.Inventory;
            int ret = 0;

            var nRetNum = self.AskNumber("With #b" + needNumber + " " + needItem + "(s)#k, I can create " + itemNumber + " " + makeItem + "(s). I can make these for you for free, as long as you have the required materials. What do you think? How many of these do you want to make?", 1, 1, 100);
            var nNeedNum = nRetNum * needNumber;
            var nAllNum = nRetNum * itemNumber;
            var nRetBuy = self.AskYesNo("Do you want to make #b" + makeItem + "#k " + nRetNum + " time(s)? Then I will need #r" + nNeedNum + " " + needItem + "(s)#k. Are you sure you want to do it?");
            if (nRetBuy == 0) self.Say("Don't have the materials? You can gather materials by eliminating nearby monsters! Go fight them!");
            else
            {
                if (index == 1) ret = inventory.Exchange(0, 4000003, -nNeedNum, 4003001, nAllNum);
                else if (index == 2) ret = inventory.Exchange(0, 4000018, -nNeedNum, 4003001, nAllNum);
                else if (index == 3) ret = inventory.Exchange(0, 4011001, -nNeedNum, 4011000, -nNeedNum, 4003000, nAllNum);
                if (ret == 0) self.Say("Please make sure you have all the necessary items with you. Otherwise, check your etc. inventory, it might be full!!");
                else self.Say("Here, take #b" + nAllNum + " " + makeItem + "(s). I think it turned out pretty cool. Isn't it beautiful? Check back often!");
            }
        }
        public static void chat_message4(int index, string makeItem, string needItemIcon, string needItemString, int unitPrice)
        {
            var inventory = target.Inventory;
            int ret = 0;

            if (index == 200 || index == 201)
            {
                var nRetNum = self.AskNumber("In order to make a rare gem, you will need the materials worthy of it... well, how many #b" + makeItem + "(s)#k would you like to make?", 1, 1, 100);
                var nPrice = unitPrice * nRetNum;
                var nRetBuy = self.AskYesNo("Do you want #b" + nRetNum + " " + makeItem + "(s)#k? If so, you will need #r" + nPrice + " mesos and " + needItemIcon + " " + nRetNum + " " + needItemString + "(s) each#k. What do you think? Do you really want me to do it?");
                if (nRetBuy == 0) self.Say("I understand... Please come back again whenever you want! I'll be here as always, waiting for customers like you!");
                else
                {
                    // rare jewel
                    if (index == 200) ret = inventory.Exchange(-10000, 4011000, -nRetNum, 4011001, -nRetNum, 4011002, -nRetNum, 4011003, -nRetNum, 4011004, -nRetNum, 4011005, -nRetNum, 4011006, -nRetNum, 4011007, nRetNum);
                    else if (index == 201) ret = inventory.Exchange(-15000, 4021000, -nRetNum, 4021001, -nRetNum, 4021002, -nRetNum, 4021003, -nRetNum, 4021004, -nRetNum, 4021005, -nRetNum, 4021006, -nRetNum, 4021007, -nRetNum, 4021008, -nRetNum, 4021009, nRetNum);

                    if (ret == 0) self.Say("Please make sure you have all the necessary items with you. Otherwise, check your etc. inventory, it might be full!!");
                    else self.Say("Here, take #b" + nRetNum + " " + makeItem + "(s)#k. I think it turned out pretty cool. Isn't it beautiful? Check back often!");
                }
            }
            else
            {
                int nRetNum = self.AskNumber("To make " + makeItem + ", I will need the following materials. How many do you want to make?\r\n\r\n#b" + needItemIcon + " " + needItemString + "s\r\n" + unitPrice + " mesos#k", 1, 1, 100);
                int nPrice = unitPrice * nRetNum;
                var nAllNum = nRetNum * 10;
                int nRetBuy = self.AskYesNo("To make #b" + nRetNum + " " + makeItem + "(s)#k, I will need the following materials. Are you sure you want to do it?\r\n\r\n#b" + needItemIcon + " " + nAllNum + " " + needItemString + "(s)\r\n" + nPrice + " mesos#k");
                if (nRetBuy == 0) self.Say("I understand... Please come back again whenever you want! I'll be here as always, waiting for customers like you!");
                else
                {
                    // mineral
                    if (index == 1) ret = inventory.Exchange(-nPrice, 4010000, -nAllNum, 4011000, nRetNum);
                    else if (index == 2) ret = inventory.Exchange(-nPrice, 4010001, -nAllNum, 4011001, nRetNum);
                    else if (index == 3) ret = inventory.Exchange(-nPrice, 4010002, -nAllNum, 4011002, nRetNum);
                    else if (index == 4) ret = inventory.Exchange(-nPrice, 4010003, -nAllNum, 4011003, nRetNum);
                    else if (index == 5) ret = inventory.Exchange(-nPrice, 4010004, -nAllNum, 4011004, nRetNum);
                    else if (index == 6) ret = inventory.Exchange(-nPrice, 4010005, -nAllNum, 4011005, nRetNum);
                    else if (index == 7) ret = inventory.Exchange(-nPrice, 4010006, -nAllNum, 4011006, nRetNum);
                    else if (index == 8) ret = inventory.Exchange(-nPrice, 4010007, -nAllNum, 4011008, nRetNum);
                    // ºžŒ®
                    else if (index == 100) ret = inventory.Exchange(-nPrice, 4020000, -nAllNum, 4021000, nRetNum);
                    else if (index == 101) ret = inventory.Exchange(-nPrice, 4020001, -nAllNum, 4021001, nRetNum);
                    else if (index == 102) ret = inventory.Exchange(-nPrice, 4020002, -nAllNum, 4021002, nRetNum);
                    else if (index == 103) ret = inventory.Exchange(-nPrice, 4020003, -nAllNum, 4021003, nRetNum);
                    else if (index == 104) ret = inventory.Exchange(-nPrice, 4020004, -nAllNum, 4021004, nRetNum);
                    else if (index == 105) ret = inventory.Exchange(-nPrice, 4020005, -nAllNum, 4021005, nRetNum);
                    else if (index == 106) ret = inventory.Exchange(-nPrice, 4020006, -nAllNum, 4021006, nRetNum);
                    else if (index == 107) ret = inventory.Exchange(-nPrice, 4020007, -nAllNum, 4021007, nRetNum);
                    else if (index == 108) ret = inventory.Exchange(-nPrice, 4020008, -nAllNum, 4021008, nRetNum);
                    // crystal
                    else if (index == 300) ret = inventory.Exchange(-nPrice, 4004000, -nAllNum, 4005000, nRetNum);
                    else if (index == 301) ret = inventory.Exchange(-nPrice, 4004001, -nAllNum, 4005001, nRetNum);
                    else if (index == 302) ret = inventory.Exchange(-nPrice, 4004002, -nAllNum, 4005002, nRetNum);
                    else if (index == 303) ret = inventory.Exchange(-nPrice, 4004003, -nAllNum, 4005003, nRetNum);
                    else if (index == 304) ret = inventory.Exchange(-nPrice, 4004004, -nAllNum, 4005004, nRetNum);

                    if (ret == 0) self.Say("Please make sure you have all the necessary items with you. Otherwise, check your etc. inventory, it might be full!!");
                    else self.Say("Here, take #b" + nRetNum + " " + makeItem + "(s). I think it turned out pretty cool. Isn't it beautiful? Check back often!");
                }
            }
        }
        public static void chat_message1(int index, string makeItem, string needItem, int unitPrice)
        {
            var inventory = target.Inventory;
            var field = self.Field;
            int ret = 0;

            if (index == 200 || index == 201)
            {
                var nRetNum = self.AskNumber("In order to make a rare gem, you will need the materials worthy of it... well, how many #b" + makeItem + "(s)#k would you like to make?", 1, 1, 100);
                var nPrice = unitPrice * nRetNum;
                var nRetBuy = self.AskYesNo("Do you want to make #b" + nRetNum + " " + makeItem + "(s)#k? If so, you will need #r" + nPrice + " mesos and " + nRetNum + " " + needItem + " each#k. What do you think? Do you really want me to do it?");
                if (nRetBuy == 0) self.Say("I understand... Please come back again whenever you want! I'll be here as always, waiting for customers like you!");
                else
                {
                    // rare jewel
                    if (index == 200) ret = inventory.Exchange(-9000 * nRetNum, 4011000, -nRetNum, 4011001, -nRetNum, 4011002, -nRetNum, 4011003, -nRetNum, 4011004, -nRetNum, 4011005, -nRetNum, 4011006, -nRetNum, 4011007, nRetNum);
                    else if (index == 201) ret = inventory.Exchange(-13500 * nRetNum, 4021000, -nRetNum, 4021001, -nRetNum, 4021002, -nRetNum, 4021003, -nRetNum, 4021004, -nRetNum, 4021005, -nRetNum, 4021006, -nRetNum, 4021007, -nRetNum, 4021008, -nRetNum, 4021009, nRetNum);

                    if (ret == 0) self.Say("Please make sure you have all the necessary items with you. Otherwise, check your etc. inventory, it might be full!!");
                    else self.Say("Here, take #b" + nRetNum + " " + makeItem + "(s)#k. I think it turned out pretty cool. Isn't it beautiful? Check back often!");
                }
            }
            else
            {
                var nRetNum = self.AskNumber("To make " + makeItem + ", I will need the following materials. How many would you like to make?\r\n\r\n#b10" + needItem + "\r\n#v4031138# " + unitPrice + " mesos#k", 1, 1, 100);
                var nPrice = unitPrice * nRetNum;
                var nAllNum = nRetNum * 10;
                var nRetBuy = self.AskYesNo("To make #b" + nRetNum + " " + makeItem + "(s)#k, I will need the following materials. Are you sure you want to do it?\r\n\r\n#b" + nAllNum + " " + needItem + "\r\n#v4031138# " + nPrice + " mesos#k");
                if (nRetBuy == 0) self.Say("I understand... Please come back again whenever you want! I'll be here as always, waiting for customers like you!");
                else
                {
                    // mineral
                    if (index == 1) ret = inventory.Exchange(-nPrice, 4010000, -nAllNum, 4011000, nRetNum);
                    else if (index == 2) ret = inventory.Exchange(-nPrice, 4010001, -nAllNum, 4011001, nRetNum);
                    else if (index == 3) ret = inventory.Exchange(-nPrice, 4010002, -nAllNum, 4011002, nRetNum);
                    else if (index == 4) ret = inventory.Exchange(-nPrice, 4010003, -nAllNum, 4011003, nRetNum);
                    else if (index == 5) ret = inventory.Exchange(-nPrice, 4010004, -nAllNum, 4011004, nRetNum);
                    else if (index == 6) ret = inventory.Exchange(-nPrice, 4010005, -nAllNum, 4011005, nRetNum);
                    else if (index == 7) ret = inventory.Exchange(-nPrice, 4010006, -nAllNum, 4011006, nRetNum);
                    else if (index == 8) ret = inventory.Exchange(-nPrice, 4010007, -nAllNum, 4011008, nRetNum);
                    // ºžŒ®
                    else if (index == 100) ret = inventory.Exchange(-nPrice, 4020000, -nAllNum, 4021000, nRetNum);
                    else if (index == 101) ret = inventory.Exchange(-nPrice, 4020001, -nAllNum, 4021001, nRetNum);
                    else if (index == 102) ret = inventory.Exchange(-nPrice, 4020002, -nAllNum, 4021002, nRetNum);
                    else if (index == 103) ret = inventory.Exchange(-nPrice, 4020003, -nAllNum, 4021003, nRetNum);
                    else if (index == 104) ret = inventory.Exchange(-nPrice, 4020004, -nAllNum, 4021004, nRetNum);
                    else if (index == 105) ret = inventory.Exchange(-nPrice, 4020005, -nAllNum, 4021005, nRetNum);
                    else if (index == 106) ret = inventory.Exchange(-nPrice, 4020006, -nAllNum, 4021006, nRetNum);
                    else if (index == 107) ret = inventory.Exchange(-nPrice, 4020007, -nAllNum, 4021007, nRetNum);
                    else if (index == 108) ret = inventory.Exchange(-nPrice, 4020008, -nAllNum, 4021008, nRetNum);
                    // crystal
                    else if (index == 300) ret = inventory.Exchange(-nPrice, 4004000, -nAllNum, 4005000, nRetNum);
                    else if (index == 301) ret = inventory.Exchange(-nPrice, 4004001, -nAllNum, 4005001, nRetNum);
                    else if (index == 302) ret = inventory.Exchange(-nPrice, 4004002, -nAllNum, 4005002, nRetNum);
                    else if (index == 303) ret = inventory.Exchange(-nPrice, 4004003, -nAllNum, 4005003, nRetNum);
                    else if (index == 304) ret = inventory.Exchange(-nPrice, 4004004, -nAllNum, 4005004, nRetNum);

                    if (ret == 0) self.Say("Please make sure you have all the necessary items with you. Otherwise, check your etc. inventory, it might be full!!");
                    else self.Say("Here, take #b" + nRetNum + " " + makeItem + "(s). I think it turned out pretty cool. Isn't it beautiful? Check back often!");
                }
            }
        }
        // Ludibrium - creating mineral, jewel, material, arrow
        [Script("make_ludi1")]
        class make_ludi1 : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                make4.self = self;
                make4.target = target;
                var nRet1 = self.AskYesNo("Do you have ores of a jewel or mineral? For a small service fee, I can turn them into materials for weapons and armor. I can even create some powerful arrows. Oh! And the service fee is about #r10%#k, so feel free when needed. How's it sound?");
                if (nRet1 == 0) self.Say("I understand. Well, if you ever find any strange ores or items, just drop by and see me. I will create something for you that only I can make.");
                else
                {
                    var v1 = self.AskMenu("Alright! Before doing this, make sure you have some space available in your etc. inventory. Let's see, what would you like to do?",
                        "Refine a mineral",
                        "Refine a jewel",
                        "Refine a rare gem",
                        "Refine a crystal",
                        "Create materials",
                        "Create arrows"
                    );
                    if (v1 == 0)
                    {
                        var v2 = self.AskMenu("What kind of mineral would you like to make?\r\n#b#L0# #t4011000##l\r\n#L1# #t4011001##l\r\n#L2# #t4011002##l\r\n#L3# #t4011003##l\r\n#L4# #t4011004##l\r\n#L5# #t4011005##l\r\n#L6# #t4011006##l");
                        if (v2 == 0) chat_message4(1, "#t4011000#", "#v4010000#", "#t4010000#s", 270);
                        else if (v2 == 1) chat_message4(2, "#t4011001#", "#v4010001#", "#t4010001#s", 270);
                        else if (v2 == 2) chat_message4(3, "#t4011002#", "#v4010002#", "#t4010002#s", 270);
                        else if (v2 == 3) chat_message4(4, "#t4011003#", "#v4010003#", "#t4010003#s", 450);
                        else if (v2 == 4) chat_message4(5, "#t4011004#", "#v4010004#", "#t4010004#s", 450);
                        else if (v2 == 5) chat_message4(6, "#t4011005#", "#v4010005#", "#t4010005#s", 450);
                        else if (v2 == 6) chat_message4(7, "#t4011006#", "#v4010006#", "#t4010006#s", 720);
                    }
                    else if (v1 == 1)
                    {
                        var v2 = self.AskMenu("Which jewel would you like to refine?\r\n#b#L0# #t4021000##l\r\n#L1# #t4021001##l\r\n#L2# #t4021002##l\r\n#L3# #t4021003##l\r\n#L4# #t4021004##l\r\n#L5# #t4021005##l\r\n#L6# #t4021006##l\r\n#L7# #t4021007##l\r\n#L8# #t4021008##l");
                        if (v2 == 0) chat_message4(100, "#t4021000#", "#v4020000#", "#t4020000#s", 450);
                        else if (v2 == 1) chat_message4(101, "#t4021001#", "#v4020001#", "#t4020001#s", 450);
                        else if (v2 == 2) chat_message4(102, "#t4021002#", "#v4020002#", "#t4020002#s", 450);
                        else if (v2 == 3) chat_message4(103, "#t4021003#", "#v4020003#", "#t4020003#s", 450);
                        else if (v2 == 4) chat_message4(104, "#t4021004#", "#v4020004#", "#t4020004#s", 450);
                        else if (v2 == 5) chat_message4(105, "#t4021005#", "#v4020005#", "#t4020005#s", 450);
                        else if (v2 == 6) chat_message4(106, "#t4021006#", "#v4020006#", "#t4020006#s", 450);
                        else if (v2 == 7) chat_message4(107, "#t4021007#", "#v4020007#", "#t4020007#s", 900);
                        else if (v2 == 8) chat_message4(108, "#t4021008#", "#v4020008#", "#t4020008#s", 2700);
                    }
                    else if (v1 == 2)
                    {
                        var v2 = self.AskMenu("Alright! I can refine any mineral or jewel. I just need enough materials to do it. Which one would you like to make?\r\n#b#L0##t4011007##l\r\n#L1##t4021009##l#k");
                        if (v2 == 0) chat_message1(200, "#t4011007#", "#t4011000#, #t4011001#, #t4011002#, #t4011003#, #t4011004#, #t4011005#, #t4011006#", 9000);
                        else if (v2 == 1) chat_message1(201, "#t4021009#", "#t4021000#, #t4021001#, #t4021002#, #t4021003#, #t4021004#, #t4021005#, #t4021006#, #t4021007#, #t4021008#", 13500);
                    }
                    else if (v1 == 3)
                    {
                        var v2 = self.AskMenu("So you happen to have a crystal? It's been a long time since I last saw one, but... if you have one, I can refine it for you. Which crystal would you like to refine?\r\n#b#L0# #t4005000##l\r\n#L1# #t4005001##l\r\n#L2# #t4005002##l\r\n#L3# #t4005003##l#k");
                        if (v2 == 0) chat_message4(300, "#t4005000#", "#v4004000#", "#t4004000#s", 4500);
                        else if (v2 == 1) chat_message4(301, "#t4005001#", "#v4004001#", "#t4004001#s", 4500);
                        else if (v2 == 2) chat_message4(302, "#t4005002#", "#v4004002#", "#t4004002#s", 4500);
                        else if (v2 == 3) chat_message4(303, "#t4005003#", "#v4004003#", "#t4004003#s", 4500);
                    }
                    else if (v1 == 4)
                    {
                        var v2 = self.AskMenu("So, you want to create some materials? Let's see... what kind of materials would you like to make?\r\n#L0##b Create #t4003001# with #t4000003##k#l\r\n#L1##b Create #t4003001# with #t4000018# #k#l\r\n#L2##b Create #t4003000##k#l");
                        if (v2 == 0) chat_message2(1, "#t4003001#", "#t4000003#", 10, 1);
                        else if (v2 == 1) chat_message2(2, "#t4003001#", "#t4000018#", 5, 1);
                        else if (v2 == 2) chat_message2(3, "#t4003000#s", "#t4011001# and #t4011000# each", 1, 15);
                    }
                    else if (v1 == 5)
                    {
                        var v2 = self.AskMenu("So, you want to make some arrows? A powerful arrow can help you a lot during combat. Let's see... What kind of arrow would you like to make?\r\n#L0##b #t2060000##k#l\r\n#L1##b #t2061000##k#l\r\n#L2##b #t2060001##k#l\r\n#L3##b #t2061001##k#l\r\n#L4##b #t2060002##k#l\r\n#L5##b #t2061002##k#l");
                        if (v2 == 0) chat_message3(1, "#t2060000#", "#v4003001# #t4003001#\r\n#v4003004# #t4003004#", 1000, "");
                        else if (v2 == 1) chat_message3(2, "#t2061000#", "#v4003001# #t4003001#\r\n#v4003004# #t4003004#", 1000, "");
                        else if (v2 == 2) chat_message3(3, "#t2060001#", "#v4011000# #t4011000#\r\n#v4003001# 3 #t4003001#s\r\n#v4003004# 10 #t4003004#s", 900, "Attack 1");
                        else if (v2 == 3) chat_message3(4, "#t2061001#", "#v4011000# #t4011000#\r\n#v4003001# 3 #t4003001#s\r\n#v4003004# 10 #t4003004#s", 900, "Attack 1");
                        else if (v2 == 4) chat_message3(5, "#t2060002#", "#v4011001# #t4011001#\r\n#v4003001# 5 #t4003001#s\r\n#v4003005# 15 #t4003005#s", 800, "Attack 2");
                        else if (v2 == 5) chat_message3(6, "#t2061002#", "#v4011001# #t4011001#\r\n#v4003001# 5 #t4003001#s\r\n#v4003005# 15 #t4003005#s", 800, "Attack 2");
                    }
                }
            }
        }
        // creating gloves at Ludibrium
        [Script("make_ludi2")]
        class make_ludi2 : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                make4.self = self;
                make4.target = target;
                var nRet1 = self.AskYesNo("Man... no one understands how hard it is to create a cool glove, but everyone who uses my gloves agrees that they are the best you can find. How about you let me make one for you?");
                if (nRet1 == 0) self.Say("Hmm~ you're better off not treating me the way you treat other glove makers. I can do something that nobody else can, so if you want to know more about it, come back later and talk to me.");
                else
                {
                    var v1 = self.AskMenu("Alright! The service fee will be very reasonable, don't you worry. What would you like to do?",
                        "What is the simulator?",
                        "Upgrade a glove using the stimulator",
                        "Create a regular Warrior glove",
                        "Create a regular Magician glove",
                        "Create a regular Bowman glove",
                        "Create a regular Thief glove",
                        "Upgrade a regular Warrior glove",
                        "Upgrade a regular Magician glove",
                        "Upgrade a regular Bowman glove",
                        "Upgrade a regular Thief glove"
                    );
                    string jobName = Strings.Job.GetName((short)((v1 - 1) * 100));
                    string ask;
                    if (v1 < 6)
                    {
                        ask = "So you want to create a " + jobName + " glove? You will not be able to use the stimulator for this. Please choose the item you want to create~";
                    }
                    else
                    {
                        ask = "Do you want to upgrade a regular " + jobName + " glove? There is no chance of failure, as the stimulator will not be used, and the item option will be basic. Please choose the item you want to create~";
                    }

                    if (v1 == 0)
                    {
                        self.Say("I told you this is something only I can do. Would you like to know more about the #rstimulator#k? It's a mysterious potion that is included in the crafting process for a pair of gloves, and after being used, the gloves will be made with a slight weapon boost option, just as if you received the gloves from a monster. The stimulator can be used not only on gloves but also on other items. Be sure to carry plenty of them with you as there are different types of stimulators available for different types of gloves.");
                        self.Say("But you should be aware of a few things. If the stimulator is used, it is very likely that the item choice will be changed, and the problem with this is that the result can #bend up being worse#k, way worse than the original. You will also risk a #b10% failure rate when crafting the item#k, which means saying goodbye to the items you used to craft the item in question. Sucks, right?");
                        self.Say("Even with these related risks, many travelers seek my help in creating a perfect glove using the stimulator. The thought of the item being of poor quality, getting worse than it was, or even disappearing might scare you, but why not give it a try anyway? If you're lucky, your item might be a wonder. That's all I can tell you.");
                    }
                    else if (v1 == 1)
                    {
                        var v2 = self.AskMenu("So, you want to upgrade your glove using the stimulator? Please choose the job for the glove you want to upgrade!",
                            "Upgrade a Warrior glove using the stimulator",
                            "Upgrade a Magician glove using the stimulator",
                            "Upgrade a Bowman glove using the stimulator",
                            "Upgrade a Thief glove using the stimulator"
                        );
                        jobName = Strings.Job.GetName((short)((v2 + 1) * 100));
                        ask = "Which " + jobName + " glove would you want to upgrade using the stimulator?";
                        if (v2 == 0)
                        {
                            var v3 = self.AskMenu(ask + "\r\n#L0##b #t1082005##k (Level Requirement : 30, Warrior)#l\r\n#L1##b #t1082006##k (Level Requirement : 30, Warrior)#l\r\n#L2##b #t1082035##k (Level Requirement : 35, Warrior)#l\r\n#L3##b #t1082036##k (Level Requirement : 35, Warrior)#l\r\n#L4##b #t1082024##k (Level Requirement : 40, Warrior)#l\r\n#L5##b #t1082025##k (Level Requirement : 40, Warrior)#l\r\n#L6##b #t1082010##k (Level Requirement : 50, Warrior)#l\r\n#L7##b #t1082011##k (Level Requirement : 50, Warrior)#l");
                            if (v3 == 0) chat_message4(0, "#t1082005#", "#v4130000# #t4130000#\r\n#v1082007# #t1082007#\r\n#v4011001# #t4011001#\r\n#v4031138# 20,000 mesos", 30, "STR +1");
                            else if (v3 == 1) chat_message4(1, "#t1082006#", "#v4130000# #t4130000#\r\n#v1082007# #t1082007#\r\n#v4011005# 2 #t4011005#s\r\n#v4031138# 25,000 mesos", 30, "STR +2");
                            else if (v3 == 2) chat_message4(2, "#t1082035#", "#v4130000# #t4130000#\r\n#v1082008# #t1082008#\r\n#v4021006# 3 #t4021006#s\r\n#v4031138# 30,000 mesos", 35, "STR +1");
                            else if (v3 == 3) chat_message4(3, "#t1082036#", "#v4130000# #t4130000#\r\n#v1082008# #t1082008#\r\n#v4021008# #t4021008#\r\n#v4031138# 40,000 mesos", 35, "STR +2");
                            else if (v3 == 4) chat_message4(4, "#t1082024#", "#v4130000# #t4130000#\r\n#v1082023# #t1082023#\r\n#v4011003# 4 #t4011003#s\r\n#v4031138# 45,000 mesos", 40, "STR +2");
                            else if (v3 == 5) chat_message4(5, "#t1082025#", "#v4130000# #t4130000#\r\n#v1082023# #t1082023#\r\n#v4021008# 2 #t4021008#s\r\n#v4031138# 50,000 mesos", 40, "STR +3");
                            else if (v3 == 6) chat_message4(6, "#t1082010#", "#v4130000# #t4130000#\r\n#v1082009# #t1082009#\r\n#v4011002# 5 #t4011002#s\r\n#v4031138# 55,000 mesos", 50, "STR + 2, DEX + 1");
                            else if (v3 == 7) chat_message4(7, "#t1082011#", "#v4130000# #t4130000#\r\n#v1082009# #t1082009#\r\n#v4011006# 4 #t4011006#s\r\n#v4031138# 60,000 mesos", 50, "STR + 3, Accuracy + 1");
                        }
                        else if (v2 == 1)
                        {
                            var v3 = self.AskMenu(ask+"\r\n#L0##b #t1082052##k (Level Requirement : 30, Magician)#l\r\n#L1##b #t1082053##k (Level Requirement : 30, Magician)#l\r\n#L2##b #t1082055##k (Level Requirement : 35, Magician)#l\r\n#L3##b #t1082056##k (Level Requirement : 35, Magician)#l\r\n#L4##b #t1082063##k (Level Requirement : 40, Magician)#l\r\n#L5##b #t1082064##k (Level Requirement : 40, Magician)#l\r\n#L6##b #t1082082##k (Level Requirement : 50, Magician)#l\r\n#L7##b #t1082080##k (Level Requirement : 50, Magician)#l");
                            if (v3 == 0) chat_message4(100, "#t1082052#", "#v4130000# #t4130000#\r\n#v1082051# #t1082051#\r\n#v4021005# 3 #t4021005#s\r\n#v4031138# 35,000 mesos", 30, "INT +1");
                            else if (v3 == 1) chat_message4(101, "#t1082053#", "#v4130000# #t4130000#\r\n#v1082051# #t1082051#\r\n#v4021008# #t4021008#\r\n#v4031138# 40,000 mesos", 30, "INT +2");
                            else if (v3 == 2) chat_message4(102, "#t1082055#", "#v4130000# #t4130000#\r\n#v1082054# #t1082054#\r\n#v4021005# 3 #t4021005#s\r\n#v4031138# 40,000 mesos", 35, "INT +1");
                            else if (v3 == 3) chat_message4(103, "#t1082056#", "#v4130000# #t4130000#\r\n#v1082054# #t1082054#\r\n#v4021008# #t4021008#\r\n#v4031138# 45,000 mesos", 35, "INT +2");
                            else if (v3 == 4) chat_message4(104, "#t1082063#", "#v4130000# #t4130000#\r\n#v1082062# #t1082062#\r\n#v4021002# 4 #t4021002#s\r\n#v4031138# 45,000 mesos", 40, "INT +2");
                            else if (v3 == 5) chat_message4(105, "#t1082064#", "#v4130000# #t4130000#\r\n#v1082062# #t1082062#\r\n#v4021008# 2 #t4021008#s\r\n#v4031138# 50,000 mesos", 40, "INT +3");
                            else if (v3 == 6) chat_message4(106, "#t1082082#", "#v4130000# #t4130000#\r\n#v1082081# #t1082081#\r\n#v4021002# 5 #t4021002#s\r\n#v4031138# 55,000 mesos", 50, "INT +2, MP +15");
                            else if (v3 == 7) chat_message4(107, "#t1082080#", "#v4130000# #t4130000#\r\n#v1082081# #t1082081#\r\n#v4021008# 3 #t4021008#s\r\n#v4031138# 60,000 mesos", 50, "INT +3, MP +30");
                        }
                        else if (v2 == 2)
                        {
                            var v3 = self.AskMenu(ask+"\r\n#L0##b #t1082049##k (Level Requirement : 30, Bowman)#l\r\n#L1##b #t1082050##k (Level Requirement : 30, Bowman)#l\r\n#L2##b #t1082069##k (Level Requirement : 35, Bowman)#l\r\n#L3##b #t1082070##k (Level Requirement : 35, Bowman)#l\r\n#L4##b#t1082072##k (Level Requirement : 40, Bowman)#l\r\n#L5##b#t1082073##k (Level Requirement : 40, Bowman)#l\r\n#L6##b#t1082085##k (Level Requirement : 50, Bowman)#l\r\n#L7##b#t1082083##k (Level Requirement : 50, Bowman)#l");
                            if (v3 == 0) chat_message4(200, "#t1082049#", "#v4130000# #t4130000#\r\n#v1082048# #t1082048#\r\n#v4021003# 3 #t4021003#s\r\n#v4031138# 15,000 mesos", 30, "DEX +1");
                            else if (v3 == 1) chat_message4(201, "#t1082050#", "#v4130000# #t4130000#\r\n#v1082048# #t1082048#\r\n#v4021008# #t4021008#\r\n#v4031138# 20,000 mesos", 30, "DEX +2");
                            else if (v3 == 2) chat_message4(202, "#t1082069#", "#v4130000# #t4130000#\r\n#v1082068# #t1082068#\r\n#v4011002# 4 #t4011002#s\r\n#v4031138# 22,000 mesos", 35, "DEX +1");
                            else if (v3 == 3) chat_message4(203, "#t1082070#", "#v4130000# #t4130000#\r\n#v1082068# #t1082068#\r\n#v4011006# 2 #t4011006#s\r\n#v4031138# 25,000 mesos", 35, "DEX +2");
                            else if (v3 == 4) chat_message4(204, "#t1082072#", "#v4130000# #t4130000#\r\n#v1082071# #t1082071#\r\n#v4011006# 4 #t4011006#s\r\n#v4031138# 30,000 mesos", 40, "DEX +2");
                            else if (v3 == 5) chat_message4(205, "#t1082073#", "#v4130000# #t4130000#\r\n#v1082071# #t1082071#\r\n#v4021008# 2 #t4021008#s\r\n#v4031138# 40,000 mesos", 40, "DEX +3");
                            else if (v3 == 6) chat_message4(206, "#t1082085#", "#v4130000# #t4130000#\r\n#v1082084# #t1082084#\r\n#v4021000# 5 #t4021000#s\r\n#v4011000# #t4011000#\r\n#v4031138# 55,000 mesos", 50, "DEX +2, MP +10");
                            else if (v3 == 7) chat_message4(207, "#t1082083#", "#v4130000# #t4130000#\r\n#v1082084# #t1082084#\r\n#v4021008# 2 #t4021008#s\r\n#v4011006# 2 #t4011006#s\r\n#v4031138# 60,000 mesos", 50, "DEX +3, MP +20");
                        }
                        else if (v2 == 3)
                        {
                            var v3 = self.AskMenu(ask+"\r\n#L0##b #t1082043##k (Level Requirement : 30, Thief)#l\r\n#L1##b #t1082044##k (Level Requirement : 30, Thief)#l\r\n#L2##b #t1082047##k (Level Requirement : 35, Thief)#l\r\n#L3##b #t1082045##k (Level Requirement : 35, Thief)#l\r\n#L4##b #t1082076##k (Level Requirement : 40, Thief)#l\r\n#L5##b #t1082074##k (Level Requirement : 40, Thief)#l\r\n#L6##b #t1082067##k (Level Requirement : 50, Thief)#l\r\n#L7##b #t1082066##k (Level Requirement : 50, Thief)#l");
                            if (v3 == 0) chat_message4(300, "#t1082043#", "#v4130000# #t4130000#\r\n#v1082042# #t1082042#\r\n#v4011004# 2 #t4011004#s\r\n#v4031138# 15,000 mesos", 30, "LUK +1");
                            else if (v3 == 1) chat_message4(301, "#t1082044#", "#v4130000# #t4130000#\r\n#v1082042# #t1082042#\r\n#v4011006# #t4011006#\r\n#v4031138# 20,000 mesos", 30, "LUK +2");
                            else if (v3 == 2) chat_message4(302, "#t1082047#", "#v4130000# #t4130000#\r\n#v1082046# #t1082046#\r\n#v4011005# 3 #t4011005#s\r\n#v4031138# 22,000 mesos", 35, "LUK +1");
                            else if (v3 == 3) chat_message4(303, "#t1082045#", "#v4130000# #t4130000#\r\n#v1082046# #t1082046#\r\n#v4011006# 2 #t4011006#s\r\n#v4031138# 25,000 mesos", 35, "LUK +2");
                            else if (v3 == 4) chat_message4(304, "#t1082076#", "#v4130000# #t4130000#\r\n#v1082075# #t1082075#\r\n#v4011006# 4 #t4011006#s\r\n#v4031138# 45,000 mesos", 40, "LUK +2");
                            else if (v3 == 5) chat_message4(305, "#t1082074#", "#v4130000# #t4130000#\r\n#v1082075# #t1082075#\r\n#v4021008# 2 #t4021008#s\r\n#v4031138# 50,000 mesos", 40, "LUK +3");
                            else if (v3 == 6) chat_message4(306, "#t1082067#", "#v4130000# #t4130000#\r\n#v1082065# #t1082065#\r\n#v4021000# 5 #t4021000#s\r\n#v4031138# 55,000 mesos", 50, "LUK +2, DEX +1");
                            else if (v3 == 7) chat_message4(307, "#t1082066#", "#v4130000# #t4130000#\r\n#v1082065# #t1082065#\r\n#v4011006# 2 #t4011006#s\r\n#v4021008# #t4021008#\r\n#v4031138# 60,000 mesos", 50, "LUK + 3, Avoidability + 1");
                        }
                    }
                    else if (v1 == 2)
                    {
                        var v2 = self.AskMenu(ask + "\r\n#L0##b #t1082007##k (Level Requirement : 30, Warrior)#l\r\n#L1##b #t1082008##k (Level Requirement : 35, Warrior)#l\r\n#L2##b #t1082023##k (Level Requirement : 40, Warrior)#l\r\n#L3##b #t1082009##k (Level Requirement : 50, Warrior)#l");
                        if (v2 == 0) chat_message5(0, "#t1082007#", "#v4011000# 3 #t4011000#s\r\n#v4011001# 2 #t4011001#s\r\n#v4003000# 15 #t4003000#s\r\n#v4031138# 18,000 mesos", 30);
                        else if (v2 == 1) chat_message5(1, "#t1082008#", "#v4000021# 30 #t4000021#s\r\n#v4011001#  4 #t4011001#s\r\n#v4003000# 30 #t4003000#s\r\n#v4031138# 27,000 mesos", 35);
                        else if (v2 == 2) chat_message5(2, "#t1082023#", "#v4000021# 50 #t4000021#s\r\n#v4011001# 5 #t4011001#s\r\n#v4003000# 40 #t4003000#s\r\n#v4031138# 36,000 mesos", 40);
                        else if (v2 == 3) chat_message5(3, "#t1082009#", "#v4011001# 3 #t4011001#s\r\n#v4021007# 2 #t4021007#s\r\n#v4000030# 30 #t4000030#s\r\n#v4003000# 45 #t4003000#s\r\n#v4031138# 45,000 mesos", 50);
                    }
                    else if (v1 == 3)
                    {
                        var v2 = self.AskMenu(ask + "\r\n#L0##b #t1082051##k (Level Requirement : 30, Magician)#l\r\n#L1##b #t1082054##k (Level Requirement : 35, Magician)#l\r\n#L2##b #t1082062##k (Level Requirement : 40, Magician)#l\r\n#L3##b #t1082081##k (Level Requirement : 50, Magician)#l");
                        if (v2 == 0) chat_message5(100, "#t1082051#", "#v4000021# 60 #t4000021#s\r\n#v4011006# #t4011006#\r\n#v4021000# 2 #t4021000#s\r\n#v4031138# 22,500 mesos", 30);
                        else if (v2 == 1) chat_message5(101, "#t1082054#", "#v4000021# 70 #t4000021#s\r\n#v4011006# #t4011006#\r\n#v4011001# 3 #t4011001#s\r\n#v4021000# 2 #t4021000#s\r\n#v4031138# 27,000 mesos", 35);
                        else if (v2 == 2) chat_message5(102, "#t1082062#", "#v4000021# 80 #t4000021#s\r\n#v4021000# 3 #t4021000#s\r\n#v4021006# 3 #t4021006#s\r\n#v4003000# 30 #t4003000#s\r\n#v4031138# 36,000 mesos", 40);
                        else if (v2 == 3) chat_message5(103, "#t1082081#", "#v4021000# 3 #t4021000#s\r\n#v4011006# 2 #t4011006#s\r\n#v4000030# 35 #t4000030#s\r\n#v4003000# 40 #t4003000#s\r\n#v4031138# 45,000 mesos", 50);
                    }
                    else if (v1 == 4)
                    {
                        var v2 = self.AskMenu(ask + "\r\n#L0##b #t1082048##k (Level Requirement : 30, Bowman)#l\r\n#L1##b #t1082068##k (Level Requirement : 35, Bowman)#l\r\n#L2##b #t1082071##k (Level Requirement : 40, Bowman)#l\r\n#L3##b #t1082084##k (Level Requirement : 50, Bowman)#l");
                        if (v2 == 0) chat_message5(200, "#t1082048#", "#v4000021# 50 #t4000021#s\r\n#v4021001# #t4021001#\r\n#v4011006# 2 #t4011006#s\r\n#v4031138# 18,000 mesos", 30);
                        else if (v2 == 1) chat_message5(201, "#t1082068#", "#v4000021# 60 #t4000021#s\r\n#v4011001# 3 #t4011001#s\r\n#v4011000# #t4011000#\r\n#v4003000# 15 #t4003000#s\r\n#v4031138# 27,000 mesos", 35);
                        else if (v2 == 2) chat_message5(202, "#t1082071#", "#v4000021# 80 #t4000021#s\r\n#v4021002# 3 #t4021002#s\r\n#v4011001# 3 #t4011001#s\r\n#v4021000# #t4021000#\r\n#v4003000# 25 #t4003000#s\r\n#v4031138# 36,000 mesos", 40);
                        else if (v2 == 3) chat_message5(203, "#t1082084#", "#v4011004# 3 #t4011004#s\r\n#v4021002# 2 #t4021002#s\r\n#v4011006# #t4011006#\r\n#v4000030# 40 #t4000030#s\r\n#v4003000# 35 #t4003000#s\r\n#v4031138# 45,000 mesos", 50);
                    }
                    else if (v1 == 5)
                    {
                        var v2 = self.AskMenu(ask + "\r\n#L0##b #t1082042##k (Level Requirement : 30, Thief)#l\r\n#L1##b #t1082046##k (Level Requirement : 35, Thief)#l\r\n#L2##b #t1082075##k (Level Requirement : 40, Thief)#l\r\n#L3##b #t1082065##k (Level Requirement : 50, Thief)#l");
                        if (v2 == 0) chat_message5(300, "#t1082042#", "#v4011001# 2 #t4011001#s\r\n#v4000021# 50 #t4000021#s\r\n#v4003000# 10 #t4003000#s\r\n#v4031138# 22,500 mesos", 30);
                        else if (v2 == 1) chat_message5(301, "#t1082046#", "#v4011001# 3 #t4011001#s\r\n#v4011000# #t4011000#\r\n#v4000021# 60 #t4000021#s\r\n#v4003000# 15 #t4003000#s\r\n#v4031138# 27,000 mesos", 35);
                        else if (v2 == 2) chat_message5(302, "#t1082075#", "#v4021000# 3 #t4021000#s\r\n#v4000101# 100 #t4000101#s\r\n#v4000021# 80 #t4000021#s\r\n#v4003000# 30 #t4003000#s\r\n#v4031138# 36,000 mesos", 40);
                        else if (v2 == 3) chat_message5(303, "#t1082065#", "#v4021005# 3 #t4021005#s\r\n#v4021008# #t4021008#\r\n#v4000030# 40 #t4000030#s\r\n#v4003000# 30 #t4003000#s\r\n#v4031138# 45,000 mesos", 50);
                    }
                    else if (v1 == 6)
                    {
                        var v2 = self.AskMenu(ask + "\r\n#L0##b #t1082005##k (Level Requirement : 30, Warrior)#l\r\n#L1##b #t1082006##k (Level Requirement : 30, Warrior)#l\r\n#L2##b #t1082035##k (Level Requirement : 35, Warrior)#l\r\n#L3##b #t1082036##k (Level Requirement : 35, Warrior)#l\r\n#L4##b #t1082024##k (Level Requirement : 40, Warrior)#l\r\n#L5##b #t1082025##k (Level Requirement : 40, Warrior)#l\r\n#L6##b #t1082010##k (Level Requirement : 50, Warrior)#l\r\n#L7##b #t1082011##k (Level Requirement : 50, Warrior)#l");
                        if (v2 == 0) chat_message6(0, "#t1082005#", "#v1082007# #t1082007#\r\n#v4011001# #t4011001#\r\n#v4031138# 18,000 mesos", 30, "STR +1");
                        else if (v2 == 1) chat_message6(1, "#t1082006#", "#v1082007# #t1082007#\r\n#v4011005# 2 #t4011005#s\r\n#v4031138# 22,500 mesos", 30, "STR +2");
                        else if (v2 == 2) chat_message6(2, "#t1082035#", "#v1082008# #t1082008#\r\n#v4021006# 3 #t4021006#s\r\n#v4031138# 27,000 mesos", 35, "STR +1");
                        else if (v2 == 3) chat_message6(3, "#t1082036#", "#v1082008# #t1082008#\r\n#v4021008# #t4021008#\r\n#v4031138# 36,000 mesos", 35, "STR +2");
                        else if (v2 == 4) chat_message6(4, "#t1082024#", "#v1082023# #t1082023#\r\n#v4011003# 4 #t4011003#s\r\n#v4031138# 40,500 mesos", 40, "STR +2");
                        else if (v2 == 5) chat_message6(5, "#t1082025#", "#v1082023# #t1082023#\r\n#v4021008# 2 #t4021008#s\r\n#v4031138# 45,000 mesos", 40, "STR +3");
                        else if (v2 == 6) chat_message6(6, "#t1082010#", "#v1082009# #t1082009#\r\n#v4011002# 5 #t4011002#s\r\n#v4031138# 49,500 mesos", 50, "STR + 2, DEX + 1");
                        else if (v2 == 7) chat_message6(7, "#t1082011#", "#v1082009# #t1082009#\r\n#v4011006# 4 #t4011006#s\r\n#v4031138# 54,000 mesos", 50, "STR + 3, Accuracy + 1");
                    }
                    else if (v1 == 7)
                    {
                        var v2 = self.AskMenu(ask + "\r\n#L0##b #t1082052##k (Level Requirement : 30, Magician)#l\r\n#L1##b #t1082053##k (Level Requirement : 30, Magician)#l\r\n#L2##b #t1082055##k (Level Requirement : 35, Magician)#l\r\n#L3##b #t1082056##k (Level Requirement : 35, Magician)#l\r\n#L4##b #t1082063##k (Level Requirement : 40, Magician)#l\r\n#L5##b #t1082064##k (Level Requirement : 40, Magician)#l\r\n#L6##b #t1082082##k (Level Requirement : 50, Magician)#l\r\n#L7##b #t1082080##k (Level Requirement : 50, Magician)#l");
                        if (v2 == 0) chat_message6(100, "#t1082052#", "#v1082051# #t1082051#\r\n#v4021005# 3 #t4021005#s\r\n#v4031138# 31,500 mesos", 30, "INT +1");
                        else if (v2 == 1) chat_message6(101, "#t1082053#", "#v1082051# #t1082051#\r\n#v4021008# #t4021008#\r\n#v4031138# 36,000 mesos", 30, "INT +2");
                        else if (v2 == 2) chat_message6(102, "#t1082055#", "#v1082054# #t1082054#\r\n#v4021005# 3 #t4021005#s\r\n#v4031138# 36,000 mesos", 35, "INT +1");
                        else if (v2 == 3) chat_message6(103, "#t1082056#", "#v1082054# #t1082054#\r\n#v4021008# #t4021008#\r\n#v4031138# 40,500 mesos", 35, "INT +2");
                        else if (v2 == 4) chat_message6(104, "#t1082063#", "#v1082062# #t1082062#\r\n#v4021002# 4 #t4021002#s\r\n#v4031138# 40,500 mesos", 40, "INT +2");
                        else if (v2 == 5) chat_message6(105, "#t1082064#", "#v1082062# #t1082062#\r\n#v4021008# 2 #t4021008#s\r\n#v4031138# 45,000 mesos", 40, "INT +3");
                        else if (v2 == 6) chat_message6(106, "#t1082082#", "#v1082081# #t1082081#\r\n#v4021002# 5 #t4021002#s\r\n#v4031138# 49,500 mesos", 50, "INT +2, MP +15");
                        else if (v2 == 7) chat_message6(107, "#t1082080#", "#v1082081# #t1082081#\r\n#v4021008# 3 #t4021008#s\r\n#v4031138# 54,000 mesos", 50, "INT +3, MP +30");
                    }
                    else if (v1 == 8)
                    {
                        var v2 = self.AskMenu(ask + "\r\n#L0##b #t1082049##k (Level Requirement : 30, Bowman)#l\r\n#L1##b #t1082050##k (Level Requirement : 30, Bowman)#l\r\n#L2##b #t1082069##k (Level Requirement : 35, Bowman)#l\r\n#L3##b #t1082070##k (Level Requirement : 35, Bowman)#l\r\n#L4##b #t1082072##k (Level Requirement : 40, Bowman)#l\r\n#L5##b #t1082073##k (Level Requirement : 40, Bowman)#l\r\n#L6##b #t1082085##k (Level Requirement : 50, Bowman)#l\r\n#L7##b #t1082083##k (Level Requirement : 50, Bowman)#l");
                        if (v2 == 0) chat_message6(200, "#t1082049#", "#v1082048# #t1082048#\r\n#v4021003# 3 #t4021003#s\r\n#v4031138# 13,500 mesos", 30, "DEX +1");
                        else if (v2 == 1) chat_message6(201, "#t1082050#", "#v1082048# #t1082048#\r\n#v4021008# #t4021008#\r\n#v4031138# 18,000 mesos", 30, "DEX +2");
                        else if (v2 == 2) chat_message6(202, "#t1082069#", "#v1082068# #t1082068#\r\n#v4011002# 4 #t4011002#s\r\n#v4031138# 19,800 mesos", 35, "DEX +1");
                        else if (v2 == 3) chat_message6(203, "#t1082070#", "#v1082068# #t1082068#\r\n#v4011006# 2 #t4011006#s\r\n#v4031138# 22,500 mesos", 35, "DEX +2");
                        else if (v2 == 4) chat_message6(204, "#t1082072#", "#v1082071# #t1082071#\r\n#v4011006# 4 #t4011006#s\r\n#v4031138# 27,000 mesos", 40, "DEX +2");
                        else if (v2 == 5) chat_message6(205, "#t1082073#", "#v1082071# #t1082071#\r\n#v4021008# 2 #t4021008#s\r\n#v4031138# 36,000 mesos", 40, "DEX +3");
                        else if (v2 == 6) chat_message6(206, "#t1082085#", "#v1082084# #t1082084#\r\n#v4021000# 5 #t4021000#s\r\n#v4011000# #t4011000#\r\n#v4031138# 49,500 mesos", 50, "DEX +2, MP +10");
                        else if (v2 == 7) chat_message6(207, "#t1082083#", "#v1082084# #t1082084#\r\n#v4021008# 2 #t4021008#s\r\n#v4011006# 2 #t4011006#s\r\n#v4031138# 54,000 mesos", 50, "DEX +3, MP +20");
                    }
                    else if (v1 == 9)
                    {
                        var v2 = self.AskMenu(ask + "\r\n#L0##b #t1082043##k (Level Requirement : 30, Thief)#l\r\n#L1##b #t1082044##k (Level Requirement : 30, Thief)#l\r\n#L2##b #t1082047##k (Level Requirement : 35, Thief)#l\r\n#L3##b #t1082045##k (Level Requirement : 35, Thief)#l\r\n#L4##b #t1082076##k (Level Requirement : 40, Thief)#l\r\n#L5##b #t1082074##k (Level Requirement : 40, Thief)#l\r\n#L6##b #t1082067##k (Level Requirement : 50, Thief)#l\r\n#L7##b #t1082066##k (Level Requirement : 50, Thief)#l");
                        if (v2 == 0) chat_message6(300, "#t1082043#", "#v1082042# #t1082042#\r\n#v4011004# 2 #t4011004#s\r\n#v4031138# 13,500 mesos", 30, "LUK +1");
                        else if (v2 == 1) chat_message6(301, "#t1082044#", "#v1082042# #t1082042#\r\n#v4011006# #t4011006#\r\n#v4031138# 17,000 mesos", 30, "LUK +2");
                        else if (v2 == 2) chat_message6(302, "#t1082047#", "#v1082046# #t1082046#\r\n#v4011005# 3 #t4011005#s\r\n#v4031138# 19,800 mesos", 35, "LUK +1");
                        else if (v2 == 3) chat_message6(303, "#t1082045#", "#v1082046# #t1082046#\r\n#v4011006# 2 #t4011006#s\r\n#v4031138# 22,500 mesos", 35, "LUK +2");
                        else if (v2 == 4) chat_message6(304, "#t1082076#", "#v1082075# #t1082075#\r\n#v4011006# 4 #t4011006#s\r\n#v4031138# 40,500 mesos", 40, "LUK +2");
                        else if (v2 == 5) chat_message6(305, "#t1082074#", "#v1082075# #t1082075#\r\n#v4021008# 2 #t4021008#s\r\n#v4031138# 45,000 mesos", 40, "LUK +3");
                        else if (v2 == 6) chat_message6(306, "#t1082067#", "#v1082065# #t1082065#\r\n#v4021000# 5 #t4021000#s\r\n#v4031138# 49,500 mesos", 50, "LUK +2, DEX +1");
                        else if (v2 == 7) chat_message6(307, "#t1082066#", "#v1082065# #t1082065#\r\n#v4011006# 2 #t4011006#s\r\n#v4021008# #t4021008#\r\n#v4031138# 54,000 mesos", 50, "LUK + 3, Avoidability + 1");
                    }
                }
            }
        }
    }
}
