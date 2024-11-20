using System.Collections.Generic;
using System.Linq;
using WvsBeta.Common;
using WvsBeta.Common.Extensions;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Game.Handlers.GuildQuest
{
    public static class GuildQuestHandler
    {
        public static int Register(GameCharacter chr)
        {
            var gd = chr.Guild;
            var gid = chr.GuildID;
            var fs = FieldSet.Instances["Guild1"];
            if (chr.GPQRegistration != null) return -(chr.GPQRegistration.ChannelID + 1);
            else if (!chr.CanChangeRank || !fs.CheckLevel(chr)) return 0;

            var pw = new Packet(ISServerMessages.GuildQuestRegister);
            var registration = new GuildQuestRegistration(Server.Instance.ID, chr.GuildID, chr.ID);
            registration.Encode(pw);

            Server.Instance.BroadcastPacket(pw, HandleRegister);
            NotifyRegistration(registration, registration.QueueIndex);

            TryStartFieldSet(chr);

            return 1;
        }
        public static void HandleRegister(Packet pr)
        {
            var registration = GuildQuestRegistration.Decode(pr);
            if (registration.Guild != null)
            {
                registration.Guild.GPQRegistration = registration;
            }
            var list = Server.Instance.GuildQuestRegistrations;
            if (!list.ContainsKey(registration.ChannelID))
            {
                list.Add(registration.ChannelID, new List<GuildQuestRegistration>());
            }
            list[registration.ChannelID].Add(registration);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="registration"></param>
        /// <returns>1 = cancelled, 0 = its your turn so you cant cancel</returns>
        public static int Unregister(GuildQuestRegistration registration, bool force)
        {
            if (registration == null) return 1;
            int guildId = registration.GuildID;
            byte channelId = registration.ChannelID;
            var first = registration.QueueIndex == 0;
            if (!force && first) return 0; // First in queue, can't cancel

            var pw = new Packet(ISServerMessages.GuildQuestUnregister);
            pw.WriteInt(guildId);
            pw.WriteByte(channelId);
            Server.Instance.BroadcastPacket(pw, HandleUnregister);

            return 1;
        }
        public static void HandleUnregister(Packet pr)
        {
            int guildId = pr.ReadInt();
            byte channelId = pr.ReadByte();
            if (!Server.Instance.GuildQuestRegistrations.TryGetValue(channelId, out var channelRegistrations)) return;
            int idx = channelRegistrations.FindIndex(i => i.GuildID == guildId);
            if (idx == -1) return;
            var registration = channelRegistrations[idx];
            if (registration.Guild != null)
            {
                registration.Guild.GPQRegistration = null;
            }
            channelRegistrations.RemoveAt(idx);

            // The registration was on this channel and was first in line
            if (registration.ChannelID == Server.Instance.ID && idx == 0)
            {
                // Attempt to find the next registration in line (if any)
                StartNextRegistration(channelRegistrations);
            }

            NotifyChannelRegistrations(registration.ChannelID);
        }

        private static void StartNextRegistration(List<GuildQuestRegistration> channelRegistrations)
        {
            var nextRegistration = channelRegistrations.FirstOrDefault();
            if (nextRegistration == null) return;
            var leader = Server.Instance.GetCharacter(nextRegistration.Leader);
            if (leader == null || !leader.IsOnline)
            {
                channelRegistrations.RemoveAt(0);
                StartNextRegistration(channelRegistrations);
            }
            else
            {
                TryStartFieldSet(leader);
            }
        }

        private static void NotifyChannelRegistrations(byte channelID)
        {
            var registrations = Server.Instance.GuildQuestRegistrations.GetValue(channelID);
            for (int i = 0; i < (registrations?.Count ?? 0); i++)
            {
                var registration = registrations[i];
                NotifyRegistration(registration, i);
            }
        }

        private static void NotifyRegistration(GuildQuestRegistration registration, int idx)
        {
            string message;
            string channelName = $"{registration.ChannelID + 1}";
            if (idx == 0) message = $"Please go see the Guild Quest NPC at Channel {channelName} immediately to enter.";
            else if (idx == 1) message = $"Your guild is up next. Please head to the Guild Quest map at Channel {channelName} and wait.";
            else message = $"There's currently 1 guild participating in the Guild Quest, and your guild is number {idx + 1} on the waitlist.";
            Server.Instance.GetCharacter(registration.Leader)?.Message(message);
        }

        /// <summary>
        /// Attempts to start the GPQ fieldset. If there are no registrations queued, or if the fieldset is already started, nothing will happen. The registration is only set when the owner Enters, and only the owner of the first registration in queue can enter the fieldset.
        /// </summary>
        private static void TryStartFieldSet(GameCharacter owner)
        {
            var r = Server.Instance.GuildQuestChannelRegistrations;
            if ((r?.Count ?? 0) == 0) return;
            var fs = FieldSet.Instances["Guild1"];
            fs.Start(owner);
        }
    }
}
