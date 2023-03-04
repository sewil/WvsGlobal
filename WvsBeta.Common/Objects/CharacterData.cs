using System.Linq;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Common.Objects
{
    public class CharacterData
    {
        protected virtual CharacterBase Character { get; private set; }
        public CharacterData(CharacterBase chr)
        {
            Character = chr;
        }
        public virtual void Encode(Packet packet, CharacterDataFlag flags = CharacterDataFlag.All)
        {
            packet.WriteShort((short)flags);

            if (flags.HasFlag(CharacterDataFlag.Stats))
            {
                Character.CharacterStat.Encode(packet);
                packet.WriteByte(Character.BuddyListCapacity);
            }

            Character.Inventory.GenerateInventoryPacket(packet, flags);

            if (flags.HasFlag(CharacterDataFlag.Skills))
            {
                Character.Skills.AddSkills(packet);
            }

            if ((flags & CharacterDataFlag.AnyQuests) != 0)
            {
                var quests = Character.BaseQuests.GetQuests();
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
        }
    }
}
