using WvsBeta.Common.Sessions;

namespace WvsBeta.Game.Handlers.GuildQuest
{
    public class GuildQuestRegistration
    {
        public readonly byte channelId;
        public readonly int guildId;
        public readonly int master;
        public readonly int[] members;
        public GuildQuestRegistration(byte channelId, int guildId, int master, int[] members)
        {
            this.channelId = channelId;
            this.guildId = guildId;
            this.master = master;
            this.members = members;
        }
        public void Encode(Packet pw)
        {
            pw.WriteByte(channelId);
            pw.WriteInt(guildId);
            pw.WriteInt(master);
            pw.WriteShort((short)members.Length);
            foreach (int member in members)
            {
                pw.WriteInt(member);
            }
        }
        public static GuildQuestRegistration Decode(Packet pr)
        {
            byte channelId = pr.ReadByte();
            int guildId = pr.ReadInt();
            int master = pr.ReadInt();
            var members = new int[pr.ReadShort()];
            for (int i = 0; i < members.Length; i++)
            {
                members[i] = pr.ReadInt();
            }
            return new GuildQuestRegistration(channelId, guildId, master, members);
        }
    }
}
