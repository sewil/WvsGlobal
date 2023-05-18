using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.ConstrainedExecution;
using System.Windows.Forms;
using WvsBeta.Common;
using WvsBeta.Common.Characters;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Objects;
using WvsBeta.Common.Packets;
using WvsBeta.Common.Sessions;
using WvsBeta.Game.Events;
using WvsBeta.Game.GameObjects;
using WvsBeta.Game.Handlers.Contimove;
using WvsBeta.Game.Packets;
using WvsBeta.Game.Scripting;

namespace WvsBeta.Game
{
    public static class MapPacket
    {
        [Flags]
        public enum AvatarModFlag : byte
        {
            AvatarLook = 1,
            Speed = 2,
            ItemEffects = 4,
        }

        public static void HandleMove(GameCharacter chr, Packet packet)
        {
            if (packet.ReadByte() != chr.PortalCount) return;

            var movePath = new MovePath();
            movePath.DecodeFromPacket(packet, MovePath.MovementSource.Player);
            chr.TryTraceMovement(movePath);

            if (chr.AssertForHack(movePath.Elements.Length == 0, "Received Empty Move Path"))
            {
                return;
            }

            bool allowed = Common.Packets.PacketHelper.ValidateMovePath(chr, movePath);
            if (!allowed && !chr.IsGM)
            {
                //this.Session.Socket.Shutdown(System.Net.Sockets.SocketShutdown.Both);
                //return;
                // TODO: Update speed of character
                // Program.MainForm.LogAppendFormat("Move incorrect: {0}", chr.Name);
            }
            SendPlayerMove(chr, movePath);

            if (!chr.Field.ReallyOutOfBounds.Contains(chr.Position.X, chr.Position.Y))
            {
                if (chr.OutOfMBRCount++ > 5)
                {
                    // Okay, reset.
                    chr.ChangeMap(chr.CharacterStat.MapID, chr.Field.GetClosestStartPoint(chr.Position));
                    chr.OutOfMBRCount = 0;
                }
            }
            else
            {
                chr.OutOfMBRCount = 0;
            }
        }

        public static void OnContiMoveState(GameCharacter chr, Packet packet)
        {
            int mapid = packet.ReadInt();

            var p = new Packet(ServerMessages.CONTISTATE);
            p.WriteByte((byte)ContinentMan.Instance.GetInfo(mapid, 0));
            p.WriteByte((byte)ContinentMan.Instance.GetInfo(mapid, 1));
            chr.SendPacket(p);
        }

        public static void HandleNPCChat(GameCharacter chr, Packet packet)
        {
            int npcId = packet.ReadInt();
            NpcLife npc = chr.Field.GetNPC(npcId);

            if (chr.AssertForHack(!chr.CanAttachAdditionalProcess, "Tried to chat to npc while not able to attach additional process"))
            {
                InventoryOperationPacket.NoChange(chr);
                return;
            }

            // Npc doesnt exist
            if (npc == null)
            {
                InventoryOperationPacket.NoChange(chr);
                return;
            }

            int RealID = npc.ID;
            if (!GameDataProvider.NPCs.TryGetValue(RealID, out NPCData npcData)) return;

            if (npcData.Shop.Count > 0)
            {
                // It's a shop!
                chr.ShopNPCID = RealID;
                NpcPacket.SendShowNPCShop(chr, RealID);
            }
            else if (npcData.Trunk > 0)
            {
                chr.TrunkNPCID = RealID;
                StoragePacket.SendShowStorage(chr, chr.TrunkNPCID);
            }
            else
            {
                Action<string> errorHandlerFnc = null;
#if !DEBUG
                if (chr.IsGM)
                {
#endif
                    errorHandlerFnc = (error) =>
                    {
                        chr.Notice("Error compiling script '" + error + "'!");
                    };
#if !DEBUG
                }
#endif

                NpcChatSession.Start(npc, npcData, chr, errorHandlerFnc);
            }
        }

