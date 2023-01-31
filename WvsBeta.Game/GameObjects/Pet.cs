using System;
using WvsBeta.Common;
using WvsBeta.Common.Objects;
using WvsBeta.Game.Packets;

namespace WvsBeta.Game
{
    static class Pet
    {
        public static void IncreaseCloseness(GameCharacter chr, PetItem petItem, short inc)
        {
            if (petItem.Closeness >= Constants.MaxCloseness) return;
            petItem.Closeness = (short)Math.Min(Constants.MaxCloseness, petItem.Closeness + inc);

            var possibleLevel = GetLevel(petItem);
            if (possibleLevel != petItem.Level)
            {
                petItem.Level = possibleLevel;
                PlayerEffectPacket.SendPetEffect(chr, PetEffectType.LevelUp);
            }
        }

        public static byte GetLevel(PetItem petItem)
        {
            var expCurve = Constants.PetExp;
            for (byte i = 0; i < expCurve.Length; i++)
            {
                if (expCurve[i] > petItem.Closeness)
                    return (byte)(i + 1);
            }
            return 1;
        }

        public static void UpdatePet(GameCharacter chr, PetItem petItem)
        {
            InventoryPacket.AddItem(chr, Constants.getInventory(petItem.ItemID), petItem, false);
        }

        public static bool IsNamedPet(PetItem petItem)
        {
            return (DataProvider.Pets.TryGetValue(petItem.ItemID, out var petData) &&
                    petItem.Name != petData.Name);
        }
    }
}
