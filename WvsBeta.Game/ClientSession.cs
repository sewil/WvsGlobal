using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using log4net;
using WvsBeta.Common;
using WvsBeta.Common.Sessions;
using WvsBeta.Common.Tracking;
using WvsBeta.Game.GameObjects.MiniRoom;
using WvsBeta.Game.Handlers;
using WvsBeta.Game.Handlers.Guild;
using WvsBeta.Game.Packets;

namespace WvsBeta.Game
{
    public class ClientSession : ConnectionSession
    {
        private static ILog log = LogManager.GetLogger("ClientSession");
        public Player Player { get; }
        public bool Loaded { get; set; }


        public ClientSession(Socket pSocket) : base(pSocket, false)
        {
            Loaded = false;

            Player = new Player
            {
                Socket = this,
                Character = null
            };
            Server.Instance.AddPlayer(Player);
            Pinger.Add(this);

            SendHandshake(Constants.MAPLE_VERSION, Constants.MAPLE_PATCH_LOCATION, Constants.MAPLE_LOCALE);
            SendMemoryRegions();
        }

        public override void StartLogging()
        {
            base.StartLogging();
            Player?.Character?.SetupLogging();
        }

        public override void EndLogging()
        {
            base.EndLogging();
            GameCharacter.RemoveLogging();
        }

        public override void OnDisconnect()
        {
            try
            {
                StartLogging();

                if (Loaded && Player.Character != null)
                {
                    var chr = Player.Character;
                    var cc = Player.IsCC;
                    var migrating = Server.Instance.InMigration && !Server.Instance.IsNewServerInMigration;

                    Program.MainForm.LogAppend($"{chr.Name} disconnected. CC? {cc} Migrating? {migrating}");

                    try
                    {
                        if (chr.DoorMapId != Constants.InvalidMap)
                        {
                            DataProvider.Maps[chr.DoorMapId].DoorPool.TryRemoveDoor(chr.ID);
                        }

                        if (chr.MapChair != -1 && !migrating)
                        {
                            chr.Field.UsedSeats.Remove(chr.MapChair);
                            chr.MapChair = -1;
                            MapPacket.SendCharacterSit(chr, -1);
                        }
                    }
                    catch (Exception ex)
                    {
                        Program.MainForm.LogAppend(ex.ToString());
                    }

                    try
                    {
                        chr.CleanupInstances();
                    }
                    catch (Exception ex)
                    {
                        Program.MainForm.LogAppend(ex.ToString());
                    }

                    try
                    {
                        chr.Field.LeavePlayer(chr);
                        chr.Summons.RemoveAllSummons();
                    }
                    catch (Exception ex)
                    {
                        Program.MainForm.LogAppend(ex.ToString());
                    }

                    try
                    {
                        chr.FlushDamageLog(true);
                    }
                    catch (Exception ex)
                    {
                        Program.MainForm.LogAppend(ex.ToString());
                    }

                    try
                    {
                        if (chr.GuildID > 0)
                        {
                            GuildHandler.SendMemberIsOnline(chr, false);
                        }
                    }
                    catch (Exception ex)
                    {
                        Program.MainForm.LogAppend(ex.ToString());
                    }

                    // Clean up potential guild invites
                    GuildHandler.Invites.Remove(chr.ID);

                    Server.Instance.CharacterList.Remove(chr.ID);
                    Server.Instance.StaffCharacters.Remove(chr);

                    chr.Save();

                    Server.Instance.CenterConnection?.UnregisterCharacter(chr.ID, cc);

                    Program.MainForm.ChangeLoad(false);

                    if (!cc)
                    {
                        RedisBackend.Instance.RemovePlayerOnline(chr.UserID);
                    }
                    RedisBackend.Instance.RemovePlayerCCIsBeingProcessed(chr.ID);
                    Player.Character = null;
                }

                Player.Socket = null;
                Server.Instance.RemovePlayer(Player.SessionHash);
            }
            catch (Exception ex)
            {
                Program.MainForm.LogAppend(ex.ToString());
            }
            finally
            {
                EndLogging();
                Pinger.Remove(this);
            }
        }

