using System.Collections.Generic;
using WvsBeta.Common;
using WvsBeta.Common.Enums;
using WvsBeta.Game;
using WvsBeta.Game.Scripting;
using static WvsBeta.Common.Constants.JobTracks;

namespace WvsBeta.Scripts.Scripts
{
    public static partial class Job2
    {
        private static IDictionary<Tracks, (int letterid, int mapid, string jobName, int jobmaster, int homemap, int outside)> values = new Dictionary<Tracks, (int letterid, int mapid, string jobName, int jobmaster, int homemap, int outside)>()
        {
            { Tracks.Warrior, (ItemEtcIds.DancesWithBalrogLetter, MapIds.WarriorRockyMountain, "Warrior", NpcIds.DancesWithBalrog, MapIds.Perion, MapIds.WestRockyMountainIV) },
            { Tracks.Magician, (ItemEtcIds.GrendelLetter, MapIds.MagicianTreeDungeon, "Magician", NpcIds.Grendel, MapIds.Ellinia, MapIds.ForestNorthOfEllinia) },
            { Tracks.Bowman, (ItemEtcIds.AthenaLetter, MapIds.BowmanAntTunnel, "Bowman", NpcIds.AthenaPierce, MapIds.Henesys, MapIds.RoadToTheDungeon) },
            { Tracks.Thief, (ItemEtcIds.DarkLordLetter, MapIds.ThiefConstructionSite, "Thief", NpcIds.DarkLord, MapIds.KerningCity, MapIds.ConstructionSiteNorth) },
        };
        private static void GoIn(GameCharacter target, int mapid)
        {
            int mapId = mapid + Rand32.NextBetween(0, 2);
            target.ChangeMap(mapId, "");
        }
        public static void Start(INpcHost self, GameCharacter target, Tracks track)
        {
            short jobId = (short)((short)track * 100);
            var value = values[track];
            if (target.Job == jobId && target.Level >= 30)
            {
                var inventory = target.Inventory;
                var nBlack = inventory.ItemCount(ItemEtcIds.BlackMarble);
                if (inventory.ItemCount(value.letterid) >= 1)
                {
                    if (nBlack == 0)
                    {
                        self.Say($"Hmmm...it is definitely a letter from #b#p1032001##k...so you came all the way here to take the test and make the 2nd job advancement as the {value.jobName}. Alright, I'll explain the test to you. Don't sweat it too much, it's not that complicated.");
                        self.Say("I'll send you to a hidden map. You'll see monsters you don't normally see. They look the same like the regular ones, but with a totally different attitude. They neither boost your experience level nor provide you with item.");
                        self.Say("You'll be able to acquire a marble called #b#t4031013##k while knocking down those monsters. It is a special marble made out of their sinister, evil minds. Collect 30 of those, and then go talk to a colleague of mine in there. That's how you pass the test.");
                        var nRet = self.AskYesNo("Once you go inside, you can't leave until you take care of your mission. If you die, your experience level will decrease...so you better really buckle up and get ready...well, do you want to go for it now?");
                        if (nRet == 0) self.Say("I don't think you're qualified for this. Come talk to me when you're much more prepared. There are no portals or stores there, so you'd better be prepared in advance.");
                        else if (nRet == 1)
                        {
                            GoIn(target, value.mapid);
                            self.Say("Defeat the monsters inside, collect 30 #t4031013#, then strike up a conversation with a colleague of mine inside. He'll give you #b#t4031012##k, the proof that you've passed the test. Best of luck to you.");
                        }
                    }
                    else if (nBlack > 0)
                    {
                        var nRet = self.AskYesNo("So you already gave up once. Don't worry, you can always retake the test. Now... do you want to go back there and try one more time?");
                        if (nRet == 0) self.Say("You don't seem very prepared for this. Look for me when you're READY. There are no portals or stores there, so you better be 100% prepared.");
                        else if (nRet == 1)
                        {
                            self.Say("Okay, I'll let you in! I'm sorry, but I'll have to remove all your marbles first. Once inside the map, defeat the monsters and gather 30 #t4031013#. After that, go talk to my colleague who is also there to receive #b#t4031012##k, the proof that you've passed the test. Best of luck to you.");
                            inventory.Exchange(0, ItemEtcIds.BlackMarble, -nBlack);
                            GoIn(target, value.mapid);
                        }
                    }
                }
                else self.Say($"Want to become a more powerful {value.jobName} than you already are? Allow me to take care of this. You seem to be qualified. Go look for #b#p{value.jobmaster}##k of #m{value.homemap}# first.");
            }
            else if (target.Job == jobId && target.Level < 30) self.Say($"Want to become a more powerful {value.jobName} than you already are? Allow me to take care of this. However, you seem a little weak. Train some more, get stronger and then come back here.");
            else if (target.Job > jobId && target.Job < jobId + 100) self.Say($"Ah, you were the one who passed my test the other day! What do you think? Did you manage to become stronger? Good! Now I can definitely feel your {value.jobName} presence...");
        }

