using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WvsBeta.Game;
using WvsBeta.Game.Scripting;

namespace WvsBeta.Scripts.Scripts
{
    internal class MTaxi : INpcScript
    {
        int fee;
        public void Run(INpcHost self, GameCharacter target, byte state, byte answer, string stringAnswer, int integerAnswer)
        {
            var cJob = target.Job;

            if (state == 0)
            {
                self.SendNext("Hey! This taxi is for VIP customers only. Instead of simply taking you to cities like regular taxis, we offer much better service worthy of the VIP class. It's a little more expensive, but... for just 10,000 mesos, we'll get you safely to #bAnt Tunnel Park#k.");
            }
            else if (state == 1)
            {
                if (cJob == 0)
                {
                    self.AskYesNo("We have a special 90% discount for beginner. Ant Tunnel is located at the very bottom of the Dungeon, which is in the center of Victoria Island, where #p1061001# is located. Would you like to go there for #b1,000 mesos#k?");
                    fee = 1000;
                }
                else
                {
                    self.AskYesNo("The standard rate applies to all non-beginners. Ant Tunnel is located at the very bottom of the Dungeon, which is in the center of Victoria Island, where #p1061001# is located. Would you like to go there for #b10,000 mesos#k?");
                    fee = 10000;
                }
            }
            else if (answer == 0)
            {
                self.SendNext("This town also has a lot to offer. Look for us if and when you feel the need to go to Ant Tunnel Park.");
                self.Stop();
            }
            else
            {
                var fRet = target.AddMesos(-fee, false, true);
                if (fRet == -1)
                {
                    self.SendNext("It looks like you don't have enough mesos. Sorry, but you won't be able to use this without it.");
                }
                else
                {
                    target.ChangeMap(105070001);
                }
                self.Stop();
            }
        }
    }
}
