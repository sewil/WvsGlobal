using WvsBeta.Common;
using WvsBeta.Game;
using WvsBeta.Game.Packets;
using WvsBeta.Game.Scripting;
using System.Linq;
using WvsBeta.Common.Extensions;

namespace WvsBeta.Scripts.Scripts
{
    public class party
    {
        public static void party1_takeawayitem(GameCharacter target)
        {
            var inven = target.Inventory;
            var count = inven.ItemCount(4001007);
            if (count > 0) inven.Exchange(0, 4001007, -count);
            count = inven.ItemCount(4001008);
            if (count > 0) inven.Exchange(0, 4001008, -count);

            return;
        }
        // reward 
        public static void party1_reward(INpcHost self, GameCharacter target)
        {
            self.Say("Incredible! You've completed all the stages to get here. Here's a little reward for a job well done. But, before accepting, check that you have available slots in the use and etc inventories.");

            var inven = target.Inventory;
            if (inven.SlotCount(2) > inven.HoldCount(2) && inven.SlotCount(4) > inven.HoldCount(4))
            {
                var rnum = Rand32.NextBetween(0, 39);
                var nNewItemID = 0;
                var nNewItemNum = 0;
                if (rnum == 0)
                {
                    nNewItemID = 2000004;
                    nNewItemNum = 5;
                }
                else if (rnum == 1)
                {
                    nNewItemID = 2000001;
                    nNewItemNum = 100;
                }

                else if (rnum == 2)
                {
                    nNewItemID = 2000002;
                    nNewItemNum = 70;
                }
                else if (rnum == 3)
                {
                    nNewItemID = 2000003;
                    nNewItemNum = 100;
                }
                else if (rnum == 4)
                {
                    nNewItemID = 2000006;
                    nNewItemNum = 50;
                }
                else if (rnum == 5)
                {
                    nNewItemID = 2022000;
                    nNewItemNum = 15;
                }
                else if (rnum == 6)
                {
                    nNewItemID = 2022003;
                    nNewItemNum = 15;
                }
                else if (rnum == 7)
                {
                    nNewItemID = 2040002;
                    nNewItemNum = 1;
                }
                else if (rnum == 8)
                {
                    nNewItemID = 2040402;
                    nNewItemNum = 1;
                }
                else if (rnum == 9)
                {
                    nNewItemID = 2040502;
                    nNewItemNum = 1;
                }
                else if (rnum == 10)
                {
                    nNewItemID = 2040505;
                    nNewItemNum = 1;
                }
                else if (rnum == 11)
                {
                    nNewItemID = 2040602;
                    nNewItemNum = 1;
                }
                else if (rnum == 12)
                {
                    nNewItemID = 2040802;
                    nNewItemNum = 1;
                }
                else if (rnum == 13)
                {
                    nNewItemID = 4003000;
                    nNewItemNum = 30;
                }
                else if (rnum == 14)
                {
                    nNewItemID = 4010000;
                    nNewItemNum = 8;
                }
                else if (rnum == 15)
                {
                    nNewItemID = 4010001;
                    nNewItemNum = 8;
                }
                else if (rnum == 16)
                {
                    nNewItemID = 4010002;
                    nNewItemNum = 8;
                }
                else if (rnum == 17)
                {
                    nNewItemID = 4010003;
                    nNewItemNum = 8;
                }
                else if (rnum == 18)
                {
                    nNewItemID = 4010004;
                    nNewItemNum = 8;
                }
                else if (rnum == 19)
                {
                    nNewItemID = 4010005;
                    nNewItemNum = 8;
                }
                else if (rnum == 20)
                {
                    nNewItemID = 4010006;
                    nNewItemNum = 5;
                }
                else if (rnum == 21)
                {
                    nNewItemID = 4020000;
                    nNewItemNum = 8;
                }
                else if (rnum == 22)
                {
                    nNewItemID = 4020001;
                    nNewItemNum = 8;
                }
                else if (rnum == 23)
                {
                    nNewItemID = 4020002;
                    nNewItemNum = 8;
                }
                else if (rnum == 24)
                {
                    nNewItemID = 4020003;
                    nNewItemNum = 8;
                }
                else if (rnum == 25)
                {
                    nNewItemID = 4020004;
                    nNewItemNum = 8;
                }
                else if (rnum == 26)
                {
                    nNewItemID = 4020005;
                    nNewItemNum = 8;
                }
                else if (rnum == 27)
                {
                    nNewItemID = 4020006;
                    nNewItemNum = 8;
                }
                else if (rnum == 28)
                {
                    nNewItemID = 4020007;
                    nNewItemNum = 3;
                }
                else if (rnum == 29)
                {
                    nNewItemID = 4020008;
                    nNewItemNum = 3;
                }
                else if (rnum == 30)
                {
                    nNewItemID = 1032002;
                    nNewItemNum = 1;
                }
                else if (rnum == 31)
                {
                    nNewItemID = 1032004;
                    nNewItemNum = 1;
                }
                else if (rnum == 32)
                {
                    nNewItemID = 1032005;
                    nNewItemNum = 1;
                }
                else if (rnum == 33)
                {
                    nNewItemID = 1032006;
                    nNewItemNum = 1;
                }
                else if (rnum == 34)
                {
                    nNewItemID = 1032007;
                    nNewItemNum = 1;
                }
                else if (rnum == 35)
                {
                    nNewItemID = 1032009;
                    nNewItemNum = 1;
                }
                else if (rnum == 36)
                {
                    nNewItemID = 1032010;
                    nNewItemNum = 1;
                }
                else if (rnum == 37)
                {
                    nNewItemID = 1002026;
                    nNewItemNum = 1;
                }
                else if (rnum == 38)
                {
                    nNewItemID = 1002089;
                    nNewItemNum = 1;
                }
                else if (rnum == 39)
                {
                    nNewItemID = 1002090;
                    nNewItemNum = 1;
                }
                var ret = inven.Exchange(0, nNewItemID, nNewItemNum);
                if (ret == 0) self.Say("Hmmm... are you sure that your use and etc inventory have a free slot? I cannot reward you for the effort if your inventories are full.");
                else target.ChangeMap(103000805, "");
            }

            else self.Say("Your use and etc inventory need to have at least one empty slot to receive the rewards. Please make the necessary adjustments and then talk to me again.");
        }
        // The individual quest within a party quest 
        public static void party1_personal(INpcHost self, GameCharacter target)
        {
            int q;
            var charName = target.Name + "_";

            var quest = FieldSet.Instances["Party1"];
            var prob = quest.GetVar(charName);
            if (prob == "clear")
            {
                self.Say("Wow, you handled my challenge well. Here's the party pass; deliver it to your leader.");
                return;
            }

            if (prob == "") q = Rand32.NextBetween(0, 8); // Dish out questions.
            else q = int.Parse(prob); // Repeat the question.

            // Explaining the questions and the answers 
            var desc = "";
            int ans = 0;
            if (q == 0) { desc = "Task: The number of coupons you must collect is the same number of EXP needed to advance from #level 1 to level 2."; ans = 15; }

            else if (q == 1) { desc = "This is the task: The number of coupons you must collect is the same number as the level needed to make the first job advancement as a magician."; ans = 8; }
            else if (q == 2) { desc = "This is the task: The number of coupons you must collect is the same number as the minimum amount of STR needed to make the first job advancement as a warrior."; ans = 35; }
            else if (q == 3) { desc = "This is the task: The number of coupons you must collect is the same number as the minimum amount of INT required to make the first job advancement as a magician."; ans = 20; }
            else if (q == 4) { desc = "This is the task: The number of coupons you must collect is the same number as the minimum amount of DEX required to make the first job advancement as a bowman."; ans = 25; }
            else if (q == 5) { desc = "This is the task: The number of coupons you must collect is the same number as the minimum amount of DEX required to make the first job advancement as a thief."; ans = 25; }
            else if (q == 6) { desc = "This is the task: The number of coupons you must collect is the same number as the level needed to make the first job advancement as a warrior."; ans = 10; }
            else if (q == 7) { desc = "This is the task: The number of coupons you must collect is the same number as the level needed to make the first job advancement as a bowman."; ans = 10; }
            else if (q == 8) { desc = "This is the task: The number of coupons you must collect is the same number as the level needed to make the first job advancement as a thief."; ans = 10; }

            // If the question is given 
            if (prob == "")
            {
                quest.SetVar(charName, q.ToString());
                self.Say("You need to collect the same number of #bcoupons#k from the Alligators as the answer to my individual riddles.");
                self.Say(desc);
                return;
            }

            // Scoring 
            var inven = target.Inventory;
            if (inven.ItemCount(4001007) == ans)
            {
                if (inven.Exchange(0, 4001007, -ans, 4001008, 1) == 0)
                {
                    self.Say("You need to collect the number of coupons suggested by the answer. Not more or less. Make sure you have the coupons.");
                    return;
                }
                quest.SetVar(charName, "clear");
                self.Say("That's the right answer! You have just received a #bpass#k. Please give it to your party leader.");
            }
            else
            {
                self.Say("That's not the right answer. I can only give you the pass if you collect the number of #bcoupons#k given by the answer to the question. I will now repeat the question.");
                self.Say(desc);
            }
        }
        public static void party1_help(INpcHost self, GameCharacter target)
        {
            var field = target.Field;
            if (field.ID == 103000801) self.Say("I will describe the 2nd stage. You'll see some ropes next to me. #b3 of them are connected to the portal to the next stage#k. All you need is for #b3 party members to find the right ropes and hang onto them#k.\r\nBUT it won't count if you hang too low. Please go high enough for the answer to be considered correct. And only 3 members of your group will be allowed on the ropes. When this happens, the group leader must #bclick me twice to know if the answer is correct or not#k. Good luck to you!");
            else if (field.ID == 103000802) self.Say("I will describe the 3rd stage. You'll see a bunch of barrels with kittens in them on top of the platforms. #b 3 of these platforms will be connected to the portal leading to the next stage#k. #b3 party members need to find the correct platforms to climb on and complete the stage.\r\nBUT you must stand firmly in the center, not the edge, for the answer to be considered correct. And only 3 members of your group will be allowed on the platforms. When the members are on the platforms, the leader of the group must #bclick me twice to know if the answer is correct or not#k. Good luck to you~!");
            else if (field.ID == 103000803) self.Say("I will describe the 4th stage. You'll see a bunch of barrels nearby. #b3 of these barrels will be connected to the portal leading to the next stage#k. #b3 party members need to find the correct barrels and stand on top of them#k to complete the stage. BUT, for the answer to count, you need to stand firmly in the center of the barrel, not the edge. And only 3 members of your party can stand on top of the barrels. When the members are on top, the group leader must #bclick me twice to know if the answer is correct or not#k. Good luck to you!");
        }

