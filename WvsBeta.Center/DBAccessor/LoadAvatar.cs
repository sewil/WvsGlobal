using System.Collections.Generic;
using WvsBeta.Common;
using WvsBeta.Common.Characters;
using WvsBeta.Common.Enums;
using static WvsBeta.Common.Constants.EquipSlots;

namespace WvsBeta.Center.DBAccessor
{
    public partial class CharacterDBAccessor
    {
        public static AvatarData LoadAvatar(int characterId)
        {
            var ad = new AvatarData();

            ad.CharacterStat = GetCharacterData(characterId);

            var equips = new Dictionary<Slots, int>();
            var equipsCash = new Dictionary<Slots, int>();

            foreach (var (itemid, slot) in GetEquippedItemID(characterId))
            {
                var eqSlot = Constants.getEquipSlot(slot, out EquippedType type);
                if (eqSlot == Slots.Invalid) continue;
                if (type == EquippedType.Cash)
                {
                    equipsCash[eqSlot] = itemid;
                }
                else
                {
                    equips[eqSlot] = itemid;
                }
            }

            ad.AvatarLook = new AvatarLook(ad.CharacterStat, equipsCash, equips, false);

            return ad;
        }
    }
}
