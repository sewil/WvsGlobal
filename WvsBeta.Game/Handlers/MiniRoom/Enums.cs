namespace WvsBeta.Game.GameObjects.MiniRoom
{
    public enum MiniRoomOpClient
    {
        Create = 0,
        Invite = 2,
        DeclineInvite = 3,
        Enter = 4,
        Chat = 6,
        Leave = 10,
        AddAnnounceBox = 11,
        TradePutItem = 13,
        TradePutMesos = 14,
        TradeSelect = 15,
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
    public enum MiniRoomOpServer
    {
        Invite = 2,
        InviteResult = 3,
        Enter = 4,
        EnterResult = 5,
        Chat = 6,
        Avatar = 9,
        Leave = 10,
        TradePutItem = 13,
        TradePutMesos = 14,
        TradeSelect = 15,
        MiniGameUpdate = 0x24
    }
    public enum MiniRoomType
    {
        None = 0,
        Omok = 1,
        MatchCards = 2,
        Trade = 3,
        PlayerShop = 4,
        EntrustedShop = 5,
    }
    public enum MiniGameType : int
    {
        Omok = 1,
        MatchCards = 2
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
    public enum MiniRoomLeaveReason : byte
    {
        YouHaveLeft = 0,
        YouCantEstablishAMiniRoomHere = 1,
        Cancel = 2,
        RoomIsClosed = 3,
        Expelled = 4,
        TradeSuccessful = 6,
        TradeUnsuccessful = 7,
        TradeInventoryFull = 8,
        TradeWrongMap = 9,
    }
    public enum GameResult
    {
        Win = 0,
        Tie = 1,
        Forfeit = 2,
    }
}
