using WvsBeta.Common.Sessions;

namespace WvsBeta.Login.Packets
{
    internal class Socket17Packet
    {
        public Socket17Packet()
        {

        }
        public Packet Encode()
        {
            var packet = new Packet(ServerMessages.__UNKNOWN_17);
            packet.WriteByte(0);
            packet.WriteInt(0);
            return packet;
        }
    }
}
