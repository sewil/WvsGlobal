using System;
using WvsBeta.Game;
using WvsBeta.Game.Scripting;

namespace WvsBeta.Scripts.Scripts
{
    public static class ossyria1
    {
        static INpcHost self;
        static GameCharacter target;
        public static void sellItem(int itemCode, int unitPrice, string desc)
        {
            var nRetNum = self.AskNumber("So the item you need is #b#t" + itemCode + "##k, right? That's " + desc + " It's not an easy item to get, but for you, I'll sell it for cheap. It'll cost you #b" + unitPrice + " mesos #k per. How many would you like to buy?", 1, 1, 100);
            var nPrice = unitPrice * nRetNum;
            var nRetBuy = self.AskYesNo("Do you really want to buy #r" + nRetNum + " #t" + itemCode + "#(s)#k? It'll cost you " + unitPrice + " mesos per #t" + itemCode + "#, which is #r" + nPrice + "#k mesos in total.");
            if (nRetBuy == 0) self.Say("I understand. You see, I have many different items here. Take a look. I am selling these items just for you. So I won't rob you at all.");
            else
            {
                var inventory = target.Inventory;
                var ret = inventory.Exchange(-nPrice, itemCode, nRetNum);
                if (ret == 0) self.Say("Are you sure you have enough mesos? Please check if your use or etc. inventory is full and that you have at least #r" + nPrice + "#k mesos.");
                else self.Say("Thank you. If some other day you are in need of items, stop by. I may have gotten old with time, but I can still make magic items easily.");
            }
        }
        // Alcaster
        [Script("oldBook1")]
        class oldBook1 : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                ossyria1.self = self;
                ossyria1.target = target;
                var qr = target.QuestRecord;
                var val = qr.GetState(3035);

