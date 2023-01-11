using MySqlX.XDevAPI.Common;
using WvsBeta.Common.Sessions;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace WvsBeta.Login.Packets
{
    internal class CheckPasswordResultPacket : Packet
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

        public CheckPasswordResultPacket(LoginState loginState, Player player, string username, byte banReason, long banExpire) : base(ServerMessages.CHECK_PASSWORD_RESULT)
        {
            WriteByte((byte)loginState); //Login State
            WriteByte(0); // nRegStatID
            WriteInt(0); // nUseDay
            if (loginState == LoginState.SUCCESS)
            {
                WriteInt(player.ID);
                WriteByte((byte)player.Gender);
                WriteBool(player.IsGM);
                WriteByte(0x01); //Country ID
                WriteString(username);
            }
            else if (loginState == LoginState.BANNED)
            {
                WriteByte(banReason);
                WriteLong(banExpire);
            }
        }
    }
}
