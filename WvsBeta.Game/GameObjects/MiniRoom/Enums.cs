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
        RequestTie = 24,
        RequestTieResult = 25,
        GiveUp = 26,
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

    public enum MiniGameError : byte
    {
        RoomAlreadyClosed = 0x01,
        FullCapacity = 0x02,
        OtherRequests = 0x03,
        CantWhileDead = 0x04,
        CantInMiddleOfEvent = 0x05,
        UnableToDoIt = 0x06,
        OtherItemsAtPoint = 0x07, // or 0x0E
        CantEstablishRoom = 0x0A,
        Trade2OnSameMap = 0x09,
        NotEnoughMesos = 0x0F,
        CantStartGameHere = 0x0B,
        BuiltAtMainTown = 0x0C,
        UnableToEnterTournament = 0x0D,
        IncorrectPassword = 0x10,
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
