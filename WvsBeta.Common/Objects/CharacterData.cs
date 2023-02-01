using System.Runtime.InteropServices;
using WvsBeta.Common;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Common.Objects
{
    public struct CharacterData
    {
        private readonly CharacterBase chr;
        public CharacterData(CharacterBase chr)
        {
            this.chr = chr;
        }
        public void Encode(Packet packet, CharacterDataFlag flags = CharacterDataFlag.All)
        {
            packet.WriteShort((short)flags);

            if (flags.HasFlag(CharacterDataFlag.Stats))
            {
                chr.CharacterStat.Encode(packet);
                packet.WriteByte(chr.BuddyListCapacity);
            }

            chr.Inventory.GenerateInventoryPacket(packet, flags);

            if (flags.HasFlag(CharacterDataFlag.Skills))
            {
                chr.Skills.AddSkills(packet);
            }

            if (flags.HasFlag(CharacterDataFlag.Quests))
            {
                var questsWithData = chr.Quests.GetWZExistingQuests();
                packet.WriteShort((short)questsWithData.Count); // Running quests
                foreach (var kvp in questsWithData)
                {
                    packet.WriteShort((short)kvp.Key);
                    packet.WriteString(kvp.Value.Data);
                }
            }

            if (flags.HasFlag(CharacterDataFlag.Unk4000))
            {
                packet.WriteShort(0);
            }

            if (flags.HasFlag(CharacterDataFlag.MinigameStats))
            {
                packet.WriteShort(0); // RPS Game(s)
            }

            if (flags.HasFlag(CharacterDataFlag.FriendRing))
            {
                packet.WriteShort(0); // ?
            }

            if (flags.HasFlag(CharacterDataFlag.TeleportRock)) // 5 locations
            {
                chr.Inventory.AddRockPacket(packet);
            }
        }
    }
}
