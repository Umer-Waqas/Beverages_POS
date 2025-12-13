using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Core.ViewModels
{
    public class ItemWiseSaleVM
    {
        public long ItemId { get; set; }
        public long InvoiceId { get; set; }
        public string ItemName { get; set; }
        public double SaleCount { get; set; }
        public double ReturnedQuantity { get; set; }
        public double SaleRevenue { get; set; }
        public long GrandSalesTotal { get; set; }


        // calculating available stock variables
        public double TotalStock { get; set; }
        public double AvailableStock { get; set; }
        public double ConsumedStock { get; set; }
        public double AdjustedStock { get; set; }
        public double ExpiredStock { get; set; }
        public double HoldStock { get; set; }
    }
}