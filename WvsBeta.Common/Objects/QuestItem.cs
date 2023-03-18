using reNX.NXProperties;
using WvsBeta.Common.Enums;

namespace WvsBeta.Common.Objects
{
    public class QuestItem
    {
        public int ItemID { get; }
        public short Amount { get; }
        public int Prop { get; }
        public PlayerGender Gender { get; }
        public QuestItem(NXNode node)
        {
            foreach (var subNode in node)
            {
                switch (subNode.Name)
                {
                    case "count":
                        Amount = subNode.ValueInt16();
                        break;
                    case "id":
                        ItemID = subNode.ValueInt32();
                        break;
                    case "prop":
                        Prop = subNode.ValueInt32();
                        break;
                    case "gender":
                        Gender = (PlayerGender)subNode.ValueByte();
                        break;
                }
            }
        }
    }
}
