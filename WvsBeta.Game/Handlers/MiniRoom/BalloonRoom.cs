using WvsBeta.Game.GameObjects.MiniRoom;

namespace WvsBeta.Game.Handlers.MiniRoom
{
    public class BalloonRoom : MiniRoomBase
    {
        public bool GameStarted { get; protected set; }
        public byte GameBoardType { get; protected set; }
        public string Title { get; }
        public bool Private { get; }
        public string Password { get; }
        public BalloonRoom(GameCharacter owner, byte maxUsers, MiniRoomType type, string title, string password, bool @private) : base(owner, maxUsers, type)
        {
            Title = title;
            Password = password;
            Private = @private;
        }

        public override void OnInitialize()
        {
            base.OnInitialize();
            Field.BalloonRooms.Add(ID, this);
        }

        public void SendBalloon(bool remove)
        {
            if (Owner == null) return;
            if (remove) MiniRoomBalloonPacket.Remove(Owner);
            else MiniRoomBalloonPacket.Send(Owner, this);
        }

        public void Open()
        {
            SendBalloon(false);
        }

        public override void Close(bool sendPacket = true, MiniRoomLeaveReason reason = MiniRoomLeaveReason.Closed)
        {
            SendBalloon(true);
            Field.BalloonRooms.Remove(ID);
            base.Close(sendPacket, reason);
        }
        public override void AddPlayer(GameCharacter pCharacter)
        {
            base.AddPlayer(pCharacter);
            if (Users.Count > 1) SendBalloon(false);
        }

        public override void RemovePlayer(GameCharacter pCharacter, MiniRoomLeaveReason pReason)
        {
            base.RemovePlayer(pCharacter, pReason);
            if (Users.Count != 0) SendBalloon(false);
        }
    }
}