        public static void party1_stage1(INpcHost self, GameCharacter target)
        {
            var quest = FieldSet.Instances["Party1"];
            var stage = quest.GetVar("stage");
            if (stage == "")
            {
                quest.SetVar("stage", "1");
                self.Say("Hello and welcome to the first stage. Look around and you'll see Alligators walking around. Once defeated, they will drop a #bcoupon#k. Apart from the leader, each member of the group must talk to me, listen to the question and collect the same number of #bcoupons#k as the answer.\r\nI will give the #bpass#k to the player who collects the correct number of #bcoupons#k . Once all the members have obtained #bpasses#k and handed them to the leader, he will give the #bpasses#k to me, completing the stage. The faster the stages are completed, the more stages there are to challenge. For this reason, I suggest that you quickly take care of everything. Well, good luck to you!");
                return;
            }
            if (stage != "1")
            {
                self.Say("You have completed this stage. Proceed to the next stage using the portal. Be careful...");
                return;
            }

            // checking the number of passes 
            var users = quest.UserCount - 1; // exclude the leader of the party 

            var inven = target.Inventory;
            if (inven.ItemCount(4001008) < users) self.Say("Sorry, but you don't have enough passes. You need to give me the correct number of passes; it must be the same number as the number of members in your party, excluding the leader, #b" + users + " " + users.Pluralis("passes", "pass") + "#k to complete this stage. Tell your party members to answer the questions, collect the passes and give them to you.");
            else
            {
                self.Say("You gathered #b" + users + " " + users.Pluralis("passes", "pass") + "#k! Congratulations on completing the stage! I will create the portal that sends you to the next stage. There is a time limit to get there, so hurry up. Good luck to you all!");
                if (inven.Exchange(0, 4001008, -users) == 0)
                {
                    self.Say("You need to give me the correct number of passes; it should be the same number as the number of members in your party, excluding the leader. Not more or less. Please make sure you have the correct amount.");
                    return;
                }
                var field = target.Field;

                field.EffectPartyClear();
                field.EffectObject("gate");
                field.EnablePortal("next00", 1);
                quest.SetVar("stage", "2");
                quest.IncExpAll(100);
            }
        }

