using WvsBeta.Common.Sessions;
using WvsBeta.Game.GameObjects.MiniRoom;

namespace WvsBeta.Game.Handlers.MiniRoom
{
    public class BalloonRoom : MiniRoomBase
    {
        public byte PieceType { get; protected set; }
        public string Title { get; }
        public bool Private { get; }
        public string Password { get; }
        public BalloonRoom(GameCharacter owner, byte maxUsers, MiniRoomType type, string title, string password, bool @private) : base(owner, maxUsers, type)
        {
            Title = title;
            Password = password;
            Private = @private;
        }

        public void SendBalloon(bool remove)
        {
            if (Owner == null) return;
            if (remove) MiniRoomBalloonPacket.Remove(Owner);
            else MiniRoomBalloonPacket.Send(Owner, this);
        }
        public override void Close(bool sendPacket = false, MiniRoomLeaveReason reason = MiniRoomLeaveReason.RoomIsClosed)
        {
            base.Close(sendPacket, reason);
            SendBalloon(true);
        }
        public override void AddPlayer(GameCharacter pCharacter)
        {
            base.AddPlayer(pCharacter);
            SendBalloon(false);
        }

        public override void RemovePlayer(GameCharacter pCharacter, MiniRoomLeaveReason pReason)
        {
            base.RemovePlayer(pCharacter, pReason);
            if (Users.Count != 0) SendBalloon(false);
        }
    }
}
