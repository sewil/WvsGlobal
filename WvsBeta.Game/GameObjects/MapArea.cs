using reNX.NXProperties;
using System.Drawing;
using WvsBeta.Common;

namespace WvsBeta.Game.GameObjects
{
    public class MapArea
    {
        public string ID { get; private set; }
        public Rectangle Rectangle { get; private set; }

        public MapArea(NXNode node)
        {
            ID = node.Name;
            Rectangle = Rectangle.FromLTRB(node["x1"].ValueInt32(), node["y1"].ValueInt32(), node["x2"].ValueInt32(), node["y2"].ValueInt32());
        }
        public bool Contains(GameCharacter chr)
        {
            return Contains(chr.Position);
        }
        public bool Contains(Pos pos)
        {
            return Rectangle.Contains(pos);
        }
    }
}
