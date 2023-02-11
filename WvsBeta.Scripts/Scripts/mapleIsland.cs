using System;
using WvsBeta.Game;
using WvsBeta.Game.Scripting;

namespace WvsBeta.Scripts.Scripts
{
    public static class mapleIsland
    {
        static INpcHost self;
        static GameCharacter target;
        static dynamic v;
        public static void HelpString()
        {
            v = self.AskMenu("Now... you can ask me anything you want to know about traveling!!\r\n#b#L0#How do I move?#l\r\n#L1#How do I eliminate monsters?#l\r \n#L2#How can I get an item?#l\r\n#L3#What happens when I die?#l\r\n#L4#When can I choose a career?#l\r\n #L5#Tell me more about this island!#l\r\n#L6# What should I do to become a Warrior?#l\r\n#L7# What should I do to become an Archer?#l \r\n#L8# What should I do to become a Thief?#l\r\n#L9# What should I do to become a Warlock?#l\r\n#L10# How do I increase my stats character? (S)#l\r\n#L11# How do I view the items I just picked up?#l\r\n#L12# How can I equip an item?#l\r\n#L13# How can I check equipped items?#l\r\n#L14# What are the skills?(K)#l\r\n#L15# How do I get to Victoria Island?#l\r\n#L16# What are mesos?#l");
            if (v == 0)
            {
                self.Say("Well done! That's how you move. Use the #bleft and right arrows#k to move on flat terrain and roads and press #bAlt#k to jump. Some types of shoes make you improve your speed and jump skills.");
            }
            else if (v == 1)
            {
                self.Say("Now, how to eliminate a monster. Each monster has its own HP, and you can finish them off by attacking them with a weapon or with spells. Of course, the stronger they are, the harder they are to be eliminated .");
                self.Say("To be able to attack the monsters, you must be equipped with weapons. When the weapon is equipped, press #bCtrl#k to use it. With a little training, you can eliminate the monsters easily.");
                self.Say("Once you have advanced in the career, you will get different types of skills and you can assign hotkeys to them in order to access them more easily. If it is an attack skill, you don't need to press CTRL to attack; just press the designated hotkey for that weapon.");
            }
            else if (v == 2)
            {
                self.Say("This is how you get an item. After you have eliminated a monster, an item will be dropped. When this happens, stand in front of the item and press #bZ#k or #b0 on the number pad#k to get it it.");
                self.Say("Keep in mind that if your inventory is full, you won't be able to store new items. So if you have an item that you don't need, you can sell it or do something else with it. You can gain more inventory slots when advancing in career.");
            }
            else if (v == 3)
            {
                self.Say("Are you #Gcurious:curious# to know what happens when you die? When your HP reaches zero, you will become a ghost. There will be a tombstone at this location and you will not be able to move, but you will still be able to chat .");
                self.Say("You don't lose much if you die when you're just an apprentice. Once you have a career, that's a whole different story. You'll lose some of your EXP points when you die, so be careful not to die when in danger.");
            }
            else if (v == 4)
            {
                self.Say("When can you choose your career? Hahaha, take it easy, mate. Each career has a set of requirements you need to meet. Generally, a level between 8 and 10 is ideal, so work hard .");
                self.Say("Level is not the only thing that determines your career advancement. You must also level up a particular skill based on your career. For example, to become a Warrior, your STR must be above 35, and so on, get it? Make sure to increase skills that are directly tied to your career.");
            }
            else if (v == 5)
            {
                self.Say("Want to know more about this island? It's called Maple Island, and it's floating in the air. It's been floating in the sky for quite some time, so those nasty monsters don't usually hang around. It's a very peaceful island ! Perfect for learners!");
                self.Say("But if you want to be a power player, you better not think about staying here. After all, you won't be able to get a career. Below this island there is another huge island called Victoria Island. That place is so much bigger that this one...");
                self.Say("How do you get to Victoria Island? In the east part of this island, there is a port called #m60000#. There, you will find a flying ship. In front of the ship is the captain. Ask him.");
                self.Say("Oh yeah! One more piece of info before you go. If you don't know where you are, just hit the #bW#k key. The world map will pop up with an indicator showing where you are. With that feature, you don't have to worry if you get #Glost:lost#.");
            }
            else if (v == 6)
            {
                self.Say("Do you wish to become #Gum warrior:a warrior#? Hmm, then I suggest you go to Victoria Island. Go to the warrior city called #rPerion#k and talk to #bChief Balrog#k. He will teach you how to become #Gum warrior:a real warrior#. Oh! And there's one more very important thing: You will have to be at least level 10 to become #Gwarrior:warrior#!");
            }
            else if (v == 7)
            {
                self.Say("Do you wish to become #Gum archer:an archer#? You will have to go to Victoria Island to advance in the career. Go to the archer town called #rHenesys#k and talk to the beautiful #bAthene Certeira #k and learn the ups and downs of being #Gum archer:an archer# Oh! And there's one more very important thing: You'll have to be at least level 10 to become #Gum archer:an archer#!! ");
            }
            else if (v == 8)
            {
                self.Say("Do you want to become #Gum burglar:a burglar#? To become one, you will have to go to Victoria Island. Go to the burglar town known as #rKerning#ke, in the darkest part of town , you will find a burglar hideout. There you will find the #blacklord#k, who will teach you about how to be #Gum burglar:a burglar#. Oh! And there is one more very important thing: You will have to be level 10, at the very least, to become #Gum cat:a cat#!!");
            }
            else if (v == 9)
            {
                self.Say("Do you wish to become #Gumwitch:awitch#? To do this, you will have to go to Victoria Island. Go to the wizarding town known as #rEllinia#ke, there, on the highest part of it , is the Magic Library. Inside it you will find the chief of all wizards, #bGrendel the Very Old#k, who will teach you all about becoming a #Gum wizard:a witch#.");
                self.Say("Ah! By the way, unlike other careers, to become a magician, you only need to be level 8. Actually, the ease of being able to become a magician earlier is because it takes a lot of time for someone become a truly powerful magician. Think carefully and choose your path carefully.");
            }
            else if (v == 10)
            {
                self.Say("Do you want to know how to increase your character's skill attributes? First, press S to check the skills window. Every time you level up, you will receive 5 skill points (also known as PH). Spend those points on the skill of your choice. It's that simple.");
                self.Say("Hover your mouse cursor over abilities for a quick explanation of them. For example, STR for fighters, DEX for archers, INT for magicians, and LUCK for thieves. That's not all you need to know , so you will have to think hard about how to distribute the points in order to emphasize your character's strength, according to your choice.");
            }
            else if (v == 11)
            {
                self.Say("You want to know how to view the items you've picked up, don't you? When you defeat a monster, it will drop an item on the ground, and you can press Z to pick up that item. That item will be stored in your inventory , and you can view it by pressing the I key.");
            }
            else if (v == 12)
            {
                self.Say("You want to know how to equip items, right? Press the I key to open the inventory window. Position your mouse cursor over an item and double-click it to equip it to your character. If If you are unable to use an item, it is very likely that your character does not meet the stats and level requirements. You can also use the item by opening the equip inventory (E) and dragging the item onto the character. To unequip an item, double-click the item in the equip inventory.");
            }
            else if (v == 13)
            {
                self.Say("You want to check your equipped items, right? Press the E key to open the gear inventory, where you can see exactly what you are currently wearing. To unequip an item, double-click it. The item will be sent to inventory.");
            }
            else if (v == 14)
            {
                self.Say("The special skills you get after achieving a career are called 'skills'. You will acquire specific skills according to your career. You are not at that stage yet, so you don't have any skills yet. But remember, to check your skills, just press K to open the Skill Book. This will help you in the future.");
            }
            else if (v == 15)
            {
                self.Say("You can go to Victoria Island on a ship that leaves from Porto Sul and docks at Porto Lith. Press the W key to view the world map and you will see where on the island you are. Find Porto South on the map. That's where you should go. You'll also need some mesos for the trip, so you might need to eliminate some monsters in the vicinity.");
            }
            else if (v == 16)
            {
                self.Say("Mesos is the currency used in MapleStory. With mesos, you can buy items. To earn them, you can defeat monsters, sell items in the shop, or complete quests...");
            }
            HelpString();
        }
        // 2. use the items
        [Script("begin2")]
        class begin2 : INpcScript
        {
            dynamic right, val, ret, inventory;
            public void Run(INpcHost self, GameCharacter target)
            {
                mapleIsland.self = self;
                mapleIsland.target = target;
                var qr = target.QuestRecord;
                val = qr.GetState(1002);

                inventory = target.Inventory;
                if (val == 0)
                {
                    if (target.Gender == 0)
                    {
                        self.Say("Hey, you there! Can I talk to you for a minute? Hahah! I'm #p2000#, an instructor helping new travelers like you.");
                        self.Say("Who told you to do THIS? HAHAHAH! You are a VERY curious traveler! Good, good, good... I do it because I want to. That's it.");
                    }
                    else if (target.Gender == 1)
                    {
                        self.Say("Hey! You there! Are you free? Hehe... I'm #p2000#, the instructor, and I love chatting with nice girls like you, and of course helping out during the game.# I");
                        self.Say("Hey! Give me a minute of your time. I'll give you lots of valuable information. Anything for a cutie like you. Hahaha!!!#I");
                    }
                    self.Say("Alright! Let's have fun! Yahh!");
                    double val2 = target.HP / 2;
                    target.IncHP(-val2, 0);
                    self.Say("Surprised? You can't keep your HP below 0, I'll give you a #r#t2010007##k to eat. You can recover your strength like this. Open your inventory and double click on it.");
                    self.Say("You'll have to eat every #t2010007# I gave you, but you can also recover HP by standing there, so come talk to me when you've fully recovered your HP.");
                    right = inventory.Exchange(0, 2010007, 1);
                    if (ret == 0) self.Say("Didn't you eat a little too much?");
                    else qr.Set(1002, "");
                }
                else if (val == 1 && inventory.ItemCount(2010007) == 0 && target.HP == target.MHP)
                {
                    self.Say("What do you do if you want to get the item? It's easy, right? You can assign it as a #bHotkey#k in the lower right corner of the screen. You didn't know that, did you? Hahaha!");
                    self.Say("Okay! You've learned a lot, so here's a little treat. You shouldn't thank me for learning a skill. Use it when necessary.");
                    self.Say("This is all I can teach you. It's sad, but I have to say goodbye. And be careful out there. See you later...");
                    target.IncEXP(2, 0);
                    right = inventory.Exchange(0, 2000000, 3, 2000003, 3);
                    if (ret == 0) self.Say("Didn't you eat a little too much?");
                    qr.SetComplete(1002);
                }
                else if (inventory.ItemCount(2010007) > 0) self.Say("Come on, eat the #r#t2010007##k I gave you~ Open your inventory and press the #b'Use'#k tab, then click the #t2010000# twice to get it.");
                else if (target.HP != target.MHP) self.Say("You haven't fully recovered your strength. Did you really eat the #t2010007# I gave you? Are you certain?");
                else self.Say("The weather is great today!");
            }
        }
        // 4. Character control.
        [Script("begin4")]
        class begin4 : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                mapleIsland.self = self;
                mapleIsland.target = target;
                if (target.Job != 0)
                {
                    self.Say("This is the quest area for apprentices. You're not an apprentice, are you?");
                    target.ChangeMap(104000000, "");
                }
                else
                {
                    self.Say("You've come this far... amazing! You can start traveling around right now! Okay, I'll take you to the next stop.");
                    self.Say("But I'll give you a piece of advice: Once you leave here, you're free, in places with lots of monsters and no way to get back. Well then, see you later!");
                    target.IncEXP(3, 0);
                    target.ChangeMap(40000, "");
                }
            }
        }
        // 5 . ÀüÅõ,Á×±â  
        [Script("begin5")]
        class begin5 : INpcScript
        {
            dynamic v;
            public void Run(INpcHost self, GameCharacter target)
            {
                mapleIsland.self = self;
                mapleIsland.target = target;
                HelpString();
            }
        }
        // 7. sending the character to victoria island.  
        [Script("begin7")]
        class begin7 : INpcScript
        {
            dynamic ret, nRet;
            public void Run(INpcHost self, GameCharacter target)
            {
                mapleIsland.self = self;
                mapleIsland.target = target;
                nRet = self.AskYesNo("Take that ship and you'll leave for a bigger continent. With #e150 mesos#n I can take you to #Victoria Ball#k. Problem is, once you leave here, you can't come back never again. What do you think? Do you want to go to Victoria Island?");
                if (nRet == 0) self.Say("Hmm... I suppose you still have some work to do here?");
                else if (nRet == 1)
                {
                    if (target.Level < 7) self.Say("Let's see... I don't think you're strong enough. You have to be at least #bLevel 7 #k to go to Victoria Island.");
                    else
                    {
                        self.Say("Are you #Gtired:tired# of this place? Well... first, give me #e150 mesos#n...");
                        ret = target.IncMoney(-150, 1);
                        if (ret == 0) self.Say("What? Are you trying to say that you want to travel without any money? You are #Girl:weird#...");
                        else
                        {
                            self.Say("Too much! #e150 mesos#n accepted! Well done! Let's go to the #Victoria pool#k!");
                            target.ChangeMap(104000000, "maple00");
                        }
                    }
                }
            }
        }
        // 8 . Ambassador Bari  
        [Script("bari")]
        class bari : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                mapleIsland.self = self;
                mapleIsland.target = target;
                self.Say("This town is called #b#m60000##k and it's east of Maple Island. Don't you want to advance your career so you can fight much stronger monsters? So, board the ship and head to the #bIle Victoria#k.");
                self.Say("You'll need 150 mesos, but it's not that much money... you can get that amount by eliminating the monsters scattered around the island. You can even sell items in shops around town.");
            }
        }
        // 10. Rain's lines  
        [Script("rein")]
        class rein : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                mapleIsland.self = self;
                mapleIsland.target = target;
                self.Say("This town is called #b#m1010000##k and is northeast of Maple Island. You knew that Maple Island is only for apprentices, didn't you? I'm glad there are only weak monsters around here.");
                self.Say("If you want to get stronger, go to #b#m60000##k where there is a port, and there you can take a gigantic ship and land in a place called #bIlha Victoria#k. It is much bigger than this little island.");
                self.Say("In Victoria Island you can choose your career. Is it called #b#m102000000##k...? I heard that there is a desolate, open-air city where warriors live. What kind of place would that be?");
            }
        }
        // 11. Pio's lines  
        [Script("pio")]
        class pio : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                mapleIsland.self = self;
                mapleIsland.target = target;
                self.Say("Here in #m1010000#, there are shops for weapons and supplies, and in #m60000#, a port with a huge ship called Victoria, and that's where you can find a shop that sells shields.");
                self.Say("A few days ago, I borrowed a hammer with #p11000# from the gun shop, and the hammer broke. What should I do?");
            }
        }
    }
}
