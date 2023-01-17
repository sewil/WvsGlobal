using log4net;
using WvsBeta.Common.Character;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Extensions;
using WvsBeta.Common.Objects;
using WvsBeta.Common.Sessions;
using WvsBeta.Login.Packets;

namespace WvsBeta.Login.PacketHandlers
{
    internal class ChannelSelectHandler
    {
        public static void HandleClientRequest(ClientSession session, ILog log, Packet packet)
        {
            if (log.AssertWarning(session.Player.State != GameState.ChannelSelect,
                "Tried to select channel while not in channel select.")) return;

            var worldId = packet.ReadByte();
            var channelId = packet.ReadByte();

            if (worldId != session.Player.World ||
                !Server.Instance.GetWorld(session.Player.World, out Center center) ||
                channelId >= center.Channels)
            {
                var p = new Packet(ServerMessages.SELECT_CHANNEL_RESULT);
                p.WriteByte(8);
                session.SendPacket(p);
                return;
            }

            var pack = new ISRequestChannelStatusPacket(session.Player, channelId);
            center.Connection.SendPacket(pack);
        }
        public static void HandleCenterResult(ClientSession session, Packet packet, Player player)
        {
            player.Channel = packet.ReadByte();
            var characters = packet.ReadByte();
            var avatars = new AvatarData[characters];
            var rankings = new WorldRanking[characters];
            for (int i = 0; i < characters; i++)
            {
                avatars[i] = new AvatarData(packet);
                var hasRanking = packet.ReadBool();
                if (hasRanking)
                {
                    rankings[i] = new WorldRanking(
                        packet.ReadInt(),
                        packet.ReadInt(),
                        packet.ReadInt(),
                        packet.ReadInt()
                    );
                }
            }

            var pw = new ChannelSelectResultPacket(characters, player, avatars, rankings);
            session.SendPacket(pw);
            player.State = GameState.CharacterSelect;
        }
    }
}
