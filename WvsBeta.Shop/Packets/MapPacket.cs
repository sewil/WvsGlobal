using System.Collections.Generic;
using System.Linq;
using WvsBeta.Common.Sessions;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Objects;
using WvsBeta.Common;
using System;
using WvsBeta.Shop.GameObjects;

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

            // Custom commodity items
            //List<CommodityInfo> commodityItems = DataProvider.Commodity.Select(i => i.Value).Where(i => !i.OnSale).ToList();
            //pack.WriteShort((short)commodityItems.Count);
            //foreach (var commodityItem in commodityItems)
            //{
            //    commodityItem.Encode(pack);
            //}
            pack.WriteShort(0);

            //// Newer versions will have discount-per-category stuff here
            //// byte amount, foreach { byte category, byte categorySub, byte discountRate  }

            // Categories
            foreach(CommodityCategory category in CommodityExtensions.GetCategories())
            {
                Server.Instance.BestItems.TryGetValue(category, out var bi);
                for (byte gender = 0; gender <= 1; gender++)
                {
                    // Top 5 items
                    for (byte topIdx = 0; topIdx < 5; topIdx++)
                    {
                        pack.WriteInt((int)category);
                        pack.WriteInt(gender);

                        int sn = bi?.ElementAtOrDefault(topIdx)?.SerialNumber ?? 0;
                        pack.WriteInt(sn);
                    }
                }
            }

            pack.WriteBytes(new byte[120]); // TODO: Why is gift button disabled

            var customStockState = DataProvider.Commodity.Values.Where(x => x.StockState != StockState.DefaultState).ToList();
            pack.WriteShort((short)customStockState.Count);
            customStockState.ForEach(x =>
            {
                pack.WriteInt(x.SerialNumber);
                pack.WriteInt((int)x.StockState);
            });

            chr.SendPacket(pack);
        }
    }
}
