using System;
using System.Collections.Generic;

namespace Restaurant_MS_Core.Entities;

public partial class StockConsumption
{
    public long StockConsumptionId { get; set; }

    public string? Comment { get; set; }

    public long InvoiceId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? SyncedAt { get; set; }

    public bool IsActive { get; set; }

    public bool IsNew { get; set; }

    public bool IsUpdate { get; set; }

    public bool IsSynced { get; set; }

    public int UserId { get; set; }

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual ICollection<StockConsumptionItem> StockConsumptionItems { get; set; } = new List<StockConsumptionItem>();

    public virtual User User { get; set; } = null!;
}
