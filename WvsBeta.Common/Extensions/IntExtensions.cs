using System;
using System.Globalization;

namespace WvsBeta.Common.Extensions
{
    public static class IntExtensions
    {
        public static string Culture(this int value)
        {
            return value.ToString("N0", new CultureInfo("en-US"));
        }
        public static string Pluralis(this int value, string plural, string singular)
        {
            return value == 1 ? singular : plural;
        }
        public static int Clamp(int n, int min, int max)
        {
            return Math.Max(min, Math.Min(max, n));
        }
    }
}
