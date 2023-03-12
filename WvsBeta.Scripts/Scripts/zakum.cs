using System;
using WvsBeta.Common;
using WvsBeta.Game;
using WvsBeta.Game.Packets;
using WvsBeta.Game.Scripting;

namespace WvsBeta.Scripts.Scripts
{
    #region NPC
    public static class zakum
    {
        static INpcHost self;
        static GameCharacter target;
        static dynamic sTime, qr, ret, cTime, nItem1, cName, inven, v0, aTime, nItem3, nItem2;
        public static void zakum_normal()
        {
            qr = target.QuestRecord;
            cName = self.GetStrReg("name");
            sTime = self.GetStrReg("time");
            cTime = MasterThread.CurrentTimeStr;

            if (target.Name == cName)
            {
                aTime = MasterThread.CompareTime(cTime, sTime);
                if (aTime <= 5)
                {
                    v0 = self.AskMenu("What can I do for you?\r\n#b#L0# Clear 1st stage of Zakum#l\r\n#L1# Clear 2nd Stage of Zakum#l\r\n#L2# Clear 3rd Stage of Zakum and get the items#l\r\n#L3# Return to uncleared status#l\r\n#L4# Normal proceed#l");
                    if (v0 == 0)
                    {
                        qr.Set(7000, "end");
                        self.Say("You have cleared the 1st stage.");
                    }
                    else if (v0 == 1)
                    {
                        qr.Set(7001, "end");
                        self.Say("You have cleared the 2nd stage.");
                    }
                    else if (v0 == 2)
                    {
                        qr.Set(7002, "end");
                        inven = target.Inventory;
                        ret = inven.Exchange(0, 4001017, 1);
                        if (ret == 0) self.Say("You have cleared the quests but couldn't receive the items due to lack of space in invertory.");
                    }
                    else if (v0 == 3)
                    {
                        qr.Set(7000, "");
                        qr.Set(7001, "");
                        qr.Set(7002, "");
                    }
                    else if (v0 == 4)
                    {
                        return;
                    }
                }
            }
            return;
        }
        public static void zakum_master()
        {
            qr = target.QuestRecord;
            self.Say("Hello GM~ For your better GM life, you can clear the zakum quest by stages.");
            v0 = self.AskMenu("What can I do for you?\r\n#b#L0# Clear 1st stage of Zakum#l\r\n#L1# Clear 2nd Stage of Zakum#l\r\n#L2# Clear 3rd stage of zakum and get the items#l\r\n#L3# Return to the uncleared status#l\r\n#L4# Normal proceed#l\r\n#L5# Set up other characters#l\r\n#L6# Cancel setting availability#l");
            if (v0 == 0)
            {
                qr.Set(7000, "end");
                self.Say("You have been set to clear the 1st stage.");
            }
            else if (v0 == 1)
            {
                qr.Set(7001, "end");
                self.Say("You have been set to clear the 2nd stage.");
            }
            else if (v0 == 2)
            {
                qr.Set(7002, "end");
                inven = target.Inventory;
                ret = inven.Exchange(0, 4001017, 1);
                if (ret == 0) self.Say("You have clear the quest but couldn't receive the items due to lack of space in inventory.");
            }
            else if (v0 == 3)
            {
                qr.Set(7000, "");
                qr.Set(7001, "");
                qr.Set(7002, "");
            }
            else if (v0 == 4)
            {
                return;
            }
            else if (v0 == 5)
            {
                cName = self.AskText("Please enter the neme of the character you wish to set the clearance.", "", 0, 16);
                self.SetStrReg("name", cName);
                self.SetStrReg("time", MasterThread.CurrentTimeStr);
                self.Say("#b" + cName + "#k has 5 minutes to set the clear of the quests.");
            }
            else if (v0 == 6)
            {
                self.SetStrReg("name", "");
                self.SetStrReg("time", "");
            }
            return;
        }
        public static void zakum_takeawayitem()
        {
            var inven = target.Inventory;

            nItem1 = inven.ItemCount(4001015);
            nItem2 = inven.ItemCount(4001016);
            nItem3 = inven.ItemCount(4001018);

            if (nItem1 > 0) inven.Exchange(0, 4001015, -nItem1);
            if (nItem2 > 0) inven.Exchange(0, 4001016, -nItem2);
            if (nItem3 > 0) inven.Exchange(0, 4001018, -nItem3);

            return;
        }
        // Adobis
        [Script("Zakum00")]
        class Zakum00 : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                zakum.self = self;
                zakum.target = target;

