using Restaurant_MS_Core.Entities;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Core.ViewModels
{
    public class ItemsVM
    {
        public long ItemId { get; set; }
        public string ItemName { get; set; }      
        public string Barcode { get; set; }
        public string Suppliers { get; set; }
        public Manufacturer Manufacturer { get; set; }
        public List<Supplier> SuppliersList { get; set; }
        public int ReorderingLevel { get; set; }
        public double RetailPrice { get; set; }
        public double UnitCost { get; set; }

        public double ItemQuantity { get; set; }
        public double TotalStock { get; set; }
        public double AvailableStock { get; set; }
        public double ConsumedStock { get; set; }
        public double AdjustedStock { get; set; }
        public double ExpiredStock { get; set; }
        public double ExpiredConsumedStock { get; set; }
        public double HoldStock { get; set; }
        public int UnitValue { get; set; }
        public string Unit { get; set; }
        public int ConversionUnit { get; set; }
        public bool IsNarcotic { get; set; }
        public string ItemType { get; set; }
        public string RackNo { get; set; }
    }
}