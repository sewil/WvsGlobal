using System;
using WvsBeta.Game;
using WvsBeta.Game.Scripting;

namespace WvsBeta.Scripts.Scripts
{
    public static class make3
    {
        static INpcHost self;
        static GameCharacter target;
        public static void chat_message5(int index, string makeItem, string needItem, int reqLevel, string itemOption)
        {
            var inventory = target.Inventory;
            int ret = 0;

            var nRet = self.AskYesNo("You want to make " + makeItem + "? To do this, you will need the following materials. There is an improvement of #r" + itemOption + "#k on this item, and the Level Requirement is " + reqLevel + ". What do you think? You want to make it??\r\n\r\n#b" + needItem);
            if (nRet == 0) self.Say("I don't just make gloves, but I can also upgrade gloves, so after you think it over, just come back to me...");
            else
            {
                // making warrior gloves 
                if (index == 0) ret = inventory.Exchange(-70000, 4011007, -1, 4011000, -8, 4011006, -2, 4000030, -50, 4003000, -50, 1082059, 1);
                else if (index == 1) ret = inventory.Exchange(-90000, 4005000, -2, 4011000, -8, 4011006, -3, 4000030, -70, 4003000, -55, 1082103, 1);
                else if (index == 2) ret = inventory.Exchange(-100000, 4005000, -2, 4005002, -1, 4021005, -8, 4000030, -90, 4003000, -60, 1082114, 1);
                // making magician gloves 
                else if (index == 100) ret = inventory.Exchange(-70000, 4011007, -1, 4011001, -8, 4021007, -1, 4000030, -50, 4003000, -50, 1082086, 1);
                else if (index == 101) ret = inventory.Exchange(-90000, 4005001, -2, 4011000, -6, 4011004, -6, 4000030, -70, 4003000, -55, 1082098, 1);
                else if (index == 102) ret = inventory.Exchange(-100000, 4005001, -2, 4005003, -1, 4021003, -8, 4000030, -90, 4003000, -60, 1082121, 1);
                // making bowman gloves 
                else if (index == 200) ret = inventory.Exchange(-70000, 4011007, -1, 4021006, -8, 4011006, -2, 4000030, -50, 4003000, -50, 1082089, 1);
                else if (index == 201) ret = inventory.Exchange(-90000, 4005002, -2, 4021005, -8, 4011004, -3, 4000030, -70, 4003000, -55, 1082106, 1);
                else if (index == 202) ret = inventory.Exchange(-100000, 4005002, -2, 4005000, -1, 4021000, -8, 4000030, -90, 4003000, -60, 1082109, 1);
                // making thief gloves 
                else if (index == 300) ret = inventory.Exchange(-70000, 4011007, -1, 4011000, -8, 4021007, -1, 4000030, -50, 4003000, -50, 1082092, 1);
                else if (index == 301) ret = inventory.Exchange(-90000, 4005003, -2, 4011000, -6, 4011003, -6, 4000030, -70, 4003000, -55, 1082095, 1);
                else if (index == 302) ret = inventory.Exchange(-100000, 4005003, -2, 4005002, -1, 4011002, -8, 4000030, -90, 4003000, -60, 1082118, 1);
                // upgrading warrior gloves 
                else if (index == 400) ret = inventory.Exchange(-70000, 1082059, -1, 4011002, -3, 4021005, -5, 1082060, 1);
                else if (index == 401) ret = inventory.Exchange(-80000, 1082059, -1, 4021007, -2, 4021008, -2, 1082061, 1);
                else if (index == 402) ret = inventory.Exchange(-90000, 1082103, -1, 4011002, -6, 4021006, -4, 1082104, 1);
                else if (index == 403) ret = inventory.Exchange(-100000, 1082103, -1, 4021006, -8, 4021008, -3, 1082105, 1);
                else if (index == 404) ret = inventory.Exchange(-110000, 1082114, -1, 4005000, -1, 4005002, -1, 4021003, -7, 1082115, 1);
                else if (index == 405) ret = inventory.Exchange(-120000, 1082114, -1, 4005002, -3, 4021000, -8, 1082116, 1);
                else if (index == 406) ret = inventory.Exchange(-120000, 1082114, -1, 4005000, -2, 4005002, -1, 4021008, -4, 1082117, 1);
                // upgrading warrior gloves 
                else if (index == 500) ret = inventory.Exchange(-70000, 1082086, -1, 4011004, -3, 4011006, -5, 1082087, 1);
                else if (index == 501) ret = inventory.Exchange(-80000, 1082086, -1, 4021008, -2, 4011006, -3, 1082088, 1);
                else if (index == 502) ret = inventory.Exchange(-90000, 1082098, -1, 4021002, -6, 4021007, -2, 1082099, 1);
                else if (index == 503) ret = inventory.Exchange(-100000, 1082098, -1, 4021008, -3, 4011006, -3, 1082100, 1);
                else if (index == 504) ret = inventory.Exchange(-110000, 1082121, -1, 4005001, -1, 4005003, -1, 4021005, -7, 1082122, 1);
                else if (index == 505) ret = inventory.Exchange(-120000, 1082121, -1, 4005001, -2, 4005003, -1, 4021008, -4, 1082123, 1);
                // upgrading bowman gloves 
                else if (index == 600) ret = inventory.Exchange(-70000, 1082089, -1, 4021007, -1, 4021000, -5, 1082090, 1);
                else if (index == 601) ret = inventory.Exchange(-80000, 1082089, -1, 4021007, -2, 4021008, -2, 1082091, 1);
                else if (index == 602) ret = inventory.Exchange(-90000, 1082106, -1, 4021006, -5, 4011006, -3, 1082107, 1);
                else if (index == 603) ret = inventory.Exchange(-100000, 1082106, -1, 4021007, -2, 4021008, -3, 1082108, 1);
                else if (index == 604) ret = inventory.Exchange(-110000, 1082109, -1, 4005002, -1, 4005000, -1, 4021005, -7, 1082110, 1);
                else if (index == 605) ret = inventory.Exchange(-110000, 1082109, -1, 4005002, -1, 4005000, -1, 4021003, -7, 1082111, 1);
                else if (index == 606) ret = inventory.Exchange(-120000, 1082109, -1, 4005002, -2, 4005000, -1, 4021008, -4, 1082112, 1);
                // upgrading thief gloves 
                else if (index == 700) ret = inventory.Exchange(-70000, 1082092, -1, 4011001, -7, 4000014, -200, 1082093, 1);
                else if (index == 701) ret = inventory.Exchange(-80000, 1082092, -1, 4011006, -7, 4000027, -150, 1082094, 1);
                else if (index == 702) ret = inventory.Exchange(-90000, 1082095, -1, 4011004, -6, 4021007, -2, 1082096, 1);
                else if (index == 703) ret = inventory.Exchange(-100000, 1082095, -1, 4021007, -3, 4011006, -3, 1082097, 1);
                else if (index == 704) ret = inventory.Exchange(-110000, 1082118, -1, 4005003, -1, 4005002, -1, 4021001, -7, 1082119, 1);
                else if (index == 705) ret = inventory.Exchange(-120000, 1082118, -1, 4005003, -2, 4005002, -1, 4021000, -8, 1082120, 1);

                if (ret == 0) self.Say("Please make sure you have all items ready, and that your equipment inventory has enough space.");
                else self.Say("Here... here is the " + makeItem + " you asked for. What do you think? You won't find an item as well made as this one anywhere! Feel free to come back whenever you want...");
            }
        }
        public static void chat_message2(int index, string makeItem, string needItem, int reqLevel, string itemOption)
        {
            var inventory = target.Inventory;
            int ret = 0;

            var nRet = self.AskYesNo("You want to make " + makeItem + "? If so, I will need the following materials. There is an improvement of #r" + itemOption + "#k on this item. The Level Requirement is " + reqLevel + ". What do you think? You want to do it?\r\n\r\n#b" + needItem);
            if (nRet == 0) self.Say("Haha... after you leave town, you'll need shoes specially made for you. If you still need my help, just come back here.");
            else
            {
                // Warrior shoes 
                if (index == 0) ret = inventory.Exchange(-60000, 4021008, -1, 4011007, -1, 4021005, -8, 4000030, -80, 4003000, -55, 1072147, 1);
                else if (index == 1) ret = inventory.Exchange(-60000, 4021008, -1, 4011007, -1, 4011005, -8, 4000030, -80, 4003000, -55, 1072148, 1);
                else if (index == 2) ret = inventory.Exchange(-60000, 4021008, -1, 4011007, -1, 4021000, -8, 4000030, -80, 4003000, -55, 1072149, 1);
                else if (index == 3) ret = inventory.Exchange(-70000, 4005000, -1, 4005002, -3, 4011002, -5, 4000048, -100, 4003000, -60, 1072154, 1);
                else if (index == 4) ret = inventory.Exchange(-70000, 4005000, -2, 4005002, -2, 4011005, -5, 4000048, -100, 4003000, -60, 1072155, 1);
                else if (index == 5) ret = inventory.Exchange(-70000, 4005000, -3, 4005002, -1, 4021008, -1, 4000048, -100, 4003000, -60, 1072156, 1);
                else if (index == 6) ret = inventory.Exchange(-80000, 4005000, -2, 4005002, -3, 4021000, -7, 4000030, -90, 4003000, -65, 1072210, 1);
                else if (index == 7) ret = inventory.Exchange(-80000, 4005000, -3, 4005002, -2, 4021002, -7, 4000030, -90, 4003000, -65, 1072211, 1);
                else if (index == 8) ret = inventory.Exchange(-80000, 4005000, -4, 4005002, -1, 4021008, -2, 4000030, -90, 4003000, -65, 1072212, 1);
                // Wizard shoes 
                else if (index == 100) ret = inventory.Exchange(-60000, 4021009, -1, 4011006, -4, 4011005, -5, 4000030, -70, 4003000, -50, 1072136, 1);
                else if (index == 101) ret = inventory.Exchange(-60000, 4021009, -1, 4011006, -4, 4021003, -5, 4000030, -70, 4003000, -50, 1072137, 1);
                else if (index == 102) ret = inventory.Exchange(-60000, 4021009, -1, 4011006, -4, 4011003, -5, 4000030, -70, 4003000, -50, 1072138, 1);
                else if (index == 103) ret = inventory.Exchange(-60000, 4021009, -1, 4011006, -4, 4021002, -5, 4000030, -70, 4003000, -50, 1072139, 1);
                else if (index == 104) ret = inventory.Exchange(-70000, 4005001, -1, 4005003, -3, 4021002, -5, 4000051, -100, 4003000, -55, 1072157, 1);
                else if (index == 105) ret = inventory.Exchange(-70000, 4005001, -2, 4005003, -2, 4021000, -5, 4000051, -100, 4003000, -55, 1072158, 1);
                else if (index == 106) ret = inventory.Exchange(-70000, 4005001, -3, 4005003, -1, 4011003, -5, 4000051, -100, 4003000, -55, 1072159, 1);
                else if (index == 107) ret = inventory.Exchange(-70000, 4005001, -3, 4005003, -1, 4011006, -3, 4000051, -100, 4003000, -55, 1072160, 1);
                else if (index == 108) ret = inventory.Exchange(-80000, 4005001, -2, 4005003, -3, 4021003, -7, 4000030, -85, 4003000, -60, 1072177, 1);
                else if (index == 109) ret = inventory.Exchange(-80000, 4005001, -3, 4005003, -2, 4021001, -7, 4000030, -85, 4003000, -60, 1072178, 1);
                else if (index == 110) ret = inventory.Exchange(-80000, 4005001, -4, 4005003, -1, 4021008, -2, 4000030, -85, 4003000, -60, 1072179, 1);
                // bowman shoes 
                else if (index == 200) ret = inventory.Exchange(-60000, 4021007, -1, 4011006, -5, 4021000, -8, 4000030, -75, 4003000, -50, 1072144, 1);
                else if (index == 201) ret = inventory.Exchange(-60000, 4021007, -1, 4011006, -5, 4021005, -8, 4000030, -75, 4003000, -50, 1072145, 1);
                else if (index == 202) ret = inventory.Exchange(-60000, 4021007, -1, 4011006, -5, 4021003, -8, 4000030, -75, 4003000, -50, 1072146, 1);
                else if (index == 203) ret = inventory.Exchange(-70000, 4005002, -1, 4005000, -3, 4021005, -5, 4000055, -100, 4003000, -55, 1072164, 1);
                else if (index == 204) ret = inventory.Exchange(-70000, 4005002, -2, 4005000, -2, 4021004, -5, 4000055, -100, 4003000, -55, 1072165, 1);
                else if (index == 205) ret = inventory.Exchange(-70000, 4005002, -2, 4005000, -2, 4021003, -5, 4000055, -100, 4003000, -55, 1072166, 1);
                else if (index == 206) ret = inventory.Exchange(-70000, 4005002, -3, 4005000, -1, 4021008, -1, 4000055, -100, 4003000, -55, 1072167, 1);
                else if (index == 207) ret = inventory.Exchange(-80000, 4005002, -2, 4005000, -3, 4021002, -7, 4000030, -90, 4003000, -60, 1072182, 1);
                else if (index == 208) ret = inventory.Exchange(-80000, 4005002, -3, 4005000, -2, 4021000, -7, 4000030, -90, 4003000, -60, 1072183, 1);
                else if (index == 209) ret = inventory.Exchange(-80000, 4005002, -4, 4005000, -1, 4021003, -7, 4000030, -90, 4003000, -60, 1072184, 1);
                else if (index == 210) ret = inventory.Exchange(-80000, 4005002, -5, 4021008, -2, 4000030, -90, 4003000, -60, 1072185, 1);
                // thief shoes 
                else if (index == 300) ret = inventory.Exchange(-60000, 4021007, -1, 4011007, -1, 4021000, -8, 4000030, -75, 4003000, -50, 1072150, 1);
                else if (index == 301) ret = inventory.Exchange(-60000, 4021007, -1, 4011007, -1, 4011006, -5, 4000030, -75, 4003000, -50, 1072151, 1);
                else if (index == 302) ret = inventory.Exchange(-60000, 4021007, -1, 4011007, -1, 4021008, -1, 4000030, -75, 4003000, -50, 1072152, 1);
                else if (index == 303) ret = inventory.Exchange(-70000, 4005003, -1, 4005000, -3, 4021001, -5, 4000051, -100, 4003000, -55, 1072161, 1);
                else if (index == 304) ret = inventory.Exchange(-70000, 4005000, -1, 4005002, -3, 4021005, -5, 4000051, -100, 4003000, -55, 1072162, 1);
                else if (index == 305) ret = inventory.Exchange(-70000, 4005002, -1, 4005003, -3, 4021000, -5, 4000051, -100, 4003000, -55, 1072163, 1);
                else if (index == 306) ret = inventory.Exchange(-80000, 4005000, -3, 4005003, -2, 4021003, -7, 4000030, -90, 4003000, -60, 1072172, 1);
                else if (index == 307) ret = inventory.Exchange(-80000, 4005002, -3, 4005003, -2, 4021000, -7, 4000030, -90, 4003000, -60, 1072173, 1);
                else if (index == 308) ret = inventory.Exchange(-80000, 4005003, -3, 4005002, -2, 4021008, -7, 4000030, -90, 4003000, -60, 1072174, 1);

                if (ret == 0) self.Say("Alright, alright, alright... Please, make sure you have all the necessary materials, and that there is free space in your inventory.");
                else self.Say("Here, take the " + makeItem + "! What do you think? With these little beauties it looks like you won't have any problems when you're walking around? Right?");
            }
        }
        public static void chat_message6(int index, string makeItem, string needItemIcon, string needItemString, int unitPrice)
        {
            var inventory = target.Inventory;
            int ret = 0;

            if (index == 200 || index == 201)
            {
                var nRetNum = self.AskNumber("Very good, very good ... how many #b" + makeItem + "s#k would you like to make?", 1, 1, 100);
                var nPrice = unitPrice * nRetNum;
                var nRetBuy = self.AskYesNo("Alright, you wanna create #b" + nRetNum + " " + makeItem + "#k(s)?? For that you will need #r" + nPrice + "mesos and " + needItemIcon + " " + nRetNum + " " + needItemString + "#k each. What do you think? Do you really want to do it?");
                if (nRetBuy == 0) self.Say("I understand. Is the service fee too high for you? But understand that I'll be in this town for a long time, so if you ever want to refine anything just bring it to me.");
                else
                {
                    // a rare jewel 
                    if (index == 200) ret = inventory.Exchange(-10000, 4011000, -nRetNum, 4011001, -nRetNum, 4011002, -nRetNum, 4011003, -nRetNum, 4011004, -nRetNum, 4011005, -nRetNum, 4011006, -nRetNum, 4011007, nRetNum);
                    else if (index == 201) ret = inventory.Exchange(-15000, 4021000, -nRetNum, 4021001, -nRetNum, 4021002, -nRetNum, 4021003, -nRetNum, 4021004, -nRetNum, 4021005, -nRetNum, 4021006, -nRetNum, 4021007, -nRetNum, 4021008, -nRetNum, 4021009, nRetNum);

                    if (ret == 0) self.Say("Hmm... Please make sure you have all the necessary materials, and that you have some free space in your etc. inventory...");
                    else self.Say("Here! Take #b" + nRetNum + " " + makeItem + "#k(s). It's been 50 years, but I still have my skills. If you need my help in the near future, feel free to drop by.");
                }
            }
            else
            {
                var nRetNum = self.AskNumber("To make a " + makeItem + ", I will need the following materials. How many would you like to make?\r\n\r\n#b" + needItemIcon + " 10 " + needItemString + "\r\n" + unitPrice + " mesos#k", 1, 1, 100);
                var nPrice = unitPrice * nRetNum;
                var nAllNum = nRetNum * 10;
                var nRetBuy = self.AskYesNo("You want to make #b" + nRetNum + " " + makeItem + "(s)#k?? Then you will need #r" + nPrice + " mesos and " + needItemIcon + " " + nAllNum + " " + needItemString + "#k(s). What do you think? You wanna do it?");
                if (nRetBuy == 0) self.Say("I understand... Is the service fee too high for you? Know that I will be in this town for a long time, so if you ever want to refine anything just bring it to me.");
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

                    // crystal 
                    else if (index == 300) ret = inventory.Exchange(-nPrice, 4004000, -nAllNum, 4005000, nRetNum);
                    else if (index == 301) ret = inventory.Exchange(-nPrice, 4004001, -nAllNum, 4005001, nRetNum);
                    else if (index == 302) ret = inventory.Exchange(-nPrice, 4004002, -nAllNum, 4005002, nRetNum);
                    else if (index == 303) ret = inventory.Exchange(-nPrice, 4004003, -nAllNum, 4005003, nRetNum);
                    else if (index == 304) ret = inventory.Exchange(-nPrice, 4004004, -nAllNum, 4005004, nRetNum);

                    if (ret == 0) self.Say("Hmm... Please make sure you have all the necessary materials, and that you have some free space in your etc. inventory...");
                    else self.Say("Here! Take #b" + nRetNum + " " + makeItem + "(s)#k. It's been 50 years, but I still have my skills. If you need my help in the near future, feel free to drop by.");
                }
            }
        }
        public static void chat_message4(int index, string makeItem, string needItem, int unitNum, string itemOption)
        {
            var inventory = target.Inventory;
            int nRet, ret = 0;

            if (itemOption == "") nRet = self.AskYesNo("To make #b" + unitNum + " " + makeItem + "#ks, I will need the following materials. If you have the materials, it will be much better for you to create the item here than to buy it in the store. So, you want to make the item?\r\n\r\n#b" + needItem);
            else nRet = self.AskYesNo("You want to make #b" + unitNum + " " + makeItem + "(s)#k? Bring me the following items and I will make them for you. By the way, that arrow has a #r" + itemOption + "#k enhancement on it. It's a special kind of arrow, so it's not such a bad idea if you have the materials. You want to do it?\r\n\r\n#b" + needItem);
            if (nRet == 0) self.Say("We take items that are important to bowmen and make them at a low price, so take a good look around. You also know that nothing is FREE, right?");
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
        public static void chat_message3(int index, string makeItem, string needItem, int needNumber, int itemNumber)
        {
            var inventory = target.Inventory;
            int ret = 0;

            var nRetNum = self.AskNumber("I can make #b" + itemNumber + " " + makeItem + "(s) with " + needNumber + " " + needItem + "#k. This one is free, as long as you have the necessary materials, then you're good to go. What do you think? How many would you like to make?", 1, 1, 100);
            var nNeedNum = nRetNum * needNumber;
            var nAllNum = nRetNum * itemNumber;
            var nRetBuy = self.AskYesNo("Alright, you want to make #b" + makeItem + "#k " + nRetNum + " times? I'm going to need #r" + nNeedNum + " " + needItem + "#k to do it. Do you still want me to make them?");
            if (nRetBuy == 0) self.Say("Don't have the materials? You can get something by eliminating the monsters in this area, so work hard on this task...");
            else
            {
                if (index == 1) ret = inventory.Exchange(0, 4000003, -nNeedNum, 4003001, nAllNum);
                else if (index == 2) ret = inventory.Exchange(0, 4000018, -nNeedNum, 4003001, nAllNum);
                else if (index == 3) ret = inventory.Exchange(0, 4011001, -nNeedNum, 4011000, -nNeedNum, 4003000, nAllNum);
                if (ret == 0) self.Say("Hmm... Please make sure you have all the necessary materials, and that you have some free space in your etc. inventory...");
                else self.Say("Here! Take #b" + nAllNum + " " + makeItem + "#k! It's been 50 years, but I still have my skills. If you need my help in the near future, please feel free to drop by.");
            }
        }
        public static void chat_message1(int index, string makeItem, string needItem, int unitPrice)
        {
            var inventory = target.Inventory;
            int ret = 0;

            if (index == 200 || index == 201)
            {
                var nRetNum = self.AskNumber("Very good, very good ... how many #b" + makeItem + "s#k would you like to make?", 1, 1, 100);
                var nPrice = unitPrice * nRetNum;
                var nRetBuy = self.AskYesNo("Alright, you wanna create #b" + nRetNum + " " + makeItem + "#k(s)?? For that you will need #r" + nPrice + "mesos and " + nRetNum + " " + needItem + "#k each. What do you think? Do you really want to do it?");
                if (nRetBuy == 0) self.Say("I understand. Is the service fee too high for you? But understand that I'll be in this town for a long time, so if you ever want to refine anything just bring it to me.");
                else
                {
                    // a rare jewel 
                    if (index == 200) ret = inventory.Exchange(-10000 * nRetNum, 4011000, -nRetNum, 4011001, -nRetNum, 4011002, -nRetNum, 4011003, -nRetNum, 4011004, -nRetNum, 4011005, -nRetNum, 4011006, -nRetNum, 4011007, nRetNum);
                    else if (index == 201) ret = inventory.Exchange(-15000 * nRetNum, 4021000, -nRetNum, 4021001, -nRetNum, 4021002, -nRetNum, 4021003, -nRetNum, 4021004, -nRetNum, 4021005, -nRetNum, 4021006, -nRetNum, 4021007, -nRetNum, 4021008, -nRetNum, 4021009, nRetNum);

                    if (ret == 0) self.Say("Hmm... Please make sure you have all the necessary materials, and that you have some free space in your etc. inventory...");
                    else self.Say("Here! Take #b" + nRetNum + " " + makeItem + "#k(s). It's been 50 years, but I still have my skills. If you need my help in the near future, feel free to drop by.");
                }
            }
            else
            {
                var nRetNum = self.AskNumber("To make a " + makeItem + ", I will need the following materials. How many would you like to make?\r\n\r\n#b" + needItem + "s\r\n#v4031138#" + unitPrice + " mesos#k", 1, 1, 100);
                var nPrice = unitPrice * nRetNum;
                var nAllNum = nRetNum * 10;
                var nRetBuy = self.AskYesNo("You want to make #b" + nRetNum + " " + makeItem + "(s)#k?? Then you will need #r#v4031138# " + nPrice + " mesos and " + nAllNum + " " + needItem + "#k. What do you think? You wanna do it?");
                if (nRetBuy == 0) self.Say("I understand... Is the service fee too high for you? Know that I will be in this town for a long time, so if you ever want to refine anything just bring it to me.");
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

                    // crystal 
                    else if (index == 300) ret = inventory.Exchange(-nPrice, 4004000, -nAllNum, 4005000, nRetNum);
                    else if (index == 301) ret = inventory.Exchange(-nPrice, 4004001, -nAllNum, 4005001, nRetNum);
                    else if (index == 302) ret = inventory.Exchange(-nPrice, 4004002, -nAllNum, 4005002, nRetNum);
                    else if (index == 303) ret = inventory.Exchange(-nPrice, 4004003, -nAllNum, 4005003, nRetNum);
                    else if (index == 304) ret = inventory.Exchange(-nPrice, 4004004, -nAllNum, 4005004, nRetNum);

                    if (ret == 0) self.Say("Hmm... Please make sure you have all the necessary materials, and that you have some free space in your etc. inventory...");
                    else self.Say("Here! Take #b" + nRetNum + " " + makeItem + "(s)#k. It's been 50 years, but I still have my skills. If you need my help in the near future, feel free to drop by.");
                }
            }
        }
        // refining ores and jewels at El Nath 
        [Script("refine_elnath")]
        class refine_elnath : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                make3.self = self;
                make3.target = target;
                var nRet1 = self.AskYesNo("Looks like you have quite a bit of ores and jewels with you. For a small service fee, I can refine them into the materials needed to create shields or weapons. I've been doing this for 50 years, so it's a piece of cake! What do you think? You want me to do it?");
                if (nRet1 == 0) self.Say("I understand. Is the service fee too high for you? But understand that I'll be in this town for a long time, so if you ever want to refine anything just bring it to me.");
                else
                {
                    var v1 = self.AskMenu("Good decision! Give me the ores and the service fee, and I can refine them so that they'll be of some use. Before doing so, don't forget to check your etc. inventory to make sure that you have enough free space for the new items. Let's see, what would you like me to do?",
                        "Refine the ore of a mineral",
                        "Refine the ore of a jewel",
                        "Refine a rare gem",
                        "Refine a crystal",
                        "Create materials",
                        "Create arrows"
                    );
                    if (v1 == 0)
                    {
                        var v2 = self.AskMenu("Which of these minerais would you like to make?\r\n#b#L0##t4011000##l\r\n#L1##t4011001##l\r\n#L2##t4011002##l\r\n#L3##t4011003##l\r\n#L4##t4011004##l\r\n#L5##t4011005##l\r\n#L6##t4011006##l");
                        // DEBUG 
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
                        var v2 = self.AskMenu("Which jewel would you like to refine?\r\n#b#L0##t4021000##l\r\n#L1##t4021001##l\r\n#L2##t4021002##l\r\n#L3##t4021003##l\r\n#L4##t4021004##l\r\n#L5##t4021005##l\r\n#L6##t4021006##l\r\n#L7##t4021007##l\r\n#L8##t4021008##l");
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
                        var v2 = self.AskMenu("Yes, I can refine even rare gems. I may need a lot of material to do this, but it is possible. Which gem would you like to refine?\r\n#b#L0##t4011007##l\r\n#L1##t4021009##l#k");
                        if (v2 == 0) chat_message1(200, "#t4011007#", "refined #t4011000#, #t4011001#, #t4011002#, #t4011003#, #t4011004#, #t4011005#, #t4011006#", 10000);
                        else if (v2 == 1) chat_message1(201, "#t4021009#", "refined #t4021000#, #t4021001#, #t4021002#, #t4021003#, #t4021004#, #t4021005#, #t4021006#, #t4021007#, #t4021008#", 15000);
                    }
                    else if (v1 == 3)
                    {
                        var v2 = self.AskMenu("Hmmm... Do you really have a crystal? I haven't seen one of them in a while, so I don't really believe you, but if you really have one I can refine it and turn it into something useful. So, which crystal would you like to refine?\r\n#b#L0# #t4005000##l\r\n#L1# #t4005001##l\r\n#L2# #t4005002##l\r\n#L3# #t4005003##l\r\n#L4# #t4005004##l#k");
                        if (v2 == 0) chat_message6(300, "#t4005000#", "#v4004000#", "#t4004000#", 5000);
                        else if (v2 == 1) chat_message6(301, "#t4005001#", "#v4004001#", "#t4004001#", 5000);
                        else if (v2 == 2) chat_message6(302, "#t4005002#", "#v4004002#", "#t4004002#", 5000);
                        else if (v2 == 3) chat_message6(303, "#t4005003#", "#v4004003#", "#t4004003#", 5000);
                        else if (v2 == 4) chat_message6(304, "#t4005004#", "#v4004004#", "#t4004004#", 100000);
                    }
                    else if (v1 == 4)
                    {
                        var v2 = self.AskMenu("So, you want to create some materials! Let's see, what type of material would you like to make?\r\n#L0##bCreate #t4003001# with #t4000003#es#k#l\r\n#L1##bCreate #t4003001# with #t4000018#s#k#l\r\n#L2##bCreate #t4003000#s#k#l");
                        if (v2 == 0) chat_message3(1, "#t4003001#", "#t4000003#", 10, 1);
                        else if (v2 == 1) chat_message3(2, "#t4003001#", "#t4000018#s", 5, 1);
                        else if (v2 == 2) chat_message3(3, "#t4003000#s", "#t4011001#(s) and #t4011000#(s) each", 1, 15);
                    }
                    else if (v1 == 5)
                    {
                        var v2 = self.AskMenu("So, you want to create arrows! With a strong arrow, you will have a better advantage in battle. Let's see, what kind of arrow would you like me to create?\r\n#L0##b#t2060000##k#l\r\n#L1##b#t2061000##k#l\r\n#L2##b#t2060001##k#l\r\n#L3##b#t2061001##k#l\r\n#L4##b#t2060002##k#l\r\n#L5##b#t2061002##k#l");
                        if (v2 == 0) chat_message4(1, "#t2060000#", "#v4003001# #t4003001# \r\n#v4003004# #t4003004# ", 1000, "");
                        else if (v2 == 1) chat_message4(2, "#t2061000#", "#v4003001# #t4003001# \r\n#v4003004# #t4003004# ", 1000, "");
                        else if (v2 == 2) chat_message4(3, "#t2060001#", "#v4011000# #t4011000# \r\n#v4003001# 3 #t4003001#s \r\n#v4003004# 10 #t4003004#s ", 900, "Atk. +1");
                        else if (v2 == 3) chat_message4(4, "#t2061001#", "#v4011000# #t4011000# \r\n#v4003001# 3 #t4003001#s \r\n#v4003004# 10 #t4003004#s ", 900, "Atk. +1");
                        else if (v2 == 4) chat_message4(5, "#t2060002#", "#v4011001# #t4011001# \r\n#v4003001# 5 #t4003001#s \r\n#v4003005# 15 #t4003005#s ", 800, "Atk. +2");
                        else if (v2 == 5) chat_message4(6, "#t2061002#", "#v4011001# #t4011001# \r\n#v4003001# 5 #t4003001#s \r\n#v4003005# 15 #t4003005#s ", 800, "Atk. +2");
                    }
                }
            }
        }
        // Making shoes at El Nath 
        [Script("make_elnath")]
        class make_elnath : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                make3.self = self;
                make3.target = target;
                var v1 = self.AskMenu("In this city it's easy to obtain the necessary materials to create high-quality shoes. The leather and skins of the monsters that roam here make perfect materials for shoes. They are a little dangerous, but you won't have any problems... Would you be looking for someone to make you a perfect pair of shoes? If so, I can make it happen... What kind of shoes would you like me to make?",
                    "Create Warrior shoes",
                    "Create Magician shoes",
                    "Create Bowman shoes",
                    "Create Thief shoes"
                );
                if (v1 == 0)
                {
                    var v2 = self.AskMenu("So, you want shoes just for Warriors? What kind of shoes would you like to make?\r\n#b#L0##b #t1072147##k (Level Requirement: 60, Warrior)#l\r\n#L1##b #t1072148##k (Level Requirement: 60, Warrior)#l\r\n#L2##b #t1072149##k (Level Requirement: 60, Warrior)#l\r\n#L3##b #t1072154##k (Level Requirement: 70, Warrior)#l\r\n#L4##b #t1072155##k (Level Requirement: 70, Warrior)#l\r\n#L5##b #t1072156##k (Level Requirement: 70, Warrior)#l\r\n#L6##b #t1072210##k (Level Requirement: 80, Warrior)#l\r\n#L7##b #t1072211##k (Level Requirement: 80, Warrior)#l\r\n#L8##b #t1072212##k (Level Requirement: 80, Warrior)#l");
                    if (v2 == 0) chat_message2(0, "#t1072147#", "#v4021008# #t4021008# \r\n#v4011007# #t4011007# \r\n#v4021005# 8 #t4021005#s \r\n#v4000030# 80 #t4000030#s \r\n#v4003000# 55 #t4003000#s \r\n#v4031138# 60,000 mesos", 60, "STR +1, DEX +3");
                    else if (v2 == 1) chat_message2(1, "#t1072148#", "#v4021008# #t4021008# \r\n#v4011007# #t4011007# \r\n#v4011005# 8 #t4011005#s \r\n#v4000030# 80 #t4000030#s \r\n#v4003000# 55 #t4003000#s \r\n#v4031138# 60,000 mesos", 60, "STR + 2, DEX + 2");
                    else if (v2 == 2) chat_message2(2, "#t1072149#", "#v4021008# #t4021008# \r\n#v4011007# #t4011007# \r\n#v4021000# 8 #t4021000#s \r\n#v4000030# 80 #t4000030#s \r\n#v4003000# 55 #t4003000#s \r\n#v4031138# 60,000 mesos", 60, "STR + 3, DEX + 1");
                    else if (v2 == 3) chat_message2(3, "#t1072154#", "#v4005000# #t4005000# \r\n#v4005002# 3 #t4005002#s \r\n#v4011002# 5 #t4011002#s \r\n#v4000048# 100 #t4000048#s \r\n#v4003000# 60 #t4003000#s \r\n#v4031138# 70,000 mesos", 70, "STR +1, DEX +3, Accuracy +1");
                    else if (v2 == 4) chat_message2(4, "#t1072155#", "#v4005000# 2 #t4005000#s \r\n#v4005002# 2 #t4005002#s \r\n#v4011005# 5 #t4011005#s \r\n#v4000048# 100 #t4000048#s \r\n#v4003000# 60 #t4003000#s \r\n#v4031138# 70,000 mesos", 70, "STR + 2, DEX + 2, Accuracy + 1");
                    else if (v2 == 5) chat_message2(5, "#t1072156#", "#v4005000# 3 #t4005000#s \r\n#v4005002# #t4005002# \r\n#v4021008# #t4021008# \r\n#v4000048# 100 #t4000048#s \r\n#v4003000# 60 #t4003000#s \r\n#v4031138# 70,000 mesos", 70, "STR + 3, DEX + 1, Accuracy + 1");
                    else if (v2 == 6) chat_message2(6, "#t1072210#", "#v4005000# 2 #t4005000#s \r\n#v4005002# 3 #t4005002#s \r\n#v4021000# 7 #t4021000#s \r\n#v4000030# 90 #t4000030#s \r\n#v4003000# 65 #t4003000#s \r\n#v4031138# 80,000 mesos", 80, "STR +2, DEX +3");
                    else if (v2 == 7) chat_message2(7, "#t1072211#", "#v4005000# 3 #t4005000#s \r\n#v4005002# 2 #t4005002#s \r\n#v4021002# 7 #t4021002#s \r\n#v4000030# 90 #t4000030#s \r\n#v4003000# 65 #t4003000#s \r\n#v4031138# 80,000 mesos", 80, "STR +3, DEX +2");
                    else if (v2 == 8) chat_message2(8, "#t1072212#", "#v4005000# 4 #t4005000#s \r\n#v4005002# #t4005002# \r\n#v4021008# 2 #t4021008#s \r\n#v4000030# 90 #t4000030#s \r\n#v4003000# 65 #t4003000#s \r\n#v4031138# 80,000 mesos", 80, "STR +4, DEX +1");
                }
                else if (v1 == 1)
                {
                    var v2 = self.AskMenu("So, you want shoes just for Magicians? Then, what kind of shoes would you like to make?\r\n#b#L0##b #t1072136##k (Level Requirement: 60, Magician)#l\r\n#L1##b #t1072137##k (Level Requirement: 60, Magician)#l\r\n#L2##b #t1072138##k (Level Requirement: 60, Magician)#l\r\n#L3##b #t1072139##k (Level Requirement: 60, Magician)#l\r\n#L4##b #t1072157##k (Level Requirement: 70, Magician)#l\r\n#L5##b #t1072158##k (Level Requirement: 70, Magician)#l\r\n#L6##b #t1072159##k (Level Requirement: 70, Magician)#l\r\n#L7##b #t1072160##k (Level Requirement: 70, Magician)#l\r\n#L8##b #t1072177##k (Level Requirement: 80, Magician)#l\r\n#L9##b #t1072178##k (Level Requirement: 80, Magician)#l\r\n#L10##b #t1072179##k (Level Requirement: 80, Magician)#l");
                    if (v2 == 0) chat_message2(100, "#t1072136#", "#v4021009# #t4021009#\r\n#v4011006# 4 #t4011006#s\r\n#v4011005# 5 #t4011005#s\r\n#v4000030# 70 #t4000030#s\r\n#v4003000# 50 #t4003000#s\r\n#v4031138# 60,000 mesos", 60, "INT +1, LUK +3");
                    else if (v2 == 1) chat_message2(101, "#t1072137#", "#v4021009# #t4021009# \r\n#v4011006# 4 #t4011006#s \r\n#v4021003# 5 #t4021003#s \r\n#v4000030# 70 #t4000030#s \r\n#v4003000# 50 #t4003000#s \r\n#v4031138# 60,000 mesos", 60, "INT +2, LUK +2");
                    else if (v2 == 2) chat_message2(102, "#t1072138#", "#v4021009# #t4021009# \r\n#v4011006# 4 #t4011006#s \r\n#v4011003# 5 #t4011003#s \r\n#v4000030# 70 #t4000030#s \r\n#v4003000# 50 #t4003000#s \r\n#v4031138# 60,000 mesos", 60, "INT +3, LUK +1");
                    else if (v2 == 3) chat_message2(103, "#t1072139#", "#v4021009# #t4021009# \r\n#v4011006# 4 #t4011006#s \r\n#v4021002# 5 #t4021002#s \r\n#v4000030# 70 #t4000030#s \r\n#v4003000# 50 #t4003000#s \r\n#v4031138# 60,000 mesos", 60, "INT +3, LUK +1");
                    else if (v2 == 4) chat_message2(104, "#t1072157#", "#v4005001# #t4005001# \r\n#v4005003# 3 #t4005003#s \r\n#v4021002# 5 #t4021002#s \r\n#v4000051# 100 #t4000051#s \r\n#v4003000# 55 #t4003000#s \r\n#v4031138# 70,000 mesos", 70, "INT +1, LUK +3, Magic Atk. +1");
                    else if (v2 == 5) chat_message2(105, "#t1072158#", "#v4005001# 2 #t4005001#s \r\n#v4005003# 2 #t4005003#s \r\n#v4021000# 5 #t4021000#s \r\n#v4000051# 100 #t4000051#s \r\n#v4003000# 55 #t4003000#s \r\n#v4031138# 70,000 mesos", 70, "INT + 2, LUK + 2, Magic Atk. +1");
                    else if (v2 == 6) chat_message2(106, "#t1072159#", "#v4005001# 3 #t4005001#s \r\n#v4005003# #t4005003# \r\n#v4011003# 5 #t4011003#s \r\n#v4000051# 100 #t4000051#s \r\n#v4003000# 55 #t4003000#s \r\n#v4031138# 70,000 mesos", 70, "INT + 3, LUK + 1, Magic Atk. +1");
                    else if (v2 == 7) chat_message2(107, "#t1072160#", "#v4005001# 3 #t4005001#s \r\n#v4005003# #t4005003# \r\n#v4011006# 3 #t4011006#s \r\n#v4000051# 100 #t4000051#s \r\n#v4003000# 55 #t4003000#s \r\n#v4031138# 70,000 mesos", 70, "INT + 3, LUK + 1, Magic Atk. +1");
                    else if (v2 == 8) chat_message2(108, "#t1072177#", "#v4005001# 2 #t4005001#s \r\n#v4005003# 3 #t4005003#s \r\n#v4021003# 7 #t4021003#s \r\n#v4000030# 85 #t4000030#s \r\n#v4003000# 60 #t4003000#s \r\n#v4031138# 80,000 mesos", 80, "INT +2, LUK +3");
                    else if (v2 == 9) chat_message2(109, "#t1072178#", "#v4005001# 3 #t4005001#s \r\n#v4005003# 2 #t4005003#s \r\n#v4021001# 7 #t4021001#s \r\n#v4000030# 85 #t4000030#s \r\n#v4003000# 60 #t4003000#s \r\n#v4031138# 80,000 mesos", 80, "INT +3, LUK +2");
                    else if (v2 == 10) chat_message2(110, "#t1072179#", "#v4005001# 4 #t4005001#s \r\n#v4005003# #t4005003# \r\n#v4021008# 2 #t4021008#s \r\n#v4000030# 85 #t4000030#s \r\n#v4003000# 60 #t4003000#s \r\n#v4031138# 80,000 mesos", 80, "INT +4, LUK +1");
                }
                else if (v1 == 2)
                {
                    var v2 = self.AskMenu("So, you want shoes just for Bowmen? Then, what kind of shoes would you like to make?\r\n#b#L0##b #t1072144##k (Level Requirement: 60, Bowman)#l\r\n#L1##b #t1072145##k (Level Requirement: 60, Bowman)#l\r\n#L2##b #t1072146##k (Level Requirement: 60, Bowman)#l\r\n#L3##b #t1072164##k (Level Requirement: 70, Bowman)#l\r\n#L4##b #t1072165##k (Level Requirement: 70, Bowman)#l\r\n#L5##b #t1072166##k (Level Requirement: 70, Bowman)#l\r\n#L6##b #t1072167##k (Level Requirement: 70, Bowman)#l\r\n#L7##b #t1072182##k (Level Requirement: 80, Bowman)#l\r\n#L8##b #t1072183##k (Level Requirement: 80, Bowman)#l\r\n#L9##b #t1072184##k (Level Requirement: 80, Bowman)#l\r\n#L10##b #t1072185##k (Level Requirement: 80, Bowman)#l");
                    if (v2 == 0) chat_message2(200, "#t1072144#", "#v4021007# #t4021007#s\r\n#v4011006# 5 #t4011006#s\r\n#v4021000# 8 #t4021000#s\r\n#v4000030# 75 #t4000030#s\r\n#v4003000# 50 #t4003000#s \r\n#v4031138# 60,000 mesos", 60, "DEX +1, STR +3");
                    else if (v2 == 1) chat_message2(201, "#t1072145#", "#v4021007# #t4021007# \r\n#v4011006# 5 #t4011006#s \r\n#v4021005# 8 #t4021005#s \r\n#v4000030# 75 #t4000030#s \r\n#v4003000# 50 #t4003000#s \r\n#v4031138# 60,000 mesos", 60, "DEX +1, STR +3");
                    else if (v2 == 2) chat_message2(202, "#t1072146#", "#v4021007# #t4021007# \r\n#v4011006# 5 #t4011006#s \r\n#v4021003# 8 #t4021003#s \r\n#v4000030# 75 #t4000030#s \r\n#v4003000# 50 #t4003000#s \r\n#v4031138# 60,000 mesos", 60, "DEX +1, STR +3");
                    else if (v2 == 3) chat_message2(203, "#t1072164#", "#v4005002# #t4005002# \r\n#v4005000# 3 #t4005000#s \r\n#v4021005# 5 #t4021005#s \r\n#v4000055# 100 #t4000055#s \r\n#v4003000# 55 #t4003000#s \r\n#v4031138# 70,000 mesos", 70, "DEX +1, STR +3, Avoidability +1");
                    else if (v2 == 4) chat_message2(204, "#t1072165#", "#v4005002# 2 #t4005002#s \r\n#v4005000# 2 #t4005000#s \r\n#v4021004# 5 #t4021004#s \r\n#v4000055# 100 #t4000055#s \r\n#v4003000# 55 #t4003000#s \r\n#v4031138# 70,000 mesos", 70, "DEX + 2, STR + 2, Avoidability + 1");
                    else if (v2 == 5) chat_message2(205, "#t1072166#", "#v4005002# 2 #t4005002#s \r\n#v4005000# 2 #t4005000#s \r\n#v4021003# 5 #t4021003#s \r\n#v4000055# 100 #t4000055#s \r\n#v4003000# 55 #t4003000#s \r\n#v4031138# 70,000 mesos", 70, "DEX + 2, STR + 2, Avoidability + 1");
                    else if (v2 == 6) chat_message2(206, "#t1072167#", "#v4005002# 3 #t4005002#s \r\n#v4005000# #t4005000# \r\n#v4021008# #t4021008# \r\n#v4000055# 100 #t4000055#s \r\n#v4003000# 55 #t4003000#s \r\n#v4031138# 70,000 mesos", 70, "DEX + 3, STR + 1, Avoidability + 1");
                    else if (v2 == 7) chat_message2(207, "#t1072182#", "#v4005002# 2 #t4005002#s \r\n#v4005000# 3#t4005000#s \r\n#v4021002# 7 #t4021002#s \r\n#v4000030# 90 #t4000030#s \r\n#v4003000# 60 #t4003000#s \r\n#v4031138# 80,000 mesos", 80, "DEX +2, STR +3");
                    else if (v2 == 8) chat_message2(208, "#t1072183#", "#v4005002# 3 #t4005002#s \r\n#v4005000# 2#t4005000#s \r\n#v4021000# 7 #t4021000#s \r\n#v4000030# 90 #t4000030#s \r\n#v4003000# 60 #t4003000#s \r\n#v4031138# 80,000 mesos", 80, "DEX + 3, STR + 2");
                    else if (v2 == 9) chat_message2(209, "#t1072184#", "#v4005002# 4 #t4005002#s \r\n#v4005000# #t4005000# \r\n#v4021003# 7 #t4021003#s \r\n#v4000030# 90 #t4000030#s \r\n#v4003000# 60 #t4003000#s \r\n#v4031138# 80,000 mesos", 80, "DEX + 4, STR + 1");
                    else if (v2 == 10) chat_message2(210, "#t1072185#", "#v4005002# 5 #t4005002#s \r\n#v4021008# 2 #t4021008#s \r\n#v4000030# 90 #t4000030#s \r\n#v4003000# 60 #t4003000#s \r\n#v4031138# 80,000 mesos", 80, "DEX +5");
                }
                else if (v1 == 3)
                {
                    var v2 = self.AskMenu("So, you want shoes just for Thieves? Then, what kind of shoes would you like to make?\r\n#b#L0##b #t1072150##k (Level Requirement: 60, Thief)#l\r\n#L1##b #t1072151##k (Level Requirement: 60, Thief)#l\r\n#L2##b #t1072152##k (Level Requirement: 60, Thief)#l\r\n#L3##b #t1072161##k (Level Requirement: 70, Thief)#l\r\n#L4##b #t1072162##k (Level Requirement: 70, Thief)#l\r\n#L5##b #t1072163##k (Level Requirement: 70, Thief)#l\r\n#L6##b #t1072172##k (Level Requirement: 80, Thief)#l\r\n#L7##b #t1072173##k (Level Requirement: 80, Thief)#l\r\n#L8##b #t1072174##k (Level Requirement: 80, Thief)#l");
                    if (v2 == 0) chat_message2(300, "#t1072150#", "#v4021007# #t4021007# \r\n#v4011007# #t4011007# \r\n#v4021000# 8 #t4021000#s \r\n#v4000030# 75 #t4000030#s \r\n#v4003000# 50 #t4003000#s \r\n#v4031138# 60,000 mesos", 60, "LUK + 1, STR + 3");
                    else if (v2 == 1) chat_message2(301, "#t1072151#", "#v4021007# #t4021007# \r\n#v4011007# #t4011007# \r\n#v4011006# 5 #t4011006#s \r\n#v4000030# 75 #t4000030#s \r\n#v4003000# 50 #t4003000#s \r\n#v4031138# 60,000 mesos", 60, "STR +1, DEX +3");
                    else if (v2 == 2) chat_message2(302, "#t1072152#", "#v4021007# #t4021007# \r\n#v4011007# #t4011007# \r\n#v4021008# #t4021008# \r\n#v4000030# 75 #t4000030#s \r\n#v4003000# 50 #t4003000#s \r\n#v4031138# 60,000 mesos", 60, "DEX + 1, LUK + 3");
                    else if (v2 == 3) chat_message2(303, "#t1072161#", "#v4005003# #t4005003# \r\n#v4005000# 3 #t4005000#s \r\n#v4021001# 5 #t4021001#s \r\n#v4000051# 100 #t4000051#s \r\n#v4003000# 55 #t4003000#s \r\n#v4031138# 70,000 mesos", 70, "LUK + 1, STR + 3, Avoidability + 1");
                    else if (v2 == 4) chat_message2(304, "#t1072162#", "#v4005000# #t4005000# \r\n#v4005002# 3 #t4005002#s \r\n#v4021005# 5 #t4021005#s \r\n#v4000051# 100 #t4000051#s \r\n#v4003000# 55 #t4003000#s \r\n#v4031138# 70,000 mesos", 70, "STR + 1, DEX + 3, Avoidability + 1");
                    else if (v2 == 5) chat_message2(305, "#t1072163#", "#v4005002# #t4005002# \r\n#v4005003# 3 #t4005003#s \r\n#v4021000# 5 #t4021000#s \r\n#v4000051# 100 #t4000051#s \r\n#v4003000# 55 #t4003000#s \r\n#v4031138# 70,000 mesos", 70, "DEX + 1, LUK + 3, Avoidability + 1");
                    else if (v2 == 6) chat_message2(306, "#t1072172#", "#v4005000# 3 #t4005000#s \r\n#v4005003# 2 #t4005003#s \r\n#v4021003# 7 #t4021003#s \r\n#v4000030# 90 #t4000030#s \r\n#v4003000# 60 #t4003000#s \r\n#v4031138# 80,000 mesos", 80, "STR + 3, LUK + 2");
                    else if (v2 == 7) chat_message2(307, "#t1072173#", "#v4005002# 3 #t4005002#s \r\n#v4005003# 2 #t4005003#s \r\n#v4021000# 7 #t4021000#s \r\n#v4000030# 90 #t4000030#s \r\n#v4003000# 60 #t4003000#s \r\n#v4031138# 80,000 mesos", 80, "DEX + 3, LUK + 2");
                    else if (v2 == 8) chat_message2(308, "#t1072174#", "#v4005003# 3 #t4005003#s \r\n#v4005002# 2 #t4005002#s \r\n#v4021008# 7 #t4021008#s \r\n#v4000030# 90 #t4000030#s \r\n#v4003000# 60 #t4003000#s \r\n#v4031138# 80,000 mesos", 80, "LUK +3, DEX +2");
                }
            }
        }
        // making gloves at Orbis 
        [Script("make_orbis")]
        class make_orbis : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                make3.self = self;
                make3.target = target;
                var v1 = self.AskMenu("It seems that people today don't consider gloves that important... That's because they don't understand the true value of a good glove. What do you think? If you need one, how about you let me handle it?",
                    "Create a Warrior glove",
                    "Create a Magician glove",
                    "Create a Bowman glove",
                    "Create a Thief glove",
                    "Upgrade a Warrior glove",
                    "Upgrade a Magician glove",
                    "Upgrade a Bowman glove",
                    "Upgrade a Thief glove"
                );
                if (v1 == 0)
                {
                    var v2 = self.AskMenu("So, you want gloves just for Warriors? Here, choose the one that meets your needs!\r\n#b#L0##b #t1082059##k (Level Requirement: 60, Warrior)#l\r\n#b#L1##b #t1082103##k (Level Requirement: 70, Warrior)#l\r\n#b#L2##b #t1082114##k (Level Requirement: 80, Warrior)#l");
                    if (v2 == 0) chat_message5(0, "#t1082059#", "#v4011007# #t4011007# \r\n#v4011000# 8 #t4011000#s \r\n#v4011006# 2 #t4011006#s \r\n#v4000030# 50 #t4000030#s \r\n#v4003000# 50 #t4003000#s \r\n#v4031138# 70,000 mesos", 60, "STR +1");
                    else if (v2 == 1) chat_message5(1, "#t1082103#", "#v4005000# 2 #t4005000#s \r\n#v4011000# 8 #t4011000#s \r\n#v4011006# 3 #t4011006#s \r\n#v4000030# 70 #t4000030#s \r\n#v4003000# 55 #t4003000#s \r\n#v4031138# 90,000 mesos", 70, "STR +2");
                    else if (v2 == 2) chat_message5(2, "#t1082114#", "#v4005000# 2 #t4005000#s \r\n#v4005002# #t4005002# \r\n#v4021005# 8 #t4021005#s \r\n#v4000030# 90 #t4000030#s \r\n#v4003000# 60 #t4003000#s \r\n#v4031138# 100,000 mesos", 80, "STR +2, DEX +1, MP +10");
                }
                else if (v1 == 1)
                {
                    var v2 = self.AskMenu("So, you want gloves just for Magicians? Here, choose the one that meets your needs!\r\n#b#L0##b #t1082086##k (Level Requirement: 60, Magician)#l\r\n#b#L1##b #t1082098##k (Level Requirement: 70, Magician)#l\r\n#b#L2##b #t1082121##k (Level Requirement: 80, Magician)#l");
                    if (v2 == 0) chat_message5(100, "#t1082086#", "#v4011007# #t4011007# \r\n#v4011001# 8 #t4011001#s \r\n#v4021007# #t4021007# \r\n#v4000030# 50 #t4000030#s \r\n#v4003000# 50 #t4003000#s \r\n#v4031138# 70,000 mesos", 60, "INT + 1, LUK + 1, MP + 30");
                    else if (v2 == 1) chat_message5(101, "#t1082098#", "#v4005001# 2 #t4005001#s \r\n#v4011000# 6 #t4011000#s \r\n#v4011004# 6 #t4011004#s \r\n#v4000030# 70 #t4000030#s \r\n#v4003000# 55 #t4003000#s \r\n#v4031138# 90,000 mesos", 70, "INT +2");
                    else if (v2 == 2) chat_message5(102, "#t1082121#", "#v4005001# 2 #t4005001#s \r\n#v4005003# #t4005003# \r\n#v4021003# 8 #t4021003#s \r\n#v4000030# 90 #t4000030#s \r\n#v4003000# 60 #t4003000#s \r\n#v4031138# 100,000 mesos", 80, "INT +2, LUK +1, MP +10");
                }
                else if (v1 == 2)
                {
                    var v2 = self.AskMenu("So, you want gloves just for Bowmen? Here, choose the one that meets your needs!\r\n#b#L0##b #t1082089##k (Level Requirement: 60, Bowman)#l\r\n#b#L1##b #t1082106##k (Level Requirement: 70, Bowman)#l\r\n#b#L2##b #t1082109##k (Level Requirement: 80, Bowman)#l");
                    if (v2 == 0) chat_message5(200, "#t1082089#", "#v4011007# #t4011007# \r\n#v4021006# 8 #t4021006#s \r\n#v4011006# 2 #t4011006#s \r\n#v4000030# 50 #t4000030#s \r\n#v4003000# 50 #t4003000#s \r\n#v4031138# 70,000 mesos", 60, "DEX + 1, STR + 1");
                    else if (v2 == 1) chat_message5(201, "#t1082106#", "#v4005002# 2 #t4005002#s \r\n#v4021005# 8 #t4021005#s \r\n#v4011004# 3 #t4011004#s \r\n#v4000030# 70 #t4000030#s \r\n#v4003000# 55 #t4003000#s \r\n#v4031138# 90,000 mesos", 70, "DEX +2");
                    else if (v2 == 2) chat_message5(202, "#t1082109#", "#v4005002# 2 #t4005002#s \r\n#v4005000# #t4005000# \r\n#v4021000# 8 #t4021000#s \r\n#v4000030# 90 #t4000030#s \r\n#v4003000# 60 #t4003000#s \r\n#v4031138# 100,000 mesos", 80, "DEX +2, STR +1, MP +10");
                }
                else if (v1 == 3)
                {
                    var v2 = self.AskMenu("So, you want gloves just for Thieves? Here, choose the one that meets your needs!\r\n#b#L0##b #t1082092##k (Level Requirement: 60, Thief)#l\r\n#b#L1##b #t1082095##k (Level Requirement: 70, Thief)#l\r\n#b#L2##b #t1082118##k (Level Requirement: 80, Thief)#l");
                    if (v2 == 0) chat_message5(300, "#t1082092#", "#v4011007# #t4011007# \r\n#v4011000# 8 #t4011000#s \r\n#v4021007# #t4021007# \r\n#v4000030# 50 #t4000030#s \r\n#v4003000# 50 #t4003000#s \r\n#v4031138# 70,000 mesos", 60, "LUK +1, DEX +1");
                    else if (v2 == 1) chat_message5(301, "#t1082095#", "#v4005003# 2 #t4005003#s \r\n#v4011000# 6 #t4011000#s \r\n#v4011003# 6 #t4011003#s \r\n#v4000030# 70 #t4000030#s \r\n#v4003000# 55 #t4003000#s \r\n#v4031138# 90,000 mesos", 70, "LUK +2");
                    else if (v2 == 2) chat_message5(302, "#t1082118#", "#v4005003# 2 #t4005003#s \r\n#v4005002# #t4005002# \r\n#v4011002# 8 #t4011002#s \r\n#v4000030# 90 #t4000030#s \r\n#v4003000# 60 #t4003000#s \r\n#v4031138# 100,000 mesos", 80, "LUK +2, DEX +1, MP +10");
                }
                else if (v1 == 4)
                {
                    var v2 = self.AskMenu("So, you want to upgrade your Warrior gloves, right? Choose the one you want to improve...\r\n#b#L0##b #t1082060##k (Level Requirement: 60, Warrior)#l\r\n#b#L1##b #t1082061##k (Level Requirement: 60, Warrior)#l\r\n#b#L2##b #t1082104##k (Level Requirement: 70, Warrior)#l\r\n#b#L3##b #t1082105##k (Level Requirement: 70, Warrior)#l\r\n#b#L4##b #t1082115##k (Level Requirement: 80, Warrior)#l\r\n#b#L5##b #t1082116##k (Level Requirement: 80, Warrior)#l\r\n#b#L6##b #t1082117##k (Level Requirement: 80, Warrior)#l");
                    if (v2 == 0) chat_message5(400, "#t1082060#", "#v1082059# #t1082059# \r\n#v4011002# 3 #t4011002#s \r\n#v4021005# 5 #t4021005#s \r\n#v4031138# 70,000 mesos", 60, "STR + 2, Accuracy + 1");
                    else if (v2 == 1) chat_message5(401, "#t1082061#", "#v1082059# #t1082059# \r\n#v4021007# 2 #t4021007#s \r\n#v4021008# 2 #t4021008#s \r\n#v4031138# 80,000 mesos", 60, "STR + 3, Accuracy + 2");
                    else if (v2 == 2) chat_message5(402, "#t1082104#", "#v1082103# #t1082103# \r\n#v4011002# 6 #t4011002#s \r\n#v4021006# 4 #t4021006#s \r\n#v4031138# 90,000 mesos", 70, "STR + 3, Accuracy + 1");
                    else if (v2 == 3) chat_message5(403, "#t1082105#", "#v1082103# #t1082103# \r\n#v4021006# 8 #t4021006#s \r\n#v4021008# 3 #t4021008#s \r\n#v4031138# 100,000 mesos", 70, "STR + 4, Accuracy + 1");
                    else if (v2 == 4) chat_message5(404, "#t1082115#", "#v1082114# #t1082114# \r\n#v4005000# #t4005000# \r\n#v4005002# #t4005002# \r\n#v4021003# 7 #t4021003#s \r\n#v4031138# 110,000 mesos", 80, "STR +3, DEX +2, MP +15");
                    else if (v2 == 5) chat_message5(405, "#t1082116#", "#v1082114# #t1082114# \r\n#v4005002# 3 #t4005002#s \r\n#v4021000# 8 #t4021000#s \r\n#v4031138# 120,000 mesos", 80, "DEX +4, STR +2, MP +20");
                    else if (v2 == 6) chat_message5(406, "#t1082117#", "#v1082114# #t1082114# \r\n#v4005000# 2 #t4005000#s \r\n#v4005002# #t4005002# \r\n#v4021008# 4 #t4021008#s \r\n#v4031138# 120,000 mesos", 80, "STR +4, DEX +2, MP +20");
                }
                else if (v1 == 5)
                {
                    var v2 = self.AskMenu("So, you want to upgrade your Magician gloves, right? Choose the one you want to improve...\r\n#b#L0##b #t1082087##k (Level Requirement: 60, Magician)#l\r\n#b#L1##b #t1082088##k (Level Requirement: 60, Magician)#l\r\n#b#L2##b #t1082099##k (Level Requirement: 70, Magician)#l\r\n#b#L3##b #t1082100##k (Level Requirement: 70, Magician)#l\r\n#b#L4##b #t1082122##k (Level Requirement: 80, Magician)#l\r\n#b#L5##b #t1082123##k (Level Requirement: 80, Magician)#l");
                    if (v2 == 0) chat_message5(500, "#t1082087#", "#v1082086# #t1082086# \r\n#v4011004# 3 #t4011004#s \r\n#v4011006# 5 #t4011006#s \r\n#v4031138# 70,000 mesos", 60, "INT +2, LUK +1, MP +30");
                    else if (v2 == 1) chat_message5(501, "#t1082088#", "#v1082086# #t1082086# \r\n#v4021008# 2 #t4021008#s \r\n#v4011006# 3 #t4011006#s \r\n#v4031138# 80,000 mesos", 60, "INT + 3, LUK + 1, MP + 30");
                    else if (v2 == 2) chat_message5(502, "#t1082099#", "#v1082098# #t1082098# \r\n#v4021002# 6 #t4021002#s \r\n#v4021007# 2 #t4021007#s \r\n#v4031138# 90,000 mesos", 70, "INT +3, LUK +1");
                    else if (v2 == 3) chat_message5(503, "#t1082100#", "#v1082098# #t1082098# \r\n#v4021008# 3 #t4021008#s \r\n#v4011006# 3 #t4011006#s \r\n#v4031138# 100,000 mesos", 70, "INT +4, LUK +1");
                    else if (v2 == 4) chat_message5(504, "#t1082122#", "#v1082121# #t1082121# \r\n#v4005001# #t4005001# \r\n#v4005003# #t4005003# \r\n#v4021005# 7 #t4021005#s \r\n#v4031138# 110,000 mesos", 80, "INT + 3, LUK + 2, MP + 15");
                    else if (v2 == 5) chat_message5(505, "#t1082123#", "#v1082121# #t1082121# \r\n#v4005001# 2 #t4005001#s \r\n#v4005003# #t4005003# \r\n#v4021008# 4 #t4021008#s \r\n#v4031138# 120,000 mesos", 80, "INT + 4, LUK + 2, MP + 30");
                }
                else if (v1 == 6)
                {
                    var v2 = self.AskMenu("So, you want to upgrade your Bowman gloves, right? Choose the one you want to improve...\r\n#b#L0##b #t1082090##k (Level Requirement: 60, Bowman)#l\r\n#b#L1##b #t1082091##k (Level Requirement: 60, Bowman)#l\r\n#b#L2##b #t1082107##k (Level Requirement: 70, Bowman)#l\r\n#b#L3##b #t1082108##k (Level Requirement: 70, Bowman)#l\r\n#b#L4##b #t1082110##k (Level Requirement: 80, Bowman)#l\r\n#b#L5##b #t1082111##k (Level Requirement: 80, Bowman)#l\r\n#b#L6##b #t1082112##k (Level Requirement: 80, Bowman)#l");
                    if (v2 == 0) chat_message5(600, "#t1082090#", "#v1082089# #t1082089# \r\n#v4021007# #t4021007# \r\n#v4021000# 5 #t4021000#s \r\n#v4031138# 70,000 mesos", 60, "DEX +2, STR +1, MP +15");
                    else if (v2 == 1) chat_message5(601, "#t1082091#", "#v1082089# #t1082089# \r\n#v4021007# 2 #t4021007#s \r\n#v4021008# 2 #t4021008#s \r\n#v4031138# 80,000 mesos", 60, "DEX +3, STR +1, MP +30");
                    else if (v2 == 2) chat_message5(602, "#t1082107#", "#v1082106# #t1082106# \r\n#v4021006# 5 #t4021006#s \r\n#v4011006# 3 #t4011006#s \r\n#v4031138# 90,000 mesos", 70, "DEX + 3, Avoidability + 1");
                    else if (v2 == 3) chat_message5(603, "#t1082108#", "#v1082106# #t1082106# \r\n#v4021007# 2 #t4021007#s \r\n#v4021008# 3 #t4021008#s \r\n#v4031138# 100,000 mesos", 70, "DEX + 4, Avoidability + 1");
                    else if (v2 == 4) chat_message5(604, "#t1082110#", "#v1082109# #t1082109# \r\n#v4005002# #t4005002# \r\n#v4005000# #t4005000# \r\n#v4021005# 7 #t4021005#s \r\n#v4031138# 110,000 mesos", 80, "DEX +3, STR +2, MP +15");
                    else if (v2 == 5) chat_message5(605, "#t1082111#", "#v1082109# #t1082109# \r\n#v4005002# #t4005002# \r\n#v4005000# #t4005000# \r\n#v4021003# 7 #t4021003#s \r\n#v4031138# 110,000 mesos", 80, "DEX +3, STR +2, MP +15");
                    else if (v2 == 6) chat_message5(606, "#t1082112#", "#v1082109# #t1082109# \r\n#v4005002# 2 #t4005002#s \r\n#v4005000# #t4005000# \r\n#v4021008# 4 #t4021008#s \r\n#v4031138# 120,000 mesos", 80, "DEX +4, STR +2, MP +30");
                }
                else if (v1 == 7)
                {
                    var v2 = self.AskMenu("So, you want to upgrade your Thief gloves, right? Choose the one you want to improve...\r\n#b#L0##b #t1082093##k (Level Requirement: 60, Thief)#l\r\n#b#L1##b #t1082094##k (Level Requirement: 60, Thief)#l\r\n#b#L2##b #t1082096##k (Level Requirement: 70, Thief)#l\r\n#b#L3##b #t1082097##k (Level Requirement: 70, Thief)#l\r\n#b#L4##b #t1082119##k (Level Requirement: 80, Thief)#l\r\n#b#L5##b #t1082120##k (Level Requirement: 80, Thief)#l");
                    if (v2 == 0) chat_message5(700, "#t1082093#", "#v1082092# #t1082092# \r\n#v4011001# #t4011001# \r\n#v4000014# 200 #t4000014#s \r\n#v4031138# 70,000 mesos", 60, "LUK +2, DEX +1, MP +15");
                    else if (v2 == 1) chat_message5(701, "#t1082094#", "#v1082092# #t1082092# \r\n#v4011006# 7 #t4011006#s \r\n#v4000027# 150 #t4000027#s \r\n#v4031138# 80,000 mesos", 60, "LUK +3, DEX +1, MP +30");
                    else if (v2 == 2) chat_message5(702, "#t1082096#", "#v1082095# #t1082095# \r\n#v4011004# 6 #t4011004#s \r\n#v4021007# 2 #t4021007#s \r\n#v4031138# 90,000 mesos", 70, "LUK + 3, EVA + 1");
                    else if (v2 == 3) chat_message5(703, "#t1082097#", "#v1082095# #t1082095# \r\n#v4021007# 3 #t4021007#s \r\n#v4011006# 3 #t4011006#s \r\n#v4031138# 100,000 mesos", 70, "LUK + 4, EVA + 1");
                    else if (v2 == 4) chat_message5(704, "#t1082119#", "#v1082118# #t1082118# \r\n#v4005003# #t4005003# \r\n#v4005002# #t4005002# \r\n#v4021001# 7 #t4021001#s \r\n#v4031138# 110,000 mesos", 80, "LUK +3, DEX +2, MP +15");
                    else if (v2 == 5) chat_message5(705, "#t1082120#", "#v1082118# #t1082118# \r\n#v4005003# 2 #t4005003#s \r\n#v4005002# #t4005002# \r\n#v4021000# 8 #t4021000#s \r\n#v4031138# 120,000 mesos", 80, "LUK +4, DEX +2, MP +30");
                }
            }
        }
    }
}
