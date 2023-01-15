using WvsBeta.Common.Sessions;
using WvsBeta.Login.Objects;

namespace WvsBeta.Login.Packets
{
    internal class ISRequestChannelStatusPacket : Packet
    {
        public ISRequestChannelStatusPacket(Player player, byte channelId) : base(ISClientMessages.PlayerRequestChannelStatus)
        {
            WriteString(player.SessionHash);
            WriteByte(player.World);
            WriteByte(channelId);
            WriteInt(player.ID);
        }
    }
}
