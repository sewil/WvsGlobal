using WvsBeta.Common.Characters;
using WvsBeta.Common.Objects;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Login.Packets
{
    internal class ChannelSelectResultPacket : Packet
    {
        public enum Status : byte
        {
            Success = 0,
            DoNothing = 1,
            IDAlreadyLoggedIn = 2,
            Banned = 3,
            IncorrectPassword = 4,
            NotRegistered = 5,
            SystemError = 6,
            IDAlreadyLoggedInLogout = 7,
            SystemError2 = 8
        }

        public ChannelSelectResultPacket(byte characters, Player player, AvatarData[] avatars, WorldRanking[] rankings, Status status = Status.Success) : base(ServerMessages.SELECT_CHANNEL_RESULT)
        {
            WriteByte((byte)status);
            WriteByte(characters);

            for (int index = 0; index < characters; index++)
            {
                var ad = avatars[index];
                ad.Encode(this);
                var ranking = rankings[index];
                var hasRanking = ranking != null;
                WriteBool(hasRanking);
                if (hasRanking)
                {
                    WriteInt(ranking.worldRanking);
                    WriteInt(ranking.worldChange);
                    WriteInt(ranking.classRanking);
                    WriteInt(ranking.classChange);
                }
                player.Characters[ad.CharacterStat.ID] = ad.CharacterStat.Name;
            }
        }
    }
}
