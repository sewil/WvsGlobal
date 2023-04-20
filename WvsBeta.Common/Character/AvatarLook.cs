using System.Collections.Generic;
using System.Linq;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Extensions;
using WvsBeta.Common.Sessions;
using static WvsBeta.Common.Constants.EquipSlots;

namespace WvsBeta.Common.Character
{
    public class AvatarLook
    {
        public GW_CharacterStat CharacterStat { get; private set; } = new GW_CharacterStat();
        public byte Gender { get => CharacterStat.Gender; set => CharacterStat.Gender = value; }
        public byte Skin { get => CharacterStat.Skin; set => CharacterStat.Skin = value; }
        public int Face { get => CharacterStat.Face; set => CharacterStat.Face = value; }
        public int Hair { get => CharacterStat.Hair; set => CharacterStat.Hair = value; }
        public bool Messenger { get; set; }

        public readonly Dictionary<Slots, int> CashEquips = new Dictionary<Slots, int>();
        public readonly Dictionary<Slots, int> NormalEquips = new Dictionary<Slots, int>();

        public AvatarLook(CharacterBase character, bool messenger) : this(
            character.CharacterStat,
            character.Inventory?.Equipped[EquippedType.Cash].Select(i => new KeyValuePair<Slots,int>(i.Key, i.Value.ItemID)).ToDictionary(i => i.Key, i => i.Value),
            character.Inventory?.Equipped[EquippedType.Normal].Select(i => new KeyValuePair<Slots,int>(i.Key, i.Value.ItemID)).ToDictionary(i => i.Key, i => i.Value),
            messenger
        ) { }
        public AvatarLook(GW_CharacterStat cs, Dictionary<Slots, int> cashEquips, Dictionary<Slots, int> normalEquips, bool messenger)
        {
            CharacterStat = cs;
            CashEquips = cashEquips;
            NormalEquips = normalEquips;
            Messenger = messenger;
        }

        public void Encode(Packet packet)
        {
            packet.WriteByte(Gender);
            packet.WriteByte(Skin);
            packet.WriteInt(Face);
            packet.WriteBool(Messenger);
            packet.WriteInt(Hair);

            foreach (var item in NormalEquips)
            {
                packet.WriteByte((byte)item.Key);
                packet.WriteInt(item.Value);
            }

            foreach (var item in CashEquips.Where(i => i.Key >= Slots.Ring1))
            {
                packet.WriteByte((byte)item.Key);
                packet.WriteInt(item.Value);
            }

            packet.WriteSByte(-1);

            foreach (var item in CashEquips.Where(i => i.Key < Slots.Ring1))
            {
                if (item.Key == Slots.Weapon) continue;
                packet.WriteByte((byte)item.Key);
                packet.WriteInt(item.Value);
            }

            packet.WriteSByte(-1);

            packet.WriteInt(CashEquips.GetValue(Slots.Weapon));
            packet.WriteInt(CashEquips.GetValue(Slots.PetAccessory));
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
                if (eqSlot >= Slots.Ring1)
                {
                    CashEquips[eqSlot] = packet.ReadInt();
                }
                else
                {
                    NormalEquips[eqSlot] = packet.ReadInt();
                }
            }

            while ((slot = packet.ReadByte()) != 0xFF)
            {
                var eqSlot = Constants.getEquipSlot(slot, out EquippedType _);
                CashEquips[eqSlot] = packet.ReadInt();
            }

            CashEquips[Slots.Weapon] = packet.ReadInt();
            CashEquips[Slots.PetAccessory] = packet.ReadInt();
        }
    }
}
