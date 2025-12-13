using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Core.ViewModels
{
    public class DeadStockVM
    {
        public long ItemId { get; set; }
        public string ItemName { get; set; }
        public double TotalStock { get; set; }
        public double AvailableStock { get; set; }
        public double ConsumedStock { get; set; }
        public double AdjustedStock { get; set; }
        public double ExpiredStock { get; set; }
        public double AvailableQuantity { get; set; }
        public string LastSalesAt { get; set; }
    }
}