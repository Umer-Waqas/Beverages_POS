using System;
using System.Collections.Generic;

namespace Restaurant_MS_Core.Entities;

public partial class StoreClosing
{
    public int StoreClosingId { get; set; }

    public double OpeningCash { get; set; }

    public double TotalInflow { get; set; }

    public double TotalOutFlow { get; set; }

    public double SystemCash { get; set; }

    public double PhysicalCash { get; set; }

    public double CashDiff { get; set; }

    public DateTime? ClosingDate { get; set; }

    public double CashSubmittedToBank { get; set; }

    public double CashSubmittedToHO { get; set; }

    public double TotalCashSubmitted { get; set; }

    public double ClosingCash { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? SyncedAt { get; set; }

    public bool IsActive { get; set; }

    public bool IsNew { get; set; }

    public bool IsUpdate { get; set; }

    public bool IsSynced { get; set; }

    public int UserId { get; set; }

    public DateTime? OpenDate { get; set; }

    public long CreatedBy { get; set; }

    public long? StoreClosedBy { get; set; }

    public long? StoreOpenedBy { get; set; }

    public DateTime? ClosedAt { get; set; }

    public double OpeningBalance { get; set; }

    public double ClosingBalance { get; set; }

    public virtual User User { get; set; } = null!;
}
