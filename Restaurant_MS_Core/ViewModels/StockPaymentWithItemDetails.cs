using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Core.ViewModels
{
    public class StockPaymentWithItemDetails
    {
        public StockPaymentWithItemDetails()
        {
            Items = new List<StockPayment_ItemsVM>();
        }

        public decimal DocumentNo { get; set; }
        public DateTime? SupplierIvoiceDate { get; set; }
        public DateTime? StockDate { get; set; }
        public string SupplierInvoiceNo { get; set; }
        public double GrandTotal { get; set; }
        public double TotalPaid { get; set; }
        public string SupplierName { get; set; }
        public string UserName { get; set; }
        public List<StockPayment_ItemsVM> Items { get; set; }
    }
}