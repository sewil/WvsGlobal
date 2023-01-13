using WvsBeta.Common.Sessions;

namespace WvsBeta.Login.Packets
{
    internal class Socket19Packet
    {
        public Packet Encode()
        {
            var packet = new Packet(ServerMessages.__UNKNOWN_19);
            packet.WriteInt(0);
            packet.WriteByte(0);
            return packet;
        }
    }
}
