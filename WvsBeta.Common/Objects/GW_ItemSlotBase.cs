using System;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Common.Objects
{
    public enum ItemSlotType
    {
        Equip = 1,
        Bundle = 2,
        Pet = 3,
        Cash = 4,
    }
    public class GW_ItemSlotBase
    {
        private BaseItem item;
        public GW_ItemSlotBase(BaseItem item)
        {
            this.item = item;
        }
        public void Encode(Packet packet, bool writeSlot, bool shortSlot)
        {
            if (writeSlot)
            {
                short slot = item.InventorySlot;
                if (slot != 0)
                {
                    if (shortSlot)
                    {
                        packet.WriteShort(slot);
                    }
                    else
                    {
                        slot = Math.Abs(slot);
                        if (slot > 100) slot -= 100;
                        packet.WriteByte((byte)slot);
                    }
                }
            }
            var slotType = Constants.getItemSlotType(item.ItemID);
            packet.WriteByte((byte)slotType);

            item.Encode(packet);
        }
    }
}
