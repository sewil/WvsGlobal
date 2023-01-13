using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WvsBeta.Game.Handlers.Commands
{
    public class HandlerHelpers
    {

        public static int GetMapidFromName(string name)
        {
            if (CommandHandling.MapNameList.TryGetValue(name, out var mapid)) return mapid;

            return -1;
        }

        public static void ShowError(string message) => SendResponse(ChatPacket.MessageTypes.RedText, message);
        public static void ShowInfo(string message) => SendResponse(ChatPacket.MessageTypes.Notice, message);

        public static void SendResponse(ChatPacket.MessageTypes type, string message)
        {
            if (MainCommandHandler.CommandCharacter == null) return;

            ChatPacket.SendText(
                type,
                message,
                MainCommandHandler.CommandCharacter,
                ChatPacket.MessageMode.ToPlayer
            );
        }
    }
}
