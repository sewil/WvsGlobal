using log4net;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Extensions;
using WvsBeta.Common.Sessions;
using WvsBeta.Login.Packets;

namespace WvsBeta.Login.PacketHandlers
{
    internal class CharacterSelectHandler
    {
        public CharacterSelectHandler(ClientSession session, ILog log, Packet packet)
        {
            if (log.AssertWarning(
                session.Player.State != GameState.CharacterSelect &&
                session.Player.State != GameState.CharacterCreation, "Trying to select character while not in character select screen.")) return;
            var result = new CharacterSelectPacket(packet);

            if (log.AssertWarning(session.Player.HasCharacterWithID(result.characterId) == false, "Trying to select a character that the player doesnt have. ID: " + result.characterId)) return;

            if (Server.Instance.GetWorld(session.Player.World, out Center center))
            {
                var pack = new ISPlayerChangeServerPacket(session.Player, result.characterId).Encode();
                center.Connection.SendPacket(pack);
                return;
            }

            // Server is offline
            session.SendPacket(result.Encode(CharacterSelectPacket.Status.SystemError));
        }
    }
}
