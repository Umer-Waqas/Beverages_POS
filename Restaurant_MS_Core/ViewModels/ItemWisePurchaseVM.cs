using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Core.ViewModels
{
    public class ItemWisePurchaseVM
    {
        public long ItemId { get; set; }
        public long StockId { get; set; }
        public string ItemName { get; set; }
        public double PurchaseCount { get; set; }
        public double Quantity { get; set; }        
        public long GrandPurchaseTotal { get; set; }



        // calculating available stock variables
        public double TotalStock { get; set; }
        public double AvailableStock { get; set; }
        public double ConsumedStock { get; set; }
        public double AdjustedStock { get; set; }
        public double ExpiredStock { get; set; }
        public double HoldStock { get; set; }
    }
}
