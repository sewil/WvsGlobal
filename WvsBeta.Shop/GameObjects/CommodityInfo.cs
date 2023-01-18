using WvsBeta.Common.Sessions;

namespace WvsBeta.Shop
{
    public enum StockState
    {
        InStock = -1,
        DefaultState = 0,
        OutOfStock = 1,
        NotAvailable = 2,
    }

    public class CommodityInfo
    {
        public int SerialNumber { get; set; }
        public int ItemID { get; set; }
        public short Count { get; set; }
        public short Period { get; set; }
        public bool OnSale { get; set; }
        public int Price { get; set; }
        public CommodityGenders Gender { get; set; }

        public StockState StockState { get; set; } = StockState.DefaultState;

        public void Encode(Packet packet)
        {
            packet.WriteInt(SerialNumber);
            packet.WriteInt(ItemID);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(0);

            packet.WriteInt(0);
            packet.WriteByte(0);
            
            packet.WriteInt(0);
            packet.WriteByte(0);
            
            packet.WriteInt(0);
            packet.WriteByte(0);
            
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