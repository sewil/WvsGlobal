using System.Linq;
using log4net;
using WvsBeta.Common.Character;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Objects;
using WvsBeta.Common.Sessions;
using WvsBeta.Game.Handlers.MiniRoom;

namespace WvsBeta.Game.GameObjects.MiniRoom
{
    public static class MiniRoomPacket
    {
        private static ILog miniroomLog = LogManager.GetLogger("MiniroomLog");
        private static ILog miniroomChatLog = LogManager.GetLogger("MiniroomChatLog");

        public static void HandlePacket(GameCharacter pCharacter, Packet pPacket)
        {
            //MessagePacket.SendNotice("PACKET: " + pPacket.ToString(), pCharacter);
            var action = (MiniRoomOpClient)pPacket.ReadByte();

            switch (action)
            {
                case MiniRoomOpClient.Create: // Create miniroom
                    {
                        if (pCharacter.AssertForHack(!pCharacter.CanAttachAdditionalProcess, "Trying to create a miniroom while he cannot attach additional process."))
                        {
                            return;
                        }
                        CreateMiniRoomBase(pCharacter, pPacket);
                        break;
                    }

                case MiniRoomOpClient.Invite: // Invite To miniroom
                    {
                        if (pCharacter.Room == null)
                        {
                            InviteResult(pCharacter, 1);
                            return; // NOT OPENED OR FULL
                        }

                        int playerid = pPacket.ReadInt();
                        GameCharacter victim = pCharacter.Field.GetPlayer(playerid);

                        if (victim == null)
                        {
                            miniroomLog.Info($"{pCharacter.Name} fails to invite charid {playerid}: not found?");
                            // Not found!
                            InviteResult(pCharacter, 1);
                        }
                        else if (pCharacter.Room.IsFull())
                        {
                            miniroomLog.Info($"{pCharacter.Name} fails to invite charid {playerid}: room already full?");
                            InviteResult(pCharacter, 2, victim.Name); // DEM REAL DEAL
                        }
                        #if !DEBUG
                        else if ((!pCharacter.IsGM && victim.IsGM) || (pCharacter.IsGM && !victim.IsGM))
                        {
                            miniroomLog.Info($"{pCharacter.Name} fails to invite charid {playerid}: non-admin tried to invite admin or vice versa");

                            InviteResult(pCharacter, 1);
                        }
                        #endif
                        else
                        {
                            miniroomLog.Info($"{pCharacter.Name} invited {victim.Name} (charid {playerid})");
                            Invite(pCharacter.Room, pCharacter, victim);
                        }

                        break;
                    }

                case MiniRoomOpClient.DeclineInvite: // Decline Invite
                    {
                        int roomid = pPacket.ReadInt();

                        miniroomLog.Info($"{pCharacter.Name} declined invite.");
                        if (!MiniRoomBase.MiniRooms.ContainsKey(roomid))
                        {
                            // REPORT
                            //ReportManager.FileNewReport("Tried opening a trade room without a proper ID.", pCharacter.ID, 0);
                            //MessagePacket.SendNotice("Tried opening a trade room without a proper ID. ID was: " + roomid.ToString(), pCharacter);
                            return;
                        }

                        MiniRoomBase mrb = MiniRoomBase.MiniRooms[roomid];
                        //if (mrb.IsFull())
                        //{

                        //}
                        break;
                    }

                case MiniRoomOpClient.Enter: // Enter Room
                    {
                        EnterMiniRoom(pCharacter, pPacket);
                        break;
                    }

                case MiniRoomOpClient.Chat: // Chat
                    {
                        if (pCharacter.Room == null) return;

                        var text = pPacket.ReadString();

                        var chatLogLine = pCharacter.Name + ": " + text;
                        if (ChatPacket.ShowMuteMessage(pCharacter))
                        {
                            miniroomChatLog.Info("[MUTED] " + chatLogLine);
                        }
                        else
                        {
                            miniroomChatLog.Info(chatLogLine);
                            Chat(pCharacter.Room, pCharacter, text, -1);
                        }

                        break;
                    }

                case MiniRoomOpClient.AddShopItem: //Add item to Player Shop
                    {
                        var room = pCharacter.Room;
                        if (room?.Type != MiniRoomType.PlayerShop) return;

                        Inventory inventory = (Inventory)pPacket.ReadByte();
                        short inventoryslot = pPacket.ReadShort();
                        short bundleamount = pPacket.ReadShort();
                        short AmountPerBundle = pPacket.ReadShort();
                        int price = pPacket.ReadInt();
                        ((PlayerShop)room).HandleShopUpdateItem(pCharacter, inventory, inventoryslot, bundleamount, AmountPerBundle, price);
                        break;
                    }

                case MiniRoomOpClient.BuyShopItem: //Buy item from shop
                    {
                        if (pCharacter.Room == null) return;

                        byte slot = pPacket.ReadByte();
                        short bundleamount = pPacket.ReadShort();
                        PlayerShop ps = (PlayerShop)MiniRoomBase.MiniRooms[pCharacter.Room.ID];

                        if (ps != null)
                        {
                            ps.BuyItem(pCharacter, slot, bundleamount);
                        }

                        break;
                    }

                case MiniRoomOpClient.Leave: //Leave
                    {
                        MiniRoomBase mr = pCharacter.Room;
                        if (mr == null) return;

                        miniroomLog.Info($"{pCharacter.Name} declined invite.");

                        if (mr.Type == MiniRoomType.Trade)
                        {
                            mr.Close(true, MiniRoomLeaveReason.Cancel);
                        }
                        else if (mr.Type == MiniRoomType.PlayerShop)
                        {
                            mr.RemovePlayer(pCharacter, MiniRoomLeaveReason.YouHaveLeft);
                        }
                        else if (mr.Type == MiniRoomType.Omok)
                        {
                            //MessagePacket.SendNotice("leave omok", pCharacter);
                            Omok omok = (Omok)MiniRoomBase.MiniRooms[pCharacter.Room.ID];

                            if (pCharacter == omok.Owner)
                            {
                                omok.Close(true, MiniRoomLeaveReason.RoomIsClosed);
                            }
                            else
                            {
                                omok.RemovePlayer(pCharacter, MiniRoomLeaveReason.YouHaveLeft);
                            }
                        }

                        break;
                    }

                case MiniRoomOpClient.AddAnnounceBox: //Add announce box
                    {
                        var room = pCharacter.Room;
                        if (room == null || !(room is BalloonRoom)) return;
                        MiniRoomBalloonPacket.Send(pCharacter, room as BalloonRoom);

                        switch (room.Type)
                        {
                            case MiniRoomType.Omok:
                                {
                                    pCharacter.Field.Omoks.Add(room.ID, (Omok)room);
                                    break;
                                }
                            case MiniRoomType.PlayerShop:
                                {
                                    pCharacter.Field.PlayerShops.Add(room.ID, (PlayerShop)room);
                                    break;
                                }
                        }
                        break;
                    }

                case MiniRoomOpClient.MoveItemShopToInv: //Move Item from player shop to inventory
                    {
                        return;
                        if (pCharacter.AssertForHack(!(pCharacter.Room is PlayerShop), "PlayerShop hack: taking back item while not in playershop")) return;
                        byte slot = pPacket.ReadByte(); //reads as byte, sends as short... wtf lol
                        PlayerShop ps = pCharacter.Room as PlayerShop;
                        if (pCharacter.AssertForHack(ps.Owner != pCharacter, "PlayerShop hack: taking back item while not owner")) return;

                        ps.HandleMoveItemBack(pCharacter, slot);
                        ps.Items.Remove(slot);
                        break;
                    }

                case MiniRoomOpClient.RequestTie:
                    {
                        MiniGamePacket.RequestTie(pCharacter, pCharacter.Room);
                        break;
                    }
                case MiniRoomOpClient.RequestTieResult:
                    {
                        bool result = pPacket.ReadBool();
                        if (result && MiniRoomBase.MiniRooms.TryGetValue(pCharacter.Room.ID, out MiniRoomBase omok))
                        {
                            ((Omok)omok).UpdateGame(pCharacter, GameResult.Tie);
                        }
                        else
                        {
                            MiniGamePacket.RequestTieDeny(pCharacter, pCharacter.Room);
                        }
                        break;
                    }
                case MiniRoomOpClient.GiveUp:
                    {
                        if (MiniRoomBase.MiniRooms.TryGetValue(pCharacter.Room.ID, out MiniRoomBase omok))
                        {
                            GameCharacter winner = omok.Users[(byte)(pCharacter.RoomSlotId == 0 ? 1 : 0)];
                            ((Omok)omok).UpdateGame(winner, GameResult.Forfeit);
                        }
                        break;
                    }

                case MiniRoomOpClient.Ready: //Ready
                    {
                        MiniGamePacket.Ready(pCharacter, pCharacter.Room);
                        break;
                    }

                case MiniRoomOpClient.Unready:
                    {
                        MiniGamePacket.UnReady(pCharacter, pCharacter.Room);
                        break;
                    }

                case MiniRoomOpClient.Expel:
                    {
                        pCharacter.Room.RemovePlayer(pCharacter.Room.Users[1], MiniRoomLeaveReason.Expelled);
                        break;
                    }

                case MiniRoomOpClient.StartOmok:
                    {
                        Omok omok = (Omok)MiniRoomBase.MiniRooms[pCharacter.Room.ID];
                        omok.Start(pCharacter);
                        break;
                    }

                case MiniRoomOpClient.OmokWinner:
                    {
                        Omok omok = (Omok)MiniRoomBase.MiniRooms[pCharacter.Room.ID];
                        omok.UpdateGame(pCharacter, GameResult.Win);
                        break;
                    }

                case MiniRoomOpClient.PlaceOmokPiece: //Place omok piece
                    {
                        Omok omok = (Omok)MiniRoomBase.MiniRooms[pCharacter.Room.ID];
                        int X = pPacket.ReadInt();
                        int Y = pPacket.ReadInt();
                        byte type = pPacket.ReadByte();

                        if (omok.Board[X, Y] != type && omok.Board[X, Y] != omok.GetOtherType(type))
                        {
                            MiniGamePacket.MoveOmokPiece(pCharacter, pCharacter.Room, X, Y, type);
                            var stone = new OmokStone(X, Y, type);
                            omok.AddStone(stone, pCharacter);
                        }
                        else
                        {
                            MiniGamePacket.OmokMessage(pCharacter, pCharacter.Room, 0);
                        }
                        //MessagePacket.SendNotice("X : " + X + " Y : " + Y, pCharacter);
                        if (omok.CheckStone(type))
                        {
                            //MessagePacket.SendNotice("Win!", pCharacter);
                            omok.UpdateGame(pCharacter, GameResult.Win);
                        }
                        break;
                    }

                case MiniRoomOpClient.RequestHandicap:
                    {
                        MiniGamePacket.RequestHandicap(pCharacter, pCharacter.Room);
                        break;
                    }

                case MiniRoomOpClient.RequestHandicapResult: //Request handicap result
                    {
                        bool result = pPacket.ReadBool();
                        Omok omok = (Omok)MiniRoomBase.MiniRooms[pCharacter.Room.ID];

                        int otherPlrIdx = pCharacter.RoomSlotId == 0 ? 1 : 0;
                        byte countBack = omok.mCurrentTurnIndex == otherPlrIdx ? (byte)2 : (byte)1;
                        for (byte i = 0; i < countBack; i++)
                        {
                            int stoneIdx = omok.Stones.Count - 1;
                            omok.RemoveStone(stoneIdx);
                        }
                        MiniGamePacket.RequestHandicapResult(pCharacter, pCharacter.Room, result, countBack);
                        break;
                    }

                default:
                    {
                        if (pCharacter.Room != null)
                        {
                            pCharacter.Room.OnPacket(pCharacter, action, pPacket);
                        }
                        //MessagePacket.SendNotice("This feature is currently disabled due to maintenance.", pCharacter);
                        break;
                    }
            }
        }

