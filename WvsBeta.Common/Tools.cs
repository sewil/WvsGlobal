using System;

namespace WvsBeta.Common
{
    public class Tools
    {
        public static long GetFileTimeWithAddition(TimeSpan span)
        {
            return (MasterThread.CurrentDate + span).ToFileTimeUtc();
        }
        
        public static long GetTimeAsMilliseconds(DateTime pNow)
        {
            return pNow.ToFileTime() / 10000;
        }
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
    }
}
