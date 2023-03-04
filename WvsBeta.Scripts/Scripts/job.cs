using System;
using System.Collections.Generic;
using WvsBeta.Common;
using WvsBeta.Game;
using WvsBeta.Game.Scripting;
using static WvsBeta.Common.Constants.JobTracks;

namespace WvsBeta.Scripts.Scripts
{
    public static class Job
    {
        public const string AdvanceDeny = "Really? Think on it a little more. There's no rush... it's not something you should just do anyway... Let me know when you make up your mind, okay?";
    }
    public static partial class Job2
    {
        public static IDictionary<short, ((string,string) name, string[] descriptions)> JobStrings = new Dictionary<short, ((string,string), string[])>()
        {
            #region warrior
            { Constants.Fighter.ID, (("Fighter", "Fighters"), new string[] {
                "It's the most common kind of Warrior. The weapons they use are #bswords#k and #baxes#k, because there will be advanced skills available to acquire later on. I strongly recommend you avoid using both weapons, but rather stick to the one of your liking...",
                "Other than that, there are also skills such as #b#q1101006##k and #b#q1101007##k available for Fighters. #b#q1101006##k is an ability that allows you and your party to temporarily enhance your weapon power. With that you can take out enemies with a sudden surge of power, so it'll come very handy for you. The downside to this is that your guarding ability (defense) goes down a bit.",
                "#b#q1101007##k is an ability that allows you to return a portion of the damage that you take from a physical hit by an enemy. The harder the hit, the higher the damage they'll get in return. It'll help those who prefer close combat. What do you think? Isn't being a Fighter pretty cool?"
            }) },
            { Constants.Page.ID, (("Page", "Pages"), new string[] {
                "Let me explain the role of the Page. The Page is an apprentice knight taking their first steps. They often use #bswords#k and/or #bmaces#k. It's not a good idea to use both weapons, so you're better off picking one and sticking with it.",
                "In addition, there are also skills like #b#q1201006##k and #b#q1101007##k for you to learn. #b#q1201006##k makes any opponent around you lose some attack and defense for a certain amount of time. It is very useful against powerful monsters with a high physical attack. It's also quite useful when partying with others.",
                "#b#q1101007##k is a skill that allows you to return a certain amount of damage done by monsters for a certain time. The more damage you take, the more damage you also deal to the enemy. It's the perfect skill for Warriors who are specializing in melee combat. What do you think? Doesn't it sound cool to be a Page?"
            }) },
            { Constants.Spearman.ID, (("Spearman", "Spearmen"), new string[] {
                "Let me explain the role of the Spearman. It is a class that specializes in the use of long weapons, such as #bspears#k and #bpolearms#k. There are many useful skills to be gained with both weapons, but I recommend picking one and sticking with it.",
                "In addition, there are also skills like #b#q1301006##k and #b#q1301007##k for you to learn. #b#q1301006##k increases the weapon and magic defense for you and your party for a certain amount of time. It's a very useful skill for Spearmen with weapons that require both hands and can't defend at the same time.",
                "#b#q1301007##k is a skill that temporarily increases the HP and MP for you and your party. You will be able to give a maximum increase of up to 160%, so the skill will help you and your party, especially when you are going after really powerful opponents. What do you think? Doesn't it sounds cool to be a Spearman?"
            }) },
            #endregion
            #region mage
            { Constants.FPWizard.ID, (("Wizard of Fire and Poison", "Wizards"), new string[] {
                "Allow me to explain the Wizard of Fire and Poison. They specialize in fire and poison magic. Skills like #b#q2101001##k, that allows you and your whole party's magic ability to increase for a time being, and #b#q2100000##k, which allows you a certain probability of absorbing some of your enemy's MP, are essential to all the attacking Magicians.",
                "I'll explain to you a magic attack called #b#q2101004##k. It fires flaming arrows at the enemies, making this attack the most powerful skill available among 2nd-level skills. It works best against enemies that are weak to fire in general, as the damage will be much higher. On the other hand, if you use it against enemies that are resistant to fire, the damage will be halved. Do not forget that.",
                "I'll explain to you a magic attack called #b#q2101005##k. It fires poisonous bubbles at the enemies, and thus they become poisoned. After that, the enemy's HP will decrease more and more over time. If the spell doesn't work very well or if the monster has high HP, it might be a good idea to shoot as many times as necessary to kill it with an overdose of poison..."
            }) },
            { Constants.ILWizard.ID, (("Wizard of Ice and Lightening", "Wizards"), new string[] {
                "Allow me to explain the Wizard of Ice and Lightening. They specialize in ice and lightening magic. Skills like #b#q2101001##k, that allows you and your whole party's magic ability to increase for a time being, and #b#q2100000##k, which allows you a certain probability of absorbing some of your enemy's MP, are essential to all the attacking Magicians.",
                "I'll explain to you a magic attack called #b#q2201004##k. It fires away pieces of ice at the enemies, and although not quite as powerful as Fire Arrow, whoever's struck by it will be frozen for a short period of time. The damage increases much more if the enemy happens to be weak against ice. The opposite holds true, too, in that if the enemy is used to ice, the damage won't be quite as much, so keep that in mind.",
                "I'll explain to you a magic attack called #b#q2201005##k. It's the only 2nd-level skill for Magicians that can be considered the Total Spell, affecting a lot of monsters at once. It may not dish out a lot of damage, but the advantage is that it damages all the monsters around you. You can only attack up to six monsters at once, though. Still, it's a pretty incredible attack."
            }) },
            { Constants.Cleric.ID, (("Cleric", "Clerics"), new string[] {
                "Allow me to explain the Cleric. Clerics use religious magic on monsters through prayers and incantation. Skills like #b#q2301004##k, which temporarily improves the weapon def., magic def., accuracy, avoidability, and #b#q2301003##k, which decreases the weapon damage for a certain amount, help Magicians overcome their shortcomings ...",
                "Cleric is the only Wizard that can perform recovering magic. Clerics are the only ones that can do recovery magic. It's called #b#q2301002##k, and the more MP, INT's, and the skill level for this skill you have, the more HP you may recover. It also affects your party close by so it's a very useful skill, enabling you to continue to hunt without the help of the potion.",
                "Clerics also have a magic attack called #b#q2301005##k. It is a spell that allows the Cleric to fire phantom arrows at monsters. The effect isn't very big, but it can deal huge damage to zombies and other evil monsters. These monsters are very weak to holy attacks. What do you think? Does it not sound interesting?"
            }) },
            #endregion
            #region bowman
            { Constants.Hunter.ID, (("Hunter", "Hunters"), new string[] {
                "OK. Here's what it's like to be a Hunter. Hunters have skills like #q3100000# and #q3101002# that allow them to use bows well. There is also a skill called #q3101004# for Hunters who use too many arrows. It allows you to shoot arrows for a longer period of time without actually wasting your ammo. So, if you've already spent a lot of mesos on arrows, this skill is perfect for you...",
                "I'll explain to you one of the Hunter's abilities, #b#q3101003##k. No one is better than a Hunter at ranged attacks, but the story changes completely when there are multiple enemies or if you need to attack them up close. Therefore, this skill is very important. It not only allows you to attack the enemy at close range, but also knocks back several monsters. It's a very important skill which grants you a little more space, which is much needed.",
                "I'll explain to you the Hunter's attack skill, #b#q3101005##k. It's a skill that allows you to shoot arrows with bombs. If it hits the target, the bomb will explode on top of the enemy, damaging those around them and temporarily knocking them down. Combine this skill with #q3000001#, the first level skill, and the damage will be incredible. You should try becoming a Hunter."
            }) },
            { Constants.Crossbowman.ID, (("Crossbowman", "Crossbowmen"), new string[] {
                "OK. Here's what it's like to be a Crossbowman. For the Crossbowman, skills such as #q3200000# and #q3201002# are available along with #q3101004# for those who wear down their bows by shooting too much and hitting too little. This ability allows the player to shoot arrows for a longer period of time without wearing down the bow. So if you've already spent a lot of mesos on arrows, you'll want to check it out...",
                "One of the skills a Crossbowman can have is #b#q3101003##k. No one can get close to the Crossbowman's ranged attacks, but it's a different story when it comes to melee combat or taking on many enemies at once. Therefore, this skill is very important. It allows you to attack at full power, knocking back several enemies in the process. It's a very important skill that grants you a little more space, which is much needed.",
                "I'll explain one of the Crossbowman's attack skills, #b#q3201005##k. This skill allows you to attack multiple enemies, as the arrow goes through the monster and hits another monster behind it. The damage decreases each time the arrow passes through an enemy, but it still manages to hit multiple enemies at once, a very nice skill to have. And... if it's combined with #q3000001#... it's simply amazing."
            }) },
            #endregion
            #region thief
            { Constants.Assassin.ID, (("Assassin", "Assassins"), new string[] {
                "Let me explain the role of the Assassin. The Assassin is the Thief who uses ninja stars or daggers. Skills like #b#q4100000##k and #b#q4100001##k will help you to better use your ninja stars. Upgrade #q4100000# further and your maximum number of ninja stars will increase. So, you'd better learn this skill. Keep that in mind.",
                "I'll explain to you one of the Assassin's abilities, #b#q4101004##k. It temporarily enhances you and your party's jumping abilities and movement speed. Perfect for when facing very fast enemies. It is also useful when walking to a distant place. Wouldn't it be great to get to your destination right away instead of taking a whole day?",
                "And here is another skill available for the Assassin: #b#q4101005##k. It allows you to recover a portion of damage dealt to an enemy and absorb it as HP! The greater the damage, the more health you'll recover... isn't that amazing? Remember that the maximum you can absorb at once will be half your maximum HP. The higher the enemy's HP, the more you can absorb."
            }) },
            { Constants.Bandit.ID, (("Bandit", "Bandits"), new string[] {
                "Here's what it's like to be a Bandit. Bandits are Thieves who specialize in using daggers. Skills like #b#q4200000##k and #b#q4201002##k will help you to better use your dagger. For starters, daggers have a fast attack speed and if you add a specialization to that... wow! It's going to be so fast it'll scare any monster!",
                "I'll explain to you what #b#q4201004##k does for the Bandits. It offers you a chance to steal an item from the enemy. You can only steal from one enemy at a time, but you can keep trying until you succeed. The stolen item will fall to the ground; don't forget to pick it up immediately, as anyone can get it.",
                "I'll explain to you what #b#q4201005##k does for the Bandits. It uses HP and MP to attack the enemy 6 TIMES with the dagger. The higher the skill level, the more attacks you will be able to do. You will cut the enemy to pieces with the dagger... aaaah, isn't that great? What do you think? Want to become a Bandit and feel the adrenaline rush?"
            }) },
            #endregion
        };
        public const string WarriorStart = "Wow, you've really grown up! You're not small and weak anymore... Now I can feel your Warrior presence! Impressive... so what do you think? Do you want to become stronger than you already are? Just do a simple test! Accepted?";
        public const string StartDeny = "Really? Getting stronger quickly will help you a lot during your journey... If you change your mind in the future, you can come back here whenever you want. Remember that I will make you much more powerful than you already are.";

