using System;
using System.Collections.Generic;
using System.Diagnostics;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Objects;

namespace WvsBeta.Common
{
    public static class Constants
    {
        public const ushort MAPLE_VERSION = 22;
        public const ushort MAPLE_CRYPTO_VERSION = 22; // for IV stuff
        public const string MAPLE_PATCH_LOCATION = "";
        public const byte MAPLE_LOCALE = 8;

        // 2011000
        public static int[] EXP = new int[] {
            15, 34, 57, 92, 135, 372, 560, 840, 1242, 1716,
            2360, 3216, 4200, 5460, 7050, 8840, 11040, 13716, 16680, 20216,
            24402, 28980, 34320, 40512, 47216, 54900, 63666, 73080, 83720, 95700,
            108480, 122760, 138666, 155540, 174216, 194832, 216600, 240500, 266682, 294216,
            324240, 356916, 391160, 428280, 468450, 510420, 555680, 604416, 655200, 709716,
            748608, 789631, 832902, 878545, 926689, 977471, 1031036, 1087536, 1147132, 1209994,
            1276301, 1346242, 1420016, 1497832, 1579913, 1666492, 1757815, 1854143, 1955750, 2062925,
            2175973, 2295216, 2420993, 2553663, 2693603, 2841212, 2996910, 3161140, 3334370, 3517093,
            3709829, 3913127, 4127566, 4353756, 4592341, 4844001, 5109452, 5389449, 5684790, 5996316,
            6324914, 6671519, 7037118, 7422752, 7829518, 8258575, 8711144, 9188514, 9692044, 10223168,
            10783397, 11374327, 11997640, 12655110, 13348610, 14080113, 14851703, 15665576, 16524049, 17429566,
            18384706, 19392187, 20454878, 21575805, 22758159, 24005306, 25320796, 26708375, 28171993, 29715818,
            31344244, 33061908, 34873700, 36784778, 38800583, 40926854, 43169645, 45535341, 48030677, 50662758,
            53439077, 56367538, 59456479, 62714694, 66151459, 69776558, 73600313, 77633610, 81887931, 86375389,
            91108760, 96101520, 101367883, 106992842, 112782213, 118962678, 125481832, 132358236, 139611467, 147262175,
            155332142, 163844343, 172823012, 182293713, 192283408, 202820538, 213935103, 225658746, 238024845, 251068606,
            264827165, 279339639, 294647508, 310794191, 327825712, 345790561, 364739883, 384727628, 405810702, 428049128,
            451506220, 476248760, 502347192, 529875818, 558913012, 589541445, 621848316, 655925603, 691870326, 729784819,
            769777027, 811960808, 856456260, 903390063, 952895838, 1005114529, 1060194805, 1118293480, 1179575962, 1244216724,
            1312399800, 1384319309, 1460180007, 1540197871, 1624600714, 1713628833, 1807535693, 1906558648, 2011069705
        };

        public const int PartyMinLevelOffset = 5;
        public const double PartyPerUserBonus = 0.05;
        public const double PartyTotalBonus = 1.0;
        public const int MaxPartyMembers = 6;

        public const byte PlayerLevels = 200;
        public const byte PetLevels = 30;
        public const byte MaxPetName = 12;
        public const byte MinPetName = 4;
        public const byte MaxCharacterName = 12;
        public const byte MinCharacterName = 4;
        public const byte MaxSpeakerTextLength = 60;
        public const short MaxMaxHp = 30000;
        public const short MinMaxHp = 1;
        public const short MaxMaxMp = 30000;
        public const short MinMaxMp = 1;
        public const short MaxStat = 2000;
        public const short MinStat = 0;
        public const short MaxFame = 30000;
        public const short MinFame = -30000;
        public const short MaxCloseness = 30000;
        public const short ApPerLevel = 5;
        public const short SpPerLevel = 3;
        public const byte MaxFullness = 100;
        public const byte MinFullness = 0;
        public const byte PetFeedFullness = 30;
        public const int MaxDamage = 99999;
        public const int InvalidMap = 999999999;

