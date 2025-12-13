using System;
using System.Collections.Generic;

namespace Restaurant_MS_Core.Entities;

public partial class Batch
{
    public long BatchId { get; set; }

    public string? BatchName { get; set; }

    public DateTime? Expiry { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? SyncedAt { get; set; }

    public bool IsActive { get; set; }

    public bool IsNew { get; set; }

    public bool IsUpdate { get; set; }

    public bool IsSynced { get; set; }

    public int UserId { get; set; }

    public virtual ICollection<AdjustmentItem> AdjustmentItems { get; set; } = new List<AdjustmentItem>();

    public virtual ICollection<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();

    public virtual ICollection<StockConsumptionItem> StockConsumptionItems { get; set; } = new List<StockConsumptionItem>();

    public virtual ICollection<StockItem> StockItems { get; set; } = new List<StockItem>();

    public virtual User User { get; set; } = null!;
}