        public const string AdvanceTooMuchSp = "Hmm... looks like you have too much #bSP#k... you can't advance to 2nd job with so much SP saved. Please use more SP on 1st job skills and come back later.";
        public static Func<int,string> AdvanceNoLetter = (int npcid) => $"Hmm... Are you sure you have #b#t4031012##k from #p{npcid}#? I can't allow you to make the 2nd job advancement without it.";
        public const string GivenSp = "I have also given you a little bit of #bSP#k. Open the #bSkill Menu#k located at the bottom right corner. You'll be able to boost up your newly-acquired 2nd job skills. A word of warning though: You can't boost them up all at once. Some of the skills are only available after you have learned other skills. Make sure to remember that.";
        public static bool AskJobDescriptionMenu(INpcHost self, short[] jobs)
        {
            string menu = "Alright, when you have made your decision, click on [I'll choose my occupation!] at the very bottom.";
            for (int i = 0; i < jobs.Length; i++)
            {
                menu += $"\r\n#b#L{i}#Please explain the characteristics of the {JobStrings[jobs[i]].name.Item1}.#k#l";
            }
            menu += $"\r\n#b#L{jobs.Length}#I'll choose my occupation!#k#l";
            int v1 = self.AskMenu(menu);

            if (v1 < jobs.Length)
            {
                foreach (string desc in JobStrings[jobs[v1]].descriptions)
                {
                    self.Say(desc);
                }
                return true;
            }
            return false;
        }
        public static string JobMenu(short[] jobs)
        {
            string menu = "";
            for (int i = 0; i < jobs.Length; i++)
            {
                menu += $"\r\n#b#L{i}#{JobStrings[jobs[i]].name.Item1}#k#l";
            }
            return menu;
        }
        public static bool Advance(int trainerid, string prompt, INpcHost self, GameCharacter target, short jobId, short incMMP, short incMHP, params (byte, byte)[] slots)
        {
            var mJob = self.AskYesNo(prompt);
            var inventory = target.Inventory;
            if (mJob == 0) self.Say(Job.AdvanceDeny);
            else if (mJob == 1)
            {
                var nPSP = (target.Level - 30) * 3;
                if (target.SP > nPSP) self.Say(Job2.AdvanceTooMuchSp);
                else
                {
                    var ret = inventory.Exchange(0, 4031012, -1);
                    if (ret == 0) self.Say(AdvanceNoLetter(trainerid));
                    else
                    {
                        target.SetJob(jobId);
                        target.IncSP(1, 0);
                        
                        if (incMMP > 0)
                            target.IncMMP(incMMP, 0);
                        if (incMHP > 0)
                            target.IncMHP(incMHP, 0);
                        foreach (var slot in slots)
                            inventory.IncSlotCount(slot.Item1, slot.Item2);
                        return true;
                    }
                }
            }
            return false;
        }
    }
    public static class Job3
    {
        struct JobData
        {
            public string jobmaster;
            public string dimension;
            public string[] strings;
            public JobData(string jobmaster, string dimension, string[] strings)
            {
                this.jobmaster = jobmaster;
                this.dimension = dimension;
                this.strings = strings;
            }
        }
        private static IDictionary<Tracks, JobData> Data = new Dictionary<Tracks, JobData>()
            {
                { Tracks.Warrior, new JobData("Tylus", "ant tunnel", new string[] {
                    "I was waiting for you. Few days ago, I heard about you from #bTylus#k in Ossyria. Well... I'd like to test your strength. There is a secret passage near the ant tunnel. Nobody but you can go into that passage. If you go into the passage, you will meet my the other self. Beat him and bring #b#t4031059##k to me.",
                    "My the other self is quite strong. He uses many special skills and you should fight with him 1 on 1. However, people cannot stay long in the secret passage, so it is important to beat him ASAP. Well... Good luck! I will look forward to you bringing #b#t4031059##k to me."
                }) },
                { Tracks.Magician, new JobData("Robeira", "forest of Ellinia", new string[] {
                    "I was waiting for you. Few days ago, I heard about you from #bRobeira#k in Ossyria. Well... I'd like to test your strength. There is a secret passage near the forest of Ellinia. Nobody but you can go into that passage. If you go into the passage, you will meet my the other self. Beat him and bring #b#t4031059##k to me.",
                    "My the other self is quite strong. He uses many special skills and you should fight with him 1 on 1. However, people cannot stay long in the secret passage, so it is important to beat him ASAP. Well... Good luck! I will look forward to you bringing #b#t4031059##k to me."
                }) },
                { Tracks.Bowman, new JobData("Rene", "sleepy dungeon", new string[] {
                    "Been waiting for you. A while ago, #bRene#k of Ossyria gave me a word on you. I see that you are interested in making the leap to the third job advancement. To do that, I will have to conduct a test of strength on you, in order to see whether you are worthy of the advancement. In the midst of a thick forest somewhere in the Victoria Island, you'll find a secret passage that'll lead you to a whole new dimension. Once inside, you'll face a clone of myself. Your task is to defeat her and bring #b#t4031059##k with you.",
                    "Since she is a clone of myself, you can expect a tough battle ahead. She uses a number of special attacking skills unlike any you have ever seen, and it is your task to successfully take her down. There is a time limit in the secret passage, so it is crucial that you defeat her fast. I wish you the best of luck, and I hope you bring the #b#t4031059##k with you."
                }) },
            };
        public static void Test1Start(INpcHost self, GameCharacter character, Tracks track)
        {
            var qr = character.Quests;
            qr.Set(7500, "p1");
            var data = Data[track];
            self.Say(data.strings[0]);
            self.Say(data.strings[1]);
        }
        public static void Test1Complete(INpcHost self, GameCharacter character, Tracks track)
        {
            var inventory = character.Inventory;
            var qr = character.Quests;
            var value = Data[track];
            if (inventory.ItemCount(4031059) >= 1)
            {
                self.Say($"Wow... You beat my the other self and brought #b#t4031059##k to me. Good! This surely proves your strength. In terms of strength, you are ready to advance to 3rd job. As I promised, I will give #b#t4031057##k to you. Give this necklace to #b{value.jobmaster}#k in Ossyria and you will be able to take second test of 3rd job advancement. Good luck~");
                var ret = inventory.Exchange(0, 4031059, -1, 4031057, 1);
                if (ret == 0) self.Say("hmmm... weird. Are you sure that you have #b#t4031059##k? If you do have it, make sure you have empty slot in item tab.");
                else qr.Set(7500, "p2");
            }
            else self.Say($"There is a secret passage near the {value.dimension}. Nobody but you can go into that passage. If you go into the passage, you will meet my the other self. Beat him and bring #b#t4031059##k to me. My the other self is quite strong. He uses many special skills and you should fight with him 1 on 1. However, people cannot stay long in the secret passage, so it is important to beat him ASAP. Well... Good luck! I will look forward to you bringing #b#t4031059##k to me.");
        }
        public static void Test1LostLetter(INpcHost self, GameCharacter target, Tracks track)
        {
            var inventory = target.Inventory;
            var value = Data[track];
            if (inventory.ItemCount(4031057) <= 0)
            {
                self.Say("Ahh! You lost #b#t4031057##k, huh? I said you should be careful... For God's sake, I'll give you another one... AGAIN. Please be careful this time. Without this, you will not be able to take the second test of 3rd job advancement.");
                var ret = inventory.Exchange(0, 4031057, 1);
                if (ret == 0) self.Say("hmmm... weird. Make sure you have empty slot in item tab.");
            }
            else self.Say($"Give this necklace to #b{value.jobmaster}#k in Ossyria and you will be able to take the second test of 3rd job advancement. Good luck~");
        }
        public static bool Test1(INpcHost self, GameCharacter target, Tracks track)
        {
            var qr = target.QuestRecord;
            var val = qr.Get(7500);

            bool isSecondJob = target.Job >= ((int)track * 100 + 10) && target.Job < ((int)track * 100 + 99);
            if (val == "s" && isSecondJob)
            {
                Test1Start(self, target, track);
            }
            else if (val == "p1")
            {
                Test1Complete(self, target, track);
            }
            else if (val == "p2")
            {
                Test1LostLetter(self, target, track);
            }
            else
            {
                return false;
            }
            return true;
        }
    }

