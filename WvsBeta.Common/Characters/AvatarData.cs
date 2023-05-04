using WvsBeta.Common.Sessions;

namespace WvsBeta.Common.Characters
{
    public class AvatarData
    {
        public GW_CharacterStat CharacterStat { get; set; }
        public AvatarLook AvatarLook { get; set; }
        public AvatarData()
        {
        }
        public AvatarData(Packet packet)
        {
            CharacterStat = new GW_CharacterStat();
            CharacterStat.Decode(packet);
            AvatarLook = new AvatarLook(packet);
        }

        public void Encode(Packet packet)
        {
            CharacterStat.Encode(packet);
            AvatarLook.Encode(packet);
        }
    }
}