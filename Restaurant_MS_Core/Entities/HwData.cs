using System;
using System.Collections.Generic;

namespace Restaurant_MS_Core.Entities;

public partial class HwData
{
    public int HwDataId { get; set; }

    public string? SystemExpiry { get; set; }

    public bool IsSyncManual { get; set; }
}
