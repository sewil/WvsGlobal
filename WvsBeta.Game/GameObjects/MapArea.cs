using reNX.NXProperties;
using WvsBeta.Common;

namespace WvsBeta.Game.GameObjects
{
    public class MapArea
    {
        public string ID { get; set; }
        public Pos Pos1 { get; set; }
        public Pos Pos2 { get; set; }

        public MapArea(NXNode node)
        {
            ID = node.Name;
            Pos1 = new Pos(node["x1"].ValueInt16(), node["y1"].ValueInt16());
            Pos2 = new Pos(node["x2"].ValueInt16(), node["y2"].ValueInt16());
        }
        public bool IntersectsWith(Pos pos)
        {
            return Pos1.X <= pos.X && pos.X <= Pos2.X && Pos1.Y <= pos.Y && pos.Y <= Pos2.Y;
        }
    }
}
