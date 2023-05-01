using System;
using System.Diagnostics;
using WvsBeta.Common;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Objects;
using WvsBeta.Common.Sessions;
using WvsBeta.Game.Packets;

namespace WvsBeta.Game
{
    public enum PetRemoveReason
    {
        None = 0,
        Hungry = 1,
        Expire = 2
    }
    public enum PetSpawnFlags
    {
        Spawn = 1,
        Connect = 2,
        ShowRemote = 4,
        ChangeMap = 8,
        ResetPos = Spawn|Connect|ChangeMap,
        ResetHunger = Spawn|Connect,
        ResetStat = Spawn|Connect
    }
    public static class PetsPacket
    {
        public static void SpawnPet(GameCharacter chr, PetItem pet, PetSpawnFlags type, GameCharacter toChar = null)
        {
            chr.CharacterStat.PetCashId = pet.CashId;
            Server.Instance.SpawnedPets[chr.ID] = pet;

            if ((type & PetSpawnFlags.ResetPos) != 0)
            {
                var ml = pet.MovableLife;
                ml.Foothold = chr.Foothold;
                ml.Position = new Pos(chr.Position);
                ml.Position.Y += (short)(type == PetSpawnFlags.Connect ? -12 : 40);
                ml.Stance = 0;
            }

            SendSpawnPet(chr, pet, toChar);

            if ((type & PetSpawnFlags.ResetHunger) != 0)
            {
                chr.PetLastHunger = MasterThread.CurrentTime;
            }
            if ((type & PetSpawnFlags.ResetStat) != 0)
            {
                CharacterStatsPacket.SendStatChanged(chr, StatFlags.Pet);
            }
        }

        public static void RemovePet(GameCharacter chr, PetRemoveReason reason, bool despawn)
        {
            SendRemovePet(chr, reason);
            Server.Instance.SpawnedPets.Remove(chr.ID);
            if (despawn) chr.CharacterStat.PetCashId = 0;
        }

        public static void HandleSpawnPet(GameCharacter chr, short slot)
        {
            if (!(chr.Inventory.GetItem(Inventory.Cash, slot) is PetItem petItem))
            {
                InventoryOperationPacket.NoChange(chr);
                return;
            }

            bool havePet = chr.CharacterStat.PetCashId != 0;
            bool samePet = havePet && chr.CharacterStat.PetCashId == petItem.CashId;

            if (havePet)
            {
                RemovePet(chr, PetRemoveReason.None, true);
            }
            if (!samePet)
            {
                chr.PetLastInteraction = MasterThread.CurrentTime;
                SpawnPet(chr, petItem, PetSpawnFlags.Spawn);
            }

            InventoryOperationPacket.NoChange(chr);
        }

        public static void HandleMovePet(GameCharacter chr, Packet packet)
        {
            // 48 00 00 00 00 03 00 00 00 D1 00 00 00 9E 02 00 00 06 E0 01 00 00 00 D7 00 00 00 00 00 00 00 06 09 00 00 00 00 D7 00 00 00 00 00 88 00 04 15 00 00 

            var petItem = chr.GetSpawnedPet();
            if (petItem == null) return;

            var movePath = new MovePath();
            movePath.DecodeFromPacket(packet, MovePath.MovementSource.Pet);
            chr.TryTraceMovement(movePath);

            Common.Packets.PacketHelper.ValidateMovePath(petItem.MovableLife, movePath);

            SendMovePet(chr, movePath);
        }

        public static void HandleInteraction(GameCharacter chr, Packet packet)
        {
            var petItem = chr.GetSpawnedPet();
            if (petItem == null) return;

            bool inc = false;
            double multiplier = 1.0;
            // 4A 00 00 
            byte doMultiplier = packet.ReadByte();

            if (doMultiplier != 0 && Pet.IsNamedPet(petItem))
                multiplier = 1.5;

            byte interactionId = packet.ReadByte();
            
            if (!DataProvider.Pets.TryGetValue(petItem.ItemID, out var petData) || 
                !petData.Reactions.TryGetValue(interactionId, out var petReactionData)) return;

            long timeSinceLastInteraction = MasterThread.CurrentTime - chr.PetLastInteraction;

            // shouldnt be able to do this yet.
            if (petReactionData.LevelMin > petItem.Level ||
                petReactionData.LevelMax < petItem.Level ||
                timeSinceLastInteraction < 15000) goto send_response;

            // sick math

            chr.PetLastInteraction = MasterThread.CurrentTime;
            double additionalSucceedProbability = (((timeSinceLastInteraction - 15000.0) / 10000.0) * 0.01 + 1.0) * multiplier;

            var random = Rand32.Next() % 100;
            if (random >= (petReactionData.Prob * additionalSucceedProbability) ||
                petItem.Fullness < 50) goto send_response;

            inc = true;
            Pet.IncreaseCloseness(chr, petItem, petReactionData.Inc);

            send_response:
            SendPetInteraction(chr, interactionId, inc, false);
        }

