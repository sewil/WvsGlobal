using log4net;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Extensions;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Login.PacketHandlers
{
    internal class UpdatePINHandler
    {
        public UpdatePINHandler(ClientSession session, ILog log, Packet packet)
        {
            if (log.AssertWarning(session.Player.State != GameState.PinCheck,
                        "Tried to update pin while not in pincheck")) return;

            packet.ReadByte();

        }
    }
}