                //    if ( serverType == 1 ) { //개발서버 테스트용
                if (Server.Tespia)
                {
                    if (target.Job == 500)
                    {
                        zakum_master();
                    }
                    else
                    {
                        zakum_normal();
                    }
                }

                var qr = target.QuestRecord;

                var val1 = qr.Get(7000);
                var val2 = qr.Get(7001);
                var val3 = qr.Get(7002);
                int nRet;

                if (qr.GetState(6273) == 2)
                {
                    nRet = self.AskMenu("Well ... alright. You seem more than qualified for this. Which of these tasks do want to tackle on?\r\n#b#L0# Explore the Dead Mine. (Level 1)#l\r\n#b#L1# Observe the Zakum Dungeon. (Level 2)#l\r\n#b#L2# Request for a refinery. (Level 3)#l\r\n#b#L3# Get briefed for the quest.#l\r\n#L4# Enter the center of Lava.#l");
                }
                else nRet = self.AskMenu("Well ... alright. You seem more than qualified for this. Which of these tasks do want to tackle on?\r\n#b#L0# Explore the Dead Mine. (Level 1)#l\r\n#b#L1# Observe the Zakum Dungeon. (Level 2)#l\r\n#b#L2# Request for a refinery. (Level 3)#l\r\n#b#L3# Get briefed for the quest.#l");
                if (nRet == 0)
                {
                    if (!target.IsPartyBoss)
                    {
                        self.Say("This journey will be a neverending maze of quests you won't be able to solve by yourself. But if you're willing to take on the challenge, then talk to the chief of your occupation at the Chief's Residence in El Nath to receive the quest.");
                        self.Say("After receiving the quest, either join a party or form one yourself, and have the leader of the party speak to me to start the quest. Once you are ready, have the leader of the party come up and talk to me.");
                        return;
                    }
                    else
                    {
                        if (qr.Get(7006) != "")
                        {
                            string today = MasterThread.CurrentTimeStr.Substring(0, 8);
                            int cNum = int.Parse(qr.Get(7006).Substring(0, 1));
                            string cDate = qr.Get(7006).Substring(1, 8);
                            if (cDate == today)
                            {
                                if (cNum >= 3)
                                {
                                    self.Say("You have already looked through the Cave at the Dead Mine three times today and therefore I cannot let you in once more. Please come back tomorrow.");
                                    return;
                                }
                            }
                            else
                            {
                                qr.Set(7006, "");
                            }
                        }

                        var setParty = FieldSet.Instances["Party2"];
                        var res = setParty.Enter(target, 0);
                        if (res == FieldSet.EnterStatus.Invalid) self.Say("I can't let you in for unknown reasons. Please try again later.");
                        else if (res == FieldSet.EnterStatus.NotInParty) self.Say("You are not currently in a party right now. You may only tackle this assignment as a party.");
                        else if (res == FieldSet.EnterStatus.WrongMemberCount) self.Say("Not all members are currently present. Please bring all members here before tackling this assignment.");
                        else if (res == FieldSet.EnterStatus.TooWeak) self.Say("There's a member of your party that hasn't received the quest from the chief of the occupation at El Nath. Every single one of the party members must receive the quest from the chiefs of their respective occupation in order to do this.");
                        else if (res == FieldSet.EnterStatus.Full) self.Say("An another party has already started this quest. Please try again later.");
                        else
                        {
                            zakum_takeawayitem();
                            setParty.SetVar("ctime", MasterThread.CurrentTimeStr);
                        }
                    }
                }