        public static void HandleEnterPortal(GameCharacter chr, int toMapID, string toPortalName)
        {
            if (
                GameDataProvider.Maps.TryGetValue(toMapID, out Map toMap) &&
                toMap.Portals.TryGetValue(toPortalName, out Portal toPortal)
            )
            {
                if (toPortal.Enabled)
                {
                    chr.ChangeMap(toMapID, toPortal);
                }
                else
                {
                    BlockedMessage(chr, PortalBlockedMessage.CannotGoToThatPlace);
                }
            }
            else
            {
                Program.MainForm.LogDebug(chr.CharacterStat.Name + " tried to arrive at unknown portal " + toPortalName + ", " + toMapID);
                BlockedMessage(chr, PortalBlockedMessage.ClosedForNow);
            }
        }
        private static void HandleEnterPortal(GameCharacter chr, string portalName)
        {
            if (chr.Field.Portals.TryGetValue(portalName, out Portal portal))
            {
                var pos = new Pos(portal.X, portal.Y);
                var dist = chr.Position - pos;
                if (chr.AssertForHack(dist > 300, "Portal distance hack (" + dist + ")", dist > 600))
                {
                    InventoryOperationPacket.NoChange(chr);
                }
                else if (!portal.Enabled)
                {
                    Program.MainForm.LogDebug(chr.CharacterStat.Name + " tried to enter a disabled portal.");
                    BlockedMessage(chr, PortalBlockedMessage.ClosedForNow);
                    InventoryOperationPacket.NoChange(chr);
                }
                else if (!chr.Field.PortalsOpen)
                {
                    Program.MainForm.LogDebug(chr.CharacterStat.Name + " tried to enter a disabled portal.");
                    BlockedMessage(chr, PortalBlockedMessage.ClosedForNow);
                    InventoryOperationPacket.NoChange(chr);
                }
                else
                {
                    HandleEnterPortal(chr, portal.ToMapID, portal.ToName);
                }
            }
            else
            {
                Program.MainForm.LogDebug(chr.CharacterStat.Name + " tried to enter unknown portal??? " + portalName + ", " + chr.Field.ID);
                BlockedMessage(chr, PortalBlockedMessage.ClosedForNow);
            }
        }

        public static void OnEnterPortal(Packet packet, GameCharacter chr)
        {
            if (packet.ReadByte() != chr.PortalCount)
            {
                InventoryOperationPacket.NoChange(chr);
                return;
            }

            int status = packet.ReadInt();
            // 04 00 69 6E 30 33
            string portalName = packet.ReadString();
            if (portalName.Length > 0)
            {
                // 33 03 03 F4
                new Pos(packet); // Current pos
            }

            // 00 00 00
            packet.ReadByte(); // Related to teleporting to party member? Always 0
            packet.ReadByte(); // unk
            packet.ReadByte(); // unk

            switch (status)
            {
                case 0:
                    {
                        if (chr.HP == 0)
                        {
                            chr.HandleDeath();
                        }
                        else if (!chr.IsGM)
                        {
                            Program.MainForm.LogAppend($"Not handling death of {chr.CharacterStat.ID}, because user is not dead. Killing him again. HP: " + chr.HP);
                            // Kill him anyway
                            chr.DamageHP(30000);
                        }
                        else
                        {
                            // Admin /map 0
                            chr.ChangeMap(status);
                        }
                        break;
                    }
                case -1:
                    HandleEnterPortal(chr, portalName);
                    break;
                default:
                    {
                        if (chr.IsGM)
                        {
                            chr.ChangeMap(status);
                        }
                        break;
                    }
            }
        }

        public static void OnEnterScriptPortal(Packet packet, GameCharacter chr)
        {
            string portalName = packet.ReadString();
            if (chr.Field.Portals.TryGetValue(portalName, out Portal portal))
            {
                PortalScriptSession.Run(portal, chr, script => {
                    chr.Notice("Error compiling script: " + script);
                });
            }
            else
            {
                Program.MainForm.LogDebug(chr.CharacterStat.Name + " tried to enter unknown portal " + portalName + ", " + chr.Field.ID);
                BlockedMessage(chr, PortalBlockedMessage.ClosedForNow);
            }
        }
        public static void HandleSitChair(GameCharacter chr, Packet packet)
        {
            short chair = packet.ReadShort();

            if (chair == -1)
            {
                if (chr.MapChair != -1)
                {
                    chr.Field.UsedSeats.Remove(chr.MapChair);
                    chr.MapChair = -1;
                    SendCharacterSit(chr, -1);
                }
                else
                {
                    InventoryOperationPacket.NoChange(chr);
                }
            }
            else
            {
                if (chr.Field != null && chr.Field.Seats.ContainsKey(chair) && !chr.Field.UsedSeats.Contains(chair))
                {
                    chr.Field.UsedSeats.Add(chair);
                    chr.MapChair = chair;
                    SendCharacterSit(chr, chair);
                }
                else
                {
                    InventoryOperationPacket.NoChange(chr);
                }
            }
        }

