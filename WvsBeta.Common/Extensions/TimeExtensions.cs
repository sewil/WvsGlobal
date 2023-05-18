using System;
using System.Globalization;

namespace WvsBeta.Common.Extensions
{
    public static class TimeExtensions
    {
        public const long TicksPerNanosecond = 100;
        public const int DaysPerYear = 365;
        public static long GetFileTimeWithAddition(this TimeSpan span)
        {
            return (MasterThread.CurrentDate + span).ToFileTimeUtc();
        }

        public static long GetTimeAsMilliseconds(this DateTime pNow)
        {
            return pNow.ToFileTimeUtc() / 10000;
        }
        public static DateTime DateFromMillis(this long millis)
        {
            return new DateTime(TimeSpan.TicksPerMillisecond * millis);
        }
        public static DateTime DateFromDoB(this int dob)
        {
            return DateTime.ParseExact(dob.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture);
        }
        public static double GetYears(this TimeSpan timeSpan)
        {
            return timeSpan.TotalDays / DaysPerYear;
        }

        public static long GetFileTimeFromCurrentTime(long ctime)
        {
            return new DateTime(ctime * TimeSpan.TicksPerMillisecond).ToFileTimeUtc();
        }

        public static string ChineseZodiac(this DateTime date)
        {
            EastAsianLunisolarCalendar cc = new ChineseLunisolarCalendar();
            int sexagenaryYear = cc.GetSexagenaryYear(date);
            int terrestrialBranch = cc.GetTerrestrialBranch(sexagenaryYear);

            string[] years = new string[] { "Rat", "Ox", "Tiger", "Rabbit", "Dragon", "Snake", "Horse", "Goat", "Monkey", "Rooster", "Dog", "Pig" };

            return years[terrestrialBranch - 1];
        }
    }
}
