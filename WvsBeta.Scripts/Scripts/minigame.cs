using System;
using WvsBeta.Game;
using WvsBeta.Game.Scripting;

namespace WvsBeta.Scripts.Scripts
{
    public static class minigame
    {
        static INpcHost self;
        static GameCharacter target;
        public static void makeItem(int setID, int needItem1, int needItem2, int needItem3)
        {

            var inventory = target.Inventory;

            if (inventory.ItemCount(needItem1) >= 99 && inventory.ItemCount(needItem2) >= 99 && inventory.ItemCount(needItem3) >= 1)
            {
                self.Say("Wow, you really collected #r#t" + needItem1 + "#, #t" + needItem2 + "#, and #t" + needItem3 + "##k! Okay... you did bring all the necessary items. Right, wait a second and I'll make it right away.");
                var ret = inventory.Exchange(0, needItem1, -99, needItem2, -99, needItem3, -1, setID, 1);
                if (ret == 0) self.Say("Are you sure you collected #b#t" + needItem1 + "#, #t" + needItem2 + "#, #t" + needItem3 + "##k? If so, please check your etc. inventory if it's full or not.");
                else
                {
                    self.Say("Here is #b#t" + setID + "##k! You can open the Omok Room anywhere in the game and enjoy playing against other Maple players. If you end up with an impressive score, something good can come of it. I'll cheer you on, now go play!");
                    self.Say("Oh, and if you have any questions about playing Omok, feel free to ask. I'll be here for a while. Well, practice playing with other players until you feel like you can beat me at minigames. But of course that will never happen, hahaha. Right, I'm out~");
                }
            }
        }
        // In relation to the minigames
        [Script("minigame00")]
        class minigame00 : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                minigame.self = self;
                minigame.target = target;
                var field = self.Field;
                var nField = field.ID;
                int v1 = 0;

                if (nField == 100000203) v1 = self.AskMenu("Hey, looks like you need some rest from the hunt. You should be enjoying life like me. Well, if you have some items, I can trade you for an item used to play minigames. So... what can I do for you?",
                    "Create a minigame item",
                    "Explain more about minigames"
                );
                else if (nField == 220000300) v1 = self.AskMenu("Well, hello! I'm #b#p2040014##k and I'm responsible for everything involving minigames. You seem to have a certain interest in that so... I can certainly help you! Alright... so what can I do for you?",
                    "Create a minigame item",
                    "Explain more about minigames"
                );

