using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using WvsBeta;
using WvsBeta.Common;
using WvsBeta.Game;
using WvsBeta.Game.Handlers.Contimove;
using WvsBeta.Game.Scripting;

namespace WvsBeta.Scripts.Scripts
{
    public static class EventScript
    {
        public static int check_kawi(GameCharacter target)
        {
            var inven = target.Inventory;
            var iCode = 4031332;
            var result = 0;
            for (int i = 0; i <= 9; i++)
            {
                var code = iCode + i;
                if (inven.ItemCount(code) > 0)
                {
                    result = 1;
                    break;
                }
                else
                {
                    result = 0;
                }
            }
            return result;
        }

        struct EventData
        {
            public Map Map { get; }
            public string Name { get; }
            public int Capacity { get; }
            public EventData(int mapID, int capacity, string name = null)
            {
                Map = GameDataProvider.Maps[mapID];
                Name = name ?? Map.Name;
                Capacity = capacity;
            }
        }

        private readonly static IList<EventData> Events = new List<EventData>()
        {
            new EventData(109030001, 80),
            new EventData(109030101, 80),
            new EventData(109030201, 80),
            new EventData(109030301, 80),
            new EventData(109030401, 80),

            new EventData(109040000, 70),
            new EventData(109020001, 90),

            new EventData(109080000, 60),
            new EventData(109080001, 60),
            new EventData(109080002, 60),

            new EventData(109060001, 80),
            new EventData(109010000, 60),
        };

        // Paul : 9000000 
        [Script("Event00")]
        class Event00 : INpcScript
        {
            private readonly static string[] eventMenu;
            private readonly static string[] eventMenuWithClose;
            private readonly static string userEventMenu;
            static Event00()
            {
                eventMenu = new string[Events.Count];
                eventMenu = Events.Select(e => e.Name + " (" + e.Map.ID + ")").ToArray();

                eventMenuWithClose = new string[Events.Count + 1];
                for (int i = 0; i < eventMenu.Length; i++) eventMenuWithClose[i] = eventMenu[i];
                eventMenuWithClose[Events.Count] = "Close Event Map Entry";

                userEventMenu = @"\r\n#L0##e1. #n#bWhat kind of event is this?#k#l\r\n#L1##e2. #n#bExplain the game event to me.#k#l\r\n#L2##e3. #n#bThat's right, let's go!#k#l";
            }

