using log4net;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Login.PacketHandlers
{
    internal class CharacterSelectHandler
    {
        public CharacterSelectHandler(ClientSession session, ILog log, Packet packet)
        {
            if (log.AssertWarning(
                session.Player.State != Player.LoginState.CharacterSelect &&
                session.Player.State != Player.LoginState.CharacterCreation, "Trying to select character while not in character select screen.")) return;
            int charid = packet.ReadInt();

            if (log.AssertWarning(session.Player.HasCharacterWithID(charid) == false, "Trying to select a character that the player doesnt have. ID: " + charid)) return;

            if (Server.Instance.GetWorld(session.Player.World, out Center center))
            {
                center.Connection.RequestCharacterConnectToWorld(session.Player.SessionHash, charid, session.Player.World, session.Player.Channel);
                return;
            }

            // Server is offline
            var pw = new Packet(ServerMessages.SELECT_CHARACTER_RESULT);
            pw.WriteByte(6); // Connection failed due to system error
            pw.WriteByte(0);
            session.SendPacket(pw);
        }
    }
}
