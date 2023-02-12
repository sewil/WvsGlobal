using System.Collections.Generic;
using System.Linq;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Game.Handlers.Guild
{
    public enum GuildRank : int
    {
        Member1 = 1,
        Member2 = 2,
        Member3 = 3,
        JrMaster = 4,
        Master = 5,
    }
    public class GuildMember
    {
        public int ID { get; set; }
        public GuildRank Rank { get; set; }
    }
    public class GuildData
    {
        public int ID { get; private set; }
        public string Name { get; private set; }
        public int Capacity { get; private set; }
        public string[] RankNames { get; private set; }
        public GuildMember[] Members { get; private set; }
        public static Dictionary<int, GuildData> Guilds { get; private set; } = new Dictionary<int, GuildData>();

        public GuildData(int id, string name, int capacity, string[] rankNames, GuildMember[] members)
        {
            ID = id;
            Name = name;
            Capacity = capacity;
            Members = members;
            RankNames = rankNames;
        }

        public void Encode(Packet pw)
        {
            pw.WriteInt(ID);
            pw.WriteString(Name);
            foreach (var rankName in RankNames)
            {
                pw.WriteString(rankName);
            }
            var chars = Members.Select(m => Server.Instance.GetCharacter(m.ID)).Where(c => c != null).ToArray();
            pw.WriteByte((byte)chars.Length);
            for (int i = 0; i < chars.Length; i++)
            {
                var member = chars[i];
                pw.WriteInt(member.ID);
                // 29
                pw.WriteString13(member.Name);
                pw.WriteInt(member.Job);
                pw.WriteInt(member.Level);
                pw.WriteInt((int)Members[i].Rank);
                pw.WriteInt(member.IsOnline ? 1 : 0);
                //pw.WriteInt(memIdx);
            }
            pw.WriteInt(Capacity);

            pw.WriteShort(1030); // logo background
            pw.WriteByte(3); // Logo bg color

            pw.WriteShort(4017); // logo
            pw.WriteByte(2); // logo color
        }
    }
}
