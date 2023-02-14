using MySql.Data.MySqlClient;
using System;
using System.Linq;
using System.Security.Cryptography;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Game.Handlers.Guild
{
    public enum GuildAction
    {
        Invite = 5,
        SaveRanks = 13,
    }
    public class GuildException : Exception
    {
        public GuildResultType Type { get; private set; }
        public GuildException(GuildResultType type)
        {
            Type = type;
        }
    }
    public static class GuildHandler
    {
        public static void SendUpdateRanks(GameCharacter chr, GuildData guild)
        {
            var pw = new Packet(ISClientMessages.BroadcastPacketToGameServers);
            pw.WriteByte((byte)ISServerMessages.GuildUpdateRanks);
            pw.WriteInt(chr.ID);
            pw.WriteInt(guild.ID);
            for (int i = 0; i < GuildData.RANKS; i++)
            {
                pw.WriteString(guild.RankNames[i]);
            }
            Server.Instance.CenterConnection.SendPacket(pw);
        }
        public static void HandleUpdateRanks(Packet pr)
        {
            int cid = pr.ReadInt();
            int guildId = pr.ReadInt();
            GuildData guild = Server.Instance.Guilds[guildId];
            for (int i = 0; i < GuildData.RANKS; i++)
            {
                guild.RankNames[i] = pr.ReadString();
            }
            foreach (var c in guild.Members.Where(m => m.Key != cid && m.Value.IsOnline).Select(m => Server.Instance.GetCharacter(m.Key)).Where(c => c != null))
            {
                c.SendPacket(GuildPacket.UpdateRanks(guildId, guild.RankNames));
            }
        }
        public static void HandleAction(GameCharacter chr, Packet pr)
        {
            GuildAction action = (GuildAction)pr.ReadByte();
            try
            {
                switch (action)
                {
                    case GuildAction.Invite:
                        break;
                    case GuildAction.SaveRanks:
                        {
                            if (!chr.IsGuildMaster) throw new GuildException(GuildResultType.ErrorNotAcceptingInviteMsg);
                            // 4C 0D 04 00 42 6F 73 73 00 00 00 00 00 00 00 00 = save ranks (Master to Boss)
                            var guild = chr.Guild;
                            for (int i = 0; i < GuildData.RANKS; i++)
                            {
                                guild.RankNames[i] = pr.ReadString();
                            }
                            
                            foreach (var c in guild.Members.Select(i => i.Key).Where(cid => cid != chr.ID).Select(cid => Server.Instance.GetCharacter(cid)).Where(i => i != null))
                            {
                                c.SendPacket(GuildPacket.UpdateRanks(guild.ID, guild.RankNames));
                            }
                            break;
                        }
                    default:
                        break;
                }
            }
            catch (GuildException e)
            {
                chr.SendPacket(GuildPacket.GuildResult(e.Type));
            }
            // 4C 05 04 00 61 73 64 66 = invite
        }
        public static void SendGuild(GameCharacter chr, GuildData guild)
        {
            var pw = GuildPacket.GuildInfo(guild);
            chr.SendPacket(pw);
        }
        public static void SendMemberIsOnline(GameCharacter chr, bool isOnline)
        {
            GuildData guild = chr.Guild;
            var pw = new Packet(ISClientMessages.BroadcastPacketToGameServers);
            pw.WriteByte((byte)ISServerMessages.GuildMemberIsOnline);
            pw.WriteInt(guild.ID);
            pw.WriteInt(chr.ID);
            pw.WriteBool(isOnline);
            guild.Members[chr.ID].IsOnline = isOnline;
            Server.Instance.CenterConnection.SendPacket(pw);
        }
        public static void HandleMemberIsOnline(Packet pr)
        {
            int guildId = pr.ReadInt();
            int cid = pr.ReadInt();
            bool isOnline = pr.ReadBool();
            var guild = Server.Instance.Guilds[guildId];
            guild.Members[cid].IsOnline = isOnline;

            foreach (var c in guild.Characters.Where(c => c.ID != cid))
            {
                c.SendPacket(GuildPacket.MemberSetOnline(guildId, cid, isOnline));
            }
        }
        public static void SaveMember(GameCharacter chr)
        {
            if (chr.GuildMember == null) throw new Exception("Error saving guild member " + chr.ID + " in guild " + chr.GuildID + ". Guild member is null");
            int cid = chr.ID;
            int guildId = chr.GuildID;
            GuildRank rank = chr.GuildMember.Rank;
            // Save rank
            Server.Instance.CharacterDatabase.RunQuery(@"
                UPDATE guild_members
                SET `rank` = @rank
                WHERE character_id = @cid AND guild_id = @guildId
            ", "@rank", (int)rank, "@cid", cid, "@guildId", guildId);
        }
        public static GuildData LoadGuild(GameCharacter chr)
        {
            int guildId = chr.GuildID;
            try
            {
                GuildData guild;
                using (var guildReader = Server.Instance.CharacterDatabase.RunQuery(@"SELECT * FROM guilds WHERE id = @id", "@id", guildId) as MySqlDataReader)
                {
                    if (!guildReader.Read())
                    {
                        throw new ArgumentException("Failed reading guild with guild id " + guildId);
                    }
                    guild = GuildData.LoadFromReader(guildReader);
                }
                using (var membersReader = Server.Instance.CharacterDatabase.RunQuery(@"
                    SELECT g.*, c.level, c.job, c.name
                    FROM guild_members g
                    LEFT JOIN characters c ON c.id = g.character_id
                    WHERE g.guild_id = @guild_id
                ", "@guild_id", guildId) as MySqlDataReader)
                {
                    while (membersReader.Read())
                    {
                        int cid = membersReader.GetInt32("character_id");
                        GuildMember member = GuildMember.LoadFromReader(membersReader, cid == chr.ID);
                        guild.Members[cid] = member;
                    }

                    Server.Instance.Guilds[guildId] = guild;
                    var pw = new Packet(ISClientMessages.BroadcastPacketToGameServersExcept);
                    pw.WriteInt(Server.Instance.ID);
                    pw.WriteByte((byte)ISServerMessages.GuildLoad);
                    guild.Encode(pw);
                    Server.Instance.CenterConnection.SendPacket(pw);

                    return guild;
                }
            }
            catch (Exception e)
            {
                Program.MainForm.LogAppend($"Loading guild {guildId} failed! {e.Message}");
                return null;
            }
        }
        public static void HandleLoadGuild(Packet pr)
        {
            var guild = GuildData.Decode(pr);
            Server.Instance.Guilds[guild.ID] = guild;
        }
    }
}
