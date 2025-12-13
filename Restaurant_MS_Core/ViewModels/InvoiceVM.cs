using Restaurant_MS_Core.Entities;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Core.ViewModels
{
    public class InvoiceVM
    {
        public long InvoiceId { get; set; }
        public long? EmployeeId { get; set; }
        public long InvoiceRefNo { get; set; }
        public DateTime CreatedAt { get; set; }
        public double TotalAmount { get; set; }
        public double TotalDiscount { get; set; }
        public double ModifiedDiscount { get; set; }
        public int DiscountType { get; set; }
        public double SubTotal { get; set; }
        public double GrandTotal { get; set; }
        public double FirstPayment { get; set; }
        public int OrderType { get; set; }
        public int? SeatingTableId { get; set; }
        public int OrderStatus { get; set; }
        public int PaymentStatus { get; set; }
        public double Due { get; set; }
        public double Balance { get; set; }
        public double Advance { get; set; }
        public PatientVM Patient { get; set; }
        public Patient ObjPatient { get; set; }
        public double TotalPaid { get; set; }
        public int PaymentType { get; set; }
        public double TotalRefund { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime RefundDate { get; set; }
        public string PatientName { get; set; }
        public string NarcoticDrugs { get; set; }
        public int Quantity { get; set; }
        public string Note { get; set; }
        public string FBR_InvoiceNo { get; set; }
        public string OrderTypeString { get; set; }
        public IEnumerable<InvoiceItemVM> InvoiceItems { get; set; }
        public IEnumerable<InvoicePaymentVM> InvoicePayments { get; set; }
        public List<InvoicePayment> ObjInvoicePayments { get; set; }
    }
}