    [Script("fighter")]
    class fighter : INpcScript
    {
        dynamic v2, v, nSec, nRet, ret;
        public void Run(INpcHost self, GameCharacter target)
        {
            var inventory = target.Inventory;

            if (Job3.Test1(self, target, Tracks.Warrior)) return;

            if (target.Job == 100)
            {
                if (target.Level >= 30)
                {
                    if (inventory.ItemCount(4031008) >= 1) self.Say("Haven't found the person yet? Look for #b#p1072000##k who is located around #b#m102020300##k near #m102000000#. Give him the letter and maybe he'll tell you what you need to do.");
                    else if (inventory.ItemCount(4031012) >= 1)
                    {
                        var jobs = new short[] { Constants.Fighter.ID, Constants.Page.ID, Constants.Spearman.ID };
                        self.Say("OHH... you came back safe and sound! I knew you'd breeze through... I'll admit you are a strong, formidable warrior... alright, I'll make you an even stronger Warrior then you already are right now... But before THAT... you need to choose one of the three paths that you'll be given... it isn't going to be easy, so if you have any questions, feel free to ask.");

                        if (!Job2.AskJobDescriptionMenu(self, jobs))
                        {
                            v2 = self.AskMenu($"Hmmmm, have you made up your mind? Then choose the 2nd job advancement of your liking.{Job2.JobMenu(jobs)}");
                            short jobId = jobs[v2];
                            var jobName = Job2.JobStrings[jobId].name;
                            short incMMP = 0;
                            short incMHP = 0;
                            string jobDescription = null;
                            if (v2 == 0)
                            {
                                jobDescription = "A Fighter strives to become the strongest of the strong, and never stops fighting. Don't ever lose that will to fight, and push forward 24/7.";
                                incMHP = (short)Rand32.NextBetween(300, 350);
                            }
                            else if (v2 == 1)
                            {
                                jobDescription = "Pages have the intelligence and bravery of a Warrior... I hope you follow the right path with the right mindset...";
                                incMMP = (short)Rand32.NextBetween(100, 150);
                            }
                            else if (v2 == 2)
                            {
                                jobDescription = "The Spearman uses the power of darkness to strike down enemies, always in the shadows... believe in yourself and your incredible power as you go on your journey...";
                                incMMP = (short)Rand32.NextBetween(100, 150);
                            }

                            if (Job2.Advance(1072000, $"So you want to make the 2nd job advancement as a #b{jobName.Item1}#k? Once you make that decision you can't go back and choose another job... do you still wanna do it?", self, target, jobId, incMMP, incMHP, (2, 4), (4, 4)))
                            {
                                self.Say($"Alright! You have now become a #b{jobName.Item1}#k! {jobDescription} I'll help you become even stronger than you already are.");
                                self.Say($"I have just given you a book that gives you the list of skills you can acquire as a {jobName.Item1}. In that book, you'll find a bunch of skills a {jobName.Item1} can learn. Your use and etc. inventories have also been expanded with an additional row of slots also available. Your max {(incMMP > 0 ? "MP" : "HP")} has also been increased...go check and see for it yourself.");
                                self.Say(Job2.GivenSp);
                                self.Say($"{jobName.Item2} have to be strong. But remember that you can't abuse that power and use it on a weakling. Please use your enormous power the right way, because...for you to use that the right way, that is much harder than just getting stronger. Find me after you have advanced much further.");
                            }
                        }
                    }
                    else
                    {
                        nSec = self.AskYesNo("Wow, you've really grown up! You're not small and weak anymore... Now I can feel your Warrior presence! Impressive... so what do you think? Do you want to become stronger than you already are? Just do a simple test! Accepted?");
                        if (nSec == 0) self.Say(Job2.StartDeny);
                        else if (nSec == 1)
                        {
                            self.Say("Good thinking. You are strong, don't get me wrong, but there is still a need to test your strength and see if your power is real. The test isn't too hard, you'll do just fine... Here, take this letter. Don't lose it.");
                            ret = inventory.Exchange(0, 4031008, 1);
                            if (ret == 0) self.Say("Hmm... I can't give you the letter because you don't have enough space in your etc. inventory. Come back after you've cleared a space or two in your inventory, as you can only take the test with the letter.");
                            else self.Say("Deliver this letter to #b#p1072000##k who may be close to #b#m102020300##k near #m102000000#. He's replacing me as instructor as I've been busy around here. Give him the letter and he will administer the test on my behalf. More information will be passed directly by him to you. Best of luck to you.");
                        }
                    }
                }
                else
                {
                    v = self.AskMenu("Oh, you have a question? \r\n#b#L0#What are the general characteristics of a Warrior?#l\r\n#L1#What are the weapons of a Warrior?#l\r\n#L2#What are the armors of a Warrior?#l\r\n#L3#What skills are available for a Warrior?#l");
                    if (v == 0)
                    {
                        self.Say("Let me explain the role of the Warrior. Warriors possess incredible physical strength and power. They also know how to defend themselves against monster attacks, so they are the best to fight in close combat with monsters. With a high level of health, you won't die easily.");
                        self.Say("However, to attack the monsters accurately, you'll need a good dose of DEX, so don't just focus on improving your STR. If you want to grow quickly, I recommend you face stronger monsters.");
                    }
                    else if (v == 1)
                    {
                        self.Say("Let me explain the weapons a Warrior uses. The use weapons that allow them to slash, stab and attack. You will not be able to use weapons such as bows and projectile weapons. Much less small staves.");
                        self.Say("The most common weapons are swords, maces, polearms, spears, axes, and so on... Every weapon has its advantages and disadvantages, so examine them well before choosing one of them. For now, try to use the ones with a high attack level.");
                    }
                    else if (v == 2)
                    {
                        self.Say("Let me explain the armor a Warrior wears. Warriors are strong and have a lot of health, so they can wear heavy and resistant armor. They aren't very pretty... but they serve their purpose well: Being the best armor.");
                        self.Say("Shields, in particular, are perfect for Warriors. Keep in mind, however, that you cannot use a shield if you are wielding a two-handed weapon. I know it will be a tough decision...");
                    }
                    else if (v == 3)
                    {
                        self.Say("The abilities available to Warriors are aimed at their incredible physical strength and power. The ones that improve melee combat are the ones that will help you the most. There is also a skill that allows you to recover your HP. You better become an expert on it.");
                        self.Say("The two attack skills available are #b#q1001004##k and #b#q1001005##k. #q1001004# is the one that deals huge damage to a single enemy. You will be able to hone this skill right from the start.");
                        self.Say("On the other hand, #q1001005# doesn't do much damage, but attacks multiple enemies in an area at once. You can use it only when you have already upgraded #q1001004# once. You decide.");
                    }
                }
            }
            else if (target.Job == 0)
            {
                self.Say("Do you want to become a Warrior? You need to meet certain criteria for this. #bYou must be at least level 10, with at least 35 STR#k. We will see...");
                if (target.Level > 9 && target.STR > 34)
                {
                    nRet = self.AskYesNo("You definitely have the potential to be a Warrior. You may not be there yet, but I can already see a Warrior in you. What do you think? Do you want to become a Warrior?");
                    if (nRet == 0) self.Say("Really? Need more time to think it through? Make yourself at home... it's not something you should do lightly anyway. Come talk to me when you make your decision.");
                    else if (nRet == 1)
                    {
                        self.Say("From now on, you are a Warrior! Keep pushing... I'm going to improve your skills a little bit, in the hope that you'll keep training to be stronger than you already are. Zaaaaaaz!!");
                        target.SetJob(100);
                        var incval = (short)Rand32.NextBetween(200, 250);
                        target.IncMHP(incval, 0);
                        target.IncSP(1, 0);
                        inventory.IncSlotCount(1, 4);
                        inventory.IncSlotCount(2, 4);
                        inventory.IncSlotCount(3, 4);
                        inventory.IncSlotCount(4, 4);
                        self.Say("You are much stronger now. Also, all your inventories have extra slots. A whole row, to be exact. You can check. I just gave you some #bSP#k. When you open the #bskills window#k in the lower right corner of the screen, you will see the skills you can learn using SP. One caveat: You won't be able to increase them all at once. There are also those that will become available only after you learn some skills first.");
                        self.Say("One more warning. After choosing your class, try to stay alive as long as you can. If you die, you will lose experience points. You don't want to lose your hard-earned experience points, do you? That's all I can teach you... From now on, you'll have to try harder and harder to become better and better. Come see me when you realize you're feeling more empowered than you are now.");
                        self.Say("Oh, and... If you have any questions about being a Warrior, just come and ask me. I don't know EVERYTHING, but I will help you with everything I know. Until then...");
                    }
                }
                else self.Say("I don't think you have what it takes to be a Warrior yet. You need to train a lot to become one or you won't be able to handle the situation. Become much stronger and then come find me.");
            }
            else if (target.Job == 110) self.Say("Ahhh! It's you! What do you think? How is the life of a Fighter? You... look a lot stronger than before! I hope you keep getting stronger.");
            else if (target.Job == 120) self.Say("Ahhh... it's you! What do you think? How is the life of a Page? I know you are still an apprentice, but soon your training will end and you will be knighted!");
            else if (target.Job == 130) self.Say("Ahhh... it's you! What do you think? How is the life of a Spearman? Keep training with dedication, because one day you will become an unparalleled knight...");
            else if (target.Job == 111) self.Say("Ahhh... You finally became a #bCrusader#k... I knew you wouldn't let me down. So what do you think of life as a Crusader? Please dedicate yourself and train even more.");
            else if (target.Job == 121) self.Say("Ahhh... You finally became a #bKnight#k... I knew you wouldn't let me down. So what do you think of life as a Knight? Please dedicate yourself and train even more.");
            else if (target.Job == 131) self.Say("Ahhh... You finally became a #bDragon Knight#k... I knew you wouldn't let me down. So what do you think of life as a Dragon Knight? Please dedicate yourself and train even more.");
            else self.Say("What a magnificent physique! What incredible power! Warriors are the best!!!! What do you think? Do you want to rank up as a warrior??");
        }
    }
    [Script("magician")]
    class magician : INpcScript
    {
        dynamic v2, v, nSec, nRet, ret;
        public void Run(INpcHost self, GameCharacter target)
        {
            var inventory = target.Inventory;
            if (Job3.Test1(self, target, Tracks.Magician)) return;

            if (target.Job == 200)
            {
                if (target.Level >= 30)
                {
                    if (inventory.ItemCount(4031009) >= 1) self.Say("Still haven't seen him? Go find #b#p1072001##k that is close to #b#m101020000##k near #m101000000#... give him this letter and he will tell you what you have to do...");
                    else if (inventory.ItemCount(4031012) >= 1)
                    {
                        var jobs = new short[] { Constants.FPWizard.ID, Constants.ILWizard.ID, Constants.Cleric.ID };
                        self.Say("You got back here safely. Well done. I knew you'd pass the tests very easily...alright, I'll make you much stronger now. Before that, though...you need to choose one of the three paths that will be given to you. It will be a tough decision for you to make, but...if you have any questions about it, feel free to ask.");
                        if (!Job2.AskJobDescriptionMenu(self, jobs))
                        {
                            v2 = self.AskMenu("Now, have you made up your mind? Please select your occupation for your 2nd job advancement.\r\n#b#L0#The Wizard of Fire and Poison#k#l\r\n#b#L1#The Wizard of Ice and Lightening#k#l\r\n#b#L2#Cleric#k#l");
                            var jobName = Job2.JobStrings[jobs[v2]].name;
                            short jobId = jobs[v2];
                            short incMMP = (short)Rand32.NextBetween(450, 500);
                            (byte, byte) slots = (4, 4);
                            string jobDesc = null;
                            string jobDesc2 = null;
                            if (v2 == 0 || v2 == 1)
                            {
                                jobDesc = "Wizards use high intelligence and the power of nature all around us to take down the enemies...please continue with your studies, for one day I may make you much more powerful with my own power...";
                                jobDesc2 = "The Wizards have to be strong. But remember that you can't abuse that power and use it on a weakling. Please use your enormous power the right way, because...for you to use it the right way, that is much harder than just getting stronger. Find me after you have advanced much further. I'll be waiting for you...";
                            }
                            else if (v2 == 2)
                            {
                                jobDesc = "Clerics breathe life into every living organism with their unwavering faith in God. Never stop working on your faith...then one day, I'll help you become much more powerful...";
                                jobDesc2 = "The Cleric needs faith more than anything else. They keep their faith in God and treats all individuals with the respect and dignity they deserve. Keep on trying and one day you will have even more magical and religious power... right... look for me after you have made more pilgrimages. I will be waiting for you...";
                            }

                            if (Job2.Advance(1072001, $"So you want to make the 2nd job advancement as the #b{jobName.Item1}#k? Once you have made the decision, you won't be able to go back and change your job anymore...are you still sure about the decision?", self, target, jobId, incMMP, 0, slots))
                            {
                                self.Say($"Alright, you have now become the #b{jobName.Item1}#k... {jobDesc}");
                                self.Say($"I have just given you a book that gives you the list of skills you can acquire as the {jobName.Item1}...I've also expanded your etc. inventory by adding another row to it. Your maximum MP has gone up, too. Go see for it yourself.");
                                self.Say(Job2.GivenSp);
                                self.Say(jobDesc2);
                            }
                        }
                    }
                    else
                    {
                        nSec = self.AskYesNo("Hmmm...you have grown quite a bit since last time. You\r\nlook much different from before, where you looked weak\r\nand small...instead now I can definitely feel you presence\r\nas the Magician...so...what do you think? Do you want to\r\nget even stronger than you are right now? Pass a simple\r\ntest and I can do just that for you...do you want to do it?");
                        if (nSec == 0) self.Say(Job2.StartDeny);
                        else if (nSec == 1)
                        {
                            self.Say("Good...you look strong, alright, but I need to see if it is for\r\nreal. The test isn't terribly difficult, and you should be able\r\nto pass it. Here, take my letter first. Make sure you don't\r\nlose it.");
                            ret = inventory.Exchange(0, 4031009, 1);
                            if (ret == 0) self.Say("I believe you do not have enough space in your inventory to receive my letter. Free up space in your etc. inventory and talk to me again. After all, you will only be able to take the test with the letter.");
                            else self.Say("Please get this letter to #b#p1072001##k around #b#m101020000##k near #m101000000#. He's doing the role of\r\nan instructor in place of me ... Get him the letter and he'll\r\ntest you in place of me. He'll give you all the details about it.\r\nBest of luck to you...");
                        }
                    }
                }
                else
                {
                    v = self.AskMenu("Any questions about how to be a Magician?\r\n#b#L0#What are the basic characteristics of a Magician?\r\n#L1#What are the weapons of a Magician?\r\n#L2#What are the armors of a Magician?\r\n#L3#What are the skills available for a Magician?#l");
                    if (v == 0)
                    {
                        self.Say("I'll tell you more about being a Magician. Magicians use very high levels of magic and intelligence. They can use the power of nature around them to kill enemies, but they are very weak in melee combat. Their health isn't high either, so be careful and avoid dying by any means.");
                        self.Say("The fact that you are able to attack monsters from a distance will help you a lot. Try to improve your INT level if you want to attack your enemies with accurate spells. The higher your intelligence, the better you will be at handling your magic...");
                    }
                    else if (v == 1)
                    {
                        self.Say("I'll tell you more about the weapons of a Magician. It really doesn't mean much to a Magician to attack their opponents with weapons. Magicians lack strength and dexterity, so you might have a hard time defeating even a snail.");
                        self.Say("Magic powers are another story. Magicians use maces, staffs and wands. Maces are good for, well, strength attacks, but... I wouldn't recommend it for a Magician, period.");
                        self.Say("In fact, staves and wands are the preferred weapons. They have special magical powers and therefore improve the Magician's performance. It's a good idea to carry a weapon with a lot of magical power...");
                    }
                    else if (v == 2)
                    {
                        self.Say("I'll tell you more about the armor of a Magician. Honestly, Magicians don't have a lot of armor, as they have little physical strength and health. Their defense skills aren't good either, so I don't know if it will help at all...");
                        self.Say("Some armor, however, has the ability to eliminate magic power, so it can protect you from magical attacks. They won't help much, but it's still better than not using them at all... so be sure to buy them if you have time...");
                    }
                    else if (v == 3)
                    {
                        self.Say("The skills available to Magicians use the high levels of intelligence and magic that Magicians possess. Also available are Magic Guard and Magic Armor, which prevent low health Magicians from dying.");
                        self.Say("The attack skills are #b#q2001004##k and #b#q2001005##k. First of all, #q2001004# is a skill that deals a lot of damage to the opponent with minimal use of MP.");
                        self.Say("#q2001005#, on the other hand, uses a lot of MP to attack the opponent TWICE. But you can only use this skill when #q2001004# has at least one upgrade. Do not forget that. The decision is up to you...");
                    }
                }
            }
            else if (target.Job == 0)
            {
                self.Say("Do you want to be a Magician? You need to meet some requirements in order to do so. You need to be at least at\r\n#bLevel 8, and INT 20#k. Let's see if you have what it takes to become a Magician...");
                if (target.Level > 7 && target.INT > 19)
                {
                    nRet = self.AskYesNo("You definitely have the look of a Magician. You may not be there yet, but I can see the Magician in you... what do you think? Do you want to become the Magician?");
                    if (nRet == 0) self.Say(Job.AdvanceDeny);
                    else if (nRet == 1)
                    {
                        self.Say("Alright, you're a Magician from here on out, since I, #p1032001#, the head Magician, allow you so. It isn't\r\nmuch, but I'll give you a little bit of what I have...");
                        target.SetJob(200);
                        var incval = (short)Rand32.NextBetween(100, 150);
                        target.IncMMP(incval, 0);
                        target.IncSP(1, 0);
                        self.Say("You have just equipped yourself with much more magical power. Please keep training and make yourself much\r\nbetter...I'll be watching you from here and there...");
                        self.Say("I just gave you a little bit of #bSP#k. When you open up the #bSkill menu#k on the lower right corner of the screen, there are skills you can learn by using SP's. One warning, though: You\r\ncan't raise it all together all at once. There are also skills\r\nyou can acquire only after having learned a couple of skills first.");
                        self.Say("One more warning. Once you have chosen your job, try to stay alive as much as you can. Once you reach that level, when you die, you will lose your experience level. You wouldn't want to lose your hard-earned experience points,\r\ndo you?");
                        self.Say("OK! This is all I can teach you. Go to places, train and\r\nbetter yourself. Find me when you feel like you've done all you can, and need something interesting. I'll be waiting for you here...");
                        self.Say("Oh, and... if you have questions about being the Magician, feel free to ask. I don't know EVERYTHING, per se, but I'll help you out with all that I know of. Til then...");
                    }
                }
                else self.Say("You need to train more to be a Magician. Therefore, you have to strive to become more powerful than you already are. Come back when you're stronger.");
            }
            else if (target.Job == 210) self.Say("Ahhh... it's you... what do you think of life as a Wizard? You... seem pretty comfortable with those flaming arrows now... please dedicate yourself and train even harder.");
            else if (target.Job == 220) self.Say("Ahhh... it's you... what do you think of life as a Wizard? You... seem to be able to handle ice and lightening with ease... please dedicate yourself and train even harder.");
            else if (target.Job == 230) self.Say("Ahhh... it's you... what do you think of life as a Cleric? You... seem to be able to handle holy magic with ease... please dedicate yourself and train even more.");
            else if (target.Job == 211 || target.Job == 221) self.Say("Ahhh... You finally became a #bMage#k... I knew you wouldn't let me down. So what do you think of life as a Mage? Please dedicate yourself and train even more.");
            else if (target.Job == 231) self.Say("Ahhh... You finally became a #bPriest#k... I knew you wouldn't let me down. So what do you think of life as a Priest? Please dedicate yourself and train even more.");
            else self.Say("Would you like to have the power of nature itself in your hands? It may be a long and difficult road, but you will surely receive a reward in the end, reaching the pinnacle of the magician's art...");
        }
    }
    [Script("bowman")]
    class bowman : INpcScript
    {
        dynamic info, inven, v2, nPSP, valm, valh, ret1, v1, v, rank, incval2, nSec, esTime, ret, v0, cTime, qr, mJob, eeTime, cJob, inventory, ret2, val, quest, nRet, incval1;

