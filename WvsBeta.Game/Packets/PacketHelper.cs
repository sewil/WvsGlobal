using System.Diagnostics;
using System.Linq;
using WvsBeta.Common;
using WvsBeta.Common.Sessions;
using WvsBeta.Game.Packets;

namespace WvsBeta.Game
{
    internal class PacketHelper : Common.Packets.PacketHelper
    {
        public static void AddAvatar(Packet pPacket, GameCharacter pCharacter)
        {
            pPacket.WriteByte(pCharacter.Gender);
            pPacket.WriteByte(pCharacter.Skin);
            pPacket.WriteInt(pCharacter.Face);
            pPacket.WriteByte(0); // Part of equips lol
            pPacket.WriteInt(pCharacter.Hair);
            pCharacter.Inventory.GeneratePlayerPacket(pPacket);
            pPacket.WriteByte(0xFF); // Equips shown end
            pPacket.WriteInt(pCharacter.Inventory.GetEquippedItemId((short)Constants.EquipSlots.Slots.Weapon, true));
        }
    }
}