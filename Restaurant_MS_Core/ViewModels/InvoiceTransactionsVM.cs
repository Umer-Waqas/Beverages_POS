using Restaurant_MS_Core.Entities;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Core.ViewModels
{
    public class InvoiceTransactionsVM
    {
        public long InvoiceId { get; set; }
        public long InvoiceRefNo { get; set; }
        public string InvoiceNoString { get; set; }
        public DateTime CreatedAt { get; set; }
        public double TotalAmount { get; set; }
        public double TotalDiscount { get; set; }
        public int DiscountType { get; set; }
        public List<InvoiceItemVM> InvoiceItems { get; set; }
        public double SubTotal { get; set; }
        public double GrandTotal { get; set; }
        public double Due { get; set; }
        public double Advance { get; set; }
        public Patient Patient { get; set; }

        public string PatientName { get; set; }
        public string MrNo { get; set; }
        public double TotalPaid { get; set; }
        public double TotalRefund { get; set; }
        public string ReasonForRefund { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime PaymentCreatedAt { get; set; }
        public string PaymentType { get; set; }
        public DateTime RefundDate { get; set; }
        public double SaleQuantity { get; set; }
        public double ReturnedQty { get; set; }
        public string Description { get; set; }
        public List<InvoicePayment> InvoicePayments { get; set; }
        public User User { get; set; }
        public string UserName { get; set; }
        public DateTime DeletedAt { get; set; }
        public bool IsProcedureInvoice { get; set; }
        public int PaymentStatus { get; set; }
        public string EmployeeName { get; set; }
        
    }
}