using WvsBeta.Common.Sessions;

namespace WvsBeta.Login.Packets
{
    internal struct CharacterSelectPacket
    {
        public readonly int charid;
        public CharacterSelectPacket(Packet packet)
        {
            charid = packet.ReadInt();
        }
        public Packet Encode() 
        {
            var packet = new Packet(ServerMessages.SELECT_CHARACTER_RESULT);
            packet.WriteByte(6); // Connection failed due to system error
            packet.WriteByte(0);
            return packet;
        }
    }
}
