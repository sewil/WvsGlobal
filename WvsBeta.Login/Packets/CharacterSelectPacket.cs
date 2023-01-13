using System;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Login.Packets
{
    internal struct CharacterSelectPacket
    {
        public enum Status
        {
            Success = 0,
            SystemError = 6,
        }
        public readonly int characterId;
        string characterName;
        byte[] ip;
        ushort port;
        byte bit;
        public CharacterSelectPacket(Packet packet)
        {
            characterId = packet.ReadInt();
            characterName = null;
            ip = null;
            port = 0;
            bit = 0;
        }
        public CharacterSelectPacket(byte[] ip, ushort port, int characterId, string characterName, byte bit)
        {
            this.ip = ip;
            this.port = port;
            this.characterId = characterId;
            this.characterName = characterName;
            this.bit = bit;
        }
        public Packet Encode(Status status)
        {
            var packet = new Packet(ServerMessages.SELECT_CHARACTER_RESULT);
            packet.WriteByte((byte)status);
            packet.WriteByte(0);

            if (status != Status.Success)
            {
                return packet;
            }

            packet.WriteBytes(ip); // Always 4
            packet.WriteUShort(port);
            packet.WriteInt(characterId);
            packet.WriteByte(bit);
            packet.WriteInt(0);
            return packet;
        }
    }
}
