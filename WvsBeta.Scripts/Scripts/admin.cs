using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WvsBeta.Common;
using WvsBeta.Game;
using WvsBeta.Game.Scripting;

namespace WvsBeta.Scripts.Scripts
{
    // KIN
    [Script("levelUP")]
    class LevelUP : INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            if (!target.IsGM)
            {
                self.Say("Hey, how'd you get here?!");
                return;
            }
            int nRet1 = self.AskMenu("Hey there! What would you like to test?\r\n#L0##bAsk pet\r\n#L1#Send yes no\r\n#L2#Say lines\r\n#L3#Print pet ids\r\n#L4#Ask style\r\n#L5#Quests");
            if (nRet1 == 0) // Ask pet
            {
                string stringAnswer = self.AskPet("Select pet");
                self.Say("Selected pet: " + stringAnswer);
            }
            else if (nRet1 == 1) // Ask yes no
            {
                int nRet = self.AskYesNo("Yes no question");
                if (nRet == 0) self.Say("You answered no");
                else self.Say("You answered yes");
            }
            else if (nRet1 == 2) // Say lines
            {
                self.Say("Line 1");
                self.Say("Line 2");
                self.Say("Line 3");
                self.Say("Line 4");
            }
            else if (nRet1 == 3) // Print pet ids
            {
                int pi = 0;
                self.Say("Pets:" + string.Join("", target.Inventory.GetPets().Select(p => "\r\n#L" + pi++ + "##b#t" + p.ItemID + "##k - ItemID: #b" + p.ItemID + "#k, CashID: #b" + p.CashId)));
            }
            else if (nRet1 == 4)
            {
                int nRet = self.AskStyle("Select style", new List<int> { 0, 1, 2, 3, 4 });
                self.Say("Selected style: " + nRet);
            }
            else if (nRet1 == 5)
            {
                int nRet2 = self.AskMenu("Select an option\r\n#L0##bComplete quest (manual)\r\n#L1#Complete quest (auto)");
                short qid = (short)self.AskNumber("QuestID:", -1, short.MinValue, short.MaxValue);
                if (nRet2 == 0)
                {
                    string nRetStr = self.AskText("EndTime (UNIX):", "0", 0, (short)long.MaxValue.ToString().Length);
                    long unixtime;
                    try
                    {
                        unixtime = long.Parse(nRetStr);
                    }
                    catch
                    {
                        self.Say("Failed to parse long '" + nRetStr + "'");
                        return;
                    }

                    long filetime = MasterThread.GetFileTime(unixtime);
                    QuestPacket.SendCompleteQuest(target, qid, filetime);
                    self.Say("QuestID: #b" + qid + "#k. EndTime: #b" + unixtime + "#k. FileTime: #b" + filetime + "#k");
                }
                else
                {
                    target.Quests.SetComplete(qid);
                    self.Say("Quest #b" + qid + "#k set as complete.");
                }
            }
        }
    }
    // NimaKIN
    [Script("levelUP2")]
    class LevelUP2 : INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            int nRet1 = self.AskMenu("Hey, what's up? I'm NimaKIN. What do you want to do?\r\n#L0##bLevel up");
            if (nRet1 == 0) // Level up
            {
                bool nRet2 = target.LevelUP();
                if (!nRet2) self.Say("Looks like you've already reached max level, congratulations!");
                else self.Say("It is done.");
            }
        }
    }
}
