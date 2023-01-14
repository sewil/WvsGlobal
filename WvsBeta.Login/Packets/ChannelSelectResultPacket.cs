using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WvsBeta.Common.Character;
using WvsBeta.Common.Objects;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Login.Packets
{
    internal struct ChannelSelectResultPacket
    {
        byte characters;
        AvatarData[] avatars;
        WorldRanking[] rankings;
        Player player;

        public ChannelSelectResultPacket(Packet packet, Player player)
        {
            player.Channel = packet.ReadByte();
            characters = packet.ReadByte();
            avatars = new AvatarData[characters];
            rankings = new WorldRanking[characters];
            this.player = player;
            for (int i = 0; i < characters; i++)
            {
                avatars[i] = new AvatarData(packet);
                var hasRanking = packet.ReadBool();
                if (hasRanking)
                {
                    rankings[i] = new WorldRanking(
                        packet.ReadInt(),
                        packet.ReadInt(),
                        packet.ReadInt(),
                        packet.ReadInt()
                    );
                }
            }
        }
        public Packet Encode()
        {
            var pack = new Packet(ServerMessages.SELECT_CHANNEL_RESULT);
            pack.WriteByte(0); //Success, other values generate error messages
            pack.WriteByte(characters);

            for (int index = 0; index < characters; index++)
            {
                var ad = avatars[index];
                ad.Encode(pack);
                var ranking = rankings[index];
                var hasRanking = ranking != null;
                pack.WriteBool(hasRanking);
                if (hasRanking)
                {
                    pack.WriteInt(ranking.worldRanking);
                    pack.WriteInt(ranking.worldChange);
                    pack.WriteInt(ranking.classRanking);
                    pack.WriteInt(ranking.classChange);
                }
                player.Characters[ad.CharacterStat.ID] = ad.CharacterStat.Name;
            }
            return pack;
        }
    }
}
