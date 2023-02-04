using System.Linq;
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
        
        public int[] UnseenEquip { get; } = new int[Constants.EquipSlots.MaxSlotIndex];
        public int[] HairEquip { get; } = new int[Constants.EquipSlots.MaxSlotIndex];
        public bool CashInUnseen { get; private set; }

        public AvatarLook(CharacterBase character, bool putCashInUnseen = false) : this(
            character.CharacterStat,
            character.Inventory?.Equipped[Enums.EquippedVisibility.Visible].Select(i => i?.ItemID ?? 0).ToArray() ?? new int[Constants.EquipSlots.MaxSlotIndex],
            character.Inventory?.Equipped[Enums.EquippedVisibility.Hidden].Select(i => i?.ItemID ?? 0).ToArray() ?? new int[Constants.EquipSlots.MaxSlotIndex],
            putCashInUnseen
        ) { }
        public AvatarLook(GW_CharacterStat cs, int[] equips, int[] equipsCash, bool putCashInUnseen)
        {
            CharacterStat = cs;
            HairEquip[0] = cs.Hair;
            CashInUnseen = putCashInUnseen;

            if (putCashInUnseen)
            {
                for (byte i = 1; i < Constants.EquipSlots.MaxSlotIndex; i++)
                {
                    HairEquip[i] = equips[i];
                    UnseenEquip[i] = equipsCash[i];
                }
            }
            else
            {
                for (byte i = 1; i < Constants.EquipSlots.MaxSlotIndex; i++)
                {
                    bool isCash = equipsCash[i] != 0;
                    int hairId = 0;
                    int unseenId = 0;
                    if (putCashInUnseen || !isCash)
                    {
                        hairId = equips[i];
                        unseenId = equipsCash[i];
                    }
                    else
                    {
                        hairId = equipsCash[i];
                        unseenId = equips[i];
                    }
                    HairEquip[i] = hairId;
                    UnseenEquip[i] = unseenId;
                }
            }

            WeaponStickerID = equipsCash[(byte)Constants.EquipSlots.Slots.Weapon];
            //PetItemId = equipsCash[(byte)Constants.EquipSlots.Slots.pet]
        }


        public void Encode(Packet packet)
        {
            packet.WriteByte(Gender);
            packet.WriteByte(Skin);
            packet.WriteInt(Face);

            packet.WriteByte(0); // First slot
            packet.WriteInt(HairEquip[0]); // First equip
            // Note: this could use i = 0, but for the sake of clarity, we do not do that
            // Because also the client doesn't go from zero.
            for (byte i = 1; i < Constants.EquipSlots.MaxSlotIndex; i++)
            {
                int itemid = HairEquip[i];
                if (itemid == 0) continue;

                packet.WriteByte(i);
                packet.WriteInt(itemid);
            }
            packet.WriteSByte(-1);

            for (byte i = 1; i < Constants.EquipSlots.MaxSlotIndex; i++)
            {
                int itemid = UnseenEquip[i];
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
            CashInUnseen = false;
            for (var i = HairEquip.Length - 1; i >= 0; i--)
                HairEquip[i] = 0;
            for (var i = UnseenEquip.Length - 1; i >= 0; i--)
                UnseenEquip[i] = 0;

            Gender = packet.ReadByte();
            Skin = packet.ReadByte();
            Face = packet.ReadInt();

            byte slot = 0;
            while ((slot = packet.ReadByte()) != 0xFF)
            {
                HairEquip[slot] = packet.ReadInt();
            }

            while ((slot = packet.ReadByte()) != 0xFF)
            {
                UnseenEquip[slot] = packet.ReadInt();
            }

            WeaponStickerID = packet.ReadInt();
            PetItemId = packet.ReadInt();
        }
    }
}
