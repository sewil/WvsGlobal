using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Web;
using WvsBeta.Common.Sessions;
using WvsBeta.Game.Packets;

namespace WvsBeta.Game.Handlers.Guild
{
    public enum GuildAction
    {
        EnterGuildName = 2,
        Invite = 5,
        SaveRanks = 13,
        ChangeMemberRank = 14,
        ContractRespond = 21,
    }
    class GuildException : Exception
    {
        public GuildException(string message) : base(message)
        {
        }
    }
    public static class GuildHandler
    {
        private static Server S => Server.Instance;

        private static void BroadcastPacket(Packet packet, Action<Packet> packetHandler = null)
        {
            var pw = new Packet(ISClientMessages.BroadcastPacketToGameServersExcept);
            pw.WriteInt(S.ID);

            var pr = new Packet(packet.ToArray());
            pw.WriteBytes(pr.ReadLeftoverBytes());
            S.CenterConnection.SendPacket(pw);
            if (packetHandler != null)
            {
                pr.Position = 1;
                packetHandler(pr);
            }
        }
        #region Ranks
        public static void SendUpdateRanks(GuildData guild)
        {
            var pw = new Packet(ISServerMessages.GuildUpdateRanks);
            pw.WriteInt(guild.ID);
            for (int i = 0; i < GuildData.RANKS; i++)
            {
                pw.WriteString(guild.RankNames[i]);
            }
            BroadcastPacket(pw, HandleUpdateRanks);
            GuildDbHandler.SaveRanks(guild.ID, guild.RankNames);
        }
        public static void HandleUpdateRanks(Packet pr)
        {
            int guildId = pr.ReadInt();
            GuildData guild = S.Guilds[guildId];
            for (int i = 0; i < GuildData.RANKS; i++)
            {
                guild.RankNames[i] = pr.ReadString();
            }
            foreach (var c in guild.Characters)
            {
                c.SendPacket(GuildPacket.UpdateRanks(guildId, guild.RankNames));
            }
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
            GuildData guild = S.Guilds[guildId];
            int cid = pr.ReadInt();
            GuildRank rank = (GuildRank)pr.ReadByte();
            guild.Members[cid].Rank = rank;
            foreach (var c in guild.Characters)
            {
                c.SendPacket(GuildPacket.ChangeMemberRank(guildId, cid, rank));
            }
        }
        #endregion
        #region Create guild
        public static void SendCreateGuildEnterName(GameCharacter chr)
        {
            chr.SendPacket(GuildPacket.EnterName());
        }
        public static void HandleCreateGuildEnterName(GameCharacter chr, string guildName)
        {
            // Check name not taken
            if (GuildDbHandler.IsNameTaken(guildName))
            {
                chr.SendPacket(new GuildPacket((byte)GuildFormType.GuildNameAlreadyInUse));
                return;
            }

            // Deduct fee
            if (!chr.IncMoney(chr._guildPendingFee, MessageAppearType.ChatGrey))
            {
                chr.SendPacket(new GuildPacket((byte)GuildFormType.NpcFormError));
                return;
            }

            // Send contract to party
            chr._guildNamePending = guildName;
            chr._guildContractPending = new List<bool>();
            chr.Field.SendPacket(GuildPacket.ShowGuildContract(chr.PartyID, chr.Name, guildName));
        }
        public static void HandleContractResponse(int cid, bool response)
        {
            var c = S.GetCharacter(cid);
            if (c.Party == null) return;
            var ldr = S.GetCharacter(c.Party.Leader);
            lock (ldr._guildContractPending)
            {
                if (ldr._guildContractPending == null) return;
                if (!response)
                {
                    ldr._guildContractPending = null;
                    ldr.SendPacket(GuildPacket.NpcContractDisagreedMsg());
                    return;
                }
                ldr._guildContractPending.Add(response);
#if DEBUG
                if (ldr._guildContractPending.Count == c.Party.Characters.Count() - 1)
#else
                if (ldr._guildContractPending.Count == 5)
#endif
                {
                    ldr._guildContractPending = null;
                    goto CREATE;
                }
                else
                {
                    goto END;
                }
            }
        CREATE:
            HandleGuildCreate(ldr);
        END:
            return;
        }
        public static void HandleGuildCreate(GameCharacter ldr)
        {
            if (ldr.Party == null || string.IsNullOrWhiteSpace(ldr._guildNamePending))
            {
                ldr.SendPacket(new GuildPacket((byte)GuildFormType.NpcFormError));
                return;
            }
            // Handle create guild, db insert, etc
            var partyChars = ldr.Party.Characters.ToList();
            var members = partyChars.Select(c => new KeyValuePair<int, GuildMember>(c.ID, new GuildMember(
                c.ID == ldr.ID ? GuildRank.Master : GuildRank.JrMaster,
                c.Name,
                c.Job,
                c.Level,
                true
            ))).ToDictionary(c => c.Key, c => c.Value);
            var guild = new GuildData(0, ldr._guildNamePending, members);

            if (!GuildDbHandler.AddGuild(guild))
            {
                ldr.SendPacket(new GuildPacket((byte)GuildFormType.NpcFormError));
                return;
            }

            SendLoadGuild(guild);

            ldr.SendPacket(GuildPacket.GuildHasBeenMade(guild.ID, ldr.ID));
            foreach (var c in partyChars)
            {
                c.GuildID = guild.ID;
                c.SendPacket(GuildPacket.GuildCreate(guild));
                RemotePacket.SendCharacterGuildInfo(c);
            }
        }
        #endregion