        public static short[] PetExp = new short[PetLevels - 1] {
            1, 3, 6, 14, 31, 60, 108, 181, 287, 434,
            632, 891, 1224, 1642, 2161, 2793, 3557, 4467, 5542, 6801,
            8263, 9950, 11882, 14084, 16578, 19391, 22548, 26074, 30000
        };

        /// <summary>
        /// Data for HP/MP formula
        /// First index is the job category (jobid / 100)
        /// Second index is 0 for levelup, 1 for hpmp ups
        /// Third index are the following:
        /// 0: HP min
        /// 1: HP max
        /// 2: Unknown
        /// 3: MP min
        /// 4: MP max
        /// 5: MP int stat multiplier
        /// </summary>
        public static short[,,] HpMpFormulaArguments = new short[6, 2, 6]
        {
            {{12, 16, 0, 10, 12, 20}, {8, 12, 0, 6, 8, 15}}, // Beginner
			{{24, 28, 0, 4, 6, 20}, {20, 24, 0, 2, 4, 15}}, // Warrior
			{{10, 14, 0, 22, 24, 20}, {6, 10, 0, 18, 20, 15}}, // Magician
			{{20, 24, 0, 14, 16, 20}, {16, 20, 0, 10, 12, 15}}, // Bowman
			{{20, 24, 0, 14, 16, 20}, {16, 20, 0, 10, 12, 15}}, // Thief
			{{20, 24, 0, 14, 16, 20}, {16, 20, 0, 10, 12, 15}}, // GM
        };

        public enum HpMpFormulaFields : int
        {
            HPMin = 0,
            HPMax = 1,
            Unk = 2,
            MPMin = 3,
            MPMax = 4,
            MPIntStatMultiplier = 5,
        }

        public static class EquipSlots
        {
            // Update this when we go to a newer version with new slots (looks like third job?)
            public const short MaxSlotIndex = (short)Slots._END;

            public static bool IsValidEquipSlot(short slot)
            {
                if (slot < 0) slot = (short)-slot;
                if (slot > 100) slot -= 100;

                return slot > 0 && slot <= MaxSlotIndex;
            }

            public enum Slots : short
            {
                Invalid = 0,
                Helm = 1,
                Face = 2,
                Eye = 3,
                Earring = 4,
                Top = 5,
                Bottom = 6,
                Shoe = 7,
                Glove = 8,
                Cape = 9,
                Shield = 10,
                Weapon = 11,
                Ring1 = 12,
                Ring2 = 13,
                PetAccessory = 14,
                Ring3 = 15,
                Ring4 = 16,
                PetAbility1 = 18,
                PetAbility2 = 19,
                _END
            }
        }

        public static class Items
        {
            public const int PoisonousMushroom = 2011000;
            public const int PetMesoMagnet = 1812000;
            public const int PetItemPouch = 1812001;
            public const int PetAutoHp = 1812002;
            public const int PetAutoMp = 1812003;
            public const int PetWingBoots = 1812004;
            public const int PetBinoculars = 1812005;
            public const int PetMagicScales = 1812006;
            public const int Choco = 4090000; // 5110000 in newer versions

            public static class Types
            {
                public enum ItemTypes
                {
                    ArmorHelm = 100,
                    AccessoryFace = 101,
                    AccessoryEye = 102,
                    AccessoryEarring = 103,
                    ArmorTop = 104,
                    ArmorOverall = 105,
                    ArmorBottom = 106,
                    ArmorShoe = 107,
                    ArmorGlove = 108,
                    ArmorShield = 109,
                    ArmorCape = 110,
                    ArmorRing = 111,
                    ArmorPendant = 112,
                    Medal = 114,
                    Weapon1hSword = 130,
                    Weapon1hAxe = 131,
                    Weapon1hMace = 132,
                    WeaponDagger = 133,
                    WeaponWand = 137,
                    WeaponStaff = 138,
                    Weapon2hSword = 140,
                    Weapon2hAxe = 141,
                    Weapon2hMace = 142,
                    WeaponSpear = 143,
                    WeaponPolearm = 144,
                    WeaponBow = 145,
                    WeaponCrossbow = 146,
                    WeaponClaw = 147,
                    WeaponSkillFX = 160,
                    WeaponCash = 170,
                    PetEquip = 180,
                    PetSkills = 181,

