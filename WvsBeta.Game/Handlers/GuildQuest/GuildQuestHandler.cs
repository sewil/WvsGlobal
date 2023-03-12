using System.Collections.Generic;
using System.Linq;
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
        public static int Unregister(int guildId, bool force)
        {
            if (guildId == 0) return 0;
            var registrations = Server.Instance.GuildQuestRegistrations;
            GuildQuestRegistration registration = null;
            foreach (var item in registrations.Select(i => i.Value))
            {
                registration = item.FirstOrDefault(i => i.guildId == guildId);
                if (registration != null) break;
            }
            if (registration == null) return 0;
            else if (!force && registrations[registration.channelId].FindIndex(i => i.guildId == registration.guildId) == 0) return 0;

            var pw = new Packet(ISServerMessages.GuildQuestUnregister);
            registration.Encode(pw);
            Server.Instance.BroadcastPacket(pw, HandleUnregister);
            return 1;
        }
        public static void HandleUnregister(Packet pr)
        {
            var registration = GuildQuestRegistration.Decode(pr);
            var master = Server.Instance.GetCharacter(registration.master);
            if (master != null)
            {
                master.GPQRegistration = null;
            }
            foreach (var member in registration.members.Select(i => Server.Instance.GetCharacter(i)).Where(i => i != null))
            {
                member.GPQRegistration = null;
            }
            Server.Instance.GuildQuestRegistrations[registration.channelId].Remove(registration);
        }
    }
}