        private static readonly HashSet<ClientMessages> ignoreClientPackets = new HashSet<ClientMessages>
        {
            ClientMessages.PONG, ClientMessages.MOVE_PLAYER, ClientMessages.HEAL_OVER_TIME, ClientMessages.MOB_MOVE, ClientMessages.MOB_APPLY_CONTROL,
            ClientMessages.TAKE_DAMAGE, ClientMessages.CLIENT_HASH, ClientMessages.NPC_ANIMATE
        };

        private static readonly HashSet<ServerMessages> ignoreServerPackets = new HashSet<ServerMessages>
        {
            ServerMessages.PING,
            ServerMessages.MOVE_PLAYER, ServerMessages.STAT_CHANGED,
            ServerMessages.NPC_ANIMATE, ServerMessages.NPC_CHANGE_CONTROLLER,
            ServerMessages.MOB_CHANGE_CONTROLLER, ServerMessages.MOB_MOVE, ServerMessages.MOB_CTRL_ACK, ServerMessages.MOB_ENTER_FIELD,
            ServerMessages.DAMAGE_PLAYER,
            ServerMessages.UPDATE_PARTYMEMBER_HP
        };

        public override void AC_OnPacketInbound(Packet packet)
        {
            ClientMessages header = 0;
            try
            {
                header = (ClientMessages)packet.ReadByte();

                if (!Loaded || Player?.Character == null)
                {
                    switch (header)
                    {
                        case ClientMessages.MIGRATE_IN:
                            OnPlayerLoad(packet);
                            break; //updated
                    }
                }
                // Block packets as we are migrating
                else if (Server.Instance.InMigration == false || Server.Instance.IsNewServerInMigration)
                {
                    var character = Player.Character;

                    if (!ignoreClientPackets.Contains(header))
                        Program.MainForm.LogAppend($"[{character.Name}->CH{Player.Channel}][{header}] {packet}");

                    switch (header)
                    {
                        case ClientMessages.RETURN_TO_LOGIN:
                            ReturnToLoginPacket.Handle(this, packet, log);
                            break;
                        case ClientMessages.ENTER_PORTAL:
                            MapPacket.OnEnterPortal(packet, character);
                            break;
                        case ClientMessages.ENTER_SCRIPTED_PORTAL:
                            MapPacket.OnEnterScriptPortal(packet, character);
                            break;
                        case ClientMessages.CHANGE_CHANNEL:
                            OnChangeChannel(character, packet);
                            break;
                        case ClientMessages.ENTER_CASH_SHOP:
                            OnEnterCashShop(character);
                            break;
                        case ClientMessages.MOVE_PLAYER:
                            MapPacket.HandleMove(character, packet);
                            break;
                        case ClientMessages.SIT_REQUEST:
                            MapPacket.HandleSitChair(character, packet);
                            break;
                        case ClientMessages.ENTER_TOWN_PORTAL:
                            MapPacket.HandleDoorUse(character, packet);
                            break;
                        case ClientMessages.CLOSE_RANGE_ATTACK:
                            AttackPacket.HandleMeleeAttack(character, packet);
                            break;
                        case ClientMessages.RANGED_ATTACK:
                            AttackPacket.HandleRangedAttack(character, packet);
                            break;
                        case ClientMessages.MAGIC_ATTACK:
                            AttackPacket.HandleMagicAttack(character, packet);
                            break;
                        case ClientMessages.TAKE_DAMAGE:
                            CharacterStatsPacket.HandleCharacterDamage(character, packet);
                            break;
                        case ClientMessages.CLIENT_HASH:
                            break;
                        case ClientMessages.CHAT:
                            ChatPacket.HandleChat(character, packet);
                            break;
                        case ClientMessages.QUEST_ACTION:
                            QuestPacket.HandleAction(character, packet);
                            break;
                        case ClientMessages.GROUP_MESSAGE:
                            ChatPacket.HandleGroupMessage(character, packet);
                            break;
                        case ClientMessages.WHISPER:
                            ChatPacket.HandleCommand(character, packet);
                            break;
                        case ClientMessages.EMOTE:
                            MapPacket.SendEmotion(character, packet.ReadInt());
                            break;

                        case ClientMessages.NPC_TALK:
                            MapPacket.HandleNPCChat(character, packet);
                            break;
                        case ClientMessages.NPC_TALK_MORE:
                            NpcPacket.HandleNPCChat(character, packet);
                            break;
                        case ClientMessages.SHOP_ACTION:
                            NpcPacket.HandleNPCShop(character, packet);
                            break;
                        case ClientMessages.STORAGE_ACTION:
                            StoragePacket.HandleStorage(character, packet);
                            break;

                        case ClientMessages.ITEM_MOVE:
                            InventoryPacket.HandleInventoryPacket(character, packet);
                            break;
                        case ClientMessages.ITEM_USE:
                            InventoryPacket.HandleUseItemPacket(character, packet);
                            break;
                        case ClientMessages.SUMMON_BAG_USE:
                            InventoryPacket.HandleUseSummonSack(character, packet);
                            break;
                        case ClientMessages.CASH_ITEM_USE:
                            CashPacket.HandleCashItem(character, packet);
                            break;
                        case ClientMessages.RETURN_SCROLL_USE:
                            InventoryPacket.HandleUseReturnScroll(character, packet);
                            break;
                        case ClientMessages.SCROLL_USE:
                            InventoryPacket.HandleScrollItem(character, packet);
                            break;

                        case ClientMessages.DISTRIBUTE_AP:
                            CharacterStatsPacket.HandleStats(character, packet);
                            break;
                        case ClientMessages.HEAL_OVER_TIME:
                            CharacterStatsPacket.HandleHeal(character, packet);
                            break;
                        case ClientMessages.DISTRIBUTE_SP:
                            SkillPacket.HandleAddSkillLevel(character, packet);
                            break;
                        case ClientMessages.PREPARE_SKILL:
                            SkillPacket.HandlePrepareSkill(character, packet);
                            break;
                        case ClientMessages.GIVE_BUFF:
                            SkillPacket.HandleUseSkill(character, packet);
                            break;
                        case ClientMessages.CANCEL_BUFF:
                            SkillPacket.HandleStopSkill(character, packet);
                            break;

                        case ClientMessages.DROP_MESOS:
                            DropPacket.HandleDropMesos(character, packet.ReadInt());
                            break;
                        case ClientMessages.GIVE_FAME:
                            FamePacket.HandleFame(character, packet);
                            break;
                        case ClientMessages.CHAR_INFO_REQUEST:
                            MapPacket.SendPlayerInfo(character, packet);
                            break;
                        case ClientMessages.SPAWN_PET:
                            PetsPacket.HandleSpawnPet(character, packet.ReadShort());
                            break;
                        case ClientMessages.SUMMON_MOVE:
                            MapPacket.HandleSummonMove(character, packet);
                            break;
                        case ClientMessages.GUILD_ACTION:
                            GuildHandler.HandleAction(character, packet);
                            break;
                        case ClientMessages.GUILD_DECLINE_INVITATION:
                            GuildHandler.HandleDeclineInvitation(character, packet);
                            break;
                        case ClientMessages.SUMMON_ATTACK:
                            AttackPacket.HandleSummonAttack(character, packet);
                            break;

                        case ClientMessages.SUMMON_DAMAGED:
                            MapPacket.HandleSummonDamage(character, packet);
                            break;

                        case ClientMessages.MOB_MOVE:
                            MobPacket.HandleMobControl(character, packet);
                            break;

                        case ClientMessages.NPC_ANIMATE:
                            MapPacket.HandleNPCAnimation(character, packet);
                            break;

                        case ClientMessages.PET_MOVE: PetsPacket.HandleMovePet(character, packet); break;
                        case ClientMessages.PET_INTERACTION: PetsPacket.HandleInteraction(character, packet); break;
                        case ClientMessages.PET_ACTION: PetsPacket.HandlePetAction(character, packet); break;

                        case ClientMessages.FIELD_CONTIMOVE_STATE:
                            MapPacket.OnContiMoveState(character, packet);
                            break;

                        case ClientMessages.DROP_PICK_UP:
                            DropPacket.HandlePickupDrop(character, packet);
                            break;

                        case ClientMessages.MESSENGER:
                            MessengerHandler.HandleMessenger(character, packet);
                            break;

                        case ClientMessages.MINI_ROOM_OPERATION:
                            MiniRoomPacket.HandlePacket(character, packet);
                            break;
                        case ClientMessages.FRIEND_OPERATION:
                            BuddyHandler.HandleBuddy(character, packet);
                            break;
                        case ClientMessages.PARTY_OPERATION:
                            PartyHandler.HandleParty(character, packet);
                            break;

                        case ClientMessages.DENY_PARTY_REQUEST:
                            PartyHandler.HandleDecline(character, packet);
                            break;

                        case ClientMessages.REACTOR_HIT:
                            ReactorPacket.HandleReactorHit(character, packet);
                            break;

                        case ClientMessages.REPORT_USER:
                            MiscPacket.ReportPlayer(character, packet);
                            break;
                            

                        //this is a garbage opcode that i use when doing janky client packet workarounds. This is where packets go to die.
                        case ClientMessages.JUNK:
                            Program.MainForm.LogDebug("received junk packet");
                            break;

                        // eh.. ignore?
                        // Happens when one of the following buffs are set:
                        // Stun, Poison, Seal, Darkness, Weakness, Curse
                        // Maybe patch out of the client
                        case ClientMessages.CHARACTER_IS_DEBUFFED: break;

                        // TODO: Implement???
                        case ClientMessages.MOB_APPLY_CONTROL: break;

                        case ClientMessages.PONG:
                            // Make sure we update the player online thing
                            RedisBackend.Instance.SetPlayerOnline(
                                character.UserID,
                                Server.Instance.GetOnlineId()
                            );

                            // Cleanup expired items
                            character.Inventory.CheckExpired();
                            break;

                        default:
                            if (character.Field.HandlePacket(character, packet, header) == false)
                            {
                                Program.MainForm.LogAppend(
                                    "[{0}] Unknown packet received! " + packet,
                                    header
                                );
                            }

                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Program.MainForm.LogAppend($"---- ERROR ----\r\n{ex}");
                Program.MainForm.LogAppend($"Packet: {packet}");
                FileWriter.WriteLine(@"etclog\ExceptionCatcher.log", "[Game Server " + Server.Instance.ID + "][" + DateTime.Now + "] Exception caught: " + ex, true);
                //Disconnect();
            }


#if DEBUG
            if (packet.Length != packet.Position)
            {
                var packetStr = packet.ToString();
                packetStr = packetStr.Substring(0, packet.Position * 3 - 1) + "-x-" + packetStr.Substring(packet.Position * 3);

                log.Debug($"Did not read full message in packet: {header} {packetStr}");
            }
#endif
        }

        public override void OnHackDetected()
        {
            if (!Loaded || !HackDetected.HasValue) return;
            var character = Player.Character;
            var hack = HackDetected.Value;
            if (hack.HasFlag(RedisBackend.HackKind.Speedhack))
            {
                ChatPacket.SendNoticeGMs(
                    $"Detected speed hacks on character '{character.Name}', map {character.MapID}...",
                    ChatPacket.MessageTypes.RedText);

                if (character.IsGM == false)
                {
                    character.PermaBan(
                        "Detected speedhack",
                        extraDelay: (int)((2 * 60) + Rand32.Next() % (5 * 60))
                    );
                }
            }

            if (hack.HasFlag(RedisBackend.HackKind.MemoryEdits))
            {
                ChatPacket.SendNoticeGMs(
                    $"Detected memory edits on character '{character.Name}', map {character.MapID}.",
                    ChatPacket.MessageTypes.RedText
                );

                if (character.IsGM == false)
                {
                    // Add some randomness
                    character.PermaBan(
                        "Detected memory edits",
                        // Between 2 and 12 minutes
                        extraDelay: (int)((2 * 60) + Rand32.Next() % (10 * 60))
                    );
                }
            }

        }

        public void OnChangeChannel(GameCharacter character, Packet packet)
        {
            if (character.Field.DisableChangeChannel)
            {
                MapPacket.BlockedMessage(character, MapPacket.PortalBlockedMessage.CannotGoToThatPlace);
                return;
            }

            var channel = packet.ReadByte();
            DoChangeChannelReq(channel);
        }

        public void OnEnterCashShop(GameCharacter character)
        {
            if (character.Field.DisableGoToCashShop)
            {
                MapPacket.BlockedMessage(character, MapPacket.PortalBlockedMessage.CannotGoToThatPlace);
                return;
            }

            Server.Instance.CenterConnection.RequestCharacterConnectToWorld(
                Player.SessionHash,
                character.ID,
                Server.Instance.WorldID,
                50,
                character
            );
        }

        public void DoChangeChannelReq(byte channel)
        {
            Server.Instance.CenterConnection.RequestCharacterConnectToWorld(Player.SessionHash, Player.Character.ID, Server.Instance.WorldID, channel, Player.Character);
        }


        public void SendConnectToServer(byte[] IP, ushort port, bool noScheduledDisconnect = false)
        {
            var pw = new Packet(ServerMessages.MIGRATE_COMMAND);
            pw.WriteBool(true);
            pw.WriteBytes(IP);
            pw.WriteUShort(port);
            SendPacket(pw);

            if (!noScheduledDisconnect)
            {
                ScheduleDisconnect();
            }
        }

        public void OnPlayerLoad(Packet packet)
        {
            var characterId = packet.ReadInt();
            ThreadContext.Properties["CharacterID"] = characterId;

            if (RedisBackend.Instance.HoldoffPlayerConnection(characterId))
            {
                Program.MainForm.LogAppend("Bouncing charid: " + characterId);
                SendConnectToServer(Server.Instance.PublicIP.GetAddressBytes(), Server.Instance.Port, true);
                return;
            }


            if (RedisBackend.Instance.PlayerIsMigrating(characterId, true) == false)
            {
                var msg = "Disconnecting because not migrating. Charid: " + characterId;
                Server.Instance.ServerTraceDiscordReporter.Enqueue(msg);
                Program.MainForm.LogAppend(msg);
                goto cleanup_and_disconnect;
            }


            var uId = Server.Instance.CharacterDatabase.UserIDByCharID(characterId);
            ThreadContext.Properties["UserID"] = uId;
            if (Server.Instance.CharacterDatabase.IsBanned(uId))
            {
                var msg = "Disconnecting because banned. Charid: " + characterId + ". Userid: " + uId;
                Server.Instance.ServerTraceDiscordReporter.Enqueue(msg);
                Program.MainForm.LogAppend(msg);
                goto cleanup_and_disconnect;
            }

            if (Server.Instance.CharacterDatabase.IsIpBanned(IP))
            {
                var msg = "Disconnecting because IP banned. Charid: " + characterId + ". Userid: " + uId + ". IP: " + IP;
                Server.Instance.ServerTraceDiscordReporter.Enqueue(msg);
                Program.MainForm.LogAppend(msg);
                goto cleanup_and_disconnect;
            }

            if (Server.Instance.CharacterList.ContainsKey(characterId))
            {
                var msg = "Disconnecting characterId " + characterId + " from IP " + IP + ". Already connected in this channel.";
                Server.Instance.ServerTraceDiscordReporter.Enqueue(msg);
                Program.MainForm.LogAppend(msg);
                goto cleanup_and_disconnect;
            }

            var character = new GameCharacter(characterId);
            var loadResult = character.Load(IP);
            if (loadResult != GameCharacter.LoadFailReasons.None)
            {
                var msg = "Disconnected characterId " + characterId + " from IP " + IP + ". " + loadResult;
                Server.Instance.ServerTraceDiscordReporter.Enqueue(msg);
                Program.MainForm.LogAppend(msg);
                goto cleanup_and_disconnect;
            }

            Player.Character = character;
            character.Player = Player;

            StartLogging();

            Program.MainForm.LogAppend(character.Name + " connected from IP " + IP + ".");

            Program.MainForm.ChangeLoad(true);
            Server.Instance.CharacterList.Add(characterId, character);
            if (character.IsGM)
                Server.Instance.StaffCharacters.Add(character);
            Loaded = true;

            //have to load summons after he joins the map, so i have moved this from the load method -Exile
            if (Server.Instance.CCIngPlayerList.TryGetValue(character.ID, out var info))
            {
                Server.Instance.CCIngPlayerList.Remove(character.ID);
            }

            var ccPacket = info?.Item1;

            if (ccPacket != null)
            {
                character.PrimaryStats.DecodeForCC(ccPacket);
            }

            character.PrimaryStats.CheckHPMP();

            MapPacket.SendSetField(character);
            if (character.Guild != null)
            {
                GuildHandler.SendGuild(character, character.Guild);
            }

            if (character.IsGM)
            {
                string glevel = character.GMLevel == 1 ? "(GM Intern)" : character.GMLevel == 2 ? "(GM)" : "(Admin)";
                ChatPacket.SendNotice("Your GM Level: " + character.GMLevel + " " + glevel + ". Undercover? " + (character.Undercover ? "Yes" : "No"), character);
            }

            if (character.IsGM && !character.Undercover)
            {
                character.TryActivateHide();
            }

            character.Field.AddPlayer(character);

            if (ccPacket != null)
            {
                character.Summons.DecodeForCC(ccPacket);
            }

            ChatPacket.SendText(ChatPacket.MessageTypes.Header, Server.Instance.ScrollingHeader, character, ChatPacket.MessageMode.ToPlayer);

            Server.Instance.CenterConnection.RegisterCharacter(character.ID, character.Name, character.CharacterStat.Job, character.CharacterStat.Level, character.GMLevel);

            Server.Instance.CenterConnection.RequestBuddyListLoad(character.Name, false, character.BuddyListCapacity); //Sends a packet that request the buddylistload from the centerserver 
            character.IsOnline = true;
            Server.Instance.CenterConnection.PlayerUpdateMap(character);

            // Just to be sure, check if he was banned

            if (RedisBackend.Instance.TryGetNonGameHackDetect(Player.Character.UserID, out var hax))
            {
                HackDetected = hax;
                OnHackDetected();
            }
            else if (HackDetected.HasValue)
                OnHackDetected();

            return;


            cleanup_and_disconnect:

            Server.Instance.CCIngPlayerList.Remove(characterId);
            Disconnect();
        }

        public override void SendPacket(Packet pPacket)
        {
            var header = (ServerMessages)pPacket.Opcode;
            if (!ignoreServerPackets.Contains(header))
                Program.MainForm.LogAppend($"[CH{Player.Channel}->{Player.Character.Name}][{header}] {pPacket}");
            base.SendPacket(pPacket);
        }
    }
}