        private static void CreateMiniRoomBase(GameCharacter chr, Packet packet)
        {
            if (chr.Room != null)
            {
                return;
            }

            var nType = (MiniRoomType)packet.ReadByte();

            switch (nType)
            {
                case MiniRoomType.Omok:
                    {
                        miniroomLog.Info($"{chr.Name} creates an omok miniroom");
                        string title = packet.ReadString();
                        bool isPrivate = packet.ReadBool();
                        string password = null;
                        if (isPrivate) password = packet.ReadString();
                        byte pieceType = packet.ReadByte();
                        Omok omok = new Omok(chr, title, isPrivate, password, pieceType);
                        break;
                    }

                case MiniRoomType.MatchCards: // Match Cards TODO!
                    {
                        miniroomLog.Info($"{chr.Name} creates a match cards");
                        string title = packet.ReadString();
                        bool isPrivate = packet.ReadBool();
                        string password = null;
                        if (isPrivate) password = packet.ReadString();
                        packet.Skip(7);
                        byte cardType = packet.ReadByte();
                        MatchCard mc = new MatchCard(chr, title, isPrivate, password, cardType);
                        break;
                    }

                case MiniRoomType.Trade:
                    {
                        miniroomLog.Info($"{chr.Name} creates a trade miniroom");
                        Trade trade = new Trade(chr);
                        break;
                    }

                case MiniRoomType.PlayerShop:
                    { // TODO: Handle shops overlap
                        miniroomLog.Info($"{chr.Name} creates a player shop miniroom");
                        string title = packet.ReadString();
                        bool isPrivate = packet.ReadBool();
                        short x = packet.ReadShort(); // might be type of shop (different shops had different outer designs/looks)? unused var. not sure what it's purpose it serves.
                        int objectId = packet.ReadInt();
                        PlayerShop shop = new PlayerShop(chr, title, objectId);
                        break;
                    }
            }
        }

