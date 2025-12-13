
using Restaurant_MS_Core.Entities;

namespace Restaurant_MS_Core.ViewModels
{
    public class StockItemVM
    {
        public long StockItemId { get; set; }
        public Item Item { get; set; }
        public int Unit { get; set; } // int value of Unit 0 or 1
        public string UnitString { get; set; } // string value of unit 0:specified with item, 1 : units
        public int ConversionUnit { get; set; }
        public double Quantity { get; set; }
        public double BonusQuantity { get; set; }
        public double UnitCost { get; set; }
        public double TotalCost { get; set; }
        public double RetailPrice { get; set; }
        public Batch Batch { get; set; }
        public double BonusDiscount { get; set; }
        public double Discount { get; set; }
        public int DiscountType { get; set; }
        public int PercDiscType { get; set; }
        public double SalesTax { get; set; }
        public int SalesTaxType { get; set; }
        public int PercSalesTaxType { get; set; }
        public double NetValue { get; set; }
        public DateTime CreatedAt { get; set; }
        public Stock Stock { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        public bool IsNew { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsSynced { get; set; }
        public double StockConsumed { get; set; }
        public double DiscountVal { get; set; }
        public double SalesTaxVal { get; set; }

        public long StockId { get; set; }
        public long ItemId { get; set; }
        public string ItemName { get; set; }
        public string BatchName { get; set; }
        public DateTime BatchExpiry { get; set; }
        public string DiscoutnTypeString { get; set; }
        public string SalesTaxTypeString { get; set; }
    }
}
