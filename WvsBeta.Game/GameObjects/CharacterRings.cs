using System.Collections.Generic;
using MySql.Data.MySqlClient;
using WvsBeta.Common;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Objects;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Game
{
    public class Ring
    {
        public int RingID { get; set; }
        public int CashID { get; set; }
        public int InventorySlot { get; set; }
        public int ItemID { get; set; }
        public int CharacterID { get; set; }
        public int PartnerRingID { get; set; }
        public bool Equipped { get; set; }
        public void Encode(Packet pw)
        {
            if (!Equipped) return;
            // 33U
            pw.WriteLong(RingID);
            pw.WriteLong(PartnerRingID);
            pw.WriteInt(ItemID);
        }
    }
    public class CharacterRings
    {
        public readonly List<Ring> Rings;

        public CharacterRings()
        {
            Rings = new List<Ring>();
        }

        public void Encode(Packet pw)
        {
            pw.WriteByte((byte)Rings.Count);
            foreach (var ring in Rings)
            {
                ring.Encode(pw);
            }
        }

        public static CharacterRings Load(GameCharacter chr)
        {
            var rings = new CharacterRings();
            using (var data = (MySqlDataReader)Server.Instance.CharacterDatabase.RunQuery(
                "SELECT * FROM rings WHERE charid = @charid",
                "@charid", chr.ID
            ))
            {
                while (data.Read())
                {
                    BaseItem eq = chr.Inventory.GetEquippedItem(EquipIds.CrushRing, out EquippedType _);
                    bool isEquipped = eq != null && (short)Constants.EquipSlots.Slots.Ring1 <= eq.InventorySlot && eq.InventorySlot <= (short)Constants.EquipSlots.Slots.Ring4;
                    rings.Rings.Add(new Ring
                    {
                        RingID = data.GetInt32("id"),
                        ItemID = data.GetInt32("itemid"),
                        CharacterID = chr.ID,
                        PartnerRingID = data.GetInt32("partnerid"),
                        Equipped = isEquipped
                    });
                }
            }
            return rings;
        }
    }
}
