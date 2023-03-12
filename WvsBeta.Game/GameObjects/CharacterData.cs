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
