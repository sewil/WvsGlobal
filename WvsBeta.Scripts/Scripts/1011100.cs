using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WvsBeta.Game;
using WvsBeta.Game.Scripting;

namespace WvsBeta.Scripts.Scripts
{
    // Luna Henesys Grocer
    [Script("1011100")]
    internal class Luna : INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            self.AskShop(
                new ShopItemData { ItemID = 2000000, Price = 50 },
                new ShopItemData { ItemID = 2000001, Price = 160 },
                new ShopItemData { ItemID = 2000002, Price = 320 },
                new ShopItemData { ItemID = 2000003, Price = 200 },
                new ShopItemData { ItemID = 2000006, Price = 620 },
                new ShopItemData { ItemID = 2002000, Price = 500 },
                new ShopItemData { ItemID = 2002001, Price = 400 },
                new ShopItemData { ItemID = 2002002, Price = 500 },
                new ShopItemData { ItemID = 2002004, Price = 500 },
                new ShopItemData { ItemID = 2002005, Price = 500 },
                new ShopItemData { ItemID = 2010000, Price = 30 },
                new ShopItemData { ItemID = 2010002, Price = 50 },
                new ShopItemData { ItemID = 2010001, Price = 106 },
                new ShopItemData { ItemID = 2010003, Price = 100 },
                new ShopItemData { ItemID = 2010004, Price = 310 },
                new ShopItemData { ItemID = 2060000, Price = 1, UnitRechargeRate = 0 },
                new ShopItemData { ItemID = 2061000, Price = 1, UnitRechargeRate = 0 },
                new ShopItemData { ItemID = 2030000, Price = 400 },
                new ShopItemData { ItemID = 2030004, Price = 500 },
                new ShopItemData { ItemID = 2050000, Price = 200 },
                new ShopItemData { ItemID = 2050001, Price = 200 },
                new ShopItemData { ItemID = 2050002, Price = 300 },
                new ShopItemData { ItemID = 2020003, Price = 500 },
                new ShopItemData { ItemID = 2070000, Price = 500, UnitRechargeRate = 1 },

                new ShopItemData { ItemID = 2060001, UnitRechargeRate = 0 },
                new ShopItemData { ItemID = 2060002, UnitRechargeRate = 0 },
                new ShopItemData { ItemID = 2060003, UnitRechargeRate = 0 },
                new ShopItemData { ItemID = 2061001, UnitRechargeRate = 0 },
                new ShopItemData { ItemID = 2061002, UnitRechargeRate = 0 },
                new ShopItemData { ItemID = 2061003, UnitRechargeRate = 0 },
                new ShopItemData { ItemID = 2070001, UnitRechargeRate = 0 },
                new ShopItemData { ItemID = 2070002, UnitRechargeRate = 0 },
                new ShopItemData { ItemID = 2070003, UnitRechargeRate = 0 },
                new ShopItemData { ItemID = 2070004, UnitRechargeRate = 0 },
                new ShopItemData { ItemID = 2070005, UnitRechargeRate = 0 },
                new ShopItemData { ItemID = 2070006, UnitRechargeRate = 0 },
                new ShopItemData { ItemID = 2070007, UnitRechargeRate = 0 },
                new ShopItemData { ItemID = 2070008, UnitRechargeRate = 0 },
                new ShopItemData { ItemID = 2070009, UnitRechargeRate = 0 },
                new ShopItemData { ItemID = 2070010, UnitRechargeRate = 0 },
                new ShopItemData { ItemID = 2070011, UnitRechargeRate = 0 },
                new ShopItemData { ItemID = 2070012, UnitRechargeRate = 0 },
                new ShopItemData { ItemID = 2070013, UnitRechargeRate = 0 }
            );
        }
    }
}