        public static Packet NpcChangeController(NpcLife npcLife)
        {
            var pw = new Packet(ServerMessages.NPC_CHANGE_CONTROLLER);
            pw.WriteBool(true);
            pw.WriteUInt(npcLife.SpawnID);
            pw.WriteInt(npcLife.ID);
            pw.WriteShort(npcLife.X);
            pw.WriteShort(npcLife.Y);
            pw.WriteBool(!npcLife.FacesLeft);
            pw.WriteUShort(npcLife.Foothold);
            pw.WriteShort(npcLife.Rx0);
            pw.WriteShort(npcLife.Rx1);
            return pw;
        }
        public static Packet NpcEnterField(NpcLife npcLife)
        {
            Packet pw = new Packet(ServerMessages.NPC_ENTER_FIELD);
            pw.WriteUInt(npcLife.SpawnID);
            pw.WriteInt(npcLife.ID);
            pw.WriteShort(npcLife.X);
            pw.WriteShort(npcLife.Y);
            pw.WriteBool(!npcLife.FacesLeft);
            pw.WriteUShort(npcLife.Foothold);
            pw.WriteShort(npcLife.Rx0);
            pw.WriteShort(npcLife.Rx1);
            return pw;
        }
        public static Packet NpcLeaveField(uint spawnId)
        {
            Packet pw = new Packet(ServerMessages.NPC_LEAVE_FIELD);
            pw.WriteUInt(spawnId);
            return pw;
        }
        public static void HandleNPCAnimation(GameCharacter controller, Packet packet)
        {
            Packet pw = new Packet(ServerMessages.NPC_ANIMATE);
            pw.WriteBytes(packet.ReadLeftoverBytes());

            controller.SendPacket(pw);
        }

        public static void SendWeatherEffect(Map map, GameCharacter victim = null)
        {
            Packet pw = new Packet(ServerMessages.BLOW_WEATHER);
            pw.WriteBool(map.WeatherIsAdmin);
            pw.WriteInt(map.WeatherID);
            if (!map.WeatherIsAdmin)
                pw.WriteString(map.WeatherMessage);

            if (victim != null)
                victim.SendPacket(pw);
            else
                map.SendPacket(pw);
        }

        public static void SendPlayerMove(GameCharacter chr, MovePath movePath)
        {
            Packet pw = new Packet(ServerMessages.MOVE_PLAYER);
            pw.WriteInt(chr.CharacterStat.ID);
            movePath.EncodeToPacket(pw);

            chr.Field.SendPacket(chr, pw, chr);
        }

        public static void SendChatMessage(GameCharacter who, string message)
        {
            Packet pw = new Packet(ServerMessages.CHAT);
            pw.WriteInt(who.ID);
            pw.WriteBool(who.IsGM && !who.Undercover);
            pw.WriteString(message);

            who.Field.SendPacket(who, pw);
        }

        public static void SendEmotion(GameCharacter chr, int emotion)
        {
            Packet pw = new Packet(ServerMessages.FACIAL_EXPRESSION);
            pw.WriteInt(chr.ID);
            pw.WriteInt(emotion);

            chr.Field.SendPacket(chr, pw, chr);
        }

        public static void SendCharacterLeavePacket(GameCharacter who)
        {
            Packet pw = new Packet(ServerMessages.USER_LEAVE_FIELD);
            pw.WriteInt(who.ID);
            who.Field.SendPacket(who, pw, who);
        }

        public static void SendCharacterLeavePacket(int id, GameCharacter victim)
        {
            Packet pw = new Packet(ServerMessages.USER_LEAVE_FIELD);
            pw.WriteInt(id);
            victim.SendPacket(pw);
        }