        public static string area_check(GameCharacter target, int num, int user)
        {
            var field = target.Field;
            var count = 0;
            var answer = "";
            for (int i = 0; i < num; i++)
            {
                var cur = field.CountUserInArea(i.ToString());
                count = count + cur;
                answer = answer + cur.ToString();
            }
            if (count == user) return answer;
            return "";
        }
        public static int check_stage(INpcHost self, GameCharacter target, string st, int checkall)
        {
            var quest = FieldSet.Instances["Party1"];
            var stage = quest.GetVar("stage");
            if (stage != st)
            {
                self.Say("You all completed the quest for this stage. Use the portal to proceed to the next stage...");
                return 0;
            }

            var field = target.Field;
            if (checkall == 1 && quest.UserCount != field.Characters.Count)
            {
                self.Say("It seems that not all members of the group are together. Everyone in your party needs to come from the previous stage to participate in the quest. Please have all members ready here...");
                return 0;
            }
            return 1;
        }

        public static void party1_stage2(INpcHost self, GameCharacter target)
        {
            if (check_stage(self, target, "2", 1) == 0) return;

            var quest = FieldSet.Instances["Party1"];
            var question = quest.GetVar("ans2");
            if (question == "")
            {
                quest.SetVar("ans2", Tools.Shuffle(1, "1110"));
                self.Say("Hey. Welcome to the 2nd stage. You'll see some ropes near me. #b3 of these ropes will be connected to the portal leading to the next stage#k. All you need is for #b3 party members to find the ropes and hang onto them#k.\r\nBUT this doesn't count as a correct answer if you hang too low. Please go high enough for the answer to be considered correct. And only 3 members of your group will be allowed on the ropes. When this happens, the group leader must #bclick me twice to know if the answer is correct or not#k. Now, find the right ropes to hang from!");
                return;
            }

            var field = target.Field;
            var answer = area_check(target, 4, 3);

            if (answer == "") self.Say("Looks like you haven't found the 3 ropes yet. Think of a different rope combination. Only 3 members can hang from the ropes. And don't hang too low or the answer won't count. Keep going!");
            else if (question != answer)
            {
                field.EffectPartyWrong();
            }
            else
            {
                field.EffectPartyClear();
                field.EffectObject("gate");
                field.EnablePortal("next00", 1);
                quest.SetVar("stage", "3");
                quest.IncExpAll(200);
            }
        }
        public static void party1_stage3(INpcHost self, GameCharacter target)
        {
            if (check_stage(self, target, "3", 1) == 0) return;

            var quest = FieldSet.Instances["Party1"];
            var question = quest.GetVar("ans3");
            if (question == "")
            {
                quest.SetVar("ans3", Tools.Shuffle(1, "11100"));
                self.Say("Hello and welcome to the 3rd stage. On top of the platforms, you'll see some barrels nearby with kittens inside. From these platforms, #b3 will lead to the portal to the next stage#k. #b3 party members need to find the correct platforms to climb on and complete the stage.\r\nBUT you must stand firmly in the center, not the edge, for the answer to be considered correct. And only 3 members of your group will be allowed on the platforms. When the members are on the platforms, the leader of the group must #bclick me twice to know if the answer is correct or not#k. Now, find the right platforms~!");
                return;
            }

            var field = target.Field;
            var answer = area_check(target, 5, 3);

            if (answer == "") self.Say("Looks like you haven't found the 3 platforms yet. Think of a different combination of platforms. And remember that only 3 members may stand on the platforms, firmly in the center, for the answer to be valid. Keep going!");
            else if (question != answer)
            {
                field.EffectPartyWrong();
            }
            else
            {
                field.EffectPartyClear();
                field.EffectObject("gate");
                field.EnablePortal("next00", 1);
                quest.SetVar("stage", "4");
                quest.IncExpAll(400);
            }
        }
        public static void party1_stage4(INpcHost self, GameCharacter target)
        {
            if ( check_stage(self, target, "4", 1 ) == 0 ) return;

            var quest = FieldSet.Instances["Party1"];
            var question = quest.GetVar( "ans4" ); 
            if ( question == "" ) { 
                quest.SetVar( "ans4", Tools.Shuffle( 1, "111000" ) ); 
                self.Say("Hey. Welcome to the 4th stage. You'll see some barrels nearby. 3 of these barrels will be connected to the portal leading to the next stage. #b3 party members need to find the correct barrels and stand on top of them#k to complete the stage. BUT, for the answer to count, you need to stand firmly in the center of the barrel, not the edge. And only 3 members of your party can stand on top of the barrels. When the members are on top, the group leader must #bclick me twice to know if the answer is correct or not#k. Now, find the right barrels~!"); 
                return; 
            } 

            var field = target.Field;
            var answer = area_check(target, 6, 3 );

            if ( answer == "" ) self.Say("Looks like you haven't found the 3 barrels yet. Think of a different combination of barrels. And don't forget that only 3 members may stand on top of the barrels, firmly in the center for the answer to count as correct. Continues!"); 
            else if ( question != answer )
            {
                field.EffectPartyWrong();
            } 
            else
            {
                field.EffectPartyClear();
                field.EffectObject("gate");
                field.EnablePortal("next00", 1);
                quest.SetVar( "stage", "5" );
                quest.IncExpAll(800);
            } 
        } 

