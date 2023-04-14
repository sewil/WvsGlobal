using System;

namespace WvsBeta.Common.Enums
{
    [Flags]
    public enum CharacterDataFlag
    {
        None = 0x00,
        Stats = 0x01,
        Money = 0x02,
        Equips = 0x04,
        Consume = 0x08,
        Install = 0x10,
        Etc = 0x20,
        Pet = 0x40,
        MaxSlots = 0x80,
        Skills = 0x100,
        Quests = 0x200,
        MinigameStats = 0x400,
        CoupleRing = 0x800,
        TeleportRock = 0x1000,
        CompletedQuests = 0x4000,
        All = 0xFFFF,

        AnyQuests = Quests|CompletedQuests,
        Items = Equips | Consume | Install | Etc | Pet,
        CashShop = Stats | Money | MaxSlots | Items,
    }
}