using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.NetworkInformation;
using System.Reflection.Emit;
using WvsBeta.Common;
using WvsBeta.Common.Enums;
using WvsBeta.Game;
using WvsBeta.Game.GameObjects;
using WvsBeta.Game.Scripting;

namespace WvsBeta.Scripts.Scripts
{
    // Arwen and the glass shoes
    [Script("owen")]
    class Owen : INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            if (target.Job != 0 && target.Level > 39)
            {
                var inventory = target.Inventory;
                self.Say("Yes... I am the chief alchemist of the fairies. It's just that fairies don't come into contact with a human being for possibly a long period of time... but a strong person like you will be fine. If you get the materials, I'll make you a special item.");
                int nRet = self.AskMenu("What do you want to make? \r\n#b#L0##t4011007##l\r\n#L1##t4021009##l\r\n#L2##t4031042##l");
                string nNewItemID1 = "";
                if (nRet == 0)
                {
                    nRet = self.AskYesNo("So you want to make #t4011007#. To do this, you need one of each of these refined items: #b#t4011000#, #t4011001#, #t4011002#, #t4011003#, #t4011004#, #t4011005# and #t4011006##k. Give me 10,000 mesos and I'll make it for you.");
                    if (nRet == 0) self.Say("It's not easy to make #t4011007#. Please get the materials soon.");
                    else if (nRet == 1)
                    {
                        var ret = target.Inventory.TryExchange(-10000, 4011000, -1, 4011001, -1, 4011002, -1, 4011003, -1, 4011004, -1, 4011005, -1, 4011006, -1, 4011007, 1);
                        if (!ret) self.Say("Are you sure you have enough money? Please make sure that you have #b#t4011000#, #t4011001#, #t4011002#, #t4011003#, #t4011004#, #t4011005# and #t4011006##k refined, one of each.");
                        else nNewItemID1 = "#t4011007#";
                    }
                }
                else if (nRet == 1)
                {
                    nRet = self.AskYesNo("So you want to make #t4021009#? For that you will need each of these, refined: #b#t4021000#, #t4021001#, #t4021002#, #t4021003#, #t4021004#, #t4021005#, #t4021006#, #t4021007# and #t4021008##k. Send me 15,000 mesos and I'll make it for you.");
                    if (nRet == 0) self.Say("It's not easy to make #t4021009#. Please get the materials soon.");
                    else if (nRet == 1)
                    {
                        var ret = inventory.TryExchange(-15000, 4021000, -1, 4021001, -1, 4021002, -1, 4021003, -1, 4021004, -1, 4021005, -1, 4021006, -1, 4021007, -1, 4021008, -1, 4021009, 1);
                        if (!ret) self.Say("Are you sure you have enough money? Please make sure that you have #b#t4021000#, #t4021001#, #t4021002#, #t4021003#, #t4021004#, #t4021005#, #t4011006##k #, #t4021007# and #t4021008##k refined, one of each.");
                        else nNewItemID1 = "#t4021009#";
                    }
                }
                else if (nRet == 2)
                {
                    nRet = self.AskYesNo("So you want to make #t4031042#? To do this, you'll need #b1 #t4001006#, 1 #t4011007# and 1 #t4021008##k. Send me 30,000 mesos and I'll make it for you. Ah yes, this feather is a very special item. If you happen to drop it, it will disappear, and you can't trade it with anyone else.");

                    if (nRet == 0) self.Say("It's not easy to make #t4031042#. Please get the materials soon.");
                    else if (nRet == 1)
                    {
                        var ret = inventory.TryExchange(-30000, 4001006, -1, 4021008, -1, 4011007, -1, 4031042, 1);
                        if (!ret) self.Say("Are you sure you have enough money? Veja se você tem #b1 #t4001006#, 1 #t4011007# e 1 #t4021008##k prontos para mim.");
                        else nNewItemID1 = "#t4031042#";
                    }
                }
                if (nNewItemID1 != "") self.Say("Nice! Here, take " + nNewItemID1 + ". It's very well made, it's important to use good materials. If you ever need my help, count on me, okay?");
            }
            else self.Say("I make items that are valuable and rare, but unfortunately I can't make them for a stranger like you.");
        }
    }
    // Mike
    [Script("mike")]
    class Mike : INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            self.Say("Pass through here and you'll find Victoria Island's Central Dungeon. Be careful...");
            if (target.Job != 0 && target.Level > 44)
            {
                var qr = target.Quests;
                var val = qr.GetState(2048);
                if (val == 1)
                {
                    self.Say("Hmm... so you want to know how to get #b#t4021009##k, #b#t4003002##k, #b#t4001005##k and #b#t4001006##k? What do you plan to do with these precious materials? I've heard... since I studied a bit about the island before working as a guard...");
                    self.Say("#b#t4021009##k and #b#t4003002##k huh... I have a feeling the fairies from #m101000000# must know something about them. It's the faeries if it really is the #t4003002# that never melts we're talking about... they're probably making #t4003002# too.");
                    self.Say("#b#t4001005##k and #b#t4001006##k are problematic. The monsters will likely have it, since they've been around here for a long time... as for #t4001005#... Ah, yes! The Golem must have it, since they were created by magicians a long time ago...");
                    self.Say("#b#t4001006##k ... I've heard about it, a flame that looks like a feather... it has something to do with a dragon that breathes fire, something like that... Anyway, it's pretty strong, so it will be hard for you to get #t4001006# from it. Good luck!");
                }
            }
        }
    }
    // The statue that lets the character in
    [Script("flower_in")]
    class FlowerIn : INpcScript
    {
        int mapnum;
        int itemid;
        public void Run(INpcHost self, GameCharacter target)
        {
            var qr = target.Quests;
            var val1 = qr.GetState(2052);
            var val2 = qr.GetState(2053);
            var val3 = qr.GetState(2054);

            var inventory = target.Inventory;
            if ((val1 != 0 || val2 != 0 || val3 != 0)) // Has quest
            {
                if (val3 != 0)
                {
                    mapnum = 105040314;
                    itemid = 4031028;
                }
                else if (val2 != 0)
                {
                    mapnum = 105040312;
                    itemid = 4031026;
                }
                else if (val1 != 0)
                {
                    mapnum = 105040310;
                    itemid = 4031025;
                }

                var nFlower = inventory.GetItemAmount(itemid);
                if (nFlower == 0)
                {
                    int nRet = self.AskYesNo("As I place my hand on the statue, a strange light surrounds me, and I feel like it's sucking me in somewhere. Is it okay to be moved to such an unpredictable place?");
                    if (nRet == 0) self.Say("As I take my hand off the statue, it goes inactive, as if nothing had happened.");
                    else target.ChangeMap(mapnum);
                }
                else self.Say("I put my hand on the statue, but nothing happened. It must be because of the #t" + itemid + "# I have, because it seems to interfere with the statue's power.");
            }
            else self.Say("I put my hand on the statue, but nothing happened.");
        }
    }
    class JohnFlower
    {
        public static void Run(INpcHost self, GameCharacter target, short questID, int flowerID, int flowerAmount, params int[] rewards)
        {
            var qr = target.Quests;
            var val = qr.GetState(questID);

            var inventory = target.Inventory;

            if (val == 0) self.Say("Many flowers are blooming around here, except the one for #b#t" + flowerID + "##k.");
            else if (val == 2)
            {
                if (inventory.SlotCount(Inventory.Etc) > inventory.HoldCount(Inventory.Etc))
                {
                    var rnum = Rand32.NextBetween(0, rewards.Length-1);
                    int nNewItemID = rewards[rnum];
                    var ret = inventory.TryExchange(0, nNewItemID, 2);
                    if (!ret) self.Say("Your etc inventory is full, so you cannot hold any more items there. Please leave at least one slot empty.");
                    else target.ChangeMap(105040300);
                }
            }
            else
            {
                if (inventory.ItemCount(flowerID) == 0)
                {
                    if (inventory.SlotCount(Inventory.Etc) > inventory.HoldCount(Inventory.Etc))
                    {
                        var ret = inventory.TryExchange(0, flowerID, flowerAmount);
                        if (!ret) self.Say("Sorry, but your etc inventory is full, so you cannot hold the flowers. Leave at least one slot empty for the flowers.");
                        else target.ChangeMap(105040300);
                    }
                    else self.Say("Your etc inventory is full, so you cannot hold any more items there. Please leave at least one slot empty.");
                }
                else self.Say("Many #b#t" + flowerID + "##k are blooming, but you already have them, so you can't take more for now. You need to take the flowers to John who is located at #m104000000#.");
            }
        }
    }
    // Pink Viola
    [Script("viola_pink")]
    class ViolaPink : INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            JohnFlower.Run(self, target, 2052, 4031025, 10, 4010000, 4010001, 4010002, 4010003, 4010004, 4010005);
        }
    }

    // Blue viola
    [Script("viola_blue")]
    class ViolaBlue : INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            JohnFlower.Run(self, target, 2053, 4031026, 20, 4020000, 4020001, 4020002, 4020003, 4020004, 4020005, 4020006);
        }
    }
    // White viola
    [Script("viola_white")]
    class ViolaWhite : INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            JohnFlower.Run(self, target, 2054, 4031028, 30, 4010006, 4020007, 4020008);
        }
    }
    // Leave the flower mission map
    [Script("flower_out")]
    class FlowerOut : INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            int nRet = self.AskYesNo("When I put my hand on the statue, a strange light envelops me, and I feel like it's sucking me in somewhere. Is it ok to go back to #m105040300#?");
            if (nRet == 0) self.Say("When I take my hand off the statue, it goes inactive, as if nothing had happened.");
            else target.ChangeMap(105040300);
        }
    }
    // Pet Info
    [Script("petmaster")]
    class PetMaster : INpcScript
    {
        int v1;
        public void Run(INpcHost self, GameCharacter target)
        {
            self.Say("Hmmmm... are you by any chance educating one of my children? I have perfected a spell that uses #t5180000# to breathe life into a doll. People call it #bPet#k. If you have one, you can ask me anything you want.");
            /*
            Brown Kitty y
            Brown Puppy y
            Pink Bunny y
            Mini Kargo y
            Black Kitty y
            White Bunny y
            Husky y
            Black Pig y
            Panda y
            Dino Boy y
            Dino Girl y
            Monkey y
            Rudolph y
            */
            int v1 = self.AskMenu("What do you want to know more about? \r\n#b#L0# Tell me more about Pets. #l\r\n#L1# How to educate Pets? #l\r\n#L2# Do pets die too? \r\n#b#L3# What are the commands for the Brown and Black Kittens? \r\n#b#L4# What are the commands for Brown Puppy? \r\n#b#L5# What are the commands for the Pink and White Bunnies? \r\n#b#L6# What are the commands for Mini Kargo? \r\n#b#L7# What are the commands for Rudolph? \r\n#b#L8# What are the commands for Black Pig? \r\n#b#L9# What are the commands for Panda? \r\n#b#L10# What are the commands for Husky? \r\n#b#L11# What are the commands for Dino Boy, Dino Girl? \r\n#b#L12# What are the commands for Monkey?");
            if (v1 == 0)
            {
                self.Say("So you wish to know more about pets. A long time ago I made a doll, sprinkled #t5180000# on it and cast a charm to create a magical animal. I know it sounds unbelievable, but these are dolls that have really come to life. They understand and obey people very well.");
                self.Say("But #t5180000# only appears a little bit, right at the base of the World Tree, so I can't give it much time to live... I know, it's very sad... but even if it becomes a doll again, I can always bring it back to life. Be nice while you're with it.");
                self.Say("Ah yes! They will react when you give them special commands. You can scold them, give them affection... it all depends on how you are going to take care of them. They are afraid to be separated from their owners, so be nice to them, show them you love them. They may suddenly become sad...");
            }
            else if (v1 == 1)
            {
                self.Say("Depending on the command you give, pets can love, hate and demonstrate other types of reactions. If you give a command and the pet obeys correctly, closeness increases. By double clicking on the pet you can see closeness, level, energy, etc.");
                self.Say("Talk to the pet, pay attention to it, and your level of closeness will grow. Eventually your overall level will go up as well. If the level of closeness increases, the general level of the pet will increase soon after. As the general level increases, one day the pet can even speak a little bit, like a person. Then try to increase it for real! Of course it won't be that easy...");
                self.Say("They may just be living dolls, but they have a normal life, just like us. So they feel hungry too. The #bFullness#k shows the pet's hunger level. 100 is the maximum, and the lower it gets, it means the pet is getting hungry. After a while it won't even obey your commands and will get aggressive, so be careful with that.");
                self.Say("Ah yes! Pets cannot eat human food. My disciple #b#p1012004##k sells #b#t2120000##k at the #m100000000# market. If you need food for your pet, look in #m100000000#. It's a good idea to keep a supply of food and feed your pet before it gets too hungry.");
                self.Say("Oh... and if you don't feed the pet for an extended period of time, it will go home on its own. You can take it out of its house and feed it, but that is not good for its health. Try to feed it regularly so it doesn't get to that level, okay? I think that will do it.");
            }
            else if (v1 == 2)
            {
                self.Say("Die... well, they aren't technically ALIVE, they came to life through someone else's influence, so I don't know if death is the right term. They are dolls with my magic power, and the power of #t5180000# makes objects come alive. Of course, while alive, they are just like a normal animal...");
                self.Say("After some time... that's for sure, they stop moving. They simply revert back to dolls after the spell's effect wears off and #t5180000# wears off. But that doesn't mean it stopped forever, because if you spill #t5180000# on it, it will come back to life.");
                self.Say("Even if they move again, it's sad to see them completely stopped. Be nice to them while they're alive, okay? Feed them well, too. Isn't it nice to know that there's something alive that keeps you company and obeys only you?");
            }
            else if (v1 == 3)
            {
                self.Say("These are the commands for the #rBrown Kitty and Black Kitty#k. The level displayed next to the command shows what level the pet needs to be in order for it to respond. \r\n#b sit#k (level 1 ~ 30) \r\n#b bad, no, badgirl, badboy#k (level 1 ~ 30) \r\n#b stupid, ihateyou, dummy#k (level 1 ~ 30) \r\n#b iloveyou#k (level 1 ~ 30) \r\n#b poop#k (level 1 ~ 30) \r\n#b talk, chat, say#k (level 10 ~ 30) \r\n#b cutie#k (level 10 ~ 30) \r\n#b up, stand, rise#k (level 20 ~ 30)");
            }
            else if (v1 == 4)
            {
                self.Say("These are the commands for the #rBrown Puppy#k. The level displayed next to the command shows what level the pet needs to be in order for it to respond. \r\n#b sit#k (level 1 ~ 30) \r\n#b bad, no, badgirl, badboy#k (level 1 ~ 30) \r\n#b stupid, ihateyou, dummy#k (level 1 ~ 30) \r\n#b iloveyou#k (level 1 ~ 30) \r\n#b pee#k (level 1 ~ 30) \r\n#b talk, chat, say#k (level 10 ~ 30) \r\n#bdown#k (level 10 ~ 30) \r\n#b up, stand, rise#k (level 20 ~ 30)");
            }
            else if (v1 == 5)
            {
                self.Say("These are the commands for the #rPink Bunny and White Bunny#k. The level displayed next to the command shows what level the pet needs to be in order for it to respond. \r\n#bsit#k (level 1 ~ 30) \r\n#b bad, no, badgirl, badboy#k (level 1 ~ 30) \r\n#bup, stand, rise#k (level 1 ~ 30) \r\n#b iloveyou#k (level 1 ~ 30) \r\n#bpoop#k (level 1 ~ 30) \r\n#b talk, chat, say#k (level 10 ~ 30) \r\n#bhug#k (level 10 ~ 30) \r\n#bsleep, sleepy, gotobed#k (level 20 ~ 30)");
            }
            else if (v1 == 6)
            {
                self.Say("These are the commands for the #rMini Kargo#k. The level displayed next to the command shows what level the pet needs to be in order for it to respond. \r\n#bsit#k (level 1 ~ 30) \r\n#b bad, no, badgirl, badboy#k (level 1 ~ 30) \r\n#bup, stand, rise#k (level 1 ~ 30) \r\n#b iloveyou#k (level 1 ~ 30) \r\n#bpee#k (level 1 ~ 30) \r\n#b talk, chat, say#k (level 10 ~ 30) \r\n#bthelook, charisma#k (level 10 ~ 30) \r\n#bdown#k (level 20 ~ 30) \r\n#bgoodboy, goodgirl#k (level 20 ~ 30)");
            }
            else if (v1 == 7)
            {
                self.Say("These are the commands for #rRudolph#k. The level displayed next to the command shows what level the pet needs to be in order for it to respond. \r\n#bsit#k (level 1 ~ 30) \r\n#bbad, no, badgirl, badboy#k (level 1 ~ 30) \r\n#bup, stand#k (level 1 ~ 30) \r\n#b stupid, ihateyou, dummy#k (level 1 ~ 30) \r\n#bmerryxmas, merrychristmas#k (level 1 ~ 30) \r\n#biloveyou#k (level 1 ~ 30) \r\n#bpoop#k (level 1 ~ 30) \r\n#btalk, chat, say#k (level 11 ~ 30) \r\n#blonely, alone#k (level 11 ~ 30) \r\n#bcutie#k (level 11 ~ 30) \r\n#bmush, go#k (level 21 ~ 30)");
            }
            else if (v1 == 8)
            {
                self.Say("These are the commands for the #rBlack Pig#k. The level displayed next to the command shows what level the pet needs to be in order for it to respond. \r\n#bsit#k (level 1 ~ 30) \r\n#b bad, no, badgirl, badboy#k (level 1 ~ 30) \r\n#bpoop#k (level 1 ~ 30) \r\n#biloveyou#k (level 1 ~ 30) \r\n#bhand#k (level 1 ~ 30) \r\n#bstupid, ihateyou, dummy#k (level 1 ~ 30) \r\n#btalk, chat, say#k (level 10 ~ 30) \r\n#bsmile#k (level 10 ~ 30) \r\n#bthelook, charisma#k (level 20 ~ 30)");
            }
            else if (v1 == 9)
            {
                self.Say("These are the commands for the #rPanda#k. The level displayed next to the command shows what level the pet needs to be in order for it to respond. \r\n#bsit#k (level 1 ~ 30) \r\n#bchill, relax#k (level 1 ~ 30) \r\n#bbad, no, badgirl, badboy#k (level 1 ~ 30) \r\n#bpoop#k (level 1 ~ 30) \r\n#biloveyou#k (level 1 ~ 30) \r\n#bup, stand, rise#k (level 1 ~ 30) \r\n#btalk, chat, say#k (level 10 ~ 30) \r\n#bletsplay#k (level 10 ~ 30) \r\n#bmeh, bleh#k (level 10 ~ 30) \r\n#bsleep#k (level 20 ~ 30)");
            }
            else if (v1 == 10)
            {
                self.Say("These are the commands for the #rHusky#k. The level displayed next to the command shows what level the pet needs to be in order for it to respond. \r\n#b sit#k (level 1 ~ 30) \r\n#bbad, no, badgirl, badboy#k (level 1 ~ 30) \r\n#bstupid, ihateyou, dummy#k (level 1 ~ 30) \r\n#bhand#k (level 1 ~ 30) \r\n#bpoop#k (level 1 ~ 30) \r\n#biloveyou#k (level 1 ~ 30) \r\n#bdown#k (level 10 ~ 30) \r\n#btalk, chat, say#k (level 10 ~ 30) \r\n#bup, stand, rise#k (level 20 ~ 30)");
            }
            else if (v1 == 11)
            {
                self.Say("These are the commands for the #rDino Boy and Dino Girl#k. The level displayed next to the command shows what level the pet needs to be in order for it to respond. \r\n#bsit#k (level 1 ~ 30) \r\n#b bad, no, badgirl, badboy#k (level 1 ~ 30) \r\n#biloveyou#k (level 1 ~ 30) \r\n#bpoop#k (level 1 ~ 30) \r\n#bsmile, laugh#k (level 1 ~ 30) \r\n#bstupid, ihateyou, dummy#k (level 1 ~ 30) \r\n#btalk, chat, say#k (level 10 ~ 30) \r\n#bcutie#k (level 10 ~ 30) \r\n#bsleep, nap, sleepy#k (level 20 ~ 30)");
            }
            else if (v1 == 12)
            {
                self.Say("These are the commands for the #rMonkey#k. The level displayed next to the command shows what level the pet needs to be in order for it to respond. \r\n#bsit#k (level 1 ~ 30) \r\n#brest#k (level 1 ~ 30) \r\n#bbad, no, badgirl, badboy#k (level 1 ~ 30) \r\n#bpee#k (level 1 ~ 30) \r\n#biloveyou#k (level 1 ~ 30) \r\n#bup, stand#k (level 1 ~ 30) \r\n#btalk, chat, say#k (level 10 ~ 30) \r\n#bplay#k (level 10 ~ 30) \r\n#bmelong#k (level 10 ~ 30) \r\n#bsleep, gotobed, sleepy#k (level 20 ~ 30)");
            }
        }
    }
    // Extending pet's life
    [Script("pet_life")]
    class PetLife : INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            var qr = target.Quests;
            var val = qr.GetState(2049);

            var inventory = target.Inventory;
            int v1 = self.AskMenu("I'm Mar the Fairy. I can revive pets or transfer existing EXP to another one. \r\n#b#L0# I want the doll to become my pet again. #l\r\n#L1# I want to transfer my pet's existing EXP to a new pet.#l\r\n#k");

            if (v1 == 0) // Revive pet
            {
                string isPet = self.AskPet("");
                if (isPet != "")
                {
                    if (val == 0 || val == 2)
                    {
                        self.Say("Nice to meet you! I'm #p1032102# and I study various types of spells here at #m101000000#. I am especially fascinated by the magic of life. The mystery that has no end, the mystery known as life... I'm trying to figure out how to create life.");
                        int nRet1 = self.AskYesNo("Looks like you already found #p1012005#. #p1012005# is a person who studied the magic of life with me. I heard that he used an incomplete life spell on a doll to create a living animal... Is the doll you have the same one that #p1012005# created, called #bPet#k?");
                        if (nRet1 == 0)
                        {
                            self.Say("But it looks like the one made by #p1012005# for sure. Ah... well, never mind. I've seen #p1012005# for years and I'm pretty sure he can't succeed in life magic for dolls. Well then...");
                        }
                        else if (nRet1 == 1)
                        {
                            self.Say("I understand. The doll became a living animal... but the same item that #p1012005# used to bring the animal to life, #b#t5180000##k ran out of stock and so it returned to being a doll... obviously it's not moving, since it's a doll now... hmmm... is this thing called life really, not something you can create with magic...?");
                            self.Say("Do you want to bring back the doll as it was, with life? You want to go back to the time when your pet obeyed you, only you, and kept you company, right? Of course, it's totally possible. Since I'm the Fairy who studied the magic of life with #p1012005#... Maybe I can get it moving again...");
                            int nRet2 = self.AskYesNo("If you can get me #b#t5180000##k and a #b#t4031034##k, maybe I can bring the doll back to life. What do you think? Do you want to gather the items? Bring me the items and I'll try to make your doll come back to life...");
                            if (nRet2 == 0)
                            {
                                self.Say("Do you want to leave the doll as it is? It's a dummy and all, but... it'll be hard to erase your memory with this too. If you regret it, look for me, okay?");
                            }
                            else
                            {
                                qr.Set(2049, "");
                                self.Say("Very good. I'll say it again, what I need is #b#t5180000##k and a #b#t4031034##k. Get them and I can bring the doll back to life. The #b#t4031034##k is the hardest to get... how about looking for #b#p1012006##k from #bHenesys#k? Maybe that person can give you a tip or two...");
                            }
                        }
                    }
                    else if (val == 1)
                    {
                        inventory = target.Inventory;
                        if (inventory.ItemCount(5180000) > 0 && inventory.ItemCount(4031034) > 0)
                        {
                            int nRet3 = self.AskYesNo("You bought #b#t5180000##k and #b#t4031034##k... With them I can bring the doll back to life with my magic power. What do you think? Do you want to use the items and wake up your pet...?");
                            if (nRet3 == 0)
                            {
                                self.Say("I understand... you're not 100% ready for this, are you? You're not thinking of leaving this little animal as a doll, right? Please come back if you change your mind...");
                            }
                            else
                            {
                                string petcashid = self.AskPet("So which pet do you want to revive? Choose the pet you most want alive...");
                                var okPet = inventory.SetPetLife(petcashid, 5180000, 4031034);
                                if (!okPet) self.Say("Something is not right... are you sure you have #b#t5180000##k and #b#t4031034##k? Without these two I can't make the doll go back to being a pet.");
                                else
                                {
                                    qr.SetComplete(2049);
                                    self.Say("Your doll is now your pet again. However, my magic is not perfect, so I can't promise it eternal life... Please take good care of this pet before #t5180000# runs out. Well then... bye...");
                                }
                            }
                        }
                        else
                        {
                            self.Say("Still didn't get #b#t5180000##k and #b#t4031034##k, right? Go see #b#p1012006##k, from #m100000000#, that person should know about the scroll. Please gather these items quickly...");
                        }
                    }
                }
                else
                {
                    if (val == 2) self.Say("Hi... how is the pet with this new life? I feel very good to see you happy with your pet. Well, then... I'll have to go back to my studies...");
                    else self.Say("Hi, I'm #p1032102# and I study various types of spells here at #m101000000#. I've been studying the magic of life for hundreds of years, but it's never ending... Well, then I'll have to go back to my studies...");
                }
            }
            else if (v1 == 1) // Transfer pet exp
            {
                string isPet = self.AskPetAllExcept("", "");
                if (isPet != "" && inventory.ItemCount(4160011) > 0)
                {
                    string petcashid = self.AskPetAllExcept("So you want to transfer your pet's existing closeness? If you do this, your pet's closeness and level will be set to 0, and the new pet will have its closeness and level. Please choose the pet you want to transfer your proximity to.", "");
                    string petcashid2 = self.AskPetAllExcept("It's time to transfer the closeness of the chosen pet to the new pet. Please choose the new pet.", petcashid);
                    int nRet4 = self.AskYesNo("Now the magic will begin. Are you sure you want to make this transfer?");
                    if (nRet4 != 0)
                    {
                        var okPet = inventory.MovePetStat(petcashid, petcashid2, 4160011);
                        if (okPet == 0) self.Say("Pet closeness transferred successfully.");
                        else if (okPet == 1) self.Say("Please check that you have the required item.");
                        else if (okPet == 2) self.Say("The closeness of the new pet appears to be higher than that of the existing pet. Check again.");
                    }
                }
                else
                {
                    self.Say("I don't think you have #t4160011# or a pet for closeness to be transferred... Cloy of Henesys would definitely know about #t4160011#...");
                }
            }
        }
    }
    // Giving the item that extends the life of the pet
    [Script("pet_lifeitem")]
    class PetLifeItem : INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            var qr = target.Quests;
            var val = qr.GetState(2049);

            var isPet = self.AskPet("");

            var v = self.AskMenu("Do you have any business with me? \r\n#b#L0#Tell me about this place. #l\r\n#L1#I'm here because of the Scroll of Life. #l\r\n#L1#...#l");
            if (v == 0)
            {
                var inventory = target.Inventory;
                if (inventory.ItemCount(4031035) > 0) self.Say("Take that letter, jump over the obstacles with your pet and take the letter to my brother #p1012007#. Take the letter to him and something good will happen to your pet.");
                else
                {
                    var nRet = self.AskYesNo("This is a street where you can walk your pet. You can just walk down the street or you can train your pet to go through obstacles. If you still don't feel very close to your pet, it could be a problem, as he won't obey you very much... so, what do you think? Do you want to train your pet?");
                    if (nRet == 0) self.Say("Hmm... too busy to do this now? Anyway, if you want, come back and find me.");
                    else
                    {
                        var ret = inventory.TryExchange(0, 4031035, 1);
                        if (!ret) self.Say("Your etc inventory is full! I cannot give you the letter unless there is space in your inventory. Leave a slot free and then talk to me.");
                        else self.Say("Okay, here's the letter. He wouldn't know I sent you if you went there straight away. Go through the obstacles with your pet, go all the way to the top and then talk to #p1012007# and give him the letter. It won't be difficult if you pay attention to your pet while going through the obstacles. Good luck!");
                    }
                }
            }
            else if (v == 1)
            {
                if (isPet != "" && val == 1)
                {
                    var inventory = target.Inventory;
                    if (inventory.ItemCount(4031034) > 0) self.Say("Hmm... you already have #b#t4031034##k. Take this scroll to #b#p1032102##k of #m101000000#.");
                    else
                    {
                        self.Say("Are you here with a #bpet#b that doesn't move#k? Sad to see... Huh? Did you come here via #b#p1032102##k? I see... #b#t4031034##k, uh... hey hey~ like I really have with me... what, what is this in my pocket?");
                        self.Say("Wow!!! Is this... is this #b#t4031034##k? Oh right... #p1012005# probably appropriated my clothes and took off or something... damn it! I told him not to just go grab other people's clothes and put them on... Well, it's not mine anyway... do you need this? Hmm...");
                        var nRet1 = self.AskYesNo("I don't think I can just give it to you! I need to test your knowledge about pets in general. Horrible for a pet if its owner doesn't even know how to take care of it. You need to get everything right or you won't get the scroll. What do you think? Want to take the test?");
                        if (nRet1 == 0) self.Say("What?! Are you already giving up? If you had raised your pet well, this would be a piece of cake! Look for me when you change your mind.");
                        else
                        {
                            self.Say("Right! 5 questions and you need to answer them all correctly! You ready? Let's go!");
                            var v1 = self.AskMenu("Question 1) What town is #p1012004#, the person selling #t2120000#, from?\r\n#b#L0# #m104000000##l\r\n#L1# #m100000000##l\r\n#L2# #m102000000##l\r\n#L3# #m101000000##l\r\n#L4# #m103000000##l\r\n#L5# #m105040300##l");
                            if (v1 == 0 || v1 == 2 || v1 == 3 || v1 == 4 || v1 == 5 ) self.Say("Wrong! You really don't know much... did you really raise a pet? It's horrible!");
                            else if (v1 == 1)
                            {
                                var v2 = self.AskMenu("Question 2) Haha... that was just a warmup! Okay, so... out of these people, choose someone who has nothing to do with pets. \r\n#b#L0# #p1032102##l\r\n#L1# #p1012005##l\r\n#L2# #p1012101##l");
                                if (v2 == 0 || v2 == 1 ) self.Say("Wrong! You really don't know much... did you really raise a pet? It's horrible!");
                                else if (v2 == 2)
                                {
                                    var v3 = self.AskMenu("Question 3) Very easy, right? Okay, out of these descriptions of pets, choose the one that doesn't make sense. \r\n#b#L0#To name a pet, you need the Pet Tag item. #l\r\n#L1#When you give a pet a command and it obeys, sometimes the level of closeness increases. #l\r\n#L2#If you don't feed your pet well, the level of closeness can decrease. #l\r\n#L3#Pets can attack monsters with their owners.#k#l");
                                    if (v3 == 0 || v3 == 1 || v3 == 2 ) self.Say("Wrong! You really don't know much... did you really raise a pet? It's horrible!");
                                    else if (v3 == 3)
                                    {
                                        var v4 = self.AskMenu("Question 4) Two to go! Well, at what level do pets start using human phrases? \r\n#L0##e1. #n#bLevel 5 #k#l\r\n#L1##e2. #n#bLevel 10 #k#l\r\n#L2##e3. #n#bLevel 15 #k#l\r\n#L3##e4. #n#bLevel 20#k#l");
                                        if (v4 == 0 || v4 == 2 || v4 == 3 ) self.Say("Wrong! You really don't know much... did you really raise a pet? It's horrible!");
                                        else if (v4 == 1)
                                        {
                                            var v5 = self.AskMenu("Question 5) Last question! #p1012004# of #m100000000# sells #t2120000#. How much fullness does it increase?\r\n#b#L0# 10#l\r\n#L1# 20#l\r\n#L2# 30#l\r\n#L3# 40#l");
                                            if (v5 == 0 || v5 == 1 || v5 == 3 ) self.Say("Oh no!!! What a waste! It's the last question! Don't give up!");
                                            else
                                            {
                                                self.Say("Right! Hmmm... you know a lot about pets. Cool, since you know a lot, I'll gladly give you the parchment. I know it's not mine and all, but... who would wear someone else's clothes and leave something so important in them? There you go!");
                                                inventory = target.Inventory;
                                                var ret = inventory.TryExchange(0, 4031034, 1);
                                                if (!ret) self.Say("Ouch... do you have a slot available in your etc. inventory? I can't give it to you if it's full.");
                                                else self.Say("Ok... So, all you need to do now is take it and go talk to #p1032102# and also get a #b#t5180000##k... Hahaha good luck to you!");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else self.Say("Hey, are you sure you found #b#p1032102##k? Don't lie to me if you've never met him, because that's obvious. It wasn't even a good lie!");
            }
        }
    }
    // Raising Pet's Closeness
    [Script("pet_letter")]
    class PetLetter : INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            var inventory = target.Inventory;
            if (inventory.ItemCount(4031035) > 0)
            {
                self.Say("Yay, it's my brother's letter! Probably scolding me for thinking I'm not working... hmm? Ahhh... you followed my brother's advice, trained your pet and came up here, huh? Cool! Since you've worked hard to get here, I'm going to increase your level of intimacy with your pet.");
                var pet = target.GetSpawnedPet();
                if (pet == null) self.Say("Hmmm... did you really come here with your pet? These obstacles are for animals. What are you doing here without him? Are you kidding me!!!");
                else
                {
                    var ret = inventory.TryExchange(0, 4031035, -1);
                    if (!ret) self.Say("Hey, do you really have my brother's letter? Go to your etc inventory and see if the letter is really there or not!");
                    else
                    {
                        Pet.IncreaseCloseness(target, pet, 2);
                        self.Say("What do you think? Don't you think you got closer to your pet? If you have time, train your pet again on this obstacle course... of course, with my brother's permission.");
                    }
                }
            }
            else self.Say("My brother told me to be careful with the obstacle course, but... since I'm so far from it, I can't resist, I want to make a mess... hehe. I haven't seen him in person anyway, I think I can just relax for a few minutes.");
        }
    }
}
