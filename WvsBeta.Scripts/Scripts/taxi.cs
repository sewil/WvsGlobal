using WvsBeta.Game.Scripting;
using WvsBeta.Game;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using WvsBeta.Common.Extensions;

namespace WvsBeta.Scripts.Scripts
{
    public class Taxi
    {
        protected void HandleState(INpcHost self, GameCharacter target, Dictionary<int, int> priceMap)
        {
            bool isBeginner = target.Job == 0;
            int i = 0;
            string priceList = string.Join("\r\n", priceMap.Select((p) => string.Format("#b#L{0}##m{1}# ({2} mesos)#l", i++, p.Key, (isBeginner ? (int)(p.Value * 0.1) : p.Value).Culture())));
            self.Say("How's it going? I drive the #p1022001#. If you want to go from town to town safely and fast, then ride our cab. We'll gladly take you to your destination for an affordable price.");
            int nRet;
            if (isBeginner)
            {
                nRet = self.AskMenu("We have a special 90% discount for beginners. Choose your destination, prices vary from place to place. \r\n" + priceList);
            }
            else
            {
                nRet = self.AskMenu("Choose your destination, for fees will change from place to place.\r\n" + priceList);
            }
            var selected = priceMap.ElementAt(nRet);
            int mapID = selected.Key;
            int fee = isBeginner ? (int)(selected.Value * 0.1) : selected.Value;
            nRet = self.AskYesNo("You have nothing else to do here, huh? Would you like to go to #b#m" + mapID + "##k? It will cost #b" + fee.Culture() + " mesos#k.");
            if (nRet == 1)
            {
                if (target.Inventory.Exchange(-fee) == 0) self.Say("You do not have enough mesos. I'm sorry, but without enough mesos you won't be able to ride the taxi.");
                else target.ChangeMap(mapID);
            }
            else
            {
                self.Say("There is a lot to see in this town too. Come back and look for us when you need to go somewhere else.");
            }
        }
    }
    [Script("taxi1")]
    public class Taxi1 : Taxi, INpcScript
    {
        Dictionary<int,int> priceMap = new Dictionary<int, int> {
            { 104000000, 1200 },
            { 100000000, 1000 },
            { 101000000, 1000 },
            { 103000000, 800 }
        };
        public void Run(INpcHost self, GameCharacter target)
        {
            HandleState(self, target, priceMap);
        }
    }
    [Script("taxi2")]
    public class Taxi2 : Taxi, INpcScript
    {
        Dictionary<int, int> priceMap = new Dictionary<int, int> {
            { 104000000, 800 },
            { 102000000, 1000 },
            { 101000000, 1000 },
            { 103000000, 1200 }
        };
        public void Run(INpcHost self, GameCharacter target)
        {
            HandleState(self, target, priceMap);
        }
    }
    [Script("taxi3")]
    public class Taxi3 : Taxi, INpcScript
    {
        Dictionary<int, int> priceMap = new Dictionary<int, int> {
            { 104000000, 1000 },
            { 102000000, 800 },
            { 101000000, 1200 },
            { 100000000, 1000 }
        };
        public void Run(INpcHost self, GameCharacter target)
        {
            HandleState(self, target, priceMap);
        }
    }
    [Script("taxi4")]
    public class Taxi4 : Taxi, INpcScript
    {
        Dictionary<int, int> priceMap = new Dictionary<int, int> {
            { 104000000, 1200 },
            { 102000000, 1000 },
            { 100000000, 1000 },
            { 103000000, 1200 }
        };
        public void Run(INpcHost self, GameCharacter target)
        {
            HandleState(self, target, priceMap);
        }
    }
    [Script("mTaxi")]
    public class MTaxi : INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            var cJob = target.Job;

