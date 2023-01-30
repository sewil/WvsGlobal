using System;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Game.GameObjects.MiniRoom
{
    class MiniRoomBalloonPacket : Packet
    {
        private readonly GameCharacter chr;
        private MiniRoomBalloonPacket(GameCharacter chr) : base(ServerMessages.MINI_ROOM_BALLOON)
        {
            if (chr == null)
            {
                throw new ArgumentException("Cannot remove mini room balloon with null chr");
            }
            this.chr = chr;
            WriteInt(chr.ID);
            WriteByte(0); // Remove balloon
        }
        public MiniRoomBalloonPacket(MiniRoomBase room, GameCharacter chr) : base(ServerMessages.MINI_ROOM_BALLOON)
        {
            if (room == null)
            {
                throw new ArgumentException("Cannot add mini room balloon with null room");
            }
            if (chr == null)
            {
                throw new ArgumentException("Cannot add mini room balloon with null chr");
            }
            this.chr = chr;
            WriteInt(chr.ID);
            WriteByte((byte)room.Type); //no game : 0,  omok : 1,  card game : 2, shop : 4, 
            if (room.Type == MiniRoomType.None)
            {
                return;
            }
            WriteInt(room.ID); //game object  (Make match cards done with characterid, and omok with objectid)
            WriteString(room.Title);
            WriteBool(room.Private); //0 : public 1 : private
            WriteByte((byte)room.CardType);
            WriteByte(1); // Current slots (1-4)
            WriteByte(2); // Max slots (1-4)
            WriteBool(room.GameStarted);
        }
        public MiniRoomBalloonPacket(MiniRoomBase room) : this(room, room?.Users[0])
        {
        }
        public void Send()
        {
            chr.Field.SendPacket(chr, this);
        }
        public static MiniRoomBalloonPacket Remove(GameCharacter chr)
        {
            return new MiniRoomBalloonPacket(chr);
        }
    }
}
