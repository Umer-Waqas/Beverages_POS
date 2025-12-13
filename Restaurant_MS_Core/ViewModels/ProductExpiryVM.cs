using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Core.ViewModels
{
    public class ProductExpiryVM
    {
        public long ItemId { get; set; }
        public string ItemName { get; set; }
        public string BatchNo { get; set; }
        public string LastTrDate { get; set; }
        public string BatchExp { get; set; }
        public double Quantity { get; set; }
        public double TotalStock { get; set; }
        public double ConsumedStock { get; set; }
        public double ExpiredStock { get; set; }
        public double HoldStock { get; set; }
        public double AdjustedStock { get; set; }
    }
}
