using System.Collections.Generic;
using System.Linq;
using WvsBeta.Common.Sessions;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Objects;
using WvsBeta.Common;
using System;

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

            // Commodity items not on sale (skip)
            List<CommodityInfo> commodityItems = DataProvider.Commodity.Where(ci => !ci.Value.OnSale).Select(i => i.Value).ToList();
            pack.WriteShort((short)commodityItems.Count);
            foreach (var commodityItem in commodityItems)
            {
                commodityItem.Encode(pack);
            }

            //// Newer versions will have discount-per-category stuff here
            //// byte amount, foreach { byte category, byte categorySub, byte discountRate  }

            // Categories
            foreach(CommodityCategory category in Enum.GetValues(typeof(CommodityCategory)))
            {
                for (byte gender = 0; gender <= 1; gender++)
                {
                    // Top 5 items
                    for (byte topIdx = 0; topIdx < 5; topIdx++)
                    {
                        pack.WriteInt((int)category);
                        pack.WriteInt(gender);

                        int sn = Server.Instance.BestItems[category].ElementAtOrDefault(topIdx)?.SerialNumber ?? 0;
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
