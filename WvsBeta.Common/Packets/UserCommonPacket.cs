using WvsBeta.Common.Sessions;

namespace WvsBeta.Common.Packets
{
    public class UserCommonPacket : Packet
    {
        public UserCommonPacket(ServerMessages msg, int charId) : base(msg)
        {
            WriteInt(charId);
        }
    }
}
