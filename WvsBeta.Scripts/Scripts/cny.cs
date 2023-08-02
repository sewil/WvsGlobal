using System;
using WvsBeta.Common;
using WvsBeta.Common.Extensions;
using WvsBeta.Game;
using WvsBeta.Game.Scripting;

namespace WvsBeta.Scripts.Scripts
{
    [Script("cny")]
    class cny : INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            //self.Say( "Você teve um bom Ano-Novo Lunar? Eu me diverti muito comemorando com meus amigos aqui em MapleStory! Estou aqui te desejando toda sorte, por todo este ano!" );

            DateTime cDate = MasterThread.CurrentDate;
            string cTime = MasterThread.CurrentDateStr;
            string cYear = cDate.ToString("yy");
            string zodiac = cDate.ChineseZodiac();

            int esTime = MasterThread.CompareTime(cTime, cYear + "/01/30/09/00");
            int eeTime = MasterThread.CompareTime(cYear + "/02/20/00/00", cTime);
            string noEnv = "Are you sure you have a #b#t4031249##k? I make the best offer in town!";

            if (esTime < 0) self.Say("The event has not started yet. Please hold on.");
            else
            {
                if (eeTime < 0) self.Say("The event is over. See you next year!");
                else
                {

                    GameInventory inventory = target.Inventory;
                    CharacterQuests qr = target.QuestRecord;
                    string val = qr.Get(8208);

                    if (val == "end")
                    {            // if a user already finished the 1st nana quest
                        var nRet = self.AskYesNo("Hey, good to see you again! How are you? So, did you find more #b#t4031249##k for me? Would you like to make another exchange?");
                        if (nRet == 0) self.Say("Oh, really? All right... If you find more #b#t4031249##k, you know where to find me, because I'll be here for a while.");
                        else
                        {
                            self.Say("Sweet. That sounds like news. I'll be here waiting.");
                            qr.Set(8208, "ing");
                        }
                    }
                    else if (val == "ing")
                    {
                        int nItem = inventory.ItemCount(4031249);

                        if (nItem > 0)
                        {
                            int rnum = Rand32.NextBetween(0, 800000);
                            int won;
                            string reaction;
                            if (rnum <= 1)
                            {
                                won = 10_000_000;
                                reaction = "Oh my God! This is your year! 10,000,000 mesos for a #b#t4031249##k... I think I'm going to have an ulcer. A deal is a deal... Enjoy your money!";
                            }
                            else if (rnum <= 49900)
                            {
                                if (rnum <= 99) won = 1_000_000;
                                else won = 100_000;
                                reaction = "There you go! Wow!? I think you got a lot, but a deal is a deal. I'm still short of #b#t4031249#s#k, so bring them to me when you find them.";
                            }
                            else
                            {
                                if (rnum <= 150000) won = 10_000;
                                else won = 1_000;
                                reaction = "There you go! Hope you have a great year. I'm still short of #b#t4031249#s#k, so bring them to me when you find them.";
                            }

                            int ret = inventory.Exchange(won, 4031249, -1);
                            if (ret != 0)
                            {
                                qr.Set(8208, "end");
                                self.Say(reaction);
                            }
                            else self.Say("Sorry, but I'm having trouble getting items into your inventory. Please confirm with me later.");
                        }
                        else self.Say(noEnv);
                    }
                    else
                    {
                        self.Say($"Happy Lunar New Year! May all your dreams come true in the Year of the {zodiac}! {cDate.Year - 1} was a great year for me. I've made a fortune selling recycled weapons and armor, and I'm here to share my good fortune with you as a way of wishing you a Happy New Year.");

                        int nRet = self.AskYesNo("The monsters ate all my empty #b#t4031249#s#k. Vile creatures! I desperately need them to give to my family for New Years. I will buy from you for a certain sum of mesos, depending on how much money I have in my pocket. So what do you think? Want to do business?");
                        if (nRet != 0)
                        {
                            self.Say("Alright... Good luck!!!");
                            qr.Set(8208, "ing");
                        }
                        else self.Say(noEnv);
                    }
                }
            }
        }
    }
}
