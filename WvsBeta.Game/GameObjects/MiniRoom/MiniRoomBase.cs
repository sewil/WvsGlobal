using System.Collections.Generic;
using WvsBeta.Common;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Game.GameObjects.MiniRoom
{
    public class MiniRoomBase
    {
        public const int MAX_PLAYERS = 2;
        protected string _transaction = Cryptos.GetNewSessionHash();
        public string TransactionID => _transaction;

        public static Dictionary<int, MiniRoomBase> MiniRooms = new Dictionary<int, MiniRoomBase>();
        public static Dictionary<int, PlayerShop> PlayerShops = new Dictionary<int, PlayerShop>();
        public static Dictionary<int, Omok> Omoks = new Dictionary<int, Omok>();

        public int ID { get; protected set; }
        public int BalloonID { get; protected set; }
        public string Title { get; protected set; }
        public string Password { get; protected set; }
        public byte MaxUsers { get; protected set; }
        public byte EnteredUsers { get; protected set; }
        public GameCharacter[] Users { get; protected set; }
        public bool Opened { get; protected set; }
        public bool Private { get; protected set; }
        public bool CloseRequest { get; protected set; }
        public bool GameStarted { get; set; }
        public bool Tournament { get; protected set; }
        public int RoundID { get; protected set; }
        public Pos mHost { get; protected set; }
        public MiniRoomType Type { get; private set; }
        public MatchCardsType CardType { get; protected set; }
        public int ObjectID { get; private set; }
        public byte PieceType { get; private set; }
        public byte mWinnerIndex { get; set; }
        public GameCharacter Owner { get; private set; }

        public MiniRoomBase(GameCharacter owner, byte pMaxUsers, MiniRoomType pType)
        {
            Owner = owner;
            ID = Server.Instance.MiniRoomIDs.NextValue();
            MiniRooms.Add(ID, this);
            Title = "";
            Password = "";
            MaxUsers = pMaxUsers;
            Users = new GameCharacter[MaxUsers];
            Opened = false;
            CloseRequest = false;
            Tournament = false;
            GameStarted = false;
            Type = pType;
        }

        public virtual void Close(byte pReason)
        {
            MiniRooms.Remove(ID);
            Owner = null;
            for (var i = 0; i < MaxUsers; i++)
                Users[i] = null;
        }

        public byte GetEmptySlot()
        {
            for (byte i = 0; i < MaxUsers; i++)
            {
                if (Users[i] == null)
                {
                    return i;
                }
            }

            return 0xFF;
        }

        public byte GetCharacterSlotID(GameCharacter pCharacter)
        {
            return pCharacter.RoomSlotId;
        }

        public void BroadcastPacket(Packet pPacket, GameCharacter pSkipMeh = null, bool playersOnly = false)
        {
            for (int i = 0; i < Users.Length; i++)
            {
                if (playersOnly && i > (MAX_PLAYERS-1)) break;
                GameCharacter user = Users[i];
                if (user == null || (pSkipMeh != null && user == pSkipMeh)) continue;
                user.SendPacket(pPacket);
            }
        }

        public bool IsFull()
        {
            return EnteredUsers == MaxUsers;
        }

        public virtual void RemovePlayer(GameCharacter pCharacter, byte pReason)
        {
            //Users[pCharacter.RoomSlotId] = null;

            MiniRoomPacket.ShowLeave(this, pCharacter, pReason);
            Users[pCharacter.RoomSlotId] = null;
            pCharacter.Room = null;
            pCharacter.RoomSlotId = 0;
            EnteredUsers--;


            if (EnteredUsers == 0)
            {
                this.Close(0);
            }
        }

        public void RemovePlayerFromShop(GameCharacter pCharacter)
        {
            MiniRoomBase mrb = pCharacter.Room;

            if (pCharacter == Users[0])
            {
                // Kick all players
                for (int i = 0; i < EnteredUsers; i++)
                {
                    if (pCharacter != Users[i])
                    {
                        PlayerShopPackets.CloseShop(Users[i], MiniRoomBaseError.Closed);
                        EnteredUsers--;
                        Users[i].Room = null;
                        Users[i].RoomSlotId = 0;
                    }

                }

                PlayerShop ps = PlayerShops[mrb.ID];
                ps.RevertItems(pCharacter);
                MiniRoomBalloonPacket.Remove(pCharacter);
                PlayerShops.Remove(mrb.ID);
                pCharacter.Field.PlayerShops.Remove(mrb.ID);
                pCharacter.Room = null;
                pCharacter.RoomSlotId = 0;
            }

            else
            {
                PlayerShopPackets.RemovePlayer(pCharacter, mrb);
                EnteredUsers--;
                Users[pCharacter.RoomSlotId] = null;
                pCharacter.Room = null;
                pCharacter.RoomSlotId = 0;
            }
        }

        public virtual void AddPlayer(GameCharacter pCharacter)
        {
            _transaction += " " + pCharacter.Name + " (" + pCharacter.ID + ")";
            EnteredUsers++;
            pCharacter.RoomSlotId = GetEmptySlot();
            Users[pCharacter.RoomSlotId] = pCharacter;
        }

        public bool CheckPassword(string pPass)
        {
            return Password.Equals(pPass);
        }

        public virtual void EncodeLeave(GameCharacter pCharacter, Packet pPacket) { }

        public virtual void EncodeEnter(GameCharacter pCharacter, Packet pPacket) { }

        public virtual void EncodeEnterResult(GameCharacter pCharacter, Packet pPacket) { }

        public virtual void OnPacket(GameCharacter pCharacter, byte pOpcode, Packet pPacket) { }

        public virtual void AddItemToShop(GameCharacter pCharacter, PlayerShopItem Item) { }

        public static MiniRoomBase CreateRoom(GameCharacter pOwner, MiniRoomType pType, Packet pPacket, bool pTournament, int pRound)
        {
            switch ((MiniRoomType)pType)
            {
                case MiniRoomType.Trade:
                    {
                        Trade trade = new Trade(pOwner);
                        trade.AddPlayer(pOwner);
                        return trade;
                    }

                case MiniRoomType.Omok:
                    {
                        Omok omok = new Omok(pOwner)
                        {
                            Title = pPacket.ReadString(),
                            Private = pPacket.ReadBool()
                        };
                        if (omok.Private == true)
                        {
                            omok.Password = pPacket.ReadString();
                        }

                        omok.PieceType = pPacket.ReadByte();
                        omok.AddOwner(pOwner);
                        omok.mWinnerIndex = 1;
                        Omoks.Add(omok.ID, omok);
                        return omok;
                    }

                case MiniRoomType.PersonalShop:
                    {
                        PlayerShop ps = new PlayerShop(pOwner)
                        {
                            Title = pPacket.ReadString(),
                            Private = pPacket.ReadBool()
                        };
                        short x = pPacket.ReadShort(); // might be type of shop (different shops had different outer designs/looks)? unused var. not sure what it's purpose it serves.
                        ps.ObjectID = pPacket.ReadInt();
                        PlayerShops.Add(ps.ID, ps);
                        return ps;
                    }

                default:
                    {
                        return null;
                    }
            }
        }
    }
}