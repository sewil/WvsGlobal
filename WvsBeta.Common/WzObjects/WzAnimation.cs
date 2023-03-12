using reNX.NXProperties;
using System.Collections.Generic;

namespace WvsBeta.Common.WzObjects
{
    public class WzAnimation
    {
        public readonly IDictionary<int, WzFrame> Frames;
        public readonly int AnimationTime;
        public WzAnimation(NXNode node)
        {
            Frames = new Dictionary<int, WzFrame>();
            foreach (var subNode in node)
            {
                if (int.TryParse(subNode.Name, out int nFrame))
                {
                    Frames.Add(nFrame, new WzFrame(subNode));
                }
            }
            foreach (var frame in Frames)
            {
                AnimationTime += frame.Value.Delay;
            }
        }
    }
    public class WzFrame
    {
        public readonly int Delay;
        public readonly int Z;
        public WzFrame(NXNode node)
        {
            foreach (var subNode in node)
            {
                switch (subNode.Name)
                {
                    case "delay":
                        Delay = subNode.ValueInt32();
                        break;
                    case "z":
                        Z = subNode.ValueInt32();
                        break;
                    default: break;
                }
            }
        }
    }
}
