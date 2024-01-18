using System.Linq;
using WvsBeta.Common;
using WvsBeta.Common.Extensions;
using WvsBeta.Game;
using WvsBeta.Game.Handlers.GuildQuest;
using WvsBeta.Game.Scripting;
using static WvsBeta.Game.GameCharacter;

namespace WvsBeta.Scripts.Scripts
{
    public static class guildquest1
    {
        static INpcHost self;
        static GameCharacter target;
        public static void mobSummon()
        {
            var field = target.Field;
            field.SummonMob(-184, 140, 2100026);
            field.SummonMob(-116, 140, 2100027);
            field.SummonMob(-48, 140, 2100026);
            field.SummonMob(36, 140, 2100027);
            field.SummonMob(142, 140, 2100026);
            field.SummonMob(205, 140, 2100027);
            field.SummonMob(270, 140, 2100026);
            return;
        }
        public static string changebaseballstring(string str, int index)
        {
            string left, right;
            if (index == 0) left = "";
            else left = str.Substring(0, index);

            if (index == 3) right = "";
            else right = str.Substring(index + 1, 3 - index);

            return left + "0" + right;
        }
        public static int getitemid(int index)
        {
            if (index == 1) return 4001027;            // drop items at the spot
            else if (index == 2) return 4001028;
            else if (index == 3) return 4001029;
            return 4001030;
        }
        public static string setstatuequestion(int num)
        {
            string question = null;
            if (num == 4) question = Tools.Shuffle(1, "12340000000000000000");
            else if (num == 5) question = Tools.Shuffle(1, "12345000000000000000");
            else if (num == 6) question = Tools.Shuffle(1, "12345600000000000000");
            return question;
        }
        public static void takeawayitem()
        {
            //delte quest items
            var inven = target.Inventory;

            var nItem = inven.ItemCount(4001024); //루 비안
            int ret;
            if (nItem > 0)
            {
                ret = inven.Exchange(0, 4001024, -nItem);
            }

            nItem = inven.ItemCount(4001025); //롱기누스의 창
            if (nItem > 0)
            {
                ret = inven.Exchange(0, 4001025, -nItem);
            }

            nItem = inven.ItemCount(4001026); //열쇠
            if (nItem > 0)
            {
                ret = inven.Exchange(0, 4001026, -nItem);
            }

            nItem = inven.ItemCount(4001027); //용기의 훈장
            if (nItem > 0)
            {
                ret = inven.Exchange(0, 4001027, -nItem);
            }

            nItem = inven.ItemCount(4001028); //지혜의 두루마리
            if (nItem > 0)
            {
                ret = inven.Exchange(0, 4001028, -nItem);
            }

            nItem = inven.ItemCount(4001029); //오래된 음식
            if (nItem > 0)
            {
                ret = inven.Exchange(0, 4001029, -nItem);
            }

            nItem = inven.ItemCount(4001030); //700년산 주니어 네키 술
            if (nItem > 0)
            {
                ret = inven.Exchange(0, 4001030, -nItem);
            }

            nItem = inven.ItemCount(4001031); //샤렌3세의 바지
            if (nItem > 0)
            {
                ret = inven.Exchange(0, 4001031, -nItem);
            }

            nItem = inven.ItemCount(4001032); //샤렌3세의 신발
            if (nItem > 0)
            {
                ret = inven.Exchange(0, 4001032, -nItem);
            }

            nItem = inven.ItemCount(4001033); //샤렌3세의 상의
            if (nItem > 0)
            {
                ret = inven.Exchange(0, 4001033, -nItem);
            }

            nItem = inven.ItemCount(4001034); //샤렌3세의 왕관
            if (nItem > 0)
            {
                ret = inven.Exchange(0, 4001034, -nItem);
            }

            nItem = inven.ItemCount(4001035); //악마의 표식
            if (nItem > 0)
            {
                ret = inven.Exchange(0, 4001035, -nItem);
            }

            nItem = inven.ItemCount(4001036); //투명아이템
            if (nItem > 0)
            {
                ret = inven.Exchange(0, 4001036, -nItem);
            }

            nItem = inven.ItemCount(4001037); //녹슨열쇠
            if (nItem > 0)
            {
                ret = inven.Exchange(0, 4001037, -nItem);
            }

            nItem = inven.ItemCount(1032033); //수호석
            if (nItem > 0)
            {
                ret = inven.Exchange(0, 1032033, -nItem);
            }

            nItem = inven.RemoveEquippedItem(1032033); //장비한 수호석 제거
            if (nItem == 0)
            {
                target.Message("The equipped Protector Rock has been removed.");
            }
            return;
        }
        public static int party_check_item()
        {

            var inven = target.Inventory;
            var hacking = 0;

            var nItem00 = inven.HasThisItemInHisParty(4001024);
            var nItem01 = inven.HasThisItemInHisParty(4001025);
            var nItem02 = inven.HasThisItemInHisParty(4001026);
            var nItem03 = inven.HasThisItemInHisParty(4001027);
            var nItem04 = inven.HasThisItemInHisParty(4001028);
            var nItem05 = inven.HasThisItemInHisParty(4001029);
            var nItem06 = inven.HasThisItemInHisParty(4001030);
            var nItem07 = inven.HasThisItemInHisParty(4001031);
            var nItem08 = inven.HasThisItemInHisParty(4001032);
            var nItem09 = inven.HasThisItemInHisParty(4001033);
            var nItem10 = inven.HasThisItemInHisParty(4001034);
            var nItem11 = inven.HasThisItemInHisParty(4001035);
            var nItem12 = inven.HasThisItemInHisParty(4001036);
            var nItem13 = inven.HasThisItemInHisParty(4001037);
            var nItem14 = inven.HasThisItemInHisParty(1032033);

            if (nItem00 > 0 || nItem01 > 0 || nItem02 > 0 || nItem03 > 0 || nItem04 > 0 || nItem05 > 0 || nItem06 > 0 || nItem07 > 0 || nItem08 > 0 || nItem09 > 0 || nItem10 > 0 || nItem11 > 0 || nItem12 > 0 || nItem13 > 0 || nItem14 > 0)

                hacking = 1;

            return hacking;

        }
        //Guild Quest explanation NPC
        [Script("guildquest1_comment")]
        class guildquest1_comment : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                guildquest1.self = self;
                guildquest1.target = target;
                var nAsk = 0;
                int v0;
                while (true)
                {
                    if (nAsk == 0)
                    {
                        v0 = self.AskMenu("We, the Union of Guilds, have been trying to decipher 'Emerald Tablet,' a treasured old relic, for a long time. As a result, we have found out that Sharenian, the mysterious country from the past, lay asleep here. We also found out that clues of #t4001024#, a legendary, mythical jewelry, may be here at the remains of Sharenian. This is why the Union of Guilds have opened Guild Quest to ultimately find #t4001024#.\r\n#b#L0# What's Sharenian?#l \r\n#b#L1# #t4001024#?#l\r\n#b#L2# Guild Quest?#l\r\n#b#L3# I'm fine now.#l");
                    }
                    else
                    {
                        v0 = self.AskMenu("Do you have any other questions?\r\n#b#L0# What's Sharenian?#l \r\n#b#L1# #t4001024#? What's that?#l\r\n#b#L2# Guild Quest?#l\r\n#b#L3# No, I'm fine now.#l");
                    }

                    if (v0 == 0)
                    {
                        self.Say("Sharenian was a literate civilization from the past that had control over every area of the Victoria Island. The Temple of Golem, the Shrine in the deep part of the Dungeon, and other old architectural constructions where no one knows who built it are indeed made during the Sharenian times.");
                        self.Say("The last king of Sharenian was a gentleman named Sharen III, and apparently he was a very wise and compassionate king. But one day, the whole kingdom collapsed, and there was no explanation made for it.");
                        nAsk = 1;
                    }
                    else if (v0 == 1)
                    {
                        self.Say("#t4001024# is a legendary jewel that brings eternal youth to the one that possesses it. Ironically, it seems like everyone that had #t4001024# ended up downtrodden, which should explain the downfall of Sharenian.");
                        nAsk = 1;
                    }
                    else if (v0 == 2)
                    {
                        self.Say("I've sent groups of the explorers to Sharenian before, but none of them ever came back, which prompted us to start the Guild Quest. We've been waiting for guilds that are strong enough to take on tough challenges, guilds like yours.");
                        self.Say("The ultimate goal of this Guild Quest is to explore Sharenian and find #t4001024#. This is not a task where power solves everything. Teamwork is more important here.");
                    }
                    else if (v0 == 3)
                    {
                        self.Say("Really? If you have anything else to ask, please feel free to talk to me.");
                        return;
                    }
                }
            }
        }
        //Guild Quest bbs NPC
        [Script("guildquest1_board")]
        class guildquest1_board : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                guildquest1.self = self;
                guildquest1.target = target;
                var field = target.Field;
                if (field.ID == 101030104)
                {
                    self.Say("<Notice> \r Are you the Guild that possesses an ample amount of courage and trust? Then take on the Guild Quest and challenge yourselves!\r \r #bTo Participate :#k\r 1. The Guild must consist of at least 6 people!\r 2. The leaders of the Guild should be the Master and the Jr. Master of the Guild!\r 3. The Guild Quest may end early if the guildmembers participating reaches below 6, or if the leader decides to end it early!");
                    return;
                }

                if (field.ID == 990000000)
                {
                    self.Say("#r<Warning>#k Danger! A force of evil is running through the Sharenian Castle!\r 1. There's a force of evil currently running through the Sharenian Castle, so don't get too close, or risk death.\r 2. Use the Returning Rock if you want to end the journey early!");
                    return;
                }
            }
        }
        //knight suit of armor
        [Script("guildquest1_knight")]
        class guildquest1_knight : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                guildquest1.self = self;
                guildquest1.target = target;
                self.Say("To successfully invade Sharenian, Ergoth the devil sealed up #rLonginus Spear#k, the spear that protected Sharenian. Restore the power of #rLonginus Spear#k, and the force of evil that's blocking the path may break.");
                return;
            }
        }
        //enter
        [Script("guildquest1_enter")]
        class guildquest1_enter : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                guildquest1.self = self;
                guildquest1.target = target;
                // entering available
                var set = FieldSet.Instances["Guild1"];
                var nRet = target.CanEnterGuildQuest;
                if (nRet == 1) // Master, sets registration in fieldset when entering
                {
                    self.Say("You are on your way to entering Sharenian. The door to Sharenian will open 3 minutes after entering.");
                    takeawayitem();

                    //----------- if party members have the party items before starting the pq
                    var party_hack = party_check_item();
                    if (party_hack == 1)
                    {
                        self.Say("A member in your party has an inappropriate item. Please try again.");
                        return;
                    }
                    //------------

                    var result = set.Enter(target, 0);
                    if (result != 0) self.Say("The Guild Quest is currently not ready, and therefore closed. Please try again later.");
                    return;
                }
                else if (nRet == 2) // Party member
                {
                    if (set.UserCount >= 30)
                    {
                        self.Say("Unable to join the Guild Quest due to all 30 slots for the Guild Quest being filled with participants.");
                        return;
                    }
                    else
                    {
                        self.Say("You are on your way to entering Sharenian. The door to Sharenian will open 3 minutes after entering.");
                        takeawayitem();

                        //----------- if party members have the party items before starting the pq
                        var party_hack = party_check_item();
                        if (party_hack == 1)
                        {
                            self.Say("A member in your party has an inappropriate item. Please try again.");
                            return;
                        }
                        //------------

                        target.ChangeMap(990000000, "");
                        return;
                    }
                }

                var guildCount = target.DealWithGuildQuest(0);
                while (true)
                {
                    var v0 = self.AskMenu("Hello, there. I'm Shuang, one of the relic excavators stationed here. I'm in charge of the registration for exploring <Sharenian> and the Guild Quest. To find out the requirements to join this quest, please check out the notice at the left bulletin board. \r\n#b#L0# Register for the Guild Quest#l \r\n#b#L1# The Waitlist for the Guild Quest#l\r\n#b#L2# Cancel registration#l ");

                    if (v0 == 0)
                    {
                        // target is not a guild master or vice master. not a proper guild
                        if (!target.IsGuildMaster && !target.IsGuildSubMaster)
                        {
                            self.Say("Only the Master and the Jr. Master of the Guild may register for the Guild.");
                            return;
                        }

                        //ask to register
                        if (guildCount < 0)
                        { // already registered -guildcount is a channel number
                            self.Say("Your Guild is already registered at Channel " + (-guildCount) + ".");
                        }
                        else
                        {
                            var say = "";
                            if (guildCount == 0)
                            {
                                say = "You'll be entering Sharenian really soon. Once it reaches your turn, you'll have 3 minutes to gather up at least 6 members of your Guild here and enter the place through me. Would you like to register and wait?";
                            }
                            else if (guildCount == 1)
                            {
                                say = "There's currently 1 Guild participating in the quest, and, no other guild is waiting in line. Would you like to register and wait?";
                            }
                            else
                            {
                                say = "There's currently 1 Guild participating in the quest, and, " + (guildCount - 1) + " other Guild(s) are waiting in line. Would you like to register and wait?";
                            }
                            nRet = self.AskYesNo(say);
                            if (nRet == 0) self.Say("I'm very busy here, so please make a decision immediately and then talk to me.");
                            else
                            {
                                nRet = target.DealWithGuildQuest(1);
                                if (nRet == 1) self.Say("Your Guild has been registered, so please wait. Talk to me when your turn comes.");
                                else if (nRet < 0)
                                {
                                    self.Say("You're already registered at Channel " + (-guildCount) + ".");
                                }
                                else self.Say("Something's not right here. The registration's not working...");
                            }
                        }
                        return;
                    }
                    else if (v0 == 1)
                    {
                        //register view
                        var waitNum = target.DealWithGuildQuest(3);
                        if (waitNum == -1)
                        {
                            if (guildCount > 1)
                            {
                                self.Say("There are currently " + (guildCount - 1) + " Guild(s) waiting in line.\r I'm afraid " + target.SCharacterName + " is not in the list of guildmembers waiting in line.");
                            }
                            else self.Say("No other guild is waiting in line.\r I'm afraid " + target.SCharacterName + " is not in the list of guildmembers waiting in line.");
                        }
                        else if (waitNum == 0)
                        {
                            self.Say("You must be a member of the Guild that's currently participating in the Guild Quest.");
                        }
                        else
                        {
                            self.Say(target.SCharacterName + "'s Guild is number " + waitNum + " on the waiting list at Channel " + (-guildCount));
                        }
                        return;
                    }
                    else if (v0 == 2)
                    {
                        // target is not a guild master or vice master. not a proper guild
                        if (!target.IsGuildMaster && !target.IsGuildSubMaster)
                        {
                            self.Say("Only the Master and the Jr. Master of the Guild may cancel the registration.");
                            return;
                        }
                        //register cancel
                        if (guildCount < 0)
                        { // already registered -guildcount is a channel number
                            nRet = self.AskYesNo("You're currently registered at Channel" + (-guildCount) + ". If you cancel the registration now, you'll have to register all over again the next time up. Will you still cancel it?");
                            if (nRet == 0) self.Say("Please wait.");
                            else
                            {
                                nRet = target.DealWithGuildQuest(2);
                                if (nRet == 1) self.Say("The registration has been cancelled.");
                                else self.Say("You may not cancel the Guild Quest once it's your turn. Please check again.");
                            }
                        }
                        else self.Say("Your Guild is not registered.");
                        return;
                    }
                }
            }
        }
        // Guild Quest 클리어 NPC
        [Script("guildquest1_clear")]
        class guildquest1_clear : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                guildquest1.self = self;
                guildquest1.target = target;
                // 퇴장 가능한 조건을 여기에 추가해야 함.
                // checking the item which boss mob drops
                // don't let users take the item out of the room
                // npc should take the item when users exit from the map
                var field = target.Field;
                var quest = FieldSet.Instances["Guild1"];

                var inven = target.Inventory;
                var nItem = inven.ItemCount(4001024);

                if (nItem >= 1)
                {
                    self.Say("What?! You really brought #t4001024#? I've heard a word that a brave group of people have taken on the Guild Quest, but I really did not think you would make it.");
                    self.Say("The #t4001024# that you found is a historical item that'll live on forever as one of the most important excavations in the history of there. Okay, I'll send you now to the camp. ");
                }
                else self.Say("I'll send you all now to the camp.");
                takeawayitem(); //item delete
                target.ChangeMap(101030104, "");
                return;

            }
        }
        //protector rock
        [Script("guildquest1_out")]
        class guildquest1_out : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                guildquest1.self = self;
                guildquest1.target = target;
                var say = "Are you sure you want to prematurely end the Guild Quest? The next time you come back, you'll have to start all over again. The Guild Quest will also end if the Guild does not consist of at least 6 people. ";
                var ret = self.AskYesNo(say);
                if (ret == 1)
                {
                    say = "Are you sure you want to end it?";
                    var ret2 = self.AskYesNo(say);
                    if (ret2 == 1)
                    {
                        var quest = FieldSet.Instances["Guild1"];
                        self.Say("You have ended Guild Quest, and will be moved to the returning path out.");
                        target.ChangeMap(990001100, "st00");
                        say = target.SCharacterName + " has prematurely ended the Guild Quest.";
                        quest.BroadcastMsg(6, say);
                    }
                    if (ret2 == 0)
                    {
                        return;
                    }
                    return;
                }
                else
                {
                    return;
                }
            }
        }
        //npc who send all guild members to bonus map after they clear the boss
        [Script("guildquest1_bonus")]
        class guildquest1_bonus : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                guildquest1.self = self;
                guildquest1.target = target;
                if (!target.IsGuildQuestLeader)
                {
                    self.Say("I need the leader that possesses #t4001024# to talk to me.");
                    return;
                }

                var inven = target.Inventory;
                var nItem = inven.ItemCount(4001024); //루 비안을 가지고 있는지 체크
                if (nItem >= 1)
                {
                    self.Say("You have defeated the mighty Ergoth and brought glory back to Sharenian!");
                    var field = target.Field;
                    field.EffectScreen("quest/party/clear");
                    field.EffectSound("Party1/Clear");
                    self.Say("You, the possessor of the power of #t4001024#, I'll lead you to the treasure warehous of Sharenian.");
                    //delete Rubian
                    var ret = inven.Exchange(0, 4001024, -nItem);

                    var quest = FieldSet.Instances["Guild1"];

                    //reward GP
                    var cleartime = quest.GetQuestTime;
                    var min = cleartime / 60;

                    var point = 0;
                    if (min >= 0 && min < 25)
                    {
                        point = 850;
                    }
                    else if (min >= 25 && min < 30) {
                        point = 800 - ((min - 25) * 20);
                    } else if (min >= 30 && min < 40) {
                        point = 580 - ((min - 30) * 10);
                    } else if (min >= 40 && min < 60) {
                        point = 400 - ((min - 40) * 5);
                    } else if (min >= 60 && min < 90) {
                        point = 260 - ((min - 60) * 2);
                    } else if (min >= 90)
                    {
                        point = 200;
                    }

                    if (quest.GetVar("isIncGuildPointState08") != "1")
                    {
                        target.IncGuildPoint(point);
                        quest.SetVar("isIncGuildPointState08", "1");
                    }

                    var say = "You have cleared the Guild Quest. Talk to Nuris to leave this map.";
                    quest.BroadcastMsg(6, say);

                    //send all guild members to bonus map (990001000) 
                    var bonusmap = FieldSet.Instances["Guild1Bonus"];
                    var res = bonusmap.Enter(target, 0);
                    if (res == 0) quest.TransferFieldAll(990001000, "st00");
                    return;
                }
                else
                {
                    self.Say("Do you possess the power of #t4001024#?");
                    return;
                }
            }
        }
        //E-F map trasfer
        [Script("guild1F00")]
        class guild1F00 : IPortalScript
        {
            public void Run(IPortalHost self, GameCharacter target)
            {
                var qr = target.QuestRecord;
                var val = qr.Get(7600);

                target.PlayPortalSE();

                if (val == "0") target.ChangeMap(990000611, "st00");
                else if (val == "1") target.ChangeMap(990000620, "st00");
                else if (val == "2") target.ChangeMap(990000631, "st00");
                else target.ChangeMap(990000641, "st00");
                qr.SetComplete(7600);
            }
        }
        [Script("guild1F01")]
        class guild1F01 : IPortalScript
        {
            public void Run(IPortalHost self, GameCharacter target)
            {
                var quest = FieldSet.Instances["Guild1"];
                if (quest.GetVar("isIncGuildPointState04") != "1")
                {
                    target.IncGuildPoint(5);
                    quest.SetVar("isIncGuildPointState04", "1");
                }

                var qr = target.QuestRecord;
                qr.Set(7600, "0");
                target.PlayPortalSE();
                target.ChangeMap(990000700, "st00");
            }
        }
        [Script("guild1F02")]
        class guild1F02 : IPortalScript
        {
            public void Run(IPortalHost self, GameCharacter target)
            {
                var quest = FieldSet.Instances["Guild1"];
                if (quest.GetVar("isIncGuildPointState05") != "1")
                {
                    target.IncGuildPoint(5);
                    quest.SetVar("isIncGuildPointState05", "1");
                }

                var qr = target.QuestRecord;
                qr.Set(7600, "1");
                target.PlayPortalSE();
                target.ChangeMap(990000700, "st00");
            }
        }
        [Script("guild1F03")]
        class guild1F03 : IPortalScript
        {
            public void Run(IPortalHost self, GameCharacter target)
            {
                var quest = FieldSet.Instances["Guild1"];
                if (quest.GetVar("isIncGuildPointState06") != "1")
                {
                    target.IncGuildPoint(5);
                    quest.SetVar("isIncGuildPointState06", "1");
                }

                var qr = target.QuestRecord;
                qr.Set(7600, "2");
                target.PlayPortalSE();
                target.ChangeMap(990000700, "st00");
            }
        }
        [Script("guild1F04")]
        class guild1F04 : IPortalScript
        {
            public void Run(IPortalHost self, GameCharacter target)
            {
                var quest = FieldSet.Instances["Guild1"];
                if (quest.GetVar("isIncGuildPointState07") != "1")
                {
                    target.IncGuildPoint(5);
                    quest.SetVar("isIncGuildPointState07", "1");
                }

                var qr = target.QuestRecord;
                qr.Set(7600, "3");
                target.PlayPortalSE();
                target.ChangeMap(990000700, "st00");
            }
        }
        [Script("statuegate_open")]
        class statuegate_open : IPortalScript
        {
            public void Run(IPortalHost self, GameCharacter target)
            {
                var quest = FieldSet.Instances["Guild1"];

                var gateopen = quest.GetVar("statuegateopen");
                if (gateopen == "yes")
                {
                    var inven = target.Inventory;
                    var nItem = inven.ItemCount(1032033);
                    if (nItem != 0)
                    {
                        target.Message("The power of the Protector Rock does not feel right for some reason. You may only possess one of those, not many.");
                        return;
                    }
                    else
                    {
                        target.ChangeMap(990000301, "st00");
                        if (quest.GetVar("isIncGuildPointState01") != "1")
                        {
                            target.IncGuildPoint(15);
                            quest.SetVar("isIncGuildPointState01", "1");
                        }
                    }
                }
                else
                {
                    target.Message("The entrance is closed.");
                    return;
                }
            }
        }
        [Script("guildquest1_statue")]
        class guildquest1_statue : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                guildquest1.self = self;
                guildquest1.target = target;
                var set = FieldSet.Instances["Guild1"];

                var var = set.GetVar("statue");
                var question = set.GetVar("statueQuestion");
                var answer = set.GetVar("statueAnswer");

                if (set.GetVar("statuecleared") == "1")
                {
                    self.Say("You have passed the test. Your guild is worthy of entering the castle.");
                    return;
                }

                if (!target.IsGuildQuestLeader)
                {
                    self.Say("To the fearless ones that dare enter Sharenian, I'm the gateskeeper that protects this castle. I shall put you all through tests to see if you're worthy of entering the premise.");
                    self.Say("These statues are all my other selves. I'll be teleporting from one statue to another. Follow my path closely, and remember the order.");
                    self.Say("The brave one that registered your Guild for the Guild Quest shall now talk to me.");
                    return;
                }

                if (question == "")
                {
                    if (var == "")
                    {
                        self.Say("To the fearless ones that dare enter Sharenian, I'm the gateskeeper that protects this castle. I shall put you all through tests to see if you're worthy of entering the premise.");
                        self.Say("These statues are all my other selves. I'll be teleporting from one statue to another. Follow my path closely, and remember the order.");
                        set.SetVar("statue", "4");    // 4 statues twinkles
                    }

                    var statuenum = int.Parse(set.GetVar("statue"));
                    var say = statuenum + " statues shall light up. Remember them, and strike them in order. Then, and only then, you shall talk to me.";
                    self.Say(say);
                    question = setstatuequestion(statuenum);
                    set.SetVar("statueQuestion", question);
                    set.SetVar("statueAnswer", "00000000000000000000");
                    if (MasterThread.IsDebug)
                        target.Notice("Statue question: " + question);

                    for (int i = 1; i <= statuenum; i++)
                    {
                        int reactorID = question.IndexOf(i.ToString()) + 1;
                        int delay = 3000 * i;
                        MasterThread.RepeatingAction.Start(() =>
                        {

                            set.TriggerReactor(3, reactorID.ToString());
                        }, delay, 0);
                    }
                }
                else if (answer == question)
                {
                    if (int.Parse(var) < 6)
                    {        // 6 statues twinkles and mission completed
                        self.Say("You must be the wise one. That was the correct answer! Next stage.");
                        set.SetVar("statue", (int.Parse(var) + 1).ToString());
                        set.SetVar("statueQuestion", "");
                        string say = null;
                        if (int.Parse(var) == 4)
                        {
                            say = "You have passed the 1st stage of the Gateskeeper Test.";
                        }
                        else if (int.Parse(var) == 5)
                        {
                            say = "You have passed the 2nd stage of the Gateskeeper Test.";
                        }
                        set.BroadcastMsg(6, say);
                    }
                    else
                    {
                        self.Say("Brilliant! I will open the door to the next stage.");
                        var say = "You have passed all stages of the Gateskeeper Test.";
                        set.BroadcastMsg(6, say);
                        // 문 열기
                        set.TriggerReactor(3, "statuegate");
                        var field = target.Field;
                        field.EffectScreen("quest/party/clear");
                        field.EffectSound("Party1/Clear");
                        set.SetVar("statuegateopen", "yes");
                        set.SetVar("statuecleared", "1");
                        return;
                    }
                }
                else
                {
                    if (answer == "" || answer == "00000000000000000000") self.Say("Talk to me AFTER striking the statues.");
                    else
                    {
                        self.Say("Incorrect. If you desire for another chance, then talk to me.");
                        string say = "You have failed the Gateskeeper Test.";
                        set.BroadcastMsg(6, say);
                        set.SetVar("statue", "4");
                        set.SetVar("statueQuestion", "");
                    }
                }
            }
        }
        //각 문을 여는 스크립트
        [Script("speargate_open")]
        class speargate_open : IPortalScript
        {
            public void Run(IPortalHost self, GameCharacter target)
            {
                var quest = FieldSet.Instances["Guild1"];
                var gateopen = quest.GetVar("speargateopen");
                if (gateopen == "yes")
                {
                    target.PlayPortalSE();
                    target.ChangeMap(990000401, "st00");
                    if (quest.GetVar("isIncGuildPointState02") != "1")
                    {
                        target.IncGuildPoint(20);
                        quest.SetVar("isIncGuildPointState02", "1");
                    }
                }
                else
                {
                    target.Message("The entrance is closed.");
                    return;
                }
            }
        }
        [Script("stonegate_open")]
        class stonegate_open : IPortalScript
        {
            public void Run(IPortalHost self, GameCharacter target)
            {
                var quest = FieldSet.Instances["Guild1"];

                var gateopen = quest.GetVar("stonegateopen");
                if (gateopen == "yes")
                {
                    target.PlayPortalSE();
                    target.ChangeMap(990000430, "out00");
                }
                else
                {
                    target.Message("The entrance is closed.");
                    return;
                }
            }
        }
        [Script("metalgate_open")]
        class metalgate_open : IPortalScript
        {
            public void Run(IPortalHost self, GameCharacter target)
            {
                var quest = FieldSet.Instances["Guild1"];

                var gateopen = quest.GetVar("metalgateopen");
                if (gateopen == "yes")
                {
                    target.PlayPortalSE();
                    target.ChangeMap(990000431, "out00");
                }
                else
                {
                    target.Message("The entrance is closed.");
                    return;
                }
            }
        }
        [Script("watergate_open")]
        class watergate_open : IPortalScript
        {
            public void Run(IPortalHost self, GameCharacter target)
            {
                var quest = FieldSet.Instances["Guild1"];

                var gateopen = quest.GetVar("watergateopen");
                if (gateopen == "yes")
                {
                    target.PlayPortalSE();
                    target.ChangeMap(990000600, "st00");
                    if (quest.GetVar("isIncGuildPointState03") != "1")
                    {
                        target.IncGuildPoint(25);
                        quest.SetVar("isIncGuildPointState03", "1");
                    }
                }
                else
                {
                    target.Message("The entrance is closed.");
                    return;
                }
            }
        }
        [Script("under30gate")]
        class under30gate : IPortalScript
        {
            public void Run(IPortalHost self, GameCharacter target)
            {
                if (target.Level <= 30)
                {
                    target.PlayPortalSE();
                    target.ChangeMap(990000640, "out00");
                }
                else
                {
                    target.Message("A powerful wall of force that cannot be seen is blocking the entrance.");
                    return;
                }
            }
        }
        [Script("secretgate1_open")]
        class secretgate1_open : IPortalScript
        {
            public void Run(IPortalHost self, GameCharacter target)
            {
                var quest = FieldSet.Instances["Guild1"];

                var gateopen = quest.GetVar("secretgate1open");
                if (gateopen == "yes")
                {
                    target.PlayPortalSE();
                    target.ChangeMap(990000611, "out00");
                }
                else
                {
                    target.Message("The entrance is closed.");
                    return;
                }
            }
        }
        [Script("secretgate2_open")]
        class secretgate2_open : IPortalScript
        {
            public void Run(IPortalHost self, GameCharacter target)
            {
                var quest = FieldSet.Instances["Guild1"];

                var gateopen = quest.GetVar("secretgate2open");
                if (gateopen == "yes")
                {
                    target.PlayPortalSE();
                    target.ChangeMap(990000631, "out00");
                }
                else
                {
                    target.Message("The entrance is closed.");
                    return;
                }
            }
        }
        [Script("secretgate3_open")]
        class secretgate3_open : IPortalScript
        {
            public void Run(IPortalHost self, GameCharacter target)
            {
                var quest = FieldSet.Instances["Guild1"];

                var gateopen = quest.GetVar("secretgate3open");
                if (gateopen == "yes")
                {
                    target.PlayPortalSE();
                    target.ChangeMap(990000641, "out00");
                }
                else
                {
                    target.Message("The entrance is closed.");
                    return;
                }
            }
        }
        [Script("ghostgate_open")]
        class ghostgate_open : IPortalScript
        {
            public void Run(IPortalHost self, GameCharacter target)
            {
                var quest = FieldSet.Instances["Guild1"];

                var gateopen = quest.GetVar("ghostgateopen");
                if (gateopen == "yes")
                {
                    if (quest.GetVar("isIncGuildPointState09") != "1")
                    {
                        target.IncGuildPoint(10);
                        quest.SetVar("isIncGuildPointState09", "1");
                    }
                    target.PlayPortalSE();
                    target.ChangeMap(990000800, "st00");
                }
                else
                {
                    target.Message("The entrance is closed.");
                    return;
                }
            }
        }
        [Script("kinggate2_open")]
        class kinggate2_open : IPortalScript
        {
            public void Run(IPortalHost self, GameCharacter target)
            {
                var quest = FieldSet.Instances["Guild1"];

                var gateopen = quest.GetVar("kinggateopen");
                if (gateopen == "yes")
                {
                    target.PlayPortalSE();
                    target.ChangeMap(990000900, "st01");
                }
                else
                {
                    target.Message("The portal is currently closed.");
                    return;
                }
            }
        }
        [Script("kinggate_open")]
        class kinggate_open : IPortalScript
        {
            public void Run(IPortalHost self, GameCharacter target)
            {
                var quest = FieldSet.Instances["Guild1"];

                var gateopen = quest.GetVar("kinggateopen");
                if (gateopen == "yes")
                {
                    target.PlayPortalSE();
                    target.ChangeMap(990000900, "st00");
                }
                else
                {
                    target.Message("The entrance is closed.");
                    return;
                }
            }
        }
        //Sharen III's soul
        [Script("guildquest1_NPC1")]
        class guildquest1_NPC1 : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                guildquest1.self = self;
                guildquest1.target = target;
                var set = FieldSet.Instances["Guild1"];

                if (set.GetVar("npc1_talk") == "end")
                {
                    self.Say("You are almost there, where the throne is, but the entrance to the throne is firmly shut with Ergoth's evil force blocking the way. \r To break this, it'll require the #rforming of the holy powers#k...");
                    return;
                }

                if (!target.IsGuildQuestLeader)
                {
                    self.Say("After what I thought would be an eternal sleep, I have finally found someone that will save Sharenian.");
                    self.Say("The leader of your Guild should talk to me.");
                    return;
                }

                if (set.GetVar("npc1_talk") != "end")
                {
                    self.Say("After what I thought would be an eternal sleep, I have finally found someone that will save Sharenian. This old man will now pave the way for you to continue the quest.");
                    set.TriggerReactor(23, "ghostgate");
                    var field = target.Field;
                    field.EffectScreen("quest/party/clear");
                    field.EffectSound("Party1/Clear");
                    set.SetVar("ghostgateopen", "yes");
                    set.SetVar("npc1_talk", "end");
                    return;
                }
            }
        }
        //Sharen III's testament
        [Script("guildquest1_will")]
        class guildquest1_will : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                guildquest1.self = self;
                guildquest1.target = target;
                self.Say("I, Sharen III, lay here dying, the sheer bitterness overtaking my body. \r It's a colossal mistake to summon Ergoth the devil to protect Rubian.");
                self.Say("Ergoth got intoxicated with the mysterious force of Rubian, brought other devilish monsters with it, and attacked Sharenian. I, the king of Sharenian, have thrown away the king's robe, desperately running away from the mayhem, and wound up dying at the waterway.");
                self.Say("I have but no one to blame for the downfall of Sharenian, but it infuriates me that I may die here without any clothes! To anyone that reads my will down the road, if you feel any sympathy towards me, will you help me find my clothes?");
                self.Say("#v4001032# #t4001032# \r #v4001031# #t4001031# \r #v4001033# #t4001033# \r #v4001034# #t4001034# \r If you find these, and dress me up from the bottom up like I always did, then I may finally rest in peace. Patience is a virtue. I recommend you drop one item at a time, and wait until it reacts.");
                return;
            }
        }
        [Script("guildquest1_baseball")]
        class guildquest1_baseball : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                guildquest1.self = self;
                guildquest1.target = target;
                var quest = FieldSet.Instances["Guild1"];

                var vvar = quest.GetVar("baseball");
                var btry = quest.GetVar("baseballTry");

                if (quest.GetVar("watergatecleared") == "1")
                {
                    self.Say("The vassals are thankful that you have found the royal gift. Here's hoping everything turns out well for you...");
                    return;
                }

                if (!target.IsGuildQuestLeader)
                {
                    self.Say("The four statues you see in front of the fountain are the old vassals of the Sharenian. These vassals possessed priceless treasures bestowed by the king while they were alive. If you put those royal gifts as offerings, then they'll open the secret door that'll reveal the path to the inside of the castle..");
                    self.Say("These are the items you'll need as offerings:\r #v4001027# #t4001027#\r #v4001028# #t4001028#\r #v4001029# #t4001029#\r #v4001030# #t4001030#\r. However, there's no way to find out who wants what, and some of the items may be downright foreign, so discard the unnecessary ones.");
                    self.Say("Lay down the offering in front of the statue, and then ask if it's the right one. Please beware that these vassals will not be pleased if you make too many mistakes here. You have 7 chances to make this right!");
                    self.Say("Now, please start the Guild Quest with your Guild leader.");
                    return;
                }

                if (vvar == "")
                {
                    self.Say("The four statues you see in front of the fountain are the old vassals of the Sharenian. These vassals possessed priceless treasures bestowed by the king while they were alive. If you put those royal gifts as offerings, then they'll open the secret door that'll reveal the path to the inside of the castle..");
                    self.Say("These are the items you'll need as offerings:\r #v4001027# #t4001027#\r #v4001028# #t4001028#\r #v4001029# #t4001029#\r #v4001030# #t4001030#\r. However, there's no way to find out who wants what, and some of the items may be downright foreign, so discard the unnecessary ones.");
                    self.Say("Lay down the offering in front of the statue, and then ask if it's the right one. Please beware that these vassals will not be pleased if you make too many mistakes here. You have 7 chances to make this right!");
                    var answer = "";
                    for (var i = 1; i <= 4; i++)
                    {
                        answer += Rand32.NextBetween(1, 4).ToString();
                    }
                    if (MasterThread.IsDebug) target.Notice("Baseball answer " + answer);
                    quest.SetVar("baseball", answer);
                }
                else
                {
                    var result = "";

                    for (var areaId = 1; areaId <= 4; areaId++)
                    {
                        var itemidindex = 0;
                        for (var itemIdx = 1; itemIdx <= 4; itemIdx++)
                        {
                            var itemid = getitemid(itemIdx);
                            var field = target.Field;
                            var ret = field.IsItemInArea(areaId.ToString(), itemid);
                            if (ret == -1)
                            {
                                self.Say("Only one offering per statue.");
                                return;
                            }
                            else if (ret == 1) itemidindex = itemIdx;
                        }
                        if (itemidindex == 0)
                        {
                            self.Say("Either you have not dropped the offering, or this item is bestowed by the king.");
                            return;
                        }
                        result += itemidindex.ToString();
                    }

                    var baseballTry = (btry.ToIntSafe() + 1).ToString();
                    if (MasterThread.IsDebug) target.Notice("Baseball try " + baseballTry);
                    quest.SetVar("baseballTry", baseballTry);
                    var strike = 0;
                    var ball = 0;
                    for (int i = 1; i <= 4; i++)
                    {
                        var num = vvar.Substring(i - 1, 1);
                        var num2 = result.Substring(i - 1, 1);
                        if (num == num2)
                        {
                            vvar = changebaseballstring(vvar, i - 1);
                            result = changebaseballstring(result, i - 1);
                            strike++;
                        }
                    }
                    for (var i = 1; i <= 4; i++)
                    {
                        var num = vvar.Substring(i - 1, 1);
                        if (num != "0")
                        {
                            var isball = 0;
                            for (int j = 1; j <= 4; j++)
                            {
                                var num2 = result.Substring(j - 1, 1);
                                if (num == num2 && isball == 0)
                                {
                                    result = changebaseballstring(result, j - 1);
                                    ball++;
                                    isball = 1;
                                }
                            }
                        }
                    }

                    var unknown = 4 - (strike + ball);
                    var tried = btry.ToIntSafe() + 1;

                    // they got the right answer
                    if (strike == 4 && ball == 0)
                    {
                        self.Say("All the vassals agree that this is the right offering.");
                        self.Say("The vassals are most pleased with regaining the items bestowed to them by the king. They will open the secret door for you. Here's hoping everything turns out well for you.");
                        //open the door
                        var say = "You have passed the vassal's offering. The secret door is now open.";
                        quest.BroadcastMsg(6, say);
                        quest.TriggerReactor(12, "watergate");
                        var field = target.Field;
                        field.EffectScreen("quest/party/clear");
                        field.EffectSound("Party1/Clear");
                        quest.SetVar("watergateopen", "yes");
                        quest.SetVar("watergatecleared", "1");
                        return;
                    }

                    //several answers
                    if (strike == 0 && ball == 0)
                    {
                        if (tried == 6)
                        {
                            self.Say("No vassal knows of this offering.\r You have tried this " + tried + " time(s). \r You have one chance left. Be careful!");
                        }
                        else if (tried >= 7)
                        {
                            self.Say("You have enraged the vassals by bringing strange offerings to the table! The Dark Knights shall be swiftly summoned by them, so be careful!");
                            string say = "Failure to find out the correct offering of the vassals have resulted in monsters being summoned here.";
                            quest.BroadcastMsg(6, say);
                            quest.SetVar("baseball", "");
                            quest.SetVar("baseballTry", "");
                            mobSummon();
                            return;
                        }
                        else
                        {
                            self.Say("No vassal knows of this offering.\r You have tried this " + tried + " time(s).");
                        }
                        var field = target.Field;
                        field.SummonMob(-476, 140, 2100026);
                        field.SummonMob(552, 140, 2100027);
                        return;
                    }
                    else if (strike + ball == 4)
                    {
                        if (strike == 0)
                        {
                            if (tried == 6)
                            {
                                string say = ball + " have declared the offering incorrect.\r You have tried this " + tried + " time(s). \r You have one chance left. Be careful!";
                                self.Say(say);
                            }
                            else if (tried >= 7)
                            {
                                self.Say("You have enraged the vassals by bringing strange offerings to the table! The Dark Knights shall be swiftly summoned by them, so be careful!");
                                var say = "Failure to place the offering of the vassals the right way have resulted in monsters being summoned here.";
                                quest.BroadcastMsg(6, say);
                                quest.SetVar("baseball", "");
                                quest.SetVar("baseballTry", "");
                                mobSummon();
                                return;
                            }
                            else
                            {
                                var say = ball + " have declared the offering incorrect.\r You have tried this " + tried + " time(s).";
                                self.Say(say);
                            }
                            var field = target.Field;
                            field.SummonMob(-476, 140, 2100026);
                            field.SummonMob(552, 140, 2100027);
                            return;
                        }
                        else
                        {
                            if (tried == 6)
                            {
                                var say = strike + " agreed that the offering is correct. \r " + ball + " have declared the offering incorrect.\r You have tried this " + tried + " time(s). \r You have one chance left. Be careful!";
                                self.Say(say);
                            }
                            else if (tried >= 7)
                            {
                                self.Say("You have enraged the vassals by bringing strange offerings to the table! The Dark Knights shall be swiftly summoned by them, so be careful!");
                                var say = "Failure to place the offering of the vassals the right way have resulted in monsters being summoned here.";
                                quest.BroadcastMsg(6, say);
                                quest.SetVar("baseball", "");
                                quest.SetVar("baseballTry", "");
                                mobSummon();
                                return;
                            }
                            else
                            {
                                var say = strike + " agreed that the offering is correct. \r " + ball + " have declared the offering incorrect. \r You have tried this " + tried + " time(s).";
                                self.Say(say);
                            }
                            var field = target.Field;
                            field.SummonMob(-476, 140, 2100026);
                            field.SummonMob(552, 140, 2100027);
                            return;
                        }
                    }
                    else if (strike + ball < 4)
                    {
                        if (strike == 0)
                        {
                            if (tried == 6)
                            {
                                var say = ball + " have declared the offering incorrect. \r " + unknown + " have even said it's an unknown offering. \r You have now tried this " + tried + " times. \r This is your last chance, so please You have one chance left. Be careful!";
                                self.Say(say);
                            }
                            else if (tried >= 7)
                            {
                                self.Say("You have enraged the vassals by bringing strange offerings to the table! The Dark Knights shall be swiftly summoned by them, so be careful!");
                                var say = "Failure to place the offering of the vassals the right way have resulted in monsters being summoned here.";
                                quest.BroadcastMsg(6, say);
                                quest.SetVar("baseball", "");
                                quest.SetVar("baseballTry", "");
                                mobSummon();
                                return;
                            }
                            else
                            {
                                var say = ball + " have declared the offering incorrect. \r " + unknown + " have even said it's an unknown offering. \r You have tried this " + tried + " time(s).";
                                self.Say(say);
                            }
                            var field = target.Field;
                            field.SummonMob(-476, 140, 2100026);
                            field.SummonMob(552, 140, 2100027);
                            return;
                        }
                        else if (ball == 0)
                        {
                            if (tried == 6)
                            {
                                var say = strike + " agreed that the offering is correct. \r " + unknown + " have said it's an unknown offering.\r You have tried this " + tried + " time(s). \r You have one chance left. Be careful!";
                                self.Say(say);
                            }
                            else if (tried >= 7)
                            {
                                self.Say("You have enraged the vassals by bringing strange offerings to the table! The Dark Knights shall be swiftly summoned by them, so be careful!");
                                var say = "Failure to place the offering of the vassals the right way have resulted in monsters being summoned here.";
                                quest.BroadcastMsg(6, say);
                                quest.SetVar("baseball", "");
                                quest.SetVar("baseballTry", "");
                                mobSummon();
                                return;
                            }
                            else
                            {
                                var say = strike + " agreed that the offering is correct. \r " + unknown + " have said it's an unknown offering. \r You have tried this " + tried + " time(s).";
                                self.Say(say);
                            }
                            var field = target.Field;
                            field.SummonMob(-476, 140, 2100026);
                            field.SummonMob(552, 140, 2100027);
                            return;
                        }
                        else
                        {
                            if (tried == 6)
                            {
                                var say = strike + " agreed that the offering is correct \r " + ball + " have declared it incorrect. \r " + unknown + " have said it's an unknown offering. \r You have tried this " + tried + " time(s). \r You have one chance left. Be careful!";
                                self.Say(say);
                            }
                            else if (tried >= 7)
                            {
                                self.Say("You have enraged the vassals by bringing strange offerings to the table! The Dark Knights shall be swiftly summoned by them, so be careful!");
                                var say = "Failure to place the offering of the vassals the right way have resulted in monsters being summoned here.";
                                quest.BroadcastMsg(6, say);
                                quest.SetVar("baseball", "");
                                quest.SetVar("baseballTry", "");
                                mobSummon();
                                return;
                            }
                            else
                            {
                                var say = strike + " agreed that the offering is correct. \r " + ball + " have declared the offering incorrect.\r " + unknown + " have said it's an unknown offering.\r You have tried this " + tried + " time(s).";
                                self.Say(say);
                            }
                            var field = target.Field;
                            field.SummonMob(-476, 140, 2100026);
                            field.SummonMob(552, 140, 2100027);
                            return;
                        }
                    }
                }
            }
        }
    }
    [Script("syarenStatue")]
    class SyarenStatue : IReactorScript
    {
        public void Run(IReactorHost host, FieldReactor target)
        {
            if (target.Owner == null)
            {
                if (MasterThread.IsDebug) target.Field.Message($"Statue \"{target.Name}\" ({target.ID}) lit up without owner", BroadcastMessageType.Notice);
                return;
            }

            var set = FieldSet.Instances["Guild1"];
            if (!set.Started) return;

            var answer = set.GetVar("statueAnswer"); // "00000000000000000000"
            if (string.IsNullOrWhiteSpace(answer)) return;
            if (!int.TryParse(target.Name, out int reactorID)) return;
            int reactorIdx = reactorID - 1;

            int phase = answer.ToString().Replace("0", "").Length + 1;
            answer = answer.Remove(reactorIdx, 1).Insert(reactorIdx, phase.ToString());
            set.SetVar("statueAnswer", answer);
            if (MasterThread.IsDebug)
                target.Owner.Notice($"Statue \"{target.Name}\" ({target.ID}) lit up; phase={phase}; answer={answer}", BroadcastMessageType.Notice);
        }
    }
}
