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
        public static void SafeAdd<TKey, TValue>(this IDictionary<TKey, HashSet<TValue>> dictionary, TKey key, TValue value)
        {
            if (!dictionary.ContainsKey(key)) dictionary.Add(key, new HashSet<TValue>());
            dictionary[key].Add(value);
        }
        public static TValue GetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            dictionary.TryGetValue(key, out TValue value);
            return value;
        }
        public static void AddRange<TValue>(this IList<TValue> list, params TValue[] values)
        {
            foreach (var value in values)
            {
                list.Add(value);
            }
        }
    }
}
