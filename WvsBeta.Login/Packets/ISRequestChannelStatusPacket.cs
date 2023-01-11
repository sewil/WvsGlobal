using WvsBeta.Common.Sessions;

namespace WvsBeta.Login.Packets
{
    internal struct ISRequestChannelStatusPacket
    {
        public readonly string hash;
        public readonly byte worldId;
        public readonly byte channelId;
        public readonly int accountId;
        public ISRequestChannelStatusPacket(Player player, byte channelId)
        {
            this.hash = player.SessionHash;
            this.worldId = player.World;
            this.channelId = channelId;
            this.accountId = player.ID;
        }
        public Packet Encode()
        {
            Packet packet = new Packet(ISClientMessages.PlayerRequestChannelStatus);
            packet.WriteString(hash);
            packet.WriteByte(worldId);
            packet.WriteByte(channelId);
            packet.WriteInt(accountId);
            return packet;
        }
    }
}
