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
    }
}
