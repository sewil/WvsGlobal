using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using log4net;
using WvsBeta.Common;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Sessions;
using WvsBeta.Game.Handlers.Commands;
using WvsBeta.Game.Packets;

namespace WvsBeta.Game
{
    public enum BroadcastMessageType : byte
    {
        Notice = 0x00, // Blue text no highlight
        PopupBox = 0x01, // Pop-up text window
        Megaphone = 0x02, // Blue text with highlight
        SuperMegaphone = 0x03, // Red text with bright pink highlight
        Header = 0x04, // Scrolling header
        RedText = 0x05, // Red text with no highlight
        Blue = 0x06 // Blue text without [Notice]
    }

    public enum MessageMode : byte
    {
        ToPlayer,
        ToMap,
        ToChannel,
    }

    public enum MuteReasons : byte
    {
        FoulLanguage = 1,
        Advertising = 2,
        ImpersonationOfAGM = 3,
        AccountTrading = 4,
        ReportingOfFameTradeScams = 5,
        PenaltyAlert = 6
    }

    public static class ChatPacket
    {
        private static ILog log = LogManager.GetLogger("ChatLog");
        private static ILog commandLog = LogManager.GetLogger("CommandLog");
        private static ILog buddyLog = LogManager.GetLogger("BuddyChatLog");
        private static ILog partyLog = LogManager.GetLogger("PartyChatLog");
        private static ILog guildLog = LogManager.GetLogger("GuildChatLog");
        private static ILog whisperLog = LogManager.GetLogger("WhisperChatLog");

        public static string GetMuteReasonText(MuteReasons reason)
        {
            switch ((byte)reason)
            {
                case 1: return "Foul Language";
                case 2: return "Advertising";
                case 3: return "Impersonation of a GM";
                case 4: return "Account Trading";
                case 5: return "Reporting of fame/trade scams";
                case 6: return "Penalty Alert";
                default: return reason.ToString();
            }
        }

        public static MuteReasons ParseMuteReason(string input)
        {
            switch (input)
            {
                case "1":
                case "harrassment":
                case "foullanguage": return MuteReasons.FoulLanguage;
                case "2":
                case "advertising": return MuteReasons.Advertising;
                case "3":
                case "impersonation":
                case "fakegm": return MuteReasons.ImpersonationOfAGM;
                case "4":
                case "trading": return MuteReasons.AccountTrading;
                case "5":
                case "reportscam": return MuteReasons.ReportingOfFameTradeScams;
                case "6":
                case "penalty": return MuteReasons.PenaltyAlert;
                default: return 0;
            }
        }

        public static bool ShowMuteMessage(GameCharacter chr)
        {
            chr.LastChat = MasterThread.CurrentTime;

            if (chr.MutedUntil > MasterThread.CurrentDate)
            {
                SendText(BroadcastMessageType.RedText, $"You are muted until {chr.MutedUntil:MM/dd/yyyy HH:mm:ss}, reason {GetMuteReasonText((MuteReasons)chr.MuteReason)}", chr, MessageMode.ToPlayer);
                return true;
            }
            return false;
        }

        public static void HandleChat(GameCharacter chr, Packet packet)
        {
            string what = packet.ReadString();

            if (chr.IsGM == false && CheatInspector.CheckTextSpam(what))
            {
                log.Error("Disconnecting player for chat spam");
                chr.Player.Socket.Disconnect();
                return;
            }
            
            if (//!MainCommandHandler.HandleCommand(chr, new CommandHandling.CommandArgs(what)) &&
                !CommandHandling.HandleChat(chr, what))
            {
                if (ShowMuteMessage(chr))
                {
                    log.Info("[MUTED] " + chr.Name + ": " + what);
                    return;
                }
                log.Info(chr.Name + ": " + what);
                if (chr.Field.ChatEnabled)
                    MapPacket.SendChatMessage(chr, what);
            }
            else
            {
                commandLog.Info(chr.Name + ": " + what);
            }
        }

