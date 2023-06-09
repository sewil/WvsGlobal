using reNX.NXProperties;
using System.Collections.Generic;
using System.Security.Principal;

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
        public static List<TValue> AddRange<TValue>(this List<TValue> list, params TValue[] values)
        {
            foreach (var value in values)
            {
                list.Add(value);
            }
            return list;
        }
        public static bool TryGetValue(this NXNode node, string key, out NXNode value)
        {
            value = null;
            bool found = node.ContainsChild(key);
            if (found)
            {
                value = node[key];
            }
            return found;
        }

        public static bool TryGetValue<TValue>(this TValue[] array, int index, out TValue value)
        {
            value = default;
            if (index < 0 || index > array.Length - 1) return false;
            value = array[index];
            return true;
        }

        public static bool TryGetValue<TValue>(this IList<TValue> list, int index, out TValue value)
        {
            value = default;
            if (index < 0 || index > list.Count - 1) return false;
            value = list[index];
            return true;
        }

        public static IList<int> Fill(this IList<int> list, int from, int to)
        {
            for (int val = from; val <= to; val++)
            {
                list.Add(val);
            }
            return list;
        }
        public static IList<short> Fill(this IList<short> list, short from, short to)
        {
            for (short val = from; val <= to; val++)
            {
                list.Add(val);
            }
            return list;
        }
        public static IEnumerable<T> TakeAndRemove<T>(this Queue<T> queue, int count)
        {
            for (int i = 0; i < count && queue.Count > 0; i++)
                yield return queue.Dequeue();
        }

        public static IList<T> Shuffle<T>(this IList<T> list)
        {
            int idxRight = list.Count;
            while (idxRight > 1)
            {
                idxRight--;
                int idxLeft = Rand32.NextBetween(0, idxRight + 1);
                var valueLeft = list[idxLeft];
                var valueRight = list[idxRight];
                list[idxLeft] = valueRight;
                list[idxRight] = valueLeft;
            }
            return list;
        }
    }
}