            self.Say("Hey! This taxi is for VIP customers only. Instead of simply taking you to cities like regular taxis, we offer much better service worthy of the VIP class. It's a little more expensive, but... for just 10,000 mesos, we'll get you safely to #bAnt Tunnel Park#k.");
            int nRet;
            int fee;
            if (cJob == 0)
            {
                nRet = self.AskYesNo("We have a special 90% discount for beginner. Ant Tunnel is located at the very bottom of the Dungeon, which is in the center of Victoria Island, where #p1061001# is located. Would you like to go there for #b1,000 mesos#k?");
                fee = 1000;
            }
            else
            {
                nRet = self.AskYesNo("The standard rate applies to all non-beginners. Ant Tunnel is located at the very bottom of the Dungeon, which is in the center of Victoria Island, where #p1061001# is located. Would you like to go there for #b10,000 mesos#k?");
                fee = 10000;
            }
            if (nRet == 0)
            {
                self.Say("This town also has a lot to offer. Look for us if and when you feel the need to go to Ant Tunnel Park.");
            }
            else
            {
                if (target.Inventory.Exchange(-fee) == 0)
                {
                    self.Say("It looks like you don't have enough mesos. Sorry, but you won't be able to use this without it.");
                }
                else
                {
                    target.ChangeMap(105070001);
                }
            }
        }
    }
    // Aqua Dolphin's Taxi  ¾ÆÄí¾Æ·ÎµåÀÇ µ¹°í·¡ ÅÃ½Ã·Î »ç°¢Áö´ë °¡´Â °Í 
    [Script("aqua_taxi")]
    class aqua_taxi : INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            var inven = target.Inventory;
            bool discount = target.Job == 0;
            int fee = (int)(1000 * (discount ? 0.1 : 1));

            bool haveTicket = inven.ItemCount(4031242) >= 1;
            string prompt = "The oceans are all connected to each other. To places you cannot go on foot, you can easily go by sea. What do you think about taking the #bDolphin Taxi#k with us today?";
            if (!haveTicket && discount) prompt += " We have a special 90% discount for beginners!";
            string option;
            if (haveTicket) option = $"#kUse #b#t4031242##k to go to #bThe Sharp Unknown#k.";
            else option = $"#kGo to #bThe Sharp Unknown#k after paying {fee.Culture()} mesos.";
            int ret1 = self.AskMenu(prompt, option);
            if (ret1 == 0)
            {
                if (haveTicket)
                {
                    int ret2 = inven.Exchange(0, 4031242, -1);
                    if (ret2 == 0)
                    {
                        self.Say("I don't think you have #b#t4031242##k there with you. There must be a way to get the #b#t4031242##k through Camila of Henesys...");
                        return;
                    }
                }
                else
                {
                    int ret2 = target.IncMoney(-fee, 1);
                    if (ret2 == 0)
                    {
                        self.Say("I don't think you have enough mesos...");
                        return;
                    }
                }
                target.ChangeMap(230030200, "st00");
            }
        }
    }
    ////¹éÃÊ¸¶À» µ¹°í·¡ Å¸°í ¾ÆÄí¾Æ·Îµå·Î 
    //[Script("aqua_taxi2")]
    //class aqua_taxi2 : INpcScript
    //{
    //    public void Run(INpcHost self, GameCharacter target)
    //    {
    //        if (target.Job == 0)
    //        {
    //            var meso = 1000;
    //            var v0 = self.AskYesNo("Você irá para #b#m230000000##k agora? Temos bilhetes especiais com 90% de desconto para aprendizes! O preço é #b" + meso + " mesos#k.");
    //        }
    //        else
    //        {
    //            meso = 10000;
    //            v0 = self.AskYesNo("Você irá para #b#m230000000##k agora? O preço é #b" + meso + " mesos#k.");
    //        }

    //        if (v0 == 0) self.Say("Hummm... muito ocupado para fazer isso agora? De qualquer forma, se quiser, volte e me procure.");
    //        else
    //        {
    //            var ret = target.IncMoney(-meso, 1);
    //            if (ret == 0) self.Say("Acho que você não tem dinheiro suficiente...");
    //            else target.ChangeMap(230000000, "");
    //        }
    //    }
    //}
}