        public static void HandleGroupMessage(GameCharacter chr, Packet packet)
        {
            if (!chr.Field.ChatEnabled) return;

            //to be handled via center server
            GroupMessageType type = (GroupMessageType)packet.ReadByte();
            byte recipientsLen = packet.ReadByte();
            // Not used
            int[] recipients = new int[recipientsLen];

            for (int i = 0; i < recipientsLen; i++)
            {
                recipients[i] = packet.ReadInt();
            }

            string message = packet.ReadString();

            ILog logger = log;
            switch (type)
            {
                case GroupMessageType.Buddy: logger = buddyLog; break;
                case GroupMessageType.Party: logger = partyLog; break;
                case GroupMessageType.Guild: logger = guildLog; break;
            }

            if (ShowMuteMessage(chr))
            {
                logger.Info("[MUTED] " + chr.Name + ": " + message);
                return;
            }

            logger.Info(chr.Name + ": " + message);
            SendGroupMessageToCenter(chr.Name, type, recipients, message);
        }
        public static void SendGroupMessageToCenter(string fromName, GroupMessageType type, int[] recipients, string message)
        {
            Packet pw = new Packet(ISClientMessages.GroupMessage);
            pw.WriteString(fromName);
            pw.WriteByte((byte)type);
            pw.WriteByte((byte)recipients.Length);
            for (int i = 0; i < recipients.Length; i++)
            {
                pw.WriteInt(recipients[i]);
            }
            pw.WriteString(message);
            Server.Instance.CenterConnection.SendPacket(pw);
        }
        public static void HandleCommand(GameCharacter chr, Packet packet)
        {
            if (!chr.Field.ChatEnabled) return;

            byte type = packet.ReadByte();
            string victim = packet.ReadString();

            GameCharacter victimChar = Server.Instance.GetCharacter(victim);

            // Block find or whisper
            if (victimChar != null && (victimChar.IsGM && !chr.IsGM))
            {
                WhisperPacket.Find(chr, victim, -1, 0, false);
                return;
            }

            switch (type)
            {
                case 0x05:
                    log.Info("[FIND][ " + chr.Name + " ] " + victim);
                    if (victimChar != null)
                    {
                        WhisperPacket.Find(chr, victim, victimChar.MapID, 0, true);
                    }
                    else
                    {
                        Server.Instance.CenterConnection.PlayerFind(chr.ID, victim);
                    }
                    break;
                case 0x06:
                    string message = packet.ReadString();


                    if (ShowMuteMessage(chr))
                    {
                        whisperLog.Info("[MUTED][to " + victim + "] " + chr.Name + ":  " + message);
                        return;
                    }

                    whisperLog.Info("[to " + victim + "] " + chr.Name + ":  " + message);
                    if (victimChar != null)
                    {
                        WhisperPacket.Find(chr, victim, -1, 1, false);
                        WhisperPacket.Whisper(victimChar, chr.Name, Server.Instance.ID, message, false);
                    }
                    else
                    {
                        Server.Instance.CenterConnection.PlayerWhisper(chr.ID, victim, message);
                    }
                    break;
            }
        }

        public static void SendMegaphoneMessage(string what)
        {
            log.Info("[MEGAPHONE] " + what);
            Packet pw = new Packet(ServerMessages.BROADCAST_MSG);
            pw.WriteByte((byte)BroadcastMessageType.Megaphone);
            pw.WriteString(what);

            foreach (KeyValuePair<int, Map> kvp in DataProvider.Maps)
            {
                kvp.Value.SendPacket(pw);
            }
        }

        public static void SendMegaphoneMessage(string what, int mapid)
        {
            log.Info("[MEGAPHONE] " + what);

            Packet pw = new Packet(ServerMessages.BROADCAST_MSG);
            pw.WriteByte((byte)BroadcastMessageType.Megaphone);
            pw.WriteString(what);
            DataProvider.Maps[mapid].SendPacket(pw);
        }

        public static void SendSuperMegaphoneMessage(string what, bool WhisperOrFind, byte channel)
        {
            log.Info("[SUPERMEGAPHONE] " + what);

            Packet pw = new Packet(ServerMessages.BROADCAST_MSG);
            pw.WriteByte((byte)BroadcastMessageType.SuperMegaphone);
            pw.WriteString(what);
            if (channel == 1) channel = 0; // Bugged O.o
            pw.WriteByte(channel);
            pw.WriteBool(WhisperOrFind);

            foreach (KeyValuePair<int, Map> kvp in DataProvider.Maps)
                kvp.Value.SendPacket(pw);
        }

