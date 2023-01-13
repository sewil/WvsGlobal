using WvsBeta.Common.Sessions;
using WvsBeta.Login.Objects;

namespace WvsBeta.Login.Packets
{
    internal struct ISPlayerChangeServerPacket
    {
        string hash;
        int characterId;
        byte worldId;
        byte channelId;
        public ISPlayerChangeServerPacket(Player player, int characterId)
        {
            hash = player.SessionHash;
            this.characterId = characterId;
            worldId = player.World;
            channelId = player.Channel;
        }
        public Packet Encode()
        {
            Packet packet = new Packet(ISClientMessages.PlayerChangeServer);
            packet.WriteString(hash);
            packet.WriteInt(characterId);
            packet.WriteByte(worldId);
            packet.WriteByte(channelId);
            packet.WriteBool(false);
            return packet;
        }
    }
}