        public static void party1_stage5(INpcHost self, GameCharacter target)
        { 
            if ( check_stage(self, target, "5", 1 ) == 0 ) return;

            var quest = FieldSet.Instances["Party1"];

            var inven = target.Inventory;
            if ( inven.ItemCount( 4001008 ) < 10 ) self.Say("Hello. Welcome to the 5th and final stage. Walk around the map and you might see some Boss Monsters. Defeat them all and gather 10 #bpasses#k for me. Once you've obtained your pass, your party leader will round them up and hand them to me when he has all 10. The monsters may look familiar, but they're a lot stronger than you think. So be careful. Good luck!"); 
            else
            { 
                if ( inven.Exchange( 0, 4001008, -10 ) == 0 ) { 
                    self.Say("You've wiped out all the Boss Monsters and collected all the #b10 passes#k. Good job."); 
                    return;
                }
                var field = target.Field;
                field.EffectPartyClear();
                field.EffectObject("gate");
                quest.SetVar( "stage", "clear" );
                quest.IncExpAll(1500);
                self.Say("Here is the portal that leads to the last bonus stage. It's a stage that lets you defeat common monsters a little easier. You will have a limited time to defeat as many of them as possible, but you can leave the stage whenever you want by talking to the NPC. Once again, congratulations on completing all stages. Be careful...");
                return;
            } 
        } 
    }
    // Party Quest - The NPC that lets the users in. 
    [Script("party1_enter")]
    class party1_enter : INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            if (!target.IsPartyBoss)
            {
                self.Say("How about you and your party finish a quest together? Here you will encounter obstacles and problems that can only be solved as a team. If you want to give it a try, ask your party's #bleader#k to talk to me.");
                return;
            }