        public static void SendText(BroadcastMessageType type, string what, GameCharacter victim, MessageMode mode)
        {
            if (type == BroadcastMessageType.SuperMegaphone)
                return;

            Packet pw = new Packet(ServerMessages.BROADCAST_MSG);
            pw.WriteByte((byte)type);

            if (type == BroadcastMessageType.Header)
            {
                pw.WriteBool(what != "");
            }
            pw.WriteString(what);

            switch (mode)
            {
                case MessageMode.ToPlayer:
                    if (type != BroadcastMessageType.Header || what != "")
                    {
                        log.Info($"[MSG][ {mode} ][{type}][{victim.ID}] {what}");
                    }

                    victim.SendPacket(pw);
                    break;
                case MessageMode.ToMap:
                    if (type != BroadcastMessageType.Header || what != "")
                    {
                        log.Info($"[MSG][ {mode} ][{type}][{victim.MapID}] {what}");
                    }

                    victim.Field.SendPacket(pw);
                    break;
                case MessageMode.ToChannel:
                    if (type != BroadcastMessageType.Header || what != "")
                    {
                        log.Info($"[MSG][ {mode} ][{type}] {what}");
                    }

                    foreach (var kvp in Server.Instance.CharacterList)
                    {
                        kvp.Value.SendPacket(pw);
                    }
                    break;
            }
        }


        public static void SendAdminWarning(Map map, string what)
        {
            log.Info("[ADMIN WARNING MAP][" + map.ID + "] " + what);
            Packet pw = new Packet(ServerMessages.WARN_MESSAGE);
            pw.WriteString(what);
            map.SendPacket(pw);
        }

        public static void SendAdminWarning(GameCharacter victim, string what)
        {
            log.Info("[ADMIN WARNING][" + victim.ID + "] " + what);
            Packet pw = new Packet(ServerMessages.WARN_MESSAGE);
            pw.WriteString(what);
            victim.SendPacket(pw);
        }
        

        public static void SendNotice(string what, GameCharacter victim)
        {
            Packet pw = new Packet(ServerMessages.BROADCAST_MSG);
            pw.WriteByte((byte)BroadcastMessageType.Notice);
            pw.WriteString(what);
            if (victim == null)
            {
                log.Info("[SERVERNOTICE] " + what);
                Server.Instance.PlayerList.ForEach(x => x.Value.Socket.SendPacket(pw));
            }
            else
            {
                log.Info("[NOTICE][" + victim.ID + "] " + what);
                victim.SendPacket(pw);
            }
        }

        public static void SendNoticeGMs(string what, BroadcastMessageType severity = BroadcastMessageType.Notice)
        {
            log.Info("[GM NOTICE] " + what);
            Trace.WriteLine("[GM NOTICE] " + what);

            Packet pw = new Packet(ServerMessages.BROADCAST_MSG);
            pw.WriteByte((byte)severity);
            pw.WriteString(what);

            Server.Instance.StaffCharacters
                .Where(c => c.IsGM)
                .ForEach(a => a.SendPacket(pw));
        }

        public static void SendNoticeMap(string what, int mapid)
        {
            log.Info("[MAPNOTICE][" + mapid + "] " + what);
            Packet pw = new Packet(ServerMessages.BROADCAST_MSG);
            pw.WriteByte((byte)BroadcastMessageType.Notice);
            pw.WriteString(what);
            DataProvider.Maps[mapid].SendPacket(pw);
        }

        public static Packet BroadcastMessage(string text, BroadcastMessageType type)
        {
            Packet pw = new Packet(ServerMessages.BROADCAST_MSG);
            pw.WriteByte((byte)type);
            if (type == BroadcastMessageType.Header)
            {
                pw.WriteBool(text.Length != 0);
            }
            pw.WriteString(text);
            return pw;
        }
        public static void SendBroadcastMessage(GameCharacter chr, string text, BroadcastMessageType type, MessageMode to)
        {
            Packet pw = BroadcastMessage(text, type);
            switch (to)
            {
                case MessageMode.ToPlayer:
                    Server.Instance.CenterConnection.AdminMessage(text, type);
                    break;
                case MessageMode.ToChannel:
                    foreach (var kvp in DataProvider.Maps)
                    {
                        kvp.Value.SendPacket(pw);
                    }
                    break;
                case MessageMode.ToMap:
                    chr.Field.SendPacket(pw);
                    break;
            }
        }
    }
}
