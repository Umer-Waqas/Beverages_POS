using System;
using System.Collections.Generic;

namespace Restaurant_MS_Core.Entities;

public partial class Category
{
    public long CategoryId { get; set; }

    public long? ParentId { get; set; }

    public string? Name { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? SyncedAt { get; set; }

    public bool IsActive { get; set; }

    public bool IsNew { get; set; }

    public bool IsUpdate { get; set; }

    public bool IsSynced { get; set; }

    public int UserId { get; set; }

    public bool IncludeInSummaryReport { get; set; }

    public bool? IsSystemCategory { get; set; }

    public virtual ICollection<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();

    public virtual ICollection<Item> ItemCategories { get; set; } = new List<Item>();

    public virtual ICollection<Item> ItemSubCategories { get; set; } = new List<Item>();

    public virtual User User { get; set; } = null!;
}
