using Restaurant_MS_Core.Entities;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Core.ViewModels
{
    public class ItemsEditVM
    {
        public long ItemId { get; set; }
        public string ItemName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public Rack Rack { get; set; }
        public string Barcode { get; set; }        
        public int ReorderingLevel { get; set; }
        public double RetailPrice { get; set; }
        public double UnitCost { get; set; }
        public List<SupplierInfoVM> Suppliers { get; set; }
    }
}