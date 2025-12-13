using Restaurant_MS_Core.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Core.ViewModels
{
    public class StockVM
    {
        public long StockId { get; set; }
        public decimal DocumentNo { get; set; }
        public Supplier Supplier { get; set; }
        public string SupplierInvoiceNo { get; set; }
        public DateTime SupplierIvoiceDate { get; set; }
        public string ImagePath { get; set; }
        public ICollection<StockItemVM> StockItems { get; set; }
        public bool IsAutoInsertedStock { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        public bool IsNew { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsSynced { get; set; }
        public ICollection<StockPaymentVM> StockPayments { get; set; }
        public string SupplierName { get; set; }
        public string UserName { get; set; }

        public double TotalAmount { get; set; }
        public double TotalAmount_RetailPrice { get; set; }
        public double? TotalPaid { get; set; }
    }
}