        public static void HandlePetLoot(GameCharacter chr, Packet packet)
        {
            var pet = chr.GetSpawnedPet();
            if (pet == null) return;
            DropPacket.HandlePickupDrop(chr, packet, pet);
        }

        public static void HandlePetAction(GameCharacter chr, Packet packet)
        {
            var type = packet.ReadByte();
            var action = packet.ReadByte();
            var message = packet.ReadString();

            Trace.WriteLine($"Pet Action {type} {action} {message}");
            
            SendPetAction(chr, type, action, message);

        }

        public static void HandlePetFeed(GameCharacter chr, Packet packet)
        {
            try
            {
                short slot = packet.ReadShort();
                int itemId = packet.ReadInt();
                Inventory inv = Constants.getInventory(itemId);

                var petItem = chr.GetSpawnedPet();
                if (petItem == null) throw new ControlledException("Tried feeding non-spawned pet");
                if (!DataProvider.Items.ContainsKey(itemId)) throw new ControlledException("Invalid pet food item id " + itemId);
                var food = chr.Inventory.GetItem(inv, slot);
                if (food == null) throw new ControlledException("Not enough pet food.");
                chr.Inventory.TakeItem(food, food.Inventory, food.InventorySlot, 1);
                bool full = petItem.Fullness >= 100;
                short inc = (short)(full ? -1 : petItem.Fullness <= 75 ? 1 : 0);
                if (inc != 0) Pet.IncreaseCloseness(chr, petItem, inc, false);
                petItem.Fullness = (byte)Math.Min(100, petItem.Fullness + 30);
                Pet.UpdatePet(chr, petItem);
                SendPetInteraction(chr, 0, !full, true);
            }
            catch (ControlledException e)
            {
                if (!string.IsNullOrWhiteSpace(e.Message)) Program.MainForm.LogDebug("Pet feed controlled exception: " + e.Message);
                InventoryOperationPacket.NoChange(chr);
            }
        }

        public static void SendPetAction(GameCharacter chr, byte type, byte action, string text)
        {
            var pw = new Packet(ServerMessages.PET_ACTION);
            pw.WriteInt(chr.ID);
            pw.WriteByte(type);
            pw.WriteByte(action);
            pw.WriteString(text);
            chr.Field.SendPacket(chr, pw, chr);
        }

        public static void SendPetNamechange(GameCharacter chr, string name)
        {
            var pw = new Packet(ServerMessages.PET_NAME_CHANGED);
            pw.WriteInt(chr.ID);
            pw.WriteString(name);
            chr.Field.SendPacket(chr, pw);
        }

        public static void SendPetInteraction(GameCharacter chr, byte interactionId, bool inc, bool food)
        {
            var pw = new Packet(ServerMessages.PET_INTERACTION);
            pw.WriteInt(chr.ID);
            pw.WriteBool(food);
            if (!food)
                pw.WriteByte(interactionId);
            pw.WriteBool(inc);
            chr.Field.SendPacket(chr, pw);
        }

        public static void SendMovePet(GameCharacter chr, MovePath movePath)
        {
            var pw = new Packet(ServerMessages.PET_MOVE);
            pw.WriteInt(chr.ID);
            movePath.EncodeToPacket(pw);

            chr.Field.SendPacket(chr, pw, chr);
        }

        private static void SendSpawnPet(GameCharacter chr, PetItem pet, GameCharacter toChar = null)
        {
            SendSpawnPet(chr, pet, true, PetRemoveReason.None, toChar);
        }

        private static void SendSpawnPet(GameCharacter chr, PetItem pet, bool spawn, PetRemoveReason reason, GameCharacter toChar = null)
        {
            var pw = new Packet(ServerMessages.SPAWN_PET);
            pw.WriteInt(chr.ID);
            pw.WriteBool(spawn); // Spawns
            if (spawn)
                pet.EncodeForRemote(pw);
            else
                pw.WriteByte((byte)reason);

            if (toChar == null)
                chr.Field.SendPacket(chr, pw);
            else
                toChar.SendPacket(pw);
        }
        private static void SendRemovePet(GameCharacter chr, PetRemoveReason reason)
        {
            SendSpawnPet(chr, null, false, reason);
        }
    }
}
