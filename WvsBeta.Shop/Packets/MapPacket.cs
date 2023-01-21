using System.Collections.Generic;
using System.Linq;
using WvsBeta.Common.Sessions;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Objects;
using WvsBeta.Common;

namespace WvsBeta.Shop
{

    public static class MapPacket
    {
        public static void SendJoinCashServer(Character chr)
        {
            Packet pack = new Packet(ServerMessages.SET_CASH_SHOP);
            var flags = (
                CharacterDataFlag.Stats
                |CharacterDataFlag.Money
                |CharacterDataFlag.MaxSlots
                |CharacterDataFlag.Items
            );

            new CharacterData(chr).Encode(pack, flags);

            bool doEncode = true;
            pack.WriteBool(doEncode);
            if (doEncode)
            {
                pack.WriteString(chr.UserName);
            }

            List<CommodityInfo> commodityItems = DataProvider.Commodity.Where(x => x.Value.OnSale == false).Select(i => i.Value).ToList();
            pack.WriteShort((short)commodityItems.Count);
            commodityItems.ForEach(i => i.Encode(pack));

            //// Newer versions will have discount-per-category stuff here
            //// byte amount, foreach { byte category, byte categorySub, byte discountRate  }

            // Categories
            for (byte cat = 1; cat <= 8; cat++)
            {
                var sales = Server.Instance.BestItems.Where(i => (int)i.Category == cat).Take(5).ToList();
                // Gender (0 = male, 1 = female)
                for (byte gn = 0; gn <= 1; gn++)
                {
                    // Top 5 items
                    for (byte pos = 0; pos < 5; pos++)
                    {
                        pack.WriteInt(cat);
                        pack.WriteInt(gn);

                        int sn = sales.ElementAtOrDefault(pos)?.SerialNumber ?? 0;
                        pack.WriteInt(sn);
                    }
                }
            }

            pack.WriteBytes(new byte[121]);

            //// -1 == available, 2 is not available, 1 = default?

            var customStockState = DataProvider.Commodity.Values.Where(x => x.StockState != StockState.DefaultState).ToList();
            pack.WriteShort((short)customStockState.Count);
            customStockState.ForEach(x =>
            {
                pack.WriteInt(x.SerialNumber);
                pack.WriteInt((int)x.StockState);
            });
            //pack.WriteInt(0);
            //pack.WriteShort(0);
            //pack.WriteByte(0);
            //pack.WriteInt(75);

            chr.SendPacket(pack);
        }
    }
}
