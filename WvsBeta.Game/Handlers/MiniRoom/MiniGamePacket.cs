using System;
using WvsBeta.Common.Character;
using WvsBeta.Common.Sessions;
using WvsBeta.Game.Handlers.MiniRoom;

namespace WvsBeta.Game.GameObjects.MiniRoom
{
    public static class MiniGamePacket
    {
        public static void ErrorMessage(GameCharacter chr, MiniGameError error)
        {
            Packet pw = new Packet(ServerMessages.MINI_ROOM_BASE);
            pw.WriteByte(5);
            pw.WriteByte(0);
            pw.WriteByte((byte)error);
            chr.SendPacket(pw);
        }

        public static void Ready(GameCharacter chr, MiniRoomBase mrb)
        {
            Packet pw = new Packet(ServerMessages.MINI_ROOM_BASE);
            pw.WriteByte(0x20);
            mrb.BroadcastPacket(pw);
        }

        public static void UnReady(GameCharacter chr, MiniRoomBase mrb)
        {
            Packet pw = new Packet(ServerMessages.MINI_ROOM_BASE);
            pw.WriteByte(0x21);
            mrb.BroadcastPacket(pw);
        }

        public static void OmokMessage(GameCharacter chr, MiniRoomBase mrb, byte Type)
        {
            //You have double -3's o.o
            Packet pw = new Packet(ServerMessages.MINI_ROOM_BASE);
            pw.WriteByte(0x27);
            pw.WriteByte(Type); //0: cant put it there 0x29: you have double -3's
            chr.SendPacket(pw);
        }

        public static void RequestTie(GameCharacter chr, MiniRoomBase mrb)
        {
            Packet pw = new Packet(ServerMessages.MINI_ROOM_BASE);
            pw.WriteByte(0x18);
            mrb.BroadcastPacket(pw, chr);
        }

        public static void RequestTieDeny(GameCharacter chr, MiniRoomBase mrb)
        {
            //Your opononent denied your request for a tie
            Packet pw = new Packet(ServerMessages.MINI_ROOM_BASE);
            pw.WriteByte(0x19);
            mrb.BroadcastPacket(pw, chr);
        }

        public static void RequestHandicap(GameCharacter chr, MiniRoomBase mrb)
        {
            //Your opponent has requested for a handicap. Will you accept it? 
            Packet pw = new Packet(ServerMessages.MINI_ROOM_BASE);
            pw.WriteByte(0x1C);
            mrb.BroadcastPacket(pw, chr);
        }

        public static void RequestHandicapResult(GameCharacter chr, MiniRoomBase mrb, bool result, byte countBack)
        {
            //Your opponent denied your request for a handicap
            Packet pw = new Packet(ServerMessages.MINI_ROOM_BASE);
            pw.WriteByte(0x1D);
            pw.WriteBool(result); //deny or not ?
            pw.WriteByte(countBack);
            pw.WriteByte(chr.RoomSlotId == 0 ? (byte)1 : (byte)0);
            mrb.BroadcastPacket(pw, null);
        }

        public static void MoveOmokPiece(GameCharacter chr, MiniRoomBase mrb, int X, int Y, byte Piece)
        {
            //decodebuffer (8 bytes.. obviously 2 ints)
            Packet pw = new Packet(ServerMessages.MINI_ROOM_BASE);
            pw.WriteByte(0x26); //?
            pw.WriteInt(X);
            pw.WriteInt(Y);
            //Type
            pw.WriteByte(Piece); //Works as piece too 
            mrb.BroadcastPacket(pw);
        }

        public static void RemoveOmokPieceTest(MiniRoomBase mrb, int X, int Y, byte Piece)
        {
            Packet pw = new Packet(ServerMessages.MINI_ROOM_BASE);
            pw.WriteByte(0x26);
            pw.WriteInt(X);
            pw.WriteInt(Y);
            pw.WriteSByte(-1);
            mrb.BroadcastPacket(pw);
        }

        public static void Expel(MiniRoomBase mrb, byte pWho)
        {
            Packet pw = new Packet(ServerMessages.MINI_ROOM_BASE);
            pw.WriteByte(0x25);
            pw.WriteByte(pWho);
            mrb.BroadcastPacket(pw);
        }

        public static void Start(GameCharacter chr, MiniRoomBase mrb)
        {
            //Timer is (null) then client stops responding ;-;
            Packet pw = new Packet(ServerMessages.MINI_ROOM_BASE);
            pw.WriteByte(0x23);
            pw.WriteByte(mrb.mWinnerIndex); //0 Would let slot 1, 1 would let slot 0
            mrb.BroadcastPacket(pw);
        }
        public static void Start2(GameCharacter chr, MiniRoomBase mrb)
        {
            //Timer is (null) then client stops responding ;-;
            Packet pw = new Packet(ServerMessages.MINI_ROOM_BASE);
            pw.WriteByte(0x23);
            pw.WriteByte(1); //piece id 
            chr.SendPacket(pw);
        }

        public static void UpdateGame(MiniGameRoom mrb, GameResult result)
        {
            Packet pw = new Packet(ServerMessages.MINI_ROOM_BASE);
            pw.WriteByte((byte)MiniRoomOpServer.MiniGameUpdate);
            pw.WriteByte((byte)result);

            foreach (var user in mrb.Users)
            {
                mrb.EncodeGameStats(user.Value, pw);
            }

            pw.WriteLong(0);
            mrb.BroadcastPacket(pw);
        }
    }
}
