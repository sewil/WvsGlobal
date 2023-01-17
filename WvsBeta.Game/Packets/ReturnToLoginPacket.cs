using log4net;
using WvsBeta.Common.Extensions;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Game.Packets
{
    internal class ReturnToLoginPacket
    {
        public ReturnToLoginPacket()
        {
        }
        public Packet Encode()
        {
            var packet = new Packet(ServerMessages.__UNKNOWN_17);
            packet.WriteByte(0);
            packet.WriteInt(0);
            return packet;
        }
        public static void Handle(ClientSession session, Packet packet, ILog log)
        {
            if (log.AssertWarning(session.Player.State == Common.Enums.GameState.LoginScreen,
                            "Tried to leave login while in login")) return;
            var pack = new ReturnToLoginPacket().Encode();
            session.SendPacket(pack);
            session.Player.State = Common.Enums.GameState.LoginScreen;
            session.Loaded = false;
        }
    }
}
