using WvsBeta.Common.Objects;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Login.Packets
{
    internal struct CheckUserLimitPacket
    {
        public readonly World.Warning warning;
        public readonly World.Marker marker;
        public CheckUserLimitPacket(World.Warning warning, World.Marker marker)
        {
            this.warning = warning;
            this.marker = marker;
        }
        public CheckUserLimitPacket(Packet packet)
        {
            warning = (World.Warning)packet.ReadByte();
            marker = (World.Marker)packet.ReadByte();
        }
        public Packet Encode()
        {
            Packet packet = new Packet(ServerMessages.CHECK_USER_LIMIT_RESULT);
            packet.WriteByte((byte)warning);
            packet.WriteByte((byte)marker);
            return packet;
        }
    }
}
