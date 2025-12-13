using System;
using System.Collections.Generic;

namespace Restaurant_MS_Core.Entities;

public partial class StockItem
{
    public long StockItemId { get; set; }

    public int Unit { get; set; }

    public double Quantity { get; set; }

    public double BonusQuantity { get; set; }

    public double UnitCost { get; set; }

    public double TotalCost { get; set; }

    public double RetailPrice { get; set; }

    public double BonusDiscount { get; set; }

    public double Discount { get; set; }

    public int DiscountType { get; set; }

    public int PercDiscType { get; set; }

    public double SalesTax { get; set; }

    public int SalesTaxType { get; set; }

    public int PercSalesTaxType { get; set; }

    public double NetValue { get; set; }

    public double DiscountValue { get; set; }

    public double SalesTaxValue { get; set; }

    public long? StockConsumptionItemId { get; set; }

    public long ItemId { get; set; }

    public long BatchId { get; set; }

    public long StockId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? SyncedAt { get; set; }

    public bool IsActive { get; set; }

    public bool IsNew { get; set; }

    public bool IsUpdate { get; set; }

    public bool IsSynced { get; set; }

    public int UserId { get; set; }
    public virtual Batch Batch { get; set; } = null!;

    public virtual Item Item { get; set; } = null!;

    public virtual Stock Stock { get; set; } = null!;

    public virtual StockConsumptionItem? StockConsumptionItem { get; set; }

    public virtual User User { get; set; } = null!;
}
