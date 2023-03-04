using System;
using WvsBeta.Common.Character;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Game.GameObjects.MiniRoom
{
    public static class MiniGamePacket
    {
        private static void EncodeGameStats(MiniRoomBase mrb, byte slot, Packet pw)
        {
            pw.WriteByte(slot);
            //GW_Minigamerecord_Decode (20 bytes)
            mrb.Users[slot].GameStats.AllStats[(MiniGameType)mrb.Type].Encode(pw);
        }
        public static void ShowWindow(GameCharacter pOwner, MiniRoomBase mrb, byte OmokType)
        {
            Packet pw = new Packet(ServerMessages.MINI_ROOM_BASE);
            pw.WriteByte(5);
            pw.WriteByte((byte)MiniRoomType.Omok);
            pw.WriteByte(mrb.MaxUsers);
            pw.WriteBool(mrb.Users[0] == pOwner ? false : true);
            for (byte i = 0; i < 2; i++)
            {
                GameCharacter pUser = pOwner.Room.Users[i];
                if (pUser != null)
                {
                    pw.WriteByte(i);
                    new AvatarLook(pUser).Encode(pw);
                    pw.WriteString(pUser.Name);
                }
            }
            pw.WriteByte(0xFF);
            //End of Regular Enter base
            //Start of Omok Enter Base

            for (byte slot = 0; slot < Math.Min((byte)2, mrb.EnteredUsers); slot++)
            {
                EncodeGameStats(mrb, slot, pw);
            }

            pw.WriteByte(0xFF);
            //Rest of packet
            pw.WriteString(mrb.Title);
            pw.WriteByte(OmokType); //Pieces type

            //Continue, no idea what this part is.
            pw.WriteByte(0);
            pw.WriteByte(0);
            pw.WriteLong(0);
            pOwner.SendPacket(pw);
        }

        public static void AddVisitor(GameCharacter chr, MiniRoomBase mrb)
        {
            Packet pw = new Packet(ServerMessages.MINI_ROOM_BASE);
            pw.WriteByte(4);
            pw.WriteByte(chr.RoomSlotId);
            new AvatarLook(chr).Encode(pw);
            pw.WriteString(chr.Name);
            EncodeGameStats(mrb, 1, pw);
            mrb.BroadcastPacket(pw, chr);
        }

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

        public static void UpdateGame(MiniRoomBase mrb, GameResult result)
        {
            Packet pw = new Packet(ServerMessages.MINI_ROOM_BASE);
            pw.WriteByte(0x24);
            pw.WriteByte((byte)result);

            for (byte slot = 0; slot < Math.Min((byte)2, mrb.EnteredUsers); slot++)
            {
                EncodeGameStats(mrb, slot, pw);
            }

            pw.WriteLong(0);
            mrb.BroadcastPacket(pw);
        }
    }
}
