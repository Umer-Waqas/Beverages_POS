using Restaurant_MS_Core.Entities;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Core.ViewModels
{
    public class StockBillVM
    {      
        public long StockId { get; set; }
        public decimal DocumentNo { get; set; }
        public string SupplierInvoiceNo{ get; set; }
        public DateTime SupplierInvoiceDate { get; set; }
        public double TotalAmount { get; set; }
        public double? TotalPaid { get; set; }
        public ICollection<InvoicePayment> InvoicePayments { get; set; }
        public double Due { get; set; }
    }
}
