using System.Linq;
using WvsBeta.Common.Characters;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Common.Objects
{
    public class CharacterData
    {
        protected virtual Character Character { get; private set; }
        public CharacterData(Character chr)
        {
            Character = chr;
        }
        public virtual void Encode(Packet packet, CharacterDataFlag flags)
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
        }
    }
}
