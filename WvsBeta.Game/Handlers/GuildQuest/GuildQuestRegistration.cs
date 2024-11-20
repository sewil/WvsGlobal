using WvsBeta.Common;
using WvsBeta.Common.Extensions;
using WvsBeta.Common.Sessions;
using WvsBeta.Game.Handlers.Guild;

namespace WvsBeta.Game.Handlers.GuildQuest
{
    public class GuildQuestRegistration
    {
        public byte ChannelID { get; }
        public int GuildID { get; }
        public GuildData Guild => Server.Instance.Guilds.GetValue(GuildID);
        public int QueueIndex => Server.Instance.GuildQuestRegistrations.GetValue(ChannelID)?.FindIndex(i => i.GuildID == GuildID) ?? -1;
        public int Leader { get; }
        public GuildQuestRegistration(byte channelId, int guildId, int leader)
        {
            ChannelID = channelId;
            GuildID = guildId;
            Leader = leader;
        }
        public void Encode(Packet pw)
        {
            pw.WriteByte(ChannelID);
            pw.WriteInt(GuildID);
            pw.WriteInt(Leader);
        }
        public static GuildQuestRegistration Decode(Packet pr)
        {
            byte channelId = pr.ReadByte();
            int guildId = pr.ReadInt();
            int leader = pr.ReadInt();
            return new GuildQuestRegistration(channelId, guildId, leader);
        }
    }
}