                    ItemPotion = 200,
                    ItemSpecialPotion = 201, // Like drakes blood and poisonous mushroom
                    ItemFood = 202,
                    ItemReturnScroll = 203,
                    ItemScroll = 204,
                    ItemCure = 205,
                    ItemArrow = 206,
                    ItemStar = 207,
                    ItemMegaPhone = 208,
                    ItemWeather = 209,
                    ItemSummonBag = 210,
                    ItemPetTag = 211,
                    ItemPetFood = 212,
                    ItemKite = 213,
                    ItemMesoSack = 214,
                    ItemJukebox = 215,
                    ItemNote = 216,
                    ItemTeleportRock = 217,
                    ItemAPSPReset = 218,

                    Pet = 500,
                    SetupEventItem = 399,
                    EtcMetal = 401,
                    EtcMineral = 402,
                    EtcEmote = 404,
                    EtcCoupon = 405,
                    EtcStorePermit = 406,
                    EtcWaterOfLife = 407,
                    EtcOmokSet = 408,
                    EtcChocolate = 409,
                    EtcEXPCoupon = 410,
                    EtcGachaponTicket = 411,
                    EtcSafetyCharm = 412,
                    EtcForging = 413,
                }
            }

            public static class ScrollTypes
            {
                public enum Types
                {
                    Helm = 0,
                    Face = 100,
                    Eye = 200,
                    Earring = 300,
                    Topwear = 400,
                    Overall = 500,
                    Bottomwear = 600,
                    Shoes = 700,
                    Gloves = 800,
                    Shield = 900,
                    Cape = 1000,
                    Ring = 1100,
                    Pendant = 1200,
                    OneHandedSword = 3000,
                    OneHandedAxe = 3100,
                    OneHandedMace = 3200,
                    Dagger = 3300,
                    Wand = 3700,
                    Staff = 3800,
                    TwoHandedSword = 4000,
                    TwoHandedAxe = 4100,
                    TwoHandedMace = 4200,
                    Spear = 4300,
                    Polearm = 4400,
                    Bow = 4500,
                    Crossbow = 4600,
                    Claw = 4700,
                    PetEquip = 8000,
                }
            }

            public static class Zakum
            {
                public const int PAPER_DOCUMENT = 4001015;
                public const int THE_KEY = 4001016;
                public const int EYE_OF_FIRE = 4001017;
                public const int FIRE_ORE = 4001018;
            }
        }

        public enum Skills
        {
            EnchantSuccess = 5001008,
            EnchantFailure = 5001009
        }
        public static class Sounds
        {
            public static readonly string EnchantSuccess = "Enchant/Success";
            public static readonly string EnchantFailure  = "Enchant/Failure";
        }
        public static class MobSkills
        {
            public enum Skills
            {
                WeaponAttackUp = 100,
                WeaponAttackUpAoe = 110,
                MagicAttackUp = 101,
                MagicAttackUpAoe = 111,
                WeaponDefenseUp = 102,
                WeaponDefenseUpAoe = 112,
                MagicDefenseUp = 103,
                MagicDefenseUpAoe = 113,
                HealAoe = 114,
                SpeedUpAoe = 115,
                Seal = 120,
                Darkness = 121,
                Weakness = 122,
                Stun = 123,
                Curse = 124,
                Poison = 125,
                Slow = 126,
                CancelBuffs = 127,
                PoisonMist = 131,
                WeaponImmunity = 140,
                MagicImmunity = 141,
                Summon = 200
            }
        }

