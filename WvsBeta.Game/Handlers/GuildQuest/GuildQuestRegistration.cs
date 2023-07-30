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
        public int[] Members { get; }
        public GuildQuestRegistration(byte channelId, int guildId, int leader, int[] members)
        {
            ChannelID = channelId;
            GuildID = guildId;
            Leader = leader;
            Members = members;
        }
        public void Encode(Packet pw)
        {
            pw.WriteByte(ChannelID);
            pw.WriteInt(GuildID);
            pw.WriteInt(Leader);
            pw.WriteInt(Members.Length);
            Members.ForEach(m => pw.WriteInt(m));
        }
        public static GuildQuestRegistration Decode(Packet pr)
        {
            byte channelId = pr.ReadByte();
            int guildId = pr.ReadInt();
            int leader = pr.ReadInt();
            int[] members = new int[pr.ReadInt()];
            for (int i = 0; i < members.Length; i++) members[i] = pr.ReadInt();
            return new GuildQuestRegistration(channelId, guildId, leader, members);
        }
    }
}
