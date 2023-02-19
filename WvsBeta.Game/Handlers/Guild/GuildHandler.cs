using System;
using System.Collections.Generic;
using System.Linq;
using WvsBeta.Common.Sessions;
using WvsBeta.Game.Packets;

namespace WvsBeta.Game.Handlers.Guild
{
    public enum GuildAction
    {
        EnterGuildName = 2,
        Invite = 5,
        AcceptInvitation = 6,
        Leave = 7,
        Expel = 8,
        SaveRanks = 13,
        ChangeMemberRank = 14,
        SelectGuildEmblem = 15,
        ContractRespond = 21,
    }
    class GuildException : Exception
    {
        public GuildErrorType ErrorType { get; private set; }
        public Packet Packet { get; private set; }
        public GuildException(Packet packet)
        {
            Packet = packet;
        }
        public GuildException(GuildErrorType errorType)
        {
            ErrorType = errorType;
        }
        public GuildException(string message) : base(message)
        {
        }
        public void SendPacket(GameCharacter chr)
        {
            if (Packet != null)
            {
                chr.SendPacket(Packet);
            }
            else if (ErrorType != 0)
            {
                chr.SendPacket(new GuildPacket((byte)ErrorType));
            }
            else if (!string.IsNullOrWhiteSpace(Message))
            {
                chr.SendPacket(MessagePacket.RedText(Message));
            }
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
            var pw = new Packet(ISServerMessages.GuildMemberChangeRank);
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
                RemotePacket.SendCharacterGuildName(c);
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
        #region Guild emblem
        public static void HandleCreateGuildEmblem(GameCharacter chr)
        {
            if (!chr.IsGuildMaster) return;
            chr.SendPacket(GuildPacket.GuildEmblemSelector());
        }
        public static void SendUpdateGuildEmblem(GameCharacter chr, GuildEmblem emblem)
        {
            if (!chr.IsGuildMaster) throw new GuildException("You need to be a Guild Master to do this.");
            if (!chr.IncMoney(chr._guildPendingFee, MessageAppearType.ChatGrey)) throw new GuildException("You don't have enough mesos to do this.");
            if (!GuildDbHandler.SaveGuildEmblem(chr.GuildID, emblem)) throw new GuildException("Something went wrong. Please try again later.");
            var pw = new Packet(ISServerMessages.GuildEmblemUpdated);
            pw.WriteInt(chr.GuildID);
            emblem.Encode(pw);
            BroadcastPacket(pw, HandleGuildEmblemUpdated);
        }
        public static void HandleGuildEmblemUpdated(Packet pr)
        {
            int guildId = pr.ReadInt();
            GuildData guild = S.Guilds[guildId];
            guild.Emblem = GuildEmblem.Decode(pr);
            foreach (var c in guild.Characters)
            {
                c.SendPacket(GuildPacket.SetGuildEmblem(guildId, guild.Emblem));
                RemotePacket.SendCharacterGuildEmblem(c);
            }
        }
        public static void HandleRemoveGuildEmblem(GameCharacter chr)
        {
            try
            {
                SendUpdateGuildEmblem(chr, GuildEmblem.Default);
            }
            catch (GuildException e)
            {
                chr.SendPacket(MessagePacket.RedText(e.Message));
            }
        }
        #endregion
        #region Invite/Join
        public class GuildInvite
        {
            public GameCharacter Invitee { get; private set; }
            public GameCharacter Inviter { get; private set; }
            public GuildData Guild { get; private set; }
            public GuildInvite(GameCharacter invitee, GameCharacter inviter, GuildData guild)
            {
                Invitee = invitee;
                Inviter = inviter;
                Guild = guild;
            }
        }
        public static Dictionary<int, GuildInvite> Invites = new Dictionary<int, GuildInvite>();
        public static void HandleInvite(GameCharacter chr, string victimName)
        {
            if (!chr.CanChangeRank)
            {
                throw new GuildException("You need to be at least a Jr. Master to do this.");
            }
            else if (chr.Guild.Members.Count >= chr.Guild.Capacity)
            {
                throw new GuildException("The guild has already reached maximum capacity.");
            }
            GameCharacter victim = S.GetCharacter(victimName);
            if (victim == null)
            {
                throw new GuildException(string.Format("Unable to find '{0}'", victimName));
            }
            else if (victim.IsGuildMember)
            {
                throw new GuildException(string.Format("{0} has already joined the guild", victimName));
            }
            else if (Invites.ContainsKey(victim.ID))
            {
                throw new GuildException(GuildPacket.InviteBusy(victimName));
            }
            else
            {
                victim.SendPacket(GuildPacket.Invite(chr.GuildID, victimName));
                Invites.Add(victim.ID, new GuildInvite(victim, chr, chr.Guild));
            }
        }
        public static void HandleAcceptInvitation(GameCharacter chr)
        {
            if (!Invites.TryGetValue(chr.ID, out GuildInvite invite))
            {
                throw new GuildException("Guild invite not found.");
            }
            else if (invite.Guild.Capacity == invite.Guild.Members.Count)
            {
                throw new GuildException("The guild has already reached maximum capacity.");
            }
            else if (chr.IsGuildMember)
            {
                throw new GuildException("You are already a guild member.");
            }
            else
            {
                SendMemberJoined(invite.Guild, chr);
                Invites.Remove(chr.ID);
            }
        }
        public static void SendMemberJoined(GuildData guild, GameCharacter chr)
        {
            var pw = new Packet(ISServerMessages.GuildMemberJoin);
            pw.WriteInt(guild.ID);
            pw.WriteInt(chr.ID);
            var guildMember = new GuildMember(GuildRank.Member3, chr.Name, chr.Job, chr.Level, true);
            guildMember.Encode(pw);
            if (!GuildDbHandler.AddMember(guild.ID, chr.ID, guildMember.Rank))
            {
                chr.SendPacket(MessagePacket.RedText("Something went wrong. Please try again later."));
                return;
            }
            chr.GuildID = guild.ID;
            BroadcastPacket(pw, HandleMemberJoined);
            RemotePacket.SendCharacterGuildInfo(chr);
            chr.SendPacket(GuildPacket.GuildInfo(guild));
        }
        public static void HandleMemberJoined(Packet pr)
        {
            int guildId = pr.ReadInt();
            int cid = pr.ReadInt();
            GuildData guild = S.Guilds[guildId];
            GuildMember member = GuildMember.DecodeNew(pr);
            var chars = guild.Characters.ToList();
            guild.Members.Add(cid, member);
            foreach (var c in chars)
            {
                c.SendPacket(MessagePacket.RedText(string.Format("'{0}' has joined the guild", member.Name)));
                c.SendPacket(GuildPacket.GuildInfo(guild));
            }
        }
        public static void HandleDeclineInvitation(GameCharacter chr, Packet pr)
        {
            // 4D 2E 0A 00 73 65 77 69 6C 6E 6F 6F 6F 62 0A 00 73 65 77 69 6C 6E 6F 6F 6F 62
            byte type = pr.ReadByte();
            string name = pr.ReadString();
            string name2 = pr.ReadString();
            if (Invites.TryGetValue(chr.ID, out GuildInvite invite))
            {
                invite.Inviter.SendPacket(GuildPacket.InviteDecline(name));
                Invites.Remove(chr.ID);
            }
        }
        #endregion
        #region Leave/Expel
        public static void HandleLeave(GameCharacter chr)
        {
            SendMemberLeave(chr, chr.ID, chr.GuildID, chr.Name, false);
        }
        public static void HandleExpel(GameCharacter chr, int cid, string name)
        {
            SendMemberLeave(chr, cid, chr.GuildID, name, true);
        }
        public static void SendMemberLeave(GameCharacter chr, int cid, int guildId, string name, bool kicked)
        {
            if (!GuildDbHandler.RemoveMember(guildId, cid))
            {
                chr.SendPacket(MessagePacket.RedText("Something went wrong. Please try again later."));
                return;
            }
            var pw = new Packet(ISServerMessages.GuildMemberLeave);
            pw.WriteBool(kicked);
            pw.WriteInt(guildId);
            pw.WriteInt(cid);
            pw.WriteString(name);
            BroadcastPacket(pw, HandleMemberLeft);
        }
        public static void HandleMemberLeft(Packet pr)
        {
            bool kicked = pr.ReadBool();
            int guildId = pr.ReadInt();
            int charId = pr.ReadInt();
            string charName = pr.ReadString();
            
            GuildData guild = S.Guilds[guildId];
            GuildMemberActionType actionType = kicked ? GuildMemberActionType.MemberExpelled : GuildMemberActionType.MemberQuit;
            foreach (GameCharacter c in guild.Characters)
            {
                c.SendPacket(GuildPacket.GuildMemberAction(guildId, charId, charName, actionType));
            }
            guild.Members.Remove(charId);

            GameCharacter victim = S.GetCharacter(charId);
            if (victim != null)
            {
                victim.GuildID = 0;
                RemotePacket.SendCharacterGuildInfo(victim);
            }
        }
        #endregion
        #region Update level/job
        public static void SendUpdatePlayerJob(GameCharacter chr)
        {
            var pw = new Packet(ISServerMessages.GuildMemberJobLevelUpdate);
            pw.WriteInt(chr.GuildID);
            pw.WriteInt(chr.ID);
            pw.WriteInt(chr.Level);
            pw.WriteInt(chr.Job);
            BroadcastPacket(pw, HandleUpdatePlayerJob);
        }
        public static void HandleUpdatePlayerJob(Packet pr)
        {
            int guildId = pr.ReadInt();
            var guild = S.Guilds[guildId];
            int chrId = pr.ReadInt();
            int level = pr.ReadInt();
            int job = pr.ReadInt();
            foreach (var c in guild.Characters)
            {
                c.SendPacket(GuildPacket.UpdateMemberLevelJob(guildId, chrId, level, job));
            }
        }
        #endregion
        public static int SendExpandGuild(GameCharacter chr, int slots, int mesos)
        {
            try
            {
                if (!chr.IncMoney(mesos, MessageAppearType.ChatGrey)) return 0;
                if (!chr.IsGuildMaster) throw new Exception();
                int newCapacity = Math.Min(chr.Guild.Capacity + slots, GuildData.MAX_CAPACITY);
                if (!GuildDbHandler.UpdateGuildCapacity(chr.GuildID, newCapacity)) throw new Exception();

                var pw = new Packet(ISServerMessages.GuildExpand);
                pw.WriteInt(chr.GuildID);
                pw.WriteInt(newCapacity);
                BroadcastPacket(pw, HandleGuildExpand);
                chr.SendPacket(GuildPacket.NpcExpandSuccessMsg(chr.GuildID, (byte)newCapacity));
                return 1;
            }
            catch
            {
                chr.SendPacket(GuildPacket.NpcError(GuildNpcErrorType.NpcErrorExpandingGuild));
                return -1;
            }
        }
        public static void HandleGuildExpand(Packet pr)
        {
            int guildId = pr.ReadInt();
            int newCapacity = pr.ReadInt();
            S.Guilds[guildId].Capacity = newCapacity;
        }
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
                        {
                            string name = pr.ReadString();
                            HandleInvite(chr, name);
                            break;
                        }
                    case GuildAction.AcceptInvitation:
                        // 4C 06 14 00 00 00 09 00 00 00
                        int guildId = pr.ReadInt();
                        int charId = pr.ReadInt();
                        HandleAcceptInvitation(chr);
                        break;
                    case GuildAction.Leave:
                        {
                            // 4C 07 09 00 00 00 0A 00 73 65 77 69 6C 6E 6F 6F 6F 62
                            int cid = pr.ReadInt();
                            string name = pr.ReadString();
                            HandleLeave(chr);
                            break;
                        }
                    case GuildAction.Expel:
                        {
                            // 4C 08 09 00 00 00 0A 00 73 65 77 69 6C 6E 6F 6F 6F 62
                            int cid = pr.ReadInt();
                            string name = pr.ReadString();
                            HandleExpel(chr, cid, name);
                            break;
                        }
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
                    case GuildAction.SelectGuildEmblem:
                        {
                            GuildEmblem emblem = GuildEmblem.Decode(pr);
                            SendUpdateGuildEmblem(chr, emblem);
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
                e.SendPacket(chr);
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
