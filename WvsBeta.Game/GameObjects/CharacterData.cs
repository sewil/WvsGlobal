using System.Linq;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Game.GameObjects
{
    class CharacterData : Common.Objects.CharacterData
    {
        protected new GameCharacter Character => (GameCharacter)base.Character;
        public CharacterData(GameCharacter chr) : base(chr)
        {
        }
        public override void Encode(Packet packet, CharacterDataFlag flags = CharacterDataFlag.All)
        {
            base.Encode(packet, flags);

            if ((flags & CharacterDataFlag.AnyQuests) != 0)
            {
                var quests = Character.Quests.GetQuests();
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
                Character.GameStats.Encode(packet);
            }

            if (flags.HasFlag(CharacterDataFlag.FriendRing))
            {
                var rings = Character.Rings.Rings;
                packet.WriteShort((short)rings.Count); // ?
                foreach (var ring in rings)
                {
                    ring.Encode(packet);
                }
            }

            if (flags.HasFlag(CharacterDataFlag.TeleportRock)) // 5 locations
            {
                Character.Inventory.AddRockPacket(packet);
            }
        }
    }
}
