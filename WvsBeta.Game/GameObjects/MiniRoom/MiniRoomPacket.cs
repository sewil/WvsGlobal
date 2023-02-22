using System.Linq;
using log4net;
using WvsBeta.Common.Character;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Objects;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Game.GameObjects.MiniRoom
{
    public static class MiniRoomPacket
    {
        private static ILog miniroomLog = LogManager.GetLogger("MiniroomLog");
        private static ILog miniroomChatLog = LogManager.GetLogger("MiniroomChatLog");

        public static void HandlePacket(GameCharacter pCharacter, Packet pPacket)
        {
            //MessagePacket.SendNotice("PACKET: " + pPacket.ToString(), pCharacter);
            var action = (MiniRoomAction)pPacket.ReadByte();

            switch (action)
            {
                case MiniRoomAction.Create: // Create miniroom
                    {
                        if (pCharacter.AssertForHack(!pCharacter.CanAttachAdditionalProcess, "Trying to create a miniroom while he cannot attach additional process."))
                        {
                            return;
                        }
                        CreateMiniRoomBase(pCharacter, pPacket);
                        break;
                    }

                case MiniRoomAction.Invite: // Invite To miniroom
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
                        else if ((pCharacter.IsGM == false && victim.IsGM) ||
                            (pCharacter.IsGM && victim.IsGM == false))
                        {
                            miniroomLog.Info($"{pCharacter.Name} fails to invite charid {playerid}: non-admin tried to invite admin or vice versa");

                            InviteResult(pCharacter, 1);
                        }
                        else
                        {
                            miniroomLog.Info($"{pCharacter.Name} invited {victim.Name} (charid {playerid})");
                            Invite(pCharacter.Room, pCharacter, victim);
                        }

                        break;
                    }

                case MiniRoomAction.DeclineInvite: // Decline Invite
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

                case MiniRoomAction.Enter: // Enter Room
                    {
                        EnterMiniRoom(pCharacter, pPacket);
                        break;
                    }

                case MiniRoomAction.Chat: // Chat
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

                case MiniRoomAction.AddShopItem: //Add item to Player Shop
                    {
                        if (pCharacter.Room == null) return;

                        Inventory inventory = (Inventory)pPacket.ReadByte();
                        short inventoryslot = pPacket.ReadShort();
                        short bundleamount = pPacket.ReadShort();
                        short AmountPerBundle = pPacket.ReadShort();
                        int price = pPacket.ReadInt();
                        PlayerShop.HandleShopUpdateItem(pCharacter, inventory, inventoryslot, bundleamount, AmountPerBundle, price);
                        break;
                    }

                case MiniRoomAction.BuyShopItem: //Buy item from shop
                    {
                        if (pCharacter.Room == null) return;

                        byte slot = pPacket.ReadByte();
                        short bundleamount = pPacket.ReadShort();
                        PlayerShop ps = MiniRoomBase.PlayerShops[pCharacter.Room.ID];

                        if (ps != null)
                        {
                            ps.BuyItem(pCharacter, slot, bundleamount);
                        }

                        break;
                    }

                case MiniRoomAction.Leave: //Leave
                    {
                        MiniRoomBase mr = pCharacter.Room;
                        if (mr == null) return;

                        miniroomLog.Info($"{pCharacter.Name} declined invite.");

                        if (mr.Type == MiniRoomType.Trade)
                        {
                            for (int i = 0; i < 2; i++)
                            {
                                GameCharacter chr = mr.Users[i];
                                GameCharacter leader = mr.Users[0];

                                if (chr == null)
                                {
                                    continue;
                                }

                                mr.RemovePlayer(chr, MiniRoomLeaveReason.Unk1);
                                //mr.Users[i] = null; //send this after all characters are removed
                            }
                        }

                        else if (mr.Type == MiniRoomType.PersonalShop)
                        {
                            mr.RemovePlayerFromShop(pCharacter);
                        }

                        else if (mr.Type == MiniRoomType.Omok)
                        {
                            //MessagePacket.SendNotice("leave omok", pCharacter);
                            Omok omok = MiniRoomBase.Omoks[pCharacter.Room.ID];

                            if (pCharacter == omok.Owner)
                            {
                                omok.CloseOmok(pCharacter);
                            }
                            else
                            {
                                omok.RemovePlayer(pCharacter, MiniRoomLeaveReason.YouHaveLeft);
                            }
                        }

                        break;
                    }

                case MiniRoomAction.AddAnnounceBox: //Add announce box
                    {
                        if (pCharacter.Room == null) return;
                        MiniRoomBalloonPacket.Send(pCharacter, pCharacter.Room);
                        var roomType = pCharacter.Room.Type;

                        switch (roomType)
                        {
                            case MiniRoomType.Omok:
                                {
                                    pCharacter.Field.Omoks.Add(pCharacter.Room.ID, MiniRoomBase.Omoks[pCharacter.Room.ID]);
                                    break;
                                }
                            case MiniRoomType.PersonalShop:
                                {
                                    pCharacter.Field.PlayerShops.Add(pCharacter.Room.ID, MiniRoomBase.PlayerShops[pCharacter.Room.ID]);
                                    break;
                                }
                        }
                        break;
                    }

                case MiniRoomAction.MoveItemShopToInv: //Move Item from player shop to inventory
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

                case MiniRoomAction.RequestTie:
                    {
                        MiniGamePacket.RequestTie(pCharacter, pCharacter.Room);
                        break;
                    }
                case MiniRoomAction.RequestTieResult:
                    {
                        bool result = pPacket.ReadBool();
                        if (result && MiniRoomBase.Omoks.TryGetValue(pCharacter.Room.ID, out Omok omok))
                        {
                            omok.UpdateGame(pCharacter, GameResult.Tie);
                        }
                        else
                        {
                            MiniGamePacket.RequestTieDeny(pCharacter, pCharacter.Room);
                        }
                        break;
                    }
                case MiniRoomAction.GiveUp:
                    {
                        if (MiniRoomBase.Omoks.TryGetValue(pCharacter.Room.ID, out Omok omok))
                        {
                            GameCharacter winner = omok.Users[pCharacter.RoomSlotId == 0 ? 1 : 0];
                            omok.UpdateGame(winner, GameResult.Forfeit);
                        }
                        break;
                    }

                case MiniRoomAction.Ready: //Ready
                    {
                        MiniGamePacket.Ready(pCharacter, pCharacter.Room);
                        break;
                    }

                case MiniRoomAction.Unready:
                    {
                        MiniGamePacket.UnReady(pCharacter, pCharacter.Room);
                        break;
                    }

                case MiniRoomAction.Expel:
                    {
                        pCharacter.Room.RemovePlayer(pCharacter.Room.Users[1], MiniRoomLeaveReason.Expelled);
                        break;
                    }

                case MiniRoomAction.StartOmok:
                    {
                        Omok omok = MiniRoomBase.Omoks[pCharacter.Room.ID];
                        if (omok != null)
                        {
                            omok.Start(pCharacter);
                        }
                        break;
                    }

                case MiniRoomAction.OmokWinner:
                    {
                        Omok omok = MiniRoomBase.Omoks[pCharacter.Room.ID];
                        omok.UpdateGame(pCharacter, GameResult.Win);
                        break;
                    }

                case MiniRoomAction.PlaceOmokPiece: //Place omok piece
                    {
                        Omok omok = MiniRoomBase.Omoks[pCharacter.Room.ID];

                        if (omok != null)
                        {
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
                        }

                        break;
                    }

                case MiniRoomAction.RequestHandicap:
                    {
                        MiniGamePacket.RequestHandicap(pCharacter, pCharacter.Room);
                        break;
                    }

                case MiniRoomAction.RequestHandicapResult: //Request handicap result
                    {
                        bool result = pPacket.ReadBool();
                        Omok omok = MiniRoomBase.Omoks[pCharacter.Room.ID];
                        if (omok == null) break;

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
                            pCharacter.Room.OnPacket(pCharacter, (byte)action, pPacket);
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
                        MiniRoomBase omok = MiniRoomBase.CreateRoom(chr, MiniRoomType.Omok, packet, false, 0);
                        chr.Room = omok;

                        MiniGamePacket.ShowWindow(chr, omok, MiniRoomBase.Omoks[chr.Room.ID].OmokType);
                        MiniRoomBalloonPacket.Send(chr, omok);
                        break;
                    }

                case MiniRoomType.MatchCards: // Match Cards TODO!
                    {
                        miniroomLog.Info($"{chr.Name} creates a match cards");
                        string title = packet.ReadString();
                        bool usePassword = packet.ReadBool();
                        string password = "";
                        if (usePassword)
                        {
                            password = packet.ReadString();
                        }
                        packet.Skip(7);
                        byte cardType = packet.ReadByte();
                        break;
                    }

                case MiniRoomType.Trade:
                    {
                        miniroomLog.Info($"{chr.Name} creates a trade miniroom");
                        MiniRoomBase mrb = MiniRoomBase.CreateRoom(chr, nType, packet, false, 0);
                        chr.Room = mrb;
                        MiniRoomPacket.ShowWindow(mrb, chr);
                        break;
                    }

                case MiniRoomType.PersonalShop:
                    { // TODO: Handle shops overlap
                        miniroomLog.Info($"{chr.Name} creates a player shop miniroom");
                        MiniRoomBase mrb = MiniRoomBase.CreateRoom(chr, nType, packet, false, 0);
                        chr.Room = mrb;
                        PlayerShopPackets.OpenPlayerShop(chr, mrb);
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

            if (mrb.EnteredUsers == 0) return;

            if (mrb.IsFull())
            {
                miniroomLog.Info($"{chr.Name} cannot enter miniroom: already full.");
                return; // Error msg if full?
            }

            if (mrb.Users.ToList().Exists(u => u != null && u.MapID != chr.MapID))
            {
                InviteResult(chr, 1); // must be on same map. Show "not found" msg
                return;
            }

            switch (mrb.Type)
            {
                case MiniRoomType.Omok:
                    {
                        bool usePassword = packet.ReadBool();
                        Omok omok = MiniRoomBase.Omoks[mrb.ID];

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
                            MiniGamePacket.AddVisitor(chr, mrb);
                            MiniGamePacket.ShowWindow(chr, mrb, omok.OmokType);
                            MiniRoomBalloonPacket.Send(omok.Owner, omok);
                            chr.Inventory.ExchangeMesos(-100);
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
                        MiniRoomPacket.ShowJoin(mrb, chr);
                        MiniRoomPacket.ShowWindow(mrb, chr);
                        break;
                    }
                case MiniRoomType.PersonalShop:
                    {
                        miniroomLog.Info($"{chr.Name} entered playershop");
                        PlayerShop shop = MiniRoomBase.PlayerShops[roomId];
                        for (int i = 0; i < shop.EnteredUsers; i++)
                        {
                            GameCharacter shopUser = mrb.Users[i];
                            if (shopUser != null && shopUser != chr)
                            {
                                shop.AddPlayer(chr);
                                PlayerShopPackets.AddPlayer(chr, shopUser);
                                PlayerShopPackets.OpenPlayerShop(chr, mrb);
                                PlayerShopPackets.PersonalShopRefresh(chr, shop); //Show items 
                            }
                        }
                        break;
                    }
            }
        }

        public static void ShowWindow(MiniRoomBase pRoom, GameCharacter pTo)
        {
            Packet pw = new Packet(ServerMessages.MINI_ROOM_BASE);
            pw.WriteByte(5);
            pw.WriteByte((byte)pRoom.Type);
            pw.WriteByte(pRoom.MaxUsers);
            pw.WriteByte(pTo.RoomSlotId);

            for (int i = 0; i < pRoom.Users.Length; i++)
            {
                GameCharacter character = pRoom.Users[i];

                if (character == null)
                {
                    continue;
                }

                pw.WriteByte(character.RoomSlotId);
                new AvatarLook(character).Encode(pw);
                pw.WriteString(character.Name);
            }

            pw.WriteByte(0xFF);
            pRoom.EncodeEnter(pTo, pw);
            pTo.SendPacket(pw);
        }

        public static void ShowJoin(MiniRoomBase pRoom, GameCharacter pWho)
        {
            Packet pw = new Packet(ServerMessages.MINI_ROOM_BASE);
            pw.WriteByte(4);
            pw.WriteByte(pWho.RoomSlotId);
            new AvatarLook(pWho).Encode(pw);
            pw.WriteString(pWho.Name);
            pRoom.EncodeEnterResult(pWho, pw);
            pRoom.BroadcastPacket(pw, pWho);
        }

        public static void ShowLeaveRoom(MiniRoomBase pRoom, GameCharacter pWho, MiniRoomLeaveReason pReason)
        {
            Packet pw = new Packet(ServerMessages.MINI_ROOM_BASE);
            pw.WriteByte(0xA);
            pw.WriteByte(pWho.RoomSlotId);
            pw.WriteByte((byte)pReason);
            pRoom.BroadcastPacket(pw);
        }

        public static void Invite(MiniRoomBase pRoom, GameCharacter pWho, GameCharacter pVictim)
        {
            Packet pw = new Packet(ServerMessages.MINI_ROOM_BASE);
            pw.WriteByte(2);
            pw.WriteByte((byte)pRoom.Type);
            pw.WriteString(pWho.Name);
            pw.WriteInt(pRoom.ID);
            pVictim.SendPacket(pw);
        }

        public static void InviteResult(GameCharacter pWho, byte pFailID, string pName = "")
        {
            Packet pw = new Packet(ServerMessages.MINI_ROOM_BASE);
            pw.WriteByte(3);
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
            pw.WriteByte(6);

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
        public static void AddItem(GameCharacter pTo, byte TradeSlot, BaseItem pItem, byte User)
        {
            int itemType = (pItem.ItemID / 1000000);
            Packet pw = new Packet(ServerMessages.MINI_ROOM_BASE);
            pw.WriteByte(13);
            pw.WriteByte(User); // 0 or 1 based on left/right side of trade window
            pw.WriteByte(TradeSlot); // item slot in the trade window
            pw.WriteByte((byte)itemType); // Item Type (EQ, USE, SETUP, ETC, PET)
            pItem.Encode(pw);
            pTo.SendPacket(pw);
        }

        // This is unused. Why? Idk. This has something to do with Trading Stars being bugged. It is probably the fix. - wackyracer
        public static void AddItemWithAmount(GameCharacter pTo, byte TradeSlot, BaseItem pItem, short amount, byte User)
        {
            int itemType = (pItem.ItemID / 1000000);
            // Used for items from the same stack
            Packet pw = new Packet(ServerMessages.MINI_ROOM_BASE);
            pw.WriteByte(13);
            pw.WriteByte(User); // 0 or 1 based on left/right side of trade window
            pw.WriteByte(TradeSlot); // item slot in the trade window
            pw.WriteByte((byte)itemType); // Item Type (EQ, USE, SETUP, ETC, PET)
            pItem.Encode(pw);
            pTo.SendPacket(pw);
        }

        public static void PutCash(GameCharacter pTo, int pAmount, byte test)
        {
            Packet pw = new Packet(ServerMessages.MINI_ROOM_BASE);
            pw.WriteByte(14);
            pw.WriteByte(test);
            pw.WriteInt(pAmount);
            pTo.SendPacket(pw);
        }

        public static void SelectTrade(GameCharacter pTo)
        {
            Packet pw = new Packet(ServerMessages.MINI_ROOM_BASE);
            pw.WriteByte(0xF);
            pTo.SendPacket(pw);
        }

        public static void TradeUnsuccessful(GameCharacter pTo)
        {
            Packet pw = new Packet(ServerMessages.MINI_ROOM_BASE);
            pw.WriteByte(10);
            pw.WriteByte(pTo.RoomSlotId);
            pw.WriteByte(6);
            pTo.SendPacket(pw);
        }

        public static void TradeSuccessful(GameCharacter pCompleter)
        {
            Packet pw = new Packet(ServerMessages.MINI_ROOM_BASE);
            pw.WriteByte(10);
            pw.WriteByte(pCompleter.RoomSlotId);
            pw.WriteByte(5);
            pCompleter.SendPacket(pw);
        }
    }
}
