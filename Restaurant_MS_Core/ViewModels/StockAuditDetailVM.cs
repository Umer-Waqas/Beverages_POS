using Restaurant_MS_Core.Entities;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Core.ViewModels
{
    public class StockAuditDetailVM
    {
        public int StockAuditDetailId { get; set; }
        public long ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemUnit { get; set; }
        public int AuditUnit { get; set; }
        public int ConversionUnit { get; set; }


        // available stock attributes.....
        public double TotalStock { get; set; }
        public double AvailableStock { get; set; }
        public double ConsumedStock { get; set; }
        public double AdjustedStock { get; set; }
        public double ExpiredStock { get; set; }
        public double ExpiredConsumedStock { get; set; }
        public double HoldStock { get; set; }


        public int SystemQuantity { get; set; }
        public int PhysicalQuantity { get; set; }
        public int Differnce { get; set; }
        public int CurrentAdjustedQuantity { get; set; }
        public int RetailPrice { get; set; }
        public double AmountDifference { get; set; }
        public StockAudit StockAudit { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsNew { get; set; }
        public bool IsActive { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsSynced { get; set; }
    }
}
