using WvsBeta.Game;
using WvsBeta.Game.Scripting;

namespace WvsBeta.Scripts.Scripts
{
    // Karl Henesys Weapon Seller
    [Script("1011000")]
    class Karl : INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            if (self.AskShop()) return;
            self.AskShop(
                new ShopItemData { ItemID = 1452002, Price = 3000 },
                new ShopItemData { ItemID = 1452003, Price = 6000 },
                new ShopItemData { ItemID = 1452001, Price = 10000 },
                new ShopItemData { ItemID = 1452000, Price = 20000 },
                new ShopItemData { ItemID = 1462001, Price = 4000 },
                new ShopItemData { ItemID = 1462002, Price = 8000 },
                new ShopItemData { ItemID = 1462003, Price = 12000 },
                new ShopItemData { ItemID = 1462000, Price = 30000 },
                new ShopItemData { ItemID = 1302007, Price = 3000 },
                new ShopItemData { ItemID = 1322007, Price = 6000 },
                new ShopItemData { ItemID = 1322008, Price = 12000 },
                new ShopItemData { ItemID = 1422004, Price = 20000 },
                new ShopItemData { ItemID = 1442004, Price = 24000 }
            );
        }
    }
    // Sam Henesys Weapon Seller
    [Script("1011001")]
    class Sam : INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            if (self.AskShop()) return;
            self.AskShop(
                new ShopItemData { ItemID = 1002008, Price = 500 },
                new ShopItemData { ItemID = 1002033, Price = 3000 },
                new ShopItemData { ItemID = 1002060, Price = 3000 },
                new ShopItemData { ItemID = 1002061, Price = 3000 },
                new ShopItemData { ItemID = 1002062, Price = 3000 },
                new ShopItemData { ItemID = 1002063, Price = 3000 },
                new ShopItemData { ItemID = 1002012, Price = 3000 },
                new ShopItemData { ItemID = 1002112, Price = 4000 },
                new ShopItemData { ItemID = 1002113, Price = 4000 },
                new ShopItemData { ItemID = 1002114, Price = 4000 },
                new ShopItemData { ItemID = 1002115, Price = 4000 },
                new ShopItemData { ItemID = 1002116, Price = 4000 },
                new ShopItemData { ItemID = 1002117, Price = 12000 },
                new ShopItemData { ItemID = 1002118, Price = 12000 },
                new ShopItemData { ItemID = 1002119, Price = 12000 },
                new ShopItemData { ItemID = 1002120, Price = 12000 },
                new ShopItemData { ItemID = 1002121, Price = 12000 },
                new ShopItemData { ItemID = 1002057, Price = 800 },
                new ShopItemData { ItemID = 1002010, Price = 800 },
                new ShopItemData { ItemID = 1002156, Price = 20000 },
                new ShopItemData { ItemID = 1002157, Price = 20000 },
                new ShopItemData { ItemID = 1002158, Price = 20000 },
                new ShopItemData { ItemID = 1002160, Price = 20000 },
                new ShopItemData { ItemID = 1002159, Price = 20000 },
                new ShopItemData { ItemID = 1002161, Price = 30000 },
                new ShopItemData { ItemID = 1002162, Price = 30000 },
                new ShopItemData { ItemID = 1002163, Price = 30000 },
                new ShopItemData { ItemID = 1002164, Price = 30000 },
                new ShopItemData { ItemID = 1002165, Price = 30000 },
                new ShopItemData { ItemID = 1032003, Price = 7000 },
                new ShopItemData { ItemID = 1032001, Price = 7000 },
                new ShopItemData { ItemID = 1040008, Price = 3200 },
                new ShopItemData { ItemID = 1040071, Price = 3200 },
                new ShopItemData { ItemID = 1040007, Price = 5500 },
                new ShopItemData { ItemID = 1040011, Price = 5500 },
                new ShopItemData { ItemID = 1040003, Price = 9000 },
                new ShopItemData { ItemID = 1041032, Price = 15000 },
                new ShopItemData { ItemID = 1040023, Price = 15000 },
                new ShopItemData { ItemID = 1041034, Price = 15000 },
                new ShopItemData { ItemID = 1041033, Price = 15000 },
                new ShopItemData { ItemID = 1040067, Price = 50000 },
                new ShopItemData { ItemID = 1040068, Price = 50000 },
                new ShopItemData { ItemID = 1040069, Price = 50000 },
                new ShopItemData { ItemID = 1040070, Price = 50000 },
                new ShopItemData { ItemID = 1041007, Price = 3200 },
                new ShopItemData { ItemID = 1041061, Price = 3200 },
                new ShopItemData { ItemID = 1041008, Price = 5500 },
                new ShopItemData { ItemID = 1041062, Price = 5500 },
                new ShopItemData { ItemID = 1041063, Price = 5500 },
                new ShopItemData { ItemID = 1041027, Price = 9000 },
                new ShopItemData { ItemID = 1041028, Price = 9000 },
                new ShopItemData { ItemID = 1041013, Price = 9000 },
                new ShopItemData { ItemID = 1041032, Price = 15000 },
                new ShopItemData { ItemID = 1041033, Price = 15000 },
                new ShopItemData { ItemID = 1041034, Price = 15000 },
                new ShopItemData { ItemID = 1041035, Price = 15000 },
                new ShopItemData { ItemID = 1041054, Price = 50000 },
                new ShopItemData { ItemID = 1041055, Price = 50000 },
                new ShopItemData { ItemID = 1041056, Price = 50000 },
                new ShopItemData { ItemID = 1060005, Price = 14000 },
                new ShopItemData { ItemID = 1060056, Price = 44000 },
                new ShopItemData { ItemID = 1060057, Price = 44000 },
                new ShopItemData { ItemID = 1060058, Price = 44000 },
                new ShopItemData { ItemID = 1060059, Price = 44000 },
                new ShopItemData { ItemID = 1061057, Price = 3000 },
                new ShopItemData { ItemID = 1061009, Price = 3000 },
                new ShopItemData { ItemID = 1061006, Price = 5000 },
                new ShopItemData { ItemID = 1061058, Price = 5000 },
                new ShopItemData { ItemID = 1061059, Price = 5000 },
                new ShopItemData { ItemID = 1061024, Price = 9000 },
                new ShopItemData { ItemID = 1061025, Price = 9000 },
                new ShopItemData { ItemID = 1061026, Price = 9000 },
                new ShopItemData { ItemID = 1061050, Price = 44000 },
                new ShopItemData { ItemID = 1061051, Price = 44000 },
                new ShopItemData { ItemID = 1061052, Price = 44000 },
                new ShopItemData { ItemID = 1062004, Price = 3000 },
                new ShopItemData { ItemID = 1062002, Price = 9000 },
                new ShopItemData { ItemID = 1062006, Price = 13000 },
                new ShopItemData { ItemID = 1072059, Price = 1800 },
                new ShopItemData { ItemID = 1072015, Price = 1800 },
                new ShopItemData { ItemID = 1072060, Price = 4500 },
                new ShopItemData { ItemID = 1072061, Price = 4500 },
                new ShopItemData { ItemID = 1072016, Price = 4500 },
                new ShopItemData { ItemID = 1072067, Price = 9000 },
                new ShopItemData { ItemID = 1072068, Price = 9000 },
                new ShopItemData { ItemID = 1072025, Price = 9000 },
                new ShopItemData { ItemID = 1072026, Price = 9000 },
                new ShopItemData { ItemID = 1072012, Price = 15000 },
                new ShopItemData { ItemID = 1072054, Price = 15000 },
                new ShopItemData { ItemID = 1072055, Price = 15000 },
                new ShopItemData { ItemID = 1072056, Price = 15000 },
                new ShopItemData { ItemID = 1072064, Price = 30000 },
                new ShopItemData { ItemID = 1072018, Price = 30000 }
            );
        }
    }
    // Luna Henesys Grocer
    [Script("1011100")]
    class Luna : INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            if (self.AskShop()) return;
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
    [Script("1012004")]
    class Doofus : INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            if (self.AskShop()) return;
            self.AskShop(
                new ShopItemData { ItemID = 2120000, Price = 30}
            );
        }
    }
}
