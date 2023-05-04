using WvsBeta.Common.Sessions;
using WvsBeta.Game.Handlers.MiniRooms;

namespace WvsBeta.Game.GameObjects.MiniRooms
{
    public static class MiniGamePacket
    {
        public static void SendEnterResultError(GameCharacter chr, MiniRoomEnterError error)
        {
            Packet pw = new Packet(ServerMessages.MINI_ROOM_BASE);
            pw.WriteByte((byte)MiniRoomOpServer.EnterResult);
            pw.WriteByte(0);
            pw.WriteByte((byte)error);
            chr.SendPacket(pw);
        }

        public static void Ready(GameCharacter chr, MiniRoom mrb)
        {
            Packet pw = new Packet(ServerMessages.MINI_ROOM_BASE);
            pw.WriteByte((byte)MiniRoomOpServer.GameReady);
            mrb.BroadcastPacket(pw);
        }

        public static void UnReady(GameCharacter chr, MiniRoom mrb)
        {
            Packet pw = new Packet(ServerMessages.MINI_ROOM_BASE);
            pw.WriteByte((byte)MiniRoomOpServer.GameUnready);
            mrb.BroadcastPacket(pw);
        }

        public static void OmokMessage(GameCharacter chr, MiniRoom mrb, byte Type)
        {
            //You have double -3's o.o
            Packet pw = new Packet(ServerMessages.MINI_ROOM_BASE);
            pw.WriteByte(0x27);
            pw.WriteByte(Type); //0: cant put it there 0x29: you have double -3's
            chr.SendPacket(pw);
        }

        public static void RequestTie(GameCharacter chr, MiniRoom mrb)
        {
            Packet pw = new Packet(ServerMessages.MINI_ROOM_BASE);
            pw.WriteByte((byte)MiniRoomOpServer.GameRequestTie);
            mrb.BroadcastPacket(pw, chr);
        }

        public static void RequestTieDeny(GameCharacter chr, MiniRoom mrb)
        {
            //Your opononent denied your request for a tie
            Packet pw = new Packet(ServerMessages.MINI_ROOM_BASE);
            pw.WriteByte((byte)MiniRoomOpServer.GameRequestTieDeny);
            mrb.BroadcastPacket(pw, chr);
        }

        public static void RequestHandicap(GameCharacter chr, MiniRoom mrb)
        {
            //Your opponent has requested for a handicap. Will you accept it? 
            Packet pw = new Packet(ServerMessages.MINI_ROOM_BASE);
            pw.WriteByte((byte)MiniRoomOpServer.GameRequestHandicap);
            mrb.BroadcastPacket(pw, chr);
        }

        public static void RequestHandicapResult(GameCharacter chr, MiniRoom mrb, bool result, byte countBack)
        {
            //Your opponent denied your request for a handicap
            Packet pw = new Packet(ServerMessages.MINI_ROOM_BASE);
            pw.WriteByte((byte)MiniRoomOpServer.GameRequestHandicapResult);
            pw.WriteBool(result); //deny or not ?
            pw.WriteByte(countBack);
            pw.WriteByte(chr.RoomSlotId == 0 ? (byte)1 : (byte)0);
            mrb.BroadcastPacket(pw, null);
        }

        public static void MoveOmokPiece(GameCharacter chr, MiniRoom mrb, int X, int Y, byte Piece)
        {
            //decodebuffer (8 bytes.. obviously 2 ints)
            Packet pw = new Packet(ServerMessages.MINI_ROOM_BASE);
            pw.WriteByte((byte)MiniRoomOpServer.OmokMovePiece); //?
            pw.WriteInt(X);
            pw.WriteInt(Y);
            //Type
            pw.WriteByte(Piece); //Works as piece too 
            mrb.BroadcastPacket(pw);
        }

        public static void EndGame(MiniGameRoom mrb, GameResult result, GameCharacter winner)
        {
            GameCharacter visitor = mrb.GetVisitor();
            if (visitor == null || mrb.Owner == null)
            {
                Program.MainForm.LogAppend($"Tried to end game with null visitor or owner {result}, {mrb.ID}, {winner.ID}, {mrb.Owner?.ID}, {visitor?.ID}");
                return;
            }
            Packet pw = new Packet(ServerMessages.MINI_ROOM_BASE);
            pw.WriteByte((byte)MiniRoomOpServer.GameEnd);
            pw.WriteByte((byte)result);
            if (result != GameResult.Tie) pw.WriteByte(winner.RoomSlotId);
            mrb.EncodeGameStats(mrb.Owner, pw);
            mrb.EncodeGameStats(visitor, pw);
            mrb.BroadcastPacket(pw);
        }

        public static void MatchCardsTurn(MatchCard mc, byte slot)
        {
            var pw = new Packet(ServerMessages.MINI_ROOM_BASE);
            pw.WriteByte((byte)MiniRoomOpServer.MatchCardsTurn);
            pw.WriteByte(slot);
            mc.BroadcastPacket(pw);
        }
        public static void FlipCard(GameCharacter chr, MatchCard mc, bool first, byte idx, byte firstIdx, bool match)
        {
            // /packet C2 2A 00 01 00 00
            var pw = new Packet(ServerMessages.MINI_ROOM_BASE);
            pw.WriteByte((byte)MiniRoomOpServer.MatchCardsFlipCard);
            pw.WriteBool(first);
            pw.WriteByte(idx);
            if (!first)
            {
                pw.WriteByte(firstIdx);
                byte type = (byte)(chr.RoomSlotId + (match ? 2 : 0));
                pw.WriteByte(type); // 2 = match
            }
            mc.BroadcastPacket(pw, first ? chr : null);
        }
    }
}