        private static void EnterMiniRoom(GameCharacter chr, Packet packet)
        {
            if (chr.Room != null)
            {
                miniroomLog.Info($"{chr.Name} cannot enter miniroom: already in one.");
                return; // Already in a Mini Room
            }

            
            //MessagePacket.SendNotice("PACKET: " + packet.ToString(), chr);
            int roomId = packet.ReadInt();
            if (!MiniRoomBase.MiniRooms.TryGetValue(roomId, out var mrb))
            {
                ReportManager.FileNewReport("Tried entering a trade room without a proper ID.", chr.ID, 0);
                return; // Invalid Room ID
            }

            if (mrb.Users.Count == 0) return;

            if (mrb.IsFull())
            {
                miniroomLog.Info($"{chr.Name} cannot enter miniroom: already full.");
                return; // Error msg if full?
            }

            if (mrb.Users.ToList().Exists(u => u.Value.MapID != chr.MapID))
            {
                InviteResult(chr, 1); // must be on same map. Show "not found" msg
                return;
            }

            switch (mrb.Type)
            {
                case MiniRoomType.Omok:
                    {
                        bool usePassword = packet.ReadBool();
                        Omok omok = (Omok)MiniRoomBase.MiniRooms[mrb.ID];

                        if (usePassword)
                        {
                            string password = packet.ReadString();
                            if (password != omok.Password)
                            {
                                miniroomLog.Info($"{chr.Name} cannot enter omok: invalid password");
                                MiniGamePacket.ErrorMessage(chr, MiniGameError.IncorrectPassword);
                                chr.Room = null;
                                break;
                            }
                        }
                        if (chr.Inventory.Mesos >= 100)
                        {
                            omok.AddPlayer(chr);
                            MiniRoomBalloonPacket.Send(omok.Owner, omok);
                            chr.Inventory.AddMesos(-100);
                            miniroomLog.Info($"{chr.Name} entered omok");
                        }
                        else
                        {
                            miniroomLog.Info($"{chr.Name} cannot enter omok: not enough mesos");
                            MiniGamePacket.ErrorMessage(chr, MiniGameError.NotEnoughMesos);
                        }
                        break;
                    }
                case MiniRoomType.Trade:
                    {
                        miniroomLog.Info($"{chr.Name} entered trade");
                        mrb.AddPlayer(chr);
                        break;
                    }
                case MiniRoomType.PlayerShop:
                    {
                        miniroomLog.Info($"{chr.Name} entered playershop");
                        PlayerShop shop = (PlayerShop)MiniRoomBase.MiniRooms[roomId];
                        shop.AddPlayer(chr);
                        PlayerShopPackets.PersonalShopRefresh(chr, shop); //Show items 
                        break;
                    }
            }
        }

