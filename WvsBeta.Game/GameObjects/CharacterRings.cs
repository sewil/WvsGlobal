using System.Collections.Generic;
using MySql.Data.MySqlClient;
using WvsBeta.Common;
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
            pw.WriteInt(RingID);
            pw.WriteInt(0);
            pw.WriteInt(PartnerRingID);
            pw.WriteInt(0);
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
                    bool equipped = (chr.Inventory.GetEquippedItemId((short)Constants.EquipSlots.Slots.Ring1, true) == 1112001) ||
                        (chr.Inventory.GetEquippedItemId((short)Constants.EquipSlots.Slots.Ring2, true) == 1112001) ||
                        (chr.Inventory.GetEquippedItemId((short)Constants.EquipSlots.Slots.Ring3, true) == 1112001) ||
                        (chr.Inventory.GetEquippedItemId((short)Constants.EquipSlots.Slots.Ring4, true) == 1112001);
                    rings.Rings.Add(new Ring
                    {
                        RingID = data.GetInt32("id"),
                        ItemID = data.GetInt32("itemid"),
                        CharacterID = chr.ID,
                        PartnerRingID = data.GetInt32("partnerid"),
                        Equipped = equipped
                    });
                }
            }
            return rings;
        }
    }
}
