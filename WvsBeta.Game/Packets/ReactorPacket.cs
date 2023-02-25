using WvsBeta.Common.Sessions;

namespace WvsBeta.Game
{
    public static class ReactorPacket
    {
        public static void ShowReactor(FieldReactor reactor, bool toChar = false, GameCharacter chr = null)
        {
            Packet packet = new Packet(ServerMessages.REACTOR_ENTER_FIELD);
            packet.WriteInt(reactor.ID);
            packet.WriteInt(reactor.Reactor.ID);
            packet.WriteByte(reactor.State);
            packet.WriteShort(reactor.Position.X);
            packet.WriteShort(reactor.Position.Y);
            packet.WriteBool(reactor.FacesLeft);

            if (toChar && chr != null)
                chr.SendPacket(packet);
            else
                reactor.Field.SendPacket(packet);
        }

        public static void ReactorChangedState(FieldReactor reactor)
        {
            Packet packet = new Packet(ServerMessages.REACTOR_CHANGE_STATE);
            packet.WriteInt(reactor.ID); // Confirmed
            packet.WriteByte(reactor.State); // Confirmed
            packet.WriteShort(reactor.Position.X);
            packet.WriteShort(reactor.Position.Y);
            packet.WriteShort(reactor.FrameDelay); // Frame delay, confirmed
            packet.WriteBool(reactor.FacesLeft);
            packet.WriteByte(0);
            reactor.Field.SendPacket(packet);
        }

        public static void DestroyReactor(FieldReactor reactor)
        {
            Packet packet = new Packet(ServerMessages.REACTOR_LEAVE_FIELD);
            packet.WriteInt(reactor.ID); // Confirmed
            packet.WriteByte(reactor.State); // Confirmed
            packet.WriteShort(reactor.Position.X);
            packet.WriteShort(reactor.Position.Y);
            reactor.Field.SendPacket(packet);
            //MasterThread.RepeatingAction.Start("dr-" + reactor.Field.ID + "-" + reactor.ID, time => reactor.Field.SendPacket(packet), 650, 0);
        }

        public static void HandleReactorHit(GameCharacter chr, Packet packet)
        {
            int reactorID = packet.ReadInt();
            int stance = packet.ReadInt();
            short xOffset = packet.ReadShort();
            if (!DataProvider.Maps[chr.Field.ID].ReactorPool.ShownReactors.TryGetValue(reactorID, out FieldReactor reactor))
            {
                Program.MainForm.LogAppend("Reactor " + reactorID + " not found!");
                return;
            }
            chr.Field.ReactorPool.PlayerHitReactor(chr, reactor);
        }
    }
}
