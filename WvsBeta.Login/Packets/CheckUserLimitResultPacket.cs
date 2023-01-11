using WvsBeta.Common.Sessions;

namespace WvsBeta.Login.Packets
{
    internal class CheckUserLimitResultPacket : Packet
    {
        public CheckUserLimitResultPacket(WorldWarning warning, WorldMarker marker) : base(ServerMessages.CHECK_USER_LIMIT_RESULT)
        {
            WriteByte((byte)warning);
            WriteByte((byte)marker);
        }
    }
}
