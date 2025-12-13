using System;
using System.Collections.Generic;

namespace Restaurant_MS_Core.Entities;

public partial class PurchaseOrder
{
    public long PurchaseOrderId { get; set; }

    public decimal PurchaseOrderNo { get; set; }

    public decimal TotalAmount { get; set; }

    public long StockId { get; set; }

    public DateTime OrderDate { get; set; }

    public long? SupplierId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? SyncedAt { get; set; }

    public bool IsActive { get; set; }

    public bool IsNew { get; set; }

    public bool IsUpdate { get; set; }

    public bool IsSynced { get; set; }

    public int UserId { get; set; }

    public virtual ICollection<PurchaseOrderItem> PurchaseOrderItems { get; set; } = new List<PurchaseOrderItem>();

    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();

    public virtual Supplier? Supplier { get; set; }

    public virtual User User { get; set; } = null!;
}
