using System.Collections.Generic;
using System.Linq;
using WvsBeta.Common;
using WvsBeta.Common.Characters;
using WvsBeta.Common.Interfaces;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Game.GameObjects.MiniRooms
{
    public abstract class MiniRoom : IInitialize
    {
        protected string _transaction = Cryptos.GetNewSessionHash();
        public string TransactionID => _transaction;

        public static Dictionary<int, MiniRoom> MiniRooms = new Dictionary<int, MiniRoom>();

        public int ID { get; }
        public byte MaxUsers { get; }
        public IDictionary<byte, GameCharacter> Users { get; }
        public bool Opened { get; protected set; }
        public bool CloseRequest { get; protected set; }
        public bool Tournament { get; protected set; }
        public int RoundID { get; protected set; }
        public Pos mHost { get; protected set; }
        public MiniRoomType Type { get; private set; }
        public byte mWinnerIndex { get; set; }
        public GameCharacter Owner { get; private set; }
        public Map Field { get; }

        protected MiniRoom(GameCharacter owner, byte maxUsers, MiniRoomType type)
        {
            Owner = owner;
            Field = owner.Field;
            ID = Server.Instance.MiniRoomIDs.NextValue();
            MiniRooms.Add(ID, this);
            MaxUsers = maxUsers;
            Users = new Dictionary<byte, GameCharacter>();
            Opened = false;
            CloseRequest = false;
            Tournament = false;
            Type = type;
        }
        public virtual void OnInitialize()
        {
            AddPlayer(Owner);
        }

        public virtual void Close(bool sendPacket = true, MiniRoomLeaveReason reason = MiniRoomLeaveReason.Closed)
        {
            MiniRooms.Remove(ID);
            Owner = null;
            foreach (var user in Users.Select(i => i.Value))
            {
                if (sendPacket) MiniRoomPacket.SendLeaveRoom(user, reason);
                user.RoomSlotId = 0;
                user.Room = null;
            }
            Users.Clear();
        }

        public byte GetEmptySlot()
        {
            for (byte slot = 0; slot < MaxUsers; slot++)
            {
                if (!Users.ContainsKey(slot)) return slot;
            }

            return 0xFF;
        }

        public GameCharacter GetVisitor()
        {
            Users.TryGetValue(1, out GameCharacter visitor);
            return visitor;
        }

        public GameCharacter GetOtherUser(byte currentIdx)
        {
            byte otherIdx = (byte)(currentIdx == 0 ? 1 : 0);
            Users.TryGetValue(otherIdx, out GameCharacter user);
            return user;
        }

        public void BroadcastPacket(Packet pPacket, GameCharacter pSkipMeh = null)
        {
            foreach (var user in Users)
            {
                if ((pSkipMeh != null && user.Value == pSkipMeh)) continue;
                user.Value.SendPacket(pPacket);
            }
        }

        public bool IsFull()
        {
            return Users.Count == MaxUsers;
        }

        public virtual void RemovePlayer(GameCharacter pCharacter, MiniRoomLeaveReason pReason)
        {
            MiniRoomPacket.SendLeaveRoom(this, pCharacter, pReason);
            Users.Remove(pCharacter.RoomSlotId);
            pCharacter.Room = null;
            pCharacter.RoomSlotId = 0;

            if (Users.Count == 0) Close(false);
        }

        public virtual void AddPlayer(GameCharacter pCharacter)
        {
            _transaction += " " + pCharacter.Name + " (" + pCharacter.ID + ")";
            pCharacter.RoomSlotId = GetEmptySlot();
            Users.Add(pCharacter.RoomSlotId, pCharacter);
            pCharacter.Room = this;
            MiniRoomPacket.SendEnter(this, pCharacter);
            MiniRoomPacket.SendEnterResult(this, pCharacter);
        }

        public virtual void EncodeLeave(GameCharacter pCharacter, Packet pPacket) { }

        public virtual void EncodeEnter(GameCharacter pCharacter, Packet pw)
        {
            pw.WriteByte((byte)MiniRoomOpClient.Enter);
            pw.WriteByte(pCharacter.RoomSlotId);
            new AvatarLook(pCharacter, false).Encode(pw);
            pw.WriteString(pCharacter.Name);
        }

        public virtual void EncodeEnterResult(GameCharacter pCharacter, Packet pw)
        {
            pw.WriteByte((byte)MiniRoomOpServer.EnterResult);
            pw.WriteByte((byte)Type);
            pw.WriteByte(MaxUsers);
            pw.WriteBool(Owner != pCharacter);

            foreach (var user in Users.Select(i => i.Value))
            {
                pw.WriteByte(user.RoomSlotId);
                new AvatarLook(user, false).Encode(pw);
                pw.WriteString(user.Name);
            }

            pw.WriteByte(0xFF);
        }

        public virtual void OnPacket(GameCharacter pCharacter, MiniRoomOpClient pOpcode, Packet pPacket) { }
    }
}
