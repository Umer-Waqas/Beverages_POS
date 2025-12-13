using System;
using System.Collections.Generic;

namespace Restaurant_MS_Core.Entities;

public partial class Supplier
{
    public long SupplierID { get; set; }

    public string? Name { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public string? PrimaryContactPersonName { get; set; }

    public string? PrimaryContactPersonPhone { get; set; }

    public double OpeningBalance { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? SyncedAt { get; set; }

    public bool IsActive { get; set; }

    public bool IsNew { get; set; }

    public bool IsUpdate { get; set; }

    public bool IsSynced { get; set; }

    public int UserId { get; set; }

    public bool? IsHoSupplier { get; set; }

    public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();

    public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();

    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();

    public virtual User User { get; set; } = null!;

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();
}
