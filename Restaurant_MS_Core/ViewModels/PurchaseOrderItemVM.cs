using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Core.ViewModels
{
    public class PurchaseOrderItemVM
    {
        public long PurchaseOrderItemId { get; set; }
        public decimal PurchaseOrderNo { get; set; }
        public long ItemId { get; set; }       
        public string ItemName { get; set; }
        public double Quantity { get; set; }
        public double UnitCost { get; set; }
        public double TotalAmount { get; set; }
        public long SupplierId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}