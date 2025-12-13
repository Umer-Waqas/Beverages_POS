using System;
using System.Collections.Generic;

namespace Restaurant_MS_Core.Entities;

public partial class InvoicePayment
{
    public long InvoicePaymentId { get; set; }

    public int PaymentType { get; set; }

    public double Payment { get; set; }

    public string? RefundReason { get; set; }

    public string? ChequeNumber { get; set; }

    public string? BankName { get; set; }

    public string? ChequeStatus { get; set; }

    public DateTime PaymentDate { get; set; }

    public double Dues { get; set; }

    public int MethodType { get; set; }

    public long? InvoiceId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? SyncedAt { get; set; }

    public bool IsActive { get; set; }

    public bool IsNew { get; set; }

    public bool IsUpdate { get; set; }

    public bool IsSynced { get; set; }

    public int UserId { get; set; }

    public virtual Invoice? Invoice { get; set; }

    public virtual User User { get; set; } = null!;
}
