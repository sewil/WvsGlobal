using System.Collections.Generic;

namespace WvsBeta.Common.Extensions
{
    public static class EnumerableExtensions
    {
        public static void SafeAdd<TKey, TValue>(this IDictionary<TKey, IList<TValue>> dictionary, TKey key, TValue value)
        {
            if (!dictionary.ContainsKey(key)) dictionary.Add(key, new List<TValue>());
            dictionary[key].Add(value);
        }
        public static TValue GetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            dictionary.TryGetValue(key, out TValue value);
            return value;
        }
    }
}
