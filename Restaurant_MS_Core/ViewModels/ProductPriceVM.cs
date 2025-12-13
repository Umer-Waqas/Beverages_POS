using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Core.ViewModels
{
    public class ProductPriceVM
    {
        public long ItemId { get; set; }
        public string ItemName { get; set; }
        public string Category { get; set; }
        public double RetailPrice { get; set; }
        public double CostPrice { get; set; }
    }
}
