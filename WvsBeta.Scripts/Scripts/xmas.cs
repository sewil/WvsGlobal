using System;
using System.Diagnostics.Eventing.Reader;
using WvsBeta.Common;
using WvsBeta.Common.Extensions;
using WvsBeta.Game;
using WvsBeta.Game.Events;
using WvsBeta.Game.Scripting;

namespace WvsBeta.Scripts.Scripts
{
    public static class xmas
    {
        static INpcHost self;
        static GameCharacter target;

        static int[] Maps = new int[]
        {
            250000000,
            101000000,
            200000000,
            211000000,
            220000300,
            600000000,
            100000000,
        };

        public static bool IsEventRunning()
        {
            var cDate = MasterThread.CurrentDate;
            var cYear = cDate.Year;
            var esTime = cDate.CompareTo(new DateTime(cYear, 11, 20));
            var eeTime = cDate.CompareTo(new DateTime(cYear, 1, 16));
            bool eRunning = esTime >= 0 || eeTime < 0;
            return eRunning;
        }

        public static void go_tree(int npcID, int map)
        {
            var options = new string[]
            {
                "The room with the first tree",
                "The room with the second tree",
                "The room with the third tree",
                "The room with the fourth tree",
                "The room with the fifth tree"
            };
            int optionIdx = self.AskMenu($"Hi~ I'm #p{npcID}#. I can take you to the room where the giant Christmas tree is! For more information, speak to #b#p2001000##k. Which room do you want to enter?", options);

            var field = GameDataProvider.Maps[map + optionIdx];
            if (field.Characters.Count < 6) target.ChangeMap(map + optionIdx, "");
            else self.Say($"You wish to enter #b{options[optionIdx].ToLower()}#k? Hmm... I'm sorry, but it's a bit crowded at the moment. Please enter another room~");
        }
        // Send the character to Christmas Town
        [Script("go_xmas")]
        class go_xmas : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                xmas.self = self;
                xmas.target = target;
                var field = self.Field;

                bool eRunning = IsEventRunning();
                bool bypass = MasterThread.IsDebug || target.IsGM;
                if (!eRunning)
                {
                    if (bypass) self.Say("The event is over... huh? Hey, GM! Happyville is currently under construction but... for you I'll make an exception!");
                    else
                    {
                        self.Say("The event is over. See you next year~");
                        return;
                    }
                }

                var qr = target.QuestRecord;

                int mapIdx = Math.Max(0, Array.FindIndex(Maps, i => i == field.ID));

