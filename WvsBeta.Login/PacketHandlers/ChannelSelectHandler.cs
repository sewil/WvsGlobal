using log4net;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Login.PacketHandlers
{
    internal class ChannelSelectHandler
    {
        public ChannelSelectHandler(ClientSession session, ILog log, Packet packet)
        {
            if (log.AssertWarning(session.Player.State != Player.LoginState.ChannelSelect,
                "Tried to select channel while not in channel select.")) return;

            var worldId = packet.ReadByte();
            var channelId = packet.ReadByte();

            if (worldId != session.Player.World ||
                !Server.Instance.GetWorld(session.Player.World, out Center center) ||
                channelId >= center.Channels)
            {
                var p = new Packet(ServerMessages.SELECT_WORLD_RESULT);
                p.WriteByte(8);
                session.SendPacket(p);
                return;
            }

            center.Connection?.RequestCharacterIsChannelOnline(session.Player.SessionHash, session.Player.World, channelId, session.Player.ID);
        }
    }
}
