using System;
using System.Reflection;
using WvsBeta.Common;
using WvsBeta.Game;
using WvsBeta.Game.Scripting;

namespace WvsBeta.Scripts.Scripts
{
    public static class make5
    {
        static INpcHost self;
        static GameCharacter target;
        public static void chat_message2(int index, string makeItem, string needItem, int reqLevel, string itemOption)
        {
            var inventory = target.Inventory;
            int ret = 0;

            var nRet = self.AskYesNo("To make a " + makeItem + ", you will need the following materials. The Level Requirement will be " + reqLevel + ", so please make sure that you really need this item. Do you want me to do it?\r\n\r\n#b" + needItem);
            if (nRet == 0) self.Say("I understand. If you want to grab a cool item, just drop by! I'll be here waiting!");
            else
            {
                // warrior shoes
                if (index == 0) ret = inventory.Exchange(-18000, 4021003, -4, 4011001, -2, 4000021, -45, 4003000, -15, 1072003, 1);
                else if (index == 1) ret = inventory.Exchange(-18000, 4011002, -4, 4011001, -2, 4000021, -45, 4003000, -15, 1072039, 1);
                else if (index == 2) ret = inventory.Exchange(-18000, 4011004, -4, 4011001, -2, 4000021, -45, 4003000, -15, 1072040, 1);
                else if (index == 3) ret = inventory.Exchange(-18000, 4021000, -4, 4011001, -2, 4000021, -45, 4003000, -15, 1072041, 1);
                else if (index == 4) ret = inventory.Exchange(-19800, 4011001, -3, 4021004, -1, 4000021, -30, 4000030, -20, 4003000, -25, 1072002, 1);
                else if (index == 5) ret = inventory.Exchange(-19800, 4011002, -3, 4021004, -1, 4000021, -30, 4000030, -20, 4003000, -25, 1072112, 1);
                else if (index == 6) ret = inventory.Exchange(-22500, 4021008, -2, 4021004, -1, 4000021, -30, 4000030, -20, 4003000, -25, 1072113, 1);
                else if (index == 7) ret = inventory.Exchange(-34200, 4011003, -4, 4000021, -100, 4000030, -40, 4003000, -30, 4000103, -100, 1072000, 1);
                else if (index == 8) ret = inventory.Exchange(-34200, 4011005, -4, 4021007, -1, 4000030, -40, 4003000, -30, 4000104, -100, 1072126, 1);
                else if (index == 9) ret = inventory.Exchange(-34200, 4011002, -4, 4021007, -1, 4000030, -40, 4003000, -30, 4000105, -100, 1072127, 1);
                else if (index == 10) ret = inventory.Exchange(-45000, 4021008, -1, 4011001, -3, 4021003, -6, 4000030, -65, 4003000, -45, 1072132, 1);
                else if (index == 11) ret = inventory.Exchange(-45000, 4021008, -1, 4011001, -3, 4011002, -6, 4000030, -65, 4003000, -45, 1072133, 1);
                else if (index == 12) ret = inventory.Exchange(-45000, 4021008, -1, 4011001, -3, 4011005, -6, 4000030, -65, 4003000, -45, 1072134, 1);
                else if (index == 13) ret = inventory.Exchange(-45000, 4021008, -1, 4011001, -3, 4011006, -6, 4000030, -65, 4003000, -45, 1072135, 1);
                // magician shoes
                else if (index == 100) ret = inventory.Exchange(-16200, 4021000, -2, 4000021, -50, 4003000, -15, 1072075, 1);
                else if (index == 101) ret = inventory.Exchange(-16200, 4021002, -2, 4000021, -50, 4003000, -15, 1072076, 1);
                else if (index == 102) ret = inventory.Exchange(-16200, 4011004, -2, 4000021, -50, 4003000, -15, 1072077, 1);
                else if (index == 103) ret = inventory.Exchange(-16200, 4021008, -1, 4000021, -50, 4003000, -15, 1072078, 1);
                else if (index == 104) ret = inventory.Exchange(-18000, 4021001, -3, 4021006, -1, 4000021, -30, 4000030, -15, 4003000, -20, 1072089, 1);
                else if (index == 105) ret = inventory.Exchange(-18000, 4021000, -3, 4021006, -1, 4000021, -30, 4000030, -15, 4003000, -20, 1072090, 1);
                else if (index == 106) ret = inventory.Exchange(-19800, 4021008, -2, 4021006, -1, 4000021, -40, 4000030, -25, 4003000, -20, 1072091, 1);
                else if (index == 107) ret = inventory.Exchange(-27000, 4021000, -4, 4000030, -40, 4000110, -100, 4003000, -25, 1072114, 1);
                else if (index == 108) ret = inventory.Exchange(-27000, 4021005, -4, 4000030, -40, 4000111, -100, 4003000, -25, 1072115, 1);
                else if (index == 109) ret = inventory.Exchange(-31500, 4011006, -2, 4021007, -1, 4000030, -40, 4000100, -100, 4003000, -25, 1072116, 1);
                else if (index == 110) ret = inventory.Exchange(-36000, 4021008, -2, 4021007, -1, 4000030, -40, 4000112, -100, 4003000, -30, 1072117, 1);
                else if (index == 111) ret = inventory.Exchange(-45000, 4021009, -1, 4011006, -3, 4021000, -3, 4000030, -60, 4003000, -40, 1072140, 1);
                else if (index == 112) ret = inventory.Exchange(-45000, 4021009, -1, 4011006, -3, 4021005, -3, 4000030, -60, 4003000, -40, 1072141, 1);
                else if (index == 113) ret = inventory.Exchange(-45000, 4021009, -1, 4011006, -3, 4021001, -3, 4000030, -60, 4003000, -40, 1072142, 1);
                else if (index == 114) ret = inventory.Exchange(-45000, 4021009, -1, 4011006, -3, 4021003, -3, 4000030, -60, 4003000, -40, 1072143, 1);
                // bowman shoes
                else if (index == 200) ret = inventory.Exchange(-17100, 4021000, -2, 4000021, -50, 4003000, -15, 1072079, 1);
                else if (index == 201) ret = inventory.Exchange(-17100, 4021005, -2, 4000021, -50, 4003000, -15, 1072080, 1);
                else if (index == 202) ret = inventory.Exchange(-17100, 4021003, -2, 4000021, -50, 4003000, -15, 1072081, 1);
                else if (index == 203) ret = inventory.Exchange(-17100, 4021004, -2, 4000021, -50, 4003000, -15, 1072082, 1);
                else if (index == 204) ret = inventory.Exchange(-17100, 4021006, -2, 4000021, -50, 4003000, -15, 1072083, 1);
                else if (index == 205) ret = inventory.Exchange(-18000, 4021002, -3, 4021006, -1, 4000021, -30, 4000030, -15, 4003000, -20, 1072101, 1);
                else if (index == 206) ret = inventory.Exchange(-18000, 4021003, -3, 4021006, -1, 4000021, -30, 4000030, -15, 4003000, -20, 1072102, 1);
                else if (index == 207) ret = inventory.Exchange(-18000, 4021000, -3, 4021006, -1, 4000021, -30, 4000030, -15, 4003000, -20, 1072103, 1);
                else if (index == 208) ret = inventory.Exchange(-28800, 4021000, -4, 4000030, -45, 4000106, -100, 4003000, -30, 1072118, 1);
                else if (index == 209) ret = inventory.Exchange(-28800, 4021006, -4, 4000030, -45, 4000107, -100, 4003000, -30, 1072119, 1);
                else if (index == 210) ret = inventory.Exchange(-36000, 4011003, -5, 4000030, -45, 4000108, -100, 4003000, -30, 1072120, 1);
                else if (index == 211) ret = inventory.Exchange(-36000, 4021002, -5, 4000030, -45, 4000099, -100, 4003000, -30, 1072121, 1);
                else if (index == 212) ret = inventory.Exchange(-45000, 4021008, -1, 4011001, -3, 4021006, -3, 4000030, -60, 4000033, -80, 4003000, -35, 1072122, 1);
                else if (index == 213) ret = inventory.Exchange(-45000, 4021008, -1, 4011001, -3, 4021006, -3, 4000030, -60, 4000032, -150, 4003000, -35, 1072123, 1);
                else if (index == 214) ret = inventory.Exchange(-45000, 4021008, -1, 4011001, -3, 4021006, -3, 4000030, -60, 4000041, -100, 4003000, -35, 1072124, 1);
                else if (index == 215) ret = inventory.Exchange(-45000, 4021008, -1, 4011001, -3, 4021006, -3, 4000030, -60, 4000042, -250, 4003000, -35, 1072125, 1);
                // thief shoes
                else if (index == 300) ret = inventory.Exchange(-17100, 4011000, -3, 4000021, -50, 4003000, -15, 1072032, 1);
                else if (index == 301) ret = inventory.Exchange(-17100, 4011001, -3, 4000021, -50, 4003000, -15, 1072033, 1);
                else if (index == 302) ret = inventory.Exchange(-17100, 4011004, -2, 4000021, -50, 4003000, -15, 1072035, 1);
                else if (index == 303) ret = inventory.Exchange(-18900, 4011006, -2, 4000021, -50, 4003000, -15, 1072036, 1);
                else if (index == 304) ret = inventory.Exchange(-18000, 4021000, -3, 4021004, -1, 4000021, -30, 4000030, -15, 4003000, -20, 1072104, 1);
                else if (index == 305) ret = inventory.Exchange(-18000, 4021003, -3, 4021004, -1, 4000021, -30, 4000030, -15, 4003000, -20, 1072105, 1);
                else if (index == 306) ret = inventory.Exchange(-18000, 4021002, -3, 4021004, -1, 4000021, -30, 4000030, -15, 4003000, -20, 1072106, 1);
                else if (index == 307) ret = inventory.Exchange(-28800, 4021003, -4, 4000030, -45, 4000095, -100, 4003000, -30, 1072108, 1);
                else if (index == 308) ret = inventory.Exchange(-31500, 4021006, -4, 4000030, -45, 4000096, -100, 4003000, -30, 1072109, 1);
                else if (index == 309) ret = inventory.Exchange(-31500, 4021005, -4, 4000030, -45, 4000097, -100, 4003000, -30, 1072110, 1);
                else if (index == 310) ret = inventory.Exchange(-36000, 4021000, -5, 4000030, -45, 4000113, -100, 4003000, -30, 1072107, 1);
                else if (index == 311) ret = inventory.Exchange(-45000, 4011007, -2, 4021005, -3, 4000030, -50, 4000114, -100, 4003000, -35, 1072128, 1);
                else if (index == 312) ret = inventory.Exchange(-45000, 4011007, -2, 4021003, -3, 4000030, -50, 4000109, -100, 4003000, -35, 1072129, 1);
                else if (index == 313) ret = inventory.Exchange(-45000, 4011007, -2, 4021000, -3, 4000030, -50, 4000115, -100, 4003000, -35, 1072130, 1);
                else if (index == 314) ret = inventory.Exchange(-45000, 4011007, -2, 4021001, -3, 4000030, -50, 4000036, -80, 4003000, -35, 1072131, 1);

                if (ret == 0) self.Say("Please make sure you have all the necessary items with you. Otherwise, check your equipment inventory, it might be full.");
                else self.Say("Here, take the requested item " + makeItem + ". I think it turned out pretty cool!! Don't you think so too? Please come back again, whenever you want!");
            }
        }
        public static void chat_message1(int index, string makeItem, string needItem, int reqLevel, string itemOption)
        {
            var inventory = target.Inventory;
            int ret = 0;

            var nRet = self.AskYesNo("To make a " + makeItem + " using the stimulator, you will need the following items. The Level Requirement will be " + reqLevel + ", and the basic attack level is " + itemOption + ". If the stimulator is used the item option can be increased, but #bit can also come out worse than it was before, plus it carries a 10% failure chance, so please be careful#k. What do you think? Do you want to do it?\r\n\r\n#b" + needItem);
            if (nRet == 0) self.Say("I understand. When using the stimulator, you run the risk of not being able to craft the item and thus still wasting materials in the process. I'd be careful too, but if you change your mind, feel free to come back.");
            else
            {
                var sProp = Rand32.NextBetween(1, 10);
                if (sProp == 1)
                {
                    // failure to create warrior shoes
                    if (index == 0) ret = inventory.Exchange(0, 4130001, -1, 4021003, -4, 4011001, -2, 4000021, -45, 4003000, -15);
                    else if (index == 1) ret = inventory.Exchange(0, 4130001, -1, 4011002, -4, 4011001, -2, 4000021, -45, 4003000, -15);
                    else if (index == 2) ret = inventory.Exchange(0, 4130001, -1, 4011004, -4, 4011001, -2, 4000021, -45, 4003000, -15);
                    else if (index == 3) ret = inventory.Exchange(0, 4130001, -1, 4021000, -4, 4011001, -2, 4000021, -45, 4003000, -15);
                    else if (index == 4) ret = inventory.Exchange(0, 4130001, -1, 4011001, -3, 4021004, -1, 4000021, -30, 4000030, -20, 4003000, -25);
                    else if (index == 5) ret = inventory.Exchange(0, 4130001, -1, 4011002, -3, 4021004, -1, 4000021, -30, 4000030, -20, 4003000, -25);
                    else if (index == 6) ret = inventory.Exchange(0, 4130001, -1, 4021008, -2, 4021004, -1, 4000021, -30, 4000030, -20, 4003000, -25);
                    else if (index == 7) ret = inventory.Exchange(0, 4130001, -1, 4011003, -4, 4000021, -100, 4000030, -40, 4003000, -30, 4000033, -100);
                    else if (index == 8) ret = inventory.Exchange(0, 4130001, -1, 4011005, -4, 4021007, -1, 4000030, -40, 4003000, -30, 4000042, -250);
                    else if (index == 9) ret = inventory.Exchange(0, 4130001, -1, 4011002, -4, 4021007, -1, 4000030, -40, 4003000, -30, 4000041, -120);
                    else if (index == 10) ret = inventory.Exchange(0, 4130001, -1, 4021008, -1, 4011001, -3, 4021003, -6, 4000030, -65, 4003000, -45);
                    else if (index == 11) ret = inventory.Exchange(0, 4130001, -1, 4021008, -1, 4011001, -3, 4011002, -6, 4000030, -65, 4003000, -45);
                    else if (index == 12) ret = inventory.Exchange(0, 4130001, -1, 4021008, -1, 4011001, -3, 4011005, -6, 4000030, -65, 4003000, -45);
                    else if (index == 13) ret = inventory.Exchange(0, 4130001, -1, 4021008, -1, 4011001, -3, 4011006, -6, 4000030, -65, 4003000, -45);
                    // failure to create magician shoes
                    else if (index == 100) ret = inventory.Exchange(0, 4130001, -1, 4021000, -2, 4000021, -50, 4003000, -15);
                    else if (index == 101) ret = inventory.Exchange(0, 4130001, -1, 4021002, -2, 4000021, -50, 4003000, -15);
                    else if (index == 102) ret = inventory.Exchange(0, 4130001, -1, 4011004, -2, 4000021, -50, 4003000, -15);
                    else if (index == 103) ret = inventory.Exchange(0, 4130001, -1, 4021008, -1, 4000021, -50, 4003000, -15);
                    else if (index == 104) ret = inventory.Exchange(0, 4130001, -1, 4021001, -3, 4021006, -1, 4000021, -30, 4000030, -15, 4003000, -20);
                    else if (index == 105) ret = inventory.Exchange(0, 4130001, -1, 4021000, -3, 4021006, -1, 4000021, -30, 4000030, -15, 4003000, -20);
                    else if (index == 106) ret = inventory.Exchange(0, 4130001, -1, 4021008, -2, 4021006, -1, 4000021, -40, 4000030, -25, 4003000, -20);
                    else if (index == 107) ret = inventory.Exchange(0, 4130001, -1, 4021000, -4, 4000030, -40, 4000043, -35, 4003000, -25);
                    else if (index == 108) ret = inventory.Exchange(0, 4130001, -1, 4021005, -4, 4000030, -40, 4000037, -70, 4003000, -25);
                    else if (index == 109) ret = inventory.Exchange(0, 4130001, -1, 4011006, -2, 4021007, -1, 4000030, -40, 4000027, -20, 4003000, -25);
                    else if (index == 110) ret = inventory.Exchange(0, 4130001, -1, 4021008, -2, 4021007, -1, 4000030, -40, 4000014, -30, 4003000, -30);
                    else if (index == 111) ret = inventory.Exchange(0, 4130001, -1, 4021009, -1, 4011006, -3, 4021000, -3, 4000030, -60, 4003000, -40);
                    else if (index == 112) ret = inventory.Exchange(0, 4130001, -1, 4021009, -1, 4011006, -3, 4021005, -3, 4000030, -60, 4003000, -40);
                    else if (index == 113) ret = inventory.Exchange(0, 4130001, -1, 4021009, -1, 4011006, -3, 4021001, -3, 4000030, -60, 4003000, -40);
                    else if (index == 114) ret = inventory.Exchange(0, 4130001, -1, 4021009, -1, 4011006, -3, 4021003, -3, 4000030, -60, 4003000, -40);
                    // failure to create bowman shoes
                    else if (index == 200) ret = inventory.Exchange(0, 4130001, -1, 4021000, -2, 4000021, -50, 4003000, -15);
                    else if (index == 201) ret = inventory.Exchange(0, 4130001, -1, 4021005, -2, 4000021, -50, 4003000, -15);
                    else if (index == 202) ret = inventory.Exchange(0, 4130001, -1, 4021003, -2, 4000021, -50, 4003000, -15);
                    else if (index == 203) ret = inventory.Exchange(0, 4130001, -1, 4021004, -2, 4000021, -50, 4003000, -15);
                    else if (index == 204) ret = inventory.Exchange(0, 4130001, -1, 4021006, -2, 4000021, -50, 4003000, -15);
                    else if (index == 205) ret = inventory.Exchange(0, 4130001, -1, 4021002, -3, 4021006, -1, 4000021, -30, 4000030, -15, 4003000, -20);
                    else if (index == 206) ret = inventory.Exchange(0, 4130001, -1, 4021003, -3, 4021006, -1, 4000021, -30, 4000030, -15, 4003000, -20);
                    else if (index == 207) ret = inventory.Exchange(0, 4130001, -1, 4021000, -3, 4021006, -1, 4000021, -30, 4000030, -15, 4003000, -20);
                    else if (index == 208) ret = inventory.Exchange(0, 4130001, -1, 4021000, -4, 4000030, -45, 4000024, -40, 4003000, -30);
                    else if (index == 209) ret = inventory.Exchange(0, 4130001, -1, 4021006, -4, 4000030, -45, 4000027, -20, 4003000, -30);
                    else if (index == 210) ret = inventory.Exchange(0, 4130001, -1, 4011003, -5, 4000030, -45, 4000044, -40, 4003000, -30);
                    else if (index == 211) ret = inventory.Exchange(0, 4130001, -1, 4021002, -5, 4000030, -45, 4000009, -120, 4003000, -30);
                    else if (index == 212) ret = inventory.Exchange(0, 4130001, -1, 4021008, -1, 4011001, -3, 4021006, -3, 4000030, -60, 4000033, -80, 4003000, -35);
                    else if (index == 213) ret = inventory.Exchange(0, 4130001, -1, 4021008, -1, 4011001, -3, 4021006, -3, 4000030, -60, 4000032, -150, 4003000, -35);
                    else if (index == 214) ret = inventory.Exchange(0, 4130001, -1, 4021008, -1, 4011001, -3, 4021006, -3, 4000030, -60, 4000041, -100, 4003000, -35);
                    else if (index == 215) ret = inventory.Exchange(0, 4130001, -1, 4021008, -1, 4011001, -3, 4021006, -3, 4000030, -60, 4000042, -250, 4003000, -35);
                    // failure to create thief shoes            else if ( index == 300 ) ret = inventory.Exchange( 0, 4130001, -1, 4011000, -3, 4000021, -50, 4003000, -15 );
                    else if (index == 301) ret = inventory.Exchange(0, 4130001, -1, 4011001, -3, 4000021, -50, 4003000, -15);
                    else if (index == 302) ret = inventory.Exchange(0, 4130001, -1, 4011004, -2, 4000021, -50, 4003000, -15);
                    else if (index == 303) ret = inventory.Exchange(0, 4130001, -1, 4011006, -2, 4000021, -50, 4003000, -15);
                    else if (index == 304) ret = inventory.Exchange(0, 4130001, -1, 4021000, -3, 4021004, -1, 4000021, -30, 4000030, -15, 4003000, -20);
                    else if (index == 305) ret = inventory.Exchange(0, 4130001, -1, 4021003, -3, 4021004, -1, 4000021, -30, 4000030, -15, 4003000, -20);
                    else if (index == 306) ret = inventory.Exchange(0, 4130001, -1, 4021002, -3, 4021004, -1, 4000021, -30, 4000030, -15, 4003000, -20);
                    else if (index == 307) ret = inventory.Exchange(0, 4130001, -1, 4021003, -4, 4000030, -45, 4000032, -30, 4003000, -30);
                    else if (index == 308) ret = inventory.Exchange(0, 4130001, -1, 4021006, -4, 4000030, -45, 4000040, -3, 4003000, -30);
                    else if (index == 309) ret = inventory.Exchange(0, 4130001, -1, 4021005, -4, 4000030, -45, 4000037, -70, 4003000, -30);
                    else if (index == 310) ret = inventory.Exchange(0, 4130001, -1, 4021000, -5, 4000030, -45, 4000033, -50, 4003000, -30);
                    else if (index == 311) ret = inventory.Exchange(0, 4130001, -1, 4011007, -2, 4021005, -3, 4000030, -50, 4000037, -200, 4003000, -35);
                    else if (index == 312) ret = inventory.Exchange(0, 4130001, -1, 4011007, -2, 4021003, -3, 4000030, -50, 4000045, -80, 4003000, -35);
                    else if (index == 313) ret = inventory.Exchange(0, 4130001, -1, 4011007, -2, 4021000, -3, 4000030, -50, 4000043, -150, 4003000, -35);
                    else if (index == 314) ret = inventory.Exchange(0, 4130001, -1, 4011007, -2, 4021001, -3, 4000030, -50, 4000036, -80, 4003000, -35);

                    if (ret == 0) self.Say("Please make sure you have all the necessary items with you. Otherwise, check your equipment inventory, it may be full.");
                    else self.Say("Damn... I must have put in too much stimulator... All items are gone now... I'm sorry. I warned you about the possibility of this happening, I hope you understand.");
                }
                else
                {
                    // successfully creating warrior shoes
                    if (index == 0) ret = inventory.ExchangeEx(-20000, "4130001", -1, "4021003", -4, "4011001", -2, "4000021", -45, "4003000", -15, "1072003,Variation:1", 1);
                    else if (index == 1) ret = inventory.ExchangeEx(-20000, "4130001", -1, "4011002", -4, "4011001", -2, "4000021", -45, "4003000", -15, "1072039,Variation:1", 1);
                    else if (index == 2) ret = inventory.ExchangeEx(-20000, "4130001", -1, "4011004", -4, "4011001", -2, "4000021", -45, "4003000", -15, "1072040,Variation:1", 1);
                    else if (index == 3) ret = inventory.ExchangeEx(-20000, "4130001", -1, "4021000", -4, "4011001", -2, "4000021", -45, "4003000", -15, "1072041,Variation:1", 1);
                    else if (index == 4) ret = inventory.ExchangeEx(-22000, "4130001", -1, "4011001", -3, "4021004", -1, "4000021", -30, "4000030", -20, "4003000", -25, "1072002,Variation:1", 1);
                    else if (index == 5) ret = inventory.ExchangeEx(-22000, "4130001", -1, "4011002", -3, "4021004", -1, "4000021", -30, "4000030", -20, "4003000", -25, "1072112,Variation:1", 1);
                    else if (index == 6) ret = inventory.ExchangeEx(-25000, "4130001", -1, "4021008", -2, "4021004", -1, "4000021", -30, "4000030", -20, "4003000", -25, "1072113,Variation:1", 1);
                    else if (index == 7) ret = inventory.ExchangeEx(-38000, "4130001", -1, "4011003", -4, "4000021", -100, "4000030", -40, "4003000", -30, "4000103", -100, "1072000,Variation:1", 1);
                    else if (index == 8) ret = inventory.ExchangeEx(-38000, "4130001", -1, "4011005", -4, "4021007", -1, "4000030", -40, "4003000", -30, "4000104", -100, "1072126,Variation:1", 1);
                    else if (index == 9) ret = inventory.ExchangeEx(-38000, "4130001", -1, "4011002", -4, "4021007", -1, "4000030", -40, "4003000", -30, "4000105", -100, "1072127,Variation:1", 1);
                    else if (index == 10) ret = inventory.ExchangeEx(-50000, "4130001", -1, "4021008", -1, "4011001", -3, "4021003", -6, "4000030", -65, "4003000", -45, "1072132,Variation:1", 1);
                    else if (index == 11) ret = inventory.ExchangeEx(-50000, "4130001", -1, "4021008", -1, "4011001", -3, "4011002", -6, "4000030", -65, "4003000", -45, "1072133,Variation:1", 1);
                    else if (index == 12) ret = inventory.ExchangeEx(-50000, "4130001", -1, "4021008", -1, "4011001", -3, "4011005", -6, "4000030", -65, "4003000", -45, "1072134,Variation:1", 1);
                    else if (index == 13) ret = inventory.ExchangeEx(-50000, "4130001", -1, "4021008", -1, "4011001", -3, "4011006", -6, "4000030", -65, "4003000", -45, "1072135,Variation:1", 1);
                    // successfully creating  magician shoes
                    else if (index == 100) ret = inventory.ExchangeEx(-18000, "4130001", -1, "4021000", -2, "4000021", -50, "4003000", -15, "1072075,Variation:1", 1);
                    else if (index == 101) ret = inventory.ExchangeEx(-18000, "4130001", -1, "4021002", -2, "4000021", -50, "4003000", -15, "1072076,Variation:1", 1);
                    else if (index == 102) ret = inventory.ExchangeEx(-18000, "4130001", -1, "4011004", -2, "4000021", -50, "4003000", -15, "1072077,Variation:1", 1);
                    else if (index == 103) ret = inventory.ExchangeEx(-18000, "4130001", -1, "4021008", -1, "4000021", -50, "4003000", -15, "1072078,Variation:1", 1);
                    else if (index == 104) ret = inventory.ExchangeEx(-20000, "4130001", -1, "4021001", -3, "4021006", -1, "4000021", -30, "4000030", -15, "4003000", -20, "1072089,Variation:1", 1);
                    else if (index == 105) ret = inventory.ExchangeEx(-20000, "4130001", -1, "4021000", -3, "4021006", -1, "4000021", -30, "4000030", -15, "4003000", -20, "1072090,Variation:1", 1);
                    else if (index == 106) ret = inventory.ExchangeEx(-22000, "4130001", -1, "4021008", -2, "4021006", -1, "4000021", -40, "4000030", -25, "4003000", -20, "1072091,Variation:1", 1);
                    else if (index == 107) ret = inventory.ExchangeEx(-30000, "4130001", -1, "4021000", -4, "4000030", -40, "4000110", -100, "4003000", -25, "1072114,Variation:1", 1);
                    else if (index == 108) ret = inventory.ExchangeEx(-30000, "4130001", -1, "4021005", -4, "4000030", -40, "4000111", -100, "4003000", -25, "1072115,Variation:1", 1);
                    else if (index == 109) ret = inventory.ExchangeEx(-35000, "4130001", -1, "4011006", -2, "4021007", -1, "4000030", -40, "4000100", -100, "4003000", -25, "1072116,Variation:1", 1);
                    else if (index == 110) ret = inventory.ExchangeEx(-40000, "4130001", -1, "4021008", -2, "4021007", -1, "4000030", -40, "4000112", -100, "4003000", -30, "1072117,Variation:1", 1);
                    else if (index == 111) ret = inventory.ExchangeEx(-50000, "4130001", -1, "4021009", -1, "4011006", -3, "4021000", -3, "4000030", -60, "4003000", -40, "1072140,Variation:1", 1);
                    else if (index == 112) ret = inventory.ExchangeEx(-50000, "4130001", -1, "4021009", -1, "4011006", -3, "4021005", -3, "4000030", -60, "4003000", -40, "1072141,Variation:1", 1);
                    else if (index == 113) ret = inventory.ExchangeEx(-50000, "4130001", -1, "4021009", -1, "4011006", -3, "4021001", -3, "4000030", -60, "4003000", -40, "1072142,Variation:1", 1);
                    else if (index == 114) ret = inventory.ExchangeEx(-50000, "4130001", -1, "4021009", -1, "4011006", -3, "4021003", -3, "4000030", -60, "4003000", -40, "1072143,Variation:1", 1);
                    // successfully creating bowman shoes
                    else if (index == 200) ret = inventory.ExchangeEx(-19000, "4130001", -1, "4021000", -2, "4000021", -50, "4003000", -15, "1072079,Variation:1", 1);
                    else if (index == 201) ret = inventory.ExchangeEx(-19000, "4130001", -1, "4021005", -2, "4000021", -50, "4003000", -15, "1072080,Variation:1", 1);
                    else if (index == 202) ret = inventory.ExchangeEx(-19000, "4130001", -1, "4021003", -2, "4000021", -50, "4003000", -15, "1072081,Variation:1", 1);
                    else if (index == 203) ret = inventory.ExchangeEx(-19000, "4130001", -1, "4021004", -2, "4000021", -50, "4003000", -15, "1072082,Variation:1", 1);
                    else if (index == 204) ret = inventory.ExchangeEx(-19000, "4130001", -1, "4021006", -2, "4000021", -50, "4003000", -15, "1072083,Variation:1", 1);
                    else if (index == 205) ret = inventory.ExchangeEx(-20000, "4130001", -1, "4021002", -3, "4021006", -1, "4000021", -30, "4000030", -15, "4003000", -20, "1072101,Variation:1", 1);
                    else if (index == 206) ret = inventory.ExchangeEx(-20000, "4130001", -1, "4021003", -3, "4021006", -1, "4000021", -30, "4000030", -15, "4003000", -20, "1072102,Variation:1", 1);
                    else if (index == 207) ret = inventory.ExchangeEx(-20000, "4130001", -1, "4021000", -3, "4021006", -1, "4000021", -30, "4000030", -15, "4003000", -20, "1072103,Variation:1", 1);
                    else if (index == 208) ret = inventory.ExchangeEx(-32000, "4130001", -1, "4021000", -4, "4000030", -45, "4000106", -100, "4003000", -30, "1072118,Variation:1", 1);
                    else if (index == 209) ret = inventory.ExchangeEx(-32000, "4130001", -1, "4021006", -4, "4000030", -45, "4000107", -100, "4003000", -30, "1072119,Variation:1", 1);
                    else if (index == 210) ret = inventory.ExchangeEx(-40000, "4130001", -1, "4011003", -5, "4000030", -45, "4000108", -100, "4003000", -30, "1072120,Variation:1", 1);
                    else if (index == 211) ret = inventory.ExchangeEx(-40000, "4130001", -1, "4021002", -5, "4000030", -45, "4000099", -100, "4003000", -30, "1072121,Variation:1", 1);
                    else if (index == 212) ret = inventory.ExchangeEx(-50000, "4130001", -1, "4021008", -1, "4011001", -3, "4021006", -3, "4000030", -60, "4000033", -80, "4003000", -35, "1072122,Variation:1", 1);
                    else if (index == 213) ret = inventory.ExchangeEx(-50000, "4130001", -1, "4021008", -1, "4011001", -3, "4021006", -3, "4000030", -60, "4000032", -150, "4003000", -35, "1072123,Variation:1", 1);
                    else if (index == 214) ret = inventory.ExchangeEx(-50000, "4130001", -1, "4021008", -1, "4011001", -3, "4021006", -3, "4000030", -60, "4000041", -100, "4003000", -35, "1072124,Variation:1", 1);
                    else if (index == 215) ret = inventory.ExchangeEx(-50000, "4130001", -1, "4021008", -1, "4011001", -3, "4021006", -3, "4000030", -60, "4000042", -250, "4003000", -35, "1072125,Variation:1", 1);
                    // successfully creating thief shoes 
                    else if (index == 300) ret = inventory.ExchangeEx(-19000, "4130001", -1, "4011000", -3, "4000021", -50, "4003000", -15, "1072032,Variation:1", 1);
                    else if (index == 301) ret = inventory.ExchangeEx(-19000, "4130001", -1, "4011001", -3, "4000021", -50, "4003000", -15, "1072033,Variation:1", 1);
                    else if (index == 302) ret = inventory.ExchangeEx(-19000, "4130001", -1, "4011004", -2, "4000021", -50, "4003000", -15, "1072035,Variation:1", 1);
                    else if (index == 303) ret = inventory.ExchangeEx(-21000, "4130001", -1, "4011006", -2, "4000021", -50, "4003000", -15, "1072036,Variation:1", 1);
                    else if (index == 304) ret = inventory.ExchangeEx(-20000, "4130001", -1, "4021000", -3, "4021004", -1, "4000021", -30, "4000030", -15, "4003000", -20, "1072104,Variation:1", 1);
                    else if (index == 305) ret = inventory.ExchangeEx(-20000, "4130001", -1, "4021003", -3, "4021004", -1, "4000021", -30, "4000030", -15, "4003000", -20, "1072105,Variation:1", 1);
                    else if (index == 306) ret = inventory.ExchangeEx(-20000, "4130001", -1, "4021002", -3, "4021004", -1, "4000021", -30, "4000030", -15, "4003000", -20, "1072106,Variation:1", 1);
                    else if (index == 307) ret = inventory.ExchangeEx(-32000, "4130001", -1, "4021003", -4, "4000030", -45, "4000095", -100, "4003000", -30, "1072108,Variation:1", 1);
                    else if (index == 308) ret = inventory.ExchangeEx(-35000, "4130001", -1, "4021006", -4, "4000030", -45, "4000096", -100, "4003000", -30, "1072109,Variation:1", 1);
                    else if (index == 309) ret = inventory.ExchangeEx(-35000, "4130001", -1, "4021005", -4, "4000030", -45, "4000097", -100, "4003000", -30, "1072110,Variation:1", 1);
                    else if (index == 310) ret = inventory.ExchangeEx(-40000, "4130001", -1, "4021000", -5, "4000030", -45, "4000113", -100, "4003000", -30, "1072107,Variation:1", 1);
                    else if (index == 311) ret = inventory.ExchangeEx(-50000, "4130001", -1, "4011007", -2, "4021005", -3, "4000030", -50, "4000114", -100, "4003000", -35, "1072128,Variation:1", 1);
                    else if (index == 312) ret = inventory.ExchangeEx(-50000, "4130001", -1, "4011007", -2, "4021003", -3, "4000030", -50, "4000109", -100, "4003000", -35, "1072129,Variation:1", 1);
                    else if (index == 313) ret = inventory.ExchangeEx(-50000, "4130001", -1, "4011007", -2, "4021000", -3, "4000030", -50, "4000115", -100, "4003000", -35, "1072130,Variation:1", 1);
                    else if (index == 314) ret = inventory.ExchangeEx(-50000, "4130001", -1, "4011007", -2, "4021001", -3, "4000030", -50, "4000036", -80, "4003000", -35, "1072131,Variation:1", 1);

                    if (ret == 0) self.Say("Please make sure you have all the necessary items with you. Otherwise, check your equipment inventory, it might be full.");
                    else self.Say("Here, take this, the " + makeItem + "! Everything worked out, and they are much more powerful than a regular pair of shoes. Hope to see you again~");
                }
            }
        }
        // Creating shoes at Ludibrium
        [Script("make_ludi3")]
        class make_ludi3 : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                make5.self = self;
                make5.target = target;
                var nRet1 = self.AskYesNo("Well, well. Hello! I've been working all day, I haven't even had my lunch yet, just to make these shoes. I'm starving, but my business is doing well, so I can't complain. If you're looking for a good pair of shoes, you've come to the perfect place. How about you let me make you a pair?");
                if (nRet1 == 0) self.Say("I'm different from all the shoemakers in this area. The special shoes I make can only be made by me. If you change your mind, just stop by.");
                else
                {
                    var v1 = self.AskMenu("Oh, it might be a little pricey, but I'm sure you understand. What do you want to do?",
                        "What is the stimulator?",
                        "Create Warrior shoes using the stimulator",
                        "Create Magician shoes using the stimulator",
                        "Create Bowman shoes using the stimulator",
                        "Create Thief shoes using the stimulator",
                        "Create regular Warrior shoes",
                        "Create regular Magician shoes",
                        "Create regular Bowman shoes",
                        "Create regular Thief shoes"
                    );
                    string jobName = Strings.Job.GetName((short)((((v1-1) % 4)+1) * 100));
                    string ask;
                    if (v1 < 5)
                    {
                        ask = "Which pair of " + jobName + " shoes do you want to upgrade using the stimulator?";
                    }
                    else
                    {
                        ask = "Do you want to upgrade a regular pair of " + jobName + " shoes? There is no chance of failure as the stimulator will not be used, and the item option will be basic. Please choose the item you want to create~";
                    }

                    if (v1 == 0)
                    {
                        self.Say("In Ludibrium, many items can be crafted using the #bstimulator#k. It's a mysterious potion that's included in the crafting process for a pair of shoes, and after they're used, the shoes will be crafted with a slight attack boost option, just as if you received the shoes from a monster. The stimulator can be used not only on shoes but also on other items. Be sure to carry plenty of them with you as there are different types of stimulators available for different types of shoes.");
                        self.Say("But you should be aware of a few things. If the stimulator is used, it is very likely that the item choice will be changed, and the problem with this is that the result can #bend up being worse#k, way worse than the original. You will also risk a #b10% failure rate when crafting the item#k, which means saying goodbye to the items you used to craft the item in question. Sucks, right?");
                        self.Say("Even with these related risks, many adventurers seek my help in creating perfect pairs of shoes using the stimulator. The thought of the item being of poor quality, getting worse than it was, or even disappearing might scare you, but why not give it a try anyway? If you're lucky, your item might be a wonder. That's all I can tell you.");
                    }
                    else if (v1 == 1)
                    {
                        var v2 = self.AskMenu(ask + "\r\n#L0##b #t1072003##k (Level Requirement: 30, Warrior)#l\r\n#L1##b #t1072039##k (Level Requirement: 30, Warrior)#l\r\n#L2##b #t1072040##k (Level Requirement: 30, Warrior)#l\r\n#L3##b #t1072041##k (Level Requirement: 30, Warrior)#l\r\n#L4##b #t1072002##k (Level Requirement: 35, Warrior)#l\r\n#L5##b #t1072112##k (Level Requirement: 35, Warrior)#l\r\n#L6##b #t1072113##k (Level Requirement: 35, Warrior)#l\r\n#L7##b #t1072000##k (Level Requirement: 40, Warrior)#l\r\n#L8##b #t1072126##k (Level Requirement: 40, Warrior)#l\r\n#L9##b #t1072127##k (Level Requirement: 40, Warrior)#l\r\n#L10##b #t1072132##k (Level Requirement: 50, Warrior)#l\r\n#L11##b #t1072133##k (Level Requirement: 50, Warrior)#l\r\n#L12##b #t1072134##k (Level Requirement: 50, Warrior)#l\r\n#L13##b #t1072135##k (Level Requirement: 50, Warrior)#l");
                        if (v2 == 0) chat_message1(0, "#t1072003#", "#v4130001# #t4130001#\r\n#v4021003# 4 #t4021003#s\r\n#v4011001# 2 #t4011001#s\r\n#v4000021# 45 #t4000021#s\r\n#v4003000# 15 #t4003000#s\r\n#v4031138# 20,000 mesos", 30, "STR +1");
                        else if (v2 == 1) chat_message1(1, "#t1072039#", "#v4130001# #t4130001#\r\n#v4011002# 4 #t4011002#s\r\n#v4011001# 2 #t4011001#s\r\n#v4000021# 45 #t4000021#s\r\n#v4003000# 15 #t4003000#s\r\n#v4031138# 20,000 mesos", 30, "DEX +1");
                        else if (v2 == 2) chat_message1(2, "#t1072040#", "#v4130001# #t4130001#\r\n#v4011004# 4 #t4011004#s\r\n#v4011001# 2 #t4011001#s\r\n#v4000021# 45 #t4000021#s\r\n#v4003000# 15 #t4003000#s\r\n#v4031138# 20,000 mesos", 30, "STR +1");
                        else if (v2 == 3) chat_message1(3, "#t1072041#", "#v4130001# #t4130001#\r\n#v4021000# 4 #t4021000#s\r\n#v4011001# 2 #t4011001#s\r\n#v4000021# 45 #t4000021#s\r\n#v4003000# 15 #t4003000#s\r\n#v4031138# 20,000 mesos", 30, "DEX +1");
                        else if (v2 == 4) chat_message1(4, "#t1072002#", "#v4130001# #t4130001#\r\n#v4011001# 3 #t4011001#s\r\n#v4021004# #t4021004#\r\n#v4000021# 30 #t4000021#s\r\n#v4000030# 20 #t4000030#s\r\n#v4003000# 25 #t4003000#s\r\n#v4031138# 22,000 mesos", 35, "STR +1, MP +10");
                        else if (v2 == 5) chat_message1(5, "#t1072112#", "#v4130001# #t4130001#\r\n#v4011002# 3 #t4011002#s\r\n#v4021004# #t4021004#\r\n#v4000021# 30 #t4000021#s\r\n#v4000030# 20 #t4000030#s\r\n#v4003000# 25 #t4003000#s\r\n#v4031138# 22,000 mesos", 35, "DEX +1, MP +10");
                        else if (v2 == 6) chat_message1(6, "#t1072113#", "#v4130001# #t4130001#\r\n#v4021008# 2 #t4021008#s\r\n#v4021004# #t4021004#\r\n#v4000021# 30 #t4000021#s\r\n#v4000030# 20 #t4000030#s\r\n#v4003000# 25 #t4003000#s\r\n#v4031138# 25,000 mesos", 35, "STR +1, DEX +1, MP +10");
                        else if (v2 == 7) chat_message1(7, "#t1072000#", "#v4130001# #t4130001#\r\n#v4011003# 4 #t4011003#s\r\n#v4000021# 100 #t4000021#s\r\n#v4000030# 40 #t4000030#s\r\n#v4003000# 30 #t4003000#s\r\n#v4000103# 100 #t4000103#s\r\n#v4031138# 38,000 mesos", 40, "DEX +2, MP +20");
                        else if (v2 == 8) chat_message1(8, "#t1072126#", "#v4130001# #t4130001#\r\n#v4011005# 4 #t4011005#s\r\n#v4021007# #t4021007#\r\n#v4000030# 40 #t4000030#s\r\n#v4003000# 30 #t4003000#s\r\n#v4000104# 100 #t4000104#s\r\n#v4031138# 38,000 mesos", 40, "STR +1, DEX +1, MP +20");
                        else if (v2 == 9) chat_message1(9, "#t1072127#", "#v4130001# #t4130001#\r\n#v4011002# 4 #t4011002#s\r\n#v4021007# #t4021007#\r\n#v4000030# 40 #t4000030#s\r\n#v4003000# 30 #t4003000#s\r\n#v4000105# 100 #t4000105#s\r\n#v4031138# 38,000 mesos", 40, "STR +2, MP +20");
                        else if (v2 == 10) chat_message1(10, "#t1072132#", "#v4130001# #t4130001#\r\n#v4021008# #t4021008#\r\n#v4011001# 3 #t4011001#s\r\n#v4021003# 6 #t4021003#s\r\n#v4000030# 65 #t4000030#s\r\n#v4003000# 45 #t4003000#s\r\n#v4031138# 50,000 mesos", 50, "DEX +3");
                        else if (v2 == 11) chat_message1(11, "#t1072133#", "#v4130001# #t4130001#\r\n#v4021008# #t4021008#\r\n#v4011001# 3 #t4011001#s\r\n#v4011002# 6 #t4011002#s\r\n#v4000030# 65 #t4000030#s\r\n#v4003000# 45 #t4003000#s\r\n#v4031138# 50,000 mesos", 50, "DEX + 2, STR + 1");
                        else if (v2 == 12) chat_message1(12, "#t1072134#", "#v4130001# #t4130001#\r\n#v4021008# #t4021008#\r\n#v4011001# 3 #t4011001#s\r\n#v4011005# 6 #t4011005#s\r\n#v4000030# 65 #t4000030#s\r\n#v4003000# 45 #t4003000#s\r\n#v4031138# 50,000 mesos", 50, "DEX + 1, STR + 2");
                        else if (v2 == 13) chat_message1(13, "#t1072135#", "#v4130001# #t4130001#\r\n#v4021008# #t4021008#\r\n#v4011001# 3 #t4011001#s\r\n#v4011006# 6 #t4011006#s\r\n#v4000030# 65 #t4000030#s\r\n#v4003000# 45 #t4003000#s\r\n#v4031138# 50,000 mesos", 50, "STR +3");
                    }
                    else if (v1 == 2)
                    {
                        var v2 = self.AskMenu(ask + "\r\n#L0##b #t1072075##k (Level Requirement: 30, Magician)#l\r\n#L1##b #t1072076##k (Level Requirement: 30, Magician)#l\r\n#L2##b #t1072077##k (Level Requirement: 30, Magician)#l\r\n#L3##b #t1072078##k (Level Requirement: 30, Magician)#l\r\n#L4##b #t1072089##k (Level Requirement: 35, Magician)#l\r\n#L5##b #t1072090##k (Level Requirement: 35, Magician)#l\r\n#L6##b #t1072091##k (Level Requirement: 35, Magician)#l\r\n#L7##b #t1072114##k (Level Requirement: 40, Magician)#l\r\n#L8##b #t1072115##k (Level Requirement: 40, Magician)#l\r\n#L9##b #t1072116##k (Level Requirement: 40, Magician)#l\r\n#L10##b #t1072117##k (Level Requirement: 40, Magician)#l\r\n#L11##b #t1072140##k (Level Requirement: 50, Magician)#l\r\n#L12##b #t1072141##k (Level Requirement: 50, Magician)#l\r\n#L13##b #t1072142##k (Level Requirement: 50, Magician)#l\r\n#L14##b #t1072143##k (Level Requirement: 50, Magician)#l");
                        if (v2 == 0) chat_message1(100, "#t1072075#", "#v4130001# #t4130001#\r\n#v4021000# 2 #t4021000#s\r\n#v4000021# 50 #t4000021#s\r\n#v4003000# 15 #t4003000#s\r\n#v4031138# 18,000 mesos", 30, "INT +1");
                        else if (v2 == 1) chat_message1(101, "#t1072076#", "#v4130001# #t4130001#\r\n#v4021002# 2 #t4021002#s\r\n#v4000021# 50 #t4000021#s\r\n#v4003000# 15 #t4003000#s\r\n#v4031138# 18,000 mesos", 30, "INT +1");
                        else if (v2 == 2) chat_message1(102, "#t1072077#", "#v4130001# #t4130001#\r\n#v4011004# 2 #t4011004#s\r\n#v4000021# 50 #t4000021#s\r\n#v4003000# 15 #t4003000#s\r\n#v4031138# 18,000 mesos", 30, "INT +1");
                        else if (v2 == 3) chat_message1(103, "#t1072078#", "#v4130001# #t4130001#\r\n#v4021008# #t4021008#\r\n#v4000021# 50 #t4000021#s\r\n#v4003000# 15 #t4003000#s\r\n#v4031138# 18,000 mesos", 30, "INT +2");
                        else if (v2 == 4) chat_message1(104, "#t1072089#", "#v4130001# #t4130001#\r\n#v4021001# 3 #t4021001#s\r\n#v4021006# #t4021006#\r\n#v4000021# 30 #t4000021#s\r\n#v4000030# 15 #t4000030#s\r\n#v4003000 20 #t4003000#s\r\n#v4031138# 20,000 mesos", 35, "LUK +1, MP +10");
                        else if (v2 == 5) chat_message1(105, "#t1072090#", "#v4130001# #t4130001#\r\n#v4021000# 3 #t4021000#s\r\n#v4021006# #t4021006#\r\n#v4000021# 30 #t4000021#s\r\n#v4000030# 15 #t4000030#s\r\n#v4003000# 20 #t4003000#s\r\n#v4031138# 20,000 mesos", 35, "INT +1, MP +10");
                        else if (v2 == 6) chat_message1(106, "#t1072091#", "#v4130001# #t4130001#\r\n#v4021008# 2 #t4021008#s\r\n#v4021006# #t4021006#\r\n#v4000021# 40 #t4000021#s\r\n#v4000030# 25 #t4000030#s\r\n#v4003000# 20 #t4003000#s\r\n#v4031138# 22,000 mesos", 35, "INT +1, LUK +1");
                        else if (v2 == 7) chat_message1(107, "#t1072114#", "#v4130001# #t4130001#\r\n#v4021000# 4 #t4021000#s\r\n#v4000030# 40 #t4000030#s\r\n#v4000110# 100 #t4000110#s\r\n#v4003000# 25 #t4003000#s\r\n#v4031138# 30,000 mesos", 40, "LUK +2, MP +5");
                        else if (v2 == 8) chat_message1(108, "#t1072115#", "#v4130001# #t4130001#\r\n#v4021005# 4 #t4021005#s\r\n#v4000030# 40 #t4000030#s\r\n#v4000111# 100 #t4000111#s\r\n#v4003000# 25 #t4003000#s\r\n#v4031138# 30,000 mesos", 40, "LUK +2, MP +5");
                        else if (v2 == 9) chat_message1(109, "#t1072116#", "#v4130001# #t4130001#\r\n#v4011006# 2 #t4011006#s\r\n#v4021007# #t4021007#\r\n#v4000030# 40 #t4000030#s\r\n#v4000100# 100 #t4000100#s\r\n#v4003000# 25 #t4003000#s\r\n#v4031138# 35,000 mesos", 40, "INT +2, MP +5");
                        else if (v2 == 10) chat_message1(110, "#t1072117#", "#v4130001# #t4130001#\r\n#v4021008# 2 #t4021008#s\r\n#v4021007# #t4021007#\r\n#v4000030# 40 #t4000030#s\r\n#v4000112# 100 #t4000112#s\r\n#v4003000# 30 #t4003000#s\r\n#v4031138# 40,000 mesos", 40, "INT +2, LUK +1, MP +10");
                        else if (v2 == 11) chat_message1(111, "#t1072140#", "#v4130001# #t4130001#\r\n#v4021009# #t4021009#\r\n#v4011006# 3 #t4011006#s\r\n#v4021000# 3 #t4021000#s\r\n#v4000030# 60 #t4000030#s\r\n#v4003000# 40 #t4003000#s\r\n#v4031138# 50,000 mesos", 50, "LUK +3");
                        else if (v2 == 12) chat_message1(112, "#t1072141#", "#v4130001# #t4130001#\r\n#v4021009# #t4021009#\r\n#v4011006# 3 #t4011006#s\r\n#v4021005# 3 #t4021005#s\r\n#v4000030# 60 #t4000030#s\r\n#v4003000# 40 #t4003000#s\r\n#v4031138# 50,000 mesos", 50, "INT +1, LUK +2");
                        else if (v2 == 13) chat_message1(113, "#t1072142#", "#v4130001# #t4130001#\r\n#v4021009# #t4021009#\r\n#v4011006# 3 #t4011006#s\r\n#v4021001# 3 #t4021001#s\r\n#v4000030# 60 #t4000030#s\r\n#v4003000# 40 #t4003000#s\r\n#v4031138# 50,000 mesos", 50, "INT +2, LUK +1");
                        else if (v2 == 14) chat_message1(114, "#t1072143#", "#v4130001# #t4130001#\r\n#v4021009# #t4021009#\r\n#v4011006# 3 #t4011006#s\r\n#v4021003# 3 #t4021003#s\r\n#v4000030# 60 #t4000030#s\r\n#v4003000# 40 #t4003000#s\r\n#v4031138# 50,000 mesos", 50, "INT +3");
                    }
                    else if (v1 == 3)
                    {
                        var v2 = self.AskMenu(ask + "\r\n#L0##b #t1072079##k (Level Requirement: 30, Bowman)#l\r\n#L1##b #t1072080##k (Level Requirement: 30, Bowman)#l\r\n#L2##b #t1072081##k (Level Requirement: 30, Bowman)#l\r\n#L3##b #t1072082##k (Level Requirement: 30, Bowman)#l\r\n#L4##b #t1072083##k (Level Requirement: 30, Bowman)#l\r\n#L5##b #t1072101##k (Level Requirement: 35, Bowman)#l\r\n#L6##b #t1072102##k (Level Requirement: 35, Bowman)#l\r\n#L7##b #t1072103##k (Level Requirement: 35, Bowman)#l\r\n#L8##b #t1072118##k (Level Requirement: 40, Bowman)#l\r\n#L9##b #t1072119##k (Level Requirement: 40, Bowman)#l\r\n#L10##b #t1072120##k (Level Requirement: 40, Bowman)#l\r\n#L11##b #t1072121##k (Level Requirement: 40, Bowman)#l\r\n#L12##b #t1072122##k (Level Requirement: 50, Bowman)#l\r\n#L13##b #t1072123##k (Level Requirement: 50, Bowman)#l\r\n#L14##b #t1072124##k (Level Requirement: 50, Bowman)#l\r\n#L15##b #t1072125##k (Level Requirement: 50, Bowman)#l");
                        if (v2 == 0) chat_message1(200, "#t1072079#", "#v4130001# #t4130001#\r\n#v4021000# 2 #t4021000#s\r\n#v4000021# 50 #t4000021#s\r\n#v4003000# 15 #t4003000#s\r\n#v4031138# 19,000 mesos", 30, "DEX +1");
                        else if (v2 == 1) chat_message1(201, "#t1072080#", "#v4130001# #t4130001#\r\n#v4021005# 2 #t4021005#s\r\n#v4000021# 50 #t4000021#s\r\n#v4003000# 15 #t4003000#s\r\n#v4031138# 19,000 mesos", 30, "STR +1");
                        else if (v2 == 2) chat_message1(202, "#t1072081#", "#v4130001# #t4130001#\r\n#v4021003# 2 #t4021003#s\r\n#v4000021# 50 #t4000021#s\r\n#v4003000# 15 #t4003000#s\r\n#v4031138# 19,000 mesos", 30, "DEX +1");
                        else if (v2 == 3) chat_message1(203, "#t1072082#", "#v4130001# #t4130001#\r\n#v4021004# 2 #t4021004#s\r\n#v4000021# 50 #t4000021#s\r\n#v4003000# 15 #t4003000#s\r\n#v4031138# 19,000 mesos", 30, "DEX +1");
                        else if (v2 == 4) chat_message1(204, "#t1072083#", "#v4130001# #t4130001#\r\n#v4021006# 2 #t4021006#s\r\n#v4000021# 50 #t4000021#s\r\n#v4003000# 15 #t4003000#s\r\n#v4031138# 19,000 mesos", 30, "STR +1");
                        else if (v2 == 5) chat_message1(205, "#t1072101#", "#v4130001# #t4130001#\r\n#v4021002# 3 #t4021002#s\r\n#v4021006# #t4021006#\r\n#v4000021# 30 #t4000021#s\r\n#v4000030# 15 #t4000030#s\r\n#v4003000# 20 #t4003000#s\r\n#v4031138# 20,000 mesos", 35, "STR +2");
                        else if (v2 == 6) chat_message1(206, "#t1072102#", "#v4130001# #t4130001#\r\n#v4021003# 3 #t4021003#s\r\n#v4021006# #t4021006#\r\n#v4000021# 30 #t4000021#s\r\n#v4000030# 15 #t4000030#s\r\n#v4003000# 20 #t4003000#s\r\n#v4031138# 20,000 mesos", 35, "DEX +2");
                        else if (v2 == 7) chat_message1(207, "#t1072103#", "#v4130001# #t4130001#\r\n#v4021000# 3 #t4021000#s\r\n#v4021006# #t4021006#\r\n#v4000021# 30 #t4000021#s\r\n#v4000030# 15 #t4000030#s\r\n#v4003000# 20 #t4003000#s\r\n#v4031138# 20,000 mesos", 35, "STR + 1, DEX + 1");
                        else if (v2 == 8) chat_message1(208, "#t1072118#", "#v4130001# #t4130001#\r\n#v4021000# 4 #t4021000#s\r\n#v4000030# 45 #t4000030#s\r\n#v4000106# 100 #t4000106#s\r\n#v4003000# 30 #t4003000#s\r\n#v4031138# 32,000 mesos", 40, "STR +2, MP +5");
                        else if (v2 == 9) chat_message1(209, "#t1072119#", "#v4130001# #t4130001#\r\n#v4021006# 4 #t4021006#s\r\n#v4000030# 45 #t4000030#s\r\n#v4000107# 100 #t4000107#s\r\n#v4003000# 30 #t4003000#s\r\n#v4031138# 32,000 mesos", 40, "DEX +1, STR +1, MP +5");
                        else if (v2 == 10) chat_message1(210, "#t1072120#", "#v4130001# #t4130001#\r\n#v4011003# 5 #t4011003#s\r\n#v4000030# 45 #t4000030#s\r\n#v4000108# 100 #t4000108#s\r\n#v4003000# 30 #t4003000#s\r\n#v4031138# 40,000 mesos", 40, "DEX +2, MP +5");
                        else if (v2 == 11) chat_message1(211, "#t1072121#", "#v4130001# #t4130001#\r\n#v4021002# 5 #t4021002#s\r\n#v4000030# 45 #t4000030#s\r\n#v4000099# 100 #t4000099#s\r\n#v4003000# 30 #t4003000#s\r\n#v4031138# 40,000 mesos", 40, "DEX +2, MP +5");
                        else if (v2 == 12) chat_message1(212, "#t1072122#", "#v4130001# #t4130001#\r\n#v4021008# #t4021008#\r\n#v4011001# 3 #t4011001#s\r\n#v4021006# 3 #t4021006#s\r\n#v4000030# 60 #t4000030#s\r\n#v4000033# 80 #t4000033#s\r\n#v4003000# 35 #t4003000#s\r\n#v4031138# 50,000 mesos", 50, "STR +3");
                        else if (v2 == 13) chat_message1(213, "#t1072123#", "#v4130001# #t4130001#\r\n#v4021008# #t4021008#\r\n#v4011001# 3 #t4011001#s\r\n#v4021006# 3 #t4021006#s\r\n#v4000030# 60 #t4000030#s\r\n#v4000032# 150 #t4000032#s\r\n#v4003000# 35 #t4003000#s\r\n#v4031138# 50,000 mesos", 50, "DEX + 1, STR + 2");
                        else if (v2 == 14) chat_message1(214, "#t1072124#", "#v4130001# #t4130001#\r\n#v4021008# #t4021008#\r\n#v4011001# 3 #t4011001#s\r\n#v4021006# 3 #t4021006#s\r\n#v4000030# 60 #t4000030#s\r\n#v4000041# 100 #t4000041#s\r\n#v4003000# 35 #t4003000#s\r\n#v4031138# 50,000 mesos", 50, "DEX + 2, STR + 1");
                        else if (v2 == 15) chat_message1(215, "#t1072125#", "#v4130001# #t4130001#\r\n#v4021008# #t4021008#\r\n#v4011001# 3 #t4011001#s\r\n#v4021006# 3 #t4021006#s\r\n#v4000030# 60 #t4000030#s\r\n#v4000042# 250 #t4000042#s\r\n#v4003000# 35 #t4003000#s\r\n#v4031138# 50,000 mesos", 50, "DEX +3");
                    }
                    else if (v1 == 4)
                    {
                        var v2 = self.AskMenu(ask + "\r\n#L0##b #t1072032##k (Level Requirement: 30, Thief)#l\r\n#L1##b #t1072033##k (Level Requirement: 30, Thief)#l\r\n#L2##b #t1072035##k (Level Requirement: 30, Thief)#l\r\n#L3##b #t1072036##k (Level Requirement: 30, Thief)#l\r\n#L4##b #t1072104##k (Level Requirement: 35, Thief)#l\r\n#L5##b #t1072105##k (Level Requirement: 35, Thief)#l\r\n#L6##b #t1072106##k (Level Requirement: 35, Thief)#l\r\n#L7##b #t1072108##k (Level Requirement: 40, Thief)#l\r\n#L8##b #t1072109##k (Level Requirement: 40, Thief)#l\r\n#L9##b #t1072110##k (Level Requirement: 40, Thief)#l\r\n#L10##b #t1072107##k (Level Requirement: 40, Thief)#l\r\n#L11##b #t1072128##k (Level Requirement: 50, Thief)#l\r\n#L12##b #t1072129##k (Level Requirement: 50, Thief)#l\r\n#L13##b #t1072130##k (Level Requirement: 50, Thief)#l\r\n#L14##b #t1072131##k (Level Requirement: 50, Thief)#l");
                        if (v2 == 0) chat_message1(300, "#t1072032#", "#v4130001# #t4130001#\r\n#v4011000# 3 #t4011000#s\r\n#v4000021# 50 #t4000021#s\r\n#v4003000# 15 #t4003000#s\r\n#v4031138# 19,000 mesos", 30, "DEX +1");
                        else if (v2 == 1) chat_message1(301, "#t1072033#", "#v4130001# #t4130001#\r\n#v4011001# 3 #t4011001#s\r\n#v4000021# 50 #t4000021#s\r\n#v4003000# 15 #t4003000#s\r\n#v4031138# 19,000 mesos", 30, "LUK +1");
                        else if (v2 == 2) chat_message1(302, "#t1072035#", "#v4130001# #t4130001#\r\n#v4011004# 2 #t4011004#s\r\n#v4000021# 50 #t4000021#s\r\n#v4003000# 15 #t4003000#s\r\n#v4031138# 19,000 mesos", 30, "LUK +1");
                        else if (v2 == 3) chat_message1(303, "#t1072036#", "#v4130001# #t4130001#\r\n#v4011006# 2 #t4011006#s\r\n#v4000021# 50 #t4000021#s\r\n#v4003000# 15 #t4003000#s\r\n#v4031138# 21,000 mesos", 30, "DEX +2");
                        else if (v2 == 4) chat_message1(304, "#t1072104#", "#v4130001# #t4130001#\r\n#v4021000# 3 #t4021000#s\r\n#v4021004# #t4021004#\r\n#v4000021# 30 #t4000021#s\r\n#v4000030# 15 #t4000030#s\r\n#v4003000# 20 #t4003000#s\r\n#v4031138# 20,000 mesos", 35, "LUK +2");
                        else if (v2 == 5) chat_message1(305, "#t1072105#", "#v4130001# #t4130001#\r\n#v4021003# 3 #t4021003#s\r\n#v4021004# #t4021004#\r\n#v4000021# 30 #t4000021#s\r\n#v4000030# 15 #t4000030#s\r\n#v4003000# 20 #t4003000#s\r\n#v4031138# 20,000 mesos", 35, "DEX +2");
                        else if (v2 == 6) chat_message1(306, "#t1072106#", "#v4130001# #t4130001#\r\n#v4021002# 3 #t4021002#s\r\n#v4021004# #t4021004#\r\n#v4000021# 30 #t4000021#s\r\n#v4000030# 15 #t4000030#s\r\n#v4003000# 20 #t4003000#s\r\n#v4031138# 20,000 mesos", 35, "LUK +1, DEX +1");
                        else if (v2 == 7) chat_message1(307, "#t1072108#", "#v4130001# #t4130001#\r\n#v4021003# 4 #t4021003#s\r\n#v4000030# 45 #t4000030#s\r\n#v4000095# 100 #t4000095#s\r\n#v4003000# 30 #t4003000#s\r\n#v4031138# 32,000 mesos", 40, "DEX +2, MP +5");
                        else if (v2 == 8) chat_message1(308, "#t1072109#", "#v4130001# #t4130001#\r\n#v4021006# 4 #t4021006#s\r\n#v4000030# 45 #t4000030#s\r\n#v4000096# 100 #t4000096#s\r\n#v4003000# 30 #t4003000#s\r\n#v4031138# 35,000 mesos", 40, "LUK +1, STR +1, MP +5");
                        else if (v2 == 9) chat_message1(309, "#t1072110#", "#v4130001# #t4130001#\r\n#v4021005# 4 #t4021005#s\r\n#v4000030# 45 #t4000030#s\r\n#v4000097# 100 #t4000097#s\r\n#v4003000# 30 #t4003000#s\r\n#v4031138# 35,000 mesos", 40, "LUK +1, DEX +1, MP +5");
                        else if (v2 == 10) chat_message1(310, "#t1072107#", "#v4130001# #t4130001#\r\n#v4021000# 5 #t4021000#s\r\n#v4000030# 45 #t4000030#s\r\n#v4000113# 100 #t4000113#s\r\n#v4003000# 30 #t4003000#s\r\n#v4031138# 40,000 mesos", 40, "LUK +2, MP +5");
                        else if (v2 == 11) chat_message1(311, "#t1072128#", "#v4130001# #t4130001#\r\n#v4011007# 2 #t4011007#s\r\n#v4021005# 3 #t4021005#s\r\n#v4000030# 50 #t4000030#s\r\n#v4000114# 100 #t4000114#s\r\n#v4003000# 35 #t4003000#s\r\n#v4031138# 50,000 mesos", 50, "STR +3");
                        else if (v2 == 12) chat_message1(312, "#t1072129#", "#v4130001# #t4130001#\r\n#v4011007# 2 #t4011007#s\r\n#v4021003# 3 #t4021003#s\r\n#v4000030# 50 #t4000030#s\r\n#v4000109# 100 #t4000109#s\r\n#v4003000# 35 #t4003000#s\r\n#v4031138# 50,000 mesos", 50, "DEX +3");
                        else if (v2 == 13) chat_message1(313, "#t1072130#", "#v4130001# #t4130001#\r\n#v4011007# 2 #t4011007#s\r\n#v4021000# 3 #t4021000#s\r\n#v4000030# 50 #t4000030#s\r\n#v4000115# 100 #t4000115#s\r\n#v4003000# 35 #t4003000#s\r\n#v4031138# 50,000 mesos", 50, "LUK +3");
                        else if (v2 == 14) chat_message1(314, "#t1072131#", "#v4130001# #t4130001#\r\n#v4011007# 2 #t4011007#s\r\n#v4021001# 3 #t4021001#s\r\n#v4000030# 50 #t4000030#s\r\n#v4000036# 80 #t4000036#s\r\n#v4003000# 35 #t4003000#s\r\n#v4031138# 50,000 mesos", 50, "LUK +2, DEX +1");
                    }
                    else if (v1 == 5)
                    {
                        var v2 = self.AskMenu(ask + "\r\n#L0##b #t1072003##k (Level Requirement: 30, Warrior)#l\r\n#L1##b #t1072039##k (Level Requirement: 30, Warrior)#l\r\n#L2##b #t1072040##k (Level Requirement: 30, Warrior)#l\r\n#L3##b #t1072041##k (Level Requirement: 30, Warrior)#l\r\n#L4##b #t1072002##k (Level Requirement: 35, Warrior)#l\r\n#L5##b #t1072112##k (Level Requirement: 35, Warrior)#l\r\n#L6##b #t1072113##k (Level Requirement: 35, Warrior)#l\r\n#L7##b #t1072000##k (Level Requirement: 40, Warrior)#l\r\n#L8##b #t1072126##k (Level Requirement: 40, Warrior)#l\r\n#L9##b #t1072127##k (Level Requirement: 40, Warrior)#l\r\n#L10##b #t1072132##k (Level Requirement: 50, Warrior)#l\r\n#L11##b #t1072133##k (Level Requirement: 50, Warrior)#l\r\n#L12##b #t1072134##k (Level Requirement: 50, Warrior)#l\r\n#L13##b #t1072135##k (Level Requirement: 50, Warrior)#l");
                        if (v2 == 0) chat_message2(0, "#t1072003#", "#v4021003# 4 #t4021003#s\r\n#v4011001# 2 #t4011001#s\r\n#v4000021# 45 #t4000021#s\r\n#v4003000# 15 #t4003000#s\r\n#v4031138# 18,000 mesos", 30, "STR +1");
                        else if (v2 == 1) chat_message2(1, "#t1072039#", "#v4011002# 4 #t4011002#s\r\n#v4011001# 2 #t4011001#s\r\n#v4000021# 45 #t4000021#s\r\n#v4003000# 15 #t4003000#s\r\n#v4031138# 18,000 mesos", 30, "DEX +1");
                        else if (v2 == 2) chat_message2(2, "#t1072040#", "#v4011004# 4 #t4011004#s\r\n#v4011001# 2 #t4011001#s\r\n#v4000021# 45 #t4000021#s\r\n#v4003000# 15 #t4003000#s\r\n#v4031138# 18,000 mesos", 30, "STR +1");
                        else if (v2 == 3) chat_message2(3, "#t1072041#", "#v4021000# 4 #t4021000#s\r\n#v4011001# 2 #t4011001#s\r\n#v4000021# 45 #t4000021#s\r\n#v4003000# 15 #t4003000#s\r\n#v4031138# 18,000 mesos", 30, "DEX +1");
                        else if (v2 == 4) chat_message2(4, "#t1072002#", "#v4011001# 3 #t4011001#s\r\n#v4021004# #t4021004#\r\n#v4000021# 30 #t4000021#s\r\n#v4000030# 20 #t4000030#s\r\n#v4003000# 25 #t4003000#s\r\n#v4031138# 19,800 mesos", 35, "STR +1, MP +10");
                        else if (v2 == 5) chat_message2(5, "#t1072112#", "#v4011002# 3 #t4011002#s\r\n#v4021004# #t4021004#\r\n#v4000021# 30 #t4000021#s\r\n#v4000030# 20 #t4000030#s\r\n#v4003000# 25 #t4003000#s\r\n#v4031138# 19,800 mesos", 35, "DEX +1, MP +10");
                        else if (v2 == 6) chat_message2(6, "#t1072113#", "#v4021008# 2 #t4021008#s\r\n#v4021004# #t4021004#\r\n#v4000021# 30 #t4000021#s\r\n#v4000030# 20 #t4000030#s\r\n#v4003000# 25 #t4003000#s\r\n#v4031138# 22,500 mesos", 35, "STR +1, DEX +1, MP +10");
                        else if (v2 == 7) chat_message2(7, "#t1072000#", "#v4011003# 4 #t4011003#s\r\n#v4000021# 100 #t4000021#s\r\n#v4000030# 40 #t4000030#s\r\n#v4003000# 30 #t4003000#s\r\n#v4000103# 100 #t4000103#s\r\n#v4031138# 34,200 mesos", 40, "DEX +2, MP +20");
                        else if (v2 == 8) chat_message2(8, "#t1072126#", "#v4011005# 4 #t4011005#s\r\n#v4021007# #t4021007#\r\n#v4000030# 40 #t4000030#s\r\n#v4003000# 30 #t4003000#s\r\n#v4000104# 100 #t4000104#s\r\n#v4031138# 34,200 mesos", 40, "STR +1, DEX +1, MP +20");
                        else if (v2 == 9) chat_message2(9, "#t1072127#", "#v4011002# 4 #t4011002#s\r\n#v4021007# #t4021007#\r\n#v4000030# 40 #t4000030#s\r\n#v4003000# 30 #t4003000#s\r\n#v4000105# 100 #t4000105#s\r\n#v4031138# 34,200 mesos", 40, "STR +2, MP +20");
                        else if (v2 == 10) chat_message2(10, "#t1072132#", "#v4021008# #t4021008#\r\n#v4011001# 3 #t4011001#s\r\n#v4021003# 6#t4021003#s\r\n#v4000030# 65 #t4000030#s\r\n#v4003000# 45 #t4003000#s\r\n#v4031138# 45,000 mesos", 50, "DEX +3");
                        else if (v2 == 11) chat_message2(11, "#t1072133#", "#v4021008# #t4021008#\r\n#v4011001# 3 #t4011001#s\r\n#v4011002# 6 #t4011002#s\r\n#v4000030# 65 #t4000030#s\r\n#v4003000# 45 #t4003000#s\r\n#v4031138# 45,000 mesos", 50, "DEX + 2, STR + 1");
                        else if (v2 == 12) chat_message2(12, "#t1072134#", "#v4021008# #t4021008#\r\n#v4011001# 3 #t4011001#s\r\n#v4011005# 6 #t4011005#s\r\n#v4000030# 65 #t4000030#s\r\n#v4003000# 45 #t4003000#s\r\n#v4031138# 45,000 mesos", 50, "DEX + 1, STR + 2");
                        else if (v2 == 13) chat_message2(13, "#t1072135#", "#v4021008# #t4021008#\r\n#v4011001# 3 #t4011001#s\r\n#v4011006# 6 #t4011006#s\r\n#v4000030# 65 #t4000030#s\r\n#v4003000# 45 #t4003000#s\r\n#v4031138# 45,000 mesos", 50, "STR +3");
                    }
                    else if (v1 == 6)
                    {
                        var v2 = self.AskMenu(ask + "\r\n#L0##b #t1072075##k (Level Requirement: 30, Magician)#l\r\n#L1##b #t1072076##k (Level Requirement: 30, Magician)#l\r\n#L2##b #t1072077##k (Level Requirement: 30, Magician)#l\r\n#L3##b #t1072078##k (Level Requirement: 30, Magician)#l\r\n#L4##b #t1072089##k (Level Requirement: 35, Magician)#l\r\n#L5##b #t1072090##k (Level Requirement: 35, Magician)#l\r\n#L6##b #t1072091##k (Level Requirement: 35, Magician)#l\r\n#L7##b #t1072114##k (Level Requirement: 40, Magician)#l\r\n#L8##b #t1072115##k (Level Requirement: 40, Magician)#l\r\n#L9##b #t1072116##k (Level Requirement: 40, Magician)#l\r\n#L10##b #t1072117##k (Level Requirement: 40, Magician)#l\r\n#L11##b #t1072140##k (Level Requirement: 50, Magician)#l\r\n#L12##b #t1072141##k (Level Requirement: 50, Magician)#l\r\n#L13##b #t1072142##k (Level Requirement: 50, Magician)#l\r\n#L14##b #t1072143##k (Level Requirement: 50, Magician)#l");
                        if (v2 == 0) chat_message2(100, "#t1072075#", "#v4021000# 2 #t4021000#s\r\n#v4000021# 50 #t4000021#s\r\n#v4003000# 15 #t4003000#s\r\n#v4031138# 16,200 mesos", 30, "INT +1");
                        else if (v2 == 1) chat_message2(101, "#t1072076#", "#v4021002# 2 #t4021002#s\r\n#v4000021# 50 #t4000021#s\r\n#v4003000# 15 #t4003000#s\r\n#v4031138# 16,200 mesos", 30, "INT +1");
                        else if (v2 == 2) chat_message2(102, "#t1072077#", "#v4011004# 2 #t4011004#s\r\n#v4000021# 50 #t4000021#s\r\n#v4003000# 15 #t4003000#s\r\n#v4031138# 16,200 mesos", 30, "INT +1");
                        else if (v2 == 3) chat_message2(103, "#t1072078#", "#v4021008# #t4021008#\r\n#v4000021# 50 #t4000021#s\r\n#v4003000# 15 #t4003000#s\r\n#v4031138# 16,200 mesos", 30, "INT +2");
                        else if (v2 == 4) chat_message2(104, "#t1072089#", "#v4021001# 3 #t4021001#s\r\n#v4021006# #t4021006#\r\n#v4000021# 30 #t4000021#s\r\n#v4000030# 15 #t4000030#s\r\n#v4003000# 20 #t4003000#s\r\n#v4031138# 18,000 mesos", 35, "LUK +1, MP +10");
                        else if (v2 == 5) chat_message2(105, "#t1072090#", "#v4021000# 3 #t4021000#s\r\n#v4021006# #t4021006#\r\n#v4000021# 30 #t4000021#s\r\n#v4000030# 15 #t4000030#s\r\n#v4003000# 20 #t4003000#s\r\n#v4031138# 18,000 mesos", 35, "INT +1, MP +10");
                        else if (v2 == 6) chat_message2(106, "#t1072091#", "#v4021008# 2 #t4021008#s\r\n#v4021006# #t4021006#\r\n#v4000021# 40 #t4000021#s\r\n#v4000030# 25 #t4000030#s\r\n#v4003000# 20 #t4003000#s\r\n#v4031138# 19,800 mesos", 35, "INT +1, LUK +1");
                        else if (v2 == 7) chat_message2(107, "#t1072114#", "#v4021000# 4 #t4021000#s\r\n#v4000030# 40 #t4000030#s\r\n#v4000110# 100 #t4000110#s\r\n#v4003000# 25 #t4003000#s\r\n#v4031138# 27,000 mesos", 40, "LUK +2, MP +5");
                        else if (v2 == 8) chat_message2(108, "#t1072115#", "#v4021005# 4 #t4021005#s\r\n#v4000030# 40 #t4000030#s\r\n#v4000111# 100 #t4000111#s\r\n#v4003000# 25 #t4003000#s\r\n#v4031138# 27,000 mesos", 40, "LUK +2, MP +5");
                        else if (v2 == 9) chat_message2(109, "#t1072116#", "#v4011006# 2 #t4011006#s\r\n#v4021007# #t4021007#\r\n#v4000030# 40 #t4000030#s\r\n#v4000100# 100 #t4000100#s\r\n#v4003000# 25 #t4003000#s\r\n#v4031138# 31,500 mesos", 40, "INT +2, MP +5");
                        else if (v2 == 10) chat_message2(110, "#t1072117#", "#v4021008# 2 #t4021008#s\r\n#v4021007# #t4021007#\r\n#v4000030# 40 #t4000030#s\r\n#v4000112# 100 #t4000112#s\r\n#v4003000# 30 #t4003000#s\r\n#v4031138# 36,000 mesos", 40, "INT +2, LUK +1, MP +10");
                        else if (v2 == 11) chat_message2(111, "#t1072140#", "#v4021009# #t4021009#\r\n#v4011006# 3 #t4011006#s\r\n#v4021000# 3 #t4021000#s\r\n#v4000030# 60 #t4000030#s\r\n#v4003000# 40 #t4003000#s\r\n#v4031138# 45,000 mesos", 50, "LUK +3");
                        else if (v2 == 12) chat_message2(112, "#t1072141#", "#v4021009# #t4021009#\r\n#v4011006# 3 #t4011006#s\r\n#v4021005# 3 #t4021005#s\r\n#v4000030# 60 #t4000030#s\r\n#v4003000# 40 #t4003000#s\r\n#v4031138# 45,000 mesos", 50, "INT +1, LUK +2");
                        else if (v2 == 13) chat_message2(113, "#t1072142#", "#v4021009# #t4021009#\r\n#v4011006# 3 #t4011006#s\r\n#v4021001# 3 #t4021001#s\r\n#v4000030# 60 #t4000030#s\r\n#v4003000# 40 #t4003000#s\r\n#v4031138# 45,000 mesos", 50, "INT +2, LUK +1");
                        else if (v2 == 14) chat_message2(114, "#t1072143#", "#v4021009# #t4021009#\r\n#v4011006# 3 #t4011006#s\r\n#v4021003# 3 #t4021003#s\r\n#v4000030# 60 #t4000030#s\r\n#v4003000# 40 #t4003000#s\r\n#v4031138# 45,000 mesos", 50, "INT +3");
                    }
                    else if (v1 == 7)
                    {
                        var v2 = self.AskMenu(ask + "\r\n#L0##b #t1072079##k (Level Requirement: 30, Bowman)#l\r\n#L1##b #t1072080##k (Level Requirement: 30, Bowman)#l\r\n#L2##b #t1072081##k (Level Requirement: 30, Bowman)#l\r\n#L3##b #t1072082##k (Level Requirement: 30, Bowman)#l\r\n#L4##b #t1072083##k (Level Requirement: 30, Bowman)#l\r\n#L5##b #t1072101##k (Level Requirement: 35, Bowman)#l\r\n#L6##b #t1072102##k (Level Requirement: 35, Bowman)#l\r\n#L7##b #t1072103##k (Level Requirement: 35, Bowman)#l\r\n#L8##b #t1072118##k (Level Requirement: 40, Bowman)#l\r\n#L9##b #t1072119##k (Level Requirement: 40, Bowman)#l\r\n#L10##b #t1072120##k (Level Requirement: 40, Bowman)#l\r\n#L11##b #t1072121##k (Level Requirement: 40, Bowman)#l\r\n#L12##b #t1072122##k (Level Requirement: 50, Bowman)#l\r\n#L13##b #t1072123##k (Level Requirement: 50, Bowman)#l\r\n#L14##b #t1072124##k (Level Requirement: 50, Bowman)#l\r\n#L15##b #t1072125##k (Level Requirement: 50, Bowman)#l");
                        if (v2 == 0) chat_message2(200, "#t1072079#", "#v4021000# 2 #t4021000#s\r\n#v4000021# 50 #t4000021#s\r\n#v4003000# 15 #t4003000#s\r\n#v4031138# 17,100 mesos", 30, "DEX +1");
                        else if (v2 == 1) chat_message2(201, "#t1072080#", "#v4021005# 2 #t4021005#s\r\n#v4000021# 50 #t4000021#s\r\n#v4003000# 15 #t4003000#s\r\n#v4031138# 17,100 mesos", 30, "STR +1");
                        else if (v2 == 2) chat_message2(202, "#t1072081#", "#v4021003# 2 #t4021003#s\r\n#v4000021# 50 #t4000021#s\r\n#v4003000# 15 #t4003000#s\r\n#v4031138# 17,100 mesos", 30, "DEX +1");
                        else if (v2 == 3) chat_message2(203, "#t1072082#", "#v4021004# 2 #t4021004#s\r\n#v4000021# 50 #t4000021#s\r\n#v4003000# 15 #t4003000#s\r\n#v4031138# 17,100 mesos", 30, "DEX +1");
                        else if (v2 == 4) chat_message2(204, "#t1072083#", "#v4021006# 2 #t4021006#s\r\n#v4000021# 50 #t4000021#s\r\n#v4003000# 15 #t4003000#s\r\n#v4031138# 17,100 mesos", 30, "STR +1");
                        else if (v2 == 5) chat_message2(205, "#t1072101#", "#v4021002# 3 #t4021002#s\r\n#v4021006# #t4021006#\r\n#v4000021# 30 #t4000021#s\r\n#v4000030# 15 #t4000030#s\r\n#v4003000# 20 #t4003000#s\r\n#v4031138# 18,000 mesos", 35, "STR +2");
                        else if (v2 == 6) chat_message2(206, "#t1072102#", "#v4021003# 3 #t4021003#s\r\n#v4021006# #t4021006#\r\n#v4000021# 30 #t4000021#s\r\n#v4000030# 15 #t4000030#s\r\n#v4003000# 20 #t4003000#s\r\n#v4031138# 18,000 mesos", 35, "DEX +2");
                        else if (v2 == 7) chat_message2(207, "#t1072103#", "#v4021000# 3 #t4021000#s\r\n#v4021006# #t4021006#\r\n#v4000021# 30 #t4000021#s\r\n#v4000030# 15 #t4000030#s\r\n#v4003000# 20 #t4003000#s\r\n#v4031138# 18,000 mesos", 35, "STR + 1, DEX + 1");
                        else if (v2 == 8) chat_message2(208, "#t1072118#", "#v4021000# 4 #t4021000#s\r\n#v4000030# 45 #t4000030#s\r\n#v4000106# 100 #t4000106#s\r\n#v4003000# 30 #t4003000#s\r\n#v4031138# 28,800 mesos", 40, "STR +2, MP +5");
                        else if (v2 == 9) chat_message2(209, "#t1072119#", "#v4021006# 4 #t4021006#s\r\n#v4000030# 45 #t4000030#s\r\n#v4000107# 100 #t4000107#s\r\n#v4003000# 30 #t4003000#s\r\n#v4031138# 28,800 mesos", 40, "DEX +1, STR +1, MP +5");
                        else if (v2 == 10) chat_message2(210, "#t1072120#", "#v4011003# 5 #t4011003#s\r\n#v4000030# 45 #t4000030#s\r\n#v4000108# 100 #t4000108#s\r\n#v4003000# 30 #t4003000#s\r\n#v4031138# 36,000 mesos", 40, "DEX +2, MP +5");
                        else if (v2 == 11) chat_message2(211, "#t1072121#", "#v4021002# 5 #t4021002#s\r\n#v4000030# 45 #t4000030#s\r\n#v4000099# 100 #t4000099#s\r\n#v4003000# 30 #t4003000#s\r\n#v4031138# 36,000 mesos", 40, "DEX +2, MP +5");
                        else if (v2 == 12) chat_message2(212, "#t1072122#", "#v4021008# #t4021008#\r\n#v4011001# 3 #t4011001#s\r\n#v4021006# 3 #t4021006#s\r\n#v4000030# 60 #t4000030#s\r\n#v4000033# 80 #t4000033#s\r\n#v4003000# 35 #t4003000#s\r\n#v4031138# 45,000 mesos", 50, "STR +3");
                        else if (v2 == 13) chat_message2(213, "#t1072123#", "#v4021008# #t4021008#\r\n#v4011001# 3 #t4011001#s\r\n#v4021006# 3 #t4021006#s\r\n#v4000030# 60 #t4000030#s\r\n#v4000032# 150 #t4000032#s\r\n#v4003000# 35 #t4003000#s\r\n#v4031138# 45,000 mesos", 50, "DEX + 1, STR + 2");
                        else if (v2 == 14) chat_message2(214, "#t1072124#", "#v4021008# #t4021008#\r\n#v4011001# 3 #t4011001#s\r\n#v4021006# 3 #t4021006#s\r\n#v4000030# 60 #t4000030#s\r\n#v4000041# 100 #t4000041#s\r\n#v4003000# 35 #t4003000#s\r\n#v4031138# 45,000 mesos", 50, "DEX + 2, STR + 1");
                        else if (v2 == 15) chat_message2(215, "#t1072125#", "#v4021008# #t4021008#\r\n#v4011001# 3 #t4011001#s\r\n#v4021006# 3 #t4021006#s\r\n#v4000030# 60 #t4000030#s\r\n#v4000042# 250 #t4000042#s\r\n#v4003000# 35 #t4003000#s\r\n#v4031138# 45,000 mesos", 50, "DEX +3");
                    }
                    else if (v1 == 8)
                    {
                        var v2 = self.AskMenu(ask + "\r\n#L0##b #t1072032##k (Level Requirement: 30, Thief)#l\r\n#L1##b #t1072033##k (Level Requirement: 30, Thief)#l\r\n#L2##b #t1072035##k (Level Requirement: 30, Thief)#l\r\n#L3##b #t1072036##k (Level Requirement: 30, Thief)#l\r\n#L4##b #t1072104##k (Level Requirement: 35, Thief)#l\r\n#L5##b #t1072105##k (Level Requirement: 35, Thief)#l\r\n#L6##b #t1072106##k (Level Requirement: 35, Thief)#l\r\n#L7##b #t1072108##k (Level Requirement: 40, Thief)#l\r\n#L8##b #t1072109##k (Level Requirement: 40, Thief)#l\r\n#L9##b #t1072110##k (Level Requirement: 40, Thief)#l\r\n#L10##b #t1072107##k (Level Requirement: 40, Thief)#l\r\n#L11##b #t1072128##k (Level Requirement: 50, Thief)#l\r\n#L12##b #t1072129##k (Level Requirement: 50, Thief)#l\r\n#L13##b #t1072130##k (Level Requirement: 50, Thief)#l\r\n#L14##b #t1072131##k (Level Requirement: 50, Thief)#l");
                        if (v2 == 0) chat_message2(300, "#t1072032#", "#v4011000# 3 #t4011000#s\r\n#v4000021# 50 #t4000021#s\r\n#v4003000# 15 #t4003000#s\r\n#v4031138# 17,100 mesos", 30, "DEX +1");
                        else if (v2 == 1) chat_message2(301, "#t1072033#", "#v4011001# 3 #t4011001#s\r\n#v4000021# 50 #t4000021#s\r\n#v4003000# 15 #t4003000#s\r\n#v4031138# 17,100 mesos", 30, "LUK +1");
                        else if (v2 == 2) chat_message2(302, "#t1072035#", "#v4011004# 2 #t4011004#s\r\n#v4000021# 50 #t4000021#s\r\n#v4003000# 15 #t4003000#s\r\n#v4031138# 17,100 mesos", 30, "LUK +1");
                        else if (v2 == 3) chat_message2(303, "#t1072036#", "#v4011006# 2 #t4011006#s\r\n#v4000021# 50 #t4000021#s\r\n#v4003000# 15 #t4003000#s\r\n#v4031138# 18,900 mesos", 30, "DEX +2");
                        else if (v2 == 4) chat_message2(304, "#t1072104#", "#v4021000# 3 #t4021000#s\r\n#v4021004# #t4021004#\r\n#v4000021# 30 #t4000021#s\r\n#v4000030# 15 #t4000030#s\r\n#v4003000# 20 #t4003000#s\r\n#v4031138# 18,000 mesos", 35, "LUK +2");
                        else if (v2 == 5) chat_message2(305, "#t1072105#", "#v4021003# 3 #t4021003#s\r\n#v4021004# #t4021004#\r\n#v4000021# 30 #t4000021#s\r\n#v4000030# 15 #t4000030#s\r\n#v4003000# 20 #t4003000#s\r\n#v4031138# 18,000 mesos", 35, "DEX +2");
                        else if (v2 == 6) chat_message2(306, "#t1072106#", "#v4021002# 3 #t4021002#s\r\n#v4021004# #t4021004#\r\n#v4000021# 30 #t4000021#s\r\n#v4000030# 15 #t4000030#s\r\n#v4003000# 20 #t4003000#s\r\n#v4031138# 18,000 mesos", 35, "LUK +1, DEX +1");
                        else if (v2 == 7) chat_message2(307, "#t1072108#", "#v4021003# 4 #t4021003#s\r\n#v4000030# 45 #t4000030#s\r\n#v4000095# 100 #t4000095#s\r\n#v4003000# 30 #t4003000#s\r\n#v4031138# 28,800 mesos", 40, "DEX +2, MP +5");
                        else if (v2 == 8) chat_message2(308, "#t1072109#", "#v4021006# 4 #t4021006#s\r\n#v4000030# 45 #t4000030#s\r\n#v4000096# 100 #t4000096#s\r\n#v4003000# 30 #t4003000#s\r\n#v4031138# 31,500 mesos", 40, "LUK +1, STR +1, MP +5");
                        else if (v2 == 9) chat_message2(309, "#t1072110#", "#v4021005# 4 #t4021005#s\r\n#v4000030# 45 #t4000030#s\r\n#v4000097# 100 #t4000097#s\r\n#v4003000# 30 #t4003000#s\r\n#v4031138# 31,500 mesos", 40, "LUK +1, DEX +1, MP +5");
                        else if (v2 == 10) chat_message2(310, "#t1072107#", "#v4021000# 5 #t4021000#s\r\n#v4000030# 45 #t4000030#s\r\n#v4000113# 100 #t4000113#s\r\n#v4003000# 30 #t4003000#s\r\n#v4031138# 36,000 mesos", 40, "LUK +2, MP +5");
                        else if (v2 == 11) chat_message2(311, "#t1072128#", "#v4011007# 2 #t4011007#s\r\n#v4021005# 3 #t4021005#s\r\n#v4000030# 50 #t4000030#s\r\n#v4000114# 100 #t4000114#s\r\n#v4003000# 35 #t4003000#s\r\n#v4031138# 45,000 mesos", 50, "STR +3");
                        else if (v2 == 12) chat_message2(312, "#t1072129#", "#v4011007# 2 #t4011007#s\r\n#v4021003# 3 #t4021003#s\r\n#v4000030# 50 #t4000030#s\r\n#v4000109# 100 #t4000109#s\r\n#v4003000# 35 #t4003000#s\r\n#v4031138# 45,000 mesos", 50, "DEX +3");
                        else if (v2 == 13) chat_message2(313, "#t1072130#", "#v4011007# 2 #t4011007#s\r\n#v4021000# 3 #t4021000#s\r\n#v4000030# 50 #t4000030#s\r\n#v4000115# 100 #t4000115#s\r\n#v4003000# 35 #t4003000#s\r\n#v4031138# 45,000 mesos", 50, "LUK +3");
                        else if (v2 == 14) chat_message2(314, "#t1072131#", "#v4011007# 2 #t4011007#s\r\n#v4021001# 3 #t4021001#s\r\n#v4000030# 50 #t4000030#s\r\n#v4000036# 80 #t4000036#s\r\n#v4003000# 35 #t4003000#s\r\n#v4031138# 45,000 mesos", 50, "LUK +2, DEX +1");
                    }
                }
            }
        }
    }
}
