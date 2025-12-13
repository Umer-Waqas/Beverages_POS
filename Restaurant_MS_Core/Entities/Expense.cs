using System;
using System.Collections.Generic;

namespace Restaurant_MS_Core.Entities;

public partial class Expense
{
    public long ExpenseId { get; set; }

    public string? description { get; set; }

    public int PaymentMode { get; set; }

    public int PracticeId { get; set; }

    public int VoucherNo { get; set; }

    public bool AutoAdded { get; set; }

    public double? Amount { get; set; }

    public DateTime Date { get; set; }

    public long? StockId { get; set; }

    public long? SupplierId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? SyncedAt { get; set; }

    public bool IsActive { get; set; }

    public bool IsNew { get; set; }

    public bool IsUpdate { get; set; }

    public bool IsSynced { get; set; }

    public int UserId { get; set; }

    public long? ExpenseCategoryId { get; set; }

    public virtual ExpenseCategory? ExpenseCategory { get; set; }

    public virtual Stock? Stock { get; set; }

    public virtual Supplier? Supplier { get; set; }

    public virtual User User { get; set; } = null!;
}
