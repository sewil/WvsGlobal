using reNX.NXProperties;

namespace WvsBeta.Common.Objects
{
    public class PetReactionData
    {
        public byte ReactionID { get; }
        public byte Inc { get; }
        public byte Prob { get;  }
        public byte LevelMin { get; }
        public byte LevelMax { get; }
        public PetReactionData(NXNode node)
        {
            ReactionID = byte.Parse(node.Name);
            Inc = node["inc"].ValueByte();
            Prob = node["prob"].ValueByte();
            LevelMin = node["l0"].ValueByte();
            LevelMax = node["l1"].ValueByte();
        }
    }
}