        public enum Element
        {
            Physical = 0x0,
            Ice = 0x1,
            Fire = 0x2,
            Light = 0x3,
            Poison = 0x4,
            Holy = 0x5,
            Dark = 0x6,
            Undead = 0x7
        }

        public static Element GetElementByChargedSkillID(int SkillID)
        {
            switch (SkillID)
            {
                case WhiteKnight.Skills.SwordFireCharge:
                case WhiteKnight.Skills.BwFireCharge:
                    return Element.Fire;
                case WhiteKnight.Skills.SwordLitCharge:
                case WhiteKnight.Skills.BwLitCharge:
                    return Element.Light;
                case WhiteKnight.Skills.SwordIceCharge:
                case WhiteKnight.Skills.BwIceCharge:
                    return Element.Ice;
                default:
                    return Element.Physical;
            }
        }

        public static string getDropName(int objectid, bool isMob)
        {
            return (isMob ? "m" : "r") + objectid.ToString();
        }

        public static short getSkillJob(int skillId) => (short)(skillId / 10000);
        public static InventoryType getInventory(int itemid) { return (InventoryType)(itemid / 1000000); }
        public static ItemSlotType getItemSlotType(int itemid)
        {
            if (Constants.isPet(itemid)) return ItemSlotType.Pet;
            var inv = Constants.getInventory(itemid);
            switch (inv)
            {
                case InventoryType.Equip: return ItemSlotType.Equip;
                case InventoryType.Cash: return ItemSlotType.Cash;
                default: return ItemSlotType.Bundle;
            }
        }
        public static EquipSlots.Slots getEquipSlot(short slot, out EquippedType type)
        {
            slot = Math.Abs(slot);
            if (slot > 100) type = EquippedType.Cash;
            else type = EquippedType.Normal;

            slot = (short)(slot % 100);
            if (!Enum.IsDefined(typeof (EquipSlots.Slots), slot))
            {
                Trace.WriteLine("[Constants.getEquipSlot] Invalid equip slot " + slot + "!");
                return EquipSlots.Slots.Invalid;
            }
            return (EquipSlots.Slots)slot;
        }
        public static bool IsRingSlot(EquipSlots.Slots slot)
        {
            return slot == EquipSlots.Slots.Ring1 || slot == EquipSlots.Slots.Ring2 || slot == EquipSlots.Slots.Ring3 || slot == EquipSlots.Slots.Ring4;
        }
        public static int getItemType(int itemid) { return (itemid / 10000); }
        public static int getScrollType(int itemid) { return ((itemid % 10000) - (itemid % 100)); }
        public static int itemTypeToScrollType(int itemid) { return ((getItemType(itemid) % 100) * 100); }
        public static bool isArrow(int itemid) { return (getItemType(itemid) == (int)Items.Types.ItemTypes.ItemArrow); }
        public static bool isStar(int itemid) { return (getItemType(itemid) == (int)Items.Types.ItemTypes.ItemStar); }
        public static bool isRechargeable(int itemid) { return isStar(itemid); }
        public static bool isEquip(int itemid) { return (getInventory(itemid) == InventoryType.Equip); }
        public static bool isCoupleRing(int itemid) { return itemid == EquipIds.CrushRing; }
        public static bool isCash(int itemid) { return (getInventory(itemid) == InventoryType.Cash); }
        public static bool isPet(int itemid) { return getItemType(itemid) == (int)Items.Types.ItemTypes.Pet; }
        public static bool isStackable(int itemid) { return !(isRechargeable(itemid) || isEquip(itemid) || isCash(itemid)); }
        public static bool isOverall(int itemid) { return (getItemType(itemid) == (int)Items.Types.ItemTypes.ArmorOverall); }
        public static bool isTop(int itemid) { return (getItemType(itemid) == (int)Items.Types.ItemTypes.ArmorTop); }
        public static bool isBottom(int itemid) { return (getItemType(itemid) == (int)Items.Types.ItemTypes.ArmorBottom); }
        public static bool isShield(int itemid) { return (getItemType(itemid) == (int)Items.Types.ItemTypes.ArmorShield); }
        public static bool is2hWeapon(int itemid) { return (getItemType(itemid) / 10 == 14); }
        public static bool is1hWeapon(int itemid) { return (getItemType(itemid) / 10 == 13); }
        public static bool isBow(int itemid) { return (getItemType(itemid) == (int)Items.Types.ItemTypes.WeaponBow); }
        public static bool isCrossbow(int itemid) { return (getItemType(itemid) == (int)Items.Types.ItemTypes.WeaponCrossbow); }
        public static bool isSword(int itemid) { return (getItemType(itemid) == (int)Items.Types.ItemTypes.Weapon1hSword || getItemType(itemid) == (int)Items.Types.ItemTypes.Weapon2hSword); }
        public static bool isMace(int itemid) { return (getItemType(itemid) == (int)Items.Types.ItemTypes.Weapon1hMace || getItemType(itemid) == (int)Items.Types.ItemTypes.Weapon2hMace); }

