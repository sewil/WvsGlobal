using System;
using System.Collections.Generic;
using System.Linq;
using WvsBeta.Common.Interfaces;
using WvsBeta.Common.Sessions;
using WvsBeta.Game.Handlers.MiniRoom;

namespace WvsBeta.Game.GameObjects.MiniRoom
{
    public class MatchCard : MiniGameRoom
    {
        public IDictionary<byte, int> Cards { get; }
        public HashSet<byte> FlippedCards { get; }
        public KeyValuePair<byte,int> FirstFlip;
        private int[] matches;
        private readonly int cardsCount;
        public MatchCard(GameCharacter owner, string pTitle, bool pPrivate, string pPassword, MatchCardsSize boardSize) : base(owner, 2, MiniRoomType.MatchCards, pTitle, pPassword, pPrivate, (byte)boardSize, MiniGameType.MatchCards)
        {
            Cards = new Dictionary<byte, int>();
            cardsCount = boardSize.GetCardsCount();
            FlippedCards = new HashSet<byte>();
            matches = new int[2];
            this.Initialize();
        }

        private void ShuffleCards()
        {
            Cards.Clear();
            // Cards 0-14 (15 cards), always two per game, so 30 max (6*5)
            // Get random card ids to use
            var cardIds = Enumerable.Range(0, 15).ToList().Shuffle().Take(cardsCount / 2).ToList();

            // Set card pairs
            var cardList = new int[cardsCount];
            for (byte i = 0; i < cardsCount; i++)
            {
                int cardId = cardIds[(int)Math.Floor((double)i / 2)];
                cardList[i] = cardId;
            }
            // Shuffle all cards
            cardList.Shuffle();
            for (byte i = 0; i < cardList.Length; i++) Cards.Add(i, cardList[i]);
        }

        public override void StartGame(GameCharacter chr)
        {
            FlippedCards.Clear();
            matches = new int[2];
            ShuffleCards();

            Packet pw = new Packet(ServerMessages.MINI_ROOM_BASE);
            pw.WriteByte((byte)MiniRoomOpServer.GameStart);
            pw.WriteByte(mWinnerIndex); //0 Would let slot 1, 1 would let slot 0
            pw.WriteByte((byte)Cards.Count);
            foreach (var card in Cards)
            {
                pw.WriteInt(card.Value);
            }
            SendStartGame(pw);
        }

        public override void EndGame(GameCharacter winner, GameResult result)
        {
            base.EndGame(winner, result);
        }

        public void FlipCard(GameCharacter chr, bool first, byte idx)
        {
            if (FlippedCards.Contains(idx))
            {
                Program.MainForm.LogAppend("[MatchCards] Tried flipping card already flipped " + idx);
                return;
            }
            if (!Cards.TryGetValue(idx, out int cardId))
            {
                Program.MainForm.LogAppend("[MatchCards] Tried flipping non-existing card " + idx);
                return;
            }
            if (first)
            {
                FirstFlip = new KeyValuePair<byte, int>(idx, cardId);
            }
            bool match = !first && FirstFlip.Value == cardId;
            if (match)
            {
                FlippedCards.Add(FirstFlip.Key);
                FlippedCards.Add(idx);
                chr.GameStats[MiniGameType.MatchCards].points++;
                matches[chr.RoomSlotId]++;
            }
            MiniGamePacket.FlipCard(chr, this, first, idx, FirstFlip.Key, match);
            bool gameEnded = FlippedCards.Count == Cards.Count;
            if (gameEnded)
            {
                int ownerMatches = matches[0];
                int visitorMatches = matches[1];
                GameResult result = ownerMatches != visitorMatches ? GameResult.Win : GameResult.Tie;
                GameCharacter winner = ownerMatches > visitorMatches ? Owner : GetVisitor();
                // End game
                EndGame(winner, result);
            }
        }
    }
}
