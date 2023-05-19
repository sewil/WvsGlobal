using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using WvsBeta.Game;
using WvsBeta.Game.Scripting;
using WvsBeta.Scripts.Scripts;

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
    // Sid
    [Script("11000")]
    class sid : Shop, INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            self.AskShop(new ShopItemData[] {
                new ShopItemData(1302000, 50),
                new ShopItemData(1312004, 50),
                new ShopItemData(1322005, 50),
                new ShopItemData(1332005, 500),
            }.Concat(Rechargables));
        }
    }
    // Lucy
    [Script("11100")]
    class lucy : Shop, INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            self.AskShop(new ShopItemData[] {
                new ShopItemData(2000000, 50),
                new ShopItemData(2000001, 160),
                new ShopItemData(2000002, 320),
                new ShopItemData(2010000, 30),
                new ShopItemData(2010002, 50),
            }.Concat(Rechargables));
        }
    }
    // Pan
    [Script("21000")]
    class pan : Shop, INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            self.AskShop(new ShopItemData[] {
                new ShopItemData(1040002, 50),
                new ShopItemData(1040006, 50),
                new ShopItemData(1040010, 50),
                new ShopItemData(1041002, 50),
                new ShopItemData(1041006, 50),
                new ShopItemData(1041010, 50),
                new ShopItemData(1041011, 50),
                new ShopItemData(1060002, 50),
                new ShopItemData(1060006, 50),
                new ShopItemData(1061002, 50),
                new ShopItemData(1061008, 50),
                new ShopItemData(1072001, 50),
                new ShopItemData(1072005, 50),
            }.Concat(Rechargables));
        }
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
    // Silver
    [Script("1001000")]
    class silver : Shop, INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            self.AskShop(new ShopItemData[] {
                new ShopItemData(1302000, 50),
                new ShopItemData(1312004, 50),
                new ShopItemData(1322005, 50),
                new ShopItemData(1302007, 3000),
                new ShopItemData(1312000, 3000),
                new ShopItemData(1332007, 1000),
            }.Concat(Rechargables));
        }
    }
    // Natasha
    [Script("1001001")]
    class natasha : Shop, INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            self.AskShop(new ShopItemData[] {
                new ShopItemData(1002008, 500),
                new ShopItemData(1002014, 1000),
                new ShopItemData(1002066, 450),
                new ShopItemData(1002067, 450),
                new ShopItemData(1002068, 450),
                new ShopItemData(1002069, 450),
                new ShopItemData(1002132, 800),
                new ShopItemData(1002133, 800),
                new ShopItemData(1002134, 800),
                new ShopItemData(1002019, 2000),
                new ShopItemData(1002001, 3000),
                new ShopItemData(1040013, 3000),
                new ShopItemData(1040014, 3000),
                new ShopItemData(1041004, 3000),
                new ShopItemData(1041012, 3000),
                new ShopItemData(1060007, 1000),
                new ShopItemData(1060004, 2800),
                new ShopItemData(1062000, 4800),
                new ShopItemData(1062001, 4800),
                new ShopItemData(1072037, 50),
                new ShopItemData(1072038, 50),
                new ShopItemData(1072001, 50),
                new ShopItemData(1072005, 50),
                new ShopItemData(1072048, 5000),
                new ShopItemData(1072049, 5000),
                new ShopItemData(1072008, 5000),
                new ShopItemData(1072017, 10000),
                new ShopItemData(1072062, 10000),
                new ShopItemData(1072063, 10000),
                new ShopItemData(1092003, 2000),
            }.Concat(Rechargables));
        }
    }
    // Mina
    [Script("1001100")]
    class mina : Shop, INpcScript
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
                new ShopItemData(2020028, 3000),
                new ShopItemData(2030000, 400),
                new ShopItemData(2030001, 500),
                new ShopItemData(2060000, 1),
                new ShopItemData(2061000, 1),
                new ShopItemData(2070000, 500),
            }.Concat(Rechargables));
        }
    }
    // River
    [Script("1021000")]
    class river : Shop, INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            self.AskShop(new ShopItemData[] {
                new ShopItemData(1302007, 3000),
                new ShopItemData(1302005, 6000),
                new ShopItemData(1302002, 10000),
                new ShopItemData(1302006, 10000),
                new ShopItemData(1302003, 20000),
                new ShopItemData(1302008, 40000),
                new ShopItemData(1332006, 7000),
                new ShopItemData(1332008, 10000),
                new ShopItemData(1332010, 22000),
                new ShopItemData(1312001, 6000),
                new ShopItemData(1312003, 20000),
                new ShopItemData(1312005, 40000),
                new ShopItemData(1322000, 6000),
                new ShopItemData(1322002, 10000),
                new ShopItemData(1322004, 22000),
                new ShopItemData(1322014, 40000),
                new ShopItemData(1402001, 3000),
                new ShopItemData(1402000, 12000),
                new ShopItemData(1402008, 22000),
                new ShopItemData(1412001, 3000),
                new ShopItemData(1412002, 10000),
                new ShopItemData(1412000, 22000),
                new ShopItemData(1412006, 45000),
                new ShopItemData(1422000, 3000),
                new ShopItemData(1422002, 6000),
                new ShopItemData(1422003, 10000),
                new ShopItemData(1422006, 10000),
                new ShopItemData(1422004, 20000),
                new ShopItemData(1432000, 3000),
                new ShopItemData(1432001, 7000),
                new ShopItemData(1442000, 3000),
                new ShopItemData(1442006, 8000),
                new ShopItemData(1442007, 12000),
                new ShopItemData(1442004, 24000),
            }.Concat(Rechargables));
        }
    }
    // Harry
    [Script("1021001")]
    class harry : Shop, INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            self.AskShop(new ShopItemData[] {
                new ShopItemData(1002043, 2400),
                new ShopItemData(1002039, 2700),
                new ShopItemData(1002051, 3500),
                new ShopItemData(1002055, 8700),
                new ShopItemData(1002059, 8500),
                new ShopItemData(1002027, 11500),
                new ShopItemData(1002005, 19000),
                new ShopItemData(1040015, 3200),
                new ShopItemData(1040038, 3200),
                new ShopItemData(1040009, 5500),
                new ShopItemData(1040037, 5500),
                new ShopItemData(1040012, 9000),
                new ShopItemData(1040039, 9000),
                new ShopItemData(1040040, 16000),
                new ShopItemData(1040016, 16000),
                new ShopItemData(1040041, 16000),
                new ShopItemData(1040021, 40000),
                new ShopItemData(1040026, 40000),
                new ShopItemData(1041014, 3200),
                new ShopItemData(1041064, 3200),
                new ShopItemData(1041019, 9000),
                new ShopItemData(1041020, 9000),
                new ShopItemData(1041021, 9000),
                new ShopItemData(1041022, 16000),
                new ShopItemData(1041023, 16000),
                new ShopItemData(1041024, 16000),
                new ShopItemData(1050005, 15000),
                new ShopItemData(1050006, 15000),
                new ShopItemData(1050007, 15000),
                new ShopItemData(1050011, 70000),
                new ShopItemData(1051000, 8000),
                new ShopItemData(1051010, 60000),
                new ShopItemData(1051011, 60000),
                new ShopItemData(1051012, 60000),
                new ShopItemData(1051013, 60000),
                new ShopItemData(1060008, 3000),
                new ShopItemData(1060028, 3000),
                new ShopItemData(1060009, 5000),
                new ShopItemData(1060027, 5000),
                new ShopItemData(1060010, 9000),
                new ShopItemData(1060029, 9000),
                new ShopItemData(1060011, 14000),
                new ShopItemData(1060030, 14000),
                new ShopItemData(1060060, 14000),
                new ShopItemData(1060016, 38000),
                new ShopItemData(1060017, 38000),
                new ShopItemData(1060018, 38000),
                new ShopItemData(1060019, 38000),
                new ShopItemData(1061014, 3000),
                new ShopItemData(1061023, 3000),
                new ShopItemData(1061016, 9000),
                new ShopItemData(1061017, 9000),
                new ShopItemData(1061018, 9000),
                new ShopItemData(1061015, 14000),
                new ShopItemData(1061019, 14000),
                new ShopItemData(1061020, 14000),
                new ShopItemData(1072009, 5000),
                new ShopItemData(1072050, 5000),
                new ShopItemData(1072007, 10000),
                new ShopItemData(1072046, 10000),
                new ShopItemData(1072047, 10000),
                new ShopItemData(1072011, 15000),
                new ShopItemData(1092005, 4000),
                new ShopItemData(1092000, 6000),
            }.Concat(Rechargables));
        }
    }
    // Arturo
    [Script("1021100")]
    class arturo : Shop, INpcScript
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
                new ShopItemData(2030000, 400),
                new ShopItemData(2030003, 500),
                new ShopItemData(2060000, 1),
                new ShopItemData(2061000, 1),
                new ShopItemData(2050000, 200),
                new ShopItemData(2050001, 200),
                new ShopItemData(2050002, 300),
                new ShopItemData(2070000, 500),
            }.Concat(Rechargables));
        }
    }
    // Flora the Fairy
    [Script("1031000")]
    class flora : Shop, INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            self.AskShop(new ShopItemData[] {
                new ShopItemData(1372005, 2000),
                new ShopItemData(1372006, 5000),
                new ShopItemData(1372002, 9000),
                new ShopItemData(1372004, 18000),
                new ShopItemData(1372003, 38000),
                new ShopItemData(1382000, 3000),
                new ShopItemData(1382003, 6000),
                new ShopItemData(1382005, 6000),
                new ShopItemData(1382004, 10000),
                new ShopItemData(1382002, 20000),
                new ShopItemData(1322002, 10000),
            }.Concat(Rechargables));
        }
    }
    // Serabi the Fairy
    [Script("1031001")]
    class serabi : Shop, INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            self.AskShop(new ShopItemData[] {
                new ShopItemData(1002017, 1200),
                new ShopItemData(1002102, 3800),
                new ShopItemData(1002103, 3800),
                new ShopItemData(1002104, 3800),
                new ShopItemData(1002105, 3800),
                new ShopItemData(1002106, 3800),
                new ShopItemData(1002016, 8000),
                new ShopItemData(1040004, 2000),
                new ShopItemData(1040017, 2000),
                new ShopItemData(1040018, 8500),
                new ShopItemData(1040019, 8500),
                new ShopItemData(1040020, 8500),
                new ShopItemData(1041015, 2000),
                new ShopItemData(1041016, 2000),
                new ShopItemData(1041017, 4000),
                new ShopItemData(1041018, 4000),
                new ShopItemData(1041025, 4000),
                new ShopItemData(1041026, 4000),
                new ShopItemData(1041029, 8500),
                new ShopItemData(1041030, 8500),
                new ShopItemData(1041031, 8500),
                new ShopItemData(1041041, 42000),
                new ShopItemData(1041042, 42000),
                new ShopItemData(1041043, 42000),
                new ShopItemData(1050008, 7000),
                new ShopItemData(1050009, 7000),
                new ShopItemData(1050010, 7000),
                new ShopItemData(1050001, 16000),
                new ShopItemData(1050023, 16000),
                new ShopItemData(1050024, 16000),
                new ShopItemData(1050025, 16000),
                new ShopItemData(1050003, 40000),
                new ShopItemData(1050026, 40000),
                new ShopItemData(1050027, 40000),
                new ShopItemData(1050028, 40000),
                new ShopItemData(1051003, 16000),
                new ShopItemData(1051005, 16000),
                new ShopItemData(1051004, 1600),
                new ShopItemData(1060012, 2000),
                new ShopItemData(1060013, 2000),
                new ShopItemData(1060014, 8000),
                new ShopItemData(1060015, 8000),
                new ShopItemData(1061010, 2000),
                new ShopItemData(1061011, 2000),
                new ShopItemData(1061012, 4000),
                new ShopItemData(1061013, 4000),
                new ShopItemData(1061021, 4000),
                new ShopItemData(1061022, 4000),
                new ShopItemData(1061027, 8000),
                new ShopItemData(1061028, 8000),
                new ShopItemData(1061034, 40000),
                new ShopItemData(1061035, 40000),
                new ShopItemData(1061036, 40000),
                new ShopItemData(1072006, 1500),
                new ShopItemData(1072044, 1500),
                new ShopItemData(1072045, 1500),
                new ShopItemData(1072023, 4000),
                new ShopItemData(1072024, 4000),
            }.Concat(Rechargables));
        }
    }
    // Len the Fairy
    [Script("1031100")]
    class len : Shop, INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            self.AskShop(new ShopItemData[] {
                new ShopItemData(2000000, 50),
                new ShopItemData(2000001, 160),
                new ShopItemData(2000002, 320),
                new ShopItemData(2000003, 192),
                new ShopItemData(2000006, 604),
                new ShopItemData(2002000, 500),
                new ShopItemData(2002001, 400),
                new ShopItemData(2002002, 500),
                new ShopItemData(2002004, 500),
                new ShopItemData(2002005, 500),
                new ShopItemData(2010000, 30),
                new ShopItemData(2010002, 50),
                new ShopItemData(2010001, 106),
                new ShopItemData(2010003, 97),
                new ShopItemData(2010004, 305),
                new ShopItemData(2060000, 1),
                new ShopItemData(2061000, 1),
                new ShopItemData(2050000, 200),
                new ShopItemData(2050001, 200),
                new ShopItemData(2050002, 300),
                new ShopItemData(2050003, 500),
                new ShopItemData(2070000, 500),
            }.Concat(Rechargables));
        }
    }
    // El Moth
    [Script("1032103")]
    class elmoth : Shop, INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            self.AskShop(new ShopItemData[] {
                new ShopItemData(2040000, 35000),
                new ShopItemData(2040400, 35000),
                new ShopItemData(2040600, 35000),
                new ShopItemData(2040700, 35000),
                new ShopItemData(2040300, 35000),
                new ShopItemData(2044500, 70000),
                new ShopItemData(2044600, 70000),
                new ShopItemData(2043700, 70000),
                new ShopItemData(2043800, 70000),
            }.Concat(Rechargables));
        }
    }
    // Dr. Faymus
    [Script("1051002")]
    class drfaymus : Shop, INpcScript
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
                new ShopItemData(2030000, 400),
                new ShopItemData(2030005, 500),
                new ShopItemData(2060000, 1),
                new ShopItemData(2061000, 1),
                new ShopItemData(2050000, 200),
                new ShopItemData(2050001, 200),
                new ShopItemData(2050002, 300),
                new ShopItemData(2070000, 500),
            }.Concat(Rechargables));
        }
    }
    // Cutthroat Manny
    [Script("1051000")]
    class manny : Shop, INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            self.AskShop(new ShopItemData[] {
                new ShopItemData(1302007, 3000),
                new ShopItemData(1332000, 4000),
                new ShopItemData(1332006, 7000),
                new ShopItemData(1332002, 8000),
                new ShopItemData(1332008, 10000),
                new ShopItemData(1332013, 15000),
                new ShopItemData(1332010, 22000),
                new ShopItemData(1332004, 38000),
                new ShopItemData(1332012, 40000),
                new ShopItemData(1332009, 42000),
                new ShopItemData(1322009, 20000),
                new ShopItemData(1402001, 3000),
                new ShopItemData(1412001, 3000),
                new ShopItemData(1422000, 3000),
                new ShopItemData(1432000, 3000),
                new ShopItemData(1432001, 7000),
                new ShopItemData(1442000, 3000),
                new ShopItemData(1472000, 3000),
            }.Concat(Rechargables));
        }
    }
    // Don Hwang
    [Script("1051001")]
    class hwang : Shop, INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            self.AskShop(new ShopItemData[] {
                new ShopItemData(1002122, 900),
                new ShopItemData(1002123, 900),
                new ShopItemData(1002124, 900),
                new ShopItemData(1002125, 900),
                new ShopItemData(1002126, 900),
                new ShopItemData(1002001, 3000),
                new ShopItemData(1002107, 4000),
                new ShopItemData(1002108, 4000),
                new ShopItemData(1002109, 4000),
                new ShopItemData(1002110, 4000),
                new ShopItemData(1002111, 4000),
                new ShopItemData(1002127, 12000),
                new ShopItemData(1002128, 12000),
                new ShopItemData(1002129, 12000),
                new ShopItemData(1002130, 12000),
                new ShopItemData(1002131, 12000),
                //new ShopItemData(1002454, 7400), // Red Starry Bandana, nonexisting
                new ShopItemData(1002096, 7400),
                new ShopItemData(1002097, 7400),
                new ShopItemData(1002146, 20000),
                new ShopItemData(1002147, 20000),
                new ShopItemData(1002148, 20000),
                new ShopItemData(1002149, 20000),
                new ShopItemData(1002150, 20000),
                new ShopItemData(1002171, 30000),
                new ShopItemData(1002172, 30000),
                new ShopItemData(1002173, 30000),
                new ShopItemData(1002174, 30000),
                new ShopItemData(1002175, 30000),
                new ShopItemData(1041037, 3000),
                new ShopItemData(1041036, 3000),
                new ShopItemData(1041038, 3000),
                new ShopItemData(1040034, 5000),
                new ShopItemData(1040035, 5000),
                new ShopItemData(1040042, 9000),
                new ShopItemData(1040043, 9000),
                new ShopItemData(1040044, 9000),
                new ShopItemData(1040048, 16000),
                new ShopItemData(1040049, 16000),
                new ShopItemData(1040050, 16000),
                new ShopItemData(1040057, 45000),
                new ShopItemData(1040058, 45000),
                new ShopItemData(1040059, 45000),
                new ShopItemData(1040060, 45000),
                new ShopItemData(1041036, 3000),
                new ShopItemData(1041037, 3000),
                new ShopItemData(1041038, 3000),
                new ShopItemData(1041044, 5000),
                new ShopItemData(1041045, 5000),
                new ShopItemData(1041003, 9000),
                new ShopItemData(1041039, 9000),
                new ShopItemData(1041040, 9000),
                new ShopItemData(1041057, 18000),
                new ShopItemData(1041058, 18000),
                new ShopItemData(1041059, 18000),
                new ShopItemData(1041060, 18000),
                new ShopItemData(1041047, 45000),
                new ShopItemData(1041048, 45000),
                new ShopItemData(1041049, 45000),
                new ShopItemData(1041050, 45000),
                new ShopItemData(1061030, 2800),
                new ShopItemData(1061029, 2800),
                new ShopItemData(1061031, 2800),
                new ShopItemData(1060024, 4800),
                new ShopItemData(1060025, 4800),
                new ShopItemData(1060031, 8000),
                new ShopItemData(1060032, 8000),
                new ShopItemData(1060033, 8000),
                new ShopItemData(1060037, 19000),
                new ShopItemData(1060038, 19000),
                new ShopItemData(1060039, 19000),
                new ShopItemData(1060043, 40000),
                new ShopItemData(1060044, 40000),
                new ShopItemData(1060045, 40000),
                new ShopItemData(1060046, 40000),
                new ShopItemData(1061029, 2800),
                new ShopItemData(1061030, 2800),
                new ShopItemData(1061031, 2800),
                new ShopItemData(1061037, 4800),
                new ShopItemData(1061038, 4800),
                new ShopItemData(1061032, 8000),
                new ShopItemData(1061033, 8000),
                new ShopItemData(1061003, 8000),
                new ShopItemData(1061041, 12000),
                new ShopItemData(1061042, 12000),
                new ShopItemData(1061040, 12000),
                new ShopItemData(1061053, 18000),
                new ShopItemData(1061054, 18000),
                new ShopItemData(1061055, 18000),
                new ShopItemData(1061056, 18000),
                new ShopItemData(1061043, 40000),
                new ShopItemData(1061044, 40000),
                new ShopItemData(1061045, 40000),
                new ShopItemData(1061046, 40000),
                new ShopItemData(1072070, 1800),
                new ShopItemData(1072071, 1800),
                new ShopItemData(1072004, 2000),
                new ShopItemData(1072042, 2000),
                new ShopItemData(1072043, 2000),
                new ShopItemData(1072028, 45000),
                new ShopItemData(1072029, 45000),
                new ShopItemData(1072030, 45000),
                new ShopItemData(1072031, 45000),
                new ShopItemData(1072022, 9000),
                new ShopItemData(1072065, 9000),
                new ShopItemData(1072066, 9000),
            }.Concat(Rechargables));
        }
    }
    // Tulcus
    [Script("1052104")]
    class tulcus : Shop, INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            self.AskShop(new ShopItemData[] {
                new ShopItemData(2040000, 35000),
                new ShopItemData(2040003, 35000),
                new ShopItemData(2040500, 35000),
                new ShopItemData(2040600, 35000),
                new ShopItemData(2040700, 35000),
                new ShopItemData(2040703, 35000),
                new ShopItemData(2043000, 70000),
                new ShopItemData(2043100, 70000),
                new ShopItemData(2043200, 70000),
                new ShopItemData(2044000, 70000),
                new ShopItemData(2044100, 70000),
                new ShopItemData(2044200, 70000),
                new ShopItemData(2044300, 70000),
                new ShopItemData(2044400, 70000),
                new ShopItemData(2043300, 70000),
                new ShopItemData(2044700, 70000),
            }.Concat(Rechargables));
        }
    }
    // 24 Hr Mobile Store
    [Script("1061001")]
    class mobile24hr : Shop, INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            self.AskShop(new ShopItemData[] {
                new ShopItemData(2020001, 220),
                new ShopItemData(2020005, 320),
                new ShopItemData(2020003, 450),
                new ShopItemData(2020004, 450),
                new ShopItemData(2020006, 530),
                new ShopItemData(2022003, 1100),
                new ShopItemData(2020000, 420),
                new ShopItemData(2022000, 1650),
                new ShopItemData(2020002, 320),
                new ShopItemData(2030000, 400),
                new ShopItemData(2060000, 2),
                new ShopItemData(2061000, 2),
                new ShopItemData(2070000, 500),
            }.Concat(Rechargables));
        }
    }
    // Mr. Sweatbottom
    [Script("1061002")]
    class sweatbottom : Shop, INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            self.AskShop(new ShopItemData[] {
                new ShopItemData(2000000, 50),
                new ShopItemData(2000001, 160),
                new ShopItemData(2000002, 320),
                new ShopItemData(2000003, 200),
                new ShopItemData(2000006, 620),
                new ShopItemData(2010000, 30),
                new ShopItemData(2010002, 50),
                new ShopItemData(2010001, 106),
                new ShopItemData(2010003, 100),
                new ShopItemData(2010004, 310),
                new ShopItemData(2022003, 1100),
                new ShopItemData(2022000, 1650),
                new ShopItemData(2030000, 400),
                new ShopItemData(2030006, 600),
                new ShopItemData(2060000, 1),
                new ShopItemData(2061000, 1),
                new ShopItemData(2070000, 500),
            }.Concat(Rechargables));
        }
    }
    // Neri the Fairy
    [Script("2012003")]
    class neri : Shop, INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            self.AskShop(new ShopItemData[] {
                new ShopItemData(1302008, 40000),
                new ShopItemData(1312005, 40000),
                new ShopItemData(1322014, 40000),
                new ShopItemData(1332009, 42000),
                new ShopItemData(1332012, 40000),
                new ShopItemData(1372003, 38000),
                new ShopItemData(1382002, 20000),
                new ShopItemData(1402002, 150000),
                new ShopItemData(1412006, 45000),
                new ShopItemData(1422001, 45000),
                new ShopItemData(1432002, 60000),
                new ShopItemData(1442001, 60000),
                new ShopItemData(1452005, 150000),
                new ShopItemData(1462000, 30000),
                new ShopItemData(1472001, 20000),
            }.Concat(Rechargables));
        }
    }
    // Nuri the Fairy
    [Script("2012004")]
    class nuri : Shop, INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            self.AskShop(new ShopItemData[] {
                new ShopItemData(1002023, 120000),
                new ShopItemData(1002064, 40000),
                new ShopItemData(1002161, 30000),
                new ShopItemData(1002162, 30000),
                new ShopItemData(1002163, 30000),
                new ShopItemData(1002164, 30000),
                new ShopItemData(1002165, 30000),
                new ShopItemData(1002171, 30000),
                new ShopItemData(1002172, 30000),
                new ShopItemData(1002173, 30000),
                new ShopItemData(1002174, 30000),
                new ShopItemData(1002175, 30000),
                new ShopItemData(1040021, 40000),
                new ShopItemData(1040026, 40000),
                new ShopItemData(1041041, 42000),
                new ShopItemData(1041042, 42000),
                new ShopItemData(1041043, 42000),
                new ShopItemData(1040069, 50000),
                new ShopItemData(1040070, 50000),
                new ShopItemData(1040067, 50000),
                new ShopItemData(1040068, 50000),
                new ShopItemData(1041056, 50000),
                new ShopItemData(1041054, 50000),
                new ShopItemData(1041055, 50000),
                new ShopItemData(1040057, 45000),
                new ShopItemData(1040058, 45000),
                new ShopItemData(1040059, 45000),
                new ShopItemData(1040060, 45000),
                new ShopItemData(1041047, 45000),
                new ShopItemData(1041048, 45000),
                new ShopItemData(1041049, 45000),
                new ShopItemData(1051010, 60000),
                new ShopItemData(1051011, 60000),
                new ShopItemData(1051012, 60000),
                new ShopItemData(1051013, 60000),
                new ShopItemData(1050003, 40000),
                new ShopItemData(1050026, 40000),
                new ShopItemData(1050027, 40000),
                new ShopItemData(1050028, 40000),
                new ShopItemData(1060016, 38000),
                new ShopItemData(1060017, 38000),
                new ShopItemData(1060018, 38000),
                new ShopItemData(1060019, 38000),
                new ShopItemData(1061034, 40000),
                new ShopItemData(1061035, 40000),
                new ShopItemData(1061036, 40000),
                new ShopItemData(1060059, 44000),
                new ShopItemData(1060058, 44000),
                new ShopItemData(1060056, 44000),
                new ShopItemData(1060057, 44000),
                new ShopItemData(1061052, 44000),
                new ShopItemData(1061050, 44000),
                new ShopItemData(1061051, 44000),
                new ShopItemData(1060043, 40000),
                new ShopItemData(1060044, 40000),
                new ShopItemData(1060045, 40000),
                new ShopItemData(1060046, 40000),
                new ShopItemData(1061043, 40000),
                new ShopItemData(1061044, 40000),
                new ShopItemData(1061045, 40000),
                new ShopItemData(1061046, 40000),
                new ShopItemData(1092006, 60000),
            }.Concat(Rechargables));
        }
    }
    // Edel the Fairy
    [Script("2012005")]
    class edel : Shop, INpcScript
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
                new ShopItemData(2022003, 1100),
                new ShopItemData(2022000, 1650),
                new ShopItemData(2001000, 3200),
                new ShopItemData(2001001, 2300),
                new ShopItemData(2001002, 4000),
                new ShopItemData(2010000, 30),
                new ShopItemData(2010002, 50),
                new ShopItemData(2010001, 106),
                new ShopItemData(2010003, 100),
                new ShopItemData(2010004, 310),
                new ShopItemData(2020028, 3000),
                new ShopItemData(2050000, 200),
                new ShopItemData(2050001, 200),
                new ShopItemData(2050002, 300),
                new ShopItemData(2050003, 500),
                new ShopItemData(2030000, 400),
                new ShopItemData(2060000, 1),
                new ShopItemData(2061000, 1),
                new ShopItemData(2070000, 500),
            }.Concat(Rechargables));
        }
    }
    // Scott
    [Script("2020001")]
    class scott : Shop, INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            self.AskShop(new ShopItemData[] {
                new ShopItemData(1302004, 100000),
                new ShopItemData(1302009, 225000),
                new ShopItemData(1312006, 100000),
                new ShopItemData(1312007, 175000),
                new ShopItemData(1322015, 100000),
                new ShopItemData(1322016, 175000),
                new ShopItemData(1332001, 200000),
                new ShopItemData(1332014, 375000),
                new ShopItemData(1332011, 425000),
                new ShopItemData(1372001, 175000),
                new ShopItemData(1372000, 400000),
                new ShopItemData(1402006, 350000),
                new ShopItemData(1402007, 450000),
                new ShopItemData(1412004, 200000),
                new ShopItemData(1412005, 250000),
                new ShopItemData(1422008, 200000),
                new ShopItemData(1422007, 250000),
                new ShopItemData(1432003, 175000),
                new ShopItemData(1432005, 225000),
                new ShopItemData(1442003, 175000),
                new ShopItemData(1442009, 300000),
                new ShopItemData(1452006, 250000),
                new ShopItemData(1452007, 375000),
                new ShopItemData(1462004, 200000),
                new ShopItemData(1462005, 250000),
                new ShopItemData(1472004, 30000),
                new ShopItemData(1472007, 60000),
            }.Concat(Rechargables));
        }
    }
    // Rumi
    [Script("2022000")]
    class rumi : Shop, INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            self.AskShop(new ShopItemData[] {
                new ShopItemData(1002004, 160000),
                new ShopItemData(1002141, 96000),
                new ShopItemData(1002142, 96000),
                new ShopItemData(1002143, 96000),
                new ShopItemData(1002144, 96000),
                new ShopItemData(1002138, 100000),
                new ShopItemData(1002139, 100000),
                new ShopItemData(1002135, 100000),
                new ShopItemData(1002137, 100000),
                new ShopItemData(1002176, 100000),
                new ShopItemData(1002177, 100000),
                new ShopItemData(1002178, 100000),
                new ShopItemData(1002179, 100000),
                new ShopItemData(1002180, 100000),
                new ShopItemData(1040000, 200000),
                new ShopItemData(1040085, 200000),
                new ShopItemData(1041084, 200000),
                new ShopItemData(1041085, 200000),
                new ShopItemData(1041052, 120000),
                new ShopItemData(1041051, 120000),
                new ShopItemData(1040072, 114000),
                new ShopItemData(1040073, 114000),
                new ShopItemData(1040074, 114000),
                new ShopItemData(1040076, 114000),
                new ShopItemData(1040081, 180000),
                new ShopItemData(1040079, 180000),
                new ShopItemData(1041066, 114000),
                new ShopItemData(1041069, 114000),
                new ShopItemData(1041065, 114000),
                new ShopItemData(1041067, 114000),
                new ShopItemData(1041082, 180000),
                new ShopItemData(1041081, 180000),
                new ShopItemData(1040063, 114000),
                new ShopItemData(1040062, 114000),
                new ShopItemData(1040061, 114000),
                new ShopItemData(1040082, 180000),
                new ShopItemData(1040083, 180000),
                new ShopItemData(1041075, 180000),
                new ShopItemData(1041074, 180000),
                new ShopItemData(1050000, 112500),
                new ShopItemData(1050021, 112500),
                new ShopItemData(1051001, 112500),
                new ShopItemData(1051014, 112500),
                new ShopItemData(1050031, 300000),
                new ShopItemData(1050002, 300000),
                new ShopItemData(1050035, 450000),
                new ShopItemData(1050038, 450000),
                new ShopItemData(1050036, 450000),
                new ShopItemData(1050037, 450000),
                new ShopItemData(1051023, 450000),
                new ShopItemData(1051025, 450000),
                new ShopItemData(1051024, 450000),
                new ShopItemData(1051027, 450000),
                new ShopItemData(1051007, 111000),
                new ShopItemData(1051008, 111000),
                new ShopItemData(1051009, 111000),
                new ShopItemData(1060074, 180000),
                new ShopItemData(1060075, 180000),
                new ShopItemData(1061083, 180000),
                new ShopItemData(1061084, 180000),
                new ShopItemData(1061048, 120000),
                new ShopItemData(1061047, 120000),
                new ShopItemData(1060062, 108000),
                new ShopItemData(1060065, 108000),
                new ShopItemData(1060061, 108000),
                new ShopItemData(1060063, 108000),
                new ShopItemData(1060070, 160000),
                new ShopItemData(1060069, 160000),
                new ShopItemData(1061061, 108000),
                new ShopItemData(1061064, 108000),
                new ShopItemData(1061060, 108000),
                new ShopItemData(1061062, 108000),
                new ShopItemData(1061081, 160000),
                new ShopItemData(1061080, 160000),
                new ShopItemData(1060051, 108000),
                new ShopItemData(1060050, 108000),
                new ShopItemData(1060052, 108000),
                new ShopItemData(1060071, 160000),
                new ShopItemData(1060072, 160000),
                new ShopItemData(1061070, 160000),
                new ShopItemData(1061069, 160000),
                new ShopItemData(1072168, 30000),
                new ShopItemData(1072169, 28000),
                new ShopItemData(1072170, 29000),
                new ShopItemData(1072171, 29000),
                new ShopItemData(1092001, 100000),
                new ShopItemData(1092002, 200000),
            }.Concat(Rechargables));
        }
    }
    // Hana
    [Script("2022001")]
    class hana : Shop, INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            self.AskShop(new ShopItemData[] {
                new ShopItemData(2022001, 400),
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
                new ShopItemData(2022000, 1650),
                new ShopItemData(2001000, 3200),
                new ShopItemData(2001001, 2300),
                new ShopItemData(2001002, 4000),
                new ShopItemData(2010000, 30),
                new ShopItemData(2010002, 50),
                new ShopItemData(2010001, 106),
                new ShopItemData(2010003, 100),
                new ShopItemData(2010004, 310),
                new ShopItemData(2022003, 1100),
                new ShopItemData(2020012, 4500),
                new ShopItemData(2020013, 5600),
                new ShopItemData(2020014, 8100),
                new ShopItemData(2020015, 10200),
                new ShopItemData(2030000, 400),
                new ShopItemData(2050000, 200),
                new ShopItemData(2050001, 200),
                new ShopItemData(2050002, 300),
                new ShopItemData(2060000, 1),
                new ShopItemData(2061000, 1),
                new ShopItemData(2070000, 500),
            }.Concat(Rechargables));
        }
    }
    // Barun
    [Script("2022002")]
    class barun : Shop, INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            self.AskShop(new ShopItemData[] {
                new ShopItemData(2040003, 35000),
                new ShopItemData(2040300, 35000),
                new ShopItemData(2040400, 35000),
                new ShopItemData(2040503, 35000),
                new ShopItemData(2040600, 35000),
                new ShopItemData(2040700, 35000),
                new ShopItemData(2040800, 35000),
                new ShopItemData(2040900, 35000),
                new ShopItemData(2041000, 35000),
                new ShopItemData(2041003, 35000),
                new ShopItemData(2043000, 70000),
                new ShopItemData(2043100, 70000),
                new ShopItemData(2043200, 70000),
                new ShopItemData(2043300, 70000),
                new ShopItemData(2043700, 70000),
                new ShopItemData(2043800, 70000),
                new ShopItemData(2044000, 70000),
                new ShopItemData(2044100, 70000),
                new ShopItemData(2044200, 70000),
                new ShopItemData(2044300, 70000),
                new ShopItemData(2044400, 70000),
                new ShopItemData(2044500, 70000),
                new ShopItemData(2044600, 70000),
                new ShopItemData(2044700, 70000),
            }.Concat(Rechargables));
        }
    }
    // Glibber
    [Script("2030009")]
    class glibber : Shop, INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            self.AskShop(new ShopItemData[] {
                new ShopItemData(2000001, 166),
                new ShopItemData(2000002, 332),
                new ShopItemData(2000003, 208),
                new ShopItemData(2000006, 644),
                new ShopItemData(2002000, 520),
                new ShopItemData(2002001, 416),
                new ShopItemData(2002002, 520),
                new ShopItemData(2002004, 520),
                new ShopItemData(2002005, 520),
                new ShopItemData(2001000, 3328),
                new ShopItemData(2001001, 2392),
                new ShopItemData(2001002, 4160),
                new ShopItemData(2020005, 332),
                new ShopItemData(2010004, 322),
                new ShopItemData(2020001, 228),
                new ShopItemData(2020003, 468),
                new ShopItemData(2020004, 468),
                new ShopItemData(2020006, 551),
                new ShopItemData(2022003, 1144),
                new ShopItemData(2022000, 1716),
                new ShopItemData(2020012, 4680),
                new ShopItemData(2020013, 5824),
                new ShopItemData(2020014, 8424),
                new ShopItemData(2020015, 10608),
                new ShopItemData(2030000, 400),
                new ShopItemData(2060000, 1),
                new ShopItemData(2061000, 1),
                new ShopItemData(2070000, 500),
            }.Concat(Rechargables));
        }
    }
    // Gumball Machine
    [Script("2040049")]
    class gumball_machine : Shop, INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            self.AskShop(new ShopItemData[] {
                new ShopItemData(2000007, 50),
                new ShopItemData(2000000, 47),
                new ShopItemData(2000008, 160),
                new ShopItemData(2000001, 152),
                new ShopItemData(2000009, 320),
                new ShopItemData(2000002, 304),
                new ShopItemData(2000010, 200),
                new ShopItemData(2000003, 190),
                new ShopItemData(2000011, 620),
                new ShopItemData(2000006, 589),
                new ShopItemData(2001001, 2185),
                new ShopItemData(2001002, 3800),
                new ShopItemData(2002006, 570),
                new ShopItemData(2002007, 570),
                new ShopItemData(2002008, 570),
                new ShopItemData(2002009, 570),
                new ShopItemData(2002010, 475),
                new ShopItemData(2010001, 106),
                new ShopItemData(2010002, 47),
                new ShopItemData(2020001, 209),
                new ShopItemData(2020002, 304),
                new ShopItemData(2020003, 427),
                new ShopItemData(2020004, 427),
                new ShopItemData(2020005, 304),
                new ShopItemData(2020006, 503),
                new ShopItemData(2022003, 1144),
                new ShopItemData(2022000, 1716),
                new ShopItemData(2020012, 4680),
                new ShopItemData(2020013, 5824),
                new ShopItemData(2020014, 8424),
                new ShopItemData(2020015, 10608),
                new ShopItemData(2120000, 25),
                new ShopItemData(2060000, 1),
                new ShopItemData(2061000, 1),
                new ShopItemData(2060001, 10),
                new ShopItemData(2061001, 10),
            }.Concat(Rechargables));
        }
    }
    // Hid
    [Script("2041002")]
    class hid : Shop, INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            self.AskShop(new ShopItemData[] {
                new ShopItemData(1302008, 40000),
                new ShopItemData(1302004, 100000),
                new ShopItemData(1302009, 225000),
                new ShopItemData(1312005, 40000),
                new ShopItemData(1312006, 100000),
                new ShopItemData(1312007, 175000),
                new ShopItemData(1322014, 40000),
                new ShopItemData(1322015, 100000),
                new ShopItemData(1322016, 175000),
                new ShopItemData(1332009, 42000),
                new ShopItemData(1332012, 40000),
                new ShopItemData(1332001, 200000),
                new ShopItemData(1332014, 355000),
                new ShopItemData(1332011, 425000),
                new ShopItemData(1372003, 38000),
                new ShopItemData(1372001, 175000),
                new ShopItemData(1372000, 400000),
                new ShopItemData(1382002, 20000),
                new ShopItemData(1402002, 150000),
                new ShopItemData(1402006, 350000),
                new ShopItemData(1402007, 450000),
                new ShopItemData(1412006, 45000),
                new ShopItemData(1412004, 200000),
                new ShopItemData(1412005, 250000),
                new ShopItemData(1422001, 45000),
                new ShopItemData(1422008, 200000),
                new ShopItemData(1422007, 250000),
                new ShopItemData(1432002, 60000),
                new ShopItemData(1432003, 175000),
                new ShopItemData(1432005, 225000),
                new ShopItemData(1442001, 60000),
                new ShopItemData(1442003, 175000),
                new ShopItemData(1442009, 300000),
                new ShopItemData(1452005, 150000),
                new ShopItemData(1452006, 250000),
                new ShopItemData(1452007, 375000),
                new ShopItemData(1462000, 30000),
                new ShopItemData(1462004, 200000),
                new ShopItemData(1462005, 250000),
                new ShopItemData(1472001, 20000),
                new ShopItemData(1472004, 30000),
                new ShopItemData(1472007, 60000),
            }.Concat(Rechargables));
        }
    }
    // Miru
    [Script("2041003")]
    class miru : Shop, INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            self.AskShop(new ShopItemData[] {
                new ShopItemData(1002004, 160000),
                new ShopItemData(1040000, 200000),
                new ShopItemData(1040085, 200000),
                new ShopItemData(1041084, 200000),
                new ShopItemData(1041085, 200000),
                new ShopItemData(1050000, 112500),
                new ShopItemData(1050021, 112500),
                new ShopItemData(1051001, 112500),
                new ShopItemData(1051014, 112500),
                new ShopItemData(1060075, 180000),
                new ShopItemData(1060074, 180000),
                new ShopItemData(1061083, 180000),
                new ShopItemData(1061084, 180000),
                new ShopItemData(1092001, 100000),
                new ShopItemData(1092002, 200000),
                new ShopItemData(1002141, 96000),
                new ShopItemData(1002142, 96000),
                new ShopItemData(1002143, 96000),
                new ShopItemData(1002144, 96000),
                new ShopItemData(1041052, 120000),
                new ShopItemData(1041051, 120000),
                new ShopItemData(1050031, 300000),
                new ShopItemData(1050002, 300000),
                new ShopItemData(1050035, 450000),
                new ShopItemData(1050038, 450000),
                new ShopItemData(1050036, 450000),
                new ShopItemData(1050037, 450000),
                new ShopItemData(1051023, 450000),
                new ShopItemData(1051025, 450000),
                new ShopItemData(1051024, 450000),
                new ShopItemData(1051027, 450000),
                new ShopItemData(1061048, 120000),
                new ShopItemData(1061047, 120000),
                new ShopItemData(1002138, 100000),
                new ShopItemData(1002139, 100000),
                new ShopItemData(1002137, 100000),
                new ShopItemData(1040072, 114000),
                new ShopItemData(1040073, 114000),
                new ShopItemData(1040074, 114000),
                new ShopItemData(1040076, 114000),
                new ShopItemData(1040081, 180000),
                new ShopItemData(1040079, 180000),
                new ShopItemData(1041066, 114000),
                new ShopItemData(1041069, 114000),
                new ShopItemData(1041065, 114000),
                new ShopItemData(1041067, 114000),
                new ShopItemData(1041082, 180000),
                new ShopItemData(1041081, 180000),
                new ShopItemData(1060062, 108000),
                new ShopItemData(1060065, 108000),
                new ShopItemData(1060061, 108000),
                new ShopItemData(1060063, 108000),
                new ShopItemData(1060070, 160000),
                new ShopItemData(1060069, 160000),
                new ShopItemData(1061061, 108000),
                new ShopItemData(1061064, 108000),
                new ShopItemData(1061060, 108000),
                new ShopItemData(1061062, 108000),
                new ShopItemData(1061081, 160000),
                new ShopItemData(1061080, 160000),
                new ShopItemData(1002176, 100000),
                new ShopItemData(1002177, 100000),
                new ShopItemData(1002178, 100000),
                new ShopItemData(1002179, 100000),
                new ShopItemData(1002180, 100000),
                new ShopItemData(1040063, 114000),
                new ShopItemData(1040062, 114000),
                new ShopItemData(1040061, 114000),
                new ShopItemData(1040082, 180000),
                new ShopItemData(1040083, 180000),
                new ShopItemData(1041075, 180000),
                new ShopItemData(1041074, 180000),
                new ShopItemData(1051007, 111000),
                new ShopItemData(1051008, 111000),
                new ShopItemData(1051009, 111000),
                new ShopItemData(1060051, 108000),
                new ShopItemData(1060050, 108000),
                new ShopItemData(1060052, 108000),
                new ShopItemData(1060071, 160000),
                new ShopItemData(1060072, 160000),
                new ShopItemData(1061070, 160000),
                new ShopItemData(1061069, 160000),
            }.Concat(Rechargables));
        }
    }
    // Misky
    [Script("2041006")]
    class misky : Shop, INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            self.AskShop(new ShopItemData[] {
                new ShopItemData(2000007, 50),
                new ShopItemData(2000000, 47),
                new ShopItemData(2000008, 160),
                new ShopItemData(2000001, 152),
                new ShopItemData(2000009, 320),
                new ShopItemData(2000002, 304),
                new ShopItemData(2000010, 200),
                new ShopItemData(2000003, 190),
                new ShopItemData(2000011, 620),
                new ShopItemData(2000006, 589),
                new ShopItemData(2001001, 2185),
                new ShopItemData(2001002, 3800),
                new ShopItemData(2002006, 500),
                new ShopItemData(2002007, 500),
                new ShopItemData(2002008, 500),
                new ShopItemData(2002009, 500),
                new ShopItemData(2002010, 500),
                new ShopItemData(2010001, 106),
                new ShopItemData(2010002, 47),
                new ShopItemData(2020001, 209),
                new ShopItemData(2020002, 304),
                new ShopItemData(2020003, 427),
                new ShopItemData(2020004, 427),
                new ShopItemData(2020005, 304),
                new ShopItemData(2020006, 503),
                new ShopItemData(2020028, 2850),
                new ShopItemData(2030000, 400),
                new ShopItemData(2060000, 1),
                new ShopItemData(2061000, 1),
                new ShopItemData(2060001, 10),
                new ShopItemData(2061001, 10),
                new ShopItemData(2070000, 500),
            }.Concat(Rechargables));
        }
    }
    // Patricia
    [Script("2041014")]
    class patricia : Shop, INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            self.AskShop(new ShopItemData[] {
                new ShopItemData(2120000, 30),
            }.Concat(Rechargables));
        }
    }
    // Vega
    [Script("2041016")]
    class vega : Shop, INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            self.AskShop(new ShopItemData[] {
                new ShopItemData(2040300, 34000),
                new ShopItemData(2040000, 34000),
                new ShopItemData(2040400, 34000),
                new ShopItemData(2040503, 34000),
                new ShopItemData(2040600, 34000),
                new ShopItemData(2040700, 34000),
                new ShopItemData(2040703, 34000),
                new ShopItemData(2040706, 34000),
                new ShopItemData(2040800, 34000),
                new ShopItemData(2040900, 34000),
                new ShopItemData(2041000, 34000),
                new ShopItemData(2041003, 34000),
                new ShopItemData(2043000, 34000),
                new ShopItemData(2043100, 34000),
                new ShopItemData(2043200, 34000),
                new ShopItemData(2043300, 34000),
                new ShopItemData(2044000, 70000),
                new ShopItemData(2044100, 70000),
                new ShopItemData(2044200, 70000),
                new ShopItemData(2044300, 70000),
                new ShopItemData(2044400, 70000),
                new ShopItemData(2043700, 70000),
                new ShopItemData(2043800, 70000),
                new ShopItemData(2044500, 70000),
                new ShopItemData(2044600, 70000),
                new ShopItemData(2044700, 70000),
            }.Concat(Rechargables));
        }
    }
    // Melias
    [Script("2060003")]
    class melias : Shop, INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            self.AskShop(new ShopItemData[] {
                new ShopItemData(1002023, 120000),
                new ShopItemData(1002048, 160000),
                new ShopItemData(1002034, 60000),
                new ShopItemData(1002142, 96000),
                new ShopItemData(1002162, 60000),
                new ShopItemData(1002138, 100000),
                new ShopItemData(1002172, 60000),
                new ShopItemData(1002177, 100000),
                new ShopItemData(1051011, 60000),
                new ShopItemData(1051012, 60000),
                new ShopItemData(1051013, 60000),
                new ShopItemData(1050011, 70000),
                new ShopItemData(1040021, 40000),
                new ShopItemData(1040026, 40000),
                new ShopItemData(1060016, 38000),
                new ShopItemData(1060017, 38000),
                new ShopItemData(1050000, 112500),
                new ShopItemData(1050021, 112500),
                new ShopItemData(1051001, 112500),
                new ShopItemData(1051014, 112500),
                new ShopItemData(1051015, 112500),
                new ShopItemData(1040085, 200000),
                new ShopItemData(1040086, 200000),
                new ShopItemData(1041084, 200000),
                new ShopItemData(1041085, 200000),
                new ShopItemData(1060074, 180000),
                new ShopItemData(1060075, 180000),
                new ShopItemData(1061083, 180000),
                new ShopItemData(1061084, 180000),
                new ShopItemData(1041041, 42000),
                new ShopItemData(1041042, 42000),
                new ShopItemData(1041043, 42000),
                new ShopItemData(1061034, 40000),
                new ShopItemData(1061035, 40000),
                new ShopItemData(1061036, 40000),
                new ShopItemData(1050003, 40000),
                new ShopItemData(1050026, 40000),
                new ShopItemData(1050028, 40000),
                new ShopItemData(1041051, 120000),
                new ShopItemData(1041052, 120000),
                new ShopItemData(1061047, 120000),
                new ShopItemData(1061048, 120000),
                new ShopItemData(1050002, 300000),
                new ShopItemData(1050030, 300000),
                new ShopItemData(1050031, 300000),
                new ShopItemData(1051023, 450000),
                new ShopItemData(1051024, 450000),
                new ShopItemData(1051025, 450000),
                new ShopItemData(1050036, 450000),
                new ShopItemData(1050037, 450000),
                new ShopItemData(1050038, 450000),
                new ShopItemData(1041054, 50000),
                new ShopItemData(1041056, 50000),
                new ShopItemData(1061050, 44000),
                new ShopItemData(1061052, 44000),
                new ShopItemData(1040067, 50000),
                new ShopItemData(1040069, 50000),
                new ShopItemData(1060056, 44000),
                new ShopItemData(1060058, 44000),
                new ShopItemData(1041065, 114000),
                new ShopItemData(1041066, 114000),
                new ShopItemData(1041067, 114000),
                new ShopItemData(1061060, 108000),
                new ShopItemData(1061061, 108000),
                new ShopItemData(1061062, 108000),
                new ShopItemData(1040072, 114000),
                new ShopItemData(1040073, 114000),
                new ShopItemData(1040074, 114000),
                new ShopItemData(1060061, 108000),
                new ShopItemData(1060062, 108000),
                new ShopItemData(1060063, 108000),
                new ShopItemData(1040079, 180000),
                new ShopItemData(1040081, 180000),
                new ShopItemData(1060069, 160000),
                new ShopItemData(1060070, 160000),
                new ShopItemData(1041081, 180000),
                new ShopItemData(1041082, 180000),
                new ShopItemData(1061080, 160000),
                new ShopItemData(1061081, 160000),
                new ShopItemData(1041047, 45000),
                new ShopItemData(1041049, 45000),
                new ShopItemData(1041050, 45000),
                new ShopItemData(1061043, 40000),
                new ShopItemData(1061045, 40000),
                new ShopItemData(1061046, 40000),
                new ShopItemData(1040057, 45000),
                new ShopItemData(1040059, 45000),
                new ShopItemData(1060043, 45000),
                new ShopItemData(1060045, 45000),
                new ShopItemData(1040061, 114000),
                new ShopItemData(1040062, 114000),
                new ShopItemData(1060050, 108000),
                new ShopItemData(1060051, 108000),
                new ShopItemData(1051007, 111000),
                new ShopItemData(1051008, 111000),
                new ShopItemData(1051009, 111000),
                new ShopItemData(1040082, 180000),
                new ShopItemData(1040083, 180000),
                new ShopItemData(1060071, 160000),
                new ShopItemData(1060072, 160000),
                new ShopItemData(1041074, 180000),
                new ShopItemData(1041075, 180000),
                new ShopItemData(1061069, 160000),
                new ShopItemData(1061070, 160000),
            }.Concat(Rechargables));
        }
    }
    // Oannes
    [Script("2060004")]
    class oannes : Shop, INpcScript
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
                new ShopItemData(2022040, 300),
                new ShopItemData(2022041, 1600),
                new ShopItemData(2001000, 3200),
                new ShopItemData(2001001, 2300),
                new ShopItemData(2001002, 4000),
                new ShopItemData(2010000, 30),
                new ShopItemData(2010002, 50),
                new ShopItemData(2010001, 106),
                new ShopItemData(2010003, 100),
                new ShopItemData(2010004, 310),
                new ShopItemData(2020012, 4500),
                new ShopItemData(2020013, 5600),
                new ShopItemData(2020014, 8100),
                new ShopItemData(2020015, 10200),
                new ShopItemData(2022003, 1100),
                new ShopItemData(2022000, 1650),
                new ShopItemData(2030000, 400),
                new ShopItemData(2060000, 1),
                new ShopItemData(2061000, 1),
                new ShopItemData(2060001, 10),
                new ShopItemData(2061001, 10),
                new ShopItemData(2070000, 500),
            }.Concat(Rechargables));
        }
    }
    // Calypso
    [Script("2060007")]
    class calypso : Shop, INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            self.AskShop(new ShopItemData[] {
                new ShopItemData(1302003, 20000),
                new ShopItemData(1302008, 40000),
                new ShopItemData(1302004, 100000),
                new ShopItemData(1302009, 225000),
                new ShopItemData(1312003, 20000),
                new ShopItemData(1312005, 40000),
                new ShopItemData(1312006, 100000),
                new ShopItemData(1312007, 175000),
                new ShopItemData(1322004, 22000),
                new ShopItemData(1322014, 40000),
                new ShopItemData(1322015, 100000),
                new ShopItemData(1322016, 175000),
                new ShopItemData(1332004, 38000),
                new ShopItemData(1332012, 40000),
                new ShopItemData(1332014, 375000),
                new ShopItemData(1332011, 425000),
                new ShopItemData(1372004, 18000),
                new ShopItemData(1382002, 20000),
                new ShopItemData(1372003, 38000),
                new ShopItemData(1372001, 175000),
                new ShopItemData(1372000, 400000),
                new ShopItemData(1402008, 22000),
                new ShopItemData(1402002, 150000),
                new ShopItemData(1402006, 350000),
                new ShopItemData(1402007, 450000),
                new ShopItemData(1412000, 22000),
                new ShopItemData(1412004, 200000),
                new ShopItemData(1412006, 45000),
                new ShopItemData(1412005, 250000),
                new ShopItemData(1422004, 20000),
                new ShopItemData(1422001, 45000),
                new ShopItemData(1422008, 200000),
                new ShopItemData(1422007, 250000),
                new ShopItemData(1432002, 60000),
                new ShopItemData(1432003, 175000),
                new ShopItemData(1432005, 225000),
                new ShopItemData(1442004, 24000),
                new ShopItemData(1442001, 60000),
                new ShopItemData(1442003, 175000),
                new ShopItemData(1442009, 300000),
                new ShopItemData(1452000, 20000),
                new ShopItemData(1452005, 150000),
                new ShopItemData(1452006, 250000),
                new ShopItemData(1452007, 375000),
                new ShopItemData(1462003, 12000),
                new ShopItemData(1462000, 30000),
                new ShopItemData(1462004, 200000),
                new ShopItemData(1462005, 250000),
                new ShopItemData(1472007, 60000),
            }.Concat(Rechargables));
        }
    }
    // Tamis
    [Script("9000015")]
    class tamis : Shop, INpcScript
    {
        public void Run(INpcHost self, GameCharacter target)
        {
            self.AskShop(new ShopItemData[] {
                new ShopItemData(1302000, 50),
                new ShopItemData(1312004, 50),
                new ShopItemData(1322005, 50),
                new ShopItemData(1332005, 500),
            });
        }
    }
}
