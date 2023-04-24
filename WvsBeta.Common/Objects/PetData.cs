using reNX.NXProperties;
using System;
using System.Collections.Generic;

namespace WvsBeta.Common.Objects
{
    public class PetData
    {
        public string Name { get; set; }
        public int ID { get; }
        public byte Hungry { get; }
        public byte Life { get; }
        public Dictionary<byte, PetReactionData> Reactions { get; }
        public PetData(NXNode node)
        {
            ID = (int)Utils.ConvertNameToID(node.Name);
            Reactions = new Dictionary<byte, PetReactionData>();

            foreach (var mNode in node["interact"])
            {
                var prd = new PetReactionData(mNode);
                Reactions.Add(prd.ReactionID, prd);
            }

            foreach (var infoNode in node["info"])
            {
                switch (infoNode.Name)
                {
                    case "icon":
                    case "iconD":
                    case "iconRaw":
                    case "iconRawD":
                    case "cash":
                        break;

                    case "hungry":
                        Hungry = infoNode.ValueByte();
                        break;
                    case "life":
                        Life = infoNode.ValueByte();
                        break;

                    default:
                        Console.WriteLine($"Unhandled Pet node {infoNode.Name} for id {ID}");
                        break;
                }
            }
        }
    }
}
