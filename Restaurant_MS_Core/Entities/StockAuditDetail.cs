using System;
using System.Collections.Generic;

namespace Restaurant_MS_Core.Entities;

public partial class StockAuditDetail
{
    public int StockAuditDetailId { get; set; }

    public int Unit { get; set; }

    public int SystemQuantity { get; set; }

    public int PhysicalQuantity { get; set; }

    public int Differnce { get; set; }

    public int CurrentAdjustedQuantity { get; set; }

    public int RetailPrice { get; set; }

    public double AmountDifference { get; set; }

    public long StockAuditId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? SyncedAt { get; set; }

    public bool IsActive { get; set; }

    public bool IsNew { get; set; }

    public bool IsUpdate { get; set; }

    public bool IsSynced { get; set; }

    public int UserId { get; set; }

    public long? ItemId { get; set; }

    public virtual Item? Item { get; set; }

    public virtual StockAudit StockAudit { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
