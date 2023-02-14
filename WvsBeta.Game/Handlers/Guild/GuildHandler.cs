using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using WvsBeta.Common.Sessions;
using WvsBeta.Game.Packets;

namespace WvsBeta.Game.Handlers.Guild
{
    public enum GuildAction
    {
        Invite = 5,
        SaveRanks = 13,
        ChangeMemberRank = 14,
    }
    class GuildException : Exception
    {
        public GuildException(string message) : base(message)
        {
        }
    }
    public static class GuildHandler
    {
        private static Dictionary<int, GuildData> Guilds => Server.Instance.Guilds;
        private static void BroadcastPacket(Packet packet, Action<Packet> packetHandler = null)
        {
            var pw = new Packet(ISClientMessages.BroadcastPacketToGameServersExcept);
            pw.WriteInt(Server.Instance.ID);

            var pr = new Packet(packet.ToArray());
            pw.WriteBytes(pr.ReadLeftoverBytes());
            Server.Instance.CenterConnection.SendPacket(pw);
            if (packetHandler != null)
            {
                pr.Position = 1;
                packetHandler(pr);
            }
        }
        public static void SendDbUpdateRanks(GuildData guild)
        {
            var pw = new Packet(ISClientMessages.GuildDbUpdateRanks);
            pw.WriteInt(guild.ID);
            for (int i = 0; i < GuildData.RANKS; i++)
            {
                pw.WriteString(guild.RankNames[i]);
            }
            Server.Instance.CenterConnection.SendPacket(pw);
        }
        public static void SendUpdateRanks(GuildData guild)
        {
            var pw = new Packet(ISServerMessages.GuildUpdateRanks);
            pw.WriteInt(guild.ID);
            for (int i = 0; i < GuildData.RANKS; i++)
            {
                pw.WriteString(guild.RankNames[i]);
            }
            BroadcastPacket(pw, HandleUpdateRanks);
            SendDbUpdateRanks(guild);
        }
        public static void HandleUpdateRanks(Packet pr)
        {
            int guildId = pr.ReadInt();
            GuildData guild = Server.Instance.Guilds[guildId];
            for (int i = 0; i < GuildData.RANKS; i++)
            {
                guild.RankNames[i] = pr.ReadString();
            }
            foreach (var c in guild.Characters)
            {
                c.SendPacket(GuildPacket.UpdateRanks(guildId, guild.RankNames));
            }
        }
        public static void SendDbUpdateMember(int guildId, int cid, GuildRank rank)
        {
            var pw = new Packet(ISClientMessages.GuildDbUpdateMember);
            pw.WriteInt(guildId);
            pw.WriteInt(cid);
            pw.WriteByte((byte)rank);
            Server.Instance.CenterConnection.SendPacket(pw);
        }
        public static void SendChangeMemberRank(int guildId, int cid, GuildRank rank)
        {
            var pw = new Packet(ISServerMessages.GuildUpdateRanks);
            pw.WriteInt(guildId);
            pw.WriteInt(cid);
            pw.WriteByte((byte)rank);
            BroadcastPacket(pw, HandleMemberChangeRank);
        }
        public static void HandleMemberChangeRank(Packet pr)
        {
            int guildId = pr.ReadInt();
            GuildData guild = Server.Instance.Guilds[guildId];
            int cid = pr.ReadInt();
            GuildRank rank = (GuildRank)pr.ReadByte();
            guild.Members[cid].Rank = rank;
            foreach (var c in guild.Characters)
            {
                c.SendPacket(GuildPacket.ChangeMemberRank(guildId, cid, rank));
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
                        // 4C 05 04 00 61 73 64 66 = invite
                        break;
                    case GuildAction.SaveRanks:
                        {
                            if (!chr.IsGuildMaster) throw new GuildException("You need to be the Guild Master to do this.");
                            // 4C [0D] [04 00 42 6F 73 73] [00 00] [00 00] [00 00] [00 00] = save ranks (Master to Boss)
                            var guild = chr.Guild;
                            for (int i = 0; i < GuildData.RANKS; i++)
                            {
                                guild.RankNames[i] = pr.ReadString();
                            }

                            SendUpdateRanks(guild);
                            break;
                        }
                    case GuildAction.ChangeMemberRank:
                        {
                            // 4C 0E [05 00 00 00] [03] = title down from jr to mem1
                            if (!chr.CanChangeRank) throw new GuildException("You need to be at least a Jr. Master to do this.");
                            int cid = pr.ReadInt();
                            GuildMember member = chr.Guild.Members[cid];
                            if (member == null) throw new GuildException("This member does not exist.");
                            GuildRank newRank = (GuildRank)pr.ReadByte();
                            if (newRank < member.Rank && !chr.IsGuildMaster) throw new GuildException("You need to be a Guild Master to do this.");
                            SendChangeMemberRank(chr.GuildID, cid, newRank);
                            break;
                        }
                    default:
                        break;
                }
            }
            catch (GuildException e)
            {
                chr.SendPacket(MessagePacket.RedText(e.Message));
            }
        }
        public static void SendGuild(GameCharacter chr, GuildData guild)
        {
            var pw = GuildPacket.GuildInfo(guild);
            chr.SendPacket(pw);
        }
        public static void SendMemberIsOnline(GameCharacter chr, bool isOnline)
        {
            GuildData guild = chr.Guild;
            var pw = new Packet(ISServerMessages.GuildMemberIsOnline);
            pw.WriteInt(guild.ID);
            pw.WriteInt(chr.ID);
            pw.WriteBool(isOnline);
            BroadcastPacket(pw, HandleMemberIsOnline);
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

                    var pw = new Packet(ISServerMessages.GuildLoad);
                    guild.Encode(pw);
                    BroadcastPacket(pw, HandleLoadGuild);
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
