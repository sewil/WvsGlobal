using WvsBeta.Common.Enums;
using WvsBeta.Common.Objects;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Login.Packets
{
    internal struct CheckUserLimitPacket
    {
        public readonly WorldWarning warning;
        public readonly WorldMarker marker;
        public CheckUserLimitPacket(WorldWarning warning, WorldMarker marker)
        {
            this.warning = warning;
            this.marker = marker;
        }
        public CheckUserLimitPacket(Packet packet)
        {
            warning = (WorldWarning)packet.ReadByte();
            marker = (WorldMarker)packet.ReadByte();
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
