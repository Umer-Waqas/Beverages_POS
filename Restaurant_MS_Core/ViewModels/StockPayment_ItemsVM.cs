using Restaurant_MS_Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Core.ViewModels
{
    public class StockPayment_ItemsVM
    {
        public string ItemName { get; set; }
        public int Unit { get; set; } // int value of Unit 0 or 1
        public string UnitString { get; set; } // string value of unit 0:specified with item, 1 : units
        public int ConversionUnit { get; set; }
        public double Quantity { get; set; }
        public double BonusQuantity { get; set; }
        public double UnitCost { get; set; }
        public double TotalCost { get; set; }
        public double RetailPrice { get; set; }
        public double BonusDiscount { get; set; }
        public double Discount { get; set; }
        public int DiscountType { get; set; }
        public int PercDiscType { get; set; }
        public double SalesTax { get; set; }
        public int SalesTaxType { get; set; }
        public int PercSalesTaxType { get; set; }
        public double NetValue { get; set; }
        public double DiscountVal { get; set; }
        public double SalesTaxVal { get; set; }
    }
}
