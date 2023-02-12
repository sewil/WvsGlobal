using WvsBeta.Common.Sessions;

namespace WvsBeta.Game.Handlers.Guild
{
    public enum GuildResultType
    {
        EnterGuildName = 1,
        ShowGuildContract = 3,
        GuildInvitation = 5,
        GuildNameAlreadyInUse = 19,
        GuildContractNonAgreement = 22,
        GuildCreate = 23,
        Unk35 = 35,
        Err36,
        Err37,
        Err38,
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
        Unk51,
        Err52,
        Err53,
        Err54,
        Err55,
        Unk56,
        Unk57
    }
    public class GuildPacket : Packet
    {
        private GuildPacket(GuildResultType type) : base(ServerMessages.GUILD_RESULT)
        {
            WriteByte((byte)type);
        }
        public static void SendGuildCreate(GameCharacter chr, GuildData guild)
        {
            var pw = new GuildPacket(GuildResultType.GuildCreate);
            guild.Encode(pw);
            // Send to party
            chr.SendPacket(pw);
        }
        public static void SendResult(GameCharacter chr, GuildResultType type)
        {
            var pw = new Packet(ServerMessages.GUILD_RESULT);
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
                case GuildResultType.GuildContractNonAgreement: // No data
                    break;
                case GuildResultType.Unk35:
                    pw.WriteInt(0);
                    pw.WriteInt(0);
                    pw.WriteString("");
                    break;
                case GuildResultType.Err36:
                    break;
                case GuildResultType.Err37:
                    break;
                case GuildResultType.Err38:
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
                case GuildResultType.Unk51:
                    pw.WriteInt(0);
                    pw.WriteInt(0);
                    pw.WriteInt(0);
                    pw.WriteInt(0);
                    break;
                case GuildResultType.Err52:
                    pw.WriteInt(0);
                    pw.WriteInt(0);
                    pw.WriteByte(0);
                    break;
                case GuildResultType.Err53:
                    pw.WriteInt(0);
                    pw.WriteString("");
                    pw.WriteString("");
                    pw.WriteString("");
                    pw.WriteString("");
                    pw.WriteString("");
                    break;
                case GuildResultType.Err54:
                    break;
                case GuildResultType.Err55:
                    pw.WriteInt(0);
                    pw.WriteInt(0);
                    pw.WriteByte(0);
                    break;
                case GuildResultType.Unk56:
                    break;
                case GuildResultType.Unk57:
                    pw.WriteInt(0);
                    pw.WriteShort(0);
                    pw.WriteByte(0);
                    pw.WriteShort(0);
                    pw.WriteByte(0);
                    break;
                default:
                    break;
            }
        }
    }
}
