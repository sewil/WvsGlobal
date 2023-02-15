using WvsBeta.Common.Sessions;
using WvsBeta.Game.Handlers.Guild;

namespace WvsBeta.Game.Packets
{
    static class RemotePacket
    {
        public static void SendCharacterGuildInfo(GameCharacter chr)
        {
            SendCharacterGuildName(chr);
            SendCharacterGuildEmblem(chr);
        }
        public static void SendCharacterGuildName(GameCharacter chr)
        {
            GuildData guild = chr.Guild ?? GuildData.Default;
            var pw = new Packet(ServerMessages.REMOTE_GUILD_NAME);
            pw.WriteInt(chr.ID);
            pw.WriteString(guild.Name);
            chr.Field.SendPacket(pw);
        }
        public static void SendCharacterGuildEmblem(GameCharacter chr)
        {
            GuildData guild = chr.Guild ?? GuildData.Default;
            var pw = new Packet(ServerMessages.REMOTE_GUILD_EMBLEM);
            pw.WriteInt(chr.ID);
            guild.Emblem.EncodeForRemote(pw);
            chr.Field.SendPacket(pw);
        }
    }
}
