using reNX.NXProperties;
using System.Collections.Generic;

namespace WvsBeta.Game
{
    public class Portal
    {
        public Portal(NXNode node, byte portalId)
        {
            ID = portalId;
            Type = node["pt"].ValueByte();
            X = node["x"].ValueInt16();
            Y = node["y"].ValueInt16();
            Name = node["pn"].ValueString();
            ToMapID = node["tm"].ValueInt32();
            ToName = node["tn"].ValueString();


            string script = null;

            if (node.ContainsChild("script"))
                script = node["script"].ValueString();

            if (script == "") script = null;

            Script = script;

            if (Type != 0)
            {
                Y -= 40;
            }
            Reset();
        }

        public byte ID;
        public readonly byte Type;
        public readonly short X;
        public readonly short Y;
        public readonly string Name;
        public readonly int ToMapID;
        public readonly string ToName;
        public readonly string Script;
        public bool Enabled;

        public void Reset()
        {
            Enabled = !(Type == 4 || Type == 5);
        }
    }
}