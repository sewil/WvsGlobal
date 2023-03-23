using reNX.NXProperties;

namespace WvsBeta.Common.Objects
{
    public class QuestMob
    {
        public int ID { get; }
        public int MobID { get; }
        public int Count { get; }
        public QuestMob(NXNode node)
        {
            ID = int.Parse(node.Name);
            MobID = node["id"].ValueInt32();
            Count = node["count"].ValueInt32();
        }
    }
}