                int nRet = self.AskYesNo("Are you curious about the adventures that Happyville has in store for you? Let's go!");
                if (nRet == 0) self.Say("I suppose you are very busy right now. If you find some free time during your journey, please come and see me! You will discover a Christmas town like no other~");
                else
                {
                    qr.Set(9002, mapIdx.ToString());
                    target.ChangeMap(209000000, "st00");
                }
            }
        }
        // Leaving the christmas town : beginners go for free; others require a christmas ornament
        [Script("go_victoria")]
        class go_victoria : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                xmas.self = self;
                xmas.target = target;
                var qr = target.QuestRecord;
                int.TryParse(qr.Get(9002), out int mapIdx);
                Maps.TryGetValue(mapIdx, out int mapId);

                int nRet = self.AskYesNo($"You've got nothing else to do here, huh? Do you want to return to #m{mapId}#? If you want to go back, I can send you back to #m{mapId}# right away. What do you think? Do you want to go back?");

                if (nRet == 0) self.Say("You must have some business to take care of here, right? It's not a bad idea to rest a bit in this area and regain all your strength before heading back there again.");
                else
                {
                    target.ChangeMap(mapId, "h003");
                }
            }
        }
        // decorating tree NPC
        [Script("desc_tree")]
        class desc_tree : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                xmas.self = self;
                xmas.target = target;
                self.Say("Do you see the group of snowmen standing over there? Talk to one of them, and they will take you to the famous Christmas tree. It's gigantic! While you're there, you can redecorate the tree using all sorts of decorations. What do you think? Pretty cool, right?");
                self.Say("Only 6 at a time may be present in each map, and you will be unable to #btrade or open a shop#k while in there. Also, only you can pick up the decorations you dropped along the way, so don't worry about losing them here.");
                self.Say("Of course, the items you drop in the map will never disappear. When you're ready, you can exit through the snowman inside. When you leave, you will no longer be able to recover the dropped items within the map, so don't worry about picking up those items. Isn't it pleasant?");
                self.Say("Well, go to #p2002001#, buy more Christmas ornaments there, and then decorate the tree with them~ Awesome! The biggest and prettiest ornaments are not sold by him, however. They've probably been... taken by some monster! Dum dum dum~");
            }
        }
        // The NPC1 that leads the users to the map that decorates the tree
        [Script("go_tree1")]
        class go_tree1 : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                xmas.self = self;
                xmas.target = target;
                go_tree(2001001, 209000001);
            }
        }
        // The NPC2 that leads the users to the map that decorates the tree
        [Script("go_tree2")]
        class go_tree2 : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                xmas.self = self;
                xmas.target = target;
                go_tree(2001002, 209000006);
            }
        }
        // The NPC3 that leads the users to the map that decorates the tree
        [Script("go_tree3")]
        class go_tree3 : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                xmas.self = self;
                xmas.target = target;
                go_tree(2001003, 209000011);
            }
        }
        // The NPC that lets you leave the map of decorating trees
        [Script("out_tree")]
        class out_tree : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                xmas.self = self;
                xmas.target = target;
                var nRet = self.AskYesNo("Have you ever decorated a tree and made it look pretty? Decorating a Christmas tree with other people is an interesting experience. Don't you think? Oh yeah... are you sure you want to leave this place?");
                if (nRet == 0) self.Say("You need more time to decorate the tree, huh? If you want to leave this place, you can come talk to me~");
                else target.ChangeMap(209000000, "");
            }
        }
        // Torre Event
        [Script("thor")]
        class thor : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                xmas.self = self;
                xmas.target = target;
                if (target.Level < 10)
                {
                    self.Say("Oh no... I lost my precious horn!!!! Ah, it doesn't look like you're strong enough to help me. Please train some more and then come see me again, okay?");
                    return;
                }

                bool eRunning = IsEventRunning();

                if (!eRunning)
                {
                    self.Say("The event is over. See you next year~");
                    return;
                }

                var cTime = MasterThread.CurrentTimeStr;
                var qr = target.QuestRecord;
                var val1 = qr.Get(8845);
                var val2 = qr.Get(8846);
                var aTime = MasterThread.CompareTime(cTime, val2);
                if (aTime > 1440)
                {
                    self.Say("Hey, it's you! You found the horn for me, didn't you? You can only participate in the event once a day~ Please come back another day.");
                    return;
                }

                if (val1 == "s")
                {
                    var inven = target.Inventory;
                    if (inven.ItemCount(4031063) >= 1)
                    {
                        if (inven.SlotCount(1) > inven.HoldCount(1) && inven.SlotCount(3) > inven.HoldCount(3))
                        {
                            self.Say("Ahh cool~ This is my horn! Thank you very much! I'll give you this in return. I'm sure it will prove very useful to you. Bye~");
                            int ret = 0;
                            var rnum = Rand32.NextBetween(0, 14);
                            if (rnum == 0) ret = inven.ExchangeEx(0, "4031063", -1, "1012011,Period:21600", 1);
                            else if (rnum == 1) ret = inven.ExchangeEx(0, "4031063", -1, "1012012,Period:21600", 1);
                            else if (rnum == 2) ret = inven.ExchangeEx(0, "4031063", -1, "1012013,Period:21600", 1);
                            else if (rnum == 3) ret = inven.ExchangeEx(0, "4031063", -1, "1012014,Period:21600", 1);
                            else if (rnum == 4) ret = inven.ExchangeEx(0, "4031063", -1, "1012015,Period:21600", 1);
                            else if (rnum == 5) ret = inven.ExchangeEx(0, "4031063", -1, "1012016,Period:21600", 1);
                            else if (rnum == 6) ret = inven.ExchangeEx(0, "4031063", -1, "1012017,Period:21600", 1);
                            else if (rnum == 7) ret = inven.ExchangeEx(0, "4031063", -1, "1012018,Period:21600", 1);
                            else if (rnum == 8) ret = inven.ExchangeEx(0, "4031063", -1, "1012019,Period:21600", 1);
                            else if (rnum == 9) ret = inven.ExchangeEx(0, "4031063", -1, "1012020,Period:21600", 1);
                            else if (rnum == 10) ret = inven.Exchange(0, 4031063, -1, 3992024, 1);
                            else if (rnum == 11) ret = inven.Exchange(0, 4031063, -1, 3992025, 1);
                            else if (rnum == 12) ret = inven.Exchange(0, 4031063, -1, 3992026, 1);
                            else if (rnum == 13) ret = inven.Exchange(0, 4031063, -1, 2020012, 1);
                            else if (rnum == 14) ret = inven.Exchange(0, 4031063, -1, 2020013, 1);

                            if (ret == 0) self.Say("Do you have enough empty slots in your inventory? Have you got my horn yet? Please check again!");
                            else
                            {
                                qr.Set(8845, "end");
                                qr.Set(8846, cTime);
                            }
                        }
                        else self.Say("You need to have at least one empty slot in your equip and setup inventory. Please come back later~");
                    }
                    else self.Say("Have you gotten my horn yet? Oh... I found out which store it was. It was an item from Cash Shop. They put my horn somewhere in the shop. Have you ever been there? Hmmm... I don't know where it is... Can you get it for me?");
                }
                else
                {
                    var v1 = self.AskMenu("Torr looks at you. What would you like to say?",
                        "Hello, do you know how to get Christmas ornaments?",
                        "Why are you crying?"
                    );
                    if (v1 == 0) self.Say("Ahh... Ask Rudi, he knows all these things!!! I should be looking for my horn!!!...");
                    else
                    {
                        var v2 = self.AskMenu("Grandpa Cliff gave me another scolding... AGAIN? AGAIN? I lost my horn AGAIN!",
                            "You look great without it",
                            "Then.... You should be more careful",
                            "How did you lose it?"
                        );
                        if (v2 == 0) self.Say("... Really? Thanks, but... To help Grandpa Cliff, I need my horn...");
                        else if (v2 == 1) self.Say("I know, I know. But when I start doing something, I completely forget about things...");
                        else
                        {
                            var v3 = self.AskMenu("I lost my horn when I was practicing pushing the sled... People think I'm very clumsy... It turns out that I get really nervous when I fly through the skies... That's why I lost my horn during training!",
                                "I will help you get your horn back.",
                                "What would my reward be if I helped you find your horn?"
                            );
                            if (v3 == 1) self.Say("... Don't expect any rewards when you're helping others!! Someone COOL will show up, willing to help me.");
                            else
                            {
                                qr.Set(8845, "s");
                                self.Say("Really? Did you look in the shop? Someone told me... Hmm... Ahh, I forgot. I should be looking for my horn. If you help me find my horn, I will reward you with something special.");
                            }
                        }
                    }
                }
            }
        }
    }
}
