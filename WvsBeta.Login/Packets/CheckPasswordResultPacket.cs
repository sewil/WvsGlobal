using WvsBeta.Common.Sessions;
using WvsBeta.Login.Objects;

namespace WvsBeta.Login.Packets
{
    internal struct CheckPasswordResultPacket
    {
        public enum LoginState : byte
        {
            SUCCESS = 0,
            BANNED = 2,
            DELETED = 3,
            BLOCKED = 3,
            INVALID_PASSWORD = 4,
            NOT_REGISTERED = 5,
            SYS_ERROR = 6,
            ALREADY_ONLINE = 7,
            SYSTEM_ERROR = 9,
            TOO_MANY_REQUESTS = 10,
            OLDER_THAN_20 = 11,
            MASTER_CANNOT_LOGIN_ON_THIS_IP = 13,
            VERIFY_EMAIL = 16,
            WRONG_GATEWAY = 17,
            CHANGE_INFO = 17,
            EULA = 23
        }
        public enum BanReason : byte
        {
            Unknown,
            Hacking,
            Botting,
            IllicitAdvertising,
            Harassment,
            ProfaneLanguage,
            Scamming,
            Misconduct,
            RWT,
            IllegalFunding
        }

        public readonly LoginState loginState;
        public readonly Player player;
        public readonly string username;
        public readonly BanReason banReason;
        public readonly long banExpire;

        public CheckPasswordResultPacket(LoginState loginState, Player player, string username, BanReason banReason, long banExpire)
        {
            this.loginState = loginState;
            this.player = player;
            this.username = username;
            this.banReason = banReason;
            this.banExpire = banExpire;
        }
        public Packet Encode()
        {
            var packet = new Packet(ServerMessages.CHECK_PASSWORD_RESULT);
            packet.WriteByte((byte)loginState); //Login State
            packet.WriteByte(0); // nRegStatID
            packet.WriteInt(0); // nUseDay
            if (loginState == LoginState.SUCCESS)
            {
                packet.WriteInt(player.ID);
                packet.WriteByte((byte)player.Gender);
                packet.WriteBool(player.IsGM);
                packet.WriteByte(0x01); //Country ID
                packet.WriteString(username);
            }
            else if (loginState == LoginState.BANNED)
            {
                packet.WriteByte((byte)banReason);
                packet.WriteLong(banExpire);
            }
            return packet;
        }
    }
}