            public void Run(INpcHost self, GameCharacter target)
            {
                var field = self.Field;
                var cmap = field.ID;

                if (target.IsAdmin)
                {
                    var v1 = self.AskMenu("Please select the action of your choice.", "Select Event Map", "Check number of users in Event Map", "Go event");
                    int uMap = self.GetIntReg("map");
                    if (v1 == 0)
                    {
                        var v2 = self.AskMenu("Please select an event to open.", eventMenuWithClose);
                        if (v2 < Events.Count)
                        {
                            var selEvent = Events[v2];
                            self.SetIntReg("map", selEvent.Map.ID);
                            self.SetIntReg("count", selEvent.Capacity);
                            field.Notice(0, "The event is now open. Please click the Event NPC to enter the Event Map.");
                        }
                        else
                        {
                            self.SetIntReg("map", -1);
                            self.SetIntReg("count", 0);
                        }
                    }
                    else if (v1 == 1)
                    {
                        int v2 = self.AskMenu("Please select an event.", eventMenu);
                        var selEvent = Events[v2];
                        var selMap = selEvent.Map;
                        if (uMap == selMap.ID) self.Say($"A total of {selEvent.Capacity} can enter {selEvent.Name} ({selMap.ID}). #r" + selMap.Characters.Count + "#k users have currently entered the event map.");
                        else self.Say($"{selEvent.Name} ({selMap.ID}) This event is currently not open.");
                    }
                    else if (v1 == 2)
                    {
                        if (uMap == -1) self.Say("There is currently no event running.");
                        else target.ChangeMap(uMap);
                    }
                }
                else
                { // if user click 
                    var qr = target.QuestRecord;
                    var val = qr.Get(9000);
                    var val2 = qr.Get(9001);
                    var proof = check_kawi(target);
                    int v1 = 0;
                    string strMap = null;
                    string preMapNum = null;

                    string[] menu = new string[]
                    {
                        "#k#e1. #n#bWhat kind of event is this?",
                        "#k#e2. #n#bPlease explain the game event to me.",
                        "#k#e3. #n#bAlright, let's go!"
                    };

                    if (cmap == 60000)
                    {
                        self.Say("Hey, I'm #b#p9000000##k. If you're not busy right now... can I stay with you? I hear people are gathering around here for an #revent#k, but I don't want to go alone... Well, do you want to come with me and have a look?");
                        v1 = self.AskMenu("Huh? Which type of event? Well, it's...", menu);
                    }
                    else if (cmap == 104000000)
                    {
                        self.Say("Hey, I'm #b#p9000001##k. I'm here waiting for my brother #bPaul#k. He should've been here now...");
                        self.Say("Hmm... What should I do? The event will start soon... Many people came to participate in the event, so we better hurry...");
                        v1 = self.AskMenu("Hey... Why don't you go with me? I think my brother will come with other people.", menu);
                    }
                    else if (cmap == 200000000)
                    {
                        self.Say("Hey, I'm #b#p9000011##k. I'm here waiting for my brothers... What's taking so long? I'm upset now... If we don't get there on time, we might not be able to attend the event...");
                        self.Say("Hmm... What should I do? The event will start soon... Many people came to participate in the event, so we better hurry...");
                        v1 = self.AskMenu("Hey... why don't you come with me then?", menu);
                    }
                    else if (cmap == 220000000)
                    {
                        self.Say("Hey, I'm #b#p9000013##k. For now, I'm waiting for my brothers, but they haven't arrived yet. I'm sick and tired of doing things on my own. At least during the event, I don't feel so lonely with so many people around and everything. All events take place with a limited number of people, so if I don't get there soon, I won't be able to participate.");
                        self.Say("We may be cousins and all, but we can't stop missing each other. Dude, what should I do? The event should start any minute... A lot of people must be there waiting and there must be no room for them...");
                        v1 = self.AskMenu("What do you think? Do you want to join me and go to the event?", menu);
                    }

                    if (v1 == 0)
                    {
                        self.Say("MapleStory Global is celebrating! GMs will hold surprise GM Events during this time. So stay alert and make sure you participate in at least one of the events to get some nice prizes!");
                    }
                    else if (v1 == 1)
                    {
                        int v2 = self.AskMenu("There will be many games for this event. Here are some instructions for how to play the game before it starts. Choose the event you want to know more about!",
                            "Ola Ola",
                            "MapleStory Physical Fitness Test",
                            "Snowball",
                            "Coconut Harvest",
                            "OX Quiz",
                            "Find the Jewel"
                        );
                        if (v2 == 0) self.Say("#b[Ola Ola]#k is a game in which participants climb stairs to reach the top. Climb up and move to the next level by choosing the correct portal from the numerous available portals.\r\n\r\nThe game consists of three levels, and the time limit is #b6 MINUTES#k. During [Ola Ola], you #bwill not be able to jump, teleport, run or increase your speed with potions or items#k.\nThere are also cheat portals, which will guide you to a strange place, so please be careful with them.");
                        else if (v2 == 1) self.Say("#b[MapleStory Physical Fitness Test] is an obstacle course#k similar to The Forest of Patience. You win if you overcome obstacles and reach the finish point within the time limit.\r\n\r\nThe game has four levels, and the time limit is #b15 MINUTES#k. During [MapleStory Physical Fitness Test], you cannot teleport or run.");
                        else if (v2 == 2) self.Say("#b[Snowball]#k consists of two teams, Team Maple and Team Story, who compete to see #bwho pushed the biggest snowball the farthest within the time limit#k. If the game is not decided within the time limit, the team that pushed the snowball the furthest wins.\r\n\r\nTo push the ball, press Ctrl. Ranged attacks and skill-based attack don't work here; #bonly close range attacks#k.\r\n\r\nIf a character touches the ball, they must return to the starting point. Attack the Snowman in front of the starting point to prevent the opposing team from advancing with the snowball. This is where a well-planned strategy works, when the team will decide whether to go after the Snowball or the Snowman.");
                        else if (v2 == 3) self.Say("#b[Coconut Harvest]#k consists of two teams, Team Maple and Team Story. The two teams compete to see #bwhich one catches the most coconuts#k. The time limit is #b5 MINUTES#k. If the game ends in a tie, a 2-minute overtime will be given to determine the winner team. If for some reason the score is still tied, the game will end in a draw.\r\n\r\nAll ranged attacks and skill-based attacks will not work here; #bonly melee attacks will work#k. If you don't have a melee weapon, you can buy it from an NPC inside the event map. No matter the level of the character, weapon or skill, all damage will be the same.\r\n\r\nWatch out for obstacles and traps present in the map. If a character dies during the game, they will be eliminated. The player who attacks last before the coconut falls will win. Only coconuts that hit the ground will be counted. However, those that fall from the coconut tree or that explode occasionally WILL NOT BE COUNTED. There is also a portal hidden in one of the shells at the bottom of the map, use it wisely!");
                        else if (v2 == 4) self.Say("#b[OX Quiz]#k is a MapleStory game of Xs and Os. When you enter the game, activate the minimap by pressing M to see where the Xs and Os are. #r10 questions#k will be asked, and the player who answers all of them correctly will be the winner.\r\n\r\nAfter the question is asked, use the ladder to enter the area where the right answer may be, either X or O. If the character does not choose an answer or stays on the stairs longer than the allowed time, they are eliminated. Hold your position until [CORRECT] appears on the screen. To prevent any kind of cheating, all types of conversations will be turned off during the OX quiz.");
                        else self.Say("#b[Find the Jewel]#k is a game in which your goal is to find, in 10 minutes, the #btreasure scrolls#k that are hidden all over the map #r #k. There will be a number of mysterious hidden treasure chests, and once you break them, many items will drop from the chest. Your task is to separate the treasure scroll from these items.\n\r\nTreasure chests can be destroyed using #regular attacks#k, and having the treasure scroll with you, you can exchange it for the Scroll of Secrets through an NPC who is in charge of exchanging items. The NPC that exchanges items can be found on the Find the Jewel map, but you can also exchange your scroll through #b[Vikin]#k of Lith Harbor.\r\n\r\nThis game has some hidden portals and teleportation points. To use them, press #bup arrow#k at a certain point and you will be teleported to a different place. Try to jump randomly so you can also reach hidden ladders or ropes. There will also be a treasure chest that will take you to a hidden location, and a secret chest that can only be found through the secret portal. So, look around.\r\n\r\nDuring the Find the Jewel game, all attack skills will be #rdisabled#k, so please break the chest with a regular weapon.");
                    }
                    else if (v1 == 2)
                    {
                        var inventory = target.Inventory;
                        // Test sever 
                        if (Server.Tespia)
                        {
                            var map = self.GetIntReg("map");
                            var count = self.IncIntReg("count", -1);

                            if (map >= 0)
                            {
                                strMap = map.ToString();
                                preMapNum = strMap.Substring(0, 3);
                            }
                            else preMapNum = "";

                            if (inventory.ItemCount(4031019) < 1 && count >= 0 && preMapNum == "109")
                            {
                                var ret = inventory.Exchange(0, 4000038, 1);
                                if (ret != 0)
                                {
                                    if (cmap == 60000) qr.Set(9000, "maple");
                                    else if (cmap == 104000000) qr.Set(9000, "victoria");
                                    else if (cmap == 200000000) qr.Set(9000, "ossyria");
                                    else if (cmap == 220000000) qr.Set(9000, "ludi");
                                    target.ChangeMap(map, "");
                                }
                                else
                                {
                                    self.IncIntReg("count", 1);
                                    self.Say("Do you have an empty slot in your etc. inventory? Please check again!");
                                }
                            }
                            else
                            {
                                self.IncIntReg("count", 1);
                                self.Say("Either the event hasn't started yet, or you already have #t4031019#, or you've already participated in this event within the last 24 hours. Please try again later!");
                            }
                        }
                        // Real GL sever 
                        else
                        {
                            int map = self.GetIntReg("map");
                            int count = self.IncIntReg("count", -1);
                            int goEvent;

                            if (map >= 0)
                            {
                                strMap = map.ToString();
                                preMapNum = strMap.Substring(0, 3);
                            }
                            else preMapNum = "";

                            var cTime = MasterThread.CurrentDateStr;
                            if (val2 == "") goEvent = 1;
                            else
                            {
                                var aTime = MasterThread.CompareTime(cTime, val2);
                                if (aTime >= 1440) goEvent = 1;
                                else goEvent = 0;
                            }

                            if (goEvent == 1 && inventory.ItemCount(4031019) < 1 && count >= 0 && preMapNum == "109")
                            {
                                var ret = inventory.Exchange(0, 4000038, 1);
                                if (ret != 0)
                                {
                                    if (cmap == 60000) qr.Set(9000, "maple");
                                    else if (cmap == 104000000) qr.Set(9000, "victoria");
                                    else if (cmap == 200000000) qr.Set(9000, "ossyria");
                                    else if (cmap == 220000000) qr.Set(9000, "ludi");
                                    qr.Set(9001, cTime);
                                    target.ChangeMap(map, "");
                                }
                                else
                                {
                                    self.IncIntReg("count", 1);
                                    self.Say("Do you have an empty slot in your etc. inventory? Please check again!");
                                }
                            }
                            else
                            {
                                self.IncIntReg("count", 1);
                                self.Say("Either the event hasn't started yet, or you already have #t4031019#, or you've already participated in this event within the last 24 hours. Please try again later!");
                            }
                        }
                    }
                    else if (v1 == 3)
                    {
                        // not ready...
                    }
                }
            }
        }
        public static void ola_answer3(GameCharacter target, int num)
        {
            var @event = FieldSet.Instances["Event1"];
            var answer3 = @event.GetVar("ola_ans3");

            if (answer3.Substring(num, 1) == "0" || answer3.Substring(num, 1) == "1")
            {
                target.PlayPortalSE();
                target.ChangeMap(109050000, "start00");
            }
            else if (answer3.Substring(num, 1) == "2" || answer3.Substring(num, 1) == "3" || answer3.Substring(num, 1) == "4" || answer3.Substring(num, 1) == "5" || answer3.Substring(num, 1) == "6")
            {
                target.PlayPortalSE();
                target.ChangeMap(-1, "np03");
            }
            else if (answer3.Substring(num, 1) == "7")
            {
                target.PlayPortalSE();
                target.ChangeMap(-1, "np04");
            }
            else if (answer3.Substring(num, 1) == "8")
            {
                target.PlayPortalSE();
                target.ChangeMap(-1, "np05");
            }
            else if (answer3.Substring(num, 1) == "9")
            {
                target.PlayPortalSE();
                target.ChangeMap(-1, "np06");
            }
            else if (answer3.Substring(num, 1) == "a" || answer3.Substring(num, 1) == "b" || answer3.Substring(num, 1) == "c" || answer3.Substring(num, 1) == "d" || answer3.Substring(num, 1) == "e" || answer3.Substring(num, 1) == "f")
            {
            }
            return;
        }
        public static void ola_answer2(GameCharacter target, int num)
        {
            var @event = FieldSet.Instances["Event1"];
            var answer2 = @event.GetVar("ola_ans2");

            if (answer2.Substring(num, 1) == "0" || answer2.Substring(num, 1) == "1" || answer2.Substring(num, 1) == "2")
            {
                target.PlayPortalSE();
                target.ChangeMap(109030003, "start00");
            }
            else if (answer2.Substring(num, 1) == "3" || answer2.Substring(num, 1) == "4")
            {
                target.PlayPortalSE();
                target.ChangeMap(-1, "np01");
            }
            else if (answer2.Substring(num, 1) == "5")
            {
                target.PlayPortalSE();
                target.ChangeMap(-1, "np02");
            }
            else if (answer2.Substring(num, 1) == "6" || answer2.Substring(num, 1) == "7")
            {
            }
            return;
        }
        public static void ola_answer1(GameCharacter target, int num)
        {
            var @event = FieldSet.Instances["Event1"];
            var answer1 = @event.GetVar("ola_ans1");

            if (answer1.Substring(num, 1) == "0" || answer1.Substring(num, 1) == "1")
            {
                target.PlayPortalSE();
                target.ChangeMap(109030002, "start00");
            }
            else if (answer1.Substring(num, 1) == "2" || answer1.Substring(num, 1) == "3")
            {
                target.PlayPortalSE();
                target.ChangeMap(-1, "np00");
            }
            else if (answer1.Substring(num, 1) == "4")
            {
            }
            return;
        }
        //¿Ã¶ó¿Ã¶ó ·£ŽýÈ­ _ Æ÷Å» 
        [Script("rand_ola")]
        class rand_ola : IPortalScript
        {
            public void Run(IPortalHost self, GameCharacter target)
            {
                var field = self.Field;
                var @event = field.FieldSet;
                /* 
                    if ( @event.GetVar( "decide_num" ) != "1" ) { 
                        if ( @event.GetVar( "decide_ans" ) != "1" ) { 
                            var answer1 = "14302"; 
                            var answer2 = "74302561"; 
                            var answer3 = "f49e60a2d7c8b351"; 

                            @event.SetVar( "ola_ans1", answer1 ); 
                            @event.SetVar( "ola_ans2", answer2 ); 
                            @event.SetVar( "ola_ans3", answer3 );         
                        } 
                        @event.SetVar( "decide_num", "1" ); 
                    } 
                */
                if (field.ID == 109030001)
                {
                    if (self.GetPortalID == 19)
                    {
                        ola_answer1(target, 0);
                    }
                    else if (self.GetPortalID == 20)
                    {
                        ola_answer1(target, 1);
                    }
                    else if (self.GetPortalID == 21)
                    {
                        ola_answer1(target, 2);
                    }
                    else if (self.GetPortalID == 22)
                    {
                        ola_answer1(target, 3);
                    }
                    else if (self.GetPortalID == 23)
                    {
                        ola_answer1(target, 4);
                    }
                    return;
                }
                else if (field.ID == 109030002)
                {
                    if (self.GetPortalID == 9)
                    {
                        ola_answer2(target, 0);
                    }
                    else if (self.GetPortalID == 10)
                    {
                        ola_answer2(target, 1);
                    }
                    else if (self.GetPortalID == 11)
                    {
                        ola_answer2(target, 2);
                    }
                    else if (self.GetPortalID == 12)
                    {
                        ola_answer2(target, 3);
                    }
                    else if (self.GetPortalID == 13)
                    {
                        ola_answer2(target, 4);
                    }
                    else if (self.GetPortalID == 14)
                    {
                        ola_answer2(target, 5);
                    }
                    else if (self.GetPortalID == 15)
                    {
                        ola_answer2(target, 6);
                    }
                    else if (self.GetPortalID == 16)
                    {
                        ola_answer2(target, 7);
                    }
                    return;
                }
                else if (field.ID == 109030003)
                {
                    if (self.GetPortalID == 11)
                    {
                        ola_answer3(target, 0);
                    }
                    else if (self.GetPortalID == 12)
                    {
                        ola_answer3(target, 1);
                    }
                    else if (self.GetPortalID == 13)
                    {
                        ola_answer3(target, 2);
                    }
                    else if (self.GetPortalID == 14)
                    {
                        ola_answer3(target, 3);
                    }
                    else if (self.GetPortalID == 15)
                    {
                        ola_answer3(target, 4);
                    }
                    else if (self.GetPortalID == 16)
                    {
                        ola_answer3(target, 5);
                    }
                    else if (self.GetPortalID == 17)
                    {
                        ola_answer3(target, 6);
                    }
                    else if (self.GetPortalID == 18)
                    {
                        ola_answer3(target, 7);
                    }
                    else if (self.GetPortalID == 19)
                    {
                        ola_answer3(target, 8);
                    }
                    else if (self.GetPortalID == 20)
                    {
                        ola_answer3(target, 9);
                    }
                    else if (self.GetPortalID == 21)
                    {
                        ola_answer3(target, 10);
                    }
                    else if (self.GetPortalID == 22)
                    {
                        ola_answer3(target, 11);
                    }
                    else if (self.GetPortalID == 23)
                    {
                        ola_answer3(target, 12);
                    }
                    else if (self.GetPortalID == 24)
                    {
                        ola_answer3(target, 13);
                    }
                    else if (self.GetPortalID == 25)
                    {
                        ola_answer3(target, 14);
                    }
                    else if (self.GetPortalID == 26)
                    {
                        ola_answer3(target, 15);
                    }
                    return;
                }
            }
        }
        // Pietro : 9000002 
        [Script("Event02")]
        class Event02 : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                var qr = target.QuestRecord;
                var val = qr.Get(9000);
                var inventory = target.Inventory;

