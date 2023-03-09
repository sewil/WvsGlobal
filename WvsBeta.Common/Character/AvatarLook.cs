using System.Linq;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Common.Character
{
    public class AvatarLook
    {
        public GW_CharacterStat CharacterStat { get; private set; } = new GW_CharacterStat();
        public byte Gender { get => CharacterStat.Gender; set => CharacterStat.Gender = value; }
        public byte Skin { get => CharacterStat.Skin; set => CharacterStat.Skin = value; }
        public int Face { get => CharacterStat.Face; set => CharacterStat.Face = value; }
        public int PetItemId { get; set; }
        public int WeaponStickerID { get; set; }
        
        public int[] NormalEquips { get; } = new int[Constants.EquipSlots.MaxSlotIndex];
        public int[] CashEquips { get; } = new int[Constants.EquipSlots.MaxSlotIndex];

        public AvatarLook(CharacterBase character) : this(
            character.CharacterStat,
            character.Inventory?.Equipped[EquippedType.Normal].Select(i => i?.ItemID ?? 0).ToArray() ?? new int[Constants.EquipSlots.MaxSlotIndex],
            character.Inventory?.Equipped[EquippedType.Cash].Select(i => i?.ItemID ?? 0).ToArray() ?? new int[Constants.EquipSlots.MaxSlotIndex]
        ) { }
        public AvatarLook(GW_CharacterStat cs, int[] equips, int[] cashEquips)
        {
            CharacterStat = cs;
            CashEquips[0] = cs.Hair;

            for (byte i = 1; i < Constants.EquipSlots.MaxSlotIndex; i++)
            {
                bool isCash = cashEquips[i] != 0;
                CashEquips[i] = isCash ? cashEquips[i] : equips[i];
                NormalEquips[i] = isCash ? equips[i] : cashEquips[i];
            }

            WeaponStickerID = cashEquips[(byte)Constants.EquipSlots.Slots.Weapon];
            //PetItemId = equipsCash[(byte)Constants.EquipSlots.Slots.pet]
        }


        public void Encode(Packet packet)
        {
            packet.WriteByte(Gender);
            packet.WriteByte(Skin);
            packet.WriteInt(Face);

            packet.WriteByte(0); // First slot
            packet.WriteInt(CashEquips[0]); // First equip
            // Note: this could use i = 0, but for the sake of clarity, we do not do that
            // Because also the client doesn't go from zero.
            for (byte i = 1; i < Constants.EquipSlots.MaxSlotIndex; i++)
            {
                int itemid = CashEquips[i];
                if (itemid == 0) continue;

                packet.WriteByte(i);
                packet.WriteInt(itemid);
            }
            packet.WriteSByte(-1);

            for (byte i = 1; i < Constants.EquipSlots.MaxSlotIndex; i++)
            {
                int itemid = NormalEquips[i];
                if (itemid == 0) continue;

                packet.WriteByte(i);
                packet.WriteInt(itemid);
            }
            packet.WriteSByte(-1);

            packet.WriteInt(WeaponStickerID);
            packet.WriteInt(PetItemId);
        }

        public AvatarLook(Packet packet)
        {
            for (var i = CashEquips.Length - 1; i >= 0; i--)
                CashEquips[i] = 0;
            for (var i = NormalEquips.Length - 1; i >= 0; i--)
                NormalEquips[i] = 0;

            Gender = packet.ReadByte();
            Skin = packet.ReadByte();
            Face = packet.ReadInt();

            byte slot = 0;
            while ((slot = packet.ReadByte()) != 0xFF)
            {
                CashEquips[slot] = packet.ReadInt();
            }

            while ((slot = packet.ReadByte()) != 0xFF)
            {
                NormalEquips[slot] = packet.ReadInt();
            }

            WeaponStickerID = packet.ReadInt();
            PetItemId = packet.ReadInt();
        }
    }
}
