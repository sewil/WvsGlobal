using WvsBeta.Game.Scripting;
using WvsBeta.Game;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace WvsBeta.Scripts.Scripts
{
    public class Taxi
    {
        protected int mapID;
        protected int fee;
        protected void HandleState(INpcHost self, GameCharacter target, Dictionary<int, int> priceMap, byte state, byte answer)
        {
            bool isBeginner = target.Job == 0;
            int i = 0;
            string priceList = string.Join("\r\n", priceMap.Select((p) => string.Format("#b#L{0}##m{1}# ({2} mesos)#l", i++, p.Key, (isBeginner ? p.Value * 0.1 : p.Value).ToString("N0", new CultureInfo("en-US")))));
            if (state == 0)
            {
                self.SendNext("How's it going? I drive the #p1022001#. If you want to go from town to town safely and fast, then ride our cab. We'll gladly take you to your destination for an affordable price.");
            }
            else if (state == 1)
            {
                if (isBeginner)
                {
                    self.AskMenu("We have a special 90% discount for beginners. Choose your destination, prices vary from place to place. \r\n" + priceList);
                }
                else
                {
                    self.AskMenu("Choose your destination, for fees will change from place to place.\r\n" + priceList);
                }
            }
            else if (state == 2)
            {
                var selected = priceMap.ElementAt(answer);
                mapID = selected.Key;
                fee = isBeginner ? (int)(selected.Value * 0.1) : selected.Value;
                self.AskYesNo("You have nothing else to do here, huh? Would you like to go to #b#m" + mapID + "##k? It will cost #b" + fee.ToString("N0", new CultureInfo("en-US")) + " mesos#k.");
            }
            else if (answer == 1)
            {
                var fRet = target.AddMesos(-fee, false, true);
                if (fRet == -1) self.SendOK("You do not have enough mesos. I'm sorry, but without enough mesos you won't be able to ride the taxi.");
                else target.ChangeMap(mapID);
            }
            else
            {
                self.SendNext("There is a lot to see in this town too. Come back and look for us when you need to go somewhere else.");
                self.Stop();
            }
        }
    }
    [DisplayName("taxi1")]
    public class Taxi1 : Taxi, INpcScript
    {
        Dictionary<int,int> priceMap = new Dictionary<int, int> {
            { 104000000, 1200 },
            { 100000000, 1000 },
            { 101000000, 1000 },
            { 103000000, 800 }
        };
        public void Run(INpcHost self, GameCharacter target, byte state, byte answer, string atringAnswer, int integerAnswer)
        {
            HandleState(self, target, priceMap, state, answer);
        }
    }
    [DisplayName("taxi2")]
    public class Taxi2 : Taxi, INpcScript
    {
        Dictionary<int, int> priceMap = new Dictionary<int, int> {
            { 104000000, 800 },
            { 102000000, 1000 },
            { 101000000, 1000 },
            { 103000000, 1200 }
        };
        public void Run(INpcHost self, GameCharacter target, byte state, byte answer, string atringAnswer, int integerAnswer)
        {
            HandleState(self, target, priceMap, state, answer);
        }
    }
    [DisplayName("taxi3")]
    public class Taxi3 : Taxi, INpcScript
    {
        Dictionary<int, int> priceMap = new Dictionary<int, int> {
            { 104000000, 1000 },
            { 102000000, 800 },
            { 101000000, 1200 },
            { 100000000, 1000 }
        };
        public void Run(INpcHost self, GameCharacter target, byte state, byte answer, string atringAnswer, int integerAnswer)
        {
            HandleState(self, target, priceMap, state, answer);
        }
    }
    [DisplayName("taxi4")]
    public class Taxi4 : Taxi, INpcScript
    {
        Dictionary<int, int> priceMap = new Dictionary<int, int> {
            { 104000000, 1200 },
            { 102000000, 1000 },
            { 100000000, 1000 },
            { 103000000, 1200 }
        };
        public void Run(INpcHost self, GameCharacter target, byte state, byte answer, string atringAnswer, int integerAnswer)
        {
            HandleState(self, target, priceMap, state, answer);
        }
    }
}