        #region Disband guild
        public static bool HandleDisbandGuild(GameCharacter chr, int fee, int guildId)
        {
            if (chr.GuildID == 0) return false;
            if (!S.Guilds.ContainsKey(guildId)) return false;
            if (!chr.IncMoney(fee, MessageAppearType.ChatGrey)) return false;
            if (!GuildDbHandler.RemoveGuild(guildId)) return false;
            SendUnloadGuild(guildId);
            return true;
        }
        public static void SendUnloadGuild(int guildId)
        {
            var pw = new Packet(ISServerMessages.GuildUnload);
            pw.WriteInt(guildId);
            BroadcastPacket(pw, HandleUnloadGuild);
        }
        public static void HandleUnloadGuild(Packet pr)
        {
            int guildId = pr.ReadInt();
            GuildData guild = S.Guilds[guildId];
            foreach (var c in guild.Characters)
            {
                c.GuildID = 0;
                c.SendPacket(GuildPacket.GuildDisbanded(guildId));
                RemotePacket.SendCharacterGuildInfo(c);
            }
            S.Guilds.Remove(guildId);
        }
        #endregion
        public static void HandleAction(GameCharacter chr, Packet pr)
        {
            GuildAction action = (GuildAction)pr.ReadByte();
            try
            {
                switch (action)
                {
                    case GuildAction.EnterGuildName:
                        // 4C 02 06 00 61 73 64 66 66 66
                        string guildName = pr.ReadString();
                        HandleCreateGuildEnterName(chr, guildName);
                        break;
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
                    case GuildAction.ContractRespond:
                        {
                            // contract decline 4C 15 [05 00 00 00] 00
                            // contract timeout 4C 15 [05 00 00 00] 00
                            // contract accept  4C 15 [05 00 00 00] [01]
                            int cid = pr.ReadInt();
                            bool response = pr.ReadBool();
                            HandleContractResponse(cid, response);
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
            var guild = S.Guilds[guildId];
            guild.Members[cid].IsOnline = isOnline;

            foreach (var c in guild.Characters.Where(c => c.ID != cid))
            {
                c.SendPacket(GuildPacket.MemberSetOnline(guildId, cid, isOnline));
            }
        }
        public static void SendLoadGuild(GameCharacter chr, int guildId)
        {
            GuildData guild = GuildDbHandler.LoadGuild(chr, guildId);
            if (guild == null) return;
            SendLoadGuild(guild);
        }
        public static void SendLoadGuild(GuildData guild)
        {
            var pw = new Packet(ISServerMessages.GuildLoad);
            guild.Encode(pw);
            BroadcastPacket(pw, HandleLoadGuild);
        }
        public static void HandleLoadGuild(Packet pr)
        {
            var guild = GuildData.Decode(pr);
            S.Guilds[guild.ID] = guild;
        }
    }
}
