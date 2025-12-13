using System;
using System.Collections.Generic;

namespace Restaurant_MS_Core.Entities;

public partial class campaign
{
    public long CampaignId { get; set; }

    public string Title { get; set; } = null!;

    public string SMS { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string? Notes { get; set; }

    public string Status { get; set; } = null!;
}
