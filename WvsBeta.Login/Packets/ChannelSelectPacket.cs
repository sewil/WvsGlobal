using MySqlX.XDevAPI;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Login.Packets
{
    internal struct ChannelSelectPacket
    {
        public readonly byte worldId;
        public readonly byte channelId;
        public ChannelSelectPacket(Packet packet)
        {
            worldId = packet.ReadByte();
            channelId = packet.ReadByte();
        }
    }
}
