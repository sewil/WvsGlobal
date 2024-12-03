using reNX.NXProperties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using WvsBeta.Common;
using WvsBeta.Game;
using reNX;

public class NPCData
{
    public int ID { get; private set; }
    public string Name { get; private set; }
    public string Quest { get; private set; }
    public int Trunk { get; private set; }
    public short Speed { get; private set; }
    public byte SpeakLineCount { get; private set; }
    public List<ShopItemData> Shop { get; set; }
    /// <summary>
    /// Whether this NPC exists in a map.
    /// </summary>
    public bool IsAccessible { get; set; }

    public NPCData(NXFile pClientFile, NXNode pNode)
    {
        var infoNode = pNode["info"];

        int ID = (int)Utils.ConvertNameToID(pNode.Name);
        this.ID = ID;
        Shop = new List<ShopItemData>();

        if (pClientFile.ContainsPath($"String/Npc.img/{ID}"))
        {
            Name = pClientFile.ResolvePath($"String/Npc.img/{ID}")["name"].ValueString();
        }
        else
        {
            Program.MainForm.LogAppend("String not found for NPC " + ID);
        }

        if (infoNode.ContainsChild("link"))
        {
            var linkedNpcID = infoNode["link"].ValueString();

            Trace.WriteLine($"NPC {ID} has a link to NPC {linkedNpcID}");
        }

        foreach (var subnode in infoNode)
        {
            switch (subnode.Name)
            {
                case "hideName":
                case "link":
                case "float": // Floating NPC
                case "default": // Icon used for npc chat dialog, see NPC 2030006
                case "dcTop": // Double Click mark
                case "dcRight":
                case "dcBottom":
                case "dcLeft":
                case "dcMark": break;
                case "reg":
                    {
                        // subnodes /varset /varget variable for the NPC

                        foreach (var regNode in subnode)
                        {
                            Console.WriteLine($"reg node {regNode.Name} in npc {ID}");
                        }
                        break;
                    }
                case "quest":
                    Quest = subnode.ValueString();
                    break;
                case "trunk":
                    Trunk = subnode.ValueInt32();
                    break;
                case "speed":
                    Speed = subnode.ValueInt16();
                    break;
                case "speak":
                    SpeakLineCount = (byte)subnode.ChildCount;
                    break;
                default:
                    Console.WriteLine($"Unhandled node {subnode.Name} for NPC {ID}");
                    break;
            }
        }
    }
}
