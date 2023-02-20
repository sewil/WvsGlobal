using System;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Game.GameObjects.MiniRoom
{
    public static class MiniRoomBalloonPacket
    {
        public static void Remove(GameCharacter chr)
        {
            if (chr == null)
            {
                throw new ArgumentException("Cannot remove mini room balloon with null chr");
            }
            var pw = new Packet(ServerMessages.MINI_ROOM_BALLOON);
            pw.WriteInt(chr.ID);
            pw.WriteByte(0);
            chr.Field.SendPacket(pw);
        }
        public static void Send(GameCharacter chr, MiniRoomBase room)
        {
            if (room == null)
            {
                throw new ArgumentException("Cannot add mini room balloon with null room");
            }
            if (chr == null)
            {
                throw new ArgumentException("Cannot add mini room balloon with null chr");
            }
            var pw = new Packet(ServerMessages.MINI_ROOM_BALLOON);
            pw.WriteInt(chr.ID);
            pw.WriteByte((byte)room.Type); //no game : 0,  omok : 1,  card game : 2, shop : 4, 
            if (room.Type == MiniRoomType.None)
            {
                return;
            }
            pw.WriteInt(room.ID); //game object  (Make match cards done with characterid, and omok with objectid)
            pw.WriteString(room.Title);
            pw.WriteBool(room.Private); //0 : public 1 : private
            pw.WriteByte((byte)room.CardType);
            pw.WriteByte(room.EnteredUsers); // Current slots (1-4)
            pw.WriteByte(room.MaxUsers); // Max slots (1-4)
            pw.WriteBool(room.GameStarted);
            chr.Field.SendPacket(chr, pw);
        }
    }
}
