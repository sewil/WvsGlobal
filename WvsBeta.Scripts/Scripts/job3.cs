using WvsBeta.Common;
using WvsBeta.Game.Scripting;
using WvsBeta.Game;
using System.Collections.Generic;
using static WvsBeta.Common.Constants.JobTracks;
using WvsBeta.Common.Enums;
using static WvsBeta.Common.Strings;
using System;

namespace WvsBeta.Scripts.Scripts
{
    public static class job3
    {
        public struct Question
        {
            readonly string question;
            readonly int answer;
            readonly string[] options;
            public Question(string question, int answer, params string[] options)
            {
                this.question = question;
                this.answer = answer;
                this.options = options;
            }
            public bool Ask(INpcHost self, int num)
            {
                string text = $"Here's the {num.Ordinal()} question. ";
                int idx = self.AskMenu(text + question, options);
                return idx == answer;
            }
        }
        public static IList<Question[]> Questions = new List<Question[]>() {
            new Question[] {
                new Question("Which NPC will you NOT see in Henesys of Victoria Island ...?", 1,
                    "#p1012101#",
                    "#p1002001#",
                    "#p1010100#",
                    "#p1012100#",
                    "#p1012102#"
                ),
                new Question("Which of the following monsters will you NOT see on Maple Island ...?", 3,
                    "#o100101#",
                    "#o1210102#",
                    "#o130101#",
                    "#o1210100#",
                    "#o120100#"
                ),
                new Question("Which of the following items did Maya ask to cure her illness? ...?", 4,
                    "Incredible Medicine",
                    "Bad Medicine",
                    "The Medicine That Cures Everything",
                    "Chinese Medicine",
                    "#t4031006#"
                ),
                new Question("Which of the following cities is NOT part of Victoria Island ...?", 1,
                    "Sleepywood",
                    "Amherst",
                    "Perion",
                    "Kerning City",
                    "Ellinia"
                ),
                new Question("What monster will you NOT find in the ant tunnel of Victoria Island's central dungeon ...?", 4,
                    "#o2110200#",
                    "#o2230100#",
                    "#o5130100#",
                    "#o2230101#",
                    "#o3000000#"
                )
            },
            new Question[] {
                new Question("Which of these pairings of the monster/leftover is correctly matched ...?", 3,
                    "#o3210100# - Fire Boar Nose",
                    "#o4230100# - Eye of Cold Eye",
                    "#o1210100# - Pig Ear",
                    "#o2300100# - #t4000042#",
                    "#o2230101# - Zombie Mushroom Sun Hat"
                ),
                new Question("Which of these NPCs will you NOT see in Perion of Victoria Island ...?", 1,
                    "#p1021100#",
                    "#p1032002#",
                    "#p1022002#",
                    "#p1022003#",
                    "#p1022100#"
                ),
                new Question("Which of these NPCs is the father of Alex the runaway kid, whom you'll see in Kerning City ...?", 4,
                    "#p1012005#",
                    "#p1012002#",
                    "#p12000#",
                    "#p20000#",
                    "#p1012003#"
                ),
                new Question("Which of these items will you receive from the NPC after collecting 30 Dark Marbles during the test for the 2nd job advancement ...?", 0,
                    "#t4031012#",
                    "The Necklace of a Hero",
                    "The Pendant of a Hero",
                    "The Medal of a Hero",
                    "The Mark of a Hero"
                ),
                new Question("Which of these pairings of job/required stat matches for the 1st job advancement...?", 2,
                    "Warrior - STR 30+",
                    "Magician - INT 25+",
                    "Bowman - DEX 25+",
                    "Thief - DEX 20+",
                    "Thief - LUK 20+"
                )
            },
            new Question[] {
                new Question("Which NPC will you see FIRST in MapleStory ...?", 4,
                    "#p2000#",
                    "#p1010100#",
                    "#p2102#",
                    "#p2001#",
                    "#p2101#"
                ),
                new Question("How many EXP points are needed to go from level 1 to 2 ...?", 1,
                    "10",
                    "15",
                    "20",
                    "25",
                    "30"
                ),
                new Question("Which NPC you will NOT see in El Nath of Ossyria ...?", 2,
                    "#p2020000#",
                    "#p2020003#",
                    "#p2012010#",
                    "#p2020006#",
                    "#p2020007#"
                ),
                new Question("Which of the following jobs will you NOT be able to pick for your 2nd job advancement ...?", 4,
                    "Page",
                    "Assassin",
                    "Bandit",
                    "Cleric",
                    "Mage"
                ),
                new Question("Which quest can be done again ...?", 3,
                    "Maya and the Weird Medicine",
                    "Alex the Runaway Kid",
                    "Pia and the Blue Mushroom",
                    "Arwen and the Glass Shoes",
                    "Alpha Platoon's Network of Communication"
                )
            },
            new Question[] {
                new Question("Which NPC is NOT part of the Alpha Platoon of Ossyria ...?", 0,
                    "Sargento Peter",
                    "#p2010000#",
                    "#p2020003#",
                    "#p2030002#",
                    "#p2030001#"
                ),
                new Question("Which of the following items is NOT required to awaken the old Gladius taken from Manji of Perion ...?", 3,
                    "#t4003002#",
                    "#t4021009#",
                    "#t4001006#",
                    "#t4003003#",
                    "#t4001005#"
                ),
                new Question("Which NPC will you NOT see in Kerning City of Victoria Island ...?", 4,
                    "#p1052002#",
                    "#p1052102#",
                    "#p1052012#",
                    "#p1052100#",
                    "#p1040000#"
                ),
                new Question("Which monster/leftover pair is NOT correct ...?", 1,
                    "#o3230200# - #t4000059#",
                    "#o4230105# - Nependeath's Plant Leaf",
                    "#o6130101# - #t4000040#",
                    "#o6130103# - #t4000050#",
                    "#o3210800# - #t4000029#"
                ),
                new Question("Which of the following monsters flies in the air...?", 4,
                    "#o5130104#",
                    "#o4230105#",
                    "#o4230103#",
                    "#o4130101#",
                    "#o5300100#"
                )
            },
            new Question[] {
                new Question("Which disease/result pair triggered by a monster attack is NOT correct...?", 2,
                    "State of darkness - accuracy reduction",
                    "State of cursed - EXP gain reduction",
                    "State of weakness - speed reduction",
                    "State of sealed - unable to use skills",
                    "State of poisoning - slow decrease of HP"
                ),
                new Question("Which NPC will you NOT see in Orbis of Ossyria ...?", 1,
                    "#p2010000#",
                    "#p1022100#",
                    "#p2010003#",
                    "#p2012004#",
                    "#p2012005#"
                ),
                new Question("Which of the following quests requires the highest level to start...?", 3,
                    "Manji's Old Gladius",
                    "Luke the Security Man's Wish to Travel",
                    "In Search of the Book of Ancient",
                    "Alcaster and the Dark Crystal",
                    "Alpha Platoon's Network of Communication"
                ),
                new Question("Which NPC has NOTHING to do with refining, improving and crafting items...?", 2,
                    "#p2010003#",
                    "#p1022003#",
                    "#p1032003#",
                    "#p1032002#",
                    "#p2020000#"
                ),
                new Question("In MapleStory, which potion/result pair is correct?", 4,
                    "#t2000001# - Recovers 200 HP",
                    "#t2001001# - Recovers 2000 MP",
                    "#t2010004# - Recovers 100 MP",
                    "#t2020001# - Recovers 300 HP",
                    "#t2020003# - Recovers 400 HP"
                )
            },
            new Question[] {
                new Question("Which NPC will you NOT see in Ellinia of Victoria Island...?", 4,
                    "#p1032003#",
                    "#p1032002#",
                    "#p1032001#",
                    "#p1032100#",
                    "#p1081101#"
                ),
                new Question("What monster will you NOT face in Ossyria ...?", 1,
                    "#o5140000#",
                    "#o5130103#",
                    "#o6300000#",
                    "#o8140000#",
                    "#o5120000#"
                ),
                new Question("Which monster has the highest level...?", 3,
                    "#o1120100#",
                    "#o1210101#",
                    "#o1110100#",
                    "#o1130100#",
                    "#o1210103#"
                ),
                new Question("In MapleStory, which potion/result pair is not correct...?", 1,
                    "#t2050003# - Recovers state of cursed and sealed",
                    "#t2020014# - Recovers 3000 MP",
                    "#t2020004# - Recovers 400 HP",
                    "#t2020000# - Recovers 200 MP",
                    "#t2000003# - Recovers 100 MP"
                ),
                new Question("Which NPC has NOTHING to do with pets ...?", 3,
                    "#p1012005#",
                    "#p1032102#",
                    "#p1012007#",
                    "#p1012002#",
                    "#p1012004#"
                )
            }
        };
        public static void changeJob(INpcHost self, GameCharacter target, Action<short> skillDescription)
        {
            var track = Constants.GetBaseJobTrack(target.Job);
            string baseJobName = Strings.Job.GetName(track).ToLower();
            var nRet = self.AskYesNo($"Okay! Now you'll be transformed into a much more powerful {baseJobName} through me. Before doing that, though, please make sure your SP has been thoroughly used. You'll need to use up at least all of SP's gained until level 70 to make the 3rd job advancement. Oh, and since you have already chosen your path of the occupation by the 2nd job advancement, you won't have to choose again for the 3rd job advancement. Do you want to do it right now?");
            if (nRet == 0) self.Say("You've already passed the test, so there's nothing to worry about... well, come talk to me when you make up your mind. Once you're ready, I'll grant you your 3rd job advancement. As long as you're ready...");
            else
            {
                var nPSP = (target.Level - 70) * 3;
                if (target.SP > nPSP) self.Say("Hmm... you seem to have too much #bSP#k. You won't be able to make the 3rd job advancement with so much unused SP. Spend more SP on 1st and 2nd job skills before coming back here.");
                else
                {
                    target.SetJob((short)(target.Job + 1));
                    target.IncSP(1, 0);
                    target.IncAP(5, 0);
                    skillDescription(target.Job);
                    //string closing = "Once you feel like you cannot train yourself to reach a higher place, then, and only then, come see me. I'll be here waiting.";//4th
                    string closing = "Go, now, and be at one with the world.";
                    self.Say($"I've also given you a little bit of SP and AP, which will be beneficial to you. You have now become a powerful, powerful {baseJobName}, indeed. Remember, though, that the real world will be awaiting your arrival with even tougher obstacles to overcome. {closing}");
                }
            }
        }
        public static bool chiefTest(INpcHost self, GameCharacter target, Tracks track, int chiefid, int jobmasterid, string prompt)
        {
            var qr = target.QuestRecord;
            var val = qr.GetState(7500);
            var info = qr.Get(7500);
            var inventory = target.Inventory;
            var jobm = Npcs.Data[jobmasterid];
            if (val == 0)
            {
                var chief = Npcs.Data[chiefid];
                var nRet = self.AskYesNo($"Welcome. {chief.description} " + prompt + " Are you ready to be tested and make the 3rd job advancement?");
                if (nRet == 0) self.Say("I don't think you're ready to face the challenges ahead. Come see me only when you convince yourself that you are ready to face the challenges that come along with your advancement.");
                else
                {
                    qr.Set(7500, "s");
                    self.Say($"Good. You will be tested in two aspects that are important to {Strings.Job.GetName(track, true).ToLower()}: strength and wisdom. I will now explain to you the physical part of the test. Remember #b#p{jobmasterid}##k by #m{jobm.homeid}#? Go up to {Gender.Pronoun(jobm.gender)} and listen to {Gender.Pronoun(jobm.gender, pos: true)} explanation of the first part of the test. Complete the quest and receive #b#t4031057##k from #p{jobmasterid}#.");
                    self.Say($"The mental portion of the test will begin only when you have passed the physical portion. #b#t4031057##k will be proof that you passed the test. I'll let #b#p{jobmasterid}##k know you're on your way to {Gender.Pronoun(jobm.gender)}. Be prepared. It won't be easy, but I have a lot of faith in you. Good luck!");
                }
            }
            else if (val == 1 && (info == "s" || info == "p1")) self.Say($"You don't have #b#t4031057##k on you. Go see #m{jobm.homeid}#'s #b#p{jobmasterid}##k, pass the test and bring #b#t4031057##k with you. Only then can you take the second test. Good luck to you.");
            else if (val == 1 && info == "p2")
            {
                if (inventory.ItemCount(4031057) >= 1)
                {
                    self.Say("Congratulations on completing the physical portion of the test. I knew you could do it. Now that you've passed the first part of the test, you can take the second part. Give me the necklace first.");
                    var ret = inventory.Exchange(0, 4031057, -1);
                    if (ret == 0) self.Say($"Are you sure that you've received #b#t4031057##k from #b#p{jobmasterid}##k? Don't forget to leave some space in your etc. inventory.");
                    else
                    {
                        qr.Set(7500, "end1");
                        self.Say("Here is the 2nd part of the test. This test will determine if you are smart enough to take the next step towards greatness. There is a dark, snow-covered area called the #m211040401# in Ossyria. Not even monsters can get there. In the middle of the map, there is a giant stone called #p2030006#. You will have to offer a special item as a sacrifice. The #p2030006# will test your wisdom right there.");
                        self.Say("You will be asked to answer all questions with honesty and conviction. If you answer everything correctly, the #p2030006# will formally accept you and give you #b#t4031058##k. Bring the necklace back to me and I will help you take the next step. Good luck!");
                    }
                }
                else self.Say($"You don't have #b#t4031057##k on you. Go see #m{jobm.homeid}#'s #b#p{jobmasterid}##k, pass the test and bring #b#t4031057##k with you. Only then may you pass the first part of the test. Good luck!");
            }
            else if (val == 1 && info == "end1")
            {
                if (inventory.ItemCount(4031058) >= 1)
                {
                    self.Say("Congratulations on completing the mental portion of the test. You answered all questions correctly and wisely. I have to say, I'm quite impressed with the level of wisdom you've displayed. Hand me the necklace first before we take the next step.");
                    var ret = inventory.Exchange(0, 4031058, -1);
                    if (ret == 0) self.Say("Are you sure you have been granted #b#t4031058##k by the #p2030006#? If you're sure, don't forget to leave some space in your etc. inventory.");
                    else
                    {
                        qr.SetComplete(7500);
                        return true;
                    }
                }
                else self.Say("You don't have #b#t4031058##k on you. Find the dark, snow-covered area called #m211040401# in Ossyria, offer the special item as a sacrifice, and answer all questions with honesty and conviction in order to receive #b#t4031058##k. Bring this back to me to complete the 3rd job advancement test. Good luck to you...");
            }
            else if (val == 2) return true;
            return false;
        }
        public static int promptCommon(INpcHost self, GameCharacter target, Tracks track, int npcid)
        {
            short cJob = target.Job;
            if (target.Level < 50)
            {
                self.Say("Hmm... Looks like there's nothing I can do to help you. Come back here when you get stronger.");
                return -1;
            }

            var qr = target.QuestRecord;
            
            var m1 = self.AskMenu("What do you want from me?", "I want to make the 3rd job advancement", "Please allow me to do the Zakum Dungeon Quest");
            if (m1 == 0)
            {
                if (target.Level < 70)
                {
                    self.Say("You are not yet eligible for the 3rd job advancement. You need to be at least level 70 to do this. Train more and then come see me.");
                    return -1;
                }
                else if (!Constants.IsJobTrack(cJob, track))
                {
                    self.Say($"{Npcs.Data[npcid].description} But you don't look like a {Strings.Job.GetName(track).ToLower()}. Unfortunately, I can't help you. This room is full of chiefs for their respective jobs. If you need anything, go talk to one of them.");
                    return -1;
                }
                else if (Constants.IsThirdJob(cJob))
                {
                    self.Say($"It was you who passed the tests to move up to the 3rd career level. How is the life of a #b{Strings.Job.GetName(cJob)}#k? You will have to keep training as you make your journey through this place. Ossyria is full of powerful monsters that even I don't know about. If you have any doubts, come to me at the end of this road. I wish you good luck.");
                    return -1;
                }
                return 0;
            }
            else if (m1 == 1)
            {
                var zq = qr.Get(7000);

                if (zq == "")
                {
                    if (target.Level >= 50)
                    {
                        if (Constants.IsJobTrack(target.Job, track))
                        {
                            qr.Set(7000, "s");
                            self.Say("You want permission to do the Zakum Dungeon Quest, right? It should be #b#p2030008##k... ok, right! I'm sure you'll be fine in the dungeon. I hope you'll be careful there...");
                        }
                        else self.Say($"So you want permission to do the Zakum Dungeon Quest. I'm sorry, but you don't look like a {Strings.Job.GetName(track)}. Go find the head of your profession.");
                    }
                    else self.Say("So you want permission to do the Zakum Dungeon Quest. I'm sorry, but the dungeon is still too challening for you. You must be at least level 50 to even try... train more and then come back here.");
                }
                else self.Say("How are you doing on the Zakum Dungeon Quest? I've heard that there's an incredible monster in the depths of that place... anyway, good luck. I'm sure you'll make it.");
                return 1;
            }
            else
            {
                return -1;
            }
        }
    }
    [Script("warrior3")]
    class warrior3 : INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            int nRet = job3.promptCommon(self, target, Tracks.Warrior, NpcIds.Tylus);
            if (nRet == 0)
            {
                bool testResult = job3.chiefTest(self, target, Tracks.Warrior, NpcIds.Tylus, NpcIds.DancesWithBalrog, "You seem like the kind of warrior that wants to make the leap forward to the 3rd job advancement. But I've seen countless warriors eager to make the jump just like you, only to fail. What about you?");
                if (testResult)
                {
                    job3.changeJob(self, target, cJob => {
                        string jobName = Strings.Job.GetName(cJob);
                        string a = $"You're the #b{jobName}#k from here on out. ";
                        if (cJob == 111) self.Say($"{a}A number of new attacking skills such as #bShout#k and #bCombo Attack#k are devastating, while #bArmor Crash#k will put a dent on the monsters' defensive abilities. It'll be best to concentrate on acquiring skills with the weapon you mastered during the days as a Fighter.");
                        else if (cJob == 121) self.Say($"{a}You'll be introduced to a new skill book featuring various new attacking skills as well as element-based attacks. I suggest that you continue to use a weapon complementary to the Page, whether it be a sword or a blunt weapon. There's a skill called #bCharge#k, which adds an element of fire, ice, or lightning to the weapon, making the Knight the only warrior that can perform element-based attacks. Charge your weapon with an element that weakens the monster, and then apply massive damage with the #bCharged Blow#k. You would definitely be a devastating force.");
                        else if (cJob == 131) self.Say($"{a}You'll be introduced to a slew of new attacking skills for spears and pole arms, and whatever weapon was chosen as the Spearman should be continued as the Dragon Knight. Skills such as #bCrusher#k (maximum damage to one monster) and #bDragon Fury#k (damage to multiple monsters) are recommended as main attacking skills of choice, while a skill called #bDragon Roar#k will damage everything on screen with devastating force. The downside is the fact that the skill uses up over half of the available HP.");
                    });
                }
            }
        }
    }
    // robeira, the npc for 3rd job adv for magicians
    [Script("wizard3")]
    class wizard3 : INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            int nRet = job3.promptCommon(self, target, Tracks.Magician, NpcIds.Robeira);
            if (nRet == 0)
            {
                bool testResult = job3.chiefTest(self, target, Tracks.Magician, NpcIds.Robeira, NpcIds.Grendel, "You seem ready to take on the challenges of the 3rd job advancement, but I advise you not to get too confident. I've seen too many magicians come and go, failing to live up to my expectations in the process. I don't know about you, though.");
                if (testResult)
                {
                    job3.changeJob(self, target, cJob => {
                        string jobName = Strings.Job.GetName(cJob);
                        string a = $"You're the #b{jobName}#k from here on out. ";
                        if (cJob == 211 || cJob == 221)
                        {
                            self.Say($"{a}The new skill book features new and improved spells for {(cJob == 211 ? "fire and poison" : "ice and lightning")}, and skills such as #b#q2110001##k (improves element-based spells) and #b#q2111005##k (improves the overall speed of the attacking spells) will enable you to attack the monsters quickly and effectively. Defensive spells such as #b#q2110000##k (improved def. against certain element-based attacks) and #b#q2111004##k (seals up the monster) will help negate the one weakness Mages possess: lack of HP.");
                        }
                        else if (cJob == 231)
                        {
                            self.Say($"{a}The new skill book features new and improved holy spells such as #b#q2311004##k and #b#q2311006##k, and skills such as #b#q2311002##k (creates a door for the exit to the nearest town) and #b#q2311003##k (improves the EXP gained) can be vital to the party play. Off-beat spells such as #b#q2311005##k (turns monsters into snails) separates Priests from other jobs as the most different of all.");
                        }
                    });
                }
            }
        }
    }
    // Rene, the npc for 3rd job adv for bowman
    [Script("bowman3")]
    class bowman3 : INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            int nRet = job3.promptCommon(self, target, Tracks.Bowman, NpcIds.Rene);
            if (nRet == 0)
            {
                bool testResult = job3.chiefTest(self, target, Tracks.Bowman, NpcIds.Rene, NpcIds.AthenaPierce, "You seem like the kind of bowman that wants to make the leap forward, the one ready to take on the challenges of the 3rd job advancement. But I've seen countless bowmen eager to make the jump just like you, only to see them fail. What about you?");
                if (testResult)
                {
                    job3.changeJob(self, target, cJob => {
                        string jobName = Strings.Job.GetName(cJob);
                        string a = $"You have officially become the #b{jobName}#k. ";
                        if (cJob == 311)
                        {
                            self.Say($"{a}One of the skills that you'll truly embrace is a skill called #bMortal Blow#k, that allows Rangers to fire arrows from close-range. #bInferno#k allows Rangers to temporarily perform fire-based attacks on monsters, while skills like #bPuppet#k (summons a scarecrow which attracts the monsters' attention) and #bSilver Hawk#k (summons a Silver Hawk that attacks monsters) solidify the Bowman's status as a long-range attack extraordinaire.");
                        }
                        else
                        {
                            self.Say($"{a}One of the skills that you'll truly embrace is a skill called #bMortal Blow#k, that allows Snipers to fire arrows from close-range. #bBlizzard#k allows Snipers to temporarily perform ice-based attacks on monsters, while skills like #bPuppet#k (summons a scarecrow which attracts the monsters' attention) and #bGolden Eagle#k (summons a Golden Eagle that attacks monsters) solidify the Bowman's status as a long-range attack extraordinaire.");
                        }
                    });
                }
            }
        }
    }
    // Arec, the NPC for the thief's 3rd job adv
    [Script("thief3")]
    class thief3 : INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            int nRet = job3.promptCommon(self, target, Tracks.Thief, NpcIds.Arec);
            if (nRet == 0)
            {
                bool testResult = job3.chiefTest(self, target, Tracks.Thief, NpcIds.Arec, NpcIds.DarkLord, "You seem ready to make the leap forward, the one ready to take on the challenges of the 3rd job advancement. Too many thieves have come and gone, unable to meet the standards of achieving the 3rd job advancement. What about you?");
                if (testResult)
                {
                    job3.changeJob(self, target, cJob => {
                        string jobName = Strings.Job.GetName(cJob);
                        string a = $"You have officially been anointed as a #b{jobName}#k from here on out. ";
                        if (cJob == 411) self.Say($"{a}The skill book introduces a slew of new attacking skills for Hermits using shadows as a way of duplication and replacement, including such skills as #bShadow Meso#k (replacing MP with mesos and attack monsters with the damage based on the amount of mesos thrown) and #bShadow Partner#k (creating a shadow that mimics every move, enabling a Hermit to attack a monster as if two Hermits are there). Use those skills to take on the monsters that may have been difficult to conquer before.");
                        else if (cJob == 421) self.Say($"{a}One of the new additions to the skill book is a skill called #bBand of Thieves#k, in which you can summon fellow Bandits to attack multiple monsters at once. Chief Bandits can also utilize mesos in numerous ways, from attacking monsters (#bMeso Explosion#k, which explodes the mesos on the ground), to defending yourself (#bMeso Guard#k, which decreases the weapon damage).");
                    });
                }
            }
        }
    }
    // The door of dimension
    [Script("crack")]
    class crack : INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            var field = target.Field;
            var qr = target.QuestRecord;
            var val = qr.Get(7500);
            var cJob = target.Job;
            Tracks track = Tracks.Beginner;
            int jobmaster = 0;

            // moving the warrior
            if (field.ID == 105070001)
            {
                track = Tracks.Warrior;
                jobmaster = NpcIds.DancesWithBalrog;
            }
            // moving the magician
            else if (field.ID == 100040106)
            {
                track = Tracks.Magician;
                jobmaster = NpcIds.Grendel;
            }
            // moving the bowman
            else if (field.ID == 105040305)
            {
                track = Tracks.Bowman;
                jobmaster = NpcIds.AthenaPierce;
            }
            else if (field.ID == 107000402)
            {
                track = Tracks.Thief;
                jobmaster = NpcIds.DarkLord;
            }

            if (val == "p1" && Constants.IsSecondJob(cJob) && Constants.IsJobTrack(cJob, track))
            {
                var setParty = FieldSet.Instances["ThirdJob" + (int)track];
                var res = setParty.Enter(target, 0);
                if (res != 0) self.Say($"There is already someone struggling with #b#p{jobmaster}##k's clone. Please come back later.");
            }
            else
            {
                self.Say("It seems that there is a door that will take me to another dimension, but I cannot enter for some reason.");
                return;
            }
        }
    }
    // Getting out of the Other Dimension
    [Script("3jobExit")]
    class job3Exit : INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            var field = target.Field;

            var nRet = self.AskYesNo("You can use the #p1061010# to return to the real world. Are you sure you want to go back?");
            if (nRet != 0)
            {
                if (field.ID == 108010301) target.ChangeMap(102000000, "");
                else if (field.ID == 108010201) target.ChangeMap(101000000, "");
                else if (field.ID == 108010101) target.ChangeMap(100000000, "");
                else if (field.ID == 108010401) target.ChangeMap(103000000, "");
            }
        }
    }
    // The Holy Land
    [Script("holyStone")]
    class holyStone : INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            var qr = target.QuestRecord;
            var val = qr.Get(7500);

            if (val == "end1")
            {
                var nRet = self.AskYesNo("... ... ...\r\nIf you want to test out your wisdom, then you'll have to offer #b#t4005004##k as the sacrifice...\r\nAre you ready to offer #t4005004# and answer a set of questions for me?");
                if (nRet == 0) self.Say("Come back when you're ready.");
                else
                {
                    var inventory = target.Inventory;
                    if (inventory.SlotCount(4) > inventory.HoldCount(4))
                    {
                        if (inventory.ItemCount(4031058) >= 1) self.Say("You already have #b#t4031058##k with you...\r\nTake the necklace and go back there...");
                        else
                        {
                            var ret = inventory.Exchange(0, 4005004, -1);
                            if (ret == 0) self.Say("If you want to test out your wisdom, you must offer #b#t4005004##k as the sacrifice.");
                            else
                            {
                                self.Say("Alright ... I'll be testing out your wisdom now. Answer all the questions correctly in order to pass the test. BUT, if you lie to me even once, you'll have to start all over again ... ok, here we go.");
                                int idx = Rand32.NextBetween(0, job3.Questions.Count - 1);
                                var questions = job3.Questions[idx];
                                for (int i = 0; i < questions.Length; i++)
                                {
                                    var question = questions[i];
                                    if (!question.Ask(self, i + 1))
                                    {
                                        self.Say("That's the wrong answer...\r\nStart over...");
                                        return;
                                    }
                                }
                                self.Say("Very well. All of your answers were correct...\r\nYour wisdom has been proven.\r\nTake this necklace and go back there...");
                                inventory = target.Inventory;
                                ret = inventory.Exchange(0, 4031058, 1);
                                if (ret == 0) self.Say("Your etc. inventory seems to be full...?");
                            }
                        }
                    }
                    else self.Say("Your etc. inventory is full... make some room in it or you won't be able to take the test. After making adjustments, try again...");
                }
            }
        }
    }
}
