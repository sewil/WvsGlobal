using log4net.Core;
using System;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Game.Handlers.Guild
{
    public enum GuildResultType
    {
        EnterGuildName = 1,
        ShowGuildContract = 3,
        GuildInvitation = 5,
        GuildInfo = 17,
        GuildNameAlreadyInUse = 19,
        NpcContractAgreementError = 22,
        GuildCreate = 23,
        NpcContractSomebodyDisagreed = 27,
        NpcFormError = 29,
        TheGuildHasBeenMade = 30,
        MemberQuit = 35,
        YouAreNotInTheGuild = 36,
        GuildRequestNotAccepted = 37,
        MemberExpelled = 38,
        Err39,
        Err40,
        Err41,
        Err42,
        NpcErrorDisbandingGuild = 43,
        ErrorNotAcceptingInviteMsg = 44,
        ErrorTakingCareOfAnotherInvitation = 45,
        ErrorInvitationDenied = 46,
        Err47,
        Err48,
        Err49,
        NpcErrorExpandingGuild = 50,
        MemberUpdateLevelJob = 51,
        MemberSetOnline = 52,
        RanksUpdated = 53,
        Err54,
        MemberChangeRank = 55,
        Unk56,
        UpdateGuildEmblem = 57
    }
    public class GuildPacket : Packet
    {
        private GuildPacket(GuildResultType type) : base(ServerMessages.GUILD_RESULT)
        {
            WriteByte((byte)type);
        }
        public static GuildPacket GuildInfo(GuildData guild)
        {
            var pw = new GuildPacket(GuildResultType.GuildInfo);
            guild.Encode(pw);
            return pw;
        }
        public static GuildPacket UpdateRanks(int guildId, string[] ranks)
        {
            var pw = new GuildPacket(GuildResultType.RanksUpdated);
            pw.WriteInt(guildId);
            foreach (var rank in ranks)
            {
                pw.WriteString(rank);
            }
            return pw;
        }
        public static GuildPacket MemberExpelled(int guildId, int cid, string name)
        {
            var pw = new GuildPacket(GuildResultType.MemberExpelled);
            pw.WriteInt(guildId);
            pw.WriteInt(cid);
            pw.WriteString(name);
            return pw;
        }
        public static GuildPacket MemberQuit(int guildId, int cid, string name)
        {
            var pw = new GuildPacket(GuildResultType.MemberQuit);
            pw.WriteInt(guildId);
            pw.WriteInt(cid);
            pw.WriteString(name);
            return pw;
        }
        public static GuildPacket ChangeMemberRank(int guildId, int cid, GuildRank rank)
        {
            var pw = new GuildPacket(GuildResultType.MemberChangeRank);
            pw.WriteInt(guildId);
            pw.WriteInt(cid);
            pw.WriteByte((byte)rank);
            return pw;
        }
        public static GuildPacket UpdateMemberLevelJob(int guildId, int cid, int level, int job)
        {
            var pw = new GuildPacket(GuildResultType.MemberUpdateLevelJob);
            pw.WriteInt(guildId);
            pw.WriteInt(cid);
            pw.WriteInt(level);
            pw.WriteInt(job);
            return pw;
        }
        public static GuildPacket UpdateGuildEmblem(int guildId, GuildEmblem emblem)
        {
            var pw = new GuildPacket(GuildResultType.UpdateGuildEmblem);
            pw.WriteInt(guildId);
            emblem.Encode(pw);
            return pw;
        }
        public static GuildPacket MemberSetOnline(int guildId, int cid, bool isOnline)
        {
            var pw = new GuildPacket(GuildResultType.MemberSetOnline);
            pw.WriteInt(guildId);
            pw.WriteInt(cid);
            pw.WriteBool(isOnline);
            return pw;
        }
        public static GuildPacket GuildCreate(GuildData guild)
        {
            var pw = new GuildPacket(GuildResultType.GuildCreate);
            guild.Encode(pw);
            return pw;
        }
        public static GuildPacket GuildResult(GuildResultType type)
        {
        // 30 33 01 00 00 00 05 00 00 00 00 00 00 00 0A 00 00 00
            var pw = new GuildPacket(type);
            pw.WriteByte(0);
            int guildID = 0;
            string guildName = "guildName";
            string inviter = "inviterName";
            string inviteeName = "inviteeName";
            switch (type)
            {
                case GuildResultType.EnterGuildName: // No data
                    break;
                case GuildResultType.ShowGuildContract:
                    pw.WriteInt(guildID); // guild id
                    pw.WriteString("guildmaster");
                    pw.WriteString("guildname");
                    // Send invite to all party members
                    break;
                case GuildResultType.GuildInvitation:
                    pw.WriteInt(guildID);
                    pw.WriteString(inviter);
                    break;
                case GuildResultType.GuildNameAlreadyInUse: // No data
                    break;
                case GuildResultType.NpcContractAgreementError: // No data
                    break;
                case GuildResultType.MemberQuit:
                    pw.WriteInt(0);
                    pw.WriteInt(0);
                    pw.WriteString("");
                    break;
                case GuildResultType.YouAreNotInTheGuild:
                    break;
                case GuildResultType.MemberExpelled:
                    break;
                case GuildResultType.Err39:
                    break;
                case GuildResultType.Err40:
                    break;
                case GuildResultType.Err41:
                    pw.WriteInt(0);
                    break;
                case GuildResultType.Err42:
                    break;
                case GuildResultType.NpcErrorDisbandingGuild: // No data
                    break;
                case GuildResultType.ErrorNotAcceptingInviteMsg:
                    pw.WriteString(inviteeName);
                    break;
                case GuildResultType.ErrorTakingCareOfAnotherInvitation:
                    pw.WriteString(inviteeName);
                    break;
                case GuildResultType.ErrorInvitationDenied:
                    pw.WriteString(inviteeName);
                    break;
                case GuildResultType.Err47:
                    break;
                case GuildResultType.Err48:
                    break;
                case GuildResultType.Err49:
                    pw.WriteInt(0);
                    pw.WriteByte(0);
                    break;
                case GuildResultType.NpcErrorExpandingGuild: // No data
                    break;
                case GuildResultType.MemberUpdateLevelJob:
                    pw.WriteInt(0);
                    pw.WriteInt(0);
                    pw.WriteInt(0);
                    pw.WriteInt(0);
                    break;
                case GuildResultType.Err54:
                    break;
                case GuildResultType.MemberChangeRank:
                    pw.WriteInt(0);
                    pw.WriteInt(0);
                    pw.WriteByte(0);
                    break;
                case GuildResultType.Unk56:
                    break;
                default:
                    break;
            }
            return pw;
        }
    }
}
