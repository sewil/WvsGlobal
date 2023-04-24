using System;
using WvsBeta.Common;
using WvsBeta.Common.Objects;
using WvsBeta.Game.Packets;

namespace WvsBeta.Game
{
    public static class Pet
    {
        public static void IncreaseCloseness(GameCharacter chr, PetItem petItem, short inc, bool sendUpdate = true)
        {
            if (petItem.Closeness >= Constants.MaxCloseness) return;

            short minCloseness = (short)(petItem.Level > 1 ? Constants.PetExp[petItem.Level - 2] : 0);
            short maxCloseness = (short)(Constants.PetExp[petItem.Level - 1] - 1);
            petItem.Closeness = (short)Math.Max(minCloseness, Math.Min(Constants.MaxCloseness, petItem.Closeness + inc));
            var possibleLevel = GetLevel(petItem);
            if (petItem.Closeness > maxCloseness)
            {
                petItem.Level = possibleLevel;
                PlayerEffectPacket.SendPetEffect(chr, PetEffectType.LevelUp);
            }
            if (sendUpdate) UpdatePet(chr, petItem);
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
            InventoryOperationPacket.Add(chr, petItem);
        }

        public static bool IsNamedPet(PetItem petItem)
        {
            return (DataProvider.Pets.TryGetValue(petItem.ItemID, out var petData) &&
                    petItem.Name != petData.Name);
        }
    }
}
