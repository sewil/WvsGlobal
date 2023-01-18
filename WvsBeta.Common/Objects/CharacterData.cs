using WvsBeta.Common;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Game.Packets
{
    public struct CharacterData
    {
        private readonly CharacterBase chr;
        public CharacterData(CharacterBase chr)
        {
            this.chr = chr;
        }
        public void Encode(Packet packet)
        {
            var flags = CharacterDataFlag.All;
            packet.WriteShort((short)flags);

            if (flags.HasFlag(CharacterDataFlag.Stats))
            {
                chr.ToGWStat().Encode(packet);
                packet.WriteByte((byte)chr.PrimaryStats.BuddyListCapacity);
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
                    packet.WriteInt(kvp.Key);
                    packet.WriteString(kvp.Value.Data);
                }
            }

            if (flags.HasFlag(CharacterDataFlag.MinigameStats))
            {
                packet.WriteShort(0); // RPS Game(s)
            }

            if (flags.HasFlag(CharacterDataFlag.FriendRing))
            {
                packet.WriteShort(0); // ?
            }

            if (flags.HasFlag(CharacterDataFlag.Something33U))
            {
                packet.WriteShort(0); // ?, decodes 33u buffer wut
            }

            if (flags.HasFlag(CharacterDataFlag.TeleportRock))
            {
                chr.Inventory.AddRockPacket(packet);
            }
        }
    }
}
