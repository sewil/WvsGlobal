using System;
using System.Diagnostics;
using WvsBeta.Common;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Objects;
using WvsBeta.Common.Sessions;
using WvsBeta.Game.Packets;

namespace WvsBeta.Game
{
    public static class PetsPacket
    {
        public static void HandleSpawnPet(GameCharacter chr, short slot)
        {
            if (!(chr.Inventory.GetItem(Common.Enums.Inventory.Cash, slot) is PetItem petItem))
            {
                InventoryOperationPacket.NoChange(chr);
                return;
            }
            

            if (chr.CharacterStat.PetCashId != 0)
            {
                // Already spawned a pet
                SendRemovePet(chr);

                if (chr.CharacterStat.PetCashId == petItem.CashId)
                {
                    // Spawned the same mob
                    chr.CharacterStat.PetCashId = 0;
                    InventoryOperationPacket.NoChange(chr);
                    return;
                }
            }
            
            chr.CharacterStat.PetCashId = petItem.CashId;
            DoPetSpawn(chr);
            InventoryOperationPacket.NoChange(chr);
        }

        public static void DoPetSpawn(GameCharacter chr)
        {
            chr.PetLastInteraction = MasterThread.CurrentTime;

            var petItem = chr.GetSpawnedPet();
            var ml = petItem.MovableLife;
            ml.Foothold = chr.Foothold;
            ml.Position = new Pos(chr.Position);
            ml.Position.Y -= 20;
            ml.Stance = 0;

            SendSpawnPet(chr, petItem);
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

            byte interactionId = packet.ReadByte(); // dunno lol
            
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
                short inc = (short)(full ? -1 : 1);
                Pet.IncreaseCloseness(chr, petItem, inc, false);
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
            chr.Field.SendPacket(chr, pw);
        }

        public static void SendPetNamechange(GameCharacter chr, string name)
        {
            var pw = new Packet(ServerMessages.PET_NAME_CHANGED);
            pw.WriteInt(chr.ID);
            pw.WriteString(name);
            chr.Field.SendPacket(chr, pw);
        }

        public static void SendPetInteraction(GameCharacter chr, byte reactionId, bool inc, bool food)
        {
            var pw = new Packet(ServerMessages.PET_INTERACTION);
            pw.WriteInt(chr.ID);
            pw.WriteBool(food);
            if (!food)
                pw.WriteByte(reactionId);
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

        public static void SendSpawnPet(GameCharacter chr, PetItem pet, GameCharacter tochar = null)
        {
            // 43 10000000 01 404B4C00 0300312031 3A00000000000000 0000 00 0000  000000000000000000000000000000000000000000000000000000 
            var pw = new Packet(ServerMessages.SPAWN_PET);
            pw.WriteInt(chr.ID);
            pw.WriteBool(true); // Spawns
            pw.WriteInt(pet.ItemID);
            pw.WriteString(pet.Name);
            pw.WriteLong(pet.CashId);
            pw.WriteShort(pet.MovableLife.Position.X);
            pw.WriteShort(pet.MovableLife.Position.Y);
            pw.WriteByte(pet.MovableLife.Stance);
            pw.WriteShort(pet.MovableLife.Foothold);
            pw.WriteLong(0);
            pw.WriteLong(0);
            if (tochar == null)
                chr.Field.SendPacket(chr, pw);
            else
                tochar.SendPacket(pw);
        }

        public static void SendRemovePet(GameCharacter chr, bool gmhide = false)
        {
            var pw = new Packet(ServerMessages.SPAWN_PET);
            pw.WriteInt(chr.ID);
            pw.WriteBool(false);
            pw.WriteLong(0);
            pw.WriteLong(0);
            chr.Field.SendPacket(chr, pw, (gmhide ? chr : null));
        }
    }
}