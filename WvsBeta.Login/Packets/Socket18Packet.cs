using WvsBeta.Common.Sessions;

namespace WvsBeta.Login.Packets
{
    internal class Socket18Packet
    {
        public Packet Encode()
        {
            var packet = new Packet(ServerMessages.__UNKNOWN_18);
            byte status = 0;
            packet.WriteByte(status);
            if (status == 0)
            {
                short short0 = 0;
                packet.WriteShort(short0);
                for (int i = 0; i < short0; i++)
                {
                    packet.WriteInt(0);
                    packet.WriteInt(0);
                }
            }
            else if (status < 3)
            {
                packet.WriteInt(0);
            }
            else
            {
                packet.WriteLong(0);
                packet.WriteLong(0);
            }
            return packet;
        }
    }
}
