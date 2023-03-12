using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace WvsBeta.Common.Extensions
{
    public static class StringExtensions
    {
        public static byte[] FromHexToBytes(this string value)
        {
            return Enumerable.Range(0, value.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(value.Substring(x, 2), 16))
                             .ToArray();
        }

        /// <summary>
        /// Tries parsing to int, otherwise returns int default (0)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToIntSafe(this string value)
        {
            if (int.TryParse(value, out int result))
            {
                return result;
            }
            else
            {
                return 0;
            }
        }
    }
}
