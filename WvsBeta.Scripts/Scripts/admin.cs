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
        int nRet1;
        int nRet2;
        short qid;
        long qendtime;
        public void Run(INpcHost self, GameCharacter target, byte state, byte nRet, string stringAnswer, int nRetNum)
        {
            if (!target.IsGM)
            {
                self.Say("Hey, how'd you get here?!");
                self.Stop();
                return;
            }
            if (state == 0)
            {
                self.AskMenu("Hey there! What would you like to test?\r\n#L0##bAsk pet\r\n#L1#Send yes no\r\n#L2#Say lines\r\n#L3#Print pet ids\r\n#L4#Ask style\r\n#L5#Quests");
                return;
            }
            else if (state == 1) nRet1 = nRet;
            if (nRet1 == 0) // Ask pet
            {
                if (state == 1)
                {
                    self.AskPet("Select pet");
                    return;
                }
                else self.SendNext("Selected pet: " + stringAnswer);
            }
            else if (nRet1 == 1) // Ask yes no
            {
                if (state == 1)
                {
                    self.AskYesNo("Yes no question");
                    return;
                }
                else if (nRet == 0) self.SendNext("You answered no");
                else self.SendNext("You answered yes");
            }
            else if (nRet1 == 2) // Say lines
            {
                self.Say(1, true,
                    "Line 1",
                    "Line 2",
                    "Line 3",
                    "Line 4"
                );
                return;
            }
            else if (nRet1 == 3) // Print pet ids
            {
                int pi = 0;
                self.Say("Pets:" + string.Join("", target.Inventory.GetPets().Select(p => "\r\n#L" + pi++ + "##b#t" + p.ItemID + "##k - ItemID: #b" + p.ItemID + "#k, CashID: #b" + p.CashId)));
            }
            else if (nRet1 == 4)
            {
                if (state == 1)
                {
                    self.AskStyle("Select style", new List<int> { 0, 1, 2, 3, 4 });
                    return;
                }
                self.Say("Selected style: " + nRet);
            }
            else if (nRet1 == 5)
            {
                if (state == 1)
                {
                    self.AskMenu("Select an option\r\n#L0##bComplete quest (manual)\r\n#L1#Complete quest (auto)");
                    return;
                }
                else if (state == 2)
                {
                    nRet2 = nRet;
                    self.AskNumber("QuestID:", -1, short.MinValue, short.MaxValue);
                    return;
                }
                else if (state == 3)
                {
                    qid = (short)nRetNum;
                    if (nRet2 == 0)
                    {
                        self.AskText("EndTime (UNIX):", "0", 0, (short)long.MaxValue.ToString().Length);
                        return;
                    }
                    else
                    {
                        self.Say("Quest #b" + qid + "#k set as complete.");
                        target.Quests.SetComplete(qid);
                    }
                }
                else if(state == 4)
                {
                    long unixtime;
                    try
                    {
                        unixtime = long.Parse(stringAnswer);
                    }
                    catch
                    {
                        self.Say("Failed to parse '" + stringAnswer + "' to long");
                        self.Stop();
                        return;
                    }

                    long filetime = MasterThread.GetFileTime(unixtime);
                    self.Say("QuestID: #b" + qid + "#k. EndTime: #b" + unixtime + "#k. FileTime: #b" + filetime + "#k");
                    QuestPacket.SendCompleteQuest(target, qid, filetime);
                }
            }
            self.Stop();
        }
    }
    // NimaKIN
    [Script("levelUP2")]
    class LevelUP2 : INpcScript
    {
        int nRet1;
        public void Run(INpcHost self, GameCharacter target, byte state, byte nRet, string stringAnswer, int nRetNum)
        {
            if (state == 0)
            {
                self.AskMenu("Hey, what's up? I'm NimaKIN. What do you want to do?\r\n#L0##bLevel up");
                return;
            }
            if (state == 1) nRet1 = nRet;
            if (nRet1 == 0) // Level up
            {
                bool nRet2 = target.LevelUP();
                if (!nRet2) self.Say("Looks like you've already reached max level, congratulations!");
                else self.Say("It is done.");
            }
            self.Stop();
        }
    }
}
