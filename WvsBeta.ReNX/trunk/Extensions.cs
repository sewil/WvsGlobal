using reNX.NXProperties;

namespace reNX
{
    public static class Extensions
    {
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
    }
}
