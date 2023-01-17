using WvsBeta.Common.Sessions;

namespace WvsBeta.Login.Packets
{
    internal class Socket23Packet
    {
        public Packet Encode()
        {
            var packet = new Packet(ServerMessages.__UNKNOWN_23);
            short short0 = 0;
            packet.WriteShort(short0);
            if (short0 > 0)
            {
                byte[] bytes = new byte[22 * short0];
                packet.WriteBytes(bytes);
            }
            return packet;
            // receive COutPacket_0(19,0);
        }
    }
}
