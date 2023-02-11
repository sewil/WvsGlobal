public class ShopItemData
{
    public int ItemID { get; set; }
    public int Stock { get; set; }
    public int Price { get; set; }
    public byte Period { get; set; }
    public ShopItemData(int itemid)
    {
        ItemID = itemid;
    }
    public ShopItemData(int itemid, int price)
    {
        ItemID = itemid;
        Price = price;
    }
}