        public static void SendCharacterSit(GameCharacter chr, short chairid)
        {
            Packet pw = new Packet(ServerMessages.SIT_RESULT);
            pw.WriteBool(chairid != -1);
            if (chairid != -1)
            {
                pw.WriteShort(chairid);
            }
            chr.SendPacket(pw);
        }

        public static void Kite(GameCharacter chr, Kite Kite)
        {
            Packet pw = new Packet(ServerMessages.MESSAGE_BOX_ENTER_FIELD);
            pw.WriteInt(Kite.ID);
            pw.WriteInt(Kite.ItemID);
            pw.WriteString(Kite.Message);
            pw.WriteString(chr.Name);
            pw.WriteShort(Kite.X);
            pw.WriteShort(Kite.Y); //Should be close enough :P
            chr.Field.SendPacket(Kite, pw);
        }

        public static void RemoveKite(Map Field, Kite Kite, byte LeaveType)
        {
            Packet pw = new Packet(ServerMessages.MESSAGE_BOX_LEAVE_FIELD);
            pw.WriteByte(LeaveType);
            pw.WriteInt(Kite.ID);
            Field.SendPacket(Kite, pw);
        }

        public static void KiteMessage(GameCharacter chr)
        {
            //Can't fly it here
            Packet pw = new Packet(ServerMessages.MESSAGE_BOX_CREATE_FAILED);
            pw.WriteByte(0);
            chr.SendPacket(pw);
        }

        public static void ShowMapTimerForCharacter(GameCharacter chr, int seconds)
        {
            Packet pw = new Packet(ServerMessages.CLOCK);
            pw.WriteByte(0x02);
            pw.WriteInt(seconds);
            chr.SendPacket(pw);
        }

        public static void ShowMapTimerForMap(Map map, int time)
        {
            Packet pw = new Packet(ServerMessages.CLOCK);
            pw.WriteByte(0x02);
            pw.WriteInt(time);
            map.SendPacket(pw);
        }

        public static void SendGMEventInstructions(Map map)
        {
            //Its in korean :S
            Packet pw = new Packet(ServerMessages.DESC); // Could be quiz, dont think so though..
            pw.WriteByte(0x00);
            map.SendPacket(pw);
        }

        public static void SendMapClock(GameCharacter chr, int hour, int minute, int second)
        {
            Packet pw = new Packet(ServerMessages.CLOCK);
            pw.WriteByte(0x01);
            pw.WriteByte((byte)hour);
            pw.WriteByte((byte)minute);
            pw.WriteByte((byte)second);
            chr.SendPacket(pw);
        }

        public static void SendJukebox(Map map, GameCharacter victim)
        {
            Packet pw = new Packet(ServerMessages.PLAY_JUKE_BOX);
            pw.WriteInt(map.JukeboxID);
            if (map.JukeboxID != -1)
                pw.WriteString(map.JukeboxUser);

            if (victim != null)
                victim.SendPacket(pw);
            else
                map.SendPacket(pw);
        }

        public enum PortalBlockedMessage
        {
            ClosedForNow = 1,
            CannotGoToThatPlace = 2,
            CSUnavailable = 3
        }

        public static void BlockedMessage(GameCharacter chr, byte msg) => BlockedMessage(chr, (PortalBlockedMessage)msg);

        public static void BlockedMessage(GameCharacter chr, PortalBlockedMessage msg)
        {
            Packet pw = new Packet(ServerMessages.TRANSFER_FIELD_REQ_IGNORED);
            pw.WriteByte((byte)msg);
            chr.SendPacket(pw);
        }

        public static Packet SpawnPortal(int srcMapId, int destMapId, short destX, short destY)
        {
            //spawns a portal (Spawnpoint in the map you are going to spawn in)
            Packet pw = new Packet(ServerMessages.TOWN_PORTAL);

            pw.WriteInt(destMapId);
            pw.WriteInt(srcMapId);
            pw.WriteShort(destX);
            pw.WriteShort(destY);

            Trace.WriteLine($"TOWN_PORTAL dest: {destMapId} src: {srcMapId} x:{destX} y: {destY}");
            return pw;
        }

