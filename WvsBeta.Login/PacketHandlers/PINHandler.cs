using log4net;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Extensions;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Login.PacketHandlers
{
    public static class PINHandler
    {
        enum PinClientOp : byte
        {
            Cancel = 0,
            Enter = 1,
            Change = 2,
        }
        public enum PinServerOp : byte
        {
            Success = 0,
            RegisterNew = 1,
            InvalidPin = 2,
            SystemError = 3,
            EnterExisting = 4,
        }
        public static void ResetState(ClientSession session)
        {
            session.Loaded = false;
            session.Player.State = GameState.LoginScreen;
            session.Player.PIN = null;
        }
        public static void HandlePinCheck(ClientSession session, Packet packet, ILog log)
        {
            if (log.AssertWarning(session.Player.State != GameState.PinCheck,
                "Tried to do a pin check while not in pin check state")) return;

            PinClientOp clientOp = (PinClientOp)packet.ReadByte();
            bool first = packet.ReadBool();
            int n = packet.ReadInt();

            PinServerOp serverOp;
            bool requirePin = Server.Instance.RequiresPIN;
            if (!requirePin)
            {
                serverOp = PinServerOp.Success;
            }
            else if (clientOp == PinClientOp.Cancel)
            {
                ResetState(session);
                return;
            }
            else if (session.Player.PIN == null)
            {
                serverOp = PinServerOp.RegisterNew;
            }
            else if (first)
            {
                serverOp = PinServerOp.EnterExisting;
            }
            else
            {
                bool changePin = clientOp == PinClientOp.Change;
                string pin = packet.ReadString();
                bool valid = pin == session.Player.PIN;
                serverOp = valid ? changePin ? PinServerOp.RegisterNew : PinServerOp.Success : PinServerOp.InvalidPin;
            }

            SendPinOperation(session, serverOp);

            if (serverOp == PinServerOp.Success)
                session.Player.State = GameState.WorldSelect;
        }
        public static void SendPinOperation(ClientSession session, PinServerOp op)
        {
            Packet pw = new Packet(ServerMessages.PIN_OPERATION);
            pw.WriteByte((byte)op);
            session.SendPacket(pw);
        }
        public static void SendPinAssigned(ClientSession session, bool error)
        {
            Packet pw = new Packet(ServerMessages.PIN_ASSIGNED);
            pw.WriteBool(error);
            session.SendPacket(pw);
        }
        public static void HandlePinSet(ClientSession session, Packet pr)
        {
            PinClientOp cop = (PinClientOp)pr.ReadByte();
            ResetState(session);

            if (cop == PinClientOp.Enter)
            {
                string pin = pr.ReadString();
                bool success = false;
                if (pin.Length == 4)
                {
                    success = DbUpdatePin(session, pin);
                }

                SendPinAssigned(session, !success);
            }
        }
        public static bool DbUpdatePin(ClientSession session, string pin)
        {
            bool success = false;

            Server.Instance.UsersDatabase.RunTransaction(comm =>
            {
                comm.CommandText += "UPDATE users SET pin = @pin WHERE ID = @id";
                comm.Parameters.AddWithValue("@pin", pin);
                comm.Parameters.AddWithValue("@id", session.Player.ID);
                success = comm.ExecuteNonQuery() == 1;
            }, Program.MainForm.LogAppend);
            return success;
        }
    }
}
