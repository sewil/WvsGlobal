using System.Collections.Generic;
using System.Linq;
using WvsBeta.Common.DataProviders;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Extensions;
using WvsBeta.Common.Sessions;
using static WvsBeta.Common.Constants.EquipSlots;

namespace WvsBeta.Common.Characters
{
    public class AvatarLook
    {
        public GW_CharacterStat CharacterStat { get; private set; } = new GW_CharacterStat();
        public byte Gender { get => CharacterStat.Gender; set => CharacterStat.Gender = value; }
        public byte Skin { get => CharacterStat.Skin; set => CharacterStat.Skin = value; }
        public int Face { get => CharacterStat.Face; set => CharacterStat.Face = value; }
        public int Hair { get => CharacterStat.Hair; set => CharacterStat.Hair = value; }
        public bool Messenger { get; set; }

        private readonly Dictionary<Slots, int> visibleEquips = new Dictionary<Slots, int>();
        private readonly Dictionary<Slots, int> hiddenEquips = new Dictionary<Slots, int>();
        private readonly int cashWeapon;

        public AvatarLook(Character character, bool messenger) : this(
            character.CharacterStat,
            character.Inventory?.Equipped[EquippedType.Cash].Select(i => new KeyValuePair<Slots,int>(i.Key, i.Value.ItemID)).ToDictionary(i => i.Key, i => i.Value),
            character.Inventory?.Equipped[EquippedType.Normal].Select(i => new KeyValuePair<Slots,int>(i.Key, i.Value.ItemID)).ToDictionary(i => i.Key, i => i.Value),
            messenger
        ) { }
        public AvatarLook(GW_CharacterStat cs, Dictionary<Slots, int> cashEquips, Dictionary<Slots, int> normalEquips, bool messenger)
        {
            CharacterStat = cs;
            Messenger = messenger;

            for (Slots slot = 0; slot < Slots._END; slot++)
            {
                int cashID = cashEquips.GetValue(slot);
                int normalID = normalEquips.GetValue(slot);
                if (slot != Slots.Weapon && cashID > 0)
                {
                    visibleEquips.Add(slot, cashID);
                    hiddenEquips.Add(slot, normalID);
                }
                else if (normalID > 0)
                {
                    visibleEquips.Add(slot, normalID);
                    hiddenEquips.Add(slot, cashID);
                }
                if (slot == Slots.Weapon) cashWeapon = cashID;
            }
        }

        public void Encode(Packet packet)
        {
            packet.WriteByte(Gender);
            packet.WriteByte(Skin);
            packet.WriteInt(Face);
            packet.WriteBool(Messenger);
            packet.WriteInt(Hair);

            foreach (var equip in visibleEquips)
            {
                packet.WriteByte((byte)equip.Key);
                packet.WriteInt(equip.Value);
            }

            packet.WriteSByte(-1);

            foreach (var equip in hiddenEquips)
            {
                packet.WriteByte((byte)equip.Key);
                packet.WriteInt(equip.Value);
            }

            packet.WriteSByte(-1);

            packet.WriteInt(cashWeapon);
            packet.WriteInt(visibleEquips.GetValue(Slots.PetAccessory));
        }

        public AvatarLook(Packet packet)
        {
            Gender = packet.ReadByte();
            Skin = packet.ReadByte();
            Face = packet.ReadInt();
            Messenger = packet.ReadBool();
            Hair = packet.ReadInt();

            byte slot = 0;
            while ((slot = packet.ReadByte()) != 0xFF)
            {
                var eqSlot = Constants.getEquipSlot(slot, out EquippedType _);
                visibleEquips.Add(eqSlot, packet.ReadInt());
            }

            while ((slot = packet.ReadByte()) != 0xFF)
            {
                var eqSlot = Constants.getEquipSlot(slot, out EquippedType _);
                hiddenEquips.Add(eqSlot, packet.ReadInt());
            }

            cashWeapon = packet.ReadInt();
            visibleEquips[Slots.PetAccessory] = packet.ReadInt();
        }
    }
}
