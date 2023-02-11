using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using WvsBeta.Game;
using WvsBeta.Game.Scripting;

namespace WvsBeta.Scripts.Scripts
{
    class Shop
    {
        public ShopItemData[] Rechargables = new ShopItemData[]
        {
            new ShopItemData(2070000),
            new ShopItemData(2070001),
            new ShopItemData(2070002),
            new ShopItemData(2070003),
            new ShopItemData(2070004),
            new ShopItemData(2070005),
            new ShopItemData(2070006),
            new ShopItemData(2070007),
            new ShopItemData(2070008),
            new ShopItemData(2070009),
            new ShopItemData(2070010),
            new ShopItemData(2070011),
            new ShopItemData(2070012),
            new ShopItemData(2070013),
        };
    }
    // Karl Henesys Weapon Seller
    [Script("1011000")]
    class Karl : Shop, INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            self.AskShop(new ShopItemData[] {
                new ShopItemData(1452002, 3000),
                new ShopItemData(1452003, 6000),
                new ShopItemData(1452001, 10000),
                new ShopItemData(1452000, 20000),
                new ShopItemData(1462001, 4000),
                new ShopItemData(1462002, 8000),
                new ShopItemData(1462003, 12000),
                new ShopItemData(1462000, 30000),
                new ShopItemData(1302007, 3000),
                new ShopItemData(1322007, 6000),
                new ShopItemData(1322008, 12000),
                new ShopItemData(1422004, 20000),
                new ShopItemData(1442004, 24000),
            }.Concat(Rechargables));
        }
    }
    // Sam Henesys Weapon Seller
    [Script("1011001")]
    class Sam : Shop, INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            self.AskShop(new ShopItemData[] {
                new ShopItemData(1002008, 500),
                new ShopItemData(1002033, 3000),
                new ShopItemData(1002060, 3000),
                new ShopItemData(1002061, 3000),
                new ShopItemData(1002062, 3000),
                new ShopItemData(1002063, 3000),
                new ShopItemData(1002012, 3000),
                new ShopItemData(1002112, 4000),
                new ShopItemData(1002113, 4000),
                new ShopItemData(1002114, 4000),
                new ShopItemData(1002115, 4000),
                new ShopItemData(1002116, 4000),
                new ShopItemData(1002117, 12000),
                new ShopItemData(1002118, 12000),
                new ShopItemData(1002119, 12000),
                new ShopItemData(1002120, 12000),
                new ShopItemData(1002121, 12000),
                new ShopItemData(1002057, 800),
                new ShopItemData(1002010, 800),
                new ShopItemData(1002156, 20000),
                new ShopItemData(1002157, 20000),
                new ShopItemData(1002158, 20000),
                new ShopItemData(1002160, 20000),
                new ShopItemData(1002159, 20000),
                new ShopItemData(1002161, 30000),
                new ShopItemData(1002162, 30000),
                new ShopItemData(1002163, 30000),
                new ShopItemData(1002164, 30000),
                new ShopItemData(1002165, 30000),
                new ShopItemData(1032003, 7000),
                new ShopItemData(1032001, 7000),
                new ShopItemData(1040008, 3200),
                new ShopItemData(1040071, 3200),
                new ShopItemData(1040007, 5500),
                new ShopItemData(1040011, 5500),
                new ShopItemData(1040003, 9000),
                new ShopItemData(1041032, 15000),
                new ShopItemData(1040023, 15000),
                new ShopItemData(1041034, 15000),
                new ShopItemData(1041033, 15000),
                new ShopItemData(1040067, 50000),
                new ShopItemData(1040068, 50000),
                new ShopItemData(1040069, 50000),
                new ShopItemData(1040070, 50000),
                new ShopItemData(1041007, 3200),
                new ShopItemData(1041061, 3200),
                new ShopItemData(1041008, 5500),
                new ShopItemData(1041062, 5500),
                new ShopItemData(1041063, 5500),
                new ShopItemData(1041027, 9000),
                new ShopItemData(1041028, 9000),
                new ShopItemData(1041013, 9000),
                new ShopItemData(1041032, 15000),
                new ShopItemData(1041033, 15000),
                new ShopItemData(1041034, 15000),
                new ShopItemData(1041035, 15000),
                new ShopItemData(1041054, 50000),
                new ShopItemData(1041055, 50000),
                new ShopItemData(1041056, 50000),
                new ShopItemData(1060005, 14000),
                new ShopItemData(1060056, 44000),
                new ShopItemData(1060057, 44000),
                new ShopItemData(1060058, 44000),
                new ShopItemData(1060059, 44000),
                new ShopItemData(1061057, 3000),
                new ShopItemData(1061009, 3000),
                new ShopItemData(1061006, 5000),
                new ShopItemData(1061058, 5000),
                new ShopItemData(1061059, 5000),
                new ShopItemData(1061024, 9000),
                new ShopItemData(1061025, 9000),
                new ShopItemData(1061026, 9000),
                new ShopItemData(1061050, 44000),
                new ShopItemData(1061051, 44000),
                new ShopItemData(1061052, 44000),
                new ShopItemData(1062004, 3000),
                new ShopItemData(1062002, 9000),
                new ShopItemData(1062006, 13000),
                new ShopItemData(1072059, 1800),
                new ShopItemData(1072015, 1800),
                new ShopItemData(1072060, 4500),
                new ShopItemData(1072061, 4500),
                new ShopItemData(1072016, 4500),
                new ShopItemData(1072067, 9000),
                new ShopItemData(1072068, 9000),
                new ShopItemData(1072025, 9000),
                new ShopItemData(1072026, 9000),
                new ShopItemData(1072012, 15000),
                new ShopItemData(1072054, 15000),
                new ShopItemData(1072055, 15000),
                new ShopItemData(1072056, 15000),
                new ShopItemData(1072064, 30000),
                new ShopItemData(1072018, 30000)
            }.Concat(Rechargables));
        }
    }
    // Luna Henesys Grocer
    [Script("1011100")]
    class Luna : Shop, INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            self.AskShop(new ShopItemData[] {
                new ShopItemData(2000000, 50),
                new ShopItemData(2000001, 160),
                new ShopItemData(2000002, 320),
                new ShopItemData(2000003, 200),
                new ShopItemData(2000006, 620),
                new ShopItemData(2002000, 500),
                new ShopItemData(2002001, 400),
                new ShopItemData(2002002, 500),
                new ShopItemData(2002004, 500),
                new ShopItemData(2002005, 500),
                new ShopItemData(2010000, 30),
                new ShopItemData(2010002, 50),
                new ShopItemData(2010001, 106),
                new ShopItemData(2010003, 100),
                new ShopItemData(2010004, 310),
                new ShopItemData(2060000, 1),
                new ShopItemData(2061000, 1),
                new ShopItemData(2030000, 400),
                new ShopItemData(2030004, 500),
                new ShopItemData(2050000, 200),
                new ShopItemData(2050001, 200),
                new ShopItemData(2050002, 300),
                new ShopItemData(2020003, 500),
                new ShopItemData(2070000, 500),
            }.Concat(Rechargables));
        }
    }
    [Script("1012004")]
    class Doofus : Shop, INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            self.AskShop(
                new ShopItemData(2120000, 30)
            );
        }
    }
}
