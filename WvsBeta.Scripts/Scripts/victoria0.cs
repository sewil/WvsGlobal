using System.ComponentModel;
using WvsBeta.Common;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Extensions;
using WvsBeta.Common.Objects;
using WvsBeta.Game;
using WvsBeta.Game.Packets;
using WvsBeta.Game.Scripting;

namespace WvsBeta.Scripts.Scripts
{
    // 1. The NPC at Lith harbor that warps the characters to other towns with a service fee.
    [Script("rithTeleport")]
    public class RithTeleport : INpcScript
    {
        INpcHost self;
        GameCharacter target;
        public void GoTel(int mapNum, int fee)
        {
            int nRet = self.AskYesNo("I don't think you need to be here. Do you really want to go to #b#m" + mapNum + "##k? Well, it will cost #b" + fee.ToFormattedString() + " mesos#k. What do you think?");
            if (nRet == 1)
            {
                if (!target.Inventory.TryExchange(-fee)) self.Say("You do not have enough money. With your skills, you should have more than that!");
                else target.ChangeMap(mapNum);
            }
            self.Say("There's a lot to see in this town too. Let me know if you want to go somewhere else.");
        }
        public void Run(INpcHost self, GameCharacter target)
        {
            this.self = self;
            this.target = target;

            var cJob = target.Job;
            self.Say("Do you want to go to some other city? I can take you with little money on the move. It's a little pricey, but I give a special 90% discount to apprentices.");
            int nRet = self.AskMenu("It's understandable that you might be confused about this place if this is your first time here. If you have questions about this place, send them out. \r\n#b#L0# What types of cities are there here on Victoria Island?#l\r\n#L1#Take me somewhere else.#l");
            int nRet2 = 0;
            if (nRet == 0)
            {
                nRet2 = self.AskMenu("There are 6 major cities here on Victoria Island. Which one do you want to know better? \r\n#b#L0##m104000000##l\r\n#L1##m102000000##l\r\n#L2##m101000000##l\r\n#L3##m100000000##l\r\n#L4##m103000000##l\r\n#L5##m105040300##l");
            }
            else if (nRet == 1)
            {
                if (cJob == 0)
                {
                    nRet2 = self.AskMenu("We have a special 90% discount for beginners. Okay, where would you like to go? \r\n#b#L0##m102000000# (120 mesos) \r\n#b#L1##m101000000# (120 mesos) \r\n#b#L2##m100000000# (80 mesos) \r\n#b#L3##m103000000# (100 mesos)#l");
                }
                else
                {
                    nRet2 = self.AskMenu("Oh, you're not a beginner, huh? Then I'm sorry, but I must charge you the regular price. Where would you like to go? \r\n#b#L0##m102000000# (1,200 mesos) \r\n#b#L1##m101000000# (1,200 mesos) \r\n#b#L2##m100000000# (800 mesos) \r\n#b#L3##m103000000# (1,000 mesos)#l");
                }
            }
            if (nRet == 0)
            {
                if (nRet2 == 0)
                {
                    self.Say("The city where you are is Lith Harbor! Alright, I'll tell you more about #b#m104000000##k. It is the place where you landed on Victoria Island traveling on the ship. It's #m104000000#. Many apprentices who have just arrived from Maple Island begin their journey here.");
                    self.Say("It's a quiet town, with a long strip of water at the back, thanks to the fact that the port is located at the west end of the island. Many of the people here are or were fishermen. They may seem threatening, but if you strike up a conversation, they'll be friendly with you.");
                    self.Say("Around the city there is a beautiful prairie. For the most part, the monsters here are small and gentle, perfect for beginners. If you haven't chosen your job yet, this is a good place to give your level a boost.");
                }
                else if (nRet2 == 1)
                {
                    self.Say("Alright, I'll tell you more about #b#m102000000##k. It is a town of warriors located in the northernmost part of Victoria Island, surrounded by rocky mountains. With an unfriendly environment, only the strong survive there.");
                    self.Say("Close to the hill country, you will find very thin trees, roaming wild boars and monkeys that live all over the island. There is also a deep valley, and if you go into it, you will find huge dragons with so much power that it matches their size. Better be very careful if you choose to go there, or not go at all.");
                    self.Say("If you want to be a #bWarrior#k, find #r#p1022000##k, the chief of #m102000000#. If you are level 10 or higher and also have a good STR level, he can make you a warrior. If not, better keep training until you reach that level.");
                }
                else if (nRet2 == 2)
                {
                    self.Say("Alright, I'll tell you more about #b#m101000000##k. It is a town of magicians, located in the remote eastern part of Victoria Island. It is covered with tall and mystical trees. There you will find some fairies. They're not too fond of humans in general, so it's best to stay away from them and stay quiet.");
                    self.Say("Near the forest you will find green slimes, green mushrooms, monkeys and zombie monkeys, they all live there. Go deeper into the forest and you will find witches with broomsticks flying through the sky. A heads up for you: unless you're really strong, I recommend you don't go near them.");
                    self.Say("If you want to be a #bMagician#k, look for #r#p1032001##k, the master wizard of #m101000000#. He can make you a magician if you are level 8 or above, and have a decent amount of INT. If not, you need to work harder and train to get there.");
                }
                else if (nRet2 == 3)
                {
                    self.Say("Alright, I'll tell you more about #b#m100000000##k. It is a town of bowmen, located in the very south part of the island, in a flat region, in the middle of dense forests and grasslands. The climate is perfect and everything is abundant around that town. A great place to live. Go check it out!");
                    self.Say("Across the prairie you'll find weak monsters like snails, mushrooms and pigs. Although I've heard that in the deepest parts of Pig Park, which can be found somewhere in town, you'll occasionally find a huge, powerful mushroom called Mushmom.");
                    self.Say("If you want to be a #bBowman#k, you need to go see #r#p1012100##k in #m100000000#. At level 10 or higher and a decent amount of DEX, she should make you a bowman. If not, go train, get stronger, then try again.");
                }
                else if (nRet2 == 4)
                {
                    self.Say("Alright, I'll tell you more about #b#m103000000##k. It is a town of thieves, located in the northwest part of Victoria Island. There are buildings there that even have a kind of strange feeling around them. Most of them are covered with black clouds, but if you can climb to a very high place, you can see a beautiful sunset there.");
                    self.Say("From #m103000000# you can go to various dungeons. You can go to a swamp full of alligators and neckis, or reach the subway, full of wraiths and stirges.");
                    self.Say("If you want to be a #bThief#k, look for #r#p1052001##k, the source of the darkness in #m103000000#. He should make you a good thief if you're level 10 or above, with a good amount of DEX. If not, go hunting and train to get there.");
                }
                else if (nRet2 == 5)
                {
                    self.Say("Alright, I'll tell you more about #b#m105040300##k. It is a town of forests, located on the southeast side of Victoria Island. It's almost between #m100000000# and the Ant Tunnel dungeon. There's a hotel there, so you can rest after a tiring day in the dungeon... in general it's a calm town.");
                    self.Say("In front of the hotel there is an old Buddhist monk called #r#p1061000##k. Nobody knows anything about that monk. Apparently he collects materials from travelers and creates something, but I don't know the details very well. If you have business in that area, please look into it for me.");
                    self.Say("From #m105040300#, heading east, you will find the ant tunnel connected to the deepest part of Victoria Island. There are a lot of powerful and nasty monsters there. If you go there thinking it's a walk in the park, you'll end up dead. Before going, you have to prepare well for a challenging journey.");
                    self.Say("I heard that... apparently, at #m105040300# there is a secret entrance that leads to an unknown place. Looks like if you go deep in there, you'll find a pile of black rocks that actually move around on their own. I want to see this with my own eyes sometime...");
                }
            }
            if (nRet == 1)
            {
                if (nRet2 == 0)
                {
                    if (cJob == 0) GoTel(102000000, 120);
                    else GoTel(102000000, 1200);
                }
                else if (nRet2 == 1)
                {
                    if (cJob == 0) GoTel(101000000, 120);
                    else GoTel(101000000, 1200);
                }
                else if (nRet2 == 2)
                {
                    if (cJob == 0) GoTel(100000000, 80);
                    else GoTel(100000000, 800);
                }
                else if (nRet2 == 3)
                {
                    if (cJob == 0) GoTel(103000000, 100);
                    else GoTel(103000000, 1000);
                }
            }
        }
    }
    // 10. Jane and the wild boar
    [Script("jane")]
    class Jane : INpcScript
    {
        bool questComplete = false;
        int itemId;
        int itemPrice;
        int totalPrice;
        public void Run(INpcHost self, GameCharacter target)
        {
            var qr = target.Quests;
            var val = qr.GetState(2013);
            if (val == QuestState.Completed)
            {
                questComplete = true;
                self.Say("It's you... thanks to you I managed to do a lot. I'm currently making an item pack. If you need anything, just let me know.");
                int nRet = self.AskMenu("Which item would you like to buy? \r\n#b#L0##t2000002# (cost: 310 mesos) #l\r\n#L1##t2022003# (cost: 1,060 mesos) #l\r\n#L2##t2022000# (cost: 1,600 mesos) #l\r\n#L3##t2001000# (cost: 3,120 mesos)#l");
                int nRetNum = 0;
                if (nRet == 0)
                {
                    nRetNum = self.AskNumber("Do you want #b#t2000002##k? #t2000002# allows you to restore 300 HP. How many do you want to buy?", 0, 0, 100);
                    itemId = 200002;
                    itemPrice = 310;
                }
                else if (nRet == 1)
                {
                    nRetNum = self.AskNumber("Do you want #b#t2022003##k? #t2022003# allows you to restore 1,000 HP. How many do you want to buy?", 0, 0, 100);
                    itemId = 2022003;
                    itemPrice = 1060;
                }
                else if (nRet == 2)
                {
                    nRetNum = self.AskNumber("Do you want #b#t2022000##k? #t2022000# allows you to restore 800 HP. How many do you want to buy?", 0, 0, 100);
                    itemId = 2022000;
                    itemPrice = 1600;
                }
                else if (nRet == 3)
                {
                    nRetNum = self.AskNumber("Do you want #b#t2001000##k? #t2001000# allows you to restore 1,000 HP and MP. How many do you want to buy?", 0, 0, 100);
                    itemId = 2001000;
                    itemPrice = 3120;
                }

                totalPrice = itemPrice * nRetNum;
                self.AskYesNo("Will you buy #r" + nRetNum + "#k #b#t" + itemId + "#(s) #k? #b#t" + itemId + "# costs " + itemPrice.ToFormattedString() + " a piece, so the total cost will be #k" + totalPrice.ToFormattedString() + "#k mesos.");
                if (nRet == 0)
                {
                    self.Say("I still have some of the materials you already bought from me. It's all there, you can choose as you please.");
                }
                else
                {
                    if (!target.Inventory.TryExchange(-totalPrice, itemId, (short)nRetNum))
                    {
                        self.Say("Are you running out of money? Please check if you have an empty slot in your inventory and that you have at least #r" + totalPrice.ToFormattedString() + "#k mesos.");
                    }
                    else
                    {
                        self.Say("Thank you for coming! We can always do business here, so if you need anything, come back, okay?");
                    }
                }
            }
            else if (target.Level >= 40 && target.Job != 0) self.Say("You will only be able to buy my potions after taking care of my orders.");
            else if (target.Level >= 25 && target.Job != 0) self.Say("You don't look strong enough to buy my potions...");
            else self.Say("My dream is to travel everywhere, just like you. But my dad won't let me go because he thinks it's too dangerous. He should let me, although if I somehow show him that I'm not the weak girl he thinks I am...");
        }
    }
    // Camila's Glass Marble Script Portal
    [Script("q2073")]
    class Camila : IPortalScript
    {
        public void Run(IPortalHost host, GameCharacter target)
        {
            var qr = target.Quests;
            var val = qr.GetState(2073);
            if (val == QuestState.Started)
            {
                var quest = FieldSet.Instances["Utah"];
                if (!quest.Enter(target, 0))
                {
                    target.SendPacket(MessagePacket.RedText("Looks like someone is already visiting Utah's Farm."));
                }
            }
            else target.SendPacket(MessagePacket.RedText("There's a door that will take me somewhere, but I can't get in there."));
        }
    }
}
