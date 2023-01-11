using WvsBeta.Common.Sessions;

namespace WvsBeta.Common.Character
{
    public class AvatarData
    {
        public GW_CharacterStat CharacterStat { get; set; }
        public AvatarLook AvatarLook { get; set; }

        public void Encode(Packet packet)
        {
            CharacterStat.Encode(packet);
            AvatarLook.Encode(packet);
            // Client checks for '!((byte)slot)', so this must be zero!
            packet.WriteByte(0);
        }

        public void Decode(Packet packet)
        {
            CharacterStat = new GW_CharacterStat();
            CharacterStat.Decode(packet);
            AvatarLook = new AvatarLook();
            AvatarLook.Decode(packet);
            packet.ReadByte();
        }
    }
}