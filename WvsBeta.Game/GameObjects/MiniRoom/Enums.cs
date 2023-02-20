namespace WvsBeta.Game.GameObjects.MiniRoom
{
    public enum MiniRoomAction
    {
        Create = 0,
        Invite = 2,
        DeclineInvite = 3,
        Enter = 4,
        Chat = 6,
        Leave = 10,
        AddAnnounceBox = 11,
        AddShopItem = 18,
        BuyShopItem = 19,
        MoveItemShopToInv = 23,
        RequestTie = 25,
        RequestHandicap = 28,
        RequestHandicapResult = 29,
        Ready = 32,
        Unready = 33,
        Expel = 34,
        StartOmok = 35,
        OmokWinner = 37,
        PlaceOmokPiece = 38,
    }
    public enum MiniRoomType
    {
        None = 0,
        Omok = 1,
        MatchCards = 2,
        Trade = 3,
        PersonalShop = 4,
        EntrustedShop = 5,
    }
    public enum MatchCardsType
    {
        Red = 0,
        Green = 1,
        Blue = 2
    }
    public enum MiniRoomBaseType
    {
        Invite = 2,
        InviteResult = 3,
        Enter = 4,
        EnterResult = 5,
        Avatar = 9,
        Leave = 10
    }
    public enum MiniRoomBaseError
    {
        YouCantEstablishAMiniRoomHere = 1,
        Unk = 2,
        Closed = 3
    }
    public enum GameResult
    {
        Win = 0,
        Tie = 1,
        Forfeit = 2,
    }
}
