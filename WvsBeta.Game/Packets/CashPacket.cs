using System;
using WvsBeta.Common;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Objects;
using WvsBeta.Common.Sessions;
using WvsBeta.Common.Tracking;
using WvsBeta.Game.Handlers;
using WvsBeta.Game.Packets;

namespace WvsBeta.Game
{
    public static class CashPacket
    {
        // Thank you, Bui :D
        public enum RockModes
        {
            Delete = 0x02,
            Add = 0x03
        };

        public enum RockErrors
        {
            CannotGo2 = 0x05, // This is unused
            DifficultToLocate = 0x06,
            DifficultToLocate2 = 0x07, // This is unused
            CannotGo = 0x08,
            AlreadyThere = 0x09,
            CannotSaveMap = 0x0A
        };

        public static void HandleTeleRockFunction(GameCharacter chr, Packet packet)
        {
            bool AddCurrentMap = packet.ReadBool();
            if (AddCurrentMap)
            {
                if (chr.Inventory.AddRockLocation(chr.MapID))
                {
                    SendRockUpdate(chr, RockModes.Add);
                }
                else
                {
                    SendRockError(chr, RockErrors.CannotSaveMap);
                }
            }
            else
            {
                int map = packet.ReadInt();
                chr.Inventory.RemoveRockLocation(map);
                SendRockUpdate(chr, RockModes.Delete);
            }
        }

