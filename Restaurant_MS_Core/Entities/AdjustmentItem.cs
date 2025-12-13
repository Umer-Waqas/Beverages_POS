using System;
using System.Collections.Generic;

namespace Restaurant_MS_Core.Entities;

public partial class AdjustmentItem
{
    public long AdjustmentItemId { get; set; }

    public int Unit { get; set; }

    public double Quantity { get; set; }

    public string? Reason { get; set; }

    public long AdjustmentId { get; set; }

    public long ItemId { get; set; }

    public long BatchId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? SyncedAt { get; set; }

    public bool IsActive { get; set; }

    public bool IsNew { get; set; }

    public bool IsUpdate { get; set; }

    public bool IsSynced { get; set; }

    public int UserId { get; set; }

    public double RetailPrice { get; set; }

    public double TotalRetailPrice { get; set; }

    public double CostPrice { get; set; }

    public double TotalCostPrice { get; set; }

    public int AdjustmentType { get; set; }

    public virtual Adjustment Adjustment { get; set; } = null!;
    public virtual Batch Batch { get; set; } = null!;

    public virtual Item Item { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
