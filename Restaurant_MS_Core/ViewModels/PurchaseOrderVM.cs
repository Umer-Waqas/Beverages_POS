using Restaurant_MS_Core.Entities;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Core.ViewModels
{
    public class PurchaseOrderVM
    {
        public string SupplierName { get; set; }
        public long PurchaseOrderId { get; set; }
        public decimal? PurchaseOrderNo { get; set; }
        public long SupplierId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal TotalAmount { get; set; }
        public long StockId { get; set; }
        public IEnumerable<PurchaseOrderItemVM> PurchaseOrderItems { get; set; }
        public Supplier Supplier { get; set; }
    }
}