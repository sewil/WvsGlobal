using System;
using System.Diagnostics;
using System.Linq;
using log4net;
using WvsBeta.Common.Objects;
using WvsBeta.Game.Packets;

namespace WvsBeta.Game
{
    public partial class GameCharacter
    {
        public bool AssertForHack(bool isHack, string hackType, bool seriousHack = true)
        {
            if (!isHack || IsAdmin) return false;

            HackLog.Warn(hackType);
            Trace.WriteLine(hackType);

            if (IsGM || IsAdmin) return false;
            HackLog.Warn(hackType);
            if (seriousHack && HacklogMuted < MasterThread.CurrentDate)
            {
                ChatPacket.SendNoticeGMs(
                    $"Check '{hackType}' triggered! Character: '{Name}', Map: '{MapID}'.",
                    ChatPacket.MessageTypes.Megaphone
                );
            }

            return isHack;
        }

        public enum BanReasons
        {
            Hack = 1,
            Macro = 2,
            Advertisement = 3,
            Harassment = 4,
            BadLanguage = 5,
            Scam = 6,
            Misconduct = 7,
            Sell = 8,
            ICash = 9
        }

        public bool PermaBan(string reason, BanReasons banReason = BanReasons.Hack, bool doNotBanForNow = false, int extraDelay = 0)
        {
#if DEBUG
            return false;
#endif
            if (IsAdmin) doNotBanForNow = true;
            if (!doNotBanForNow)
            {
                Server.Instance.AddDelayedBanRecord(this, reason, banReason, extraDelay);
                return true;
            }
            else
            {
                ChatPacket.SendNoticeGMs(
                    $"Would've perma'd {Name} (uid {UserID}, cid {ID}), reason: {reason}",
                    ChatPacket.MessageTypes.Notice
                );
                return false;
            }
        }

        public bool IsInvalidTextInput(string inputType, string str, int maxLength = int.MaxValue, int minLength = 0)
        {
            if (AssertForHack(str.Length < minLength, $"Invalid text input '{str}' for inputType {inputType}: text not long enough (min: {minLength})", false) ||
                AssertForHack(str.Length > maxLength, $"Invalid text input '{str}' for inputType {inputType}: text too long (max: {maxLength})", false))
                return true;

            return AssertForHack(str.Any(c => c < 0x20 || c >= 0x80), $"Invalid text input '{str}' for inputType {inputType}", false);
        }
    }
}