                if (val == 2)
                {
                    var v1 = self.AskMenu("Thanks to you, #b#t4031056##k is safely sealed. As a result, I used up about half of the power I have accumulated over the last 800 years...but can now die in peace. Would you happen to be looking for rare items by any chance? As a sign of appreciation for your hard work, I'll sell some items in my possession to you and ONLY you. Pick out the one you want!",
                        "#t2050003#(Price : 300 mesos)",
                        "#t2050004#(Price : 400 mesos)",
                        "#t4006000#(Price : 5000 mesos)",
                        "#t4006001#(Price : 5000 mesos)"
                    );
                    if (v1 == 0) sellItem(2050003, 300, "The item that cures the state of being sealed and cursed.");
                    else if (v1 == 1) sellItem(2050004, 400, "The item that cures everything.");
                    else if (v1 == 2) sellItem(4006000, 5000, "The item of magical power used for high level skills.");
                    else if (v1 == 3) sellItem(4006001, 5000, "The item of summoning power used for high level skills.");
                }
                else if (target.Level > 54) self.Say("If you decide to help me, in return I will put items up for sale.");
                else self.Say("I am Alcaster the Sorcerer, resident of this city for over 300 years, where I have worked on many charms and spells.");
            }
        }
        // Lisa
        [Script("oldBook2")]
        class oldBook2 : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                ossyria1.self = self;
                ossyria1.target = target;
                var qr = target.QuestRecord;
                var val = qr.GetState(3006);
                var val2 = qr.GetState(3017);

                if (val == 0) self.Say("Are you looking for #bHella#k? Technically she lives here, but you won't be able to find her these days. A few months ago, she left town suddenly and never came back. It won't do much good to stop by her house, but at least the housekeeper should be there. How about talking to her?");
                else if (val2 == 0) self.Say("Where has #bHella#k gone... what? You know that she's alright? Hmmm... I don't know if I should trust a stranger's word, but if it's true, that's great. Of course you already warned Jade, right? Out of everyone, he is the most worried about her.");
                else self.Say("Monsters have been a lot more evil and cruel lately. And what if they come here?? I hope that never happens, right? Right?");
            }
        }
        // spiruna
        [Script("oldBook5")]
        class oldBook5 : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                ossyria1.self = self;
                ossyria1.target = target;
                var qr = target.QuestRecord;
                var val = qr.GetState(3014);
                string goAway = "I'm working on an important spell, so please don't disturb me and leave immediately. I can't concentrate when a stranger hangs around my house. Please go away...";

                if (val == 2)
                {
                    var v1 = self.AskMenu("Hella is a good child. Anything I ask, whether difficult or not, she does without complaining about anything. One day she will become a much better witch than I am. What do you want from me anyway??",
                        "I want to make #t4005004#",
                        "Nothing, nevermind"
                    );
                    if (v1 == 0)
                    {
                        var inventory = target.Inventory;
                        var nRet = self.AskYesNo("#b#t4005004##k?? How did you... did you hear about it from #b#p2020005##k? Yes, I know how to refine it, but... this ore is very hard to get. To make #b1 #t4005004##k, I need #b10 #t4004004#s#k and 50000 mesos. Do you want one?");
                        if (nRet == 0) self.Say("#b#t4005004##k. I haven't seen it for a long time... it's been hundreds of years since I last refined it, so I can barely remember how I did it... of course you won't have it now...");
                        else
                        {
                            var ret = inventory.Exchange(-50000, 4004004, -10, 4005004, 1);
                            if (ret == 0) self.Say("Are you out of mesos? Make sure you have #b10 #t4004004#s#k, 50000 mesos and space in your etc. inventory.");
                            else self.Say("Here, take #b1 #t4004004##k. It's been so long since I last made one, I hope it worked... By the way, how did you get the crystal ores? You must be something special indeed. Either way, it's an amazing item. Please put it to good use.");
                        }
                    }
                    else self.Say(goAway);
                }
                else self.Say(goAway);
            }
        }
        ////žóœºÅÍ ¶óÀÌµù
        //[Script("tamepig_enter")]
        //class tamepig_enter : INpcScript
        //{
        //    public void Run(INpcHost self, GameCharacter target)
        //    {
        //        ossyria1.self = self;
        //        ossyria1.target = target;
        //        var qr = target.QuestRecord;
        //        var inven = target.Inventory;
        //        var field = self.Field;
        //        if (field.ID == 230000003)
        //        {
        //            if (qr.GetState(6002) == 1)
        //            {
        //                if (inven.ItemCount(4031508) < 5 || inven.ItemCount(4031507) < 5)
        //                {
        //                    var quest = FieldSet.Instances["TamePig"];
        //                    var ret = quest.Enter(target, 0);
        //                    if (ret != 0) target.Message("Alguém já está tentando proteger o Porco Vigia. Por favor, tente mais tarde.");
        //                    else
        //                    {
        //                        var nItem = inven.ItemCount(4031508);
        //                        if (nItem > 0) inven.Exchange(0, 4031508, -nItem);
        //                        var nItem2 = inven.ItemCount(4031507);
        //                        if (nItem2 > 0) inven.Exchange(0, 4031507, -nItem2);
        //                    }
        //                }
        //                else self.Say("Ei~ você já tem bastante #b#t4031508##k e #b#t4031507##k. Eu não acho que você precise entrar lá novamente ~^^");
        //            }
        //            else if (qr.GetState(6002) == 2) self.Say("Você já se saiu bem protegendo o porco. Parabéns mais uma vez~");
        //            else self.Say("Como assim protegendo o porco? De onde você tirou esta idéia?");
        //        }
        //    }
        //}
        //[Script("tamepig_out")]
        //class tamepig_out : INpcScript
        //{
        //    public void Run(INpcHost self, GameCharacter target)
        //    {
        //        ossyria1.self = self;
        //        ossyria1.target = target;
        //        var inven = target.Inventory;
        //        if (inven.ItemCount(4031508) >= 5 && inven.ItemCount(4031507) >= 5)
        //        {
        //            self.Say("Uau~ Você conseguiu coletar 5 de cada b#t4031508##k e #b#t4031507##k!  Ok então, enviarei você para #m230000003#. Por favor, fale comigo novamente quando chegar lá.");
        //            target.ChangeMap(230000003, "");
        //        }
        //        else
        //        {
        //            var v0 = self.AskYesNo("Que pena, mas você não coletou 5 de cada #b#t4031508##k ou #b#t4031507##k. Você gostaria de parar a missão e ir embora?");
        //            if (v0 == 0) self.Say("Por favor, pense com cuidado e fale comigo.");
        //            else
        //            {
        //                self.Say("Você será transferido para #m923010100#.");
        //                target.ChangeMap(923010100, "");
        //            }
        //        }
        //    }
        //}
        //[Script("tamepig_out2")]
        //class tamepig_out2 : INpcScript
        //{
        //    public void Run(INpcHost self, GameCharacter target)
        //    {
        //        ossyria1.self = self;
        //        ossyria1.target = target;
        //        var inven = target.Inventory;
        //        //    self.Say( "Que pena, mas você não coletou 5 de cada #b#t4031508##k ou #b#t4031507##k. Vou deixar você sair, então." );
        //        var nItem = inven.ItemCount(4031508);
        //        if (nItem > 0) inven.Exchange(0, 4031508, -nItem);
        //        var nItem2 = inven.ItemCount(4031507);
        //        if (nItem2 > 0) inven.Exchange(0, 4031507, -nItem2);

        //        target.PlayPortalSE();
        //        target.ChangeMap(230000003, "");
        //    }
        //}
    }
}