        public void Run(INpcHost self, GameCharacter target)
        {
            inventory = target.Inventory;
            if (Job3.Test1(self, target, Tracks.Bowman)) return;

            if (target.Job == 300)
            {
                if (target.Level >= 30)
                {
                    if (inventory.ItemCount(4031010) >= 1) self.Say("You still haven't met her, right? Please find #b#p1072002##k around #b#m106010000##k near #m100000000#. Get her this letter and she'll let you know what to do in detail");
                    else if (inventory.ItemCount(4031012) >= 1)
                    {
                        var jobs = new[] { Constants.Hunter.ID, Constants.Crossbowman.ID };

                        self.Say("Haha...I knew you'd breece through that test. I'll admit, you're a great bowman. I'll make you much stronger than you're right now. Before that, however...you'll need to choose one of the two paths given to you. It'll be a difficult decision for you to make, but...if there's any questions to ask, please do so.");

                        if (!Job2.AskJobDescriptionMenu(self, jobs))
                        {
                            v2 = self.AskMenu($"Now... have you made up your mind? Please choose the job you'd like to select for your 2nd job advancement.{Job2.JobMenu(jobs)}");
                            short jobid = jobs[v2];
                            var job = Job2.JobStrings[jobid];
                            short incMHP = (short)Rand32.NextBetween(300, 350);
                            short incMMP = (short)Rand32.NextBetween(150, 200);
                            (byte, byte) slots = (4, 4);

                            if (Job2.Advance(1072002, $"So you want to make the second job advancement as the #b{job.name.Item1}#k? You know you won't be able to choose a different job for the 2nd job advancement once you make your decision here, right?", self, target, jobid, incMMP, incMHP, slots))
                            {
                                self.Say($"Alright, from here on out, you are a #b{job.name.Item1}#k. {job.name.Item2} are the intelligent bunch with incredble vision, able to pierce the arrow through the heart of the monsters with ease...please train yourself each and everyday. We'll help you become even stronger than you already are.");
                                self.Say($"I have just given you a book that gives you the list of skills you can acquire as a {job.name.Item1.ToLower()}. Also your etc. inventory has expanded by adding another row to it. Your max HP and MP have increased, too. Go check and see for it yourself.");
                                self.Say(Job2.GivenSp);
                                self.Say($"{job.name.Item2} need to be strong. But remember that you can't abuse that power and use it on a weakling. Please use your enormous power the right way, because...for you to use it the right way, that is much harder than just getting stronger. Please find me after you have advanced much further. I'll be waiting for you.");
                            }
                        }
                    }
                    else
                    {
                        nSec = self.AskYesNo("Hmmm...you have grown a lot since I last saw you. I don't see the weakling I saw before, and instead, look much more like a bowman now. Well, what do you think? Don't you want to get even more powerful than that? Pass a simple test and I'll do just that for you. Do you want to do it?");
                        if (nSec == 0) self.Say(Job.AdvanceDeny);
                        else if (nSec == 1)
                        {
                            self.Say("Good decision. You look strong, but I need to see if you really are strong enough to pass the test. It's not a difficult test, so you'll do just fine. Here, take my letter first...make sure you don't lose it!");
                            ret = inventory.Exchange(0, 4031010, 1);
                            if (ret == 0) self.Say("Hmm... your inventory looks full... free up some space in your etc. inventory and talk to me again. Without the letter, you won't be able to take the test.");
                            else self.Say("Please get this letter to #b#p1072002##k who's around #b#m106010000##k near #m100000000#. She's taking care of the job of an instructor in place of me. Give her the letter and she'll test you in place of me. Best of luck to you.");
                        }
                    }
                }
                else
                {
                    v = self.AskMenu("Do you have any questions about life as a Bowman? \r\n#b#L0#What are the basic characteristics of a Bowman?#l\r\n#L1#What are the weapons of a Bowman?#l\r\n#L2#What are the armors of a Bowman?#l\r\n#L3#What are the skills of a Bowman?#l");
                    if (v == 0)
                    {
                        self.Say("Here's what it's like to be a Bowman. The Bowman has a decent amount of health and strength. Their most important skill is DEX. They don't have a lot of health, so avoid close range combat if possible.");
                        self.Say("The main advantage is that you will be able to attack from afar, avoiding the close range attacks made by the monsters. Furthermore, with your high dexterity value, you will be able to dodge many close range attacks as well. The higher the DEX, the more damage you can do.");
                    }
                    else if (v == 1)
                    {
                        self.Say("I will explain to you the weapons of a Bowman. Instead of using weapons to attack or slash their opponents, they use ranged weapons like bows and crossbows to kill monsters. Both have their advantages and disadvantages.");
                        self.Say("Bows are not as powerful as crossbows, but they are much faster in an attacking situation. Crossbows, on the other hand, are more powerful but slower. It's going to be hard to make a decision...");
                        self.Say("You'll get arrows and crossbows from the monsters, so it's important that you hunt them often. But it won't be easy. Don't miss a single day of training and success will come to you...");
                    }
                    else if (v == 2)
                    {
                        self.Say("I will explain to you the armors of a Bowman. They need to move quickly, so it won't do any good to use heavy and complex armor. Garments with long, cumbersome cords are out of the question.");
                        self.Say("If you wear the massive, tough armor of the Warriors, you'll soon find yourself surrounded by enemies. Equip yourself with simple, comfortable armor that fits you perfectly and does the job. It will help you a lot when hunting monsters.");
                    }
                    else if (v == 3)
                    {
                        self.Say("The skills available to bowmen are those that make good use of their high accuracy and dexterity. It is essential that the Bowman acquire skills that allow them to attack enemies with precision.");
                        self.Say("There are two types of attack skills for the bowman: #b#q3001004##k and #b#q3001005##k. #q3001004# is a good and simple skill that allows you to deal a lot of damage to the enemy with minimal use of MP.");
                        self.Say("In turn, #q3001005# allows you to attack the enemy twice using a considerable amount of MP. You can acquire this skill only after making at least 1 upgrade in #q3001004#. Do not forget that. Whichever you choose, become an expert in the skill.");
                    }
                }
            }
            else if (target.Job == 0)
            {
                self.Say("So you wish to become an Archer??? Well... you need to meet certain requirements for that... at least #bLevel 10 and DEX 25#k. Let's see... hmm...");
                if (target.Level > 9 && target.DEX > 24)
                {
                    nRet = self.AskYesNo("You are qualified. You have a great pair of eyes to see the real monsters and the necessary skills to put an arrow through them... we needed someone like that. Do you wish to become a Bowman?");
                    if (nRet == 0) self.Say(Job.AdvanceDeny);
                    else if (nRet == 1)
                    {
                        self.Say("Right! You are now a Bowman because I say so... haha. Here's some of my power for you... Raaaa!");
                        inventory.IncSlotCount(1, 4);
                        inventory.IncSlotCount(2, 4);
                        target.SetJob(300);
                        valh = Rand32.NextBetween(100, 150);
                        valm = Rand32.NextBetween(30, 50);
                        target.IncMHP(valh, 0);
                        target.IncMMP(valm, 0);
                        target.IncSP(1, 0);
                        self.Say("Right, I've added some slots to your equip and etc. inventories. You've also become a lot stronger. Train hard and one day you will reach the pinnacle of archery skill. I will be watching from afar. Work hard.");
                        self.Say(Job2.GivenSp);
                        self.Say("One more warning. Once you've chosen your job, try to stay alive as long as you can. If you die, you lose all your experience level. You don't want to lose your hard-earned experience points, do you?");
                        self.Say("OK! That's all I can teach you. Go places, train and become even better. Come to me when you think you've done all you can and need something interesting. I will be waiting for you.");
                        self.Say("Oh, and... if you have any questions about being an Bowman, feel free to ask. I don't know everything about being a Bowman, but I'll answer what I can. Until then...");
                    }
                }
                else self.Say("You need to train more. Don't think that being Bowman is easy...");
            }
            else if (target.Job == 310) self.Say("Ahhh... it's you. What do you think? What is the life of a Hunter like? You seem to be able to handle the explosive arrows with ease... keep pushing.");
            else if (target.Job == 320) self.Say("Ahhh... it's you. What do you think? What is the life of a Crossbowman like? Watch out for those spinning arrows. You don't want to hurt innocent people with them.");
            else if (target.Job == 311) self.Say("Ahhh... you must have passed all the tests and become a #bRanger#k! I knew you could do it. What do you think? It won't be long before you rise to the top and become the best bowman in the area.");
            else if (target.Job == 321) self.Say("Ahhh... you must have passed all the tests and become a #bSniper#k! I knew you could do it. What do you think? It won't be long before you rise to the top and become the best bowman in the area.");
        }
    }
    // ľľŔű ŔüÁ÷ 
    [Script("rogue")]
    class rogue : INpcScript
    {
        dynamic info, inven, v2, nPSP, valm, valh, ret1, v1, v, rank, incval2, nSec, esTime, ret, v0, cTime, mJob, eeTime, inventory, ret2, quest, nRet, incval1;
        public void Run(INpcHost self, GameCharacter target)
        {
            inventory = target.Inventory;

            if (Job3.Test1(self, target, Tracks.Thief)) return;

            if (target.Job == 400)
            {
                if (target.Level >= 30)
                {
                    if (inventory.ItemCount(4031011) >= 1) self.Say("You still haven't seen him, have you? Go find #b#p1072003##k who is around #b#m102040000##k near #m103000000#. Give him this letter and he will tell you what to do.");
                    else if (inventory.ItemCount(4031012) >= 1)
                    {
                        self.Say("Hmm... you came back in one piece. I knew that test was going to be too easy for you. I admit that you are a great thief. Now... I'm going to make you even more powerful than you already are. But before that... you will have to choose one of two paths. It will be a difficult decision, but... you can ask me any questions.");

                        var jobs = new short[] { Constants.Assassin.ID, Constants.Bandit.ID };

                        if (Job2.AskJobDescriptionMenu(self, jobs)) return;

                        v2 = self.AskMenu($"Okay... have you made your decision yet? Choose the job you would like to select for your 2nd job advancement.{Job2.JobMenu(jobs)}");
                        var jobId = jobs[v2];
                        var job = Job2.JobStrings[jobId];
                        short incMHP = (short)Rand32.NextBetween(300, 350);
                        short incMMP = (short)Rand32.NextBetween(150, 200);
                        (byte, byte) slots = (2, 4);
                        string jobDescription;
                        
                        if (v2 == 0)
                        {
                            jobDescription = "Assassins appreciate the shadows and darkness, waiting for the right moment to pierce the enemy's heart with a dagger in a sudden and fast way... don't forget to train.";
                        }
                        else
                        {
                            jobDescription = "Bandits have quick hands and feet to subdue enemies. Don't stop training.";
                        }

                        if (Job2.Advance(1072003, $"So you want to go up to the 2nd job level as \n#b{job.name.Item1}#k? Once you've made your decision, you can't go back and change your mind. You're SURE about that, right?", self, target, jobId, incMMP, incMHP, slots))
                        {
                            self.Say($"Okay, from now on you will be a #b{job.name.Item1}#k. {jobDescription} I'm going to make you even more powerful than you already are!");
                            self.Say($"I've given you a book that contains the list of skills you can acquire as a {job.name.Item1}. I've also added a new row to your use inventory and improved your maximum HP and MP... check it out.");
                            self.Say(Job2.GivenSp);
                            self.Say($"{job.name.Item1} have to be strong. But remember that you cannot abuse this power and use it on a weaker being. Use your great power in the right way, because... using it in the right way is much more difficult than just getting stronger. Look for me after you've gone far enough.");
                        }
                    }
                    else
                    {
                        nSec = self.AskYesNo("Hummm... vocę parece bem mais forte. Vocę se livrou daquele visual medíocre e fraco e se parece muito mais com um gatuno agora. Bom, o que vocę acha? Deseja se tornar ainda mais #Gpoderoso:poderosa#? É só fazer um simples teste. Vocę aceita?");
                        if (nSec == 0) self.Say("Realmente...? Tornar-se mais forte rapidamente lhe ajudará muito durante sua jornada. Venha me ver se mudar de idéia. Vou torná-lo ainda mais #Gpoderoso:poderosa# do que já é.");
                        else if (nSec == 1)
                        {
                            self.Say("Bem pensado. Mas preciso saber se vocę é tăo forte quanto parece. Năo é um teste difícil, vocę vai conseguir passar. Primeiro, pegue esta carta... năo a perca.");
                            ret = inventory.Exchange(0, 4031011, 1);
                            if (ret == 0) self.Say("Bem... parece que năo há espaço nas suas reservas para esta carta. Libere espaço no seu inventário de Etc. e venha falar comigo. Vocę poderá fazer o teste somente com a carta.");
                            else self.Say("Leve esta carta ao #b#p1072003##k que está perto de #b#m102040000##k próximo a #m103000000#. Ele está me substituindo como instrutor. Entregue-lhe a carta e ele aplicará o teste em meu lugar. Se quiser mais detalhes, peça diretamente a ele. Desejo-lhe boa sorte.");
                        }
                    }
                }
                else
                {
                    v = self.AskMenu("Do you want to know something about Thieves?\r\n#b#L0#What are the basic characteristics of a Thief?#l\r\n#L1#What are the weapons of a Thief?#l\r\n# L2#What are the armors of a Thief?#l\r\n#L3#What are the skills available to a Thief?#l");
                    if (v == 0)
                    {
                        self.Say("Let me explain what it means to be a Thief. The Thieves have just enough stamina and strength to survive. We do not recommend that you train your strength like the Warriors. You need LUK and DEX...");
                        self.Say("If you increase your level of luck and dexterity, the damage done to enemies will also increase. Thieves also set themselves apart from other individuals by utilizing throwing weapons such as ninja stars and throwing knives. With their high luck, they are also able to dodge various attacks.");
                    }
                    else if (v == 1)
                    {
                        self.Say("Thieves use these weapons. They have just the right amount of intelligence and power... their strong point is their quick movement and even faster mind...");
                        self.Say("For this reason, we normally use daggers or throwing weapons. Daggers are basically small swords, but they are very fast, allowing you to make multiple attacks. If you're in a melee fight, use the dagger to eliminate the enemy quickly before they can react to your attack.");
                        self.Say("For throwing weapons, ninja stars and throwing knives are available. But you won't be able to use them alone. Go to the weapon shop in #m103000000# and they will sell a claw called #t1472000#. Equip the claw and you'll be able to throw the ninja star found in your Use inventory.");
                        self.Say("Just as important as choosing the right Claw is selecting the proper ninja star type. Want to know where to get those stars? Go check out the armor shop around this town... there will probably be someone who specializes in them...");
                    }
                    else if (v == 2)
                    {
                        self.Say("Let me explain what armor the Thieves use. Thieves value speed, so you'll need clothes that fit you perfectly. But they don't need chainmail like the Bowmen, as they won't help at all.");
                        self.Say("Rather than flashy clothes or stiff, tough gold plate armor, try to wear simple, comfortable clothes that fit you perfectly and that do the job. It will help a lot when hunting monsters.");
                    }
                    else if (v == 3)
                    {
                        self.Say("For Thieves, there are skills that support the high accuracy and dexterity we possess in general. There is a good variety of skills, from ninja stars to daggers. Choose your weapon carefully, as your abilities will be determined by it.");
                        self.Say("If you're using ninja stars, skills like #b#q4000001##k or #b#q4001344##k are perfect. #q4001344# allows you to throw multiple ninja stars at once, so it will help you a lot when chasing enemies.");
                        self.Say("If using daggers, choose #b#q4001002##k or #b#q4001334##k as your abilities. #q4001334# will actually be a great skill to use as it allows you to attack with a crazy chain of strikes, something that will definitely surprise the enemy.");
                    }
                }
            }
            else if (target.Job == 0)
            {
                self.Say("Want to be a Thief? You will have to meet certain criteria. We can't accept just ANYONE... #bYour level must be at least 10, with DEX greater than 25#k. Let's see...");
                if (target.Level > 9 && target.DEX > 24)
                {
                    nRet = self.AskYesNo("Oh...! You look like someone who could definitely be one of us... all you need is an evil mind and... yes, yes... so what do you think? Want to be a Thief?");
                    if (nRet == 0) self.Say("I see... well, choosing your job is a very important step. But don't you want to live a more fun life? Let me know when you make your decision, okay?");
                    else if (nRet == 1)
                    {
                        self.Say("Okay, from now on, you're one of us! You'll live like a nomad at first, but be patient and soon you'll be living the good life. OK, it's not much, but I'll give you some of my skills... RAAAAA!!");
                        inventory.IncSlotCount(1, 4);
                        inventory.IncSlotCount(4, 4);
                        target.SetJob(400);
                        valh = Rand32.NextBetween(100, 150);
                        valm = Rand32.NextBetween(30, 50);
                        target.IncMHP(valh, 0);
                        target.IncMMP(valm, 0);
                        target.IncSP(1, 0);
                        self.Say("I just created more slots in your equipment and etc. inventories. Also, you're stronger now. By becoming one of us and learning to enjoy life in many ways, you will one day be on top of our world of darkness. I'll be watching your every step. Do not let me down.");
                        self.Say(Job2.GivenSp);
                        self.Say("One more warning. After choosing your class, try to stay alive as long as you can. If you die, you lose all your experience level. You don't want to lose your hard-earned experience points, do you?");
                        self.Say("OK! That's all I can teach you. Go places, train and become even better. Come to me when you think you've done all you can and need something interesting. Then, and only then, will I give you even better experiences...");
                        self.Say("Oh, and... and if you have any questions about being a Thief, feel free to ask. I don't know EVERYTHING, but I'll help you with everything I know. Until then...");
                    }
                }
                else self.Say("Humm... you're just a rookie... I don't think you can stay with us yet... get much stronger and THEN look for me...");
            }
            else if (target.Job == 410) self.Say("It's you. What is the life of a Assassin like? Now I can feel your Assassin's presence... Very good! Keep striving!");
            else if (target.Job == 420) self.Say("It's you. What is the life of a Bandit like? Now I can feel your Bandit's presence... Very good! Keep striving!");
            else if (target.Job == 411) self.Say("Ahhh... it's you. You must have passed the test and become a #bHermit#k! I knew you could do it! What do you think? I see someone who will one day become the BEST thief in the area...");
            else if (target.Job == 421) self.Say("Ahhh... it's you. You must have passed the test and become a Chief Bandit! I knew you could do it! What do you think? I see someone who will one day become the BEST thief in the area...");
            else self.Say("Exploring is fun, and getting stronger is good... but don't you want to enjoy life to the fullest? How about becoming a thief like us and really LIVE life? Sounds good, doesn't it?");
        }
    }
}