                if (v1 == 0)
                {
                    var v2 = self.AskMenu("So you want to make a minigame item? Minigames aren't something you can just start playing out of thin air. You will need a certain item for certain minigames. Which minigame item would you like to make?",
                        "Omok Set",
                        "A set of Match Cards"
                    );
                    var inventory = target.Inventory;
                    if (v2 == 0)
                    {
                        self.Say("So you want to play #bOmok#k, huh? In order to play, you will need the Omok Set. Only those who have this item will be able to open the Omok game room. You will be able to play it almost anywhere, except for a few places in the Market.");

                        string ask = "The set will also look different depending on the pieces you want to use for the game. Which set would you like to make?";

                        if (nField == 100000203)
                        {
                            var v4 = self.AskMenu(ask + "\r\n#b#L0##t4080000##l\r\n#b#L1##t4080001##l\r\n#b#L2##t4080002##l\r\n#b#L3##t4080003##l\r\n#b#L4##t4080004##l\r\n#b#L5##t4080005##l");
                            if (v4 == 0) makeItem(4080000, 4030000, 4030001, 4030009);
                            else if (v4 == 1) makeItem(4080001, 4030000, 4030010, 4030009);
                            else if (v4 == 2) makeItem(4080002, 4030000, 4030011, 4030009);
                            else if (v4 == 3) makeItem(4080003, 4030010, 4030001, 4030009);
                            else if (v4 == 4) makeItem(4080004, 4030011, 4030010, 4030009);
                            else if (v4 == 5) makeItem(4080005, 4030011, 4030001, 4030009);
                        }
                        else
                        {
                            var v4 = self.AskMenu(ask + "\r\n#b#L0# #t4080006##l\r\n#b#L1# #t4080007##l\r\n#b#L2# #t4080008##l\r\n#b#L3# #t4080009##l\r\n#b#L4# #t4080010##l\r\n#b#L5# #t4080011##l");
                            if (v4 == 0) makeItem(4080006, 4030013, 4030014, 4030009);
                            else if (v4 == 1) makeItem(4080007, 4030013, 4030016, 4030009);
                            else if (v4 == 2) makeItem(4080008, 4030014, 4030016, 4030009);
                            else if (v4 == 3) makeItem(4080009, 4030015, 4030013, 4030009);
                            else if (v4 == 4) makeItem(4080010, 4030015, 4030014, 4030009);
                            else if (v4 == 5) makeItem(4080011, 4030015, 4030016, 4030009);
                        }
                    }
                    else if (v2 == 1)
                    {
                        self.Say("So you want to make #b#t4080100##k? Hmmm... in order to make #t4080100#, you'll need some #b#t4030012#s#k. #t4030012# can be obtained by defeating monsters around the island. Collect 99 #t4030012#s and you will be able to craft #t4080100#.");
                        if (inventory.ItemCount(4030012) >= 99)
                        {
                            self.Say("Wow, you really collected #r99 #t4030012#s#k!! Great... Okay, this is going to be fun. Just a moment~ I will make #r#t4080100##k for you in a heartbeat.");
                            var ret = inventory.Exchange(0, 4030012, -99, 4080100, 1);
                            if (ret == 0) self.Say("Are you sure you collected #r99 #t4030012#s#k? If so, please check your etc. inventory if it's full or not.");
                            else
                            {
                                self.Say("Here you go, #b#t4080100##k! The \"Match Cards\" Room can be played almost anywhere in the game. Just open a room and you will be able to play with many other players. If you end up with an impressive score, then something good can come of it. I'll cheer you on, now go play!");
                                self.Say("Oh, and if you have any questions about Match Cards, don't hesitate to ask. I'll be here for a while. Well, practice playing with other players until you feel like you will be able to beat me at the minigames. But of course that will never happen, hahaha... Alright, I'm out~");
                            }
                        }
                    }
                }
                else if (v1 == 1)
                {
                    var v3 = self.AskMenu("You want to learn more about minigames? Awesome! Ask anything you want. Which minigame would you like to know more about?",
                        "Omok",
                        "Match Cards"
                    );
                    if (v3 == 0)
                    {
                        self.Say("These are the rules for the Omok game. Listen carefully. Omok is a game where you and your opponent will, one at a time, place a piece on the board until someone finds a way to place exactly 5 pieces in a row, either horizontally, diagonally, or vertically. Whoever succeeds in doing so will be declared the winner. For starters, only those with the #bOmok Set#k can open a game room.");
                        self.Say("Each game of Omok will cost #r100 mesos#k. Even if you don't have the #bOmok Set#k, you can still enter the room and play. But if you don't have 100 mesos, you won't be able to enter the room, period. The person who opens the room will also need to have 100 mesos for this, or there is no game. If you run out of mesos during the game, you will be automatically expelled from the room!");
                        self.Say("Enter the room, and when you are ready to play, click #bReady#k. When the visitor clicks #bReady#k as well, the room owner can press #bStart#k to start the game. If you receive any unwanted visitors, the owner of the room has a right to expel them. There is a square with an X written on it to the right of the player. Simply click on it for a quick goodbye, alright?");
                        self.Say("When the game starts, #bthe owner of the room plays first#k. Remember that there is a time limit, and you may lose your turn if you don't play in time. Generally, 3x3 is not allowed, but if you get to a point where you absolutely have to place your piece there or lose, you can. 3x3 is allowed as the last line of defense! Oh, and #r6 or 7 straight#k doesn't count. Only 5!");
                        self.Say("If you find yourself pinned against the wall, you can request to #bRedo#k your move. If the opponent accepts the request, then both of your last moves will be cancelled. If you feel like you need to go to the bathroom or take a break, you can request a #bDraw#k. The game will end in a draw if the opponent accepts your request. This can be a good way to keep friendships intact.");
                        self.Say("When the game is over, a new one will start, and the loser will go first. Oh, and you can't quit mid-game. If you do this, you need to either #bgive up or call for a draw#k. Of course, if you give up, you will lose the game. So be careful. And if you click \"Leave\" during the game and ask to leave the game, you will leave as soon as the game ends. So this is a much more efficient way to exit.");
                    }
                    else if (v3 == 1)
                    {
                        self.Say("These are the rules for the Match Cards game. Listen carefully. The Match Cards game works exactly like the name says: Match a pair of cards among the cards placed on the board. When all matched pairs are found, the player with the most pairs wins the game. Like Omok, you will need #b#t4080100##k to open the game room.");
                        self.Say("Each game of Match Cards will cost #r100 mesos#k. Even if you don't have #b#t4080100##k, you can still enter the room and play. However, if you don't have 100 mesos, you won't be able to enter the room, period. The player who opens the room will also need to have 100 mesos on them, or there is no game. If you run out of mesos during the game, you will be automatically expelled from the room!");
                        self.Say("Enter the room, and when you are ready to play, click #bReady#k. When the visitor clicks #bReady#k as well, the room owner can press #bStart#k to start the game. If you receive any unwanted visitors, the owner of the room has a right to expel them. There is a square with an X written on it to the right of the player. Simply click on it for a quick goodbye, alright?");
                        self.Say("Oh, and unlike Omok, when you create a game room in Match Cards, you need need to set the number of faces you will use in the game. There are 3 modes available; 3x4, 4x5, and 5x6, which will require 12, 20, and 30 cards, respectively. Remember that you will be unable to trade once the room is open. Therefore, if you really want to trade, you will need to close the room first and then open a new one after.");
                        self.Say("When the game starts, #bthe owner of the room plays first#k. Remember that there is a time limit, and you may lose your turn if you don't play in time. When you find a matching pair on your turn, you can play again, as long as you keep finding matching cards. Use your memory for a devastating turn-based combo.");
                        self.Say("If you and your opponent have the same number of matched cards, the one with the greater gap is the winner. If you feel like you need to go to the bathroom or take a break, you can request a #bDraw#k. The game will end in a draw if the opponent accepts your request. This can be a good way to keep friendships intact.");
                        self.Say("When the game is over, a new one will start, and the loser will go first. Oh, and you can't quit mid-game. If you do this, you need to either #bgive up or call for a draw#k. Of course, if you give up, you will lose the game. So be careful. And if you click \"Leave\" during the game and ask to leave the game, you will leave as soon as the game ends. So this is a much more efficient way to exit.");
                    }
                }
            }
        }
    }
}
