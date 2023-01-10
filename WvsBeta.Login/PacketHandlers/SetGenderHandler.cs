using log4net;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Login.PacketHandlers
{
    internal class SetGenderHandler
    {
        public SetGenderHandler(ClientSession session, ILog log, Packet packet)
        {
            if (log.AssertWarning(session.Player.State != Player.LoginState.SetupGender,
                "Tried to set gender while not in setup gender state")) return;

            if (packet.ReadBool() == false)
            {
                // 'back' to login
                session.BackToLogin();
                return;
            }

            bool isFemale = packet.ReadBool();

            Server.Instance.UsersDatabase.RunQuery(
                "UPDATE users SET gender = @gender WHERE ID = @id",
                "@id", session.Player.ID,
                "@gender", isFemale ? 1 : 0
            );

            session.Player.Gender = (byte)(isFemale ? 1 : 0);
            session.Player.State = Player.LoginState.PinCheck;

            var pack = new Packet(ServerMessages.SET_ACCOUNT_RESULT);
            pack.WriteBool(isFemale);
            pack.WriteBool(true); // Is success. If false will show "Try again!" dialog

            session.SendPacket(pack);

            // Reset player state, have to re-login.
            session.Player.State = Player.LoginState.LoginScreen;
            session.Loaded = false;
        }
    }
}