        public static bool isPuppet(int skillid) { return (skillid == Sniper.Skills.Puppet || skillid == Ranger.Skills.Puppet); }
        public static bool isSummon(int skillid) { return (isPuppet(skillid) || skillid == Priest.Skills.SummonDragon || skillid == Ranger.Skills.SilverHawk || skillid == Sniper.Skills.GoldenEagle); }

        public static byte getMasteryDisplay(byte level) { return (byte)((level + 1) / 2); }

        public static short getJobTrack(short job, bool flatten = false) { return (short)(flatten ? ((job / 100) % 10) : (job / 100)); }

        public static JobTracks.Tracks GetBaseJobTrack(short jobId) { return (JobTracks.Tracks)getJobTrack(jobId, true); }

        public static QuestJob GetQuestJob(short jobID)
        {
            if (jobID == Gm.ID) return QuestJob.GM;
            var jobTrack = getJobTrack(jobID, true);
            return (QuestJob)Math.Pow(2, jobTrack);
        }

        public static bool IsThirdJob(short jobid)
        {
            return !IsSecondJob(jobid) && jobid % 10 == 1;
        }

        public static bool IsSecondJob(short jobId)
        {
            return !IsFirstJob(jobId) && jobId % 10 == 0;
        }

        public static bool IsFirstJob(short jobId)
        {
            return jobId % 100 == 0;
        }

        public static bool IsJobTrack(short jobId, JobTracks.Tracks track)
        {
            return GetBaseJobTrack(jobId) == track;
        }

        public static EquipSlots.Slots getEquipSlot(int itemId)
        {
            var itemType = (Items.Types.ItemTypes)getItemType(itemId);
            var ItemBodyPartEX = (int)itemType / 10;

            switch (itemType)
            {
                case Items.Types.ItemTypes.ArmorHelm: return EquipSlots.Slots.Helm;
                case Items.Types.ItemTypes.AccessoryFace: return EquipSlots.Slots.Face;
                case Items.Types.ItemTypes.AccessoryEye: return EquipSlots.Slots.Eye;
                case Items.Types.ItemTypes.AccessoryEarring: return EquipSlots.Slots.Earring;
                case Items.Types.ItemTypes.ArmorTop:
                case Items.Types.ItemTypes.ArmorOverall:
                    return EquipSlots.Slots.Top;
                case Items.Types.ItemTypes.ArmorBottom: return EquipSlots.Slots.Bottom;
                case Items.Types.ItemTypes.ArmorShoe: return EquipSlots.Slots.Shoe;
                case Items.Types.ItemTypes.ArmorGlove: return EquipSlots.Slots.Glove;
                case Items.Types.ItemTypes.ArmorCape: return EquipSlots.Slots.Cape;
                case Items.Types.ItemTypes.ArmorRing: return EquipSlots.Slots.Ring1; // When this is returned keep in mind there are 4 slots and this is only 1 of them
                case Items.Types.ItemTypes.ArmorShield: return EquipSlots.Slots.Shield;
                case Items.Types.ItemTypes.PetEquip: return EquipSlots.Slots.PetAccessory;
                case Items.Types.ItemTypes.PetSkills: return EquipSlots.Slots.PetAbility1; // When this is returned keep in mind there are 2 slots and this is only 1 of them
                default:
                    if (ItemBodyPartEX == 13 || ItemBodyPartEX == 14 || ItemBodyPartEX == 16 || ItemBodyPartEX == 17)
                        return EquipSlots.Slots.Weapon;
                    break;
            }
            return EquipSlots.Slots.Invalid;
        }

