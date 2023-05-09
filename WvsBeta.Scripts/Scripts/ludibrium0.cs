using System;
using System.Collections.Generic;
using System.Linq;
using WvsBeta.Common;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Objects;
using WvsBeta.Game;
using WvsBeta.Game.Scripting;
using static WvsBeta.Common.Strings;

namespace WvsBeta.Scripts.Scripts
{
    static class ludibrium0
    {
        readonly static Dictionary<int, Rock> EosRocks = new Dictionary<int, Rock>()
        {
            { NpcIds.FirstEosRock, new Rock(NpcIds.FirstEosRock, MapIds.EosTower100thFloor, 100, NpcIds.SecondEosRock) },
            { NpcIds.SecondEosRock, new Rock(NpcIds.SecondEosRock, MapIds.EosTower71stFloor, 71, NpcIds.FirstEosRock, NpcIds.ThirdEosRock) },
            { NpcIds.ThirdEosRock, new Rock(NpcIds.ThirdEosRock, MapIds.EosTower41stFloor, 41, NpcIds.SecondEosRock, NpcIds.FourthEosRock) },
            { NpcIds.FourthEosRock, new Rock(NpcIds.FourthEosRock, MapIds.EosTower1stFloor, 1, NpcIds.ThirdEosRock) },
        };
        struct Rock
        {
            public readonly int npcID;
            public readonly int mapID;
            public readonly int floorNumber;
            public readonly int[] destinationRocks;
            public Rock(int npcID, int mapID, int floorNumber, params int[] destinationRocks)
            {
                if (destinationRocks.Length == 0) throw new ArgumentException("Rock destinations can not be empty.");
                this.npcID = npcID;
                this.mapID = mapID;
                this.floorNumber = floorNumber;
                this.destinationRocks = destinationRocks;
            }

            public IEnumerable<Rock> GetDestinations()
            {
                return destinationRocks.Select(i => EosRocks[i]);
            }
        }
        static void RockGo(INpcHost self, GameCharacter target, Rock rock)
        {
            var inven = target.Inventory;

            if (inven.ItemCount(ItemEtcIds.EosRockScroll) >= 1)
            {
                string askText = $"You can use #b#t{ItemEtcIds.EosRockScroll}##k to activate #b#p{rock.npcID}##k. ";
                Rock destination;
                Rock[] destinations = rock.GetDestinations().ToArray();
                if (rock.destinationRocks.Length > 1)
                {
                    string[] menuOptions = destinations.Select(d => $"#p{d.npcID}# ({Ordinal(d.floorNumber)} floor)").ToArray();
                    int opt = self.AskMenu(askText + "Which of these rocks would you like to teleport to?", menuOptions);
                    destination = destinations[opt];
                }
                else
                {
                    destination = destinations[0];
                }

                askText += $"Will you teleport to the {Ordinal(destination.floorNumber)} floor of #b#p{destination.npcID}##k?";
                int ret1 = self.AskYesNo(askText);
                if (ret1 != 0)
                {
                    var ret2 = inven.Exchange(0, ItemEtcIds.EosRockScroll, -1);
                    if (ret2 == 0) self.Say($"You cannot activate #b#p{rock.npcID}##k without #b#t{ItemEtcIds.EosRockScroll}##k.");
                    else target.ChangeMap(destination.mapID, "go00");
                }
            }
            else
            {
                string npcs = string.Join(" or ", rock.destinationRocks.Select(npcID => $"#p{npcID}#"));
                self.Say($"There is a rock that teleports you to #b{npcs}#k, but it cannot be activated without the scroll.");
            }
        }

