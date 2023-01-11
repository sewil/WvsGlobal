using log4net;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Login.PacketHandlers
{
    internal class PinOperationHandler
    {
        public PinOperationHandler(ClientSession session, ILog log, Packet packet)
        {
            if (log.AssertWarning(session.Player.State != Player.LoginState.PinCheck,
                "Tried to do a pin check while not in pin check state")) return;

            // PINs currently disabled. TODO when we update. Just send successful auth packet for now.
            Packet pack = new Packet(ServerMessages.PIN_OPERATION);
            pack.WriteBool(false);
            session.SendPacket(pack);

            session.Player.State = Player.LoginState.WorldSelect;
        }
    }
}
