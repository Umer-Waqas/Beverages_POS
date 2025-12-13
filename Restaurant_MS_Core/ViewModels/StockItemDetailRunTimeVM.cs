using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Core.ViewModels
{
    public class StockItemDetailRunTimeVM
    {
        public double ItemId { get; set; }
        public string ItemName { get; set; }

        public string Unit { get; set; }
        public double CostPrice { get; set; }
        public double RetailPrice { get; set; }
        public int ConversionUnit { get; set; }
        public double TotalStock  { get; set; } 
        public double ConsumedStock  { get; set; } 
        public double ExpiredStock { get; set; }
        public double AdjustedStock  { get; set; }                                           
        public double AvailableQty { get; set; }            
    }
}