using System;
using System.Collections.Generic;

namespace Restaurant_MS_Core.Entities;

public partial class Invoice
{
    public long InvoiceId { get; set; }

    public long InvoiceRefNo { get; set; }

    public double TotalAmount { get; set; }

    public double TotalDiscount { get; set; }

    public double ModifiedDiscount { get; set; }

    public int DiscountType { get; set; }

    public double SubTotal { get; set; }

    public double GrandTotal { get; set; }

    public double TotalPaid { get; set; }

    public double TotalUnitCost { get; set; }

    public double Due { get; set; }

    public string? Note { get; set; }

    public bool IsProcedureInvoice { get; set; }

    public string? InvoiceNote { get; set; }

    public bool IsHoldInvoice { get; set; }

    public long? PatientId { get; set; }

    public long? StockConsumptionId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? SyncedAt { get; set; }

    public bool IsActive { get; set; }

    public bool IsNew { get; set; }

    public bool IsUpdate { get; set; }

    public bool IsSynced { get; set; }

    public int UserId { get; set; }

    public double TotalSalesTax { get; set; }

    public double TotalSalesTaxType { get; set; }

    public string? FBR_InvoiceNo { get; set; }

    public int OrderType { get; set; }

    public int PaymentStatus { get; set; }

    public int PaymentMethod { get; set; }

    public int OrderStatus { get; set; }

    public long? EmployeeId { get; set; }

    public bool? IsBillReceived { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual ICollection<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();

    public virtual ICollection<InvoicePayment> InvoicePayments { get; set; } = new List<InvoicePayment>();

    public virtual Patient? Patient { get; set; }

    public virtual StockConsumption? StockConsumption { get; set; }

    public virtual User User { get; set; } = null!;
}