            var setParty = FieldSet.Instances["Party1"];
            var res = setParty.Enter(target, 0);
            if (res == FieldSet.EnterStatus.Invalid) self.Say("I have reason not to let your party in. Please try again later.");
            else if (res == FieldSet.EnterStatus.NotInParty) self.Say("You are not in a party. You can only do this quest when you are in a party.");
            else if (res == FieldSet.EnterStatus.WrongMemberCount) self.Say("Your party does not have four members. Come back when you have four members.");
            else if (res == FieldSet.EnterStatus.TooWeak) self.Say("Someone in your party is not between levels 21~30. Please check again.");
            else if (res == FieldSet.EnterStatus.Full) self.Say("Another party has already entered to complete the quest. Please try again later.");
            else
            {
                party.party1_takeawayitem(target);
            }
        }
    }
    // party quest - the host NPC 
    [Script("party1_play")]
    class party1_play : INpcScript
    {
        FieldSet quest;
        Map field;
        public void Run(INpcHost self, GameCharacter target)
        {
            field = target.Field;
            quest = FieldSet.Instances["Party1"];

            // the reward 
            if (quest.GetVar("stage") == "clear")
            {
                party.party1_reward(self, target);
                return;
            }

            if (!target.IsPartyBoss)
            {
                if (field.ID == 103000800) party.party1_personal(self, target);
                else party.party1_help(self, target);
            }
            else
            {
                if (field.ID == 103000800) party.party1_stage1(self, target);
                else if (field.ID == 103000801) party.party1_stage2(self, target);
                else if (field.ID == 103000802) party.party1_stage3(self, target);
                else if (field.ID == 103000803) party.party1_stage4(self, target);
                else if (field.ID == 103000804) party.party1_stage5(self, target);
            }
        }
    }
    // party quest - the NPC that leads the users out 
    [Script("party1_out")]
    class party1_out : INpcScript
    {
        dynamic inven, count, nRet;
        Map field;
        public void Run(INpcHost self, GameCharacter target)
        {
            field = target.Field;
            if (field.ID == 103000805)
            {
                nRet = self.AskYesNo("Did you hunt a lot in the bonus map? Once you leave this place, you won't be able to go back and hunt again. Are you sure you want to exit?");
                if (nRet == 0) self.Say("I understand. This map was made for you to hunt as much as possible before time runs out. You need to talk to me if you want to get out of this stage.");
                else target.ChangeMap(103000890, "");
            }
            else if (field.ID == 103000890)
            {
                inven = target.Inventory;
                count = inven.ItemCount(4001007);
                if (count > 0)
                {
                    if (inven.Exchange(0, 4001007, -count) == 0)
                    {
                        self.Say("Are you sure you have the exact amount of coupons? Please check one more time.");
                        return;
                    }
                }
                count = inven.ItemCount(4001008);
                if (count > 0)
                {
                    if (inven.Exchange(0, 4001008, -count) == 0)
                    {
                        self.Say("Are you sure you have the exact amount of passes? Please check again.");
                        return;
                    }
                }
                target.ChangeMap(103000000, "");
            }
            else
            {
                // Send the user to the "mapa final" from every stage 
                nRet = self.AskYesNo("If you leave the map, you'll need to redo the whole quest if you want to try again. Still want to leave this map?");
                if (nRet == 0) self.Say("I understand. Teamwork is very important here. Please try harder with your party members.");
                else target.ChangeMap(103000890, "");
            }
        }
    }
}
