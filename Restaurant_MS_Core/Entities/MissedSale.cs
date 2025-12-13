using System;
using System.Collections.Generic;

namespace Restaurant_MS_Core.Entities;

public partial class MissedSale
{
    public int MissedSaleId { get; set; }

    public long ItemId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? SyncedAt { get; set; }

    public bool IsActive { get; set; }

    public bool IsNew { get; set; }

    public bool IsUpdate { get; set; }

    public bool IsSynced { get; set; }

    public int UserId { get; set; }

    public virtual Item Item { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
