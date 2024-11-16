using reNX.NXProperties;
using System;
using WvsBeta.Common.Objects;
using WvsBeta.Game;

public class DropData
{
    public int ItemID { get; set; }
    public int Mesos { get;  }
    public short Min { get;  }
    public short Max { get;}
    public bool Premium { get; }
    public int Chance { get; set; }
    // Expires after X days
    public ushort Period { get; }

    public long Expiration { get; }

    public DropData()
    {
    }

    public DropData(NXNode iNode, string dropper)
    {
        Expiration = Item.NoItemExpiration;

        foreach (var node in iNode)
        {
            switch (node.Name)
            {
                case "period":
                    Period = node.ValueUInt16();
                    break;
                case "dateExpire":
                    int val = node.ValueInt32();
                    int year = val / 1000000;
                    int month = val / 10000 % 100;
                    int day = val / 100 % 100;
                    int hour = val % 100;
                    var dt = new DateTime(year, month, day, hour, 0, 0);
                    var time = dt.ToFileTimeUtc() / TimeSpan.TicksPerMillisecond;

                    Expiration = time;
                    break;

                case "money":
                    Mesos = node.ValueInt32();
                    break;
                case "item":
                    ItemID = node.ValueInt32();
                    break;
                case "min":
                    Min = node.ValueInt16();
                    break;
                case "max":
                    Max = node.ValueInt16();
                    break;
                case "premium":
                    Premium = node.ValueBool();
                    break;
                case "prob":
                    Chance = GameDataProvider.CalculateDropChance(node.ValueDouble());
                    break;
                default:
                    Console.WriteLine($"Unhandled node {node.Name} in drop {dropper}");
                    break;
            }
        }
    }
}
