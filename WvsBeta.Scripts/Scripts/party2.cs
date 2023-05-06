using System;
using System.Data;
using System.Linq;
using WvsBeta.Common;
using WvsBeta.Common.Enums;
using WvsBeta.Game;
using WvsBeta.Game.Scripting;

namespace WvsBeta.Scripts.Scripts
{
    public static class party2
    {
        static class StringPool
        {
            public static Func<int, string> WrongAmount = (itemID) => $"Are you sure you have the right amount of #t{itemID}#? Please check again.";
            public static string UseEquipFull = "Your use and equip inventories must have at least one slot available. Please make the necessary adjustments and then talk to me again.";
            public static string RewardInventoryFull = "Hmmm... are you sure you have a free slot in your use and etc inventories? I cannot reward you for your effort if your inventory is full...";
            public static string PortalIsOpen = "The portal leading to the next stage is open.";
            public static string StagePassed(int itemID, int count)
            {
                return $"Good job defeating all the monsters and collecting #b{(count > 1 ? count + " " : "")}#t{itemID}#s#k. Very impressive!";
            }
        }
        static INpcHost self;
        static GameCharacter target;
        public static void party2_reward()
        {
            self.Say("Incredible! You've completed all the stages and now you're here enjoying your victory. Wow! My sincere congratulations to each of you for a job well done. Here's a little treat for you. Before accepting, please check that you have an available slot in your use and equip inventories.");

            var inven = target.Inventory;
            if (inven.SlotCount(2) > inven.HoldCount(2) && inven.SlotCount(4) > inven.HoldCount(4))
            {
                var rnum = Rand32.NextBetween(0, 250);
                var nNewItemID = 0;
                var nNewItemNum = 0;
                if (rnum == 0) { nNewItemID = 2000004; nNewItemNum = 10; }
                else if (rnum == 1) { nNewItemID = 2000002; nNewItemNum = 100; }
                else if (rnum == 2) { nNewItemID = 2000003; nNewItemNum = 100; }
                else if (rnum == 3) { nNewItemID = 2000006; nNewItemNum = 30; }
                else if (rnum == 4) { nNewItemID = 2022000; nNewItemNum = 30; }
                else if (rnum == 5) { nNewItemID = 2022003; nNewItemNum = 30; }
                else if (rnum == 6) { nNewItemID = 2040002; nNewItemNum = 1; }
                else if (rnum == 7) { nNewItemID = 2040402; nNewItemNum = 1; }
                else if (rnum == 8) { nNewItemID = 2040502; nNewItemNum = 1; }
                else if (rnum == 9) { nNewItemID = 2040505; nNewItemNum = 1; }
                else if (rnum == 10) { nNewItemID = 2040602; nNewItemNum = 1; }
                else if (rnum == 11) { nNewItemID = 2040802; nNewItemNum = 1; }
                else if (rnum == 12) { nNewItemID = 4003000; nNewItemNum = 50; }
                else if (rnum == 13) { nNewItemID = 4010000; nNewItemNum = 15; }
                else if (rnum == 14) { nNewItemID = 4010001; nNewItemNum = 15; }
                else if (rnum == 15) { nNewItemID = 4010002; nNewItemNum = 15; }
                else if (rnum == 16) { nNewItemID = 4010003; nNewItemNum = 15; }
                else if (rnum == 17) { nNewItemID = 4010004; nNewItemNum = 15; }
                else if (rnum == 18) { nNewItemID = 4010005; nNewItemNum = 15; }
                else if (rnum == 19) { nNewItemID = 4010006; nNewItemNum = 10; }
                else if (rnum == 20) { nNewItemID = 4020000; nNewItemNum = 15; }
                else if (rnum == 21) { nNewItemID = 4020001; nNewItemNum = 15; }
                else if (rnum == 22) { nNewItemID = 4020002; nNewItemNum = 15; }
                else if (rnum == 23) { nNewItemID = 4020003; nNewItemNum = 15; }
                else if (rnum == 24) { nNewItemID = 4020004; nNewItemNum = 15; }
                else if (rnum == 25) { nNewItemID = 4020005; nNewItemNum = 15; }
                else if (rnum == 26) { nNewItemID = 4020006; nNewItemNum = 15; }
                else if (rnum == 27) { nNewItemID = 4020007; nNewItemNum = 6; }
                else if (rnum == 28) { nNewItemID = 4020008; nNewItemNum = 6; }
                else if (rnum == 29) { nNewItemID = 1032002; nNewItemNum = 1; }
                else if (rnum == 30) { nNewItemID = 1032011; nNewItemNum = 1; }
                else if (rnum == 31) { nNewItemID = 1032008; nNewItemNum = 1; }
                else if (rnum == 32) { nNewItemID = 1102011; nNewItemNum = 1; }
                else if (rnum == 33) { nNewItemID = 1102012; nNewItemNum = 1; }
                else if (rnum == 34) { nNewItemID = 1102013; nNewItemNum = 1; }
                else if (rnum == 35) { nNewItemID = 1102014; nNewItemNum = 1; }
                else if (rnum == 36) { nNewItemID = 2040803; nNewItemNum = 1; }
                else if (rnum == 37) { nNewItemID = 2070011; nNewItemNum = 1; }
                else if (rnum == 38) { nNewItemID = 2043001; nNewItemNum = 1; }
                else if (rnum == 39) { nNewItemID = 2043101; nNewItemNum = 1; }
                else if (rnum == 40) { nNewItemID = 2043201; nNewItemNum = 1; }
                else if (rnum == 41) { nNewItemID = 2043301; nNewItemNum = 1; }
                else if (rnum == 42) { nNewItemID = 2043701; nNewItemNum = 1; }
                else if (rnum == 43) { nNewItemID = 2043801; nNewItemNum = 1; }
                else if (rnum == 44) { nNewItemID = 2044001; nNewItemNum = 1; }
                else if (rnum == 45) { nNewItemID = 2044101; nNewItemNum = 1; }
                else if (rnum == 46) { nNewItemID = 2044201; nNewItemNum = 1; }
                else if (rnum == 47) { nNewItemID = 2044301; nNewItemNum = 1; }
                else if (rnum == 48) { nNewItemID = 2044401; nNewItemNum = 1; }
                else if (rnum == 49) { nNewItemID = 2044501; nNewItemNum = 1; }
                else if (rnum == 50) { nNewItemID = 2044601; nNewItemNum = 1; }
                else if (rnum == 51) { nNewItemID = 2044701; nNewItemNum = 1; }
                else if (rnum == 52) { nNewItemID = 2000004; nNewItemNum = 15; }
                else if (rnum == 53) { nNewItemID = 2000002; nNewItemNum = 80; }
                else if (rnum == 54) { nNewItemID = 2000003; nNewItemNum = 80; }
                else if (rnum == 55) { nNewItemID = 2000006; nNewItemNum = 25; }
                else if (rnum == 56) { nNewItemID = 2022000; nNewItemNum = 25; }
                else if (rnum == 57) { nNewItemID = 2022003; nNewItemNum = 25; }
                else if (rnum == 58) { nNewItemID = 4003000; nNewItemNum = 55; }
                else if (rnum == 59) { nNewItemID = 4010000; nNewItemNum = 12; }
                else if (rnum == 60) { nNewItemID = 4010001; nNewItemNum = 12; }
                else if (rnum == 61) { nNewItemID = 4010002; nNewItemNum = 12; }
                else if (rnum == 62) { nNewItemID = 4010003; nNewItemNum = 12; }
                else if (rnum == 63) { nNewItemID = 4010004; nNewItemNum = 12; }
                else if (rnum == 64) { nNewItemID = 4010005; nNewItemNum = 12; }
                else if (rnum == 65) { nNewItemID = 4010006; nNewItemNum = 8; }
                else if (rnum == 66) { nNewItemID = 4020000; nNewItemNum = 12; }
                else if (rnum == 67) { nNewItemID = 4020001; nNewItemNum = 12; }
                else if (rnum == 68) { nNewItemID = 4020002; nNewItemNum = 12; }
                else if (rnum == 69) { nNewItemID = 4020003; nNewItemNum = 12; }
                else if (rnum == 70) { nNewItemID = 4020004; nNewItemNum = 12; }
                else if (rnum == 71) { nNewItemID = 4020005; nNewItemNum = 12; }
                else if (rnum == 72) { nNewItemID = 4020006; nNewItemNum = 12; }
                else if (rnum == 73) { nNewItemID = 4020007; nNewItemNum = 4; }
                else if (rnum == 74) { nNewItemID = 4020008; nNewItemNum = 4; }
                else if (rnum == 75) { nNewItemID = 2040001; nNewItemNum = 1; }
                else if (rnum == 76) { nNewItemID = 2040004; nNewItemNum = 1; }
                else if (rnum == 77) { nNewItemID = 2040301; nNewItemNum = 1; }
                else if (rnum == 78) { nNewItemID = 2040401; nNewItemNum = 1; }
                else if (rnum == 79) { nNewItemID = 2040501; nNewItemNum = 1; }
                else if (rnum == 80) { nNewItemID = 2040504; nNewItemNum = 1; }
                else if (rnum == 81) { nNewItemID = 2040601; nNewItemNum = 1; }
                else if (rnum == 82) { nNewItemID = 2040601; nNewItemNum = 1; }
                else if (rnum == 83) { nNewItemID = 2040701; nNewItemNum = 1; }
                else if (rnum == 84) { nNewItemID = 2040704; nNewItemNum = 1; }
                else if (rnum == 85) { nNewItemID = 2040707; nNewItemNum = 1; }
                else if (rnum == 86) { nNewItemID = 2040801; nNewItemNum = 1; }
                else if (rnum == 87) { nNewItemID = 2040901; nNewItemNum = 1; }
                else if (rnum == 88) { nNewItemID = 2041001; nNewItemNum = 1; }
                else if (rnum == 89) { nNewItemID = 2041004; nNewItemNum = 1; }
                else if (rnum == 90) { nNewItemID = 2041007; nNewItemNum = 1; }
                else if (rnum == 91) { nNewItemID = 2041010; nNewItemNum = 1; }
                else if (rnum == 92) { nNewItemID = 2041013; nNewItemNum = 1; }
                else if (rnum == 93) { nNewItemID = 2041016; nNewItemNum = 1; }
                else if (rnum == 94) { nNewItemID = 2041019; nNewItemNum = 1; }
                else if (rnum == 95) { nNewItemID = 2041022; nNewItemNum = 1; }
                else if (rnum >= 96 && rnum <= 150) { nNewItemID = 2000004; nNewItemNum = 10; }
                else if (rnum >= 151 && rnum <= 200) { nNewItemID = 2000002; nNewItemNum = 100; }
                else { nNewItemID = 2000003; nNewItemNum = 100; }

                if (!CheckInventory()) return;

                var ret = inven.Exchange(0, nNewItemID, nNewItemNum);
                if (ret == 0) self.Say(StringPool.RewardInventoryFull);
                else
                {
                    var qr = target.QuestRecord;
                    qr.Set(7010, "1");
                    qr.RemoveQuest(7011);
                    target.ChangeMap(221024500, "");
                }
            }
            else self.Say(StringPool.UseEquipFull);
        }
        public static bool CheckInventory()
        {
            var inven = target.Inventory;
            var count1 = inven.ItemCount(4001022);
            if (count1 > 0)
            {
                if (inven.Exchange(0, 4001022, -count1) == 0)
                {
                    self.Say(StringPool.WrongAmount(4001022));
                    return false;
                }
            }
            var count2 = inven.ItemCount(4001023);
            if (count2 > 0)
            {
                if (inven.Exchange(0, 4001023, -count2) == 0)
                {
                    self.Say(StringPool.WrongAmount(4001023));
                    return false;
                }
            }
            return true;
        }
        public static void ClearStage(int exp, string nextStage)
        {
            var quest = FieldSet.Instances["Party3"];
            var field = self.Field;
            field.EffectPartyClear();
            field.EffectObject("gate");
            quest.SetVar("stage", nextStage);
            quest.IncExpAll(exp, 7010);
            if (nextStage != "clear")
            {
                field.EnablePortal("next00", 1);
                self.Say(StringPool.PortalIsOpen);
            }
        }
        public static bool PassesStage(string description, string stage, string nextStage, int exp, int itemCount, int itemID = ItemEtcIds.PassOfDimension)
        {
            if (!CheckStage(stage, true)) return false;

            var inven = target.Inventory;
            if (inven.ItemCount(itemID) < itemCount)
            {
                self.Say(description);
                return false;
            }
            else
            {
                if (inven.Exchange(0, itemID, -itemCount) == 0)
                {
                    self.Say(StringPool.StagePassed(itemID, itemCount));
                    return false;
                }
                ClearStage(exp, nextStage);
                return true;
            }
        }
        public static void party2_stage9()
        {
            bool clear = PassesStage("You've made it this far. Now is your chance to finally get your hands on the real culprit. Go right and you will see a monster. Defeat it to find a monstrous #b#o9300012##k appearing out of nowhere. He will be very agitated by your party's presence, be careful. \r\nYour task is to defeat him, collect the #b#t4001023##k he has and bring it to me. If you manage to take the key from the monster, there is no way the dimensional door can be opened again. I have faith in you. Good luck!", "9", "clear", 8500, 1, ItemEtcIds.KeyOfDimension);

            if (clear)
            {
                var setParty = FieldSet.Instances["Party4"];
                var res = setParty.Enter(target, 0);
                if (res == FieldSet.EnterStatus.Invalid || res == FieldSet.EnterStatus.Full) self.Say("I have reasons for not letting you in. Please try again later.");
                if (res == FieldSet.EnterStatus.NotInParty) self.Say("Looks like you are not in a party. You need to be in a party in order to participate in this quest.");
                return;
            }
        }
        public static void party2_stage8()
        {
            if (!CheckStage("8", true)) return;

            int minOccupiedAreas;
            int areas = 9;
            if (MasterThread.IsDebug) minOccupiedAreas = 2;
            else minOccupiedAreas = 5;
            var quest = FieldSet.Instances["Party3"];
            var question = quest.GetVar("ans");
            string answerInit = string.Join("", Enumerable.Repeat(1, minOccupiedAreas)).PadRight(areas, '0');
            if (question == "")
            {
                quest.SetVar("ans", MasterThread.IsDebug ? answerInit : Tools.Shuffle(1, answerInit));
                self.Say("Welcome to the 8th stage. Here you will find many platforms to climb. #b5#k of them will be connected to the #bportal that leads to the next stage#k. To pass, place #b5 of your party members on the correct platform#k.\r\nA word of warning: You will need to stand firmly in the center of the platform for your answer to count as correct. Also remember that only 5 members can stay on the platform. When this happens, the party leader must #bdouble-click me to know if the answer is correct or not#k. Good luck!");
                return;
            }

            var field = self.Field;

            if (!field.CheckArea(areas, minOccupiedAreas, out string answer)) self.Say($"Looks like you still haven't found the {minOccupiedAreas} correct platforms. Please try a different combination. Remember that you need to have {minOccupiedAreas} members of your party on the platforms, standing in the center, for the answer to be valid. Keep trying!");
            else if (question != answer)
            {
                field.EffectPartyWrong();
            }
            else
            {
                ClearStage(7200, "9");
            }
        }
        public static void party2_stage7()
        {
            PassesStage("Hello! Welcome to the 7th stage. Here you will find a ridiculously powerful monster called #b#o9300010##k. Defeat the monster and get the #b#t4001022##k needed to proceed to the next stage. Please collect #b3 #t4001022#s#k.\r\nTo finish off the monster, defeat it from afar. The only way to attack would be from a long distance, but... oh yes, be careful, #o9300010# is very dangerous. You will definitely get hurt if you are not careful. Good luck!", "7", "8", 6600, 3);
        }
        public static void party2_stage6()
        {
            if (!CheckMembers()) return;

            var qr = target.QuestRecord;
            var val = qr.Get(7011);

            string description = "Hello! Welcome to the 6th stage. Here, you'll see boxes with numbers written on them, and if you stand on the correct box by pressing the UP ARROW, you'll be transported to the next box. I'll give the party leader a clue on how to get past this stage #bonly twice#k and it's the leader's duty to remember the clue and take the right step, one at a time.\r\nOnce you reach the top, you'll find the portal to the next stage. When everyone in your party has passed through the portal, the stage is complete. Everything will depend on remembering the correct boxes.";

            if (val == "e")
            {
                self.Say(description + " I've already given the clue #btwice#k and I can't help you any further. Good luck!");
            }
            else
            {
                if (val == "") val = "1";
                else if (val == "1") val = "e";
                qr.Set(7011, val);
                self.Say(description + " Here is the clue. Make sure to remember it!\r\n\r\n#bOne, 3, 3, 2, middle, 1, three, 3, 3, left, two, 3, 1, one, ?#k");
            }
        }
        public static void party2_stage5()
        {
            PassesStage("Hello! Welcome to the 5th stage. Here you will find many spaces and, inside them, you will find some monsters. Your duty is to collect with the group #b24 #t4001022#s#k. This is the explanation: There will be cases where you need to be of a certain profession or you cannot collect #b#t4001022##k. Therefore, be careful. Here's a clue. There is a monster called #b#o9300013##k that is unbeatable. Only a rogue can get to the other side of the monster. There is also a route that only witches can take. Finding out is up to you. Good luck!", "5", "7", 5400, 24);
        }
        public static void party2_stage4()
        {
            PassesStage("Hello! Welcome to the 4th stage. Here you will find a black space created by the dimensional rift. Inside, you'll find a monster called #b#o9300008##k hiding in the darkness. For this reason, you will barely be able to see it even with your eyes wide open. Defeat the monsters and collect #b6 #t4001022#s#k.\r\nYour party leader must collect all #b#t4001022#s#k from you. Like I said, #b#o9300008##k can't be seen unless you have your eyes wide open. It's a different kind of monster that stealthily merges into the darkness. Good luck!", "4", "5", 4800, 6);
        }
        public static void party2_stage3()
        {
            PassesStage("Hello! Welcome to the 3rd stage. Here you will see a bunch of monsters and boxes. If you defeat the monsters, they will drop #b#t4001022##k, just like monsters from the other dimension. If you break the box, a monster will appear and it will also give #b#t4001022##k.\r\nThe number of #b#t4001022#s#k you need to collect will be determined by the answer to the question I will ask the leader of the your party. The answer to the question will determine the number of #b#t4001022#s#k you will need to collect. Once I ask the party leader the question, they can discuss the answer with the members. Here is the question. Fill in the ??? \r\n\r\n#b The HP of a Level 1 Character - The minimum level needed to job advance as a magician - The minimum level needed to job advance as a rogue = ???#k", "3", "4", 4200, 32);
        }
        public static void party2_stage2()
        {
            PassesStage("Hello! Welcome to the 2nd stage. Walk around the map and find the boxes scattered around the place. Break a crate and you'll be sent to another map or rewarded with a #t4001022#. Search, in each box, collect #b15 #t4001022#s#k and bring them to me. Gather the collected #t4001022#s and give them to your party leader, who will in turn give them to me. Good luck!", "2", "3", 3600, 15);
        }
        public static void party2_stage1()
        {
            PassesStage("Hello! Welcome to the 1st stage. Walk around the map and you'll find various types of monsters roaming around the place. Defeat them all, collect #b25 #t4001022#s#k and bring them to me. Gather the collected #t4001022#s and give them to your party leader, who will in turn give them to me. You might be familiar with these monsters, but they might be more powerful than you'd expect. Therefore, be careful!", "1", "2", 3000, 25);
        }
        public static bool CheckMembers()
        {
            var quest = FieldSet.Instances["Party3"];
            var field = self.Field;
            if (quest.UserCount != field.Characters.Count)
            {
                self.Say("Looks like not all of your party members are present. You will need to bring all members here first to continue. Please find the missing members.");
                return false;
            }
            return true;
        }
        public static bool CheckStage(string st, bool checkAll)
        {
            var quest = FieldSet.Instances["Party3"];
            var stage = quest.GetVar("stage");
            if (stage == "")
            {
                stage = "1";
                quest.SetVar("stage", stage);
            }

            if (stage != st)
            {
                self.Say("Congratulations on completing the quests for this stage. Please use the portal you see ahead and proceed to the next stage.");
                return false;
            }

            if (checkAll && !CheckMembers())
            {
                return false;
            }
            return true;
        }
        public static void party2_help()
        {
            var field = self.Field;
            if (field.ID == 922010100) self.Say("Here is information about the 1st stage. You'll see monsters at different points on the map. These monsters have an item called #b#t4001022##k, which opens the door to another dimension. With it, you can take a step closer to the top of the Tower of Eos, where the door to the other dimension will open and, finally, you will find the culprit for everything.\r\nDefeat the monsters, collect #b25 #t4001022 #s#k and give it to your group leader, who will in turn give it to me. This will take you to the next stage. Good luck!");
            else if (field.ID == 922010200) self.Say("Here is information about the 2nd stage. You'll see crates all over the map. Break a box and you will be #Genviated:sent# to another map or #Grecompensated:rewarded# with a #t4001022#. Search each box, collect #b15 #t4001022#s#k and bring them all to me. Gather the collected #t4001022#s, give them all to your group leader, who, in turn, will give them to me.\r\nBy the way, even if you are #Genviado:sent# to another place, you can find another box there . So, don't just leave the strange place you went to #G teleported:teleported# If you just leave there, you can't go back and you'll need to start the quest from the beginning. Good luck!");
            else if (field.ID == 922010300) self.Say("Here is information about the 3rd stage. Here you will see a bunch of monsters and boxes. If you defeat the monsters, they will drop #b#t4001022##k, just like monsters from the other dimension. If you break the box, a monster will appear and it will also give #b#t4001022##k.\r\nThe number of #b#t4001022#s#k you need to collect will be determined by the answer to the question I will ask the leader of the your group. The answer to the question will determine the number of #b#t4001022#s#k you will need to collect. Once I ask the group leader the question, he can discuss the answer with the members. Good luck!");
            else if (field.ID == 922010400) self.Say("Here is information about the 4th stage. Here you will find a black space created by the dimensional rift. Inside, you'll find a monster called #b#o9300008##k hiding in the darkness. For this reason, you will barely be able to see it if you don't have your eyes wide open. Defeat the monsters and collect #b6 #t4001022#s#k.\r\nLike I said, #b#o9300008##k cannot be seen unless your eyes are wide open. It's a different kind of monster that stealthily merges into the darkness. Good luck!");
            else if (field.ID == 922010500) self.Say("Here is information about the 5th stage. Here you will find many spaces and, inside them, you will find some monsters. Your duty is to collect with the group #b24 #t4001022#s#k. This is the description: There will be cases where you need to be of a certain profession, or you cannot collect #b#t4001022##k. Therefore, be careful. Here's a clue. There is a monster called #b#o9300013##k that is unbeatable. Only a rogue can get to the other side of the monster. There is also a route that only witches can take. Finding out is up to you. Good luck!");
            else if (field.ID == 922010600) self.Say("Here is the information about the 6th stage. Here, you'll see boxes with numbers written on them, and if you stand on top of the correct box by pressing the UP ARROW, you'll be transported to the next box. I'll give the party leader a clue on how to get past this stage #bonly twice#k and it's the leader's duty to remember the clue and take the right step, one at a time.\r\nOnce you reach the top, you'll find the portal to the next stage. When everyone in your party has passed through the portal, the stage is complete. Everything will depend on remembering the correct boxes. Good luck!!");
            else if (field.ID == 922010700) self.Say("Here is information about the 7th stage. Here you will find a ridiculously powerful monster called #b#o9300010##k. Defeat the monster and find the #b#t4001022##k needed to proceed to the next stage. Please collect #b3#t4001022#s#k.\r\nTo finish off the monster, defeat it from afar. The only way to attack would be from a long distance, but... oh yes, be careful, #o9300010# is very dangerous. You will definitely get hurt if you are not careful. Good luck!");
            else if (field.ID == 922010800) self.Say("Here is information about the 8th stage. Here you will find many platforms to climb. #b5#k of them will be connected to the #bportal that leads to the next stage#k. To pass, place #b5 of your party members on the correct platform#k.\r\nA word of warning: You will need to stand firmly in the center of the platform for your answer to count as correct. Also remember that only 5 members can stay on the platform. When this happens, the group leader must #bclick me twice to know if the answer is correct or not#k. Good luck!");
            else if (field.ID == 922010900) self.Say("Here is the information about the 9th stage. Now is your chance to finally get your hands on the real culprit. Go right and you'll see a monster. Defeat it to find a monstrous #b#o9300012##k appearing out of nowhere. He will be very agitated by your group's presence, be careful. \r\nYour task is to defeat him, collect the #b#t4001023##k he has and bring it to me. If you manage to take the key away from the monster, there is no way the dimensional door can be opened again. I have faith in you. Good luck!");
            else if (field.ID == 922011000) self.Say("Welcome to the bonus stage. I can't believe you actually defeated #b#o9300012##k! Incredible! But we don't have much time, so I'll get right to the point. There are many boxes here. Your task is to break the boxes within the time limit and get the items inside. If you're lucky, you might even snag a great item here and there. If this doesn't excite you, I don't know what will. Good luck!!");
        }
        public static void party2_takwawayitem()
        {
            var inven = target.Inventory;

            var count1 = inven.ItemCount(4001022);
            if (count1 > 0) inven.Exchange(0, 4001022, -count1);
            var count2 = inven.ItemCount(4001023);
            if (count2 > 0) inven.Exchange(0, 4001023, -count2);

            return;
        }
        // Ç¥ÁöÆÇ : 2040034
        [Script("party2_enter")]
        class party2_enter : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                party2.self = self;
                party2.target = target;
                if (!target.IsPartyBoss)
                {
                    self.Say("From this point on, this place is full of dangerous obstacles and monsters. For this reason, I cannot let you go any further. However, if you're interested in saving us and bringing peace back to Ludibrium, that's another story. If you want to defeat a powerful creature that dwells on the summit, please gather your party members. It won't be easy, but... I think you can do it.");
                    return;
                }

