using System;
using System.Collections.Generic;

namespace Restaurant_MS_Core.Entities;

public partial class Deal
{
    public int DealId { get; set; }

    public long ItemId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<DealItem> DealItems { get; set; } = new List<DealItem>();

    public virtual Item Item { get; set; } = null!;
}
