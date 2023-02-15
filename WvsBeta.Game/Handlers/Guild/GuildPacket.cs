using WvsBeta.Common.Sessions;

namespace WvsBeta.Game.Handlers.Guild
{
    /*
     Ignored:
     36, 39, 47
     */
    public enum GuildMemberActionType : byte
    {
        MemberQuit = 35,
        MemberExpelled = 38,
    }
    public enum GuildFormType : byte
    {
        EnterGuildName = 1,
        ShowGuildContract = 3,
        GuildInvitation = 5,
        GuildEmblemSelector = 16,
        GuildInfo = 17,
        GuildNameAlreadyInUse = 19,
        NpcContractAgreementError = 22,
        GuildCreate = 23,
        NpcContractSomebodyDisagreed = 27,
        NpcFormError = 29,
        TheGuildHasBeenMade = 30,
        NpcExpandSuccess = 49,
    }
    public enum GuildNpcErrorType : byte
    {
        NpcErrorDisbandingGuild = 43,
        NpcErrorExpandingGuild = 50,
    }
    public enum GuildErrorType : byte
    {
        ErrorNotAcceptingInviteMsg = 44,
        ErrorTakingCareOfAnotherInvitation = 45,
        ErrorInvitationDenied = 46,
    }
    public enum GuildActionResultType : byte
    {
        GuildDisbanded = 41,
        MemberUpdateLevelJob = 51,
        MemberSetOnline = 52,
        RanksUpdated = 53,
        MemberChangeRank = 55,
        SetGuildEmblem = 57
    }
    public class GuildPacket : Packet
    {
        public GuildPacket(byte type) : base(ServerMessages.GUILD_RESULT)
        {
            WriteByte(type);
        }
        public GuildPacket(byte type, int guildId) : this(type)
        {
            WriteInt(guildId);
        }
        public static GuildPacket GuildDisbanded(int guildId)
        {
            return new GuildPacket((byte)GuildActionResultType.GuildDisbanded, guildId);
        }
        public static GuildPacket GuildInfo(GuildData guild)
        {
            var pw = new GuildPacket((byte)GuildFormType.GuildInfo);
            guild.Encode(pw);
            return pw;
        }
        /// <summary>
        /// Only works for Guild Masters at Guild HQ
        /// </summary>
        public static GuildPacket GuildEmblemSelector()
        {
            return new GuildPacket((byte)GuildFormType.GuildEmblemSelector);
        }
        public static GuildPacket GuildHasBeenMade(int guildId, int cid)
        {
            var pw = new GuildPacket((byte)GuildFormType.TheGuildHasBeenMade, guildId);
            pw.WriteInt(cid);
            return pw;
        }
        public static GuildPacket NpcContractDisagreedMsg()
        {
            return new GuildPacket((byte)GuildFormType.NpcContractSomebodyDisagreed);
        }
        public static GuildPacket ShowGuildContract(int partyId, string guildMaster, string guildName)
        {
            var pw = new GuildPacket((byte)GuildFormType.ShowGuildContract);
            pw.WriteInt(partyId);
            pw.WriteString(guildMaster);
            pw.WriteString(guildName);
            return pw;
        }
        public static GuildPacket UpdateRanks(int guildId, string[] ranks)
        {
            var pw = new GuildPacket((byte)GuildActionResultType.RanksUpdated);
            pw.WriteInt(guildId);
            foreach (var rank in ranks)
            {
                pw.WriteString(rank);
            }
            return pw;
        }
        public static GuildPacket ChangeMemberRank(int guildId, int cid, GuildRank rank)
        {
            var pw = new GuildPacket((byte)GuildActionResultType.MemberChangeRank);
            pw.WriteInt(guildId);
            pw.WriteInt(cid);
            pw.WriteByte((byte)rank);
            return pw;
        }
        public static GuildPacket UpdateMemberLevelJob(int guildId, int cid, int level, int job)
        {
            var pw = new GuildPacket((byte)GuildActionResultType.MemberUpdateLevelJob);
            pw.WriteInt(guildId);
            pw.WriteInt(cid);
            pw.WriteInt(level);
            pw.WriteInt(job);
            return pw;
        }
        public static GuildPacket SetGuildEmblem(int guildId, GuildEmblem emblem)
        {
            var pw = new GuildPacket((byte)GuildActionResultType.SetGuildEmblem);
            pw.WriteInt(guildId);
            emblem.Encode(pw);
            return pw;
        }
        public static GuildPacket MemberSetOnline(int guildId, int cid, bool isOnline)
        {
            var pw = new GuildPacket((byte)GuildActionResultType.MemberSetOnline);
            pw.WriteInt(guildId);
            pw.WriteInt(cid);
            pw.WriteBool(isOnline);
            return pw;
        }
        public static GuildPacket EnterName()
        {
            return new GuildPacket((byte)GuildFormType.EnterGuildName);
        }
        public static GuildPacket GuildCreate(GuildData guild)
        {
            var pw = new GuildPacket((byte)GuildFormType.GuildCreate);
            guild.Encode(pw);
            return pw;
        }
        public static GuildPacket GuildMemberAction(int guildId, int cid, string name, GuildActionResultType type)
        {
            var pw = new GuildPacket((byte)type, guildId);
            pw.WriteInt(cid);
            pw.WriteString(name);
            return pw;
        }
        public static GuildPacket GuildError(GuildErrorType type)
        {
            return new GuildPacket((byte)type);
        }
        /// <summary>
        /// Only triggers if inside Guild HQ
        /// </summary>
        public static GuildPacket NpcExpandSuccessMsg(int guildId, byte newCapacity)
        {
            var pw = new GuildPacket((byte)GuildFormType.NpcExpandSuccess, guildId);
            pw.WriteByte(newCapacity);
            return pw;
        }
    }
}
