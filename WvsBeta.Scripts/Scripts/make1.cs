using System;
using WvsBeta.Common.Extensions;
using WvsBeta.Game;
using WvsBeta.Game.Scripting;

namespace WvsBeta.Scripts.Scripts
{
    public static class make1
    {
        static dynamic nAllNum, ret, nRetBuy, nRet, inventory, nNeedNum;
        static int nRetNum;
        static int nPrice;
        static INpcHost self;
        static GameCharacter target;
        public static void chat_message5(int index, string makeItem, string needItem, int reqLevel, string itemOption)
        {
            inventory = target.Inventory;

            nRet = self.AskYesNo("To make " + makeItem + ", I'm going to need the following materials. There is also a #r" + itemOption + "#k upgrade, and the Level Requirement is: " + reqLevel + ". What do you think? Want to make one? \r\n\r\n#b" + needItem);
            if (nRet == 0) self.Say("It's not just minerals and jewels I can refine, but also rare and valuable shoes... Think about it and have a look around.");
            else
            {
                // Warrior Shoes
                if (index == 1) ret = inventory.Exchange(-10000, 4011004, -2, 4011001, -1, 4000021, -15, 4003000, -10, 1072051, 1);
                else if (index == 2) ret = inventory.Exchange(-10000, 4011006, -2, 4011001, -1, 4000021, -15, 4003000, -10, 1072053, 1);
                else if (index == 3) ret = inventory.Exchange(-12000, 4021008, -1, 4011001, -2, 4000021, -20, 4003000, -10, 1072052, 1);
                else if (index == 4) ret = inventory.Exchange(-20000, 4021003, -4, 4011001, -2, 4000021, -45, 4003000, -15, 1072003, 1);
                else if (index == 5) ret = inventory.Exchange(-20000, 4011002, -4, 4011001, -2, 4000021, -45, 4003000, -15, 1072039, 1);
                else if (index == 6) ret = inventory.Exchange(-20000, 4011004, -4, 4011001, -2, 4000021, -45, 4003000, -15, 1072040, 1);
                else if (index == 7) ret = inventory.Exchange(-20000, 4021000, -4, 4011001, -2, 4000021, -45, 4003000, -15, 1072041, 1);
                else if (index == 8) ret = inventory.Exchange(-22000, 4011001, -3, 4021004, -1, 4000021, -30, 4000030, -20, 4003000, -25, 1072002, 1);
                else if (index == 9) ret = inventory.Exchange(-22000, 4011002, -3, 4021004, -1, 4000021, -30, 4000030, -20, 4003000, -25, 1072112, 1);
                else if (index == 10) ret = inventory.Exchange(-25000, 4021008, -2, 4021004, -1, 4000021, -30, 4000030, -20, 4003000, -25, 1072113, 1);
                else if (index == 11) ret = inventory.Exchange(-38000, 4011003, -4, 4000021, -100, 4000030, -40, 4003000, -30, 4000033, -100, 1072000, 1);
                else if (index == 12) ret = inventory.Exchange(-38000, 4011005, -4, 4021007, -1, 4000030, -40, 4003000, -30, 4000042, -250, 1072126, 1);
                else if (index == 13) ret = inventory.Exchange(-38000, 4011002, -4, 4021007, -1, 4000030, -40, 4003000, -30, 4000041, -120, 1072127, 1);
                else if (index == 14) ret = inventory.Exchange(-50000, 4021008, -1, 4011001, -3, 4021003, -6, 4000030, -65, 4003000, -45, 1072132, 1);
                else if (index == 15) ret = inventory.Exchange(-50000, 4021008, -1, 4011001, -3, 4011002, -6, 4000030, -65, 4003000, -45, 1072133, 1);
                else if (index == 16) ret = inventory.Exchange(-50000, 4021008, -1, 4011001, -3, 4011005, -6, 4000030, -65, 4003000, -45, 1072134, 1);
                else if (index == 17) ret = inventory.Exchange(-50000, 4021008, -1, 4011001, -3, 4011006, -6, 4000030, -65, 4003000, -45, 1072135, 1);
                else if (index == 18) ret = inventory.Exchange(-60000, 4021008, -1, 4011007, -1, 4021005, -8, 4000030, -80, 4003000, -55, 1072147, 1);
                else if (index == 19) ret = inventory.Exchange(-60000, 4021008, -1, 4011007, -1, 4011005, -8, 4000030, -80, 4003000, -55, 1072148, 1);
                else if (index == 20) ret = inventory.Exchange(-60000, 4021008, -1, 4011007, -1, 4021000, -8, 4000030, -80, 4003000, -55, 1072149, 1);
                // Magician Shoes
                else if (index == 100) ret = inventory.Exchange(-3000, 4021005, -1, 4000021, -30, 4003000, -5, 1072019, 1);
                else if (index == 101) ret = inventory.Exchange(-3000, 4021001, -1, 4000021, -30, 4003000, -5, 1072020, 1);
                else if (index == 102) ret = inventory.Exchange(-3000, 4021000, -1, 4000021, -30, 4003000, -5, 1072021, 1);
                else if (index == 103) ret = inventory.Exchange(-8000, 4011004, -1, 4000021, -35, 4003000, -10, 1072072, 1);
                else if (index == 104) ret = inventory.Exchange(-8000, 4021006, -1, 4000021, -35, 4003000, -10, 1072073, 1);
                else if (index == 105) ret = inventory.Exchange(-8000, 4021004, -1, 4000021, -35, 4003000, -10, 1072074, 1);
                else if (index == 106) ret = inventory.Exchange(-18000, 4021000, -2, 4000021, -50, 4003000, -15, 1072075, 1);
                else if (index == 107) ret = inventory.Exchange(-18000, 4021002, -2, 4000021, -50, 4003000, -15, 1072076, 1);
                else if (index == 108) ret = inventory.Exchange(-18000, 4011004, -2, 4000021, -50, 4003000, -15, 1072077, 1);
                else if (index == 109) ret = inventory.Exchange(-18000, 4021008, -1, 4000021, -50, 4003000, -15, 1072078, 1);
                else if (index == 110) ret = inventory.Exchange(-20000, 4021001, -3, 4021006, -1, 4000021, -30, 4000030, -15, 4003000, -20, 1072089, 1);
                else if (index == 111) ret = inventory.Exchange(-20000, 4021000, -3, 4021006, -1, 4000021, -30, 4000030, -15, 4003000, -20, 1072090, 1);
                else if (index == 112) ret = inventory.Exchange(-22000, 4021008, -2, 4021006, -1, 4000021, -40, 4000030, -25, 4003000, -20, 1072091, 1);
                else if (index == 113) ret = inventory.Exchange(-30000, 4021000, -4, 4000030, -40, 4000043, -35, 4003000, -25, 1072114, 1);
                else if (index == 114) ret = inventory.Exchange(-30000, 4021005, -4, 4000030, -40, 4000037, -70, 4003000, -25, 1072115, 1);
                else if (index == 115) ret = inventory.Exchange(-35000, 4011006, -2, 4021007, -1, 4000030, -40, 4000027, -20, 4003000, -25, 1072116, 1);
                else if (index == 116) ret = inventory.Exchange(-40000, 4021008, -2, 4021007, -1, 4000030, -40, 4000014, -30, 4003000, -30, 1072117, 1);
                else if (index == 117) ret = inventory.Exchange(-50000, 4021009, -1, 4011006, -3, 4021000, -3, 4000030, -60, 4003000, -40, 1072140, 1);
                else if (index == 118) ret = inventory.Exchange(-50000, 4021009, -1, 4011006, -3, 4021005, -3, 4000030, -60, 4003000, -40, 1072141, 1);
                else if (index == 119) ret = inventory.Exchange(-50000, 4021009, -1, 4011006, -3, 4021001, -3, 4000030, -60, 4003000, -40, 1072142, 1);
                else if (index == 120) ret = inventory.Exchange(-50000, 4021009, -1, 4011006, -3, 4021003, -3, 4000030, -60, 4003000, -40, 1072143, 1);
                else if (index == 121) ret = inventory.Exchange(-60000, 4021009, -1, 4011006, -4, 4011005, -5, 4000030, -70, 4003000, -50, 1072136, 1);
                else if (index == 122) ret = inventory.Exchange(-60000, 4021009, -1, 4011006, -4, 4021003, -5, 4000030, -70, 4003000, -50, 1072137, 1);
                else if (index == 123) ret = inventory.Exchange(-60000, 4021009, -1, 4011006, -4, 4011003, -5, 4000030, -70, 4003000, -50, 1072138, 1);
                else if (index == 124) ret = inventory.Exchange(-60000, 4021009, -1, 4011006, -4, 4021002, -5, 4000030, -70, 4003000, -50, 1072139, 1);
                // Bowman Shoes
                else if (index == 200) ret = inventory.Exchange(-9000, 4011000, -3, 4000021, -35, 4003000, -10, 1072027, 1);
                else if (index == 201) ret = inventory.Exchange(-9000, 4021003, -1, 4000021, -35, 4003000, -10, 1072034, 1);
                else if (index == 202) ret = inventory.Exchange(-9000, 4021000, -1, 4000021, -35, 4003000, -10, 1072069, 1);
                else if (index == 203) ret = inventory.Exchange(-19000, 4021000, -2, 4000021, -50, 4003000, -15, 1072079, 1);
                else if (index == 204) ret = inventory.Exchange(-19000, 4021005, -2, 4000021, -50, 4003000, -15, 1072080, 1);
                else if (index == 205) ret = inventory.Exchange(-19000, 4021003, -2, 4000021, -50, 4003000, -15, 1072081, 1);
                else if (index == 206) ret = inventory.Exchange(-19000, 4021004, -2, 4000021, -50, 4003000, -15, 1072082, 1);
                else if (index == 207) ret = inventory.Exchange(-19000, 4021006, -2, 4000021, -50, 4003000, -15, 1072083, 1);
                else if (index == 208) ret = inventory.Exchange(-20000, 4021002, -3, 4021006, -1, 4000021, -30, 4000030, -15, 4003000, -20, 1072101, 1);
                else if (index == 209) ret = inventory.Exchange(-20000, 4021003, -3, 4021006, -1, 4000021, -30, 4000030, -15, 4003000, -20, 1072102, 1);
                else if (index == 210) ret = inventory.Exchange(-20000, 4021000, -3, 4021006, -1, 4000021, -30, 4000030, -15, 4003000, -20, 1072103, 1);
                else if (index == 211) ret = inventory.Exchange(-32000, 4021000, -4, 4000030, -45, 4000024, -40, 4003000, -30, 1072118, 1);
                else if (index == 212) ret = inventory.Exchange(-32000, 4021006, -4, 4000030, -45, 4000027, -20, 4003000, -30, 1072119, 1);
                else if (index == 213) ret = inventory.Exchange(-40000, 4011003, -5, 4000030, -45, 4000044, -40, 4003000, -30, 1072120, 1);
                else if (index == 214) ret = inventory.Exchange(-40000, 4021002, -5, 4000030, -45, 4000009, -120, 4003000, -30, 1072121, 1);
                else if (index == 215) ret = inventory.Exchange(-50000, 4021008, -1, 4011001, -3, 4021006, -3, 4000030, -60, 4000033, -80, 4003000, -35, 1072122, 1);
                else if (index == 216) ret = inventory.Exchange(-50000, 4021008, -1, 4011001, -3, 4021006, -3, 4000030, -60, 4000032, -150, 4003000, -35, 1072123, 1);
                else if (index == 217) ret = inventory.Exchange(-50000, 4021008, -1, 4011001, -3, 4021006, -3, 4000030, -60, 4000041, -100, 4003000, -35, 1072124, 1);
                else if (index == 218) ret = inventory.Exchange(-50000, 4021008, -1, 4011001, -3, 4021006, -3, 4000030, -60, 4000042, -250, 4003000, -35, 1072125, 1);
                else if (index == 219) ret = inventory.Exchange(-60000, 4021007, -1, 4011006, -5, 4021000, -8, 4000030, -75, 4003000, -50, 1072144, 1);
                else if (index == 220) ret = inventory.Exchange(-60000, 4021007, -1, 4011006, -5, 4021005, -8, 4000030, -75, 4003000, -50, 1072145, 1);
                else if (index == 221) ret = inventory.Exchange(-60000, 4021007, -1, 4011006, -5, 4021003, -8, 4000030, -75, 4003000, -50, 1072146, 1);
                // Thief Shoes
                else if (index == 300) ret = inventory.Exchange(-9000, 4021005, -1, 4000021, -35, 4003000, -10, 1072084, 1);
                else if (index == 301) ret = inventory.Exchange(-9000, 4021000, -1, 4000021, -35, 4003000, -10, 1072085, 1);
                else if (index == 302) ret = inventory.Exchange(-9000, 4021003, -1, 4000021, -35, 4003000, -10, 1072086, 1);
                else if (index == 303) ret = inventory.Exchange(-9000, 4021004, -1, 4000021, -35, 4003000, -10, 1072087, 1);
                else if (index == 304) ret = inventory.Exchange(-19000, 4011000, -3, 4000021, -50, 4003000, -15, 1072032, 1);
                else if (index == 305) ret = inventory.Exchange(-19000, 4011001, -3, 4000021, -50, 4003000, -15, 1072033, 1);
                else if (index == 306) ret = inventory.Exchange(-19000, 4011004, -2, 4000021, -50, 4003000, -15, 1072035, 1);
                else if (index == 307) ret = inventory.Exchange(-21000, 4011006, -2, 4000021, -50, 4003000, -15, 1072036, 1);
                else if (index == 308) ret = inventory.Exchange(-20000, 4021000, -3, 4021004, -1, 4000021, -30, 4000030, -15, 4003000, -20, 1072104, 1);
                else if (index == 309) ret = inventory.Exchange(-20000, 4021003, -3, 4021004, -1, 4000021, -30, 4000030, -15, 4003000, -20, 1072105, 1);
                else if (index == 310) ret = inventory.Exchange(-20000, 4021002, -3, 4021004, -1, 4000021, -30, 4000030, -15, 4003000, -20, 1072106, 1);
                else if (index == 311) ret = inventory.Exchange(-32000, 4021003, -4, 4000030, -45, 4000032, -30, 4003000, -30, 1072108, 1);
                else if (index == 312) ret = inventory.Exchange(-35000, 4021006, -4, 4000030, -45, 4000040, -3, 4003000, -30, 1072109, 1);
                else if (index == 313) ret = inventory.Exchange(-35000, 4021005, -4, 4000030, -45, 4000037, -70, 4003000, -30, 1072110, 1);
                else if (index == 314) ret = inventory.Exchange(-40000, 4021000, -5, 4000030, -45, 4000033, -50, 4003000, -30, 1072107, 1);
                else if (index == 315) ret = inventory.Exchange(-50000, 4011007, -2, 4021005, -3, 4000030, -50, 4000037, -200, 4003000, -35, 1072128, 1);
                else if (index == 316) ret = inventory.Exchange(-50000, 4011007, -2, 4021003, -3, 4000030, -50, 4000045, -80, 4003000, -35, 1072129, 1);
                else if (index == 317) ret = inventory.Exchange(-50000, 4011007, -2, 4021000, -3, 4000030, -50, 4000043, -150, 4003000, -35, 1072130, 1);
                else if (index == 318) ret = inventory.Exchange(-50000, 4011007, -2, 4021001, -3, 4000030, -50, 4000036, -80, 4003000, -35, 1072131, 1);
                else if (index == 319) ret = inventory.Exchange(-60000, 4021007, -1, 4011007, -1, 4021000, -8, 4000030, -75, 4003000, -50, 1072150, 1);
                else if (index == 320) ret = inventory.Exchange(-60000, 4021007, -1, 4011007, -1, 4011006, -5, 4000030, -75, 4003000, -50, 1072151, 1);
                else if (index == 321) ret = inventory.Exchange(-60000, 4021007, -1, 4011007, -1, 4021008, -1, 4000030, -75, 4003000, -50, 1072152, 1);

                if (ret == 0) self.Say("Please check carefully that you have all the required items, and whether your equipment inventory is full or not.");
                else self.Say("There you go. Here, take the " + makeItem + " that you requested. What do you think? You won't find a product like this anywhere else...");
            }
        }
        public static void chat_message6(int index, string makeItem, string needItemIcon, string needItemString, int unitPrice)
        {
            inventory = target.Inventory;

            nRetNum = self.AskNumber("To make " + makeItem + ", I need the following items... How many would you like to make?\r\n\r\n#b" + needItemIcon + " 10 " + needItemString + "\r\n" + unitPrice.Culture() + " mesos#k", 1, 1, 100);
            nPrice = unitPrice * nRetNum;
            nAllNum = nRetNum * 10;
            nRetBuy = self.AskYesNo("To make #b" + nRetNum + " " + makeItem + "#k, I need the following materials. What do you think? Would you like to create one?\r\n\r\n#b" + needItemIcon + " " + nAllNum + " " + needItemString + "\r\n" + nPrice.Culture() + " mesos#k");
            if (nRetBuy == 0) self.Say("It's not just minerals and jewels I can refine, but also rare and valuable shoes... Think about it and have a look around.");
            else
            {
                // Ores
                if (index == 1) ret = inventory.Exchange(-nPrice, 4010000, -nAllNum, 4011000, nRetNum);
                else if (index == 2) ret = inventory.Exchange(-nPrice, 4010001, -nAllNum, 4011001, nRetNum);
                else if (index == 3) ret = inventory.Exchange(-nPrice, 4010002, -nAllNum, 4011002, nRetNum);
                else if (index == 4) ret = inventory.Exchange(-nPrice, 4010003, -nAllNum, 4011003, nRetNum);
                else if (index == 5) ret = inventory.Exchange(-nPrice, 4010004, -nAllNum, 4011004, nRetNum);
                else if (index == 6) ret = inventory.Exchange(-nPrice, 4010005, -nAllNum, 4011005, nRetNum);
                else if (index == 7) ret = inventory.Exchange(-nPrice, 4010006, -nAllNum, 4011006, nRetNum);
                // jewel
                else if (index == 100) ret = inventory.Exchange(-nPrice, 4020000, -nAllNum, 4021000, nRetNum);
                else if (index == 101) ret = inventory.Exchange(-nPrice, 4020001, -nAllNum, 4021001, nRetNum);
                else if (index == 102) ret = inventory.Exchange(-nPrice, 4020002, -nAllNum, 4021002, nRetNum);
                else if (index == 103) ret = inventory.Exchange(-nPrice, 4020003, -nAllNum, 4021003, nRetNum);
                else if (index == 104) ret = inventory.Exchange(-nPrice, 4020004, -nAllNum, 4021004, nRetNum);
                else if (index == 105) ret = inventory.Exchange(-nPrice, 4020005, -nAllNum, 4021005, nRetNum);
                else if (index == 106) ret = inventory.Exchange(-nPrice, 4020006, -nAllNum, 4021006, nRetNum);
                else if (index == 107) ret = inventory.Exchange(-nPrice, 4020007, -nAllNum, 4021007, nRetNum);
                else if (index == 108) ret = inventory.Exchange(-nPrice, 4020008, -nAllNum, 4021008, nRetNum);

                if (ret == 0) self.Say("Please check if you have " + needItemIcon + needItemString + ", and whether your etc inventory is full or not.");
                else self.Say("Here, take " + nRetNum + " " + makeItem + "(s). What do you think? Pretty refined, huh? Hahaha... Finally, all those days spent studying refining skills paid off. Please come back another time!");
            }
        }
        public static void chat_message4(int index, string makeItem, string needItem, int unitPrice)
        {
            inventory = target.Inventory;

            nRetNum = self.AskNumber("To make " + makeItem + ", I need the following items... How many would you like to make?\r\n\r\n#b" + needItem + " \r\n#v4031138# " + unitPrice.Culture() + " mesos#k", 1, 1, 100);
            nPrice = unitPrice * nRetNum;
            nAllNum = nRetNum * 10;
            nRetBuy = self.AskYesNo("To make #b" + nRetNum + " " + makeItem + "#k, I need the following materials. What do you think? Want to make\r\n\r\n#b" + nAllNum + " " + needItem + "\r\n#v4031138# " + nPrice.Culture() + " mesos#k");
            if (nRetBuy == 0) self.Say("It's not just minerals and jewels I can refine, but also rare and valuable shoes... Think about it and have a look around.");
            else
            {
                // Ores
                if (index == 1) ret = inventory.Exchange(-nPrice, 4010000, -nAllNum, 4011000, nRetNum);
                else if (index == 2) ret = inventory.Exchange(-nPrice, 4010001, -nAllNum, 4011001, nRetNum);
                else if (index == 3) ret = inventory.Exchange(-nPrice, 4010002, -nAllNum, 4011002, nRetNum);
                else if (index == 4) ret = inventory.Exchange(-nPrice, 4010003, -nAllNum, 4011003, nRetNum);
                else if (index == 5) ret = inventory.Exchange(-nPrice, 4010004, -nAllNum, 4011004, nRetNum);
                else if (index == 6) ret = inventory.Exchange(-nPrice, 4010005, -nAllNum, 4011005, nRetNum);
                else if (index == 7) ret = inventory.Exchange(-nPrice, 4010006, -nAllNum, 4011006, nRetNum);
                // Jewels
                else if (index == 100) ret = inventory.Exchange(-nPrice, 4020000, -nAllNum, 4021000, nRetNum);
                else if (index == 101) ret = inventory.Exchange(-nPrice, 4020001, -nAllNum, 4021001, nRetNum);
                else if (index == 102) ret = inventory.Exchange(-nPrice, 4020002, -nAllNum, 4021002, nRetNum);
                else if (index == 103) ret = inventory.Exchange(-nPrice, 4020003, -nAllNum, 4021003, nRetNum);
                else if (index == 104) ret = inventory.Exchange(-nPrice, 4020004, -nAllNum, 4021004, nRetNum);
                else if (index == 105) ret = inventory.Exchange(-nPrice, 4020005, -nAllNum, 4021005, nRetNum);
                else if (index == 106) ret = inventory.Exchange(-nPrice, 4020006, -nAllNum, 4021006, nRetNum);
                else if (index == 107) ret = inventory.Exchange(-nPrice, 4020007, -nAllNum, 4021007, nRetNum);
                else if (index == 108) ret = inventory.Exchange(-nPrice, 4020008, -nAllNum, 4021008, nRetNum);

                if (ret == 0) self.Say("Please check carefully that you have all the items you need, and whether your etc inventory is full or not.");
                else self.Say("There you go. Take " + nRetNum + " " + makeItem + "(s). What do you think? You won't find a product like this anywhere else...");
            }
        }
        public static void chat_message6(int index, string makeItem, string needItem, int unitNum, string itemOption)
        {
            inventory = target.Inventory;

            if (itemOption == "") nRet = self.AskYesNo("To make #b" + unitNum + " " + makeItem + "#k(s), I need the following items. If you have the materials, it will be much better for you to craft the item here than to buy it in the store. Do you really want it?\r\n\r\n#b" + needItem);
            else nRet = self.AskYesNo("To make #b" + unitNum + " " + makeItem + "#k(s), I need the following items. By the way, this arrow also has a #b" + itemOption + "#k improvement on it. It's a special kind of arrow, so if you have the necessary materials, why not craft some? Do you really want me to do it?\r\n\r\n#b" + needItem);

            if (nRet == 0) self.Say("I make important items for bowmen and sell them at a reasonable price, so feel free to check them all out. You know nothing is free, right?");
            else
            {
                // Making an arrow
                if (index == 1) ret = inventory.Exchange(0, 4003001, -1, 4003004, -1, 2060000, 1000);
                else if (index == 2) ret = inventory.Exchange(0, 4003001, -1, 4003004, -1, 2061000, 1000);
                else if (index == 3) ret = inventory.Exchange(0, 4011000, -1, 4003001, -3, 4003004, -10, 2060001, 900);
                else if (index == 4) ret = inventory.Exchange(0, 4011000, -1, 4003001, -3, 4003004, -10, 2061001, 900);
                if (ret == 0) self.Say("Please check carefully that you have all the items you need, and whether your use inventory is full or not.");
                else self.Say("Hey! Here, take " + unitNum + " " + makeItem + ". It took a little extra work for me, but... it turned out to be a wonderful thing. Please come talk to me if you need anything later. Until that day comes and see you later...");
            }
        }
        public static void chat_message3(int index, string makeItem, string needItem, int needNumber, int itemNumber)
        {
            inventory = target.Inventory;

            nRetNum = self.AskNumber("With #b" + needNumber + " " + needItem + "#k, I can create " + itemNumber + " " + makeItem + "(s). Bring me the materials and I can make them for you for free. Now... how many would you like me to make?", 1, 1, 100);
            nNeedNum = nRetNum * needNumber;
            nAllNum = nRetNum * itemNumber;
            nRetBuy = self.AskYesNo("Do you want to make #b" + makeItem + "#k " + nRetNum + "times? I need #r" + nNeedNum + " " + needItem + "#k.");
            if (nRetBuy == 0) self.Say("Oh, really? You don't have the materials? Please come back later. Looks like I'm going to be here for a while.");
            else
            {
                if (index == 1) ret = inventory.Exchange(0, 4000003, -nNeedNum, 4003001, nAllNum);
                else if (index == 2) ret = inventory.Exchange(0, 4000018, -nNeedNum, 4003001, nAllNum);
                else if (index == 3) ret = inventory.Exchange(0, 4011001, -nNeedNum, 4011000, -nNeedNum, 4003000, nAllNum);
                if (ret == 0) self.Say("Please check carefully that you have all the items you need, and whether your etc inventory is full or not.");
                else self.Say("Hey! Here, take the " + nAllNum + " " + makeItem + "(s). It took a little extra work for me, but... it turned out to be a wonderful thing. Please come talk to me if you need anything later. Until that day comes and see you later...");
            }
        }
        public static void chat_message2(int index, string makeItem, string needItem, int reqLevel)
        {
            inventory = target.Inventory;

            nRet = self.AskYesNo("To perfect a " + makeItem + ", I'll need the following materials. The Level Requirement is: " + reqLevel + ", and please make sure you are not going to use an item that is being upgraded as a material. What do you think? Do you want one?\r\n\r\n#b" + needItem);
            if (nRet == 0) self.Say("Oh, really? You don't have the materials? Please come back later. Looks like I'm going to be here for a while.");
            else
            {
                // Upgrading a glove
                if (index == 1) ret = inventory.Exchange(-5000, 1082013, -1, 4021000, -1, 1082014, 1);
                else if (index == 2) ret = inventory.Exchange(-7000, 1082013, -1, 4021003, -2, 1082015, 1);
                else if (index == 3) ret = inventory.Exchange(-10000, 1082016, -1, 4021000, -3, 1082017, 1);
                else if (index == 4) ret = inventory.Exchange(-12000, 1082016, -1, 4021008, -1, 1082018, 1);
                else if (index == 5) ret = inventory.Exchange(-15000, 1082048, -1, 4021003, -3, 1082049, 1);
                else if (index == 6) ret = inventory.Exchange(-20000, 1082048, -1, 4021008, -1, 1082050, 1);
                else if (index == 7) ret = inventory.Exchange(-22000, 1082068, -1, 4011002, -4, 1082069, 1);
                else if (index == 8) ret = inventory.Exchange(-25000, 1082068, -1, 4011006, -2, 1082070, 1);
                else if (index == 9) ret = inventory.Exchange(-30000, 1082071, -1, 4011006, -4, 1082072, 1);
                else if (index == 10) ret = inventory.Exchange(-40000, 1082071, -1, 4021008, -2, 1082073, 1);
                else if (index == 11) ret = inventory.Exchange(-55000, 1082084, -1, 4021000, -5, 4011000, -1, 1082085, 1);
                else if (index == 12) ret = inventory.Exchange(-60000, 1082084, -1, 4021008, -2, 4011006, -2, 1082083, 1);
                else if (index == 13) ret = inventory.Exchange(-70000, 1082089, -1, 4021007, -1, 4021000, -5, 1082090, 1);
                else if (index == 14) ret = inventory.Exchange(-80000, 1082089, -1, 4021007, -2, 4021008, -2, 1082091, 1);

                if (ret == 0) self.Say("Please check carefully that you have all the items you need, and whether your etc inventory is full or not.");
                else self.Say("Hey! Here, take the " + makeItem + ". It took a little extra work for me, but... it turned out to be a wonderful thing. Please come talk to me if you need anything later. Until that day comes and see you later...");
            }
        }
        public static void chat_message1(int index, string makeItem, string needItem, int reqLevel)
        {
            inventory = target.Inventory;

            nRet = self.AskYesNo("To make " + makeItem + ", I'll need the following materials. The Level Requirement is: " + reqLevel + ", and please make sure you are not going to use an item that is being upgraded as a material. What do you think? Do you want one?\r\n\r\n#b" + needItem);
            if (nRet == 0) self.Say("Oh, really? You don't have the materials? Please come back later. Looks like I'm going to be here for a while.");
            else
            {
                // Making a bow
                if (index == 1) ret = inventory.Exchange(-800, 4003001, -5, 4000000, -30, 1452002, 1);
                else if (index == 2) ret = inventory.Exchange(-2000, 4011001, -1, 4003000, -3, 1452003, 1);
                else if (index == 3) ret = inventory.Exchange(-3000, 4003001, -30, 4000016, -50, 1452001, 1);
                else if (index == 4) ret = inventory.Exchange(-5000, 4011001, -2, 4021006, -2, 4003000, -8, 1452000, 1);
                else if (index == 5) ret = inventory.Exchange(-30000, 4011001, -5, 4011006, -5, 4021003, -3, 4021006, -2, 4003000, -30, 1452005, 1);
                else if (index == 6) ret = inventory.Exchange(-40000, 4011004, -7, 4021000, -6, 4021004, -3, 4003000, -35, 1452006, 1);
                else if (index == 7) ret = inventory.Exchange(-80000, 4021008, -1, 4011001, -10, 4011006, -3, 4000014, -50, 4003000, -40, 1452007, 1);
                // Making a crossbow
                else if (index == 100) ret = inventory.Exchange(-1000, 4003001, -7, 4003000, -2, 1462001, 1);
                else if (index == 101) ret = inventory.Exchange(-2000, 4003001, -20, 4011001, -1, 4003000, -5, 1462002, 1);
                else if (index == 102) ret = inventory.Exchange(-3000, 4003001, -50, 4011001, -1, 4003000, -8, 1462003, 1);
                else if (index == 103) ret = inventory.Exchange(-10000, 4011001, -2, 4021006, -1, 4021002, -1, 4003000, -10, 1462000, 1);
                else if (index == 104) ret = inventory.Exchange(-30000, 4011001, -5, 4011005, -5, 4021006, -3, 4003001, -50, 4003000, -15, 1462004, 1);
                else if (index == 105) ret = inventory.Exchange(-50000, 4021008, -1, 4011001, -8, 4011006, -4, 4021006, -2, 4003000, -20, 1462005, 1);
                else if (index == 106) ret = inventory.Exchange(-80000, 4021008, -2, 4011004, -6, 4003001, -30, 4003000, -30, 1462006, 1);
                else if (index == 107) ret = inventory.Exchange(-100000, 4021008, -2, 4011006, -5, 4021006, -3, 4003001, -40, 4003000, -40, 1462007, 1);
                // Making a glove
                else if (index == 200) ret = inventory.Exchange(-5000, 4000021, -15, 4000009, -20, 1082012, 1);
                else if (index == 201) ret = inventory.Exchange(-10000, 4000021, -20, 4000009, -20, 4011001, -2, 1082013, 1);
                else if (index == 202) ret = inventory.Exchange(-15000, 4000021, -40, 4000009, -50, 4011006, -2, 1082016, 1);
                else if (index == 203) ret = inventory.Exchange(-20000, 4000021, -50, 4021001, -1, 4011006, -2, 1082048, 1);
                else if (index == 204) ret = inventory.Exchange(-30000, 4000021, -60, 4011001, -3, 4011000, -1, 4003000, -15, 1082068, 1);
                else if (index == 205) ret = inventory.Exchange(-40000, 4000021, -80, 4021002, -3, 4011001, -3, 4021000, -1, 4003000, -25, 1082071, 1);
                else if (index == 206) ret = inventory.Exchange(-50000, 4011004, -3, 4021002, -2, 4011006, -1, 4000030, -40, 4003000, -35, 1082084, 1);
                else if (index == 207) ret = inventory.Exchange(-70000, 4011007, -1, 4021006, -8, 4011006, -2, 4000030, -50, 4003000, -50, 1082089, 1);

                if (ret == 0) self.Say("Please check carefully that you have all the items you need, and whether your equipment inventory is full or not.");
                else self.Say("Hey! Here, take " + makeItem + ". It took a little extra work for me, but... it turned out to be a wonderful thing. Please come talk to me if you need anything later. Until that day comes and see you later...");
            }
        }
        // creating and upgrading items at Henesys
        [Script("refine_henesys")]
        class refine_henesys : INpcScript
        {
            dynamic nAllNum, v1, ret, nRet1, nRetNum, nRetBuy, nRet, v2, inventory, nNeedNum;
            public void Run(INpcHost self, GameCharacter target)
            {
                make1.self = self;
                make1.target = target;
                nRet1 = self.AskYesNo("Hey you~ Do you need anything? Well, nothing is free around here, but... with a few materials and a small service fee, I can create some items for you. So what do you think? Want to make a good deal? Just don't let anyone in this town know about it.");
                if (nRet1 == 0) self.Say("You don't seem very interested. If you need anything later, just come back here. I can even get you items you can't find in stores.");
                else
                {
                    v1 = self.AskMenu("Great~! What can I do for you? Just tell me what you need! \r\n#b#L0# Create a bow#l\r\n#L1# Create a Crossbow#l\r\n#L2# Create a glove#l\r\n#L3# Upgrade a Glove#l \r\n#L4# Create Materials#l\r\n#L5# Create Arrows#l");
                    if (v1 == 0)
                    {
                        v2 = self.AskMenu("I'll make a bow if you can get me some materials. What kind of bow do you want?\r\n#L0##b #t1452002##k (Level Requirement: 10, Bowman)#l\r\n#L1##b #t1452003##k (Level Requirement: 15, Bowman)#l\r\n#L2##b #t1452001##k (Level Requirement: 20, Bowman)#l\r\n#L3##b #t1452000##k (Level Requirement: 25, Bowman)#l\r\n#L4##b #t1452005##k (Level Requirement: 30, Bowman)#l\r\n#L5##b #t1452006##k (Level Requirement: 35, Bowman)#l\r\n#L6##b #t1452007##k (Level Requirement: 40, Bowman)#l");
                        if (v2 == 0) chat_message1(1, "#t1452002#", "#v4003001# 5 #t4003001#s \r\n#v4000000# 30 #t4000000#s \r\n#v4031138# 800 mesos", 10);
                        else if (v2 == 1) chat_message1(2, "#t1452003#", "#v4011001# #t4011001# \r\n#v4003000# 3 #t4003000#s \r\n#v4031138# 2,000 mesos", 15);
                        else if (v2 == 2) chat_message1(3, "#t1452001#", "#v4003001# 30 #t4003001#s \r\n#v4000016# 50 #t4000016#s \r\n#v4031138# 3,000 mesos", 20);
                        else if (v2 == 3) chat_message1(4, "#t1452000#", "#v4011001# 2 #t4011001#s \r\n#v4021006# 2 #t4021006#s \r\n#v4003000# 8 #t4003000#s \r\n#v4031138# 5,000 mesos", 25);
                        else if (v2 == 4) chat_message1(5, "#t1452005#", "#v4011001# 5 #t4011001#s \r\n#v4011006# 5 #t4011006#s \r\n#v4021003# 3 #t4021003#s \r\n#v4021006# 2 #t4021006#s \r\n#v4003000# 30 #t4003000#s \r\n#v4031138# 30,000 mesos", 30);
                        else if (v2 == 5) chat_message1(6, "#t1452006#", "#v4011004# 7 #t4011004#s \r\n#v4021000# 6 #t4021000#s \r\n#v4021004# 3 #t4021004#s \r\n#v4003000# 35 #t4003000#s \r\n#v4031138# 40,000 mesos", 35);
                        else if (v2 == 6) chat_message1(7, "#t1452007#", "#v4021008# #t4021008# \r\n#v4011001# 10 #t4011001#s \r\n#v4011006# 3 #t4011006#s \r\n#v4000014# 50 #t4000014#s \r\n#v4003000# 40 #t4003000#s \r\n#v4031138# 80,000 mesos", 40);
                    }
                    else if (v1 == 1)
                    {
                        v2 = self.AskMenu("I'll make a crossbow if you can get me some materials. What kind of crossbow do you want?\r\n#L0##b #t1462001##k (Level Requirement: 12, Bowman)#l\r\n#L1##b #t1462002##k (Level Requirement: 18, Bowman)#l\r\n#L2##b #t1462003##k (Level Requirement: 22, Bowman)#l\r\n#L3##b #t1462000##k (Level Requirement: 28, Bowman)#l\r\n#L4##b #t1462004##k (Level Requirement: 32, Bowman)#l\r\n#L5##b #t1462005##k (Level Requirement: 38, Bowman)#l\r\n#L6##b #t1462006##k (Level Requirement: 42, Bowman)#l\r\n#L7##b #t1462007##k (Level Requirement: 50, Bowman)#l");
                        if (v2 == 0) chat_message1(100, "#t1462001#", "#v4003001# 7 #t4003001#s \r\n#v4003000# 2 #t4003000#s \r\n#v4031138# 1,000 mesos", 12);
                        else if (v2 == 1) chat_message1(101, "#t1462002#", "#v4003001# 20 #t4003001#s \r\n#v4011001# #t4011001# \r\n#v4003000# 5 #t4003000#s \r\n#v4031138# 2,000 mesos", 18);
                        else if (v2 == 2) chat_message1(102, "#t1462003#", "#v4003001# 50 #t4003001#s \r\n#v4011001# #t4011001# \r\n#v4003000# 8 #t4003000#s \r\n#v4031138# 3,000 mesos", 22);
                        else if (v2 == 3) chat_message1(103, "#t1462000#", "#v4011001# 2 #t4011001#s \r\n#v4021006# #t4021006# \r\n#v4021002# #t4021002# \r\n#v4003000# 10 #t4003000#s \r\n#v4031138# 10,000 mesos", 28);
                        else if (v2 == 4) chat_message1(104, "#t1462004#", "#v4011001# 5 #t4011001#s \r\n#v4011005# 5 #t4011005#s \r\n#v4021006# 3 #t4021006#s \r\n#v4003001# 50 #t4003001#s \r\n#v4003000# 15 #t4003000#s \r\n#v4031138# 30,000 mesos", 32);
                        else if (v2 == 5) chat_message1(105, "#t1462005#", "#v4021008# #t4021008# \r\n#v4011001# 8 #t4011001#s \r\n#v4011006# 4 #t4011006#s \r\n#v4021006# 2 #t4021006#s \r\n#v4003000# 30 #t4003000#s \r\n#v4031138# 50,000 mesos", 38);
                        else if (v2 == 6) chat_message1(106, "#t1462006#", "#v4021008# 2 #t4021008#s \r\n#v4011004# 6 #t4011004#s \r\n#v4003001# 30 #t4003001#s \r\n#v4003000# 30 #t4003000#s \r\n#v4031138# 80,000 mesos", 42);
                        else if (v2 == 7) chat_message1(107, "#t1462007#", "#v4021008# 2 #t4021008#s \r\n#v4011006# 5 #t4011006#s \r\n#v4021006# 3 #t4021006#s \r\n#v4003001# 40 #t4003001#s \r\n#v4003000# 40 #t4003000#s \r\n#v4031138# 100,000 mesos", 50);
                    }
                    else if (v1 == 2)
                    {
                        v2 = self.AskMenu("I'll make a glove if you can get me some materials. What kind of glove do you want?\r\n#L0##b #t1082012##k (Level Requirement: 15, Bowman)#l\r\n#L1##b #t1082013##k (Level Requirement: 20, Bowman)#l\r\n#L2##b #t1082016##k (Level Requirement: 25, Bowman)#l\r\n#L3##b #t1082048##k (Level Requirement: 30, Bowman)#l\r\n#L4##b #t1082068##k (Level Requirement: 35, Bowman)#l\r\n#L5##b #t1082071##k (Level Requirement: 40, Bowman)#l\r\n#L6##b #t1082084##k (Level Requirement: 50, Bowman)#l\r\n#L7##b #t1082089##k (Level Requirement: 60, Bowman)#l");
                        if (v2 == 0) chat_message1(200, "#t1082012#", "#v4000021# 15 #t4000021#s \r\n#v4000009# 20 #t4000009#s \r\n#v4031138# 5,000 mesos", 15);
                        else if (v2 == 1) chat_message1(201, "#t1082013#", "#v4000021# 20 #t4000021#s \r\n#v4000009# 20 #t4000009#s \r\n#v4011001# 2 #t4011001#s \r\n#v4031138# 10,000 mesos", 20);
                        else if (v2 == 2) chat_message1(202, "#t1082016#", "#v4000021# 40 #t4000021#s \r\n#v4000009# 50 #t4000009#s \r\n#v4011006# 2 #t4011006#s \r\n#v4031138# 15,000 mesos", 25);
                        else if (v2 == 3) chat_message1(203, "#t1082048#", "#v4000021# 50 #t4000021#s \r\n#v4021001# #t4021001# \r\n#v4011006# 2 #t4011006#s \r\n#v4031138# 20,000 mesos", 30);
                        else if (v2 == 4) chat_message1(204, "#t1082068#", "#v4000021# 60 #t4000021#s \r\n#v4011001# 3 #t4011001#s \r\n#v4011000# #t4011000# \r\n#v4003000# 15 #t4003000#s \r\n#v4031138# 30,000 mesos", 35);
                        else if (v2 == 5) chat_message1(205, "#t1082071#", "#v4000021# 80 #t4000021#s \r\n#v4021002# 3 #t4021002#s \r\n#v4011001# 3 #t4011001#s \r\n#v4021000# #t4021000# \r\n#v4003000# 25 #t4003000#s \r\n#v4031138# 40,000 mesos", 40);
                        else if (v2 == 6) chat_message1(206, "#t1082084#", "#v4011004# 3 #t4011004#s \r\n#v4021002# 2 #t4021002#s \r\n#v4011006# #t4011006# \r\n#v4000030# 40 #t4000030#s \r\n#v4003000# 35 #t4003000#s \r\n#v4031138# 50,000 mesos", 50);
                        else if (v2 == 7) chat_message1(207, "#t1082089#", "#v4011007# #t4011007# \r\n#v4021006# 8 #t4021006#s \r\n#v4011006# 2 #t4011006#s \r\n#v4000030# 50 #t4000030#s \r\n#v4003000# 50 #t4003000#s \r\n#v4031138# 70,000 mesos", 60);
                    }
                    else if (v1 == 3)
                    {
                        self.Say("Do you want to upgrade a glove? Better be careful about that. All items that will be used for the upgrade will disappear, and if you use an item that has already been #rupgraded#k with a scroll, the effect will disappear when upgraded. Think it over.");
                        v2 = self.AskMenu("Let's see, what kind of glove do you want to upgrade?\r\n#L0##b #t1082014##k (Level Requirement: 20, Bowman)#l\r\n#L1##b #t1082015##k (Level Requirement: 20, Bowman)#l\r\n#L2##b #t1082017##k (Level Requirement: 25, Bowman)#l\r\n#L3##b #t1082018##k (Level Requirement: 25, Bowman)#l\r\n#L4##b #t1082049##k (Level Requirement: 30, Bowman)#l\r\n#L5##b #t1082050##k (Level Requirement: 30, Bowman)#l\r\n#L6##b #t1082069##k (Level Requirement: 35, Bowman)#l\r\n#L7##b #t1082070##k (Level Requirement: 35, Bowman)#l\r\n#L8##b#t1082072##k (Level Requirement: 40, Bowman)#l\r\n#L9##b#t1082073##k (Level Requirement: 40, Bowman)#l\r\n#L10##b#t1082085##k (Level Requirement: 50, Bowman)#l\r\n#L11##b#t1082083##k (Level Requirement: 50, Bowman)#l\r\n#L12##b #t1082090##k (Level Requirement: 60, Bowman)#l\r\n#L13##b #t1082091##k (Level Requirement: 60, Bowman)#l");
                        if (v2 == 0) chat_message2(1, "#t1082014#", "#v1082013# #t1082013# \r\n#v4021000# #t4021000# \r\n#v4031138# 5,000 mesos", 20);
                        else if (v2 == 1) chat_message2(2, "#t1082015#", "#v1082013# #t1082013# \r\n#v4021003# 2 #t4021003#s \r\n#v4031138# 7,000 mesos", 20);
                        else if (v2 == 2) chat_message2(3, "#t1082017#", "#v1082016# #t1082016# \r\n#v4021000# 3 #t4021000#s \r\n#v4031138# 10,000 mesos", 25);
                        else if (v2 == 3) chat_message2(4, "#t1082018#", "#v1082016# #t1082016# \r\n#v4021008# #t4021008# \r\n#v4031138# 12,000 mesos", 25);
                        else if (v2 == 4) chat_message2(5, "#t1082049#", "#v1082048# #t1082048# \r\n#v4021003# 3 #t4021003#s \r\n#v4031138# 15,000 mesos", 30);
                        else if (v2 == 5) chat_message2(6, "#t1082050#", "#v1082048# #t1082048# \r\n#v4021008# #t4021008# \r\n#v4031138# 20,000 mesos", 30);
                        else if (v2 == 6) chat_message2(7, "#t1082069#", "#v1082068# #t1082068# \r\n#v4011002# 4 #t4011002#s \r\n#v4031138# 22,000 mesos", 35);
                        else if (v2 == 7) chat_message2(8, "#t1082070#", "#v1082068# #t1082068# \r\n#v4011006# 2 #t4011006#s \r\n#v4031138# 25,000 mesos", 35);
                        else if (v2 == 8) chat_message2(9, "#t1082072#", "#v1082071# #t1082071# \r\n#v4011006# 4 #t4011006#s \r\n#v4031138# 30,000 mesos", 40);
                        else if (v2 == 9) chat_message2(10, "#t1082073#", "#v1082071# #t1082071# \r\n#v4021008# 2 #t4021008#s \r\n#v4031138# 40,000 mesos", 40);
                        else if (v2 == 10) chat_message2(11, "#t1082085#", "#v1082084# #t1082084# \r\n#v4021000# 5 #t4021000#s \r\n#v4011000# #t4011000# \r\n#v4031138# 55,000 mesos", 50);
                        else if (v2 == 11) chat_message2(12, "#t1082083#", "#v1082084# #t1082084# \r\n#v4021008# 2 #t4021008#s \r\n#v4011006# 2 #t4011006#s \r\n#v4031138# 60,000 mesos", 50);
                        else if (v2 == 12) chat_message2(13, "#t1082090#", "#v1082089# #t1082089# \r\n#v4021007# #t4021007# \r\n#v4021000# 5 #t4021000#s \r\n#v4031138# 70,000 mesos", 60);
                        else if (v2 == 13) chat_message2(14, "#t1082091#", "#v1082089# #t1082089# \r\n#v4021007# 2 #t4021007#s \r\n#v4021008# 2 #t4021008#s \r\n#v4031138# 80,000 mesos", 60);
                    }
                    else if (v1 == 4)
                    {
                        v2 = self.AskMenu("So, you want to create some materials, right? Very well... of what kind?\r\n#L0##b Create #t4003001# with #t4000003##k#l\r\n#L1##b Create #t4003001# with #t4000018##k#l\r\n#L2##b Create #t4003000##k#l");
                        if (v2 == 0) chat_message3(1, "#t4003001#", "#t4000003#", 10, 1);
                        else if (v2 == 1) chat_message3(2, "#t4003001#", "#t4000018#(s)", 5, 1);
                        else if (v2 == 2) chat_message3(3, "#t4003000#", "#t4011001#(s) and #t4011000#(s) each", 1, 15);
                    }
                    else if (v1 == 5)
                    {
                        v2 = self.AskMenu("So, you want to create arrows? The better the arrow, the greater your advantage you will be in battle. All right, what kind?\r\n#L0##b #t2060000##k#l\r\n#L1##b #t2061000##k#l\r\n#L2##b #t2060001##k#l\r\n#L3##b #t2061001##k#l");
                        if (v2 == 0) chat_message6(1, "#t2060000#", "#v4003001# #t4003001# \r\n#v4003004# #t4003004# ", 1000, "");
                        else if (v2 == 1) chat_message6(2, "#t2061000#", "#v4003001# #t4003001# \r\n#v4003004# #t4003004# ", 1000, "");
                        else if (v2 == 2) chat_message6(3, "#t2060001#", "#v4011000# #t4011000# \r\n#v4003001# 3 #t4003001#s \r\n#v4003004# 10 #t4003004#s ", 900, "ATT +1");
                        else if (v2 == 3) chat_message6(4, "#t2061001#", "#v4011000# #t4011000# \r\n#v4003001# 3 #t4003001#s \r\n#v4003004# 10 #t4003004#s ", 900, "ATT +1");
                    }
                }
            }
        }
        // Upgrading items at Sleepywood
        [Script("refine_sleepy")]
        class refine_sleepy : INpcScript
        {
            dynamic nAllNum, v1, ret, nPrice, nRetNum, nRetBuy, nRet, v2, inventory;
            public void Run(INpcHost self, GameCharacter target)
            {
                make1.self = self;
                make1.target = target;
                v1 = self.AskMenu("Make sure you have an empty space in your etc inventory before refining a raw ore or crafting an item. Let's see... What do you want to do?\r\n#b#L0# Refine mineral ores#l\r\n#L1# Refine jewel ores#l\r\n#L2# Create Warrior Shoes#l\r\n#L3# Create Magician Shoes#l\r\n#L4# Create Bowman Shoes#l\r\n#L5# Create Thief Shoes#l");
                if (v1 == 0)
                {
                    v2 = self.AskMenu("What kind of mineral do you want to create?\r\n#b#L0# #t4011000##l\r\n#L1# #t4011001##l\r\n#L2# #t4011002##l\r\n #L3# #t4011003##l\r\n#L4# #t4011004##l\r\n#L5# #t4011005##l\r\n#L6# #t4011006##l");
                    if (v2 == 0) chat_message6(1, "#t4011000#", "#v4010000#", "#t4010000#s", 300);
                    else if (v2 == 1) chat_message6(2, "#t4011001#", "#v4010001#", "#t4010001#s", 300);
                    else if (v2 == 2) chat_message6(3, "#t4011002#", "#v4010002#", "#t4010002#s", 300);
                    else if (v2 == 3) chat_message6(4, "#t4011003#", "#v4010003#", "#t4010003#s", 500);
                    else if (v2 == 4) chat_message6(5, "#t4011004#", "#v4010004#", "#t4010004#s", 500);
                    else if (v2 == 5) chat_message6(6, "#t4011005#", "#v4010005#", "#t4010005#s", 500);
                    else if (v2 == 6) chat_message6(7, "#t4011006#", "#v4010006#", "#t4010006#s", 800);
                }
                else if (v1 == 1)
                {
                    v2 = self.AskMenu("Which jewel do you want to refine?\r\n#b#L0# #t4021000##l\r\n#L1# #t4021001##l\r\n#L2# #t4021002##l\r\n#L3# #t4021003##l\r\n#L4# #t4021004##l\r\n#L5# #t4021005##l\r\n#L6# #t4021006##l\r\n#L7# #t4021007##l\r\n#L8# #t4021008##l");
                    if (v2 == 0) chat_message6(100, "#t4021000#", "#v4020000#", "#t4020000#s", 500);
                    else if (v2 == 1) chat_message6(101, "#t4021001#", "#v4020001#", "#t4020001#s", 500);
                    else if (v2 == 2) chat_message6(102, "#t4021002#", "#v4020002#", "#t4020002#s", 500);
                    else if (v2 == 3) chat_message6(103, "#t4021003#", "#v4020003#", "#t4020003#s", 500);
                    else if (v2 == 4) chat_message6(104, "#t4021004#", "#v4020004#", "#t4020004#s", 500);
                    else if (v2 == 5) chat_message6(105, "#t4021005#", "#v4020005#", "#t4020005#s", 500);
                    else if (v2 == 6) chat_message6(106, "#t4021006#", "#v4020006#", "#t4020006#s", 500);
                    else if (v2 == 7) chat_message6(107, "#t4021007#", "#v4020007#", "#t4020007#s", 1000);
                    else if (v2 == 8) chat_message6(108, "#t4021008#", "#v4020008#", "#t4020008#s", 3000);
                }
                else if (v1 == 2)
                {
                    v2 = self.AskMenu("So, you want to create Warrior Shoes. What kind of shoes do you want to create?\r\n#L0##b #t1072051##k (Level Requirement: 25, Warrior)#l\r\n#L1##b #t1072053##k (Level Requirement: 25, Warrior)#l\r\n#L2##b #t1072052##k (Level Requirement: 25, Warrior)#l\r\n#L3##b #t1072003##k (Level Requirement: 30, Warrior)#l\r\n#L4##b #t1072039##k (Level Requirement: 30, Warrior)#l\r\n#L5##b #t1072040##k (Level Requirement: 30, Warrior)#l\r\n#L6##b #t1072041##k (Level Requirement: 30, Warrior)#l\r\n#L7##b #t1072002##k (Level Requirement: 35, Warrior)#l\r\n#L8##b #t1072112##k (Level Requirement: 35, Warrior)#l\r\n#L9##b #t1072113##k (Level Requirement: 35, Warrior)#l\r\n#L10##b #t1072000##k (Level Requirement: 40, Warrior)#l\r\n#L11##b #t1072126##k (Level Requirement: 40, Warrior)#l\r\n#L12##b #t1072127##k (Level Requirement: 40, Warrior)#l\r\n#L13##b #t1072132##k (Level Requirement: 50, Warrior)#l\r\n#L14##b #t1072133##k (Level Requirement: 50, Warrior)#l\r\n#L15##b #t1072134##k (Level Requirement: 50, Warrior)#l\r\n#L16##b #t1072135##k (Level Requirement: 50, Warrior)#l\r\n#L17##b #t1072147##k (Level Requirement: 60, Warrior)#l\r\n#L18##b #t1072148##k (Level Requirement: 60, Warrior)#l\r\n#L19##b #t1072149##k (Level Requirement: 60, Warrior)#l");
                    if (v2 == 0) chat_message5(1, "#t1072051#", "#v4011004# 2 #t4011004#s \r\n#v4011001# #t4011001# \r\n#v4000021# 15 #t4000021#s \r\n#v4003000# 10 #t4003000#s \r\n#v4031138# 10,000 mesos", 25, "DEX +1");
                    else if (v2 == 1) chat_message5(2, "#t1072053#", "#v4011006# 2 #t4011006#s \r\n#v4011001# #t4011001# \r\n#v4000021# 15 #t4000021#s \r\n#v4003000# 10 #t4003000#s \r\n#v4031138# 10,000 mesos", 25, "DEX +1");
                    else if (v2 == 2) chat_message5(3, "#t1072052#", "#v4021008# #t4021008# \r\n#v4011001# 2 #t4011001#s \r\n#v4000021# 20 #t4000021#s \r\n#v4003000# 10 #t4003000#s \r\n#v4031138# 12,000 mesos", 25, "STR +2");
                    else if (v2 == 3) chat_message5(4, "#t1072003#", "#v4021003# 4 #t4021003#s \r\n#v4011001# 2 #t4011001#s \r\n#v4000021# 45 #t4000021#s \r\n#v4003000# 15 #t4003000#s \r\n#v4031138# 20,000 mesos", 30, "STR +1");
                    else if (v2 == 4) chat_message5(5, "#t1072039#", "#v4011002# 4 #t4011002#s \r\n#v4011001# 2 #t4011001#s \r\n#v4000021# 45 #t4000021#s \r\n#v4003000# 15 #t4003000#s \r\n#v4031138# 20,000 mesos", 30, "DEX +1");
                    else if (v2 == 5) chat_message5(6, "#t1072040#", "#v4011004# 4 #t4011004#s \r\n#v4011001# 2 #t4011001#s \r\n#v4000021# 45 #t4000021#s \r\n#v4003000# 15 #t4003000#s \r\n#v4031138# 20,000 mesos", 30, "STR +1");
                    else if (v2 == 6) chat_message5(7, "#t1072041#", "#v4021000# 4 #t4021000#s \r\n#v4011001# 2 #t4011001#s \r\n#v4000021# 45 #t4000021#s \r\n#v4003000# 15 #t4003000#s \r\n#v4031138# 20,000 mesos", 30, "DEX +1");
                    else if (v2 == 7) chat_message5(8, "#t1072002#", "#v4011001# 3 #t4011001#s \r\n#v4021004# #t4021004# \r\n#v4000021# 30 #t4000021#s \r\n#v4000030# 20 #t4000030#s \r\n#v4003000# 25 #t4003000#s \r\n#v4031138# 22,000 mesos", 35, "STR +1, MP +10");
                    else if (v2 == 8) chat_message5(9, "#t1072112#", "#v4011002# 3 #t4011002#s \r\n#v4021004# #t4021004# \r\n#v4000021# 30 #t4000021#s \r\n#v4000030# 20 #t4000030#s \r\n#v4003000# 25 #t4003000#s \r\n#v4031138# 22,000 mesos", 35, "DEX +1, MP +10");
                    else if (v2 == 9) chat_message5(10, "#t1072113#", "#v4021008# 2 #t4021008#s \r\n#v4021004# #t4021004# \r\n#v4000021# 30 #t4000021#s \r\n#v4000030# 20 #t4000030#s \r\n#v4003000# 25 #t4003000#s \r\n#v4031138# 25,000 mesos", 35, "STR +1, DEX +1, MP +10");
                    else if (v2 == 10) chat_message5(11, "#t1072000#", "#v4011003# 4 #t4011003#s \r\n#v4000021# 100 #t4000021#s \r\n#v4000030# 40 #t4000030#s \r\n#v4003000# 30 #t4003000#s \r\n#v4000033# 100 #t4000033#s \r\n#v4031138# 38,000 mesos", 40, "DEX +2, MP +20");
                    else if (v2 == 11) chat_message5(12, "#t1072126#", "#v4011005# 4 #t4011005#s \r\n#v4021007# #t4021007# \r\n#v4000030# 40 #t4000030#s \r\n#v4003000# 30 #t4003000#s \r\n#v4000042# 250 #t4000042#s \r\n#v4031138# 38,000 mesos", 40, "STR +1, DEX +1, MP +20");
                    else if (v2 == 12) chat_message5(13, "#t1072127#", "#v4011002# 4 #t4011002#s \r\n#v4021007# #t4021007# \r\n#v4000030# 40 #t4000030#s \r\n#v4003000# 30 #t4003000#s \r\n#v4000041# 120 #t4000041#s \r\n#v4031138# 38,000 mesos", 40, "STR +2, MP +20");
                    else if (v2 == 13) chat_message5(14, "#t1072132#", "#v4021008# #t4021008# \r\n#v4011001# 3 #t4011001#s \r\n#v4021003# 6 #t4021003#s \r\n#v4000030# 65 #t4000030#s \r\n#v4003000# 45 #t4003000#s \r\n#v4031138# 50,000 mesos", 50, "DEX +3");
                    else if (v2 == 14) chat_message5(15, "#t1072133#", "#v4021008# #t4021008# \r\n#v4011001# 3 #t4011001#s \r\n#v4011002# 6 #t4011002#s \r\n#v4000030# 65 #t4000030#s \r\n#v4003000# 45 #t4003000#s \r\n#v4031138# 50,000 mesos", 50, "DEX + 2, STR + 1");
                    else if (v2 == 15) chat_message5(16, "#t1072134#", "#v4021008# #t4021008# \r\n#v4011001# 3 #t4011001#s \r\n#v4011005# 6 #t4011005#s \r\n#v4000030# 65 #t4000030#s \r\n#v4003000# 45 #t4003000#s \r\n#v4031138# 50,000 mesos", 50, "DEX + 1, STR + 2");
                    else if (v2 == 16) chat_message5(17, "#t1072135#", "#v4021008# #t4021008# \r\n#v4011001# 3 #t4011001#s \r\n#v4011006# 6 #t4011006#s \r\n#v4000030# 65 #t4000030#s \r\n#v4003000# 45 #t4003000#s \r\n#v4031138# 50,000 mesos", 50, "STR +3");
                    else if (v2 == 17) chat_message5(18, "#t1072147#", "#v4021008# #t4021008# \r\n#v4011007# #t4011007# \r\n#v4021005# 8 #t4021005#s \r\n#v4000030# 80 #t4000030#s \r\n#v4003000# 55 #t4003000#s \r\n#v4031138# 60,000 mesos", 60, "STR +1, DEX +3");
                    else if (v2 == 18) chat_message5(19, "#t1072148#", "#v4021008# #t4021008# \r\n#v4011007# #t4011007# \r\n#v4011005# 8 #t4011005#s \r\n#v4000030# 80 #t4000030#s \r\n#v4003000# 55 #t4003000#s \r\n#v4031138# 60,000 mesos", 60, "STR + 2, DEX + 2");
                    else if (v2 == 19) chat_message5(20, "#t1072149#", "#v4021008# #t4021008# \r\n#v4011007# #t4011007# \r\n#v4021000# 8 #t4021000#s \r\n#v4000030# 80 #t4000030#s \r\n#v4003000# 55 #t4003000#s \r\n#v4031138# 60,000 mesos", 60, "STR + 3, DEX + 1");
                }
                else if (v1 == 3)
                {
                    v2 = self.AskMenu("So, you want to create Magician Shoes. What kind of shoes do you want to create?\r\n#L0##b #t1072019##k (Level Requirement: 20, Magician)#l\r\n#L1##b #t1072020##k (Level Requirement: 20, Magician)#l\r\n#L2##b #t1072021##k (Level Requirement: 20, Magician)#l\r\n#L3##b #t1072072##k (Level Requirement: 25, Magician)#l\r\n#L4##b #t1072073##k (Level Requirement: 25, Magician)#l\r\n#L5##b #t1072074##k (Level Requirement: 25, Magician)#l\r\n#L6##b #t1072075##k (Level Requirement: 30, Magician)#l\r\n#L7##b #t1072076##k (Level Requirement: 30, Magician)#l\r\n#L8##b #t1072077##k (Level Requirement: 30, Magician)#l\r\n#L9##b #t1072078##k (Level Requirement: 30, Magician)#l\r\n#L10##b #t1072089##k (Level Requirement: 35, Magician)#l\r\n#L11##b #t1072090##k (Level Requirement: 35, Magician)#l\r\n#L12##b #t1072091##k (Level Requirement: 35, Magician)#l\r\n#L13##b #t1072114##k (Level Requirement: 40, Magician)#l\r\n#L14##b #t1072115##k (Level Requirement: 40, Magician)#l\r\n#L15##b #t1072116##k (Level Requirement: 40, Magician)#l\r\n#L16##b #t1072117##k (Level Requirement: 40, Magician)#l\r\n#L17##b #t1072140##k (Level Requirement: 50, Magician)#l\r\n#L18##b #t1072141##k (Level Requirement: 50, Magician)#l\r\n#L19##b #t1072142##k (Level Requirement: 50, Magician)#l\r\n#L20##b #t1072143##k (Level Requirement: 50, Magician)#l\r\n#L21##b #t1072136##k (Level Requirement: 60, Magician)#l\r\n#L22##b #t1072137##k (Level Requirement: 60, Magician)#l\r\n#L23##b #t1072138##k (Level Requirement: 60, Magician)#l\r\n#L24##b #t1072139##k (Level Requirement: 60, Magician)#l");
                    if (v2 == 0) chat_message5(100, "#t1072019#", "#v4021005# #t4021005# \r\n#v4000021# 30 #t4000021#s \r\n#v4003000# 5 #t4003000#s \r\n#v4031138# 3,000 mesos", 20, "INT +1");
                    else if (v2 == 1) chat_message5(101, "#t1072020#", "#v4021001# #t4021001# \r\n#v4000021# 30 #t4000021#s \r\n#v4003000# 5 #t4003000#s \r\n#v4031138# 3,000 mesos", 20, "INT +1");
                    else if (v2 == 2) chat_message5(102, "#t1072021#", "#v4021000# #t4021000# \r\n#v4000021# 30 #t4000021#s \r\n#v4003000# 5 #t4003000#s \r\n#v4031138# 3,000 mesos", 20, "INT +1");
                    else if (v2 == 3) chat_message5(103, "#t1072072#", "#v4011004# #t4011004# \r\n#v4000021# 35 #t4000021#s \r\n#v4003000# 10 #t4003000#s \r\n#v4031138# 8,000 mesos", 25, "INT +1");
                    else if (v2 == 4) chat_message5(104, "#t1072073#", "#v4021006# #t4021006# \r\n#v4000021# 35 #t4000021#s \r\n#v4003000# 10 #t4003000#s \r\n#v4031138# 8,000 mesos", 25, "INT +1");
                    else if (v2 == 5) chat_message5(105, "#t1072074#", "#v4021004# #t4021004# \r\n#v4000021# 35 #t4000021#s \r\n#v4003000# 10 #t4003000#s \r\n#v4031138# 8,000 mesos", 25, "INT +1");
                    else if (v2 == 6) chat_message5(106, "#t1072075#", "#v4021000# 2 #t4021000#s \r\n#v4000021# 50 #t4000021#s \r\n#v4003000# 15 #t4003000#s \r\n#v4031138# 18,000 mesos", 30, "INT +1");
                    else if (v2 == 7) chat_message5(107, "#t1072076#", "#v4021002# 2 #t4021002#s \r\n#v4000021# 50 #t4000021#s \r\n#v4003000# 15 #t4003000#s \r\n#v4031138# 18,000 mesos", 30, "INT +1");
                    else if (v2 == 8) chat_message5(108, "#t1072077#", "#v4011004# 2 #t4011004#s \r\n#v4000021# 50 #t4000021#s \r\n#v4003000# 15 #t4003000#s \r\n#v4031138# 18,000 mesos", 30, "INT +1");
                    else if (v2 == 9) chat_message5(109, "#t1072078#", "#v4021008# #t4021008# \r\n#v4000021# 50 #t4000021#s \r\n#v4003000# 15 #t4003000#s \r\n#v4031138# 18,000 mesos", 30, "INT +2");
                    else if (v2 == 10) chat_message5(110, "#t1072089#", "#v4021001# 3 #t4021001#s \r\n#v4021006# #t4021006# \r\n#v4000021# 30 #t4000021#s \r\n#v4000030# 15 #t4000030#s \r\n#v4003000# 20 #t4003000#s \r\n#v4031138# 20,000 mesos", 35, "LUK +1, MP +10");
                    else if (v2 == 11) chat_message5(111, "#t1072090#", "#v4021000# 3 #t4021000#s \r\n#v4021006# #t4021006# \r\n#v4000021# 30 #t4000021#s \r\n#v4000030# 15 #t4000030#s \r\n#v4003000# 20 #t4003000#s \r\n#v4031138# 20,000 mesos", 35, "INT +1, MP +10");
                    else if (v2 == 12) chat_message5(112, "#t1072091#", "#v4021008# 2 #t4021008#s \r\n#v4021006# #t4021006# \r\n#v4000021# 40 #t4000021#s \r\n#v4000030# 25 #t4000030#s \r\n#v4003000# 20 #t4003000#s \r\n#v4031138# 22,000 mesos", 35, "INT +1, LUK +1");
                    else if (v2 == 13) chat_message5(113, "#t1072114#", "#v4021000# 4 #t4021000#s \r\n#v4000030# 40 #t4000030#s \r\n#v4000043# 35 #t4000043#s \r\n#v4003000# 25 #t4003000#s \r\n#v4031138# 30,000 mesos", 40, "LUK +2, MP +5");
                    else if (v2 == 14) chat_message5(114, "#t1072115#", "#v4021005# 4 #t4021005#s \r\n#v4000030# 40 #t4000030#s \r\n#v4000037# 70 #t4000037#s \r\n#v4003000# 25 #t4003000#s \r\n#v4031138# 30,000 mesos", 40, "LUK +2, MP +5");
                    else if (v2 == 15) chat_message5(115, "#t1072116#", "#v4011006# 2 #t4011006#s \r\n#v4021007# #t4021007# \r\n#v4000030# 40 #t4000030#s \r\n#v4000027# 20 #t4000027#s \r\n#v4003000# 25 #t4003000#s \r\n#v4031138# 35,000 mesos", 40, "INT +2, MP +5");
                    else if (v2 == 16) chat_message5(116, "#t1072117#", "#v4021008# 2 #t4021008#s \r\n#v4021007# #t4021007# \r\n#v4000030# 40 #t4000030#s \r\n#v4000014# 30 #t4000014#s \r\n#v4003000# 30 #t4003000#s \r\n#v4031138# 40,000 mesos", 40, "INT +2, LUK +1, MP +10");
                    else if (v2 == 17) chat_message5(117, "#t1072140#", "#v4021009# #t4021009# \r\n#v4011006# 3 #t4011006#s \r\n#v4021000# 3 #t4021000#s \r\n#v4000030# 60 #t4000030#s \r\n#v4003000# 40 #t4003000#s \r\n#v4031138# 50,000 mesos", 50, "LUK +3");
                    else if (v2 == 18) chat_message5(118, "#t1072141#", "#v4021009# #t4021009# \r\n#v4011006# 3 #t4011006#s \r\n#v4021005# 3 #t4021005#s \r\n#v4000030# 60 #t4000030#s \r\n#v4003000# 40 #t4003000#s \r\n#v4031138# 50,000 mesos", 50, "INT +1, LUK +2");
                    else if (v2 == 19) chat_message5(119, "#t1072142#", "#v4021009# #t4021009# \r\n#v4011006# 3 #t4011006#s \r\n#v4021001# 3 #t4021001#s \r\n#v4000030# 60 #t4000030#s \r\n#v4003000# 40 #t4003000#s \r\n#v4031138# 50,000 mesos", 50, "INT +2, LUK +1");
                    else if (v2 == 20) chat_message5(120, "#t1072143#", "#v4021009# #t4021009# \r\n#v4011006# 3 #t4011006#s \r\n#v4021003# 3 #t4021003#s \r\n#v4000030# 60 #t4000030#s \r\n#v4003000# 40 #t4003000#s \r\n#v4031138# 50,000 mesos", 50, "INT +3");
                    else if (v2 == 21) chat_message5(121, "#t1072136#", "#v4021009# #t4021009# \r\n#v4011006# 4 #t4011006#s \r\n#v4011005# 5 #t4011005#s \r\n#v4000030# 70 #t4000030#s \r\n#v4003000# 50 #t4003000#s \r\n#v4031138# 60,000 mesos", 60, "INT +1, LUK +3");
                    else if (v2 == 22) chat_message5(122, "#t1072137#", "#v4021009# #t4021009# \r\n#v4011006# 4 #t4011006#s \r\n#v4021003# 5 #t4021003#s \r\n#v4000030# 70 #t4000030#s \r\n#v4003000# 50 #t4003000#s \r\n#v4031138# 60,000 mesos", 60, "INT +2, LUK +2");
                    else if (v2 == 23) chat_message5(123, "#t1072138#", "#v4021009# #t4021009# \r\n#v4011006# 4 #t4011006#s \r\n#v4011003# 5 #t4011003#s \r\n#v4000030# 70 #t4000030#s \r\n#v4003000# 50 #t4003000#s \r\n#v4031138# 60,000 mesos", 60, "INT +3, LUK +1");
                    else if (v2 == 24) chat_message5(124, "#t1072139#", "#v4021009# #t4021009# \r\n#v4011006# 4 #t4011006#s \r\n#v4021002# 5 #t4021002#s \r\n#v4000030# 70 #t4000030#s \r\n#v4003000# 50 #t4003000#s \r\n#v4031138# 60,000 mesos", 60, "INT +3, LUK +1");
                }
                else if (v1 == 4)
                {
                    v2 = self.AskMenu("So, you want to create Bowman Shoes. What kind of shoes do you want to create?\r\n#L0##b #t1072027##k (Level Requirement: 25, Bowman)#l\r\n#L1##b #t1072034##k (Level Requirement: 25, Bowman)#l\r\n#L2##b #t1072069##k (Level Requirement: 25, Bowman)#l\r\n#L3##b #t1072079##k (Level Requirement: 30, Bowman)#l\r\n#L4##b #t1072080##k (Level Requirement: 30, Bowman)#l\r\n#L5##b #t1072081##k (Level Requirement: 30, Bowman)#l\r\n#L6##b #t1072082##k (Level Requirement: 30, Bowman)#l\r\n#L7##b #t1072083##k (Level Requirement: 30, Bowman)#l\r\n#L8##b #t1072101##k (Level Requirement: 35, Bowman)#l\r\n#L9##b #t1072102##k (Level Requirement: 35, Bowman)#l\r\n#L10##b #t1072103##k (Level Requirement: 35, Bowman)#l\r\n#L11##b #t1072118##k (Level Requirement: 40, Bowman)#l\r\n#L12##b #t1072119##k (Level Requirement: 40, Bowman)#l\r\n#L13##b #t1072120##k (Level Requirement: 40, Bowman)#l\r\n#L14##b #t1072121##k (Level Requirement: 40, Bowman)#l\r\n#L15##b #t1072122##k (Level Requirement: 50, Bowman)#l\r\n#L16##b #t1072123##k (Level Requirement: 50, Bowman)#l\r\n#L17##b #t1072124##k (Level Requirement: 50, Bowman)#l\r\n#L18##b #t1072125##k (Level Requirement: 50, Bowman)#l\r\n#L19##b #t1072144##k (Level Requirement: 60, Bowman)#l\r\n#L20##b #t1072145##k (Level Requirement: 60, Bowman)#l\r\n#L21##b #t1072146##k (Level Requirement: 60, Bowman)#l");
                    if (v2 == 0) chat_message5(200, "#t1072027#", "#v4011000# 3 #t4011000#s \r\n#v4000021# 35 #t4000021#s \r\n#v4003000# 10 #t4003000#s \r\n#v4031138# 9,000 mesos", 25, "DEX +1");
                    else if (v2 == 1) chat_message5(201, "#t1072034#", "#v4021003# #t4021003# \r\n#v4000021# 35 #t4000021#s \r\n#v4003000# 10 #t4003000#s \r\n#v4031138# 9,000 mesos", 25, "DEX +1");
                    else if (v2 == 2) chat_message5(202, "#t1072069#", "#v4021000# #t4021000# \r\n#v4000021# 35 #t4000021#s \r\n#v4003000# 10 #t4003000#s \r\n#v4031138# 9,000 mesos", 25, "DEX +1");
                    else if (v2 == 3) chat_message5(203, "#t1072079#", "#v4021000# 2 #t4021000#s \r\n#v4000021# 50 #t4000021#s \r\n#v4003000# 15 #t4003000#s \r\n#v4031138# 19,000 mesos", 30, "DEX +1");
                    else if (v2 == 4) chat_message5(204, "#t1072080#", "#v4021005# 2 #t4021005#s \r\n#v4000021# 50 #t4000021#s \r\n#v4003000# 15 #t4003000#s \r\n#v4031138# 19,000 mesos", 30, "STR +1");
                    else if (v2 == 5) chat_message5(205, "#t1072081#", "#v4021003# 2 #t4021003#s \r\n#v4000021# 50 #t4000021#s \r\n#v4003000# 15 #t4003000#s \r\n#v4031138# 19,000 mesos", 30, "DEX +1");
                    else if (v2 == 6) chat_message5(206, "#t1072082#", "#v4021004# 2 #t4021004#s \r\n#v4000021# 50 #t4000021#s \r\n#v4003000# 15 #t4003000#s \r\n#v4031138# 19,000 mesos", 30, "DEX +1");
                    else if (v2 == 7) chat_message5(207, "#t1072083#", "#v4021006# 2 #t4021006#s \r\n#v4000021# 50 #t4000021#s \r\n#v4003000# 15 #t4003000#s \r\n#v4031138# 19,000 mesos", 30, "STR +1");
                    else if (v2 == 8) chat_message5(208, "#t1072101#", "#v4021002# 3 #t4021002#s \r\n#v4021006# #t4021006# \r\n#v4000021# 30 #t4000021#s \r\n#v4000030# 15 #t4000030#s \r\n#v4003000# 20 #t4003000#s \r\n#v4031138# 20,000 mesos", 35, "STR +2");
                    else if (v2 == 9) chat_message5(209, "#t1072102#", "#v4021003# 3 #t4021003#s \r\n#v4021006# #t4021006# \r\n#v4000021# 30 #t4000021#s \r\n#v4000030# 15 #t4000030#s \r\n#v4003000# 20 #t4003000#s \r\n#v4031138# 20,000 mesos", 35, "DEX +2");
                    else if (v2 == 10) chat_message5(210, "#t1072103#", "#v4021000# 3 #t4021000#s \r\n#v4021006# #t4021006# \r\n#v4000021# 30 #t4000021#s \r\n#v4000030# 15 #t4000030#s \r\n#v4003000# 20 #t4003000#s \r\n#v4031138# 20,000 mesos", 35, "STR + 1, DEX + 1");
                    else if (v2 == 11) chat_message5(211, "#t1072118#", "#v4021000# 4 #t4021000#s \r\n#v4000030# 45 #t4000030#s \r\n#v4000024# 40 #t4000024#s \r\n#v4003000# 30 #t4003000#s \r\n#v4031138# 32,000 mesos", 40, "STR +2, MP +5");
                    else if (v2 == 12) chat_message5(212, "#t1072119#", "#v4021006# 4 #t4021006#s \r\n#v4000030# 45 #t4000030#s \r\n#v4000027# 20 #t4000027#s \r\n#v4003000# 30 #t4003000#s \r\n#v4031138# 32,000 mesos", 40, "DEX +1, STR +1, MP +5");
                    else if (v2 == 13) chat_message5(213, "#t1072120#", "#v4011003# 5 #t4011003#s \r\n#v4000030# 45 #t4000030#s \r\n#v4000044# 40 #t4000044#s \r\n#v4003000# 30 #t4003000#s \r\n#v4031138# 40,000 mesos", 40, "DEX +2, MP +5");
                    else if (v2 == 14) chat_message5(214, "#t1072121#", "#v4021002# 5 #t4021002#s \r\n#v4000030# 45 #t4000030#s \r\n#v4000009# 120 #t4000009#s \r\n#v4003000# 30 #t4003000#s \r\n#v4031138# 40,000 mesos", 40, "DEX +2, MP +5");
                    else if (v2 == 15) chat_message5(215, "#t1072122#", "#v4021008# #t4021008# \r\n#v4011001# 3 #t4011001#s \r\n#v4021006# 3 #t4021006#s \r\n#v4000030# 60 #t4000030#s \r\n#v4000033# 80 #t4000033#s \r\n#v4003000# 35 #t4003000#s \r\n#v4031138# 50,000 mesos", 50, "STR +3");
                    else if (v2 == 16) chat_message5(216, "#t1072123#", "#v4021008# #t4021008# \r\n#v4011001# 3 #t4011001#s \r\n#v4021006# 3 #t4021006#s \r\n#v4000030# 60 #t4000030#s \r\n#v4000032# 150 #t4000032#s \r\n#v4003000# 35 #t4003000#s \r\n#v4031138# 50,000 mesos", 50, "DEX + 1, STR + 2");
                    else if (v2 == 17) chat_message5(217, "#t1072124#", "#v4021008# #t4021008# \r\n#v4011001# 3 #t4011001#s \r\n#v4021006# 3 #t4021006#s \r\n#v4000030# 60 #t4000030#s \r\n#v4000041# 100 #t4000041#s \r\n#v4003000# 35 #t4003000#s \r\n#v4031138# 50,000 mesos", 50, "DEX + 2, STR + 1");
                    else if (v2 == 18) chat_message5(218, "#t1072125#", "#v4021008# #t4021008# \r\n#v4011001# 3 #t4011001#s \r\n#v4021006# 3 #t4021006#s \r\n#v4000030# 60 #t4000030#s \r\n#v4000042# 250 #t4000042#s \r\n#v4003000# 35 #t4003000#s \r\n#v4031138# 50,000 mesos", 50, "DEX +3");
                    else if (v2 == 19) chat_message5(219, "#t1072144#", "#v4021007# #t4021007# \r\n#v4011006# 5 #t4011006#s \r\n#v4021000# 8 #t4021000#s \r\n#v4000030# 75 #t4000030#s \r\n#v4003000# 50 #t4003000#s \r\n#v4031138# 60,000 mesos", 60, "DEX +1, STR +3");
                    else if (v2 == 20) chat_message5(220, "#t1072145#", "#v4021007# #t4021007# \r\n#v4011006# 5 #t4011006#s \r\n#v4021005# 8 #t4021005#s \r\n#v4000030# 75 #t4000030#s \r\n#v4003000# 50 #t4003000#s \r\n#v4031138# 60,000 mesos", 60, "DEX + 2, STR + 2");
                    else if (v2 == 21) chat_message5(221, "#t1072146#", "#v4021007# #t4021007# \r\n#v4011006# 5 #t4011006#s \r\n#v4021003# 8 #t4021003#s \r\n#v4000030# 75 #t4000030#s \r\n#v4003000# 50 #t4003000#s \r\n#v4031138# 60,000 mesos", 60, "DEX + 3, STR + 1");
                }
                else if (v1 == 5)
                {
                    v2 = self.AskMenu("So, you want to create Thief Shoes. What kind of shoes do you want to create?\r\n#L0##b #t1072084##k (Level Requirement: 25, Thief)#l\r\n#L1##b #t1072085##k (Level Requirement: 25, Thief)#l\r\n#L2##b #t1072086##k (Level Requirement: 25, Thief)#l\r\n#L3##b #t1072087##k (Level Requirement: 25, Thief)#l\r\n#L4##b #t1072032##k (Level Requirement: 30, Thief)#l\r\n#L5##b #t1072033##k (Level Requirement: 30, Thief)#l\r\n#L6##b #t1072035##k (Level Requirement: 30, Thief)#l\r\n#L7##b #t1072036##k (Level Requirement: 30, Thief)#l\r\n#L8##b #t1072104##k (Level Requirement: 35, Thief)#l\r\n#L9##b #t1072105##k (Level Requirement: 35, Thief)#l\r\n#L10##b #t1072106##k (Level Requirement: 35, Thief)#l\r\n#L11##b #t1072108##k (Level Requirement: 40, Thief)#l\r\n#L12##b #t1072109##k (Level Requirement: 40, Thief)#l\r\n#L13##b #t1072110##k (Level Requirement: 40, Thief)#l\r\n#L14##b #t1072107##k (Level Requirement: 40, Thief)#l\r\n#L15##b #t1072128##k (Level Requirement: 50, Thief)#l\r\n#L16##b #t1072129##k (Level Requirement: 50, Thief)#l\r\n#L17##b #t1072130##k (Level Requirement: 50, Thief)#l\r\n#L18##b #t1072131##k (Level Requirement: 50, Thief)#l\r\n#L19##b #t1072150##k (Level Requirement: 60, Thief)#l\r\n#L20##b #t1072151##k (Level Requirement: 60, Thief)#l\r\n#L21##b #t1072152##k (Level Requirement: 60, Thief)#l");
                    if (v2 == 0) chat_message5(300, "#t1072084#", "#v4021005# #t4021005# \r\n#v4000021# 35 #t4000021#s \r\n#v4003000# 10 #t4003000#s \r\n#v4031138# 9,000 mesos", 25, "LUK +1");
                    else if (v2 == 1) chat_message5(301, "#t1072085#", "#v4021000# #t4021000# \r\n#v4000021# 35 #t4000021#s \r\n#v4003000# 10 #t4003000#s \r\n#v4031138# 9,000 mesos", 25, "LUK +1");
                    else if (v2 == 2) chat_message5(302, "#t1072086#", "#v4021003# #t4021003# \r\n#v4000021# 35 #t4000021#s \r\n#v4003000# 10 #t4003000#s \r\n#v4031138# 9,000 mesos", 25, "LUK +1");
                    else if (v2 == 3) chat_message5(303, "#t1072087#", "#v4021004# #t4021004# \r\n#v4000021# 35 #t4000021#s \r\n#v4003000# 10 #t4003000#s \r\n#v4031138# 9,000 mesos", 25, "LUK +1");
                    else if (v2 == 4) chat_message5(304, "#t1072032#", "#v4011000# 3 #t4011000#s \r\n#v4000021# 50 #t4000021#s \r\n#v4003000# 15 #t4003000#s \r\n#v4031138# 19,000 mesos", 30, "DEX +1");
                    else if (v2 == 5) chat_message5(305, "#t1072033#", "#v4011001# 3 #t4011001#s \r\n#v4000021# 50 #t4000021#s \r\n#v4003000# 15 #t4003000#s \r\n#v4031138# 19,000 mesos", 30, "LUK +1");
                    else if (v2 == 6) chat_message5(306, "#t1072035#", "#v4011004# 2 #t4011004#s \r\n#v4000021# 50 #t4000021#s \r\n#v4003000# 15 #t4003000#s \r\n#v4031138# 19,000 mesos", 30, "LUK +1");
                    else if (v2 == 7) chat_message5(307, "#t1072036#", "#v4011006# 2 #t4011006#s \r\n#v4000021# 50 #t4000021#s \r\n#v4003000# 15 #t4003000#s \r\n#v4031138# 21,000 mesos", 30, "DEX +2");
                    else if (v2 == 8) chat_message5(308, "#t1072104#", "#v4021000# 3 #t4021000#s \r\n#v4021004# #t4021004# \r\n#v4000021# 30 #t4000021#s \r\n#v4000030# 15 #t4000030#s \r\n#v4003000# 20 #t4003000#s \r\n#v4031138# 20,000 mesos", 35, "LUK +2");
                    else if (v2 == 9) chat_message5(309, "#t1072105#", "#v4021003# 3 #t4021003#s \r\n#v4021004# #t4021004# \r\n#v4000021# 30 #t4000021#s \r\n#v4000030# 15 #t4000030#s \r\n#v4003000# 20 #t4003000#s \r\n#v4031138# 20,000 mesos", 35, "DEX +2");
                    else if (v2 == 10) chat_message5(310, "#t1072106#", "#v4021002# 3 #t4021002#s \r\n#v4021004# #t4021004# \r\n#v4000021# 30 #t4000021#s \r\n#v4000030# 15 #t4000030#s \r\n#v4003000# 20 #t4003000#s \r\n#v4031138# 20,000 mesos", 35, "LUK +1, DEX +1");
                    else if (v2 == 11) chat_message5(311, "#t1072108#", "#v4021003# 4 #t4021003#s \r\n#v4000030# 45 #t4000030#s \r\n#v4000032# 30 #t4000032#s \r\n#v4003000# 30 #t4003000#s \r\n#v4031138# 32,000 mesos", 40, "DEX +2, MP +5");
                    else if (v2 == 12) chat_message5(312, "#t1072109#", "#v4021006# 4 #t4021006#s \r\n#v4000030# 45 #t4000030#s \r\n#v4000040# 3 #t4000040#s \r\n#v4003000# 30 #t4003000#s \r\n#v4031138# 35,000 mesos", 40, "LUK +1, STR +1, MP +5");
                    else if (v2 == 13) chat_message5(313, "#t1072110#", "#v4021005# 4 #t4021005#s \r\n#v4000030# 45 #t4000030#s \r\n#v4000037# 70 #t4000037#s \r\n#v4003000# 30 #t4003000#s \r\n#v4031138# 35,000 mesos", 40, "LUK +1, DEX +1, MP +5");
                    else if (v2 == 14) chat_message5(314, "#t1072107#", "#v4021000# 5 #t4021000#s \r\n#v4000030# 45 #t4000030#s \r\n#v4000033# 50 #t4000033#s \r\n#v4003000# 30 #t4003000#s \r\n#v4031138# 40,000 mesos", 40, "LUK +2, MP +5");
                    else if (v2 == 15) chat_message5(315, "#t1072128#", "#v4011007# 2 #t4011007#s \r\n#v4021005# 3 #t4021005#s \r\n#v4000030# 50 #t4000030#s \r\n#v4000037# 200 #t4000037#s \r\n#v4003000# 35 #t4003000#s \r\n#v4031138# 50,000 mesos", 50, "STR +3");
                    else if (v2 == 16) chat_message5(316, "#t1072129#", "#v4011007# 2 #t4011007#s \r\n#v4021003# 3 #t4021003#s \r\n#v4000030# 50 #t4000030#s \r\n#v4000045# 80 #t4000045#s \r\n#v4003000# 35 #t4003000#s \r\n#v4031138# 50,000 mesos", 50, "DEX +3");
                    else if (v2 == 17) chat_message5(317, "#t1072130#", "#v4011007# 2 #t4011007#s \r\n#v4021000# 3 #t4021000#s \r\n#v4000030# 50 #t4000030#s \r\n#v4000043# 150 #t4000043#s \r\n#v4003000# 35 #t4003000#s \r\n#v4031138# 50,000 mesos", 50, "LUK +3");
                    else if (v2 == 18) chat_message5(318, "#t1072131#", "#v4011007# 2 #t4011007#s \r\n#v4021001# 3 #t4021001#s \r\n#v4000030# 50 #t4000030#s \r\n#v4000036# 80 #t4000036#s \r\n#v4003000# 35 #t4003000#s \r\n#v4031138# 50,000 mesos", 50, "LUK +2, DEX +1");
                    else if (v2 == 19) chat_message5(319, "#t1072150#", "#v4021007# #t4021007# \r\n#v4011007# #t4011007# \r\n#v4021000# 8 #t4021000#s \r\n#v4000030# 75 #t4000030#s \r\n#v4003000# 50 #t4003000#s \r\n#v4031138# 60,000 mesos", 60, "LUK + 1, STR + 3");
                    else if (v2 == 20) chat_message5(320, "#t1072151#", "#v4021007# #t4021007# \r\n#v4011007# #t4011007# \r\n#v4011006# 5 #t4011006#s \r\n#v4000030# 75 #t4000030#s \r\n#v4003000# 50 #t4003000#s \r\n#v4031138# 60,000 mesos", 60, "STR +1, DEX +3");
                    else if (v2 == 21) chat_message5(321, "#t1072152#", "#v4021007# #t4021007# \r\n#v4011007# #t4011007# \r\n#v4021008# #t4021008# \r\n#v4000030# 75 #t4000030# \r\n#v4003000# 50 #t4003000# \r\n#v4031138# 60,000 mesos", 60, "DEX + 1, LUK + 3");
                }
            }
        }
    }
}