        public static void Inside(INpcHost self, GameCharacter target, Tracks track)
        {
            var value = values[track];
            var jobId = (short)track * 100;
            if (target.Job == jobId && target.Level >= 30)
            {
                var inventory = target.Inventory;
                if (inventory.ItemCount(ItemEtcIds.BlackMarble) >= 30)
                {
                    self.Say($"Ohhhhh...you collected all 30 #t4031013#s!! Should have been difficult... Incredible. Alright. You've passed the test,\r\nand for that, I'll reward you with #b#t4031012##k. Take that item and go back to #m{value.homemap}#.");
                    var nBlack = inventory.ItemCount(ItemEtcIds.BlackMarble);
                    var ret = inventory.Exchange(0, ItemEtcIds.BlackMarble, -nBlack, value.letterid, -1, ItemEtcIds.ProofOfHero, 1);
                    if (ret == 0) self.Say($"Hmm...something is wrong... make sure you have 30 #t4031013#, the letter from #b#p{value.jobmaster}##k and an empty slot in your etc. inventory.");
                    else target.ChangeMap(value.outside, "");
                }
                else
                {
                    var nRet = self.AskYesNo("Hmmmm... what's going on? I don't think you've gathered 30 #b#t4031013##k items yet... If you're having trouble with this, you can quit now and try again later. So... do you want to give up and get out of here?");
                    if (nRet == 0) self.Say("That's right! Stop complaining and start collecting the marbles. Come talk to me when you've collected 30 #b#t4031013##k.");
                    else if (nRet == 1)
                    {
                        self.Say("Really...? Okay, I'll let you out. But don't give up. You can always try again, so keep trying. Until then, goodbye...");
                        target.ChangeMap(value.outside, "");
                    }
                }
            }
            else
            {
                self.Say("What? How did you get here?... how strange... well, I'll let you out. This is a very dangerous place. Go away or take more risks.");
                target.ChangeMap(value.outside, "");
            }
        }
    }

    // The job instructor for warriors 
    [Script("change_swordman")]
    class change_swordman : INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            Job2.Start(self, target, Tracks.Warrior);
        }
    }
    // The job instructor for magicians 
    [Script("change_magician")]
    class change_magician : INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            Job2.Start(self, target, Tracks.Magician);
        }
    }
    // The job instructor for the bowman 
    [Script("change_archer")]
    class change_archer : INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            Job2.Start(self, target, Tracks.Bowman);
        }
    }
    // The instructor for the thief job advancement 
    [Script("change_rogue")]
    class change_rogue : INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            Job2.Start(self, target, Tracks.Thief);
        }
    }
    // Warrior Job Instructor 
    [Script("inside_swordman")]
    class inside_swordman : INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            Job2.Inside(self, target, Tracks.Warrior);
        }
    }
    // Magician Job Instructor 
    [Script("inside_magician")]
    class inside_magician : INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            Job2.Inside(self, target, Tracks.Magician);
        }
    }
    // Bowman Job Instructor 
    [Script("inside_archer")]
    class inside_archer : INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            Job2.Inside(self, target, Tracks.Bowman);
        }
    }
    // Thief Job Instructor 
    [Script("inside_rogue")]
    class inside_rogue : INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            Job2.Inside(self, target, Tracks.Thief);
        }
    }
}
