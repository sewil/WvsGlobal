using System.Linq;
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

            if ((flags & CharacterDataFlag.AnyQuests) != 0)
            {
                var quests = chr.BaseQuests.GetQuests();
                if (flags.HasFlag(CharacterDataFlag.Quests))
                {
                    var questsInProgress = quests.Where(q => q.Value.State == QuestState.InProgress).ToList();
                    packet.WriteShort((short)questsInProgress.Count); // Running quests
                    foreach (var quest in questsInProgress)
                    {
                        packet.WriteShort(quest.Key);
                        packet.WriteString(quest.Value.Data);
                    }
                }

                if (flags.HasFlag(CharacterDataFlag.CompletedQuests))
                {
                    var completedQuests = quests.Where(q => q.Value.State == QuestState.Completed).ToList();
                    packet.WriteShort((short)completedQuests.Count); // Completed quests
                    foreach (var completedQuest in completedQuests)
                    {
                        packet.WriteShort(completedQuest.Key);
                        packet.WriteLong(completedQuest.Value.FileTime);
                    }
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

            if (flags.HasFlag(CharacterDataFlag.TeleportRock)) // 5 locations
            {
                chr.Inventory.AddRockPacket(packet);
            }
        }
    }
}
