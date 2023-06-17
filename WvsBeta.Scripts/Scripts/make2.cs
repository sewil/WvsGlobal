using System;
using WvsBeta.Game;
using WvsBeta.Game.Scripting;

namespace WvsBeta.Scripts.Scripts
{
    public static class make2
    {
        static INpcHost self;
        static GameCharacter target;
        public static void chat_message4(int index, string makeItem, string needItem, int reqLevel, string itemOption, int pad)
        {
            var inventory = target.Inventory;
            int ret = 0;

            var nRet = self.AskYesNo("Do you want to upgrade " + makeItem + "? To make it, you will need the materials listed below. The item will have #r" + itemOption + "#k embedded into it, with it's Level Requirement being " + reqLevel + " and with a weapon attack of #r" + pad + "#k. Make sure you don't use an upgraded item as an upgrade material. Do you wanna do it?\r\n\r\n#b" + needItem);
            if (nRet == 0) self.Say("I can refine other minerals and jewels, so think about it, okay?");
            else
            {
                // Claws
                if (index == 1) ret = inventory.Exchange(-80000, 1472022, -1, 4011007, -1, 4021000, -8, 2012000, -10, 1472023, 1);
                else if (index == 2) ret = inventory.Exchange(-80000, 1472022, -1, 4011007, -1, 4021005, -8, 2012002, -10, 1472024, 1);
                else if (index == 3) ret = inventory.Exchange(-100000, 1472022, -1, 4011007, -1, 4021008, -3, 4000046, -5, 1472025, 1);

                if (ret == 0) self.Say("Please make sure you have all the items you need, and that you have some free space in your equipment inventory.");
                else self.Say("Here, take the " + makeItem + ". What do you think? Well made, huh? Hahaha ... Finally, all those days spent studying refining skills paid off. Please come back another time!");
            }
        }
        public static void chat_message5(int index, string makeItem, string needItemIcon, string needItemString, int unitPrice)
        {
            var inventory = target.Inventory;
            int ret = 0;

            var nRetNum = self.AskNumber("Do you want to make " + makeItem + "? To make it, you will need the materials listed below. How many would you like to make?\r\n\r\n#b" + needItemIcon + " 10 " + needItemString + "\r\n" + unitPrice + " mesos#k", 1, 1, 100);
            var nPrice = unitPrice * nRetNum;
            var nAllNum = nRetNum * 10;
            var nRetBuy = self.AskYesNo("To make #b" + nRetNum + " " + makeItem + "#k, you will need the items listed below. Do you really want to do it?\r\n\r\n#b" + needItemIcon + " " + nAllNum + " " + needItemString + "\r\n" + nPrice + " mesos#k");
            if (nRetBuy == 0) self.Say("I can refine other minerals and jewels, so think about it, okay?");
            else
            {
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

                if (ret == 0) self.Say("Please check that you have" + needItemIcon + needItemString + ", and that you have some free space in your etc. inventory.");
                else self.Say("Here, take the" + nRetNum + " " + makeItem + "(s). What do you think? Quite refined, huh? Hahaha ... Finally, all those days spent studying refining skills paid off. Please come back another time!");
            }
        }
        public static void chat_message3(int index, string makeItem, string needItem, int unitPrice)
        {
            var inventory = target.Inventory;
            int ret = 0;

            var nRetNum = self.AskNumber("Do you want to make " + makeItem + "? To make it, you will need the materials listed below. How many would you like to make?\r\n\r\n#b" + needItem + "\r\n" + unitPrice + " mesos#k", 1, 1, 100);
            var nPrice = unitPrice * nRetNum;
            var nAllNum = nRetNum * 10;
            var nRetBuy = self.AskYesNo("To make #b" + nRetNum + " " + makeItem + "#k, you will need the items listed below. Do you really want to do it?\r\n\r\n#b" + nAllNum + " " + needItem + "\r\n" + nPrice + " mesos#k");
            if (nRetBuy == 0) self.Say("I can refine other minerals and jewels, so think about it, okay?");
            else
            {
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

                if (ret == 0) self.Say("Please check that you have" + needItem + ", or whether your etc. inventory is full or not.");
                else self.Say("Here, take the" + nRetNum + " " + makeItem + "(s). What do you think? Quite refined, huh? Hahaha ... Finally, all those days spent studying refining skills paid off. Please come back another time!");
            }
        }
        public static void chat_message2(int index, string makeItem, string needItem, int reqLevel, string itemOption)
        {
            var inventory = target.Inventory;
            int ret = 0;

            var nRet = self.AskYesNo("To upgrade a " + makeItem + ", you will need the following items. The level requirement for the item is " + reqLevel + ", with the #r" + itemOption + "#k option embedded into it, so please see if you really need it. Ah! And there's one more thing. Please make sure NOT to use an upgraded item as an upgrade material. Are you really sure you want to make this item?\r\n\r\n#b" + needItem);
            if (nRet == 0) self.Say("Really? You must not have all the materials. Try to get them all in town. Fortunately, it seems that the monsters around the forest have all sorts of materials with them.");
            else
            {
                // Glove
                if (index == 1) ret = inventory.Exchange(-20000, 1082020, -1, 4021003, -1, 1082021, 1);
                else if (index == 2) ret = inventory.Exchange(-25000, 1082020, -1, 4021001, -2, 1082022, 1);
                else if (index == 3) ret = inventory.Exchange(-30000, 1082026, -1, 4021000, -3, 1082027, 1);
                else if (index == 4) ret = inventory.Exchange(-40000, 1082026, -1, 4021008, -1, 1082028, 1);
                else if (index == 5) ret = inventory.Exchange(-35000, 1082051, -1, 4021005, -3, 1082052, 1);
                else if (index == 6) ret = inventory.Exchange(-40000, 1082051, -1, 4021008, -1, 1082053, 1);
                else if (index == 7) ret = inventory.Exchange(-40000, 1082054, -1, 4021005, -3, 1082055, 1);
                else if (index == 8) ret = inventory.Exchange(-45000, 1082054, -1, 4021008, -1, 1082056, 1);
                else if (index == 9) ret = inventory.Exchange(-45000, 1082062, -1, 4021002, -4, 1082063, 1);
                else if (index == 10) ret = inventory.Exchange(-50000, 1082062, -1, 4021008, -2, 1082064, 1);
                else if (index == 11) ret = inventory.Exchange(-55000, 1082081, -1, 4021002, -5, 1082082, 1);
                else if (index == 12) ret = inventory.Exchange(-60000, 1082081, -1, 4021008, -3, 1082080, 1);
                else if (index == 13) ret = inventory.Exchange(-70000, 1082086, -1, 4011004, -3, 4011006, -5, 1082087, 1);
                else if (index == 14) ret = inventory.Exchange(-80000, 1082086, -1, 4021008, -2, 4011006, -3, 1082088, 1);
                // Hat
                else if (index == 100) ret = inventory.Exchange(-40000, 1002064, -1, 4011001, -3, 1002065, 1);
                else if (index == 101) ret = inventory.Exchange(-50000, 1002064, -1, 4011006, -3, 1002013, 1);

                if (ret == 0) self.Say("Please make sure you have all the items you need, and whether your etc. inventory is full or not.");
                else self.Say("Here, take the " + makeItem + "... The more I look at it, the more perfect it seems... I wouldn't be surprised to see other magicians thinking I'm a danger just because of my skills...");
            }
        }
        public static void chat_message1(int index, string makeItem, string needItem, int reqLevel)
        {
            var inventory = target.Inventory;
            int ret = 0;

            var nRet = self.AskYesNo("To make um " + makeItem + ", you will need the following items. The level requirement for the item will be " + reqLevel + ", so please first make sure that you really need the item. Are you really sure you want to make one?\r\n\r\n#b" + needItem);
            if (nRet == 0) self.Say("Really? You must not have all the materials. Try to get them all in town. Fortunately, it seems that the monsters around the forest have all sorts of materials with them.");
            else
            {
                // magic wand
                if (index == 1) ret = inventory.Exchange(-1000, 4003001, -5, 1372005, 1);
                else if (index == 2) ret = inventory.Exchange(-3000, 4003001, -10, 4000001, -50, 1372006, 1);
                else if (index == 3) ret = inventory.Exchange(-5000, 4011001, -1, 4000009, -30, 4003000, -5, 1372002, 1);
                else if (index == 4) ret = inventory.Exchange(-12000, 4011002, -2, 4003002, -1, 4003000, -10, 1372004, 1);
                else if (index == 5) ret = inventory.Exchange(-30000, 4011002, -3, 4021002, -1, 4003000, -10, 1372003, 1);
                else if (index == 6) ret = inventory.Exchange(-60000, 4021006, -5, 4011002, -3, 4011001, -1, 4003000, -15, 1372001, 1);
                else if (index == 7) ret = inventory.Exchange(-120000, 4021006, -5, 4021005, -5, 4021007, -1, 4003003, -1, 4003000, -20, 1372000, 1);
                else if (index == 8) ret = inventory.Exchange(-200000, 4011006, -4, 4021003, -3, 4021007, -2, 4021002, -1, 4003002, -1, 4003000, -30, 1372007, 1);
                // staff
                else if (index == 100) ret = inventory.Exchange(-2000, 4003001, -5, 1382000, 1);
                else if (index == 101) ret = inventory.Exchange(-2000, 4021005, -1, 4011001, -1, 4003000, -5, 1382003, 1);
                else if (index == 102) ret = inventory.Exchange(-2000, 4021003, -1, 4011001, -1, 4003000, -5, 1382005, 1);
                else if (index == 103) ret = inventory.Exchange(-5000, 4003001, -50, 4011001, -1, 4003000, -10, 1382004, 1);
                else if (index == 104) ret = inventory.Exchange(-12000, 4021006, -2, 4021001, -1, 4011001, -1, 4003000, -15, 1382002, 1);
                else if (index == 105) ret = inventory.Exchange(-180000, 4011001, -3, 4021001, -5, 4021006, -5, 4021005, -5, 4003003, -1, 4000010, -50, 4003000, -30, 1382001, 1);
                // glove
                else if (index == 200) ret = inventory.Exchange(-7000, 4000021, -15, 1082019, 1);
                else if (index == 201) ret = inventory.Exchange(-15000, 4000021, -30, 4011001, -1, 1082020, 1);
                else if (index == 202) ret = inventory.Exchange(-20000, 4000021, -50, 4011006, -2, 1082026, 1);
                else if (index == 203) ret = inventory.Exchange(-25000, 4000021, -60, 4021006, -1, 4021000, -2, 1082051, 1);
                else if (index == 204) ret = inventory.Exchange(-30000, 4000021, -70, 4011006, -1, 4011001, -3, 4021000, -2, 1082054, 1);
                else if (index == 205) ret = inventory.Exchange(-40000, 4000021, -80, 4021000, -3, 4021006, -3, 4003000, -30, 1082062, 1);
                else if (index == 206) ret = inventory.Exchange(-50000, 4021000, -3, 4011006, -2, 4000030, -35, 4003000, -40, 1082081, 1);
                else if (index == 207) ret = inventory.Exchange(-70000, 4011007, -1, 4011001, -8, 4021007, -1, 4000030, -50, 4003000, -50, 1082086, 1);

                if (ret == 0) self.Say("Please make sure you have all the items you need, and whether your etc. inventory is full or not.");
                else self.Say("Here, take the " + makeItem + ". The more I look at it, the more perfect it seems. Hahah, it doesn't hurt to think that other magicians are afraid of my skills...");
            }
        }
        //babo
        [Script("refine_ellinia")]
        class refine_ellinia : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                make2.self = self;
                make2.target = target;
                var nRet1 = self.AskYesNo("Do you want to take a look at some items? Well... thinking of creating one? I am actually a magician who was banished from the town for casting an illegal spell. And because of that I've been hiding and doing some business here... well, it doesn't really matter much. Do you want to trade with me?");
                if (nRet1 == 0) self.Say("I guess you don't trust in my abilities... hahaha... well, I was a great wizard. You still don't believe in my skills... hmm... but remember I used to be a great magician...");
                else
                {
                    var v1 = self.AskMenu("Very well... it's for our own good, right? Choose what you want to do... \r\n#b#L0#Create a wand#l\r\n#L1#Create a staff#l\r\n#L2#Create a glove#l\r\n#L3#Upgrade a glove#l\r\n#L4#Upgrade a helmet#l");
                    if (v1 == 0)
                    {
                        var v2 = self.AskMenu("If you bring me a lot of materials, I will make you a wand with my magic powers. Now... what kind of wand do you want me to make?\r\n#L0##b #t1372005##k (Level Requirement : 8, Common)#l\r\n#L1##b #t1372006##k (Level Requirement : 13, Common)#l\r\n#L2##b #t1372002##k (Level Requirement : 18, Common)#l\r\n#L3##b #t1372004##k (Level Requirement : 23, Magician)#l\r\n#L4##b #t1372003##k (Level Requirement : 28, Magician)#l\r\n#L5##b #t1372001##k (Level Requirement : 33, Magician)#l\r\n#L6##b #t1372000##k (Level Requirement : 38, Magician)#l\r\n#L7##b #t1372007##k (Level Requirement : 48, Magician)#l");
                        if (v2 == 0) chat_message1(1, "#t1372005#", "#v4003001# 5 #t4003001#s\r\n#v4031138# 1,000 mesos", 8);
                        else if (v2 == 1) chat_message1(2, "#t1372006#", "#v4003001# 10 #t4003001#s\r\n#v4000001# 50 #t4000001#s\r\n#v4031138# 3,000 mesos", 13);
                        else if (v2 == 2) chat_message1(3, "#t1372002#", "#v4011001# #t4011001#\r\n#v4000009# 30 #t4000009#s\r\n#v4003000# 5 #t4003000#s\r\n#v4031138# 5,000 mesos", 18);
                        else if (v2 == 3) chat_message1(4, "#t1372004#", "#v4011002# 2 #t4011002#s\r\n#v4003002# #t4003002#\r\n#v4003000# 10 #t4003000#s\r\n#v4031138# 12,000 mesos", 23);
                        else if (v2 == 4) chat_message1(5, "#t1372003#", "#v4011002# 3 #t4011002#s\r\n#v4021002# #t4021002#\r\n#v4003000# 10 #t4003000#s\r\n#v4031138# 30,000 mesos", 28);
                        else if (v2 == 5) chat_message1(6, "#t1372001#", "#v4021006# 5 #t4021006#s\r\n#v4011002# 3 #t4011002#s\r\n#v4011001# #t4011001#\r\n#v4003000# 15 #t4003000#s\r\n#v4031138# 60,000 mesos", 33);
                        else if (v2 == 6) chat_message1(7, "#t1372000#", "#v4021006# 5 #t4021006#s\r\n#v4021005# 5 #t4021005#s\r\n#v4021007# #t4021007#\r\n#v4003003# #t4003003#\r\n#v4003000# 20 #t4003000#s\r\n#v4031138# 120,000 mesos", 38);
                        else if (v2 == 7) chat_message1(8, "#t1372007#", "#v4011006# 4 #t4011006#s\r\n#v4021003# 3 #t4021003#s\r\n#v4021007# 2 #t4021007#s\r\n#v4021002# #t4021002#\r\n#v4003002# #t4003002#\r\n#v4003000# 30 #t4003000#s\r\n#v4031138# 200,000 mesos", 48);
                    }
                    else if (v1 == 1)
                    {
                        var v2 = self.AskMenu("If you bring me a lot of materials, I will make you a staff with my magic powers. Now... what kind of staff do you want me to make?\r\n#L0##b#t1382000##k  (Level Requirement : 10, Magician)#l\r\n#L1##b#t1382003##k (Level Requirement : 15, Magician)#l\r\n#L2##b#t1382005##k (Level Requirement : 15, Magician)#l\r\n#L3##b#t1382004##k (Level Requirement : 20, Magician)#l\r\n#L4##b#t1382002##k (Level Requirement : 25, Magician)#l\r\n#L5##b#t1382001##k (Level Requirement : 45, Magician)#l");
                        if (v2 == 0) chat_message1(100, "#t1382000#", "#v4003001# 5 #t4003001#s\r\n2,000 mesos", 10);
                        else if (v2 == 1) chat_message1(101, "#t1382003#", "#v4021005# #t4021005#\r\n#v4011001# #t4011001#\r\n#v4003000# 5 #t4003000#s\r\n#v4031138# 2,000 mesos", 15);
                        else if (v2 == 2) chat_message1(102, "#t1382005#", "#v4021003# #t4021003#\r\n#v4011001# #t4011001#\r\n#v4003000# 5 #t4003000#s\r\n#v4031138# 2,000 mesos", 15);
                        else if (v2 == 3) chat_message1(103, "#t1382004#", "#v4003001# 50 #t4003001#s\r\n#v4011001# #t4011001#\r\n#v4003000# 10 #t4003000#s\r\n#v4031138# 5,000 mesos", 20);
                        else if (v2 == 4) chat_message1(104, "#t1382002#", "#v4021006# 2 #t4021006#s\r\n#v4021001# #t4021001#\r\n#v4011001# #t4011001#\r\n#v4003000# 15 #t4003000#s\r\n#v4031138# 12,000 mesos", 25);
                        else if (v2 == 5) chat_message1(105, "#t1382001#", "#v4011001# 3 #t4011001#s\r\n#v4021001# 5 #t4021001#s\r\n#v4021006# 5 #t4021006#s\r\n#v4021005# 5 #t4021005#s\r\n#v4003003# #t4003003#\r\n#v4000010# 50 #t4000010#s\r\n#v4003000# 30 #t4003000#s\r\n#v4031138# 180,000 mesos", 45);
                    }
                    else if (v1 == 2)
                    {
                        var v2 = self.AskMenu("If you bring me a lot of materials, I'll make you a glove with my magic powers. Now... what kind of glove do you want me to make?\r\n#L0##b #t1082019##k  (Level Requirement : 15, Magician)#l\r\n#L1##b #t1082020##k (Level Requirement : 20, Magician)#l\r\n#L2##b #t1082026##k (Level Requirement : 25, Magician)#l\r\n#L3##b #t1082051##k (Level Requirement : 30, Magician)#l\r\n#L4##b #t1082054##k (Level Requirement : 35, Magician)#l\r\n#L5##b #t1082062##k (Level Requirement : 40, Magician)#l\r\n#L6##b #t1082081##k (Level Requirement : 50, Magician)#l\r\n#L7##b #t1082086##k (Level Requirement : 60, Magician)#l");
                        if (v2 == 0) chat_message1(200, "#t1082019#", "#v4000021# 15 #t4000021#s\r\n#v4031138# 7,000 mesos", 15);
                        else if (v2 == 1) chat_message1(201, "#t1082020#", "#v4000021# 30 #t4000021#s\r\n#v4011001# #t4011001#\r\n#v4031138# 15,000 mesos", 20);
                        else if (v2 == 2) chat_message1(202, "#t1082026#", "#v4000021# 50 #t4000021#s\r\n#v4011006# 2 #t4011006#s\r\n#v4031138# 20,000 mesos", 25);
                        else if (v2 == 3) chat_message1(203, "#t1082051#", "#v4000021# 60 #t4000021#s\r\n#v4021006# #t4021006#\r\n#v4021000# 2 #t4021000#s\r\n#v4031138# 25,000 mesos", 30);
                        else if (v2 == 4) chat_message1(204, "#t1082054#", "#v4000021# 70 #t4000021#s\r\n#v4011006# #t4011006#\r\n#v4011001# 3 #t4011001#s\r\n#v4021000# 2 #t4021000#s\r\n#v4031138# 30,000 mesos", 35);
                        else if (v2 == 5) chat_message1(205, "#t1082062#", "#v4000021# 80 #t4000021#s\r\n#v4021000# 3 #t4021000#s\r\n#v4021006# 3 #t4021006#s\r\n#v4003000# 30 #t4003000#s\r\n#v4031138# 40,000 mesos", 40);
                        else if (v2 == 6) chat_message1(206, "#t1082081#", "#v4021000# 3 #t4021000#s\r\n#v4011006# 2 #t4011006#s\r\n#v4000030# 35 #t4000030#s\r\n#v4003000# 40 #t4003000#s\r\n#v4031138# 50,000 mesos", 50);
                        else if (v2 == 7) chat_message1(207, "#t1082086#", "#v4011007# #t4011007#\r\n#v4011001# 8 #t4011001#s\r\n#v4021007# #t4021007#\r\n#v4000030# 50 #t4000030#s\r\n#v4003000# 50 #t4003000#s\r\n#v4031138# 70,000 mesos", 60);
                    }
                    else if (v1 == 3)
                    {
                        self.Say("Do you want to upgrade a glove? Be careful! All items used for upgrading will disappear, and if you use an item that has been #rupgraded#k with a scroll, the effect will disappear once upgraded, so you'd better think it through before making your decision...");
                        var v2 = self.AskMenu("Now... which glove do you want to upgrade??\r\n#L0##b #t1082021##k (Level Requirement : 20, Magician)#l\r\n#L1##b #t1082022##k (Level Requirement : 20, Magician)#l\r\n#L2##b #t1082027##k (Level Requirement : 25, Magician)#l\r\n#L3##b #t1082028##k (Level Requirement : 25, Magician)#l\r\n#L4##b #t1082052##k (Level Requirement : 30, Magician)#l\r\n#L5##b #t1082053##k (Level Requirement : 30, Magician)#l\r\n#L6##b #t1082055##k (Level Requirement : 35, Magician)#l\r\n#L7##b #t1082056##k (Level Requirement : 35, Magician)#l\r\n#L8##b #t1082063##k (Level Requirement : 40, Magician)#l\r\n#L9##b #t1082064##k (Level Requirement : 40, Magician)#l\r\n#L10##b #t1082082##k (Level Requirement : 50, Magician)#l\r\n#L11##b #t1082080##k (Level Requirement : 50, Magician)#l\r\n#L12##b #t1082087##k (Level Requirement : 60, Magician)#l\r\n#L13##b #t1082088##k (Level Requirement : 60, Magician)#l");
                        if (v2 == 0) chat_message2(1, "#t1082021#", "#v1082020# #t1082020#\r\n#t4021003#\r\n#v4031138# 20,000 mesos", 20, "INT +1");
                        else if (v2 == 1) chat_message2(2, "#t1082022#", "#v1082020# #t1082020#\r\n#v4021001# 2 #t4021001#s\r\n#v4031138# 25,000 mesos", 20, "INT +2");
                        else if (v2 == 2) chat_message2(3, "#t1082027#", "#v1082026# #t1082026#\r\n#v4021000# 3 #t4021000#s\r\n#v4031138# 30,000 mesos", 25, "INT +1");
                        else if (v2 == 3) chat_message2(4, "#t1082028#", "#v1082026# #t1082026#\r\n#v4021008# #t4021008#\r\n#v4031138# 40,000 mesos", 25, "INT +2");
                        else if (v2 == 4) chat_message2(5, "#t1082052#", "#v1082051# #t1082051#\r\n#v4021005# 3 #t4021005#s\r\n#v4031138# 35,000 mesos", 30, "INT +1");
                        else if (v2 == 5) chat_message2(6, "#t1082053#", "#v1082051# #t1082051#\r\n#v4021008# #t4021008#s\r\n#v4031138# 40,000 mesos", 30, "INT +2");
                        else if (v2 == 6) chat_message2(7, "#t1082055#", "#v1082054# #t1082054#\r\n#v4021005# 3 #t4021005#s\r\n#v4031138# 40,000 mesos", 35, "INT +1");
                        else if (v2 == 7) chat_message2(8, "#t1082056#", "#v1082054# #t1082054#\r\n#v4021008# #t4021008#s\r\n#v4031138# 45,000 mesos", 35, "INT +2");
                        else if (v2 == 8) chat_message2(9, "#t1082063#", "#v1082062# #t1082062#\r\n#v4021002# 4 #t4021002#s\r\n#v4031138# 45,000 mesos", 40, "INT +2");
                        else if (v2 == 9) chat_message2(10, "#t1082064#", "#v1082062# #t1082062#\r\n#v4021008# 2 #t4021008#s\r\n#v4031138# 50,000 mesos", 40, "INT +3");
                        else if (v2 == 10) chat_message2(11, "#t1082082#", "#v1082081# #t1082081#\r\n#v4021002# 5 #t4021002#s\r\n#v4031138# 55,000 mesos", 50, "INT +2, MP +15");
                        else if (v2 == 11) chat_message2(12, "#t1082080#", "#v1082081# #t1082081#\r\n#v4021008# 3 #t4021008#s\r\n#v4031138# 60,000 mesos", 50, "INT +3, MP +30");
                        else if (v2 == 12) chat_message2(13, "#t1082087#", "#v1082086# #t1082086#\r\n#v4011004# 3 #t4011004#s\r\n#v4011006# 5 #t4011006#s\r\n#v4031138# 70,000 mesos", 60, "INT +2, LUK +1, MP +15");
                        else if (v2 == 13) chat_message2(14, "#t1082088#", "#v1082086# #t1082086#\r\n#v4021008# 2 #t4021008#s\r\n#v4011006# 3 #t4011006#s\r\n#v4031138# 80,000 mesos", 60, "INT + 3, LUK + 1, MP + 30");
                    }
                    else if (v1 == 4)
                    {
                        self.Say("So you want to upgrade your helmet... Be careful! All items used for upgrading will disappear, and if you use an item that has been #rupgraded#k with a scroll, the effect will disappear once upgraded, so you better think carefully before making your decision...");
                        var v2 = self.AskMenu("Very well... which helmet do you want to upgrade?\r\n#L0##b#t1002065##k (Level Requirement : 30, Magician)#l\r\n#L1##b#t1002013##k (Level Requirement : 30, Magician)#l");
                        if (v2 == 0) chat_message2(100, "#t1002065#", "#v1002064# #t1002064#\r\n#v4011001# 3 #t4011001#s\r\n#v4031138# 40,000 mesos", 30, "INT +1");
                        else if (v2 == 1) chat_message2(101, "#t1002013#", "#v1002064# #t1002064#\r\n#v4011006# 3 #t4011006#s\r\n#v4031138# 50,000 mesos", 30, "INT +2");
                    }
                }
            }
        }
        // refining the raw ores at Kerning City.
        [Script("refine_kerning2")]
        class refine_kerning2 : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                make2.self = self;
                make2.target = target;
                var nRet1 = self.AskYesNo("What's up? All good? You wouldn't happen to have a rare ore or a jewel would you? For a small service fee, I can turn them into useful materials for weapons and armor. I learned how to refine while working fixing items... What do you think? Wanna let me do it?");
                if (nRet1 == 0) self.Say("I understand... but I'm sure that one day you'll need my help... and when that day comes you can come back and talk to me, alright?");
                else
                {
                    var v1 = self.AskMenu("Alright! With the ores and a small service fee, I can refine them into something useful. Before that though, make sure you have a slot available in your etc. inventory. Now... what do you want me to do?",
                        "Refine the ores of a mineral",
                        "Refine the ores of a jewel",
                        "I have #t4000039#...",
                        "I want to upgrade a claw..."
                    );
                    if (v1 == 0)
                    {
                        var v2 = self.AskMenu("What kind of mineral would you like to make?",
                            "#t4011000#",
                            "#t4011001#",
                            "#t4011002#",
                            "#t4011003#",
                            "#t4011004#",
                            "#t4011005#",
                            "#t4011006#"
                        );
                        if (v2 == 0) chat_message5(1, "#t4011000#", "#v4010000#", "#t4010000#s", 250);
                        else if (v2 == 1) chat_message5(2, "#t4011001#", "#v4010001#", "#t4010001#s", 250);
                        else if (v2 == 2) chat_message5(3, "#t4011002#", "#v4010002#", "#t4010002#s", 250);
                        else if (v2 == 3) chat_message5(4, "#t4011003#", "#v4010003#", "#t4010003#s", 450);
                        else if (v2 == 4) chat_message5(5, "#t4011004#", "#v4010004#", "#t4010004#s", 450);
                        else if (v2 == 5) chat_message5(6, "#t4011005#", "#v4010005#", "#t4010005#s", 450);
                        else if (v2 == 6) chat_message5(7, "#t4011006#", "#v4010006#", "#t4010006#s", 750);
                    }
                    else if (v1 == 1)
                    {
                        var v2 = self.AskMenu("What kind of jewel would you like to refine?\r\n#b#L0##t4021000##l\r\n#L1##t4021001##l\r\n#L2##t4021002##l\r\n#L3##t4021003##l\r\n#L4##t4021004##l\r\n#L5##t4021005##l\r\n#L6##t4021006##l\r\n#L7##t4021007##l\r\n#L8##t4021008##l");
                        if (v2 == 0) chat_message5(100, "#t4021000#", "#v4020000#", "#t4020000#s", 450);
                        else if (v2 == 1) chat_message5(101, "#t4021001#", "#v4020001#", "#t4020001#s", 450);
                        else if (v2 == 2) chat_message5(102, "#t4021002#", "#v4020002#", "#t4020002#s", 450);
                        else if (v2 == 3) chat_message5(103, "#t4021003#", "#v4020003#", "#t4020003#s", 450);
                        else if (v2 == 4) chat_message5(104, "#t4021004#", "#v4020004#", "#t4020004#s", 450);
                        else if (v2 == 5) chat_message5(105, "#t4021005#", "#v4020005#", "#t4020005#s", 450);
                        else if (v2 == 6) chat_message5(106, "#t4021006#", "#v4020006#", "#t4020006#s", 450);
                        else if (v2 == 7) chat_message5(107, "#t4021007#", "#v4020007#", "#t4020007#s", 950);
                        else if (v2 == 8) chat_message5(108, "#t4021008#", "#v4020008#", "#t4020008#s", 2900);
                    }
                    else if (v1 == 2)
                    {
                        var nRet2 = self.AskYesNo("You have #t4000039#? Hmm... with that I can try to make #t4011001#. If you give me #b100 #b#t4000039#s#k and #b1,000 mesos#k, I can make a #brefined #t4011001##k. What do you think? Wanna give it a go?");
                        if (nRet2 == 0) self.Say("I can refine other minerals and jewels besides these, so please think about it...");
                        else
                        {
                            var inventory = target.Inventory;
                            var ret = inventory.Exchange(-1000, 4000039, -100, 4011001, 1);
                            if (ret == 0) self.Say("Maybe you're short of cash... make sure you have 100 #t4000039#s and 1,000 mesos, and a free slot in your etc. inventory...");
                            else self.Say("Well... here's the #b#t4011001##k. What do you think? Pretty well made, huh? hahaha... I'm glad I studied my refining skills a lot... come back whenever you want~!");
                        }
                    }
                    else if (v1 == 3)
                    {
                        self.Say("So, you want to upgrade a claw? Be careful! All items used for upgrading will disappear, and if you use an item that has been #rupgraded#k with a scroll, the effect will disappear once upgraded, so you better think carefully before making your decision...");
                        var v2 = self.AskMenu("Now... What kind of claw would you like to upgrade?\r\n#L0##b#t1472023##k (Level Requirement : 60, Thief)#l\r\n#L1##b#t1472024##k (Level Requirement : 60, Thief)#l\r\n#L2##b#t1472025##k (Level Requirement : 60, Thief)#k#l");
                        if (v2 == 0) chat_message4(1, "#t1472023#", "#v1472022# #t1472022#\r\n#v4011007# #t4011007#\r\n#v4021000# 8 #t4021000#s\r\n#v2012000# 10 #t2012000#s\r\n#v4031138# 80,000 mesos", 60, "DEX + 4, Avoidability + 3", 30);
                        else if (v2 == 1) chat_message4(2, "#t1472024#", "#v1472022# #t1472022#\r\n#v4011007# #t4011007#\r\n#v4021005# 8 #t4021005#s\r\n#v2012002# 10 #t2012002#s\r\n#v4031138# 80,000 mesos", 60, "LUK + 4, Avoidability + 3", 30);
                        else if (v2 == 2) chat_message4(3, "#t1472025#", "#v1472022# #t1472022#\r\n#v4011007# #t4011007#\r\n#v4021008# 3 #t4021008#s\r\n#v4000046# 5 #t4000046#s\r\n#v4031138# 100,000 mesos", 60, "LUK + 5, Avoidability + 4", 30);
                    }
                }
            }
        }
    }
}
