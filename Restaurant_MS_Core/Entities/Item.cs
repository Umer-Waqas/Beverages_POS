using System;
using System.Collections.Generic;

namespace Restaurant_MS_Core.Entities;

public partial class Item
{
    public long ItemId { get; set; }

    public string? ItemName { get; set; }

    public bool? IsRawItem { get; set; }

    public string? Barcode { get; set; }

    public int? RackId { get; set; }

    public string? Unit { get; set; }

    public int ConversionUnit { get; set; }

    public int ReOrderingLevel { get; set; }

    public double RetailPrice { get; set; }

    public int OpeningStock { get; set; }

    public string? ChecmicalName { get; set; }

    public double UnitCostPrice { get; set; }

    public bool IsNarcotic { get; set; }

    public long MappedCategoryId { get; set; }

    public long MappedProductId { get; set; }

    public bool IsDefault { get; set; }

    public bool IsDefaultLoaded { get; set; }

    public bool IsSyncable { get; set; }

    public long? CategoryId { get; set; }

    public long? SubCategoryId { get; set; }

    public long? ManufacturerId { get; set; }

    public int CurrentQuantity { get; set; }

    public double CurrentRetailPrice { get; set; }

    public double CurrentCostPrice { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? SyncedAt { get; set; }

    public bool IsActive { get; set; }

    public bool IsNew { get; set; }

    public bool IsUpdate { get; set; }

    public bool IsSynced { get; set; }

    public int UserId { get; set; }

    public bool? CheckStockOnSale { get; set; }

    public int ItemTypeId { get; set; }

    public virtual ICollection<AdjustmentItem> AdjustmentItems { get; set; } = new List<AdjustmentItem>();

    public virtual Category? Category { get; set; }

    public virtual ICollection<DealItem> DealItems { get; set; } = new List<DealItem>();

    public virtual ICollection<Deal> Deals { get; set; } = new List<Deal>();

    public virtual ICollection<DiscountItem> DiscountItems { get; set; } = new List<DiscountItem>();

    public virtual ICollection<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();

    public virtual ItemType ItemType { get; set; } = null!;

    public virtual Manufacturer? Manufacturer { get; set; }

    public virtual ICollection<MissedSale> MissedSales { get; set; } = new List<MissedSale>();

    public virtual ICollection<PurchaseOrderItem> PurchaseOrderItems { get; set; } = new List<PurchaseOrderItem>();

    public virtual Rack? Rack { get; set; }

    public virtual ICollection<RecipeItem> RecipeItems { get; set; } = new List<RecipeItem>();

    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();

    public virtual ICollection<StockAuditDetail> StockAuditDetails { get; set; } = new List<StockAuditDetail>();

    public virtual ICollection<StockConsumptionItem> StockConsumptionItems { get; set; } = new List<StockConsumptionItem>();

    public virtual ICollection<StockItem> StockItems { get; set; } = new List<StockItem>();

    public virtual Category? SubCategory { get; set; }

    public virtual ICollection<TemplateItem> TemplateItems { get; set; } = new List<TemplateItem>();

    public virtual User User { get; set; } = null!;

    public virtual ICollection<Supplier> Suppliers { get; set; } = new List<Supplier>();
}