        public static void SpawnPortalParty(GameCharacter chr, byte ownerIdIdx, int srcMapId, int destMapId, short destX, short destY)
        {
            Packet pw = new Packet(ServerMessages.PARTY_RESULT);
            pw.WriteByte(26); //door change
            pw.WriteByte(ownerIdIdx);
            pw.WriteInt(destMapId);
            pw.WriteInt(srcMapId);
            pw.WriteShort(destX);
            pw.WriteShort(destY);
            chr.SendPacket(pw);
        }

        public static Packet RemovePortal()
        {
            Packet pw = new Packet(ServerMessages.TOWN_PORTAL);
            pw.WriteInt(Constants.InvalidMap);
            pw.WriteInt(Constants.InvalidMap);
            return pw;
        }

        public static void SendPinkText(GameCharacter chr, string text) //needs work 
        {
            Packet pw = new Packet(ServerMessages.GROUP_MESSAGE);
            pw.WriteByte(1);
            pw.WriteString(chr.Name);
            pw.WriteString(text);
            chr.SendPacket(pw);
        }
        public static Packet CharacterEnterPacket(GameCharacter player)
        {
            Packet pw = new Packet(ServerMessages.USER_ENTER_FIELD);
            pw.WriteInt(player.ID);
            player.EncodeForRemote(pw);
            return pw;
        }

        public static void SendPlayerInfo(GameCharacter chr, Packet packet)
        {
            int id = packet.ReadInt();
            GameCharacter victim = chr.Field.GetPlayer(id);
            if (victim == null || (victim.IsGM && !MasterThread.IsDebug))
            {
                InventoryOperationPacket.NoChange(chr);
                return;
            }

            Packet pw = new Packet(ServerMessages.CHARACTER_INFO); // Idk why this is in mappacket, it's part of CWvsContext
            pw.WriteInt(victim.ID);
            pw.WriteByte(victim.CharacterStat.Level);
            pw.WriteShort(victim.CharacterStat.Job);
            pw.WriteShort(victim.CharacterStat.Fame);
            pw.WriteString(victim.Guild?.Name ?? "");

            var petItem = victim.GetSpawnedPet();
            pw.WriteBool(petItem != null);
            if (petItem != null)
            {
                pw.WriteInt(petItem.ItemID);
                pw.WriteString(petItem.Name);
                pw.WriteByte(petItem.Level);
                pw.WriteShort(petItem.Closeness);
                pw.WriteByte(petItem.Fullness);
                pw.WriteInt(victim.Inventory.GetEquippedItemId(Constants.EquipSlots.Slots.PetAccessory, EquippedType.Cash));
            }

            pw.WriteByte((byte)victim.Wishlist.Count);
            victim.Wishlist.ForEach(pw.WriteInt);

            chr.SendPacket(pw);
        }

        public static void SendAvatarModified(GameCharacter chr, AvatarModFlag flags = 0)
        {
            Server.Instance.CenterConnection.MessengerAvatar(chr);
            Packet pw = new Packet(ServerMessages.AVATAR_MODIFIED);
            pw.WriteInt(chr.CharacterStat.ID);
            pw.WriteByte((byte)flags);

            if (flags.HasFlag(AvatarModFlag.AvatarLook))
            {
                new AvatarLook(chr, false).Encode(pw);
            }
            if (flags.HasFlag(AvatarModFlag.Speed))
            {
                pw.WriteByte(chr.PrimaryStats.TotalSpeed);
            }

            if (flags.HasFlag(AvatarModFlag.ItemEffects))
            {
                pw.WriteByte(0);
            }
            EquipItem coupleRing = chr.Inventory.GetEquippedCoupleRing();
            pw.WriteBool(coupleRing != null);
            if (coupleRing != null)
            {
                coupleRing.EncodeRing(pw);
            }

            chr.Field.SendPacket(chr, pw, chr);
        }

        public static void SendPlayerLevelupAnim(GameCharacter chr)
        {
            Packet pw = new Packet(ServerMessages.SHOW_FOREIGN_EFFECT);
            pw.WriteInt(chr.CharacterStat.ID);
            pw.WriteByte(0x00);

            chr.Field.SendPacket(chr, pw, chr);
        }

