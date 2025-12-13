using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Core.ViewModels
{
    public class StockValueRptVM
    {
        public string ItemName { get; set; }
        public double AvailableQuantity { get; set; }
        public double CostPrice { get; set; }
        public double RetailPrice { get; set; }
        public double TCostValue { get; set; }
        public double TRetailValue { get; set; }
        public string Unit { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
    }
}