using System;
using System.Collections.Generic;

namespace Restaurant_MS_Core.Entities;

public partial class LowStockMail
{
    public long LowStockMailId { get; set; }

    public DateTime LastSent { get; set; }

    public DateTime CreatedAt { get; set; }
}
