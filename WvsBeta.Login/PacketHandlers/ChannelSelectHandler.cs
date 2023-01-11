using log4net;
using WvsBeta.Common.Sessions;
using WvsBeta.Login.Packets;

namespace WvsBeta.Login.PacketHandlers
{
    internal class ChannelSelectHandler
    {
        public ChannelSelectHandler(ClientSession session, ILog log, Packet packet)
        {
            if (log.AssertWarning(session.Player.State != Player.LoginState.ChannelSelect,
                "Tried to select channel while not in channel select.")) return;

            var result = new ChannelSelectPacket(packet);

            if (result.worldId != session.Player.World ||
                !Server.Instance.GetWorld(session.Player.World, out Center center) ||
                result.channelId >= center.Channels)
            {
                var p = new Packet(ServerMessages.SELECT_CHANNEL_RESULT);
                p.WriteByte(8);
                session.SendPacket(p);
                return;
            }

            var pack = new ISRequestChannelStatusPacket(session.Player, result.channelId).Encode();
            center.Connection.SendPacket(pack);
        }
    }
}
