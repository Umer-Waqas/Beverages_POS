using System;
using System.Collections.Generic;

namespace Restaurant_MS_Core.Entities;

public partial class DealItem
{
    public int DealItemId { get; set; }

    public int DealId { get; set; }

    public long ItemId { get; set; }

    public decimal Quantity { get; set; }

    public string? Unit { get; set; }

    public virtual Deal Deal { get; set; } = null!;

    public virtual Item Item { get; set; } = null!;
}