        public static void HandleCashItem(GameCharacter chr, Packet packet)
        {
            short slot = packet.ReadShort();
            int itemid = packet.ReadInt();

            BaseItem item = chr.Inventory.GetItem(Inventory.Use, slot);

            if (chr.AssertForHack(item == null, "HandleCashItem with null item") ||
                chr.AssertForHack(item.ItemID != itemid, "HandleCashItem with itemid inconsistency") ||
                chr.AssertForHack(!DataProvider.Items.TryGetValue(itemid, out var data), "HandleCashItem with unknown item") ||
                chr.AssertForHack(!data.Cash, "HandleCashItem with non-cash item"))
            {
                return;
            }

            var itemType = (Constants.Items.Types.ItemTypes)Constants.getItemType(itemid);

            bool used = false;

            switch (itemType)
            {
                case Constants.Items.Types.ItemTypes.ItemWeather:
                    used = chr.Field.MakeWeatherEffect(itemid, packet.ReadString(), new TimeSpan(0, 0, 30));
                    break;
                case Constants.Items.Types.ItemTypes.ItemJukebox:
                    used = chr.Field.MakeJukeboxEffect(itemid, chr.Name, packet.ReadInt());
                    break;

                case Constants.Items.Types.ItemTypes.ItemPetTag:
                    {
                        var name = packet.ReadString();
                        var petItem = chr.GetSpawnedPet();
                        if (petItem != null &&
                            !chr.IsInvalidTextInput("Pet name tag", name, Constants.MaxPetName, Constants.MinPetName))
                        {
                            petItem.Name = name;
                            PetsPacket.SendPetNamechange(chr, petItem.Name);
                            used = true;
                        }
                    }

                    break;

                case Constants.Items.Types.ItemTypes.ItemMegaPhone:
                    {
                        var text = packet.ReadString();
                        if (!chr.IsInvalidTextInput("Megaphone item", text, Constants.MaxSpeakerTextLength))
                        {
                            switch (itemid)
                            {
                                case 2081000: // Super Megaphone (channel)
                                    ChatPacket.SendMegaphoneMessage(chr.Name + " : " + text);
                                    used = true;
                                    break;

                                case 2082000: // Super Megaphone
                                    Server.Instance.CenterConnection.PlayerSuperMegaphone(
                                        chr.Name + " : " + text,
                                        packet.ReadBool()
                                    );
                                    used = true;
                                    break;
                            }
                        }
                    }
                    break;

                case Constants.Items.Types.ItemTypes.ItemKite:
                    if (chr.Field.Kites.Count > 0)
                    {
                        //Todo : check for character positions..?
                        MapPacket.KiteMessage(chr);
                    }
                    else
                    {
                        string message = packet.ReadString();
                        Kite pKite = new Kite(chr, chr.ID, itemid, message, chr.Field);

                        used = true;
                    }
                    break;

                case Constants.Items.Types.ItemTypes.ItemMesoSack:
                    if (data.Mesos > 0)
                    {
                        chr.Inventory.AddMesos(data.Mesos, false, out int amountGot);

                        MiscPacket.SendGotMesosFromLucksack(chr, amountGot);
                        used = true;
                    }
                    break;
                case Constants.Items.Types.ItemTypes.ItemTeleportRock:
                    {
                        byte mode = packet.ReadByte();
                        int map = -1;
                        if (mode == 1)
                        {
                            string name = packet.ReadString();
                            GameCharacter target = Server.Instance.GetCharacter(name);
                            if (target != null && target != chr)
                            {
                                map = target.MapID;
                                used = true;
                            }
                            else
                            {
                                SendRockError(chr, RockErrors.DifficultToLocate);
                            }
                        }
                        else
                        {
                            map = packet.ReadInt();
                            if (!chr.Inventory.HasRockLocation(map))
                            {
                                map = -1;
                            }
                        }

                        if (map != -1)
                        {
                            //I don't think it's even possible for you to be in a map that doesn't exist and use a Teleport rock?
                            Map from = chr.Field;
                            Map to = DataProvider.Maps.ContainsKey(map) ? DataProvider.Maps[map] : null;

                            if (to == from)
                            {
                                SendRockError(chr, RockErrors.AlreadyThere);
                            }
                            else if (from.Limitations.HasFlag(FieldLimit.TeleportItemLimit))
                            {
                                SendRockError(chr, RockErrors.CannotGo);
                            }
                            else if (chr.AssertForHack(chr.PrimaryStats.Level < 7, "Using telerock while not lvl 8 or higher."))
                            {
                                // Hacks.
                            }
                            else
                            {
                                chr.ChangeMap(map);
                                used = true;
                            }
                        }

                        break;
                    }
                case Constants.Items.Types.ItemTypes.ItemAPSPReset:
                    if (itemid == ItemUseIds.APReset)
                    {
                        StatFlags up = (StatFlags)packet.ReadInt();
                        StatFlags down = (StatFlags)packet.ReadInt();
                        CharacterStatsPacket.HandleAPReset(chr, up, down);
                    }
                    else if (ItemUseIds.SPReset1st <= itemid && itemid <= ItemUseIds.SPReset3rd)
                    {
                        int up = packet.ReadInt();
                        int down = packet.ReadInt();
                        CharacterStatsPacket.HandleSPReset(chr, itemid, up, down);
                    }
                    break;
                case Constants.Items.Types.ItemTypes.ItemNote:
                    {
                        string name = packet.ReadString();
                        string message = packet.ReadString();
                        MemoHandler.SendNote(chr, name, message);
                        break;
                    }
                default:
                    Program.MainForm.LogAppend("Unknown cashitem used: {0} {1} {2}", itemType, itemid, packet.ToString());
                    break;
            }

            if (used)
            {
                ItemTransfer.ItemUsed(chr.ID, item.ItemID, 1, "");
                chr.Inventory.TakeItem(item.ItemID, 1);
            }
            else
            {
                InventoryOperationPacket.NoChange(chr);
            }
        }

        public static void SendRockError(GameCharacter chr, RockErrors code)
        {
            Packet pw = new Packet(ServerMessages.MESSAGE);
            pw.WriteByte((byte)code);
            chr.SendPacket(pw);
        }

        public static void SendRockUpdate(GameCharacter chr, RockModes mode)
        {
            Packet pw = new Packet(ServerMessages.MESSAGE);
            pw.WriteByte((byte)mode);
            chr.Inventory.AddRockPacket(pw);
            chr.SendPacket(pw);
        }
    }
}