        public static void SendPlayerBuffed(GameCharacter chr, BuffValueTypes pBuffs, short delay = 0)
        {
            Packet pw = new Packet(ServerMessages.GIVE_FOREIGN_BUFF);
            pw.WriteInt(chr.CharacterStat.ID);
            BuffPacket.EncodeForRemote(chr, pw, pBuffs);
            pw.WriteShort(delay); // the delay. usually 0, but is carried on through OnStatChangeByMobSkill / DoActiveSkill_(Admin/Party/Self)StatChange

            chr.Field.SendPacket(chr, pw, chr);
        }

        public static void SendPlayerDebuffed(GameCharacter chr, BuffValueTypes buffFlags)
        {
            Packet pw = new Packet(ServerMessages.RESET_TEMPORARY_STAT);
            pw.WriteInt(chr.ID);
            pw.WriteULong((ulong)buffFlags);

            chr.Field.SendPacket(chr, pw, chr);
        }

        public static void EmployeeEnterField(GameCharacter chr) //hired merchant :D
        {
            Packet pw = new Packet(0x83); //not the right opcode
            pw.WriteByte(chr.PortalCount);
            pw.WriteInt(chr.ID);
            pw.WriteByte(0); //??
            pw.WriteInt(chr.MapID);
            pw.WriteInt(295); //Swaglord's ID
            pw.WriteByte(chr.PortalID); //probably spawnpoint 
            pw.WriteShort(chr.Position.X);
            pw.WriteShort(chr.Position.Y);
            pw.WriteInt(1); //??
            pw.WriteShort(chr.HP);
            pw.WriteShort(chr.CharacterStat.MP);
            pw.WriteShort(1); //??
            pw.WriteLong(0);
            pw.WriteLong(0);
            chr.SendPacket(pw);


        }
        public static void SendSetField(GameCharacter chr, bool isConnecting)
        {
            Packet pw = new Packet(ServerMessages.SET_FIELD);
            bool writePos = true;
            pw.WriteInt(Server.Instance.ID);
            pw.WriteByte(chr.PortalCount);
            pw.WriteBool(isConnecting);
            if (isConnecting)
            {
                var rnd = Server.Instance.Randomizer;
                // Seeds are initialized by global randomizer
                var seed1 = rnd.Random();
                var seed2 = rnd.Random();
                var seed3 = rnd.Random();
                var seed4 = rnd.Random();

                chr.CalcDamageRandomizer.SetSeed(seed1, seed2, seed3);
                chr.RndActionRandomizer.SetSeed(seed2, seed3, seed4);

                pw.WriteUInt(seed1);
                pw.WriteUInt(seed2);
                pw.WriteUInt(seed3);
                pw.WriteUInt(seed4);

                new GameObjects.CharacterData(chr).Encode(pw);
            }
            else
            {
                pw.WriteInt(chr.MapID);
                pw.WriteByte(chr.PortalID);
                pw.WriteShort(chr.HP);
                pw.WriteBool(writePos);
                if (writePos)
                {
                    pw.WriteInt(chr.Position.X);
                    pw.WriteInt(chr.Position.Y);
                }
            }
            chr.SendPacket(pw);
        }

        public static void CancelSkillEffect(GameCharacter chr, int skillid)
        {
            Packet pw = new Packet(ServerMessages.SKILL_CANCEL);
            pw.WriteInt(chr.ID);
            pw.WriteInt(skillid);
            chr.Field.SendPacket(pw, chr);
        }

        public static Packet ShowDoor(MysticDoor door, DoorEnterType enterType)
        {
            Packet pw = new Packet(ServerMessages.TOWN_PORTAL_CREATED);
            pw.WriteByte((byte)enterType);
            pw.WriteInt(door.OwnerId);
            pw.WriteShort(door.X);
            pw.WriteShort(door.Y);

            Trace.WriteLine($"TOWN_PORTAL_CREATED enterType: {enterType} ownerId: {door.OwnerId} x: {door.X} y: {door.Y}");

            return pw;
        }

        public static Packet RemoveDoor(int ownerId, byte leaveType)
        {
            Packet pw = new Packet(ServerMessages.TOWN_PORTAL_REMOVED);
            pw.WriteByte(leaveType);
            pw.WriteInt(ownerId);

            Trace.WriteLine($"TOWN_PORTAL_REMOVED leaveType: {leaveType} ownerId: {ownerId}");
            return pw;
        }

