using System;

namespace WvsBeta.Common.Extensions
{
    public static class TimeExtensions
    {
        public const long TicksPerNanosecond = 100;
        public static long GetFileTimeWithAddition(this TimeSpan span)
        {
            return (MasterThread.CurrentDate + span).ToFileTimeUtc();
        }

        public static long GetTimeAsMilliseconds(this DateTime pNow)
        {
            return pNow.ToFileTime() / 10000;
        }
        public static DateTime DateFromMillis(this long millis)
        {
            return new DateTime(TimeSpan.TicksPerMillisecond * millis);
        }
    }
}
