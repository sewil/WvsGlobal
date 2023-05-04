using System.Linq;
using log4net;
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

                            if ((pCharacter.IsGM || MasterThread.IsDebug) && text.StartsWith("/") && text.Length > 1)
                            {
                                MiniRoomCommand(pCharacter.Room, pCharacter, text);
                            }
                            else ChatText(pCharacter.Room, pCharacter, text);
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

                        if (mr.Type == MiniRoomType.Trade) mr.Close(reason: MiniRoomLeaveReason.Cancel);
                        else if (pCharacter == mr.Owner) mr.Close(reason: MiniRoomLeaveReason.Closed);
                        else mr.RemovePlayer(pCharacter, MiniRoomLeaveReason.Leave);
                        break;
                    }

                case MiniRoomOpClient.RequestLeave:
                    {
                        if (pCharacter.Room == null || !(pCharacter.Room is MiniGameRoom)) return;
                        MiniGameRoom rm = pCharacter.Room as MiniGameRoom;
                        rm.RequestLeave(pCharacter);
                        break;
                    }
                case MiniRoomOpClient.CancelRequestLeave:
                    {
                        if (pCharacter.Room == null || !(pCharacter.Room is MiniGameRoom)) return;
                        MiniGameRoom rm = pCharacter.Room as MiniGameRoom;
                        rm.CancelRequestLeave(pCharacter);
                        break;
                    }

                case MiniRoomOpClient.Open:
                    {
                        var room = pCharacter.Room;
                        if (room == null || !(room is BalloonRoom)) return;
                        (room as BalloonRoom).Open();
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
                        if (!(pCharacter.Room is MiniGameRoom game)) return;
                        bool result = pPacket.ReadBool();
                        if (result) game.EndGame(pCharacter, GameResult.Tie);
                        else MiniGamePacket.RequestTieDeny(pCharacter, game);
                        break;
                    }
                case MiniRoomOpClient.GiveUp:
                    {
                        if (!(pCharacter.Room is MiniGameRoom game)) return;
                        GameCharacter winner = game.Users[(byte)(pCharacter.RoomSlotId == 0 ? 1 : 0)];
                        game.EndGame(winner, GameResult.Forfeit);
                        break;
                    }

                case MiniRoomOpClient.Ready: //Ready
                    {
                        if (pCharacter.Room == null) return;
                        MiniGamePacket.Ready(pCharacter, pCharacter.Room);
                        break;
                    }

                case MiniRoomOpClient.Unready:
                    {
                        if (pCharacter.Room == null) return;
                        MiniGamePacket.UnReady(pCharacter, pCharacter.Room);
                        break;
                    }

                case MiniRoomOpClient.Expel:
                    {
                        if (pCharacter.Room == null) return;
                        pCharacter.Room.RemovePlayer(pCharacter.Room.Users[1], MiniRoomLeaveReason.Expelled);
                        break;
                    }

                case MiniRoomOpClient.StartGame:
                    {
                        if (!(pCharacter.Room is MiniGameRoom game) || game.GameStarted) return;
                        game.StartGame(pCharacter);
                        break;
                    }

                case MiniRoomOpClient.EndGame:
                    {
                        if (!(pCharacter.Room is MiniGameRoom game) || !game.GameStarted) return;
                        game.EndGame(pCharacter, GameResult.Win);
                        break;
                    }

                case MiniRoomOpClient.PlaceOmokPiece: //Place omok piece
                    {
                        if (!(pCharacter.Room is Omok omok)) return;
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
                            omok.EndGame(pCharacter, GameResult.Win);
                        }
                        break;
                    }
                case MiniRoomOpClient.MatchCardsPickCard:
                    {
                        if (!(pCharacter.Room is MatchCard mc)) return;
                        bool first = pPacket.ReadBool();
                        byte idx = pPacket.ReadByte();
                        mc.FlipCard(pCharacter, first, idx);
                        break;
                    }
                case MiniRoomOpClient.RequestHandicap:
                    {
                        if (pCharacter.Room == null) return;
                        MiniGamePacket.RequestHandicap(pCharacter, pCharacter.Room);
                        break;
                    }

                case MiniRoomOpClient.RequestHandicapResult: //Request handicap result
                    {
                        if (!(pCharacter.Room is Omok omok)) return;
                        bool result = pPacket.ReadBool();

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
                        if (pCharacter.Room == null) return;
                        pCharacter.Room.OnPacket(pCharacter, action, pPacket);
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
                        string title = packet.ReadString();//49 00 02 01 00 61 00 00
                        bool isPrivate = packet.ReadBool();
                        string password = null;
                        if (isPrivate) password = packet.ReadString();
                        var boardSize = (MatchCardsSize)packet.ReadByte();
                        MatchCard mc = new MatchCard(chr, title, isPrivate, password, boardSize);
                        break;
                    }

                case MiniRoomType.Trade:
                    {
                        miniroomLog.Info($"{chr.Name} creates a trade miniroom");
                        Trade trade = new Trade(chr);
                        break;
                    }

                case MiniRoomType.PlayerShop:
                    {
                        miniroomLog.Info($"{chr.Name} creates a player shop miniroom");
                        string title = packet.ReadString();
                        bool isPrivate = packet.ReadBool();
                        short x = packet.ReadShort(); // might be type of shop (different shops had different outer designs/looks)? unused var. not sure what it's purpose it serves.
                        int objectId = packet.ReadInt();
                        // Check overlap
                        if (PlayerShop.CanPlace(chr))
                        {
                            new PlayerShop(chr, title, objectId);
                        }
                        else
                        {
                            ChatPacket.SendBroadcastMessageToPlayer(chr, "You cannot open the store here.", BroadcastMessageType.PopupBox);
                        }
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
                case MiniRoomType.MatchCards:
                case MiniRoomType.Omok:
                    {
                        bool usePassword = packet.ReadBool();
                        MiniGameRoom game = mrb as MiniGameRoom;

                        if (usePassword)
                        {
                            string password = packet.ReadString();
                            if (password != game.Password)
                            {
                                miniroomLog.Info($"{chr.Name} cannot enter minigame: invalid password");
                                MiniGamePacket.SendEnterResultError(chr, MiniRoomEnterError.IncorrectPassword);
                                chr.Room = null;
                                break;
                            }
                        }
                        if (chr.Inventory.Mesos >= 100)
                        {
                            game.AddPlayer(chr);
                            chr.Inventory.AddMesos(-100);
                            miniroomLog.Info($"{chr.Name} entered minigame");
                        }
                        else
                        {
                            miniroomLog.Info($"{chr.Name} cannot enter minigame: not enough mesos");
                            MiniGamePacket.SendEnterResultError(chr, MiniRoomEnterError.NotEnoughMesos);
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
                        mrb.AddPlayer(chr);
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
        public static void ChatNotice(MiniRoomBase room, GameCharacter chr, MiniRoomChatNoticeType noticeType)
        {
            Packet pw = new Packet(ServerMessages.MINI_ROOM_BASE);
            pw.WriteByte((byte)MiniRoomOpServer.Chat);
            pw.WriteByte((byte)MiniRoomChatType.Notice);
            pw.WriteByte((byte)noticeType);
            pw.WriteString(chr?.Name ?? "");
            room.BroadcastPacket(pw);
        }
        public static void ChatText(MiniRoomBase room, GameCharacter chr, string text)
        {
            Packet pw = new Packet(ServerMessages.MINI_ROOM_BASE);
            pw.WriteByte((byte)MiniRoomOpServer.Chat);
            pw.WriteByte((byte)MiniRoomChatType.Text);
            pw.WriteByte(chr.RoomSlotId);
            pw.WriteString(chr.Name + " : " + text);
            room.BroadcastPacket(pw);
        }

        public static void MiniRoomCommand(MiniRoomBase room, GameCharacter character, string commandText)
        {
            var args = new Handlers.CommandHandling.CommandArgs(commandText);
            switch (args.Command.ToLower())
            {
                case "packet":
                    {
                        if (args.Count > 0)
                        {
                            var pw = new Packet();
                            pw.WriteHexString(args.CommandText);
                            character.SendPacket(pw);
                        }
                        break;
                    }
                case "chatnotice":
                    {
                        if (byte.TryParse(args[0], out byte opCode))
                        {
                            ChatNotice(room, character, (MiniRoomChatNoticeType)opCode);
                        }
                        break;
                    }
                case "chattext":
                    {
                        if (args.Count >= 1)
                        {
                            string text = "";
                            for (int i = 0; i < args.Count; i++) text += args[i];
                            ChatText(room, character, text);
                        }
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        // This packet feels wonky and insecure - wackyracer
        public static void TradePutItem(GameCharacter pTo, byte tradeSlot, Item pItem, byte userSlot)
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
