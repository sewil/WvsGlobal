using reNX.NXProperties;
using System;
using WvsBeta.Common.Enums;

namespace WvsBeta.Common.Objects
{
    [Flags]
    public enum QuestJob : byte
    {
        Beginner = 1,
        Warrior = 2,
        Magician = 4,
        Bowman = 8,
        Thief = 16,
        GM = 0xFF
    }
    public class QuestItem
    {
        public int ItemID { get; }
        public short Amount { get; }
        public int Prop { get; }
        public PlayerGender Gender { get; } = PlayerGender.NotApplicable;
        public QuestJob? Job { get; }
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
                    case "job":
                        Job = (QuestJob)subNode.ValueByte();
                        break;
                }
            }
        }
    }
}
