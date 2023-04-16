using System.Collections.Generic;
using System.Linq;

namespace WvsBeta.Common.Objects
{
    public enum StockState
    {
        InStock = -1,
        DefaultState = 0,
        OutOfStock = 1,
        NotAvailable = 2,
    }

    public enum CommodityGenders : sbyte
    {
        NotApplicable = -1,
        Male = 0,
        Female = 1,
        Both = 2
    }

    public enum CommodityCategory
    {
        Main = 1,
        Event = 2,
        Equip = 3,
        Use = 4,
        Etc = 5,
        Setup = 6,
        Pet = 7,
        Package = 8,
        Quest = 9
    }
    public static class CommodityExtensions
    {
        public static IEnumerable<CommodityCategory> GetCategories()
        {
            return ((IEnumerable<CommodityCategory>)System.Enum.GetValues(typeof(CommodityCategory)))
             .Except(new CommodityCategory[] { CommodityCategory.Quest });
        }
    }

    public enum CommodityClass : int
    {
        New = 0,
        Sale = 1,
        Hot = 2,
        Event = 3
    }
    public interface ICommodityInfo
    {
        int SerialNumber { get; }
        int ItemID { get; }
        short Count { get; }
        short Period { get; }
        bool OnSale { get; }
        int Price { get; }
        CommodityGenders Gender { get; }
        CommodityClass Class { get; }
        StockState StockState { get; }
        CommodityCategory Category { get; }
    }
}
