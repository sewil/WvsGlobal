using System.Collections.Generic;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Extensions;
using WvsBeta.Game;
using WvsBeta.Game.Events.GMEvents;
using WvsBeta.Game.Scripting;

namespace WvsBeta.Scripts.Scripts
{
    [Script("standard")]
    public class Standard : IStandardScript
    {
        public Standard()
        {
        }

        public void PetMenu(INpcHost self, GameCharacter target, string ask)
        {
            bool isWisp = self.mNpcID == NpcIds.Wisp;
            var menu = new List<string>();
            if (isWisp) menu.Add("My pet is back to being a doll. Please help me get it moving again!");
            menu.AddRange(
                "Tell me more about Pets.",
                "How do I take care of Pets?",
                "Do pets die too?",
                "What are the commands for the Brown and Black Kittens?",
                "What are the commands for Brown Puppy?",
                "What are the commands for the Pink and White Bunnies?",
                "What are the commands for Mini Kargo?",
                "What are the commands for Rudolph?",
                "What are the commands for Black Pig?",
                "What are the commands for Panda?",
                "What are the commands for Husky?",
                "What are the commands for Dino Boy, Dino Girl?",
                "What are the commands for Monkey?"
            );

            int v1 = self.AskMenu(ask, menu.ToArray());
            if (isWisp) v1--;

            if (v1 == -1)
            {
                var qr = target.QuestRecord;
                var val = qr.GetState(3240);
                var inven = target.Inventory;

                bool havePetDoll = self.HavePetDoll();

                if (havePetDoll)
                {
                    if (val == 0 || val == 2)
                    {
                        var nRet1 = self.AskYesNo("Looks like you already met #p1012005#. #p1012005# is someone who studied life magic with my master, #p1032102#. I heard that he used an incomplete life spell on a doll to create a living animal... is the doll you have the same doll that #p1012005# created, called #bPet#k?");
                        if (nRet1 == 0) self.Say("But it sure looks like the work of #p1012005#. Oh well! It doesn't matter. I've seen #p1012005# in recent years and I'm pretty sure he can't create life magic in puppets. Well then...");
                        else
                        {
                            self.Say($"I understand. This puppet used to be a living animal... but the same item that #p1012005# used to bring the pet to life, #b#t{ItemEtcIds.WaterOfLife}##k, has been used up and the pet is back to being just a puppet... it doesn't move anymore because now it's a doll again... hmm... isn't life something you can create with magic...?");
                            self.Say("Do you want to make the doll go back to what it was? A living being? You want your pet to accompany you again, walking around, keeping you company, right? Of course! This is totally possible! Since I am the Fairy's disciple who studied magic with #p1012005#... Maybe I can make it become alive again...");
                            var nRet2 = self.AskYesNo($"If you can get me #b#t{ItemEtcIds.WaterOfLife}##k and #b#t{ItemEtcIds.LifeScroll}##k, maybe I can bring your doll back to life. What do you think? Do you want to go get these items? Bring the items to me and I'll try bringing your doll back to life...");
                            if (nRet2 == 0) self.Say("Do you want to leave the doll the way it is? It's a doll now, but... it will be hard to erase your memories too. If you change your mind, just come talk to me...");
                            else
                            {
                                qr.Set(3240, "");
                                self.Say($"Very good. I'll say it again, but what I need is #b#t{ItemEtcIds.WaterOfLife}##k and #b#t{ItemEtcIds.LifeScroll}##k. Bring me these two items and I can bring your doll back to life. #b#t{ItemEtcIds.LifeScroll}##k is hard to find... somewhere in Ludibrium you will find #b#p1012005##k's house. The house is empty at the moment, but look around and you might find it.");
                            }
                        }
                    }
                    else if (val == 1)
                    {
                        if (inven.ItemCount(4070000) > 0 && inven.ItemCount(4031034) > 0)
                        {
                            var nRet3 = self.AskYesNo("You brought me #b#t4070000##k and #b#t4031034##k... with these items, I can bring the doll back to life, using my master's magic power. What do you think? Do you want to use the items and revive your pet?");
                            if (nRet3 == 0) self.Say("I understand, you're not sure about this, are you? Don't you think leaving the pet like a doll is wrong? Please come back here if you change your mind...");
                            else
                            {
                                var petCode = self.AskPetDoll("So which pet do you want to restore? Please choose the pet you want to revive...");
                                bool okPet = inven.SetPetLife(petCode, (4070000, -1), (4031034, -1));
                                if (!okPet) self.Say("Something is wrong... are you sure you have #b#t4070000##k and #b#t4031034##k? Without these two items I cannot make your doll go back to being a pet.");
                                else
                                {
                                    qr.SetComplete(3240);
                                    self.Say("Your doll is now back to being a pet. However, my magic isn't perfect, so I can't promise you that it'll live forever... take care of your pet before the #t4070000# runs out. Well, I guess that's it... goodbye!");
                                }
                            }
                        }
                        else self.Say("I think you haven't gotten #b#t4070000##k and #b#t4031034##k yet. Somewhere in Ludibrium you will find #b#p1012005##k's house. The house is empty at the moment, but look around and you might find it. Cheers!");
                    }
                }
                else
                {
                    if (val == 2) self.Say("Hello! How is your revived pet doing? Glad to hear you are happy with your pet. Well, now I have to go back to the studies my master gave me, so...");
                    else self.Say("I am #p2040030#, continuing with the studies my master #p1032102# gave me. It seems that there are many pets even here in Ludibrium. I need to get back to my studies, so if you'll excuse me...");
                }
            }
            else if (v1 == 0)
            {
                if (isWisp)
                {
                    self.Say($"Hmm... you must have a lot of questions about pets. A long time ago, a person by the name of #b#p1012005##k made a doll, sprinkled #t{ItemEtcIds.WaterOfLife}# on it and cast a charm to create a magical animal. I know it sounds unbelievable, but these are dolls that have really come to life. They understand and obey people very well.");
                    self.Say($"But #t{ItemEtcIds.WaterOfLife}# only appears little by little at the base of the World Tree, so these babies can't live forever... I know, it's very sad... but even if it becomes a doll again, they can always be revived and come back to life. Be nice to it while it's alive.");
                }
                else
                {
                    self.Say($"So you wish to know more about pets. A long time ago I made a doll, sprinkled #t{ItemEtcIds.WaterOfLife}# on it and cast a charm to create a magical animal. I know it sounds unbelievable, but these are dolls that have really come to life. They understand and obey people very well.");
                    self.Say($"But #t{ItemEtcIds.WaterOfLife}# only appears a little bit, right at the base of the World Tree, so I can't give it much time to live... I know, it's very sad... but even if it becomes a doll again, I can always bring it back to life. Be nice while you're with it.");
                }
                self.Say("Ah yes! They will react when you give them special commands. You can scold them, give them affection... it all depends on how you are going to take care of them. They are afraid to be separated from their owners, so be nice to them, show them you love them. They may suddenly become sad...");
            }
            else if (v1 == 1)
            {
                self.Say("Depending on the command you give, pets can love, hate and demonstrate other types of reactions. If you give a command and the pet obeys correctly, closeness increases. By double clicking on the pet you can see closeness, level, energy, etc.");
                self.Say("Talk to the pet, pay attention to it, and your level of closeness will grow. Eventually your overall level will go up as well. If the level of closeness increases, the general level of the pet will increase soon after. As the general level increases, one day the pet can even speak a little bit, like a person. Then try to increase it for real! Of course it won't be that easy...");
                self.Say("They may just be living dolls, but they have a normal life, just like us. So they feel hungry too. The #bFullness#k shows the pet's hunger level. 100 is the maximum, and the lower it gets, it means the pet is getting hungry. After a while it won't even obey your commands and will get aggressive, so be careful with that.");
                if (isWisp)
                {
                    self.Say("That's right! Pets can't eat normal human food. A teddy bear named #b#p2041014##k, who lives in Ludibrium, sells #b#t2120000##k, so if you need food for your pet, go talk to #b#p2041014##k. It's a good idea to buy extra food and feed your pet before it gets too hungry.");
                }
                else
                {
                    self.Say("Ah yes! Pets cannot eat human food. My disciple #b#p1012004##k sells #b#t2120000##k at the #m100000000# market. If you need food for your pet, look in #m100000000#. It's a good idea to keep a supply of food and feed your pet before it gets too hungry.");
                }
                self.Say("Oh... and if you don't feed the pet for an extended period of time, it will go home on its own. You can take it out of its house and feed it, but that is not good for its health. Try to feed it regularly so it doesn't get to that level, okay? I think that will do it.");
            }
            else if (v1 == 2)
            {
                self.Say("Die... well, they aren't technically ALIVE, they came to life through someone else's influence, so I don't know if death is the right term. They are dolls with my magic power, and the power of #t4070000# makes objects come alive. Of course, while alive, they are just like a normal animal...");
                self.Say("After some time... that's for sure, they stop moving. They simply revert back to dolls after the spell's effect wears off and the #t4070000# runs out. But that doesn't mean it stays still forever, because when you sprinkle #t4070000# on it, it will come back to life.");
                self.Say("Even knowing that, it's sad to see them completely stopped. Be nice to them while they're alive and moving. And don't forget to feed them, too. Isn't it nice to know that there's a little thing alive who follows you and only obeys you?");
            }
            else if (v1 == 3)
            {
                self.Say("These are the commands for the #rBrown Kitty and Black Kitty#k. The level displayed next to the command shows what level the pet needs to be in order for it to respond. \r\n#b sit#k (level 1 ~ 30) \r\n#b bad, no, badgirl, badboy#k (level 1 ~ 30) \r\n#b stupid, ihateyou, dummy#k (level 1 ~ 30) \r\n#b iloveyou#k (level 1 ~ 30) \r\n#b poop#k (level 1 ~ 30) \r\n#b talk, chat, say#k (level 10 ~ 30) \r\n#b cutie#k (level 10 ~ 30) \r\n#b up, stand, rise#k (level 20 ~ 30)");
            }
            else if (v1 == 4)
            {
                self.Say("These are the commands for the #rBrown Puppy#k. The level displayed next to the command shows what level the pet needs to be in order for it to respond. \r\n#b sit#k (level 1 ~ 30) \r\n#b bad, no, badgirl, badboy#k (level 1 ~ 30) \r\n#b stupid, ihateyou, dummy#k (level 1 ~ 30) \r\n#b iloveyou#k (level 1 ~ 30) \r\n#b pee#k (level 1 ~ 30) \r\n#b talk, chat, say#k (level 10 ~ 30) \r\n#bdown#k (level 10 ~ 30) \r\n#b up, stand, rise#k (level 20 ~ 30)");
            }
            else if (v1 == 5)
            {
                self.Say("These are the commands for the #rPink Bunny and White Bunny#k. The level displayed next to the command shows what level the pet needs to be in order for it to respond. \r\n#bsit#k (level 1 ~ 30) \r\n#b bad, no, badgirl, badboy#k (level 1 ~ 30) \r\n#bup, stand, rise#k (level 1 ~ 30) \r\n#b iloveyou#k (level 1 ~ 30) \r\n#bpoop#k (level 1 ~ 30) \r\n#b talk, chat, say#k (level 10 ~ 30) \r\n#bhug#k (level 10 ~ 30) \r\n#bsleep, sleepy, gotobed#k (level 20 ~ 30)");
            }
            else if (v1 == 6)
            {
                self.Say("These are the commands for the #rMini Kargo#k. The level displayed next to the command shows what level the pet needs to be in order for it to respond. \r\n#bsit#k (level 1 ~ 30) \r\n#b bad, no, badgirl, badboy#k (level 1 ~ 30) \r\n#bup, stand, rise#k (level 1 ~ 30) \r\n#b iloveyou#k (level 1 ~ 30) \r\n#bpee#k (level 1 ~ 30) \r\n#b talk, chat, say#k (level 10 ~ 30) \r\n#bthelook, charisma#k (level 10 ~ 30) \r\n#bdown#k (level 20 ~ 30) \r\n#bgoodboy, goodgirl#k (level 20 ~ 30)");
            }
            else if (v1 == 7)
            {
                self.Say("These are the commands for #rRudolph#k. The level displayed next to the command shows what level the pet needs to be in order for it to respond. \r\n#bsit#k (level 1 ~ 30) \r\n#bbad, no, badgirl, badboy#k (level 1 ~ 30) \r\n#bup, stand#k (level 1 ~ 30) \r\n#b stupid, ihateyou, dummy#k (level 1 ~ 30) \r\n#bmerryxmas, merrychristmas#k (level 1 ~ 30) \r\n#biloveyou#k (level 1 ~ 30) \r\n#bpoop#k (level 1 ~ 30) \r\n#btalk, chat, say#k (level 11 ~ 30) \r\n#blonely, alone#k (level 11 ~ 30) \r\n#bcutie#k (level 11 ~ 30) \r\n#bmush, go#k (level 21 ~ 30)");
            }
            else if (v1 == 8)
            {
                self.Say("These are the commands for the #rBlack Pig#k. The level displayed next to the command shows what level the pet needs to be in order for it to respond. \r\n#bsit#k (level 1 ~ 30) \r\n#b bad, no, badgirl, badboy#k (level 1 ~ 30) \r\n#bpoop#k (level 1 ~ 30) \r\n#biloveyou#k (level 1 ~ 30) \r\n#bhand#k (level 1 ~ 30) \r\n#bstupid, ihateyou, dummy#k (level 1 ~ 30) \r\n#btalk, chat, say#k (level 10 ~ 30) \r\n#bsmile#k (level 10 ~ 30) \r\n#bthelook, charisma#k (level 20 ~ 30)");
            }
            else if (v1 == 9)
            {
                self.Say("These are the commands for the #rPanda#k. The level displayed next to the command shows what level the pet needs to be in order for it to respond. \r\n#bsit#k (level 1 ~ 30) \r\n#bchill, relax#k (level 1 ~ 30) \r\n#bbad, no, badgirl, badboy#k (level 1 ~ 30) \r\n#bpoop#k (level 1 ~ 30) \r\n#biloveyou#k (level 1 ~ 30) \r\n#bup, stand, rise#k (level 1 ~ 30) \r\n#btalk, chat, say#k (level 10 ~ 30) \r\n#bletsplay#k (level 10 ~ 30) \r\n#bmeh, bleh#k (level 10 ~ 30) \r\n#bsleep#k (level 20 ~ 30)");
            }
            else if (v1 == 10)
            {
                self.Say("These are the commands for the #rHusky#k. The level displayed next to the command shows what level the pet needs to be in order for it to respond. \r\n#b sit#k (level 1 ~ 30) \r\n#bbad, no, badgirl, badboy#k (level 1 ~ 30) \r\n#bstupid, ihateyou, dummy#k (level 1 ~ 30) \r\n#bhand#k (level 1 ~ 30) \r\n#bpoop#k (level 1 ~ 30) \r\n#biloveyou#k (level 1 ~ 30) \r\n#bdown#k (level 10 ~ 30) \r\n#btalk, chat, say#k (level 10 ~ 30) \r\n#bup, stand, rise#k (level 20 ~ 30)");
            }
            else if (v1 == 11)
            {
                self.Say("These are the commands for the #rDino Boy and Dino Girl#k. The level displayed next to the command shows what level the pet needs to be in order for it to respond. \r\n#bsit#k (level 1 ~ 30) \r\n#b bad, no, badgirl, badboy#k (level 1 ~ 30) \r\n#biloveyou#k (level 1 ~ 30) \r\n#bpoop#k (level 1 ~ 30) \r\n#bsmile, laugh#k (level 1 ~ 30) \r\n#bstupid, ihateyou, dummy#k (level 1 ~ 30) \r\n#btalk, chat, say#k (level 10 ~ 30) \r\n#bcutie#k (level 10 ~ 30) \r\n#bsleep, nap, sleepy#k (level 20 ~ 30)");
            }
            else if (v1 == 12)
            {
                self.Say("These are the commands for the #rMonkey#k. The level displayed next to the command shows what level the pet needs to be in order for it to respond. \r\n#bsit#k (level 1 ~ 30) \r\n#brest#k (level 1 ~ 30) \r\n#bbad, no, badgirl, badboy#k (level 1 ~ 30) \r\n#bpee#k (level 1 ~ 30) \r\n#biloveyou#k (level 1 ~ 30) \r\n#bup, stand#k (level 1 ~ 30) \r\n#btalk, chat, say#k (level 10 ~ 30) \r\n#bplay#k (level 10 ~ 30) \r\n#bmelong#k (level 10 ~ 30) \r\n#bsleep, gotobed, sleepy#k (level 20 ~ 30)");
            }
        }
    }
}
