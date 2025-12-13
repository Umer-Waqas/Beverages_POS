using System;
using System.Collections.Generic;

namespace Restaurant_MS_Core.Entities;

public partial class Stock
{
    public long StockId { get; set; }

    public decimal DocumentNo { get; set; }

    public string? SupplierInvoiceNo { get; set; }

    public string? ImagePath { get; set; }

    public double GrandTotal { get; set; }

    public double TotalPaid { get; set; }

    public bool IsAutoInsertedStock { get; set; }

    public DateTime SupplierIvoiceDate { get; set; }

    public DateTime StockDate { get; set; }

    public long? PurchaseOrderId { get; set; }

    public long? SupplierId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? SyncedAt { get; set; }

    public bool IsActive { get; set; }

    public bool IsNew { get; set; }

    public bool IsUpdate { get; set; }

    public bool IsSynced { get; set; }

    public int UserId { get; set; }

    public double GrandTotal_RetailPrice { get; set; }

    public bool IsOpeningStock { get; set; }

    public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();

    public virtual PurchaseOrder? PurchaseOrder { get; set; }

    public virtual ICollection<StockItem> StockItems { get; set; } = new List<StockItem>();

    public virtual Supplier? Supplier { get; set; }

    public virtual User User { get; set; } = null!;
}