        public static void HandleDoorUse(GameCharacter chr, Packet packet)
        {
            int charid = packet.ReadInt();
            Program.MainForm.LogDebug("cid: " + charid);
            bool enterFromTown = packet.ReadBool();
            if (enterFromTown)
            {
                // When you enter from town and go to a training map
                // Resulting map is _not_ a town
                if (chr.Field.DoorPool.TownDoors.TryGetValue(charid, out var door) && door.CanEnterDoor(chr))
                {
                    chr.ChangeMap(door.FieldId, PartyData.GetMemberIdx(charid) ?? 0, door);
                    return;
                }
            }
            else
            {
                // When you enter from a training map
                // Resulting map _is_ a town
                if (chr.Field.DoorPool.TryGetDoor(charid, out var door) && door.CanEnterDoor(chr))
                {
                    chr.ChangeMap(chr.Field.ReturnMap, PartyData.GetMemberIdx(charid) ?? 0, door);
                    return;
                }
            }

            InventoryOperationPacket.NoChange(chr);
        }

        public static Packet ShowSummon(Summon summon, byte enterType)
        {
            Packet pw = new Packet(ServerMessages.SPAWN_ENTER_FIELD);
            pw.WriteInt(summon.OwnerId);
            pw.WriteInt(summon.SkillId);
            pw.WriteByte(summon.SkillLevel);
            pw.WriteShort(summon.Position.X);
            pw.WriteShort(summon.Position.Y);
            pw.WriteBool(summon.MoveAction);
            pw.WriteUShort(summon.FootholdSN);
            if (summon is Puppet p)
            {
                pw.WriteByte(0); //entertype 1 is broken for puppet in v12, idk why
                pw.WriteByte(0);
                pw.WriteByte(0);
            }
            else
            {
                pw.WriteByte(enterType);
                pw.WriteLong(0); //bMoveability? bassist?
            }
            return pw;
        }

        public static Packet RemoveSummon(Summon summon, byte leaveType)
        {
            Packet pw = new Packet(ServerMessages.SPAWN_LEAVE_FIELD);
            pw.WriteInt(summon.OwnerId);
            pw.WriteInt(summon.SkillId);
            pw.WriteByte(leaveType);
            return pw;
        }

        public static void HandleSummonMove(GameCharacter chr, Packet packet)
        {
            var skillId = packet.ReadInt();
            if (chr.Summons.GetSummon(skillId, out var summon))
            {
                var movePath = new MovePath();
                movePath.DecodeFromPacket(packet, MovePath.MovementSource.Summon);
                chr.TryTraceMovement(movePath);

                PacketHelper.ValidateMovePath(summon, movePath);

                SendMoveSummon(chr, summon, movePath);
            }
        }

        private static void SendMoveSummon(GameCharacter chr, Summon summon, MovePath movePath)
        {
            Packet pw = new Packet(ServerMessages.SPAWN_MOVE);
            pw.WriteInt(chr.ID);
            pw.WriteInt(summon.SkillId);
            movePath.EncodeToPacket(pw);

            chr.Field.SendPacket(pw, chr);
        }

        public static void HandleSummonDamage(GameCharacter chr, Packet packet)
        {
            int summonid = packet.ReadInt();
            if (chr.Summons.GetSummon(summonid, out var summon) && summon is Puppet puppet)
            {
                sbyte unk = packet.ReadSByte();
                int damage = packet.ReadInt();
                int mobid = packet.ReadInt();
                byte unk2 = packet.ReadByte();

                SendDamageSummon(chr, puppet, unk, damage, mobid, unk2);

                //Program.MainForm.LogAppend("Damage: " + damage);
                puppet.TakeDamage(damage);
            }
        }

        private static void SendDamageSummon(GameCharacter chr, Puppet summon, sbyte unk, int damage, int mobid, byte unk2)
        {
            // Needs to be fixed.
            Packet pw = new Packet(ServerMessages.SPAWN_HIT);
            pw.WriteInt(chr.ID);
            pw.WriteInt(summon.SkillId);
            pw.WriteSByte(-1);
            pw.WriteInt(damage);
            pw.WriteInt(mobid);

            pw.WriteByte(0);
            pw.WriteLong(0);
            pw.WriteLong(0);
            pw.WriteLong(0);
            chr.Field.SendPacket(pw, chr);
        }


    }
}