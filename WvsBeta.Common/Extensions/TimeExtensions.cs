using System;
using System.Globalization;

namespace WvsBeta.Common.Extensions
{
    public static class TimeExtensions
    {
        public const long DayMillis = 24*3600*1000;

        public static DateTime DateFromMillis(this long millis)
        {
            return new DateTime(TimeSpan.TicksPerMillisecond * millis);
        }

        /// <summary>
        /// Converts DateTime to utc file time in milliseconds.
        /// </summary>
        public static long ToFileTimeMillis(this DateTime dateTime)
        {
            return dateTime.ToFileTimeUtc() / TimeSpan.TicksPerMillisecond;
        }

        public static double GetAge(int dob)
        {
            var dt = MasterThread.CurrentDate - DateTime.ParseExact(dob.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture);
            return dt.TotalDays / 365;
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