        public static bool IsCorrectBodyPart(int itemId, short _slot)
        {
            var slot = (EquipSlots.Slots)_slot;
            var itemType = (Items.Types.ItemTypes)getItemType(itemId);
            var ItemBodyPartEX = (int)itemType / 10;

            switch (itemType)
            {
                case Items.Types.ItemTypes.ArmorHelm: return slot == EquipSlots.Slots.Helm;
                case Items.Types.ItemTypes.AccessoryFace: return slot == EquipSlots.Slots.Face;
                case Items.Types.ItemTypes.AccessoryEye: return slot == EquipSlots.Slots.Eye;
                case Items.Types.ItemTypes.AccessoryEarring: return slot == EquipSlots.Slots.Earring;
                case Items.Types.ItemTypes.ArmorTop:
                case Items.Types.ItemTypes.ArmorOverall:
                    return slot == EquipSlots.Slots.Top;
                case Items.Types.ItemTypes.ArmorBottom: return slot == EquipSlots.Slots.Bottom;
                case Items.Types.ItemTypes.ArmorShoe: return slot == EquipSlots.Slots.Shoe;
                case Items.Types.ItemTypes.ArmorGlove: return slot == EquipSlots.Slots.Glove;
                case Items.Types.ItemTypes.ArmorCape: return slot == EquipSlots.Slots.Cape;
                case Items.Types.ItemTypes.ArmorRing:
                    return slot == EquipSlots.Slots.Ring1 || slot == EquipSlots.Slots.Ring2 || slot == EquipSlots.Slots.Ring3 || slot == EquipSlots.Slots.Ring4;
                case Items.Types.ItemTypes.ArmorShield: return slot == EquipSlots.Slots.Shield;
                case Items.Types.ItemTypes.PetEquip: return slot == EquipSlots.Slots.PetAccessory;
                case Items.Types.ItemTypes.PetSkills:
                    return slot == EquipSlots.Slots.PetAbility1 || slot == EquipSlots.Slots.PetAbility2;
                default:
                    {
                        if (ItemBodyPartEX == 13 || ItemBodyPartEX == 14 || ItemBodyPartEX == 16 || ItemBodyPartEX == 17)
                            return slot == EquipSlots.Slots.Weapon;
                        return false;
                    }
            }
        }

        public static int GetLevelEXP(byte level)
        {
            if (level >= 200) return 0;
            return EXP[level - 1];
        }

        public static class JobTracks
        {
            public enum Tracks
            {
                Beginner = 0,
                Warrior = 1,
                Magician = 2,
                Bowman = 3,
                Thief = 4,
                Gm = 5,
            }
        }

