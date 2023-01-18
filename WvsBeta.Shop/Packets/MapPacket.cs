using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WvsBeta.Common;
using WvsBeta.Common.Sessions;
using WvsBeta.Game.Packets;
using WvsBeta.Common.Enums;
using System.Net.Sockets;
using WvsBeta.Common.Objects;

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
            for (byte i = 1; i <= 9; i++)
            {
                // Gender (0 = male, 1 = female)
                for (byte j = 0; j <= 1; j++)
                {
                    // Top 5 items
                    for (byte k = 0; k < 5; k++)
                    {
                        pack.WriteInt(i);
                        pack.WriteInt(j);

                        if (Server.Instance.BestItems.TryGetValue((i, j, k), out var sn))
                        {
                            pack.WriteInt(sn);
                        }
                        else
                        {
                            pack.WriteInt(0);
                        }
                    }
                }
            } // 9 * 2 * 5 * 12 = 1080u

            //// -1 == available, 2 is not available, 1 = default?

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
