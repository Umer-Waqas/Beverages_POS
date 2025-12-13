using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class SupplierPaymentHistoryVM
    {
        public long ExpenseId { get; set; }
        public long StockId { get; set; }
        public decimal DocumentNo { get; set; }
        public DateTime PaymentDate { get; set; }
        public string Description { get; set; }
        public int PaymentMode { get; set; }
        public double PaidAmount { get; set; }
        public string SupplierName { get; set; }
    }
}
