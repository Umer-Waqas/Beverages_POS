using Restaurant_MS_Core.Entities;


using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Core.ViewModels
{
    public class StockConsumptionItemVM
    {
        public long StockConsumptionItemId { get; set; }
        public Item Item { get; set; }        
        public Batch Batch { get; set; }
        public double Quantity { get; set; }
        public int ConsumptionType { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        public bool IsNew { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsSynced { get; set; }
        public double TotalCost { get; set; }
        public User User { get; set; }

        public long InvoiceId { get; set; }
        public long ItemId { get; set; }        
        public string ItemName { get; set; }        
        public int BatchId { get; set; }        
        public string BatchName { get; set; }        
        public string ConsumptionTypeString { get; set; }
        public string UnitString { get; set; }
        public int ConversionUnit { get; set; }
    }
}
