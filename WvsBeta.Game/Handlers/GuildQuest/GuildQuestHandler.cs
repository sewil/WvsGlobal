using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using WvsBeta.Common;
using WvsBeta.Common.Sessions;
using WvsBeta.Game.Handlers.Guild;

namespace WvsBeta.Game.Handlers.GuildQuest
{
    public static class GuildQuestHandler
    {
        public static int Register(GameCharacter chr)
        {
            if (chr.Guild == null || !chr.CanChangeRank) return 0;
            else if (chr.GPQRegistration != null) return -(chr.GPQRegistration.channelId + 1);

            int[] members = chr.Guild.Characters.Where(i => i.Field.ID == chr.Field.ID && i.ID != chr.ID).Select(i => i.ID).ToArray();
            var pw = new Packet(ISServerMessages.GuildQuestRegister);
            var registration = new GuildQuestRegistration(Server.Instance.ID, chr.GuildID, chr.ID, members);
            registration.Encode(pw);

            Server.Instance.BroadcastPacket(pw, HandlerRegister);
            return 1;
        }
        public static void HandlerRegister(Packet pr)
        {
            var registration = GuildQuestRegistration.Decode(pr);
            var master = Server.Instance.GetCharacter(registration.master);
            if (master != null)
            {
                master.GPQRegistration = registration;
            }
            foreach (var member in registration.members.Select(i => Server.Instance.GetCharacter(i)).Where(i => i != null))
            {
                member.GPQRegistration = registration;
            }
            var list = Server.Instance.GuildQuestRegistrations;
            if (!list.ContainsKey(registration.channelId))
            {
                list.Add(registration.channelId, new List<GuildQuestRegistration>());
            }
            list[registration.channelId].Add(registration);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="registration"></param>
        /// <returns>1 = cancelled, 0 = its your turn so you cant cancel</returns>
        public static int Unregister(GuildQuestRegistration registration, bool force)
        {
            if (registration == null) return 1;
            int guildId = registration.guildId;
            byte channelId = registration.channelId;
            var idx = Server.Instance.GuildQuestRegistrations[channelId].FindIndex(i => i.guildId == guildId);
            if (!force && idx == 0) return 0; // First in queue, can't cancel

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
            if (!Server.Instance.GuildQuestRegistrations.TryGetValue(channelId, out var registrations)) return;
            int idx = registrations.FindIndex(i => i.guildId == guildId);
            if (idx == -1) return;
            var registration = registrations[idx];

            var master = Server.Instance.GetCharacter(registration.master);
            if (master != null) master.GPQRegistration = null;
            foreach (var member in registration.members.Select(item => Server.Instance.GetCharacter(item)).Where(item => item != null))
            {
                member.GPQRegistration = null;
            }
            registrations.RemoveAt(idx);
        }
    }
}
