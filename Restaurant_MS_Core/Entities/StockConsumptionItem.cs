using System;
using System.Collections.Generic;

namespace Restaurant_MS_Core.Entities;

public partial class StockConsumptionItem
{
    public long StockConsumptionItemId { get; set; }

    public int Unit { get; set; }

    public double Quantity { get; set; }

    public int ConsumptionType { get; set; }

    public string? Comment { get; set; }

    public long ItemId { get; set; }

    public long StockConsumptionId { get; set; }

    public long BatchId { get; set; }

    public long InvoiceId { get; set; }

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

    public virtual StockConsumption StockConsumption { get; set; } = null!;

    public virtual ICollection<StockItem> StockItems { get; set; } = new List<StockItem>();

    public virtual User User { get; set; } = null!;
}
