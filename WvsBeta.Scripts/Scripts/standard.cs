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
        public IDictionary<string, string> ScriptNameMap { get; set; }
        public Standard()
        {
            ScriptNameMap = new Dictionary<string, string>
            {
                { "Event00", "event" },
                { "Event02", "event" },
                { "Event03", "event" },
                { "Event03_1", "event" },
                { "Event04", "event" },
                { "Event05", "event" },
                { "Event06", "event" },
                { "Event09", "event" },
                { "eventItem0", "event" },
                { "eventItem1", "event" },
                { "eventItem2", "event" },
                { "eventMap0", "event" },
                { "eventMap1", "event" },
                { "eventMap2", "event" },
                { "eventMob0", "event" },
                { "friend00", "friend" },
                { "friend01", "friend" },
                { "cny", "cny" },
                { "taxi1", "taxi" },
                { "taxi2", "taxi" },
                { "taxi3", "taxi" },
                { "taxi4", "taxi" },
                { "mTaxi", "taxi" },
                { "aqua_taxi", "taxi" },
                { "market00", "market" },
                { "market01", "market" },
                { "market02", "market" },
                { "market03", "market" },
                { "market04", "market" },
                { "market05", "market" },
                { "market06", "market" },
                { "market07", "market" },
                { "market08", "market" },
                { "market09", "market" },
                { "levelUP", "etc" },
                { "levelUP2", "etc" },
                { "mBoxItem0", "mapleIsland" },
                { "begin2", "mapleIsland" },
                { "begin4", "mapleIsland" },
                { "begin5", "mapleIsland" },
                { "begin7", "mapleIsland" },
                { "bari", "mapleIsland" },
                { "rein", "mapleIsland" },
                { "pio", "mapleIsland" },
                { "rithTeleport", "victoria0" },
                { "jane", "victoria0" },
                { "q2073", "victoria0" },
                { "bush1", "victoria1" },
                { "bush2", "victoria1" },
                { "herb_out", "victoria1" },
                { "herb_in", "victoria1" },
                { "owen", "victoria2" },
                { "mike", "victoria2" },
                { "flower_in", "victoria2" },
                { "viola_pink", "victoria2" },
                { "viola_blue", "victoria2" },
                { "viola_white", "victoria2" },
                { "flower_out", "victoria2" },
                { "petmaster", "victoria2" },
                { "pet_life", "victoria2" },
                { "pet_lifeitem", "victoria2" },
                { "pet_letter", "victoria2" },
                { "subway_ticket", "victoria3" },
                { "_subway_in", "victoria3" },
                { "subway_in", "victoria3" },
                { "subway_get1", "victoria3" },
                { "subway_get2", "victoria3" },
                { "subway_get3", "victoria3" },
                { "subway_out", "victoria3" },
                { "hotel1", "victoria3" },
                { "getAboard", "ossyria0" },
                { "goDungeon", "ossyria0" },
                { "carlie", "ossyria0" },
                { "oldBook1", "ossyria1" },
                { "oldBook2", "ossyria1" },
                { "oldBook5", "ossyria1" },
                { "ossyria3_1", "ossyria2" },
                { "ossyria3_2", "ossyria2" },
                { "guild_proc", "ossyria2" },
                { "guild_mark", "ossyria2" },
                { "ludi014", "ludibrium0" },
                { "ludi015", "ludibrium0" },
                { "ludi016", "ludibrium0" },
                { "ludi017", "ludibrium0" },
                { "ludi018", "ludibrium0" },
                { "ludi019", "ludibrium0" },
                { "ludi020", "ludibrium0" },
                { "ludi021", "ludibrium0" },
                { "ludi022", "ludibrium0" },
                { "ludi023", "ludibrium0" },
                { "ludi024", "ludibrium0" },
                { "ludi025", "ludibrium0" },
                { "ludi026", "ludibrium0" },
                { "ludi027", "ludibrium0" },
                { "ludi028", "ludibrium0" },
                { "ludi029", "ludibrium0" },
                { "ludiquest0", "ludibrium0" },
                { "go221024400", "ludibrium0" },
                { "ludiquest1", "ludibrium0" },
                { "party1_enter", "party" },
                { "party1_play", "party" },
                { "party1_out", "party" },
                { "party2_enter", "party2" },
                { "party2_play", "party2" },
                { "party2_out", "party2" },
                { "ludiquest3", "party2" },
                { "go922010201", "party2" },
                { "ludiMob1", "party2" },
                { "ludiMob2", "party2" },
                { "ludiBoss0", "party2" },
                { "ludiquest4", "party2" },
                { "refine_perion", "make0" },
                { "refine_perion2", "make0" },
                { "refine_kerning", "make0" },
                { "refine_henesys", "make1" },
                { "refine_sleepy", "make1" },
                { "refine_ellinia", "make2" },
                { "refine_kerning2", "make2" },
                { "refine_elnath", "make3" },
                { "make_elnath", "make3" },
                { "make_orbis", "make3" },
                { "make_ludi1", "make4" },
                { "make_ludi2", "make4" },
                { "make_ludi3", "make5" },
                { "make_ludi4", "make6" },
                { "make_ston", "make7" },
                { "face_henesys1", "face" },
                { "face_henesys2", "face" },
                { "face_orbis1", "face" },
                { "face_orbis2", "face" },
                { "face_ludi1", "face" },
                { "face_ludi2", "face" },
                { "lens_henesys1", "face" },
                { "lens_orbis1", "face" },
                { "lens_ludi1", "face" },
                { "skin_henesys1", "skin" },
                { "skin_orbis1", "skin" },
                { "skin_ludi1", "skin" },
                { "hair_henesys1", "hair" },
                { "hair_henesys2", "hair" },
                { "hair_kerning1", "hair" },
                { "hair_kerning2", "hair" },
                { "hair_orbis1", "hair" },
                { "hair_orbis2", "hair" },
                { "hair_ludi1", "hair" },
                { "hair_ludi2", "hair" },
                { "magician", "job" },
                { "fighter", "job" },
                { "change_swordman", "job2" },
                { "change_magician", "job2" },
                { "change_archer", "job2" },
                { "change_rogue", "job2" },
                { "inside_swordman", "job2" },
                { "inside_magician", "job2" },
                { "inside_archer", "job2" },
                { "inside_rogue", "job2" },
                { "warrior3", "job3" },
                { "wizard3", "job3" },
                { "bowman3", "job3" },
                { "thief3", "job3" },
                { "crack", "job3" },
                { "3jobExit", "job3" },
                { "holyStone", "job3" },
                { "sBoxItem0", "victoria3" },
                { "sBoxItem1", "victoria3" },
                { "sell_ticket", "contimove" },
                { "get_ticket", "contimove" },
                { "goOutWaitingRoom", "contimove" },
                { "boxPaper0", "zakum" },
                { "boxKey0", "zakum" },
                { "money100", "zakum" },
                { "money10000", "zakum" },
                { "boxItem0", "zakum" },
                { "boxItem1", "zakum" },
                { "boxItem2", "zakum" },
                { "boxItem3", "zakum" },
                { "boxBItem0", "zakum" },
                { "boxMob0", "zakum" },
                { "go280010000", "zakum" },
                { "Zakum00", "zakum" },
                { "Zakum01", "zakum" },
                { "Zakum02", "zakum" },
                { "Zakum03", "zakum" },
                { "Zakum04", "zakum" },
                { "Zakum05", "zakum" },
                { "Zakum06", "zakum" },
                { "boss", "zakum" },
                { "Populatus00", "populatus" },
                { "Populatus01", "populatus" },
                { "boss2", "populatus" },
                { "11000", "shop" },
                { "11100", "shop" },
                { "21000", "shop" },
                { "1001000", "shop" },
                { "1001001", "shop" },
                { "1001100", "shop" },
                { "1011000", "shop" },
                { "1011001", "shop" },
                { "1011100", "shop" },
                { "1012004", "shop" },
                { "1021000", "shop" },
                { "1021001", "shop" },
                { "1021100", "shop" },
                { "1031000", "shop" },
                { "1031001", "shop" },
                { "1031100", "shop" },
                { "1032103", "shop" },
                { "1051000", "shop" },
                { "1051001", "shop" },
                { "1051002", "shop" },
                { "1052104", "shop" },
                { "1061001", "shop" },
                { "1061002", "shop" },
                { "2012003", "shop" },
                { "2012004", "shop" },
                { "2012005", "shop" },
                { "2020001", "shop" },
                { "2022000", "shop" },
                { "2022001", "shop" },
                { "2022002", "shop" },
                { "2030009", "shop" },
                { "2040049", "shop" },
                { "2041002", "shop" },
                { "2041003", "shop" },
                { "2041006", "shop" },
                { "2041014", "shop" },
                { "2041016", "shop" },
                { "2060003", "shop" },
                { "2060004", "shop" },
                { "2060007", "shop" },
                { "9000015", "shop" },
                { "guildquest1_comment", "guildquest1" },
                { "guildquest1_board", "guildquest1" },
                { "guildquest1_knight", "guildquest1" },
                { "guildquest1_enter", "guildquest1" },
                { "guildquest1_clear", "guildquest1" },
                { "guildquest1_out", "guildquest1" },
                { "guildquest1_bonus", "guildquest1" },
                { "guild1F00", "guildquest1" },
                { "guild1F01", "guildquest1" },
                { "guild1F02", "guildquest1" },
                { "guild1F03", "guildquest1" },
                { "guild1F04", "guildquest1" },
                { "statuegate_open", "guildquest1" },
                { "guildquest1_statue", "guildquest1" },
                { "speargate_open", "guildquest1" },
                { "stonegate_open", "guildquest1" },
                { "metalgate_open", "guildquest1" },
                { "watergate_open", "guildquest1" },
                { "under30gate", "guildquest1" },
                { "secretgate1_open", "guildquest1" },
                { "secretgate2_open", "guildquest1" },
                { "secretgate3_open", "guildquest1" },
                { "ghostgate_open", "guildquest1" },
                { "kinggate2_open", "guildquest1" },
                { "kinggate_open", "guildquest1" },
                { "guildquest1_NPC1", "guildquest1" },
                { "guildquest1_will", "guildquest1" },
                { "guildquest1_baseball", "guildquest1" },
                { "syarenCheck", "guildquest1" },
                { "syarenItem0", "guildquest1" },
                { "syarenItem1", "guildquest1" },
                { "syarenItem2", "guildquest1" },
                { "syarenItem3", "guildquest1" },
                { "syarenItem4", "guildquest1" },
                { "syarenItem5", "guildquest1" },
                { "syarenItem6", "guildquest1" },
                { "syarenItem7", "guildquest1" },
                { "syarenItem8", "guildquest1" },
                { "syarenItem9", "guildquest1" },
                { "syarenItem10", "guildquest1" },
                { "syarenItem11", "guildquest1" },
                { "syarenItem12", "guildquest1" },
                { "syarenMob0", "guildquest1" },
                { "syarenMob1", "guildquest1" },
                { "syarenNPC0", "guildquest1" },
                { "9208000", "guildquest1" },
                { "9208001", "guildquest1" },
                { "9208002", "guildquest1" },
                { "9208003", "guildquest1" },
                { "9208004", "guildquest1" },
                { "9208005", "guildquest1" },
                { "9208006", "guildquest1" },
                { "9208007", "guildquest1" },
                { "9208008", "guildquest1" },
                { "9208009", "guildquest1" },
                { "9208010", "guildquest1" },
                { "9208011", "guildquest1" },
                { "9208012", "guildquest1" },
                { "9208013", "guildquest1" },
                { "pianus", "aqua" },
                { "aquaItem0", "aqua" },
                { "aquaItem1", "aqua" },
                { "aquaItem2", "aqua" }
            };
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
