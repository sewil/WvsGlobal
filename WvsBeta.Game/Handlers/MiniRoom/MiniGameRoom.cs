using WvsBeta.Common.Sessions;
using WvsBeta.Game.GameObjects.MiniRoom;

namespace WvsBeta.Game.Handlers.MiniRoom
{
    public class MiniGameRoom : BalloonRoom
    {
        public MiniGameRoom(GameCharacter owner, byte maxUsers, MiniRoomType type, string title, string password, bool @private, byte pieceType) : base(owner, maxUsers, type, title, password, @private)
        {
            PieceType = pieceType;
        }

        public void EncodeGameStats(GameCharacter user, Packet pw)
        {
            pw.WriteByte(user.RoomSlotId);
            //GW_Minigamerecord_Decode (20 bytes)
            user.GameStats.AllStats[(MiniGameType)Type].Encode(pw);
        }

        public override void EncodeEnter(GameCharacter pCharacter, Packet pw)
        {
            base.EncodeEnter(pCharacter, pw);
            EncodeGameStats(pCharacter, pw);
        }
        public override void EncodeEnterResult(GameCharacter pCharacter, Packet pw)
        {
            base.EncodeEnterResult(pCharacter, pw);

            foreach (var user in Users)
            {
                EncodeGameStats(user.Value, pw);
            }

            pw.WriteByte(0xFF);
            //Rest of packet
            pw.WriteString(Title);
            pw.WriteByte(PieceType); //Pieces type

            //Continue, no idea what this part is.
            pw.WriteByte(0);
            pw.WriteByte(0);
            pw.WriteLong(0);
        }
    }
}
