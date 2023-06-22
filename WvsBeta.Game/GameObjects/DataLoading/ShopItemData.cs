using reNX.NXProperties;

public class ShopItemData
{
    public int ItemID { get; }
    public int Stock { get; set; }
    public int Price { get; }
    public byte Period { get; }
    public double UnitPrice { get; }
    public ShopItemData(NXNode node)
    {
        foreach (var subNode in node)
        {
            switch (subNode.Name)
            {
                case "item":
                    ItemID = subNode.ValueInt32();
                    break;
                case "price":
                    Price = subNode.ValueInt32();
                    break;
                case "period":
                    Period = subNode.ValueByte();
                    break;
                case "stock":
                    Stock = subNode.ValueInt32();
                    break;
                case "unitPrice":
                    UnitPrice = subNode.ValueDouble();
                    break;
                default:
                    break;
            }
        }
    }
}
