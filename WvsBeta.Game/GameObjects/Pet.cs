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
            petItem.Closeness = (short)Math.Max(0, Math.Min(Constants.MaxCloseness, petItem.Closeness + inc));
            petItem.Level = GetLevel(petItem, out bool levelUp);
            if (levelUp)
            {
                PlayerEffectPacket.SendPetEffect(chr, PetEffectType.LevelUp);
            }
            if (sendUpdate) UpdatePet(chr, petItem);
        }

        public static byte GetLevel(PetItem petItem, out bool levelUp)
        {
            short min = (short)(petItem.Level > 1 ? Constants.PetExp[petItem.Level - 2] : 0);
            short max = (short)(petItem.Level < Constants.PetLevels ? Constants.PetExp[petItem.Level - 1] - 1 : short.MaxValue);
            levelUp = false;

            if (petItem.Closeness < min) return (byte)Math.Max(1, petItem.Level - 1);
            else if (petItem.Closeness > max)
            {
                levelUp = true;
                return (byte)Math.Min(Constants.PetLevels, petItem.Level + 1);
            }
            else return petItem.Level;
        }

        public static void UpdatePet(GameCharacter chr, PetItem petItem)
        {
            InventoryOperationPacket.Add(chr, petItem);
        }

        public static bool IsNamedPet(PetItem petItem)
        {
            return (GameDataProvider.Pets.TryGetValue(petItem.ItemID, out var petData) &&
                    petItem.Name != petData.Name);
        }
    }
}