        // 1st eos rock : 2040024, 100th floor
        [Script("ludi014")]
        class ludi014 : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                RockGo(self, target, EosRocks[NpcIds.FirstEosRock]);
            }
        }
        // 2nd eos rock : 2040025, 71st floor
        [Script("ludi015")]
        class ludi015 : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                RockGo(self, target, EosRocks[NpcIds.SecondEosRock]);
            }
        }
        // 3rd eos rock : 2040026, 41st floor
        [Script("ludi016")]
        class ludi016 : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                RockGo(self, target, EosRocks[NpcIds.ThirdEosRock]);
            }
        }
        // the 4th eos rock : 2040027, 1st floor
        [Script("ludi017")]
        class ludi017 : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                RockGo(self, target, EosRocks[NpcIds.FourthEosRock]);
            }
        }
        // chang the assistant : 2040003
        [Script("ludi020")]
        class ludi020 : INpcScript
        {
            private void Leave(INpcHost self, GameCharacter target)
            {
                var nRet = self.AskYesNo("Are you sure you want to quit? Well, I can let you out, but you'll have to start over from the very beginning the next time you pay a visit here. Do you still want to leave this place?");
                if (nRet == 0) self.Say("That's the kind of attitude I've been looking for! If you've started something, you have to learn how to finish it! Now please search the plastic barrels and find #b10 #t4031092##k for me.");
                else target.ChangeMap(922000009, "");
            }
            public void Run(INpcHost self, GameCharacter target)
            {
                var field = self.Field;
                var qr = target.QuestRecord;
                var val = qr.GetState(3239);

                if (val == 1)
                {
                    string instructions = "Inside this room, you'll see a lot of plastic barrels lying around. Hit the barrels to make them fall and see if you can find the missing #b#t4031092##k.";
                    if (field.ID == 220020000)
                    {
                        var inven = target.Inventory;
                        self.Say($"Right then. {instructions}. You must collect #b10 #t4031092##k and then come back to me. There is a time limit to do this, so make sure to be quick!");
                        var hItem = inven.ItemCount(4031092);
                        if (hItem > 0) inven.Exchange(0, 4031092, -hItem);
                        var setParty = FieldSet.Instances["Ludi020"];
                        var res = setParty.Enter(target, 0);
                        if (res != 0) self.Say("Sorry, but it looks like someone else is inside inspecting the barrels. Only one person is allowed here at a time, so you'll have to wait your turn.");
                    }
                    else if (field.ID == 922000000)
                    {
                        var inven = target.Inventory;
                        var nItem = inven.ItemCount(4031092);
                        if (nItem == 0)
                        {
                            var v1 = self.AskMenu($"{instructions} You must collect #b10 #t4031092##k before the time limit expires and then come back to me. Time is ticking as we speak, so please hurry!", "I want to get out of here.");
                            if (v1 == 0) Leave(self, target);
                        }
                        else if (nItem >= 10)
                        {
                            self.Say("Well done! You managed to collect #b10 #t4031092##k. Well, since you've done us this huge favor, I'll reward you with something really nice. Before doing so, please check that you have space available in your use inventory.");
                            if (inven.SlotCount(2) > inven.HoldCount(2))
                            {
                                var rnum = Rand32.NextBetween(1, 4);
                                var nNewItemID = 0;
                                if (rnum == 1) nNewItemID = 2040704;
                                else if (rnum == 2) nNewItemID = 2040705;
                                else if (rnum == 3) nNewItemID = 2040707;
                                else if (rnum == 3) nNewItemID = 2040708;
                                var nNeed = inven.ItemCount(4031092);

                                var ret = inven.Exchange(0, 4031092, -nNeed, nNewItemID, 1);
                                if (ret == 0) self.Say("Are you sure you have #b10 #t4031092#s#k? If you're sure, please check that you have space available in your use inventory.");
                                else
                                {
                                    target.IncEXP(2700, 0);
                                    qr.SetComplete(3239);
                                    self.Say("What do you think? Do you like the #b#t" + nNewItemID + "##k I gave you? I don't even know how to thank you for helping me. Thanks to you, the Toy Factory is running smoothly again. I'm going to send you out now. Take care!");
                                    target.ChangeMap(220020000, "q000");
                                }
                            }
                            else self.Say("Hmm... your use inventory seems to be full at the moment. Therefore, you will not be able to receive my reward. Please free up some space in your inventory and come talk to me again.");
                        }
                        else
                        {
                            var v1 = self.AskMenu("Looks like you didn't collect the 10 missing #b#t4031092#s#k. Break the plastic barrels you see in this room and see if any of them contain the missing #b#t4031092##k. If you manage to get the 10 #b#t4031092#s#k before the time limit expires, then come and bring them to me. If you want to leave this place at any time, come talk to me.", "I want to leave this place.");
                            if (v1 == 0) Leave(self, target);
                        }
                    }
                }
                else if (val == 2) self.Say("Thanks to you, the Toy Factory is running smoothly again. I'm so glad you came to help us. We've taken good care of our extra parts, so don't worry. Well that's it! I need to get back to work!");
                else self.Say("Lately, mechanical parts have been disappearing from the Toy Factory, and that worries me a lot. I want to ask for help, but you don't seem strong enough to help us. Who should I ask to help us?");
            }
        }
        // kicking out of the toy factory quest
        [Script("ludi021")]
        class ludi021 : IPortalScript
        {
            public void Run(IPortalHost self, GameCharacter target)
            {
                var inven = target.Inventory;
                var qItem = inven.ItemCount(4031092);
                if (qItem > 0) inven.Exchange(0, 4031092, -qItem);
                target.ChangeMap(220020000, "q000");
            }
        }
        // olson the toy soldier : 2040002
        [Script("ludi023")]
        class ludi023 : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                var qr = target.QuestRecord;
                var val = qr.GetState(3230);
                var inven = target.Inventory;

                if (val == 1)
                {
                    var nItem = inven.ItemCount(4031145);
                    var nRet1 = self.AskYesNo("Hmm... I've heard a lot about you from #b#p2040001##k. You got him some #b#t4031093##k so he could escape boredom at work. Well then... There's a very, very dangerous monster inside. I want to ask you to help us by finding the monster. Would you like to help me?");
                    if (nRet1 == 0) self.Say("I understand. It's totally understandable considering the fact that you'll be facing a very dangerous monster. If you ever change your mind, come and talk to me again. I'm sure your assistance would be very helpful.");
                    else
                    {
                        self.Say("Thank you very much. In fact, #b#p2040001##k asked you to get #b#t4031093#s#k as a way to test your skills and see if you can handle it. So, don't think of this as a request coming out of nowhere. I think someone like you can tackle this challenge.");
                        var nRet2 = self.AskYesNo("A while ago, a monster from another dimension came here due to an interdimensional rift, and stole the clock's pendulum. It hid in that room and disguised itself as a dollhouse. To me they all look the same, so there's no way to figure out which is which. Would you help us find it?");
                        if (nRet2 == 0) self.Say("I understand. Come talk to me when you're ready to accept this task. I advise you not to take too long, as the monster can disguise itself as something else entirely. We have to act like we don't know anything.");
                        else
                        {
                            self.Say("Great! I'll take you to a room, where you'll find dollhouses scattered around. One of them will look a little different from the others. Your job is to locate and break down its door. If you can find it, you'll find the #b#t4031145##k. If you break one of the real dollhouses, you'll be sent back here, so pay close attention.");
                            self.Say("You'll also find monsters there, and they've gotten too powerful due to the monster from the other dimension, so powerful that you won't be able to eliminate them. Please find the #b#t4031145##k before the time runs out and notify #b#p2040028##k, who should be inside. Let's get started soon!");
                            if (nItem > 0) inven.Exchange(0, 4031145, -nItem);
                            var setParty = FieldSet.Instances["Ludi023"];
                            var res = setParty.Enter(target, 0);
                            if (res != 0) self.Say("Someone else must be inside looking for the dollhouse. Unfortunately, I can only let one person in at a time, so please wait your turn.");
                        }
                    }
                }
                else if (val == 2) self.Say("Thanks to " + target.SCharacterName + ", we recovered the #b#t4031145##k and destroyed the monster from the other dimension. Good thing we haven't found one like that since. I don't even know how to thank you for helping us. Hope you enjoy your stay here in Ludibrium!");
                else self.Say("We are the toy soldiers who guard this room to prevent anyone else from entering. I can't tell you why. Now, if you'll excuse me, I have work to do.");
            }
        }
        // mark the toy soldier : 2040028
        [Script("ludi024")]
        class ludi024 : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                var qr = target.QuestRecord;
                var val = qr.GetState(3230);
                var inven = target.Inventory;

                if (val == 1)
                {
                    if (inven.ItemCount(4031145) >= 1)
                    {
                        self.Say("Oh wow, you did locate the different-looking dollhouse and found the #b#t4031145##k! That was just incredible!! With this, the Ludibrium Clocktower will be running again! Thank you for your work and here's a little reward for your effort. Before that, though, please check your use inventory and see if it's full or not.");
                        var ret = inven.Exchange(0, 4031145, -1, 2000010, 100);
                        if (ret == 0) self.Say("Are you sure you have 1 #b#t4031145##k? If you're sure, please check your use inventory, it might be full!!");
                        else
                        {
                            target.IncEXP(2400, 0);
                            qr.SetComplete(3230);
                            self.Say("What do you think? Do you like the #b100 #t2000010#s#k that I gave you? Thank you so much for helping us out. The clocktower will be running again thanks to your heroic effort, and the monsters from the other dimension seem to have disappeared, too. I'll let you out now. I'll see you around!");
                            target.ChangeMap(221024400, "q000");
                        }
                    }
                    else
                    {
                        var v1 = self.AskMenu("Hello! I am #b#p2040028##k, and I'm in charge of protecting this room. Here, you'll find a bunch of dollhouses and you might find the one that looks a little different from the others. Your job is to find it, break down its door and find #b#t4031145##k, which is an integral part of the Ludibrium Clocktower. You will have a time limit to do this, and if you break the wrong dollhouse, you will have to come back here, so please be careful.", "I want to get out of here.");
                        if (v1 == 0)
                        {
                            var nRet = self.AskYesNo("Are you sure you want to quit now? Alright then... but remember that the next time you visit this place, the dollhouses will switch places and you'll have to look at each one very carefully again. What do you think? Do you still want to leave this place?");
                            if (nRet == 0) self.Say("I knew you'd stay! It's important to finish what you started! Now, please go find the dollhouse that is different from the others, break it and bring me the #b#t4031145##k!");
                            else target.ChangeMap(221024400, "q000");
                        }
                    }
                }
                else
                {
                    self.Say("What's this... we have prohibited people from entering this room because a monster from another dimension is hiding here. I don't know how you got here, but I'll have to ask you to leave immediately, because inside this room there is great danger.");
                    target.ChangeMap(221024400, "");
                }
            }
        }
        // wisp : 2040030
        [Script("ludi026")]
        class ludi026 : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
               self.Standard.PetMenu(self, target, "Hello there! I am #b#p1032102##k, the chief disciple of Victoria Island. #p1032102# called me to check that the pets are being treated well here in Ludibrium. What can I do for you?");
            }
        }
        // ¹®¼­¹¶Ä¡ : 2040031
        [Script("ludi027")]
        class ludi027 : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                var qr = target.QuestRecord;
                var val = qr.GetState(3240);
                var inven = target.Inventory;

                if (val == 1)
                {
                    if (inven.ItemCount(4031034) >= 1)
                    {
                        self.Say("You already have #b#t4031034##k.");
                        return;
                    }

                    var nRet = self.AskYesNo("Among so many scrolls left by #b#p1012005##k, I see one that emits a bright light. Should I take it?");
                    if (nRet != 0)
                    {
                        var ret = inven.Exchange(0, 4031034, 1);
                        if (ret == 0) self.Say("I don't have any free space in my inventory. I'd better clear up some space so I can carry this scroll with me.");
                    }
                }
            }
        }
        // weaver the trainer : 2040032
        [Script("ludi028")]
        class ludi028 : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                var inven = target.Inventory;
                if (inven.ItemCount(4031128) > 0) self.Say("Take this letter, jump over the obstacles with your pet and take the letter to my brother #p2040033#. Bring him the letter to him and something good will happen to your pet.");
                else
                {
                    var nRet = self.AskYesNo("This is the walkway where you can take your pet for a walk. You can walk on it or you can train your pet to jump over obstacles. If you're not very close with your pet yet, this could be a problem and they won't follow your orders as well... so... what do you think? Want to train your pet?");
                    if (nRet == 0) self.Say("Hmm... too busy right now? If you want to do it later, just come back and talk to me.");
                    else
                    {
                        var ret = inven.Exchange(0, 4031128, 1);
                        if (ret == 0) self.Say("Your etc. inventory is full! I cannot give you the letter unless there is free space in your inventory. Please free up space in your inventory and come talk to me again.");
                        else self.Say("Okay, here's the letter. If I sent you there without the letter, he wouldn't know who you are, so go through the obstacles with your pet, go to the top and then talk to #p2040033# and deliver the letter. It won't be difficult if you pay attention to your pet while you're overcoming the obstacles. Good luck!");
                    }
                }
            }
        }
        // nerr the trainer : 2040033
        [Script("ludi029")]
        class ludi029 : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                var inven = target.Inventory;
                if (inven.ItemCount(4031128) > 0)
                {
                    self.Say("Hey! This letter is from my brother! Probably scolding me for not feeling like I'm working... What? Ahhh... did you follow my brother's advice and train your pet to get up here? Cool!! Since you've worked so hard to get here, I'm going to increase your pet's level of closeness.");
                    PetItem pet = target.GetSpawnedPet();
                    if (pet == null) self.Say("Humm... did you manage to get here without your pet!? These obstacles are for pets. What are you doing here without one? Get out of here!");
                    else
                    {
                        var ret = inven.Exchange(0, 4031128, -1);
                        if (ret == 0) self.Say("Hey! Are you sure you have my brother's letter? Please check your etc. inventory to see if the letter is there or not!");
                        else
                        {
                            target.IncPetTame(4);
                            self.Say("What do you think? Don't you think you've gotten closer to your pet? If you have time, retrain your pet on this obstacle course... with my brother's permission of course!");
                        }
                    }
                }
                else self.Say("My brother told me to take care of the obstacle course, but... since I'm so far away from him, I can't help but play around, hehe... since he can't control me, I take the opportunity to relax a little.");
            }
        }
        [Script("library")]
        class library : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                var qr = target.QuestRecord;
                var count = 0;
                var bookname = "";

                var book1 = qr.GetState(3615); //ÈïºÎ¿Í ³îºÎ µ¿È­Ã¥ È¸¼ö
                if (book1 == 2)
                {
                    count++;
                    bookname = bookname + "#b#v4031235##t4031235##k\r\n";
                }

                var book2 = qr.GetState(3616); //ÄáÁã ÆÏÁã
                if (book2 == 2)
                {
                    count++;
                    bookname = bookname + "#b#v4031236##t4031236##k\r\n";
                }

                var book3 = qr.GetState(3617); //ÀÇÁÁÀº ÇüÁ¦
                if (book3 == 2)
                {
                    count++;
                    bookname = bookname + "#b#v4031237##t4031237##k\r\n";
                }

                var book4 = qr.GetState(3618); //µµ±úºñ ÀÌ¾ß±â
                if (book4 == 2)
                {
                    count++;
                    bookname = bookname + "#b#v4031238##t4031238##k\r\n";
                }

                var book5 = qr.GetState(3630); //ÇØ¿Í ´ÞÀÌ µÈ ¿À´©ÀÌ
                if (book5 == 2)
                {
                    count++;
                    bookname = bookname + "#b#v4031270##t4031270##k\r\n";
                }

                var book6 = qr.GetState(3633); //È¿³à ½ÉÃ»
                if (book6 == 2)
                {
                    count++;
                    bookname = bookname + "#b#v4031280##t4031280##k\r\n";
                }

                var book7 = qr.GetState(3639); //±Ýµµ³¢ Àºµµ³¢
                if (book7 == 2)
                {
                    count++;
                    bookname = bookname + "#b#v4031298##t4031298##k\r\n";
                }

                if (count != 0)
                {
                    self.Say("Vamos ver... #b" + target.SCharacterName + "#k Eu devolvi um total de #b" + count + "#k livros.\r\nA lista de livros devolvidos é:\r\n\r\n" + bookname);
                    self.Say("A biblioteca está se ajeitando agora, principalmente graças a você, #b" + target.SCharacterName + "#k. Se a história se misturar mais uma vez, então, eu poderei contar com você para repará-la novamente.");
                }
                else
                {
                    self.Say("#b" + target.SCharacterName + "#k ainda não devolveu nenhum livro de história...");
                }
            }
        }
    }
    #region Reactors
    [Script("ludiquest0")]
    public class LudiQuest0 : IReactorScript
    {
        public void Run(IReactorHost host, FieldReactor target)
        {
            target.Drop();
        }
    }
    [Script("go221024400")]
    public class go221024400 : IReactorScript
    {
        public void Run(IReactorHost host, FieldReactor target)
        {
            target.Owner.ChangeMap(221024400, "q000");
            target.Owner.Message("You will be expelled from this map by an unknown force.");
        }
    }
    [Script("ludiquest1")]
    public class LudiQuest1 : IReactorScript
    {
        public void Run(IReactorHost host, FieldReactor target)
        {
            target.Drop();
        }
    }
    #endregion
}
