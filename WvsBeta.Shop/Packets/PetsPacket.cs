using WvsBeta.Common.Sessions;
using WvsBeta.Common.Objects;

namespace WvsBeta.Shop.Packets
{
    internal class PetsPacket
    {
        public static void HandleMovePet(ShopCharacter chr, Packet packet)
        {
            // 55 F2 FE 83 FF 03 00 D3 FE 83 FF 83 FF 00 00 01 00 03 2C 01 00 C9 FE 83 FF FB FF 00 00 01 00 05 96 00 00 C9 FE 83 FF 00 00 00 00 01 00 05 3C 00 00
            var movePath = new MovePath();
            movePath.DecodeFromPacket(packet, MovePath.MovementSource.Pet);
            chr.TryTraceMovement(movePath);
            //PacketHelper.ValidateMovePath(petItem.MovableLife, movePath);
        }
    }
}
