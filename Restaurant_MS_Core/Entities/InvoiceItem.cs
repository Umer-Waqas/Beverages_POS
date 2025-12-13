using Restaurant_MS_Core.ViewModels;
using System;
using System.Collections.Generic;

namespace Restaurant_MS_Core.Entities;

public partial class InvoiceItem
{
    public long InvoiceItemId { get; set; }

    public string? ProcedureDescription { get; set; }

    public int Unit { get; set; }

    public double Rate { get; set; }

    public double Quantity { get; set; }

    public double Amount { get; set; }

    public double NetAmount { get; set; }

    public double Discount { get; set; }

    public int DiscountType { get; set; }

    public double ReturnedQuantity { get; set; }

    public double TotalCostPrice { get; set; }

    public double PerUnitCostPrice { get; set; }

    public int SerialNo { get; set; }

    public bool AffectStock { get; set; }

    public long InvoiceId { get; set; }

    public long ItemId { get; set; }

    public long? CategoryId { get; set; }

    public long BatchId { get; set; }

    public bool IsOptionalBatch { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? SyncedAt { get; set; }

    public bool IsActive { get; set; }

    public bool IsNew { get; set; }

    public bool IsUpdate { get; set; }

    public bool IsSynced { get; set; }

    public int UserId { get; set; }

    public long? Invoice_InvoiceId { get; set; }

    public double? BonusQuantity { get; set; }

    public double? SalesTax { get; set; }

    public int? SalesTaxType { get; set; }

    public bool IsLineItemDiscount { get; set; }

    public double CalculatedNetAmount { get; set; }

    public virtual Batch Batch { get; set; } = null!;

    public virtual Category? Category { get; set; }

    public virtual Invoice Invoice { get; set; } = null!;

    public virtual Item Item { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