                if (val == "maple" || val == "victoria" || val == "ossyria" || val == "ludi")
                {
                    if (inventory.ItemCount(4031019) < 1)
                    {
                        self.Say("Dun dun dun dun!!! You won the #bEVENT#k game. Congratulations on getting this far!");
                        self.Say("As a winner, you will be rewarded with #b#t4031019##k. On the scroll is secret information, written in ancient characters.");
                        self.Say("The Scroll of Secrets can be deciphered by #r#p9000007##k or #rGeanie#k in Ludibrium. Take it with you and something good will come of it.");
                        var ret = inventory.ExchangeEx(0, "4031019,Period:43200", 1);
                        if (ret == 0) self.Say("I think your inventory is full. Please make some room and then talk to me again.");
                        else
                        {
                            self.Log(target.SCharacterName + " / " + val);
                            if (val == "maple")
                            {
                                qr.Remove(9000);
                                target.ChangeMap(60000, "");
                            }
                            else if (val == "victoria")
                            {
                                qr.Remove(9000);
                                target.ChangeMap(104000000, "");
                            }
                            else if (val == "ossyria")
                            {
                                qr.Remove(9000);
                                target.ChangeMap(200000000, "");
                            }
                            else
                            {
                                qr.Remove(9000);
                                target.ChangeMap(220000000, "");
                            }
                        }
                    }
                    else
                    {
                        self.Say("Seems like you already have #r#t4031019##k. This scroll is filled with incredible magical power, so powerful that you should always carry it with you. Go and take this scroll to #r#p9000007##k immediately.");
                        if (val == "maple")
                        {
                            qr.Remove(9000);
                            target.ChangeMap(60000, "");
                        }
                        else if (val == "victoria")
                        {
                            qr.Remove(9000);
                            target.ChangeMap(104000000, "");
                        }
                        else if (val == "ossyria")
                        {
                            qr.Remove(9000);
                            target.ChangeMap(200000000, "");
                        }
                        else
                        {
                            qr.Remove(9000);
                            target.ChangeMap(220000000, "");
                        }
                    }
                }
                else self.Say("I don't think you've found #p9000001# or #p9000000#. HOW did you get here in the first place??? You... ?!?!");
            }
        }
        // Pietra : 9000010 
        [Script("Event06")]
        class Event06 : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                var qr = target.QuestRecord;
                var val = qr.Get(9000);
                var inventory = target.Inventory;

                if (inventory.ItemCount(4031018) >= 1)
                {
                    var v = self.AskMenu("So you've got the #b#t4031018##k. Instead of talking to me, you might want to talk to #p9000006# about exchanging the #t4031018# for a prize.",
                        "Who is #p9000006#?",
                        "Please take me back to where I was before."
                    );

                    if (v == 0) self.Say("#b#p9000006##k is the person who will send you to the map where you can exchange your #t4031018# for a prize. He's to the left of where I am now, finding him will be easy.");
                    else if (v == 1)
                    {
                        var nRet = self.AskYesNo("I recommend trading your #b#t4031018##k for a prize before returning. You can still trade at Lith Harbor, but if you're too busy, you can leave right now. Would you like to return home right now?");
                        if (nRet != 0)
                        {
                            if (val == "maple")
                            {
                                qr.Remove(9000);
                                target.ChangeMap(60000, "");
                            }
                            else if (val == "victoria")
                            {
                                qr.Remove(9000);
                                target.ChangeMap(104000000, "");
                            }
                            else if (val == "ossyria")
                            {
                                qr.Remove(9000);
                                target.ChangeMap(200000000, "");
                            }
                            else
                            {
                                qr.Remove(9000);
                                target.ChangeMap(220000000, "");
                            }
                        }
                    }
                }
                else
                {
                    self.Say("I'm sorry, unfortunately you didn't win the event. Try again another time. You can return to where you were through me.");

                    if (val == "maple")
                    {
                        qr.Remove(9000);
                        target.ChangeMap(60000, "");
                    }
                    else if (val == "victoria")
                    {
                        qr.Remove(9000);
                        target.ChangeMap(104000000, "");
                    }
                    else if (val == "ossyria")
                    {
                        qr.Remove(9000);
                        target.ChangeMap(200000000, "");
                    }
                    else
                    {
                        qr.Remove(9000);
                        target.ChangeMap(220000000, "");
                    }
                }
            }
        }
        public static void Viking(INpcHost self, GameCharacter target)
        {
            var inventory = target.Inventory;

            if (inventory.ItemCount(4031018) >= 1)
            {
                if (inventory.ItemCount(4031019) < 1)
                {
                    self.Say("Wow, you are incredible. Do you want to sail with us? What? You are busy? Hm... so you can't. Instead, how about I take you somewhere interesting. I will make the map available for you to use at will.");
                    var ret = inventory.Exchange(0, 4031018, -1);
                    if (ret == 0) self.Say("Oh no... Are you sure you have a #t4031018#? Please check again.");
                    else target.ChangeMap(109050000, "");
                }
                else self.Say("You already have #r#t4031019##k. This scroll is filled with incredible magical power, so powerful that you should always carry it with you. Go and take this scroll to #r#p9000007##k immediately.");
            }
            else self.Say("Hey, Hey!!! Find #t4031018#! I lost the map somewhere and I can't leave without it.");
        }
        // Vikin of Lith Harbor : 9000009 
        [Script("Event03_1")]
        class Event03_1 : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                Viking(self, target);
            }
        }
        // Vikan : 9000003, Vikon : 9000004, Vikone : 9000005, Vikoon : 9000006 
        [Script("Event03")]
        class Event03 : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                Viking(self, target);
            }
        }
        // Chun Ji : 9000007 
        [Script("Event04")]
        class Event04 : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                var inventory = target.Inventory;

                if (inventory.ItemCount(4031019) >= 1)
                {
                    self.Say("Having something so rare and precious isn't bad for a nobody like you. What? Do you want me to decipher the scroll for you? No. Even Chief Wizards cannot easily deal with the scroll that is full of secret powers from ancient times.");
                    self.Say("But, hey... would you show the scroll to me? If I can decipher it safely, it would help me tremendously in my mission to end the evil powers that roam the world.");
                    self.Say("To decrypt it safely, I will need #b50 #t4000008##k. Bring me the charms and the scroll, then I'll give you the father of all the items I've gathered, from defeating the forces of evil all these years.");
                    if (inventory.ItemCount(4000008) >= 50)
                    {
                        self.Say("All right, I'll give you the precious item I promised you. It is called #r#t4031017##k, and I obtained it by defeating one of the worst monsters of ancient times. It is NOT an easy item to get.");
                        self.Say("A hard-to-find item is inside the box. Unfortunately, I've lost the key, so I can't open it for you. You might be able to open it at #bKerning City#k where there is an amazing #rlockpicker#k that might get it for you..");
                        var ret = inventory.ExchangeEx(0, "4031019", -1, "4000008", -50, "4031017,Period:21600", 1);
                        if (ret == 0) self.Say("Looks like you don't have enough space in your inventory. Free up some space and then talk to me again.");
                    }
                }
                else self.Say("A nobody... Leave me alone...");
            }
        }
        // Mr.Pickall : 9000008 
        [Script("Event05")]
        class Event05 : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                self.Say("Welcome. hah! I pick everything in the world that can be picked! Hah! If you don't have a key to open something that's locked, bring it to me. Ha!");
                var inventory = target.Inventory;
                int nNewItemNum = 0;
                int rn2;

                if (inventory.ItemCount(4031017) >= 1)
                {
                    self.Say("Oh, how incredible. Hah! How did you get something as rare as this? huh? Look, but this is locked so tightly, I might need a lot of materials to open it. Hah!");
                    var v1 = self.AskMenu("I have everything else but 1 #t4021005# and 5 #t4000010#s... Get them for me and I'll open them for you for free. Hah!",
                        "Get him the materials.",
                        "Pay it off."
                    );
                    if (inventory.SlotCount(1) > inventory.HoldCount(1) && inventory.SlotCount(2) > inventory.HoldCount(2) && inventory.SlotCount(4) > inventory.HoldCount(4))
                    {
                        var nNewItemSort = 0;
                        var nNewItemID = 0;
                        var rn1 = Rand32.NextBetween(1, 100);

                        if (rn1 < 6) nNewItemSort = 1;
                        else if (rn1 > 5 && rn1 < 11) nNewItemSort = 2;
                        else if (rn1 > 10 && rn1 < 16) nNewItemSort = 3;
                        else if (rn1 > 15 && rn1 < 21) nNewItemSort = 4;
                        else if (rn1 > 20 && rn1 < 26) nNewItemSort = 5;
                        else if (rn1 > 25 && rn1 < 31) nNewItemSort = 6;
                        else if (rn1 > 30 && rn1 < 36) nNewItemSort = 7;
                        else if (rn1 > 35 && rn1 < 40) nNewItemSort = 8;
                        else if (rn1 > 40 && rn1 < 71) nNewItemSort = 9;
                        else if (rn1 > 70 && rn1 < 101) nNewItemSort = 10;

                        if (nNewItemSort == 1)
                        {
                            nNewItemNum = 1;
                            rn2 = Rand32.NextBetween(1, 13);
                            if (rn2 == 1) nNewItemID = 1002086;
                            else if (rn2 == 2) nNewItemID = 1002218;
                            else if (rn2 == 3) nNewItemID = 1002214;
                            else if (rn2 == 4) nNewItemID = 1002210;
                            else if (rn2 == 5) nNewItemID = 1032013;
                            else if (rn2 == 6) nNewItemID = 1072135;
                            else if (rn2 == 7) nNewItemID = 1072143;
                            else if (rn2 == 8) nNewItemID = 1072125;
                            else if (rn2 == 9) nNewItemID = 1072130;
                            else if (rn2 == 10) nNewItemID = 1082009;
                            else if (rn2 == 11) nNewItemID = 1082081;
                            else if (rn2 == 12) nNewItemID = 1082084;
                            else if (rn2 == 13) nNewItemID = 1082065;
                        }
                        else if (nNewItemSort == 2)
                        {
                            nNewItemNum = 1;
                            rn2 = Rand32.NextBetween(1, 18);
                            if (rn2 == 1) nNewItemID = 1032015;
                            else if (rn2 == 2) nNewItemID = 1092009;
                            else if (rn2 == 3) nNewItemID = 1302011;
                            else if (rn2 == 4) nNewItemID = 1312009;
                            else if (rn2 == 5) nNewItemID = 1322018;
                            else if (rn2 == 6) nNewItemID = 1332015;
                            else if (rn2 == 7) nNewItemID = 1332017;
                            else if (rn2 == 8) nNewItemID = 1372007;
                            else if (rn2 == 9) nNewItemID = 1382006;
                            else if (rn2 == 10) nNewItemID = 1402011;
                            else if (rn2 == 11) nNewItemID = 1412007;
                            else if (rn2 == 12) nNewItemID = 1422009;
                            else if (rn2 == 13) nNewItemID = 1432006;
                            else if (rn2 == 14) nNewItemID = 1442010;
                            else if (rn2 == 15) nNewItemID = 1452004;
                            else if (rn2 == 16) nNewItemID = 1462008;
                            else if (rn2 == 17) nNewItemID = 1472022;
                            else if (rn2 == 18) nNewItemID = 2070005;
                        }
                        else if (nNewItemSort == 3)
                        {
                            rn2 = Rand32.NextBetween(1, 4);
                            if (rn2 >= 1 && rn2 <= 3)
                            {
                                nNewItemNum = 5;
                                nNewItemID = 4003000;
                            }
                            else if (rn2 == 4)
                            {
                                nNewItemNum = 1;
                                nNewItemID = 2100000;
                            }
                        }
                        else if (nNewItemSort == 4)
                        {
                            nNewItemNum = 1;
                            rn2 = Rand32.NextBetween(1, 52);
                            if (rn2 == 1) nNewItemID = 2040704;
                            else if (rn2 == 2) nNewItemID = 2040501;
                            else if (rn2 == 3) nNewItemID = 2040401;
                            else if (rn2 == 4) nNewItemID = 2040601;
                            else if (rn2 == 5) nNewItemID = 2040705;
                            else if (rn2 == 6) nNewItemID = 2040502;
                            else if (rn2 == 7) nNewItemID = 2040402;
                            else if (rn2 == 8) nNewItemID = 2040602;
                            else if (rn2 == 9) nNewItemID = 2040301;
                            else if (rn2 == 10) nNewItemID = 2040302;
                            else if (rn2 == 11) nNewItemID = 2040707;
                            else if (rn2 == 12) nNewItemID = 2040708;
                            else if (rn2 == 13) nNewItemID = 2040804;
                            else if (rn2 == 14) nNewItemID = 2040805;
                            else if (rn2 == 15) nNewItemID = 2040901;
                            else if (rn2 == 16) nNewItemID = 2040902;
                            else if (rn2 == 17) nNewItemID = 2041001;
                            else if (rn2 == 18) nNewItemID = 2041002;
                            else if (rn2 == 19) nNewItemID = 2041004;
                            else if (rn2 == 20) nNewItemID = 2041005;
                            else if (rn2 == 21) nNewItemID = 2041007;
                            else if (rn2 == 22) nNewItemID = 2041008;
                            else if (rn2 == 23) nNewItemID = 2041010;
                            else if (rn2 == 24) nNewItemID = 2041011;
                            else if (rn2 == 25) nNewItemID = 2043001;
                            else if (rn2 == 26) nNewItemID = 2043002;
                            else if (rn2 == 27) nNewItemID = 2043101;
                            else if (rn2 == 28) nNewItemID = 2043102;
                            else if (rn2 == 29) nNewItemID = 2043201;
                            else if (rn2 == 30) nNewItemID = 2043202;
                            else if (rn2 == 31) nNewItemID = 2043301;
                            else if (rn2 == 32) nNewItemID = 2043302;
                            else if (rn2 == 33) nNewItemID = 2043701;
                            else if (rn2 == 34) nNewItemID = 2043702;
                            else if (rn2 == 35) nNewItemID = 2043801;
                            else if (rn2 == 36) nNewItemID = 2043802;
                            else if (rn2 == 37) nNewItemID = 2044001;
                            else if (rn2 == 38) nNewItemID = 2044002;
                            else if (rn2 == 39) nNewItemID = 2044101;
                            else if (rn2 == 40) nNewItemID = 2044102;
                            else if (rn2 == 41) nNewItemID = 2044201;
                            else if (rn2 == 42) nNewItemID = 2044202;
                            else if (rn2 == 43) nNewItemID = 2044301;
                            else if (rn2 == 44) nNewItemID = 2044302;
                            else if (rn2 == 45) nNewItemID = 2044401;
                            else if (rn2 == 46) nNewItemID = 2044402;
                            else if (rn2 == 47) nNewItemID = 2044501;
                            else if (rn2 == 48) nNewItemID = 2044502;
                            else if (rn2 == 49) nNewItemID = 2044601;
                            else if (rn2 == 50) nNewItemID = 2044602;
                            else if (rn2 == 51) nNewItemID = 2044701;
                            else if (rn2 == 52) nNewItemID = 2044702;
                        }
                        else if (nNewItemSort == 5)
                        {
                            nNewItemNum = 10;
                            rn2 = Rand32.NextBetween(1, 3);
                            if (rn2 == 1) nNewItemID = 4010006;
                            else if (rn2 == 2) nNewItemID = 4020007;
                            else if (rn2 == 3) nNewItemID = 4020008;
                        }
                        else if (nNewItemSort == 6)
                        {
                            nNewItemNum = 4;
                            rn2 = Rand32.NextBetween(1, 3);
                            if (rn2 == 1) nNewItemID = 4004000;
                            else if (rn2 == 2) nNewItemID = 4004001;
                            else if (rn2 == 3) nNewItemID = 4004002;
                            else if (rn2 == 4) nNewItemID = 4004003;
                        }
                        else if (nNewItemSort == 7)
                        {
                            rn2 = Rand32.NextBetween(1, 4);
                            if (rn2 == 1)
                            {
                                nNewItemNum = 30;
                                nNewItemID = 2000004;
                            }
                            else if (rn2 >= 2 && rn2 <= 4)
                            {
                                nNewItemNum = 100;
                                nNewItemID = 2022000;
                            }
                        }
                        else if (nNewItemSort == 8)
                        {
                            nNewItemNum = 50;
                            rn2 = Rand32.NextBetween(1, 4);
                            if (rn2 == 1) nNewItemID = 2020012;
                            else if (rn2 == 2) nNewItemID = 2020013;
                            else if (rn2 == 3) nNewItemID = 2020014;
                            else if (rn2 == 4) nNewItemID = 2020015;
                        }
                        else if (nNewItemSort == 9)
                        {
                            nNewItemNum = 15;
                            rn2 = Rand32.NextBetween(1, 13);
                            if (rn2 == 1) nNewItemID = 4010000;
                            else if (rn2 == 2) nNewItemID = 4010001;
                            else if (rn2 == 3) nNewItemID = 4010002;
                            else if (rn2 == 4) nNewItemID = 4010003;
                            else if (rn2 == 5) nNewItemID = 4010004;
                            else if (rn2 == 6) nNewItemID = 4010005;
                            else if (rn2 == 7) nNewItemID = 4020000;
                            else if (rn2 == 8) nNewItemID = 4020001;
                            else if (rn2 == 9) nNewItemID = 4020002;
                            else if (rn2 == 10) nNewItemID = 4020003;
                            else if (rn2 == 11) nNewItemID = 4020004;
                            else if (rn2 == 12) nNewItemID = 4020005;
                            else if (rn2 == 13) nNewItemID = 4020006;
                        }
                        else if (nNewItemSort == 10)
                        {
                            nNewItemNum = 100;
                            rn2 = Rand32.NextBetween(1, 3);
                            if (rn2 == 1) nNewItemID = 2001000;
                            else if (rn2 == 2) nNewItemID = 2001002;
                            else if (rn2 == 3) nNewItemID = 2001001;
                        }

                        if (v1 == 0)
                        {
                            if (inventory.ItemCount(4021005) >= 1 && inventory.ItemCount(4000010) >= 5)
                            {
                                var ret = inventory.Exchange(0, 4031017, -1, 4021005, -1, 4000010, -5, nNewItemID, nNewItemNum);
                                if (ret == 0) self.Say("Are you 100% certain you have 1 #t4021005# and 5 #t4000010#s? Hah? You might have to check again. Hah!");
                                else self.Say("I unlocked it for free! hah! See you around. hah!");
                            }
                            else self.Say("Now get me #b1 #t4021005##k and #b5 #t4000010#s#k. Hah! I unlocked it for free! Hah!");
                        }
                        else if (v1 == 1)
                        {
                            var nRet = self.AskYesNo("I need to use expensive materials for this, so it will cost you dearly. Hah! #b10000 mesos#k! Will you still do it? Hah??");
                            if (nRet == 0) self.Say("10,000 mesos is very efficient. Hah! There is a way to put them together, so, please come back again. Hah!");
                            else if (nRet == 1)
                            {
                                int ret = inventory.Exchange(-10000, 4031017, -1, nNewItemID, nNewItemNum);
                                if (ret == 0) self.Say("You don't have enough mesos. Hah! #b10000 mesos#k. Hah!");
                                else self.Say("I got the money, and I unlocked it for you. I'll see you later. Hah!");
                            }
                        }
                    }
                    else self.Say("You need at least one free space in your etc. and equipment inventory. Hah! Free up some space and then talk to me again, hah!");
                }
            }
        }
        // Mobile Maple Story : 9010001, 9010002, 9010003 
        //[Script("Event07")]
        //class Event07 : INpcScript
        //{
        //    public void Run(INpcHost self, GameCharacter target)
        //    {
        //        @event.self = self;
        //        @event.target = target;
        //        self.AskMenu("You can play MapleStory (-Wizard) on your mobile! And if you download MapleStory (-Wizard), you'll get 6 free cash items! Where can download (-Wizet). hehe",
        //            "SKT(011, 017, 010) User",
        //            "KTF(016, 018, 010) User"
        //        );
        //        self.Say("Purchase Items");
        //    }
        //}
        // Harry : 9000012 
        [Script("Event09")]
        class Event09 : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                int option = self.AskMenu("Man... It's hot!!! How can I help you?",
                    "Exit game event",
                    "Buy the weapon.(#t1322005# 1 meso)"
                );
                if (option == 0)
                {
                    var nRet = self.AskYesNo("If you leave now, you will not be able to participate in this event for the next 24 hours. Do you really want to go out?");
                    if (nRet == 0) self.Say("Good. Don't give up and try your best. If you try really hard, you'll get a reward!");
                    else target.ChangeMap(109050001, "");
                }
                else if (option == 1)
                {
                    int nRet = self.AskYesNo("#t1322005# for beginners is 1 meso. What do you think? You want it?");
                    if (nRet == 0) self.Say("Weapons with attack speed are more important than high-damage weapons. If you ever need one, please come back.");
                    else
                    {
                        var inventory = target.Inventory;
                        var ret = inventory.Exchange(-1, 1322005, 1);
                        if (ret == 0) self.Say("Are you sure you have an empty slot? Or do you not have 1 meso? Please check again.");
                        else self.Say("Did you get #t1322005#? I wish you good luck!");
                    }
                }
            }
        }
        [Script("event_master")]
        class EventMaster : IFieldSetScript
        {
            public void Run(IFieldSetScriptHost host, GameCharacter target)
            {
                var @event = host.FieldSet;
                var answer1 = Tools.Shuffle(1, "01234");
                var answer2 = Tools.Shuffle(1, "01234567");
                var answer3 = Tools.Shuffle(1, "0123456789abcdef");
                @event.SetVar("ola_ans1", answer1);
                @event.SetVar("ola_ans2", answer2);
                @event.SetVar("ola_ans3", answer3);
                @event.SetVar("decide_ans", "1");

                var say1 = " " + answer1 + ": 01-Answer  23-Starting Point  4-Not Active";
                var say2 = " " + answer2 + ": 01-Answer  34-Starting Point  5-Bottom  67-Not Active";
                var say3 = " " + answer3 + ": 01-Answer  23456-Starting Point  789-Different Portal  abcdef-Not Active";
                target.Message(say1);
                target.Message(say2);
                target.Message(say3);
            }
        }
    }
}
