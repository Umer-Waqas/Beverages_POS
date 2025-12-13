using System;
using System.Collections.Generic;

namespace Restaurant_MS_Core.Entities;

public partial class PurchaseOrderItem
{
    public long PurchaseOrderItemId { get; set; }

    public int Unit { get; set; }

    public decimal PurchaseOrderNo { get; set; }

    public double Quantity { get; set; }

    public double UnitCost { get; set; }

    public double TotalAmount { get; set; }

    public long ItemId { get; set; }

    public long PurchaseOrderId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? SyncedAt { get; set; }

    public bool IsActive { get; set; }

    public bool IsNew { get; set; }

    public bool IsUpdate { get; set; }

    public bool IsSynced { get; set; }

    public int UserId { get; set; }

    public virtual Item Item { get; set; } = null!;

    public virtual PurchaseOrder PurchaseOrder { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
