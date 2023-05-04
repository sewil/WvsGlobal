namespace WvsBeta.Game.GameObjects.MiniRooms
{
    public enum MiniRoomOpClient
    {
        Create = 0,
        Invite = 2,
        DeclineInvite = 3,
        Enter = 4,
        Chat = 6,
        Leave = 10,
        Open = 11,
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
        RequestLeave = 30,
        CancelRequestLeave = 31,
        Ready = 32,
        Unready = 33,
        Expel = 34,
        StartGame = 35,
        EndGame = 37,
        PlaceOmokPiece = 38,
        MatchCardsPickCard = 0x2A
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
        PlayerShopItemResult = 0x14,
        PlayerShopSoldItemResult = 0x16,
        GameRequestTie = 0x18,
        GameRequestTieDeny = 0x19,
        GameRequestHandicap = 0x1C,
        GameRequestHandicapResult = 0x1D,
        GameReady = 0x20,
        GameUnready = 0x21,
        GameStart = 0x23,
        GameEnd = 0x24,
        MatchCardsTurn = 0x25,
        OmokMovePiece = 0x26,
        MatchCardsFlipCard = 0x2A,
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

    public enum MiniRoomEnterError : byte
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
    public enum PlayerShopItemResult : byte
    {
        NotEnoughInStock = 1,
        NotEnoughMesos = 2,
        PriceOfItemTooHighForTrade = 3,
        BuyerDoesNotPossessEnoughMoney = 4,
        CannotCarryMoreThanOne = 5,
        InventoryFull = 6,
    }
    public enum MiniRoomLeaveReason : byte
    {
        Leave = 0,
        YouCantEstablishAMiniRoomHere = 1,
        Cancel = 2,
        Closed = 3,
        Expelled = 4,
        ForcedLeave = 5,
        TradeSuccessful = 6,
        TradeUnsuccessful = 7,
        TradeInventoryFull = 8,
        TradeWrongMap = 9,
        PlayerShopOutOfStock = 10
    }
    public enum GameResult
    {
        Win = 0,
        Tie = 1,
        Forfeit = 2,
    }
    public enum MiniRoomChatType
    {
        Text = 0,
        Notice = 7
    }
    public enum MiniRoomChatNoticeType
    {
        Expelled = 0,
        PlayersTurn = 1,
        Forfeited = 2,
        HandicapRequested = 3,
        PlayerLeft = 4,
        CalledToLeave = 5,
        LeaveCancelled = 6,
        PlayerHasEntered = 7,
        CantStartGameNotEnoughMesos = 8,
        HasMatchedCardsPleaseContinue = 9,
        TenSecondsLeft = 101,
        GameHasStarted = 102,
        GameHasEnded = 103,
    }
    public enum MatchCardsSize
    {
        Small = 0,
        Medium = 1,
        Large = 2
    }
    public static class EnumExtensions
    {
        public static byte GetCardsCount(this MatchCardsSize size)
        {
            switch (size)
            {
                case MatchCardsSize.Small: return 4 * 3;
                case MatchCardsSize.Medium: return 5 * 4;
                case MatchCardsSize.Large: return 6 * 5;
                default: return 0;
            }
        }
    }
}