        public static void SendEnter(MiniRoomBase pRoom, GameCharacter pWho)
        {
            Packet pw = new Packet(ServerMessages.MINI_ROOM_BASE);
            pRoom.EncodeEnter(pWho, pw);
            pRoom.BroadcastPacket(pw, pWho);
        }

        public static void SendEnterResult(MiniRoomBase pRoom, GameCharacter pTo)
        {
            Packet pw = new Packet(ServerMessages.MINI_ROOM_BASE);
            pRoom.EncodeEnterResult(pTo, pw);
            pTo.SendPacket(pw);
        }

        public static Packet LeaveRoom(GameCharacter victim, MiniRoomLeaveReason reason)
        {
            Packet pw = new Packet(ServerMessages.MINI_ROOM_BASE);
            pw.WriteByte((byte)MiniRoomOpClient.Leave);
            pw.WriteByte(victim.RoomSlotId);
            pw.WriteByte((byte)reason);
            return pw;
        }
        public static void SendLeaveRoom(GameCharacter victim, MiniRoomLeaveReason reason)
        {
            var pw = LeaveRoom(victim, reason);
            victim.SendPacket(pw);
        }
        public static void SendLeaveRoom(MiniRoomBase room, GameCharacter victim, MiniRoomLeaveReason reason)
        {
            var pw = LeaveRoom(victim, reason);
            room.BroadcastPacket(pw);
        }

