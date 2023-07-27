using System;
using System.Text.RegularExpressions;

namespace WvsBeta.Common
{
    public class Tools
    {
        public static string Shuffle(int amount, string value)
        {
            char[] array = value.ToCharArray();
            for (int i = 0; i < amount; i++)
            {
                int n = array.Length;
                while (n > 1)
                {
                    n--;
                    int k = Rand32.NextBetween(0, n + 1);
                    char c = array[k];
                    array[k] = array[n];
                    array[n] = c;
                }
            }
            return new string(array);
        }
        public static (int min, int max) ParseRange(string range)
        {
            var m = Regex.Match(range, @"(\d+)(-(\d+))?");
            if (!m.Success) throw new ArgumentException();
            int min = int.Parse(m.Groups[1].Value);
            int max = 0;
            if (m.Groups[3].Success)
            {
                max = int.Parse(m.Groups[3].Value);
            }
            return (min, max);
        }
        public static bool CheckRange(int value, int min = 0, int max = 0)
        {
            return (min <= 0 || min <= value) && (max <= 0 || value <= max);
        }
    }
}