                var setParty = FieldSet.Instances["Party3"];
                var res = setParty.Enter(target, 0);
                if (res == FieldSet.EnterStatus.Invalid) self.Say("I have reasons for not letting you in. Please try again later.");
                else if (res == FieldSet.EnterStatus.NotInParty) self.Say("You are not in a party. You can only participate in this quest if you are in a party.");
                else if (res == FieldSet.EnterStatus.WrongMemberCount) self.Say("Your party cannot participate in the quest because it does not have 6 members. Please gather 6 people in your party.");
                else if (res == FieldSet.EnterStatus.TooWeak) self.Say("Someone in your party is not between levels 35~50. Please check again.");
                else if (res == FieldSet.EnterStatus.Full) self.Say("Another party is inside participating in the quest. Please try again after the party opens the vacancy.");
                else
                {
                    party2_takwawayitem();
                }
            }
        }
        // ¹ú·é - °ÔÀÓ ÁøÇà NPC
        [Script("party2_play")]
        class party2_play : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                party2.self = self;
                party2.target = target;
                var field = self.Field;
                var quest = FieldSet.Instances["Party3"];

                // º¸»ó
                if (field.ID == 922011100)
                {
                    party2_reward();
                    return;
                }

                if (!target.IsPartyBoss) party2_help();
                else
                {
                    if (field.ID == 922010100) party2_stage1();
                    else if (field.ID == 922010200) party2_stage2();
                    else if (field.ID == 922010300) party2_stage3();
                    else if (field.ID == 922010400) party2_stage4();
                    else if (field.ID == 922010500) party2_stage5();
                    else if (field.ID == 922010600) party2_stage6();
                    else if (field.ID == 922010700) party2_stage7();
                    else if (field.ID == 922010800) party2_stage8();
                    else if (field.ID == 922010900) party2_stage9();
                    else if (field.ID == 922011000) party2_help();
                }
            }
        }
        // º´Á¤ ¾Ø´õ½¼ - ÅðÀå½ÃÄÑÁÖ´Â NPC
        [Script("party2_out")]
        class party2_out : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                party2.self = self;
                party2.target = target;
                var qr = target.QuestRecord;
                var field = self.Field;

                if (field.ID == 922010000)
                {
                    if (!CheckInventory()) return;
                    qr.RemoveQuest(7011);
                    target.ChangeMap(221024500, "");
                }
                else
                {
                    // °¢ ½ºÅ×ÀÌÁö¿¡¼­ ÅðÀå¸ÊÀ¸·Î º¸³½´Ù
                    var nRet = self.AskYesNo("You'll need to start from scratch if you want to take a chance on this quest after leaving this stage. Are you sure you want to leave this map?");
                    if (nRet == 0) self.Say("I understand. Gather the strength of your party members and push yourself further!");
                    else
                    {
                        qr.RemoveQuest(7011);
                        target.ChangeMap(922010000, "");
                    }
                }
            }
        }
    }

    #region Reactors
    [Script("ludiquest3")]
    public class LudiQuest3 : IReactorScript
    {
        public void Run(IReactorHost host, FieldReactor target)
        {
            target.Drop();
        }
    }
    [Script("go922010201")]
    public class GoTrap : IReactorScript
    {
        public void Run(IReactorHost host, FieldReactor target)
        {
            target.Owner.ChangeMap(922010201);
            //target.Owner.Message("An unknown force has warped you into a trap.");
        }
    }
    [Script("ludiMob1")]
    public class LudiMob1 : IReactorScript
    {
        public void Run(IReactorHost host, FieldReactor target)
        {
            target.SpawnMob(new Pos(0, 0), (9300007, 3, SummonType.Poof, null));
        }
    }
    [Script("ludiMob2")]
    public class LudiMob2 : IReactorScript
    {
        public void Run(IReactorHost host, FieldReactor target)
        {
            var offset = new Pos((short)(1 - target.Position.X), (short)(-211 - target.Position.Y));
            target.SpawnMob(offset, (9300010, 1, SummonType.Poof, null));
            target.Field.Message("Rombard is summoned somewhere in the map.");
        }
    }
    [Script("ludiBoss0")]
    public class LudiBoss0 : IReactorScript
    {
        public void Run(IReactorHost host, FieldReactor target)
        {
            var offset = new Pos((short)(941 - target.Position.X), (short)(184 - target.Position.Y));
            target.SpawnMob(offset, (9300012, 1, SummonType.Poof, null));
            target.Field.Message("Alishar is summoned.");
        }
    }
    [Script("ludiquest4")]
    public class LudiQuest4 : IReactorScript
    {
        public void Run(IReactorHost host, FieldReactor target)
        {
            target.Drop();
        }
    }
    #endregion
}
