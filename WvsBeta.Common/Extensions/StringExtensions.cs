using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WvsBeta.Common.Extensions
{
    public static class StringExtensions
    {
        public static string ToFormattedString(this int value)
        {
            return value.ToString("N0", new CultureInfo("en-US"));
        }
        public static byte[] FromHexToBytes(this string value)
        {
            return Enumerable.Range(0, value.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(value.Substring(x, 2), 16))
                             .ToArray();
        }
    }
}
