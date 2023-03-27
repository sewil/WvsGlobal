using WvsBeta.Common.Interfaces;
using WvsBeta.Game.Handlers.MiniRoom;

namespace WvsBeta.Game.GameObjects.MiniRoom
{
    public class MatchCard : MiniGameRoom
    {
        public MatchCard(GameCharacter owner, string pTitle, bool pPrivate, string pPassword, byte pieceType) : base(owner, 2, MiniRoomType.MatchCards, pTitle, pPassword, pPrivate, pieceType, MiniGameType.MatchCards)
        {
            this.Initialize();
        }
    }
}
