using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Game.Handlers.Guild
{
    public enum GuildRank : int
    {
        Master = 1,
        JrMaster = 2,
        Member1 = 3,
        Member2 = 4,
        Member3 = 5,
    }
    public class GuildMember
    {
        public GuildRank Rank { get; set; }
        public string Name { get; private set; }
        public int Job { get; private set; }
        public int Level { get; private set; }
        public bool IsOnline { get; set; }
        public GuildMember(GuildRank rank, string name, int job, int level, bool isOnline)
        {
            Rank = rank;
            Name = name;
            Job = job;
            Level = level;
            IsOnline = isOnline;
        }
        private GuildMember(MySqlDataReader reader, bool isOnline)
        {
            Rank = (GuildRank)reader.GetInt32("rank");
            Name = reader.GetString("name");
            Job = reader.GetInt16("job");
            Level = reader.GetByte("level");
            IsOnline = isOnline;
        }
        public static GuildMember LoadFromReader(MySqlDataReader reader, bool isOnline)
        {
            return new GuildMember(reader, isOnline);
        }
        private GuildMember(Packet pr)
        {
            Decode(pr);
        }
        public static GuildMember DecodeNew(Packet pr)
        {
            return new GuildMember(pr);
        }
        public GuildMember Decode(Packet pr)
        {
            Name = pr.ReadString(13, true); // 13
            Job = pr.ReadInt(); // 17
            Level = pr.ReadInt(); // 21
            Rank = (GuildRank)pr.ReadInt(); // 25
            IsOnline = pr.ReadInt() == 1; // 29
            return this;
        }
        public void Encode(Packet pw)
        {
            pw.WriteString13(Name);
            pw.WriteInt(Job);
            pw.WriteInt(Level);
            pw.WriteInt((int)Rank);
            pw.WriteInt(IsOnline ? 1 : 0);
        }
    }
    public class GuildEmblem
    {
        public static readonly GuildEmblem Default = new GuildEmblem();
        public short Background { get; private set; }
        public byte BackgroundColor { get; private set; }
        public short Logo { get; private set; }
        public byte LogoColor { get; private set; }
        private GuildEmblem()
        {
        }
        public GuildEmblem(short background, byte backgroundColor, short logo, byte logoColor)
        {
            Background = background;
            BackgroundColor = backgroundColor;
            Logo = logo;
            LogoColor = logoColor;
        }
        private GuildEmblem(Packet pr)
        {
            Background = pr.ReadShort();
            BackgroundColor = pr.ReadByte();
            Logo = pr.ReadShort();
            LogoColor = pr.ReadByte();
        }
        public static GuildEmblem Decode(Packet pr)
        {
            return new GuildEmblem(pr);
        }
        public void Encode(Packet pw)
        {
            pw.WriteShort(Background);
            pw.WriteByte(BackgroundColor);
            pw.WriteShort(Logo);
            pw.WriteByte(LogoColor);
        }
        public void EncodeForRemote(Packet pw)
        {
            Encode(pw);
        }
        private GuildEmblem(MySqlDataReader reader)
        {
            Background = reader.GetInt16("emblem_bg");
            BackgroundColor = reader.GetByte("emblem_bgc");
            Logo = reader.GetInt16("emblem_logo");
            LogoColor = reader.GetByte("emblem_logoc");
        }
        public static GuildEmblem LoadFromReader(MySqlDataReader reader)
        {
            return new GuildEmblem(reader);
        }
    }
    public class GuildData
    {
        public static readonly GuildData Default = new GuildData();
        public const int RANKS = 5;
        public int ID { get; set; }
        public string Name { get; protected set; }
        public int Capacity { get; protected set; }
        public string[] RankNames { get; protected set; } = new string[RANKS] { "", "", "", "", "" };
        public Dictionary<int, GuildMember> Members = new Dictionary<int, GuildMember>();
        public IEnumerable<GameCharacter> Characters => Members.Where(m => m.Value.IsOnline).Select(m => Server.Instance.GetCharacter(m.Key)).Where(c => c != null);
        public GuildEmblem Emblem { get; set; }
        protected GuildData()
        {
            Name = "";
            Emblem = GuildEmblem.Default;
        }
        public GuildData(int id, string name, Dictionary<int, GuildMember> members)
        :this(id, name, 20, new string[]{"Master","Jr. Master","Member", "Member", "Member"}, members, GuildEmblem.Default)
        {

            
        }
        public GuildData(int id, string name, int capacity, string[] rankNames, Dictionary<int, GuildMember> members, GuildEmblem emblem)
        {
            ID = id;
            Name = name;
            Capacity = capacity;
            Members = members;
            RankNames = rankNames;
            Emblem = emblem;
        }
        protected GuildData(Packet pr)
        {
            ID = pr.ReadInt();
            Name = pr.ReadString();
            for (int i = 0; i < RANKS; i++)
            {
                RankNames[i] = pr.ReadString();
            }
            byte memberLength = pr.ReadByte();
            var ids = new List<int>();
            for (int i = 0; i < memberLength; i++)
            {
                ids.Add(pr.ReadInt());
            }
            for (int i = 0; i < memberLength; i++)
            {
                var member = GuildMember.DecodeNew(pr);
                Members[ids[i]] = member;
            }
            Capacity = pr.ReadInt();
            Emblem = GuildEmblem.Decode(pr);
        }
        public static GuildData Decode(Packet pr)
        {
            return new GuildData(pr);
        }
        public void Encode(Packet pw)
        {
            pw.WriteInt(ID);
            pw.WriteString(Name);
            foreach (var rankName in RankNames)
            {
                pw.WriteString(rankName);
            }
            pw.WriteByte((byte)Members.Count);
            foreach (int cid in Members.Keys)
            {
                pw.WriteInt(cid);
            }
            foreach (var member in Members.Values)
            {
                member.Encode(pw);
            }
            pw.WriteInt(Capacity);

            Emblem.Encode(pw);
        }

        public void EncodeForRemote(Packet pw)
        {
            pw.WriteString(Name);
            Emblem.EncodeForRemote(pw);
        }

        public void EncodeInfo(Packet pw)
        {
            pw.WriteInt(ID);
            pw.WriteInt(Capacity);
            for (int i = 0; i < GuildData.RANKS; i++)
            {
                pw.WriteString(RankNames[i]);
            }
            Emblem.Encode(pw);
        }
        public static GuildData DecodeInfo(Packet pr)
        {
            string[] rankNames = new string[RANKS];
            for (int i = 0; i < RANKS; i++)
            {
                rankNames[i] = pr.ReadString();
            }
            var guild = new GuildData(pr.ReadInt(), "", pr.ReadInt(), rankNames, null, GuildEmblem.Decode(pr));
            return guild;
        }
        protected GuildData(MySqlDataReader reader)
        {
            ID = reader.GetInt32("id");
            Name = reader.GetString("name");
            Capacity = reader.GetInt32("capacity");
            Emblem = GuildEmblem.LoadFromReader(reader);
            for (int i = 0; i < RANKS; i++)
            {
                RankNames[i] = reader.GetString("rank" + (i+1));
            }
        }
        public static GuildData LoadFromReader(MySqlDataReader reader)
        {
            return new GuildData(reader);
        }
    }
}