        public static void Invite(MiniRoomBase pRoom, GameCharacter pWho, GameCharacter pVictim)
        {
            Packet pw = new Packet(ServerMessages.MINI_ROOM_BASE);
            pw.WriteByte((byte)MiniRoomOpServer.Invite);
            pw.WriteByte((byte)pRoom.Type);
            pw.WriteString(pWho.Name);
            pw.WriteInt(pRoom.ID);
            pVictim.SendPacket(pw);
        }

        public static void InviteResult(GameCharacter pWho, byte pFailID, string pName = "")
        {
            Packet pw = new Packet(ServerMessages.MINI_ROOM_BASE);
            pw.WriteByte((byte)MiniRoomOpServer.InviteResult);
            pw.WriteByte(pFailID);

            if (pFailID == 2 || pFailID == 0)
            {
                pw.WriteString(pName);
            }

            pWho.SendPacket(pw);
        }

        public static void Chat(MiniRoomBase pRoom, GameCharacter pCharacter, string pText, sbyte pMessageCode)
        {
            Packet pw = new Packet(ServerMessages.MINI_ROOM_BASE);
            pw.WriteByte((byte)MiniRoomOpServer.Chat);

            if (pMessageCode < 0)
            {
                pw.WriteByte(8);
                pw.WriteByte(pCharacter.RoomSlotId);
                pw.WriteString($"{pCharacter.Name} : {pText}");

            }
            else
            {
                pw.WriteByte(7);
                pw.WriteSByte(pMessageCode);
                pw.WriteString(pCharacter.Name);
            }

            pRoom.BroadcastPacket(pw);
        }

        // This packet feels wonky and insecure - wackyracer
        public static void TradePutItem(GameCharacter pTo, byte tradeSlot, BaseItem pItem, byte userSlot)
        {
            Packet pw = new Packet(ServerMessages.MINI_ROOM_BASE);
            pw.WriteByte((byte)MiniRoomOpClient.TradePutItem);
            pw.WriteByte(userSlot);
            pw.WriteByte(tradeSlot);
            new GW_ItemSlotBase(pItem).Encode(pw, false, false);
            pTo.SendPacket(pw);
        }

        public static void TradePutMesos(GameCharacter pTo, int amount, byte userSlot)
        {
            Packet pw = new Packet(ServerMessages.MINI_ROOM_BASE);
            pw.WriteByte((byte)MiniRoomOpClient.TradePutMesos);
            pw.WriteByte(userSlot);
            pw.WriteInt(amount);
            pTo.SendPacket(pw);
        }

        public static void SelectTrade(GameCharacter pTo)
        {
            Packet pw = new Packet(ServerMessages.MINI_ROOM_BASE);
            pw.WriteByte(0xF);
            pTo.SendPacket(pw);
        }
    }
}
