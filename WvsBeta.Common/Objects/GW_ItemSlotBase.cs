using System;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Common.Objects
{
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
            var itemType = item.ItemType;
            if (itemType == ItemType.Equip)
            {
                packet.WriteByte(1);
            }
            else if (itemType == ItemType.Pet)
            {
                packet.WriteByte(3);
            }
            else
            {
                packet.WriteByte(2);
            }

            item.Encode(packet);
        }
    }
}
