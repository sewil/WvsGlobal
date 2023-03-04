using System.Collections.Generic;
using WvsBeta.Common.Enums;
using static WvsBeta.Common.Constants.JobTracks;

namespace WvsBeta.Common
{
    public static class Strings
    {
        public static string ToCardinal(this int num)
        {
            string suffix;
            if (num < 3)
                suffix = num <= 1 ? "st" : num == 2 ? "nd" : "rd";
            else suffix = "th";
            return num + suffix;
        }
        public static class Gender
        {
            public static string Pronoun(byte gender, bool sub = false, bool pos = false)
            {
                switch (gender)
                {
                    case 0: return pos ? "his" : sub ? "he" : "him";
                    case 1: return pos ? "her" : sub ? "she" : "her";
                    default: return pos ? "their" : sub ? "they" : "them";
                }
            }
        }
        public static class Job
        {
            public static string GetName(Tracks track, bool plur = false)
            {
                switch (track)
                {
                    case Tracks.Beginner: return plur ? "Beginners" : "Beginner";
                    case Tracks.Warrior: return plur ? "Warriors" : "Warrior";
                    case Tracks.Magician: return plur ? "Magicians" : "Magician";
                    case Tracks.Bowman: return plur ? "Bowmen" : "Bowman";
                    case Tracks.Thief: return plur ? "Thieves" : "Thief";
                    case Tracks.Gm: return plur ? "GMs" : "GM";
                    default: return null;
                }
            }
            public static string GetName(short jobid)
            {
                switch (jobid)
                {
                    case JobIds.Crusader: return "Crusader";
                    case JobIds.Knight: return "Knight";
                    case JobIds.DragonKnight: return "Dragon Knight";
                    case JobIds.FPMage: return "Mage of Fire and Poison";
                    case JobIds.ILMage: return "Mage of Ice and Lightning";
                    case JobIds.Priest: return "Priest";
                    case JobIds.Ranger: return "Ranger";
                    case JobIds.Sniper: return "Sniper";
                    case JobIds.Hermit: return "Hermit";
                    case JobIds.ChiefBandit: return "Chief Bandit";
                    default: return null;
                }
            }
        }
        public struct NpcStringData
        {
            public string name;
            public string description;
            public int homeid;
            public byte gender;
            public NpcStringData(string name, string description, int homeid, byte gender)
            {
                this.name = name;
                this.description = description;
                this.homeid = homeid;
                this.gender = gender;
            }
        }
        public static class Npcs
        {
            public static IDictionary<int, NpcStringData> Data = new Dictionary<int, NpcStringData>() {
                { NpcIds.Tylus, new NpcStringData("Tylus", $"I'm #b#p{NpcIds.Tylus}##k, the chief of all warriors, tasked with bringing out the best in warriors who need my guidance.", MapIds.ElNath, 0) },
                { NpcIds.Robeira, new NpcStringData("Robeira", $"I'm #b#p{NpcIds.Robeira}##k, the chief of all wizards, always ready to offer my knowledge and guidance to wizards around the world.", MapIds.ElNath, 1) },
                { NpcIds.Rene, new NpcStringData("Robeira", $"I'm #b#p{NpcIds.Rene}##k, the chief of all bowmen, in charge of bringing out the best in each and every bowman that needs my guidance.", MapIds.ElNath, 1) },
                { NpcIds.Arec, new NpcStringData("Arec", $"I'm #b#p{NpcIds.Arec}##k, the chief of all thieves, ready to share my street knowledge and hard knock life to those willing to listen.", MapIds.ElNath, 0) },
                { NpcIds.DancesWithBalrog, new NpcStringData("Grendel the Really Old", "head of the Warriors", MapIds.Perion, 0) },
                { NpcIds.Grendel, new NpcStringData("Grendel the Really Old", "head of the Magicians", MapIds.Ellinia, 0) },
                { NpcIds.AthenaPierce, new NpcStringData("Athena Pierce", "head of the Bowmen", MapIds.Henesys, 1) },
                { NpcIds.DarkLord, new NpcStringData("Dark Lord", "head of the Thieves", MapIds.KerningCity, 0) },
            };
        }
    }
}