        public static class Swordsman
        {
            public const short ID = 100;
            public static class Skills
            {
                public const int ImprovedMaxHpIncrease = 1000001;
                public const int Endure = 1000002;
                public const int IronBody = 1001003;
            }
        }
        public static class Fighter
        {
            public const int ID = 110;
            public static class Skills
            {
                public const int AxeBooster = 1101005;
                public const int AxeMastery = 1100001;
                public const int PowerGuard = 1101007;
                public const int Rage = 1101006;
                public const int SwordBooster = 1101004;
                public const int SwordMastery = 1100000;
            }
        }
        public static class Crusader
        {
            public const short ID = 111;
            public static class Skills
            {
                public const int ImprovedMpRecovery = 1110000;
                public const int ArmorCrash = 1111007;
                public const int AxeComa = 1111006;
                public const int AxePanic = 1111004;
                public const int ComboAttack = 1111002;
                public const int Shout = 1111008;
                public const int SwordComa = 1111005;
                public const int SwordPanic = 1111003;
            }
        }
        public static class Page
        {
            public const short ID = 120;
            public static class Skills
            {
                public const int BwBooster = 1201005;
                public const int BwMastery = 1200001;
                public const int PowerGuard = 1201007;
                public const int SwordBooster = 1201004;
                public const int SwordMastery = 1200000;
                public const int Threaten = 1201006;
            }
        }
        public static class WhiteKnight
        {
            public const short ID = 121;
            public static class Skills
            {
                public const int ImprovedMpRecovery = 1210000;
                public const int BwFireCharge = 1211004;
                public const int BwIceCharge = 1211006;
                public const int BwLitCharge = 1211008;
                public const int ChargeBlow = 1211002;
                public const int MagicCrash = 1211009;
                public const int SwordFireCharge = 1211003;
                public const int SwordIceCharge = 1211005;
                public const int SwordLitCharge = 1211007;
            }
        }
        public static class Spearman
        {
            public const short ID = 130;
            public static class Skills
            {
                public const int HyperBody = 1301007;
                public const int IronWill = 1301006;
                public const int PolearmBooster = 1301005;
                public const int PolearmMastery = 1300001;
                public const int SpearBooster = 1301004;
                public const int SpearMastery = 1300000;
            }
        }
        public static class DragonKnight
        {
            public const short ID = 131;
            public static class Skills
            {
                public const int DragonBlood = 1311008;
                public const int DragonRoar = 1311006;
                public const int ElementalResistance = 1310000;
                public const int PowerCrash = 1311007;
                public const int Sacrifice = 1311005;
            }
        }
        public static class Magician
        {
            public const short ID = 200;
            public static class Skills
            {
                public const int ImprovedMpRecovery = 2000000;
                public const int ImprovedMaxMpIncrease = 2000001;
                public const int MagicArmor = 2001003;
                public const int MagicGuard = 2001002;
                public const int MagicClaw = 2001005;
                public const int EnergyBolt = 2001004;
            }
        }
        public static class FPWizard
        {
            public const short ID = 210;
            public static class Skills
            {
                public const int Meditation = 2101001;
                public const int MpEater = 2100000;
                public const int PoisonBreath = 2101005;
                public const int FireArrow = 2101004;
                public const int Slow = 2101003;
            }
        }
        public static class FPMage
        {
            public const short ID = 211;
            public static class Skills
            {
                public const int ElementAmplification = 2110001;
                public const int ElementComposition = 2111006;
                public const int PartialResistance = 2110000;
                public const int PoisonMyst = 2111003;
                public const int Seal = 2111004;
                public const int SpellBooster = 2111005;
                public const int Explosion = 2111002;
            }
        }
        public static class ILWizard
        {
            public const short ID = 220;
            public static class Skills
            {
                public const int ColdBeam = 2201004;
                public const int Meditation = 2201001;
                public const int MpEater = 2200000;
                public const int Slow = 2201003;
                public const int ThunderBolt = 2201005;
            }
        }
        public static class ILMage
        {
            public const short ID = 221;
            public static class Skills
            {
                public const int ElementAmplification = 2210001;
                public const int ElementComposition = 2211006;
                public const int IceStrike = 2211002;
                public const int PartialResistance = 2210000;
                public const int Seal = 2211004;
                public const int SpellBooster = 2211005;
                public const int Lightening = 2211003;
            }
        }
        public static class Cleric
        {
            public const short ID = 230;
            public static class Skills
            {
                public const int Bless = 2301004;
                public const int Heal = 2301002;
                public const int Invincible = 2301003;
                public const int MpEater = 2300000;
                public const int HolyArrow = 2301005;
            }
        }
        public static class Priest
        {
            public const short ID = 231;
            public static class Skills
            {
                public const int Dispel = 2311001;
                public const int Doom = 2311005;
                public const int ElementalResistance = 2310000;
                public const int HolySymbol = 2311003;
                public const int MysticDoor = 2311002;
                public const int SummonDragon = 2311006;
            }
        }
        public static class Archer
        {
            public const short ID = 300;
            public static class Skills
            {
                public const int BlessingOfAmazon = 3000000;
                public const int CriticalShot = 3000001;
                public const int Focus = 3001003;
            }
        }
        public static class Hunter
        {
            public const short ID = 310;
            public static class Skills
            {
                public const int PowerKnockback = 3101003;
                public const int ArrowBomb = 3101005;
                public const int BowBooster = 3101002;
                public const int BowMastery = 3100000;
                public const int SoulArrow = 3101004;
            }
        }
        public static class Ranger
        {
            public const short ID = 311;
            public static class Skills
            {
                public const int MortalBlow = 3110001;
                public const int Puppet = 3111002;
                public const int SilverHawk = 3111005;
            }
        }
        public static class Crossbowman
        {
            public const short ID = 320;
            public static class Skills
            {
                public const int PowerKnockback = 3201003;
                public const int CrossbowBooster = 3201002;
                public const int CrossbowMastery = 3200000;
                public const int SoulArrow = 3201004;
            }
        }
        public static class Sniper
        {
            public const short ID = 321;
            public static class Skills
            {
                public const int Blizzard = 3211003;
                public const int GoldenEagle = 3211005;
                public const int MortalBlow = 3210001;
                public const int Puppet = 3211002;
            }
        }
        public static class Rogue
        {
            public const short ID = 400;
            public static class Skills
            {
                public const int NimbleBody = 4000000;
                public const int DarkSight = 4001003;
                public const int Disorder = 4001002;
                public const int DoubleStab = 4001334;
                public const int LuckySeven = 4001344;
            }
        }
        public static class Assassin
        {
            public const short ID = 410;
            public static class Skills
            {
                public const int ClawBooster = 4101003;
                public const int ClawMastery = 4100000;
                public const int CriticalThrow = 4100001;
                public const int Endure = 4100002;
                public const int Drain = 4101005;
                public const int Haste = 4101004;
            }
        }
        public static class Hermit
        {
            public const short ID = 411;
            public static class Skills
            {
                public const int Alchemist = 4110000;
                public const int Avenger = 4111005;
                public const int MesoUp = 4111001;
                public const int ShadowMeso = 4111004;
                public const int ShadowPartner = 4111002;
                public const int ShadowWeb = 4111003;
            }
        }
        public static class Bandit
        {
            public const short ID = 420;
            public static class Skills
            {
                public const int DaggerBooster = 4201002;
                public const int DaggerMastery = 4200000;
                public const int Endure = 4200001;
                public const int Haste = 4201003;
                public const int SavageBlow = 4201005;
                public const int Steal = 4201004;
            }
        }
        public static class ChiefBandit
        {
            public const short ID = 421;
            public static class Skills
            {
                public const int Assaulter = 4211002;
                public const int BandOfThieves = 4211004;
                public const int Chakra = 4211001;
                public const int MesoExplosion = 4211006;
                public const int MesoGuard = 4211005;
                public const int Pickpocket = 4211003;
            }
        }
        public static class Gm
        {
            public const short ID = 500;
            public static class Skills
            {
                public const int Bless = 5001003;
                public const int Haste = 5001001;
                public const int HealPlusDispell = 5001000;
                public const int Hide = 5001004;
                public const int HolySymbol = 5001002;
                public const int Resurrection = 5001005;
                public const int SuperDragonRoar = 5001006;
                public const int Teleport = 5001007;

                public const int ItemExplosion = 5001008;
                public const int ShadowPartner = 5001009;
                public const int JumpDown = 50010010;
            }
        }
    }
}