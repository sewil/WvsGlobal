using WvsBeta.Common.Sessions;

namespace WvsBeta.Game
{
    public static class ReactorPacket
    {
        public static void ShowReactor(FieldReactor reactor, GameCharacter toChar = null)
        {
            Packet packet = new Packet(ServerMessages.REACTOR_ENTER_FIELD);
            packet.WriteInt(reactor.ID);
            packet.WriteInt(reactor.Reactor.ID);
            packet.WriteByte(reactor.State.State);
            packet.WriteShort(reactor.Position.X);
            packet.WriteShort(reactor.Position.Y);
            packet.WriteBool(reactor.FacesLeft);

            if (toChar != null)
                toChar.SendPacket(packet);
            else
                reactor.Field.SendPacket(packet);
        }

        public static void ReactorChangedState(FieldReactor reactor)
        {
            Packet packet = new Packet(ServerMessages.REACTOR_CHANGE_STATE);
            packet.WriteInt(reactor.ID); // Confirmed
            packet.WriteByte(reactor.State.State); // Confirmed
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
            packet.WriteByte(reactor.State.State); // Confirmed
            packet.WriteShort(reactor.Position.X);
            packet.WriteShort(reactor.Position.Y);
            reactor.Field.SendPacket(packet);
        }

        public static void HandleReactorHit(GameCharacter chr, Packet packet)
        {
            int reactorId = packet.ReadInt();
            int stance = packet.ReadInt();
            short xOffset = packet.ReadShort();
            chr.Field.ReactorPool.HitReactor(reactorId, chr);
        }
    }
}
