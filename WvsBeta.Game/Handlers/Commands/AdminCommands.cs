using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Game.Handlers.Commands
{
    public class AdminCommands : HandlerHelpers
    {
        [CommandHandler(
            CommandName = "map",
            Aliases = new[] { "m", "goto" }, 
            UserRanks = UserAdminLevels.AdminGmIntern | UserAdminLevels.Tespian
        )]
        public static bool HandleMapCommand(GameCharacter character,  string initialCommand, CommandHandling.CommandArgs Args)
        {
            if (Args.Count <= 0) return true;

            var FieldID = -1;

            if (!Args[0].IsNumber())
            {
                var MapStr = Args[0];
                var TempID = GetMapidFromName(MapStr);

                if (TempID == -1)
                {
                    switch (MapStr)
                    {
                        case "here":
                            FieldID = character.MapID;
                            break;
                        case "town":
                            FieldID = character.Field.ReturnMap;
                            break;
                    }
                }
                else
                    FieldID = TempID;
            }
            else
                FieldID = Args[0].GetInt32();

            if (DataProvider.Maps.ContainsKey(FieldID))
                character.ChangeMap(FieldID);
            else
                ShowError("Map not found.");
            return true;
        }

        [CommandHandler(
            CommandName = "whereami",
            Aliases = new[] { "whatmap", "pos" },
            UserRanks = UserAdminLevels.AdminGmIntern | UserAdminLevels.Tespian
        )]
        public static bool HandleCommandWhereAmI(GameCharacter character, string initialCommand, CommandHandling.CommandArgs Args)
        {
            ShowInfo($"You are on mapid {character.MapID}, X {character.Position.X}, Y {character.Position.Y}, FH {character.Foothold}");
            return true;
        }

        [CommandHandler(
            CommandName = "chase",
            Aliases = new[] {"c", "warpto"},
            UserRanks = UserAdminLevels.Tespian | UserAdminLevels.AdminGmIntern
        )]
        public static bool HandleCommandChase(GameCharacter character, string initialCommand,
            CommandHandling.CommandArgs Args)
        {
            if (Args.Count > 0)
            {
                string other = Args[0].Value.ToLower();
                var otherChar = Server.Instance.GetCharacter(other);
                if (otherChar != null)
                {
                    if (character.MapID != otherChar.MapID)
                    {
                        character.ChangeMap(otherChar.MapID);
                    }

                    var p = new Packet(0xC1);
                    p.WriteShort(otherChar.Position.X);
                    p.WriteShort(otherChar.Position.Y);
                    character.SendPacket(p);
                    return true;
                }

                ChatPacket.SendText(ChatPacket.MessageTypes.RedText, "Victim not found.",
                    character, ChatPacket.MessageMode.ToPlayer);
            }
            return true;
        }

        [CommandHandler(
            CommandName = "chasehere",
            Aliases = new[] { "warphere" },
            UserRanks = UserAdminLevels.Tespian | UserAdminLevels.AdminGmIntern
        )]
        public static bool HandleCommandChaseHere(GameCharacter character, string initialCommand,
            CommandHandling.CommandArgs Args)
        {
            if (Args.Count > 0)
            {
                string other = Args[0].Value.ToLower();
                var otherChar = Server.Instance.GetCharacter(other);
                if (otherChar != null)
                {
                    if (character.MapID != otherChar.MapID)
                    {
                        otherChar.ChangeMap(character.MapID);
                    }

                    var p = new Packet(0xC1);
                    p.WriteShort(character.Position.X);
                    p.WriteShort(character.Position.Y);
                    otherChar.SendPacket(p);
                    return true;
                }

                ChatPacket.SendText(ChatPacket.MessageTypes.RedText, "Victim not found.",
                    character, ChatPacket.MessageMode.ToPlayer);
            }
            return true;
        }

        [CommandHandler(
            CommandName = "packet",
            Aliases = new[] { "pack", "p" },
            UserRanks = UserAdminLevels.Admin
        )]
        public static bool HandleCommandPacket(GameCharacter character, string initialCommand, CommandHandling.CommandArgs Args)
        {
            try
            {
                if (Args.Count == 0) throw new Exception("Usage: /packet 01 FD 4D 23");
                byte[] bytes = Args.Args.Select(a => Convert.ToByte(a, 16)).ToArray();
                character.SendPacket(bytes);
                return true;
            }
            catch (Exception e)
            {
                ChatPacket.SendText(ChatPacket.MessageTypes.RedText, e.Message, character, ChatPacket.MessageMode.ToPlayer);
                return false;
            }
        }
    }
}
