using log4net;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Login.PacketHandlers
{
    internal class PinAssignedHandler
    {
        public PinAssignedHandler(ClientSession session, ILog log, Packet packet)
        {
            if (log.AssertWarning(session.Player.State != Player.LoginState.PinCheck,
                "Tried to do a pin check while not in pin check state")) return;

            Packet pack = new Packet(ServerMessages.PIN_ASSIGNED);
            pack.WriteBool(true);
            session.SendPacket(pack);
        }
    }
}
