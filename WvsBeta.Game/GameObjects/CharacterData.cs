using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using WvsBeta.Common;
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
                packet.WriteShort(0); // ?
            }

            if (flags.HasFlag(CharacterDataFlag.TeleportRock)) // 5 locations
            {
                Character.Inventory.AddRockPacket(packet);
            }
        }
    }
}
