using System;

namespace WvsBeta.Common.Enums
{
    [Flags]
    public enum BuffValueTypes : ulong
    {
        None = 0,
        // Byte 1
        WeaponAttack = 0x01,
        WeaponDefense = 0x02,
        MagicAttack = 0x04,
        MagicDefense = 0x08,

        Accurancy = 0x10,
        Avoidability = 0x20,
        Hands = 0x40, // Yes, this has a modifier too.
        Speed = 0x80,

        // Byte 2
        Jump = 0x100,
        MagicGuard = 0x200,
        DarkSight = 0x400,
        Booster = 0x800,

        PowerGuard = 0x1000,
        MaxHP = 0x2000,
        MaxMP = 0x4000,
        Invincible = 0x8000,

        // Byte 3
        SoulArrow = 0x10000,
        Stun = 0x20000, // Mob Skill: Stun and Dragon Roar
        Poison = 0x40000, // Mob Skill: Poison
        Seal = 0x80000, // Mob Skill: Seal

        Darkness = 0x100000, // Mob Skill: Darkness
        ComboAttack = 0x200000,
        Charges = 0x400000,
        DragonBlood = 0x800000,

        // Byte 4
        HolySymbol = 0x1000000,
        MesoUP = 0x2000000,
        ShadowPartner = 0x4000000,
        PickPocketMesoUP = 0x8000000,

        MesoGuard = 0x10_000_000,
        Thaw = 0x20_000_000,
        Weakness = 0x40_000_000, // Mob Skill: Weakness
        Curse = 0x80_000_000, // Mob Skill: Curse
        Slow = 0x1_00_00_00_00, // Mob Skill: Slow

        ALL = 0xFFFFFFFF,
        MOVEMENT_INFO_INDEX = Speed | Jump | Stun | Weakness | Slow, // flags & 0x40020180 or BYTE4(flags) & 1
        MOB_SKILL = Weakness|Curse|Slow,
        ALL_BUFFS = ~MOB_SKILL
    }
}