                else if (nRet == 1)
                {
                    if (val1 == "end")
                    {
                        int nRet2 = 0;
                        if (val2 == "") nRet2 = self.AskYesNo("You have safely cleared the 1st stage. There's still a long way to go before meeting the boss of Zakum Dungeon, however. So, what do you think? Are you ready to move on to the next stage?");
                        else if (val2 == "s") nRet2 = self.AskYesNo("Hmmm ... you must have tried this quest before and gave up midway through. What do you think? Do you want to retry this level?");
                        else if (val2 == "end") nRet2 = self.AskYesNo("Hmmm ... You have already cleared this level before.  For you to be rewarded again, you need to restart the quest from Level 1. Otherwise, you will still be able to do the quest but will not be rewarded. Do you still want to retry this level?");

                        if (nRet2 == 0) self.Say("I see ... but if you ever decide to change your mind, then talk to me.");
                        else
                        {
                            //하루3회
                            if (qr.Get(7007) != "")
                            {
                                string today = MasterThread.CurrentTimeStr.Substring(0, 8);
                                int cNum = int.Parse(qr.Get(7007).Substring(0, 1));
                                string cDate = qr.Get(7007).Substring(1, 8);
                                if (cDate == today)
                                {
                                    if (cNum >= 3)
                                    {
                                        self.Say("You have already completed the exploration of the Zakum Dungeon three times today, and therefore I cannot let you explore once more. Please come back tomorrow.");
                                        return;
                                    }
                                }
                                else
                                {
                                    qr.Set(7007, "");
                                }
                            }

                            self.Say("Alright! From here on out, you'll be transported to the map where obstacles will be aplenty. There will be a person standing at the deepest part of the map, and if you talk to her, you'll find an item that will be used as a material to create an item that summons the boss of Zakum Dungeon. Please get me that item. Good luck!");
                            if (val2 != "end") qr.Set(7001, "s");

                            //로그남기기
                            cTime = MasterThread.CurrentTimeStr;
                            qr.Set(7005, cTime);
                            target.ChangeMap(280020000, "");
                        }
                    }
                    else if (val1 == "fail") self.Say("It seems like you're in the middle of the 1st stage. You must first clear this one before moving on to Level 2. Please clear the 1st stage first.");
                    else self.Say("It doesn't look like you have cleared the previous stage, yet. Please beat the previous stage before moving onto the next level.");
                }
                else if (nRet == 2)
                {
                    if (val2 == "end")
                    {
                        if (val3 == "s" || val3 == "s2")
                        {
                            var inventory = target.Inventory;
                            if (inventory.ItemCount(4000082) >= 30)
                            {
                                self.Say("Ha ha ha, don't worry, I'll make it in a heartbeat!");
                                ret = inventory.Exchange(0, 4000082, -30, 4031061, -1, 4031062, -1, 4001017, 5);
                                if (ret == 0) self.Say("Hmmm... are you sure you have all the items required to make #rEye of Fire#k with you? If so, then please check and see if your etc. inventory is full or not.");
                                else
                                {
                                    qr.Set(7002, "end");
                                    self.Say("Here it is. You will now be able to enter the alter of the Zakum Dungeon when the door on the left is open.. You'll need #b#t4001017##k with you in order to go through the door and enter the stage. Now, let's see how many can enter this place ...?");
                                }
                            }
                            else self.Say("I don't think you have #b30 Zombie's Lost Gold Teeth#k yet. Gather them all up and I may be able to refine them and make a special item for you ...");
                        }
                        else if (val3 == "end")
                        {
                            int nRet3 = self.AskYesNo("Hmmm ... aren't you the one who refined #b#t4001017##k before? Then what can I do for you? Are you interested in mixing #b#t4031061##k with #b#t4031062##k again to create #b#t4001017##k?");
                            if (nRet3 == 0) self.Say("I see ... but please be aware that you won't be able to see the boss of Zakum Dungeon without the #b#t4001017##k.");
                            else
                            {
                                qr.Set(7002, "s2");
                                self.Say("Hmmm, by mixing #b#t4031061##k with #b#t4031062##k, I can make an the item that will be used as a sacrifice to summon the boss, called #b#t4001017##k. The problem is ... (cough cough) as you can see, I am not feeling terribly well these days, so it's difficult for me to move around and gather up items. Well ... will it be ok for you to gather up #b30 Zombie's Lost Gold Teeth#k for me? Don't ask me where I'll be using it, though ...");
                            }
                        }
                        else
                        {
                            qr.Set(7002, "s");
                            self.Say("Hmmm, by mixing #b#t4031061##k with #b#t4031062##k, I can make an the item that will be used as a sacrifice to summon the boss, called #b#t4001017##k. The problem is ... (cough cough) as you can see, I am not feeling terribly well these days, so it's difficult for me to move around and gather up items. Well ... will it be ok for you to gather up #b30 Zombie's Lost Gold Teeth#k for me? Don't ask me where I'll be using it, though ...");
                        }
                    }
                    else self.Say("Hmmm ... I don't think you have cleared the previous stage, yet. Please beat the previous stage before moving onto the next level.");
                }
                else if (nRet == 3)
                {
                    self.Say("Not sure where to start? In order to do this quest, you'll have to receive the approval from the chief of your occupation. I do not want to be scolded later on for letting someone in without going through the proper procedure. The only ones that I can let in are the party full of members that have received the approval.");
                    self.Say("Complete the quests in order of the level, and you'll be able to meet the boss of the Zakum Dungeon. Gather up the items I'll request from you, and I'll make them into a sacrificial item. Place the sacrificial item at the altar, and you'll get to see what you've come to see. To do that, first look through the Dead Mine and bring back #b#t4001018##k.");
                    self.Say("There, other than #b#t4001018##k, you'll also find Paper documents. Give that to #b#p2032002##k, and you may get something helpful in return along with Piece of Fire ore. Next, go across the lava area and find #b#t4031062##k. It'll be a treacherous road to take, but ... it's a must item, in terms of making a sacrificical item.");
                    self.Say("Once you have gotten #b#t4031062##k, you'll need to refine the #bPieces of Fire ore#k and #b#t4031062#s#k that you have acquired at level 1 and 2. Don't worry about it, though; I can refine them for you. Once you've completed them all, all you'll have left to do is to meet the boss of Zakum Dungeon. It won't be easy, at all ... but try your best.");
                }
            }
        }
        // Aura
        [Script("Zakum01")]
        class Zakum01 : INpcScript
        {
            dynamic qr, nItem, nRet, result, nRet2, inventory, nRet3;
            public void Run(INpcHost self, GameCharacter target)
            {
                zakum.self = self;
                zakum.target = target;
                qr = target.QuestRecord;
                var quest = FieldSet.Instances["Party2"];
                result = quest.GetVar("zakum");

                if (result == "yes") self.Say("You have safely passed the test and gotten the #b#t4001018##k to me. Now, enter the portal next to me, and every member of the party will receive the refined #b#t4001018##k, #b#t4031061##k. Since you also have collected more than #b30 #t4001015#s#k in the process, you are also eligible to receive #b#t2030007##k. The portal is open now.");
                else if (result == "no") self.Say("You have safely passed the test and gotten the #b#t4001018##k to me. Now, enter the portal next to me, and every member of the party will receive the refined #b#t4001018##k, #b#t4031061##k. The portal is open now.");
                else
                {
                    self.Say("You are the one who wanted to investigate the Dead Mine. You need to gather up the necessary items to reach the point of your final goal: meeting the boss of the Zakum Dungeon. To obtain that item, you'll first need to acquire the materials for that item, right? You can get one of the materials, #b#t4001018##k, right here. It won't be easy, though ...");
                    self.Say("Here, there is an entrance that leads to numerous caves. Once inside the cave, you'll see some boxes. Destroy them all, and collect #b7 of #t4001016#s#k. The box cannot be destroyed using attack skills; only the regular, basic attack works. Afterwards, gather up the 7 keys, move into the innermost room, where the treasure chest is. Drop the keys there to obtain #b#t4001018##k. It'll take some time after dropping the keys to obtain it, so be patient.");
                    self.Say("Of course, not every box contains #t4001016#. You'll all run into some very unexpected circumstances, so please be aware of that. Every once in a while, in the middle of going through the boxes, #t4001015# will pop out. Gather those up, too, and something good will definitely happen. You need to collect at least 30 #t4001015#s. This is all I can tell you, for now.");
                    nRet = self.AskMenu("Anything do you want to ask?\r\n#b#L0# I brought #t4001018##k.#l\r\n#b#L1# Forget the quest, I'm out of here.#l");
                    if (nRet == 0)
                    {
                        if (!target.IsPartyBoss) self.Say("Once you have obtained #b#t4001018##k by dropping 7 #b#t4001016#s#k at the huge chest in the cave, please hand the item over to the party leader. Once the leader of the party has #b#t4001018##k in possession and talks to me, that'll signal that you have cleared Level 1.");
                        else
                        {
                            inventory = target.Inventory;

                            if (inventory.ItemCount(4001018) >= 1)
                            {
                                nItem = inventory.ItemCount(4001015);
                                if (nItem == 0) nRet2 = self.AskYesNo("You brought back #b1 #t4001018##k safely, but it doesn't look like you have brought #b#t4001015# back. Is this all your party has gathered up?");
                                else nRet2 = self.AskYesNo("You have brought back #b1 #t4001018##k and #b" + nItem + " #t4001015#s#k. Is this all the items your party members have gathered up?");

                                if (nRet2 == 0) self.Say("All the items collected from the cave by the party members should be given to the party leader, who'll give them all to me. Please double-check.");
                                else
                                {
                                    if (nItem == 0) nRet3 = inventory.Exchange(0, 4001018, -1);
                                    else nRet3 = inventory.Exchange(0, 4001018, -1, 4001015, -nItem);

                                    if (nRet3 == 0)
                                    {
                                        if (nItem == 0) self.Say("Please check and see if you have #b1 #t4001018##k with you.");
                                        else self.Say("Please check and see if you really have #b1 #t4001018##k and #b" + nItem + " #t4001015##k with you.");
                                    }

                                    else
                                    {
                                        if (nItem >= 30) quest.SetVar("zakum", "yes");
                                        else quest.SetVar("zakum", "no");

                                        self.Say("Alright. Using the portal that's been made down there, you can return to the map where Adobis is. While using the portal, I'll be handing out #b#t4031061##k made out of #b#t4001018##k you've all given me to each and every member of the party. Congratulations on clearing Level 1. See you around ...");
                                    }
                                }
                            }
                            else self.Say("I guess you haven't gotten #b#t4001018##k yet. Please go through the various treasure chests in here within the time limit, collect #b7 of #t4001016#s#k, and drop them all at the treasure chest in the innermost part of the cave to collect #b#t4001018##k. Once you have obtained the item, please hand it to me.");
                        }
                    }
                    else if (nRet == 1)
                    {
                        nRet2 = self.AskYesNo("If you quit in the middle of a mission, you'll have to start all over again ... not only that, but since it's a party quest, even if one player decides to leave, it may be difficult to clear the level. Are you SURE you want to leave?");
                        if (nRet2 != 0)
                        {
                            self.Say("Alright, I'll send you to the Exit Map. #b#p2030011##k will be there standing. Go talk to him; He'll let you out. So long ...");
                            qr.Set(7000, "fail");
                            target.ChangeMap(280090000, "");
                        }
                    }
                }
            }
        }
        // Lira
        [Script("Zakum02")]
        class Zakum02 : INpcScript
        {
            dynamic eTime, rTime, strLog, cDate, ret, val, inven, today, cNum;
            public void Run(INpcHost self, GameCharacter target)
            {
                zakum.self = self;
                zakum.target = target;
                var qr = target.QuestRecord;

                self.Say("How did you go through such treacherous road to get here?? Incredible! #b#t4031062##k is here. Please give this to my brother. You'll finally be meeting up with the one you've been looking for, very soon.");

                inven = target.Inventory;
                eTime = qr.Get(7005); //입장시간
                rTime = MasterThread.CurrentTimeStr; //현재시간
                int result = MasterThread.CompareTime(rTime, eTime);

                if (result > 2)
                {
                    ret = inven.Exchange(0, 4031062, 1);
                    if (ret == 0) self.Say("Your etc. inventory seems to be full. Please make room in order to receive the item.");
                    else
                    {
                        //하루 세번
                        val = qr.Get(7007);
                        today = MasterThread.CurrentTimeStr.Substring(0, 8);
                        if (val == "")
                        {
                            qr.Set(7007, "1" + today);
                        }
                        else
                        {
                            cNum = int.Parse(val.Substring(0, 1)) + 1;
                            cDate = val.Substring(1, 8);
                            qr.Set(7007, cNum + cDate);
                        }

                        target.IncEXP(15000, 0);
                        qr.Set(7001, "end");
                        if (qr.Get(7005) != "") qr.RemoveQuest(7005);
                        target.ChangeMap(211042300, "");
                    }
                }
                else
                {
                    strLog = "[" + target.Name + "] zakum : " + result.ToString() + "min(s)";
                    self.Log(strLog);

                    target.Message("For completing this quest way too early, you will not be rewarded for your effort.");
                    target.ChangeMap(211042300, "");
                }
            }
        }
        // Portal
        [Script("Zakum03")]
        class Zakum03 : IPortalScript
        {
            dynamic charName, clear, qr, cDate, result, nRet, val, inventory, today, cNum;
            public void Run(IPortalHost self, GameCharacter target)
            {
                var quest = FieldSet.Instances["Party2"];
                result = quest.GetVar("zakum");
                qr = target.QuestRecord;

                inventory = target.Inventory;

                charName = target.SCharacterName + "_";
                clear = quest.GetVar(charName);

                if (result == "yes" || result == "no")
                {
                    if (quest.GetVar("tcheck") != "1")
                    {
                        int atime = MasterThread.CompareTime(MasterThread.CurrentTimeStr, quest.GetVar("ctime"));
                        if (atime > 2)
                        {
                            quest.SetVar("tresult", "1");
                        }
                        quest.SetVar("tcheck", "1");
                    }
                }

                if (quest.GetVar("tresult") == "1")
                {
                    if (result == "yes")
                    {
                        nRet = inventory.Exchange(0, 4031061, 1, 2030007, 5);
                        if (nRet == 0) target.Message("Your etc. or use inventory is full, so you can't move on to the next map.");
                        else
                        {
                            //하루에 세번
                            if (target.IsPartyBoss)
                            {
                                val = qr.Get(7006);
                                today = MasterThread.CurrentTimeStr.Substring(0, 8);
                                if (val == "")
                                {
                                    qr.Set(7006, "1" + today);
                                }
                                else
                                {
                                    cNum = int.Parse(val.Substring(0, 1)) + 1;
                                    cDate = val.Substring(1, 8);
                                    qr.Set(7006, cNum + cDate);
                                }
                            }

                            qr.Set(7000, "end");
                            if (clear == "")
                            {
                                target.IncEXP(20000, 0);
                                quest.SetVar(charName, "clear");
                            }
                            target.PlayPortalSE();
                            target.ChangeMap(280090000, "");
                        }
                    }
                    else if (result == "no")
                    {
                        nRet = inventory.Exchange(0, 4031061, 1);
                        if (nRet == 0) target.Message("Your etc. inventory is full, so you can't move on to the next map.");
                        else
                        {
                            if (target.IsPartyBoss)
                            {
                                val = qr.Get(7006);
                                today = MasterThread.CurrentTimeStr.Substring(0, 8);
                                if (val == "")
                                {
                                    qr.Set(7006, "1" + today);
                                }
                                else
                                {
                                    cNum = int.Parse(val.Substring(0, 1)) + 1;
                                    cDate = val.Substring(1, 8);
                                    qr.Set(7006, cNum + cDate);
                                }
                            }

                            qr.Set(7000, "end");
                            if (clear == "")
                            {
                                target.IncEXP(12000, 0);
                                quest.SetVar(charName, "clear");
                            }
                            target.PlayPortalSE();
                            target.ChangeMap(280090000, "");
                        }
                    }
                    else target.Message("The portal is closed for now. You'll need to clear level 1 first before moving on.");
                }
                else
                {
                    if (result == "yes" || result == "no")
                    {
                        target.Message("For completing this quest way too early, you will not be rewarded for your effort.");
                        target.PlayPortalSE();
                        target.ChangeMap(280090000, "");
                    }
                    else
                    {
                        target.Message("The portal is closed for now. You'll need to clear level 1 first before moving on.");
                    }
                }

            }
        }
        // Ali : Exit Map
        [Script("Zakum04")]
        class Zakum04 : INpcScript
        {
            dynamic qr, nItem1, val, inven, nItem3, nItem2;
            public void Run(INpcHost self, GameCharacter target)
            {
                zakum.self = self;
                zakum.target = target;
                qr = target.QuestRecord;
                val = qr.Get(7000);

                if (val == "end") self.Say("Great job clearing level 1! Alright ... I'll send you off to where #b#p2030008##k is. Before that!! Please be aware that the various, special items you have acquired here will not be carried out of here. I'll be taking away those items from your item inventory, so remember that. See ya!");
                else self.Say("Must have quit midway through. Alright, I'll send you off right now. Before that!! Please be aware that the various, special items you have acquired here will not be carried out of here. I'll be taking away those items from your item inventory, so remember that. See ya!");

                inven = target.Inventory;

                nItem1 = inven.ItemCount(4001015);
                nItem2 = inven.ItemCount(4001016);
                nItem3 = inven.ItemCount(4001018);

                if (nItem1 > 0) inven.Exchange(0, 4001015, -nItem1);
                if (nItem2 > 0) inven.Exchange(0, 4001016, -nItem2);
                if (nItem3 > 0) inven.Exchange(0, 4001018, -nItem3);

                target.ChangeMap(211042300, "");
            }
        }
        // The portal that sends you to the boss map
        [Script("Zakum05")]
        class Zakum05 : IPortalScript
        {
            public void Run(IPortalHost self, GameCharacter target)
            {
                int sChannel;
                if (Server.Tespia) sChannel = 0;
                else sChannel = 2;

                if (Server.Instance.ID == sChannel)
                {
                    var qr = target.QuestRecord;
                    var val = qr.Get(7002);
                    var val2 = qr.Get(7003);
                    var val3 = qr.Get(7004);

                    var instance = FieldSet.Instances["ZakumBoss"];
                    var started = instance.Started;
                    var users = instance.UserCount;

                    var inven = target.Inventory;

                    if ((val == "end" || val == "s2") && inven.ItemCount(4001017) >= 1)
                    {
                        cTime = MasterThread.CurrentTimeStr;
                        aTime = MasterThread.CompareTime(cTime, val2);
                        // after 24 hrs
                        if (aTime >= 1440)
                        {
                            if (started) target.Message("The battle against the boss has already begun, so you can't go in.");
                            else
                            {
                                if (val == "s2") qr.Set(7002, "end");
                                target.PlayPortalSE();
                                target.ChangeMap(280030000);
                            }
                        }
                        // 하루가 안지난 경우
                        else
                        {
                            if (started) target.Message("The battle against the boss has already begun, so you can't go in.");
                            else
                            {
                                if (val3 == "")
                                {
                                    if (val == "s2") qr.Set(7002, "end");
                                    target.PlayPortalSE();
                                    target.ChangeMap(280030000);
                                }
                                else if (val3 == "1")
                                {
                                    if (val == "s2") qr.Set(7002, "end");
                                    target.PlayPortalSE();
                                    target.ChangeMap(280030000);
                                }
                                else
                                {
                                    //                        if ( serverType == 1 ) { //개발서버
                                    if (Server.Tespia)
                                    {
                                        target.PlayPortalSE();
                                        target.ChangeMap(280030000);
                                    }
                                    else target.Message("You can only enter the altar of Zakum twice a day.");
                                }
                            }
                        }
                    }
                    else target.Message("You may only enter this place after clearing level 3. You'll also need to have the Eye of Fire in possession.");
                }
                else
                {
                    target.Message("Zakum can only be summoned at channel " + (sChannel + 1) + ".");
                }
            }
        }
        // Sending the player out in the middle of the 2nd level
        [Script("Zakum06")]
        class Zakum06 : INpcScript
        {
            dynamic eNum, qr, nRet, v0;
            public void Run(INpcHost self, GameCharacter target)
            {
                zakum.self = self;
                zakum.target = target;
                var field = target.Field;
                if (field.ID == 280030000)
                {
                    qr = target.QuestRecord;
                    eNum = qr.Get(7004);
                    //        if ( serverType == 1 ) v0 = self.AskYesNo( "Would you like to leave this place?" ); //개발서버 테스트용
                    if (Server.Tespia) v0 = self.AskYesNo("Would you like to leave this place?");
                    else
                    {
                        if (eNum == "1") v0 = self.AskYesNo("Are you sure you want to leave this place? You are entitled to enter the Zakum Altar up to twice a day, and by leaving right now, you may only re-enter this shrine once more for the rest of the day.");
                        if (eNum == "2") v0 = self.AskYesNo("Are you sure you want to leave this place? You are entitled to enter the Zakum Altar up to twice a day, and since you have been here twice already, you will be denied entrance to this shrine for the rest of the day by leaving right now.");
                    }
                    if (v0 == 1) target.ChangeMap(211042300, "");
                }
                else
                {
                    nRet = self.AskYesNo("Are you sure you want to quit and leave this place? Next time you come back in, you'll have to start all over again.");
                    if (nRet == 1) target.ChangeMap(211042300, "");
                }

            }
        }
    }
    #endregion
    #region Reactors
    [Script("boxPaper0")]
    class boxPaper0 : IReactorScript
    {
        public void Run(IReactorHost host, FieldReactor target)
        {
            target.Drop();
        }
    }
    [Script("money100")]
    class money100 : IReactorScript
    {
        public void Run(IReactorHost host, FieldReactor target)
        {
            target.Drop();
        }
    }
    [Script("money10000")]
    class money10000 : IReactorScript
    {
        public void Run(IReactorHost host, FieldReactor target)
        {
            target.Drop();
        }
    }
    [Script("boxKey0")]
    class boxKey0 : IReactorScript
    {
        public void Run(IReactorHost host, FieldReactor target)
        {
            target.Drop();
        }
    }
    [Script("boxItem0")]
    class boxItem0 : IReactorScript
    {
        public void Run(IReactorHost host, FieldReactor target)
        {
            target.Drop();
        }
    }
    [Script("boxItem1")]
    class boxItem1 : IReactorScript
    {
        public void Run(IReactorHost host, FieldReactor target)
        {
            target.Drop();
        }
    }
    [Script("boxItem2")]
    class boxItem2 : IReactorScript
    {
        public void Run(IReactorHost host, FieldReactor target)
        {
            target.Drop();
        }
    }
    [Script("boxItem3")]
    class boxItem3 : IReactorScript
    {
        public void Run(IReactorHost host, FieldReactor target)
        {
            target.Drop();
        }
    }
    [Script("boxBItem0")]
    class boxBItem0 : IReactorScript
    {
        public void Run(IReactorHost host, FieldReactor target)
        {
            target.Drop();
        }
    }
    [Script("boxMob0")]
    class boxMob0 : IReactorScript
    {
        public void Run(IReactorHost host, FieldReactor target)
        {
            target.SpawnMob(new Pos(0, -4), (9300004, 3, (SummonType)1, null));
        }
    }
    [Script("go280010000")]
    class go280010000 : IReactorScript
    {
        public void Run(IReactorHost host, FieldReactor target)
        {
            if (target.Owner == null) return;
            target.Owner.ChangeMap(280010000);
            target.Owner.SendPacket(MessagePacket.RedText("You will go back to the first place by an unknown force."));
        }
    }
    [Script("boss")]
    class boss : IReactorScript
    {
        public void Run(IReactorHost host, FieldReactor target)
        {
            if (target.Owner == null) return;
            var instance = FieldSet.Instances["ZakumBoss"];
            instance.Start();
            TriggerGate();
            instance.OnEnd += (obj, _) =>
            {
                TriggerGate();
            };
            target.Field.SendPacket(FieldEffectPacket.EffectMusic("Bgm06/FinalFight"));
            var mobOwner = target.SpawnMob(new Pos(0, -11), (8800000, 1, SummonType.Fake, null))[0];
            for (var i = 8800003; i <= 8800010; i++)
            {
                target.SpawnMob(new Pos(0, -11), (i, 1, SummonType.Regen, mobOwner));
            }
            target.Field.Message("Zakum is summoned by the force of Eye of Fire.", BroadcastMessageType.RedText);
        }
        void TriggerGate()
        {
            DataProvider.Maps[211042300].ReactorPool.TriggerReactor(0);
        }
    }
    #endregion
}
