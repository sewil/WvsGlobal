using reNX.NXProperties;
using System;
using WvsBeta.Common.Objects;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Shop.GameObjects
{
    public class CommodityInfo : ICommodityInfo
    {
        public int SerialNumber { get; }
        public int ItemID { get; }
        public short Count { get; }
        public short Period { get; }
        public bool OnSale { get; }
        public int Price { get; }
        public int Priority { get; }
        public CommodityGenders Gender { get; }
        public CommodityClass Class { get; }
        public StockState StockState { get; }
        public CommodityCategory Category { get; }

        public CommodityInfo(NXNode node)
        {
            SerialNumber = node["SN"].ValueInt32();
            ItemID = node["ItemId"].ValueInt32();
            Count = node["Count"].ValueInt16();
            Gender = (CommodityGenders)node["Gender"].ValueSByte();
            Period = node["Period"].ValueInt16();
            OnSale = node["OnSale"].ValueBool();
            Price = node["Price"].ValueInt16();
            Priority = node["Priority"].ValueInt32();
            StockState = StockState.DefaultState;
            Category = (CommodityCategory)(Math.Floor((double)SerialNumber / 10000000) + 1);
            if (node.ContainsChild("Class"))
            {
                Class = (CommodityClass)node["Class"].ValueInt32();
            }

            if (!ShopDataProvider.Items.ContainsKey(ItemID) && !ShopDataProvider.Equips.ContainsKey(ItemID) && !ShopDataProvider.Pets.ContainsKey(ItemID))
            {
                Program.MainForm.LogAppend("Ignoring commodity SN {0} as it contains unknown itemid {1}", SerialNumber, ItemID);
                OnSale = false;
                StockState = StockState.NotAvailable;
            }
            if (Price == 18000 && OnSale)
            {
                Program.MainForm.LogAppend("Making SN {0} itemid {1} not OnSale because its price is 18k", SerialNumber, ItemID);
                OnSale = false;
                StockState = StockState.NotAvailable;
            }
        }

        public void Encode(Packet packet)
        {
            packet.WriteInt(SerialNumber);
            packet.WriteInt(ItemID);
            packet.WriteInt(Count); // Count
            packet.WriteInt(Priority); // Priority
            packet.WriteInt(Price); // Discount price
            packet.WriteInt(Period); // Period
            packet.WriteInt(0); // ?

            packet.WriteInt(0); // ?
            packet.WriteBool(false); // Disable buy

            packet.WriteInt((int)Gender); // Gender
            packet.WriteBool(OnSale); // On sale

            packet.WriteInt((int)Class); // Class
            packet.WriteBool(false); // Limited time

            // ?
            var v5 = 0;
            packet.WriteInt(v5);
            for (int i = 0; i < v5; i++)
            {
                packet.WriteInt(0);
            }

            var result = 0;
            packet.WriteInt(result);
            for (int i = 0; i < result; i++)
            {
                packet.WriteInt(0);
            }
        }
    }
}
