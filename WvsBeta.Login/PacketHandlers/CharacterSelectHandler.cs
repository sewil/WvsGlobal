using log4net;
using WvsBeta.Common.Sessions;
using WvsBeta.Login.Packets;

namespace WvsBeta.Login.PacketHandlers
{
    internal class CharacterSelectHandler
    {
        public CharacterSelectHandler(ClientSession session, ILog log, Packet packet)
        {
            if (log.AssertWarning(
                session.Player.State != Player.LoginState.CharacterSelect &&
                session.Player.State != Player.LoginState.CharacterCreation, "Trying to select character while not in character select screen.")) return;
            var result = new CharacterSelectPacket(packet);

            if (log.AssertWarning(session.Player.HasCharacterWithID(result.charid) == false, "Trying to select a character that the player doesnt have. ID: " + result.charid)) return;

            if (Server.Instance.GetWorld(session.Player.World, out Center center))
            {
                center.Connection.RequestCharacterConnectToWorld(session.Player.SessionHash, result.charid, session.Player.World, session.Player.Channel);
                return;
            }

            // Server is offline
            session.SendPacket(result.Encode());
        }
    }
}
