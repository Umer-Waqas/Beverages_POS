using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Core.Entities
{

    public partial class Item
    {
        [NotMapped]
        public string RackNo { get; set; }
        [NotMapped]

        public string SupplierNames { get; set; }

        [NotMapped]

        public string SupplierIds { get; set; }
        [NotMapped]


        public string ManufacturerName { get; set; }
        [NotMapped]

        public string CategoryName { get; set; }
    }


    public partial class InvoiceItem
    {
        [NotMapped]
        public string ItemName { get; set; }
        [NotMapped]
        public string BatchName { get; set; }
        [NotMapped]
        public string UnitString { get; set; }
        [NotMapped]
        public int ConvUnit { get; set; }
        [NotMapped]
        public string DiscountTypeString { get; set; }
    }


    public partial class StockConsumptionItem
    {
        [NotMapped]
        public string ItemName { get; set; }
        [NotMapped]
        public string BatchName { get; set; }
        [NotMapped]
        public string UnitString { get; set; }
        [NotMapped]
        public int ConversionUnit { get; set; }
        [NotMapped]
        public string ConsumptionTypeString { get; set; }
    }


    public partial class AdjustmentItem
    {
        [NotMapped]
        public string ItemName { get; set; }
        [NotMapped]
        public string BatchName { get; set; }
        [NotMapped]
        public string UnitString { get; set; }
        [NotMapped]
        public int ConversionUnit { get; set; }
    }
    public partial class StockItem
    {
        [NotMapped]
        public string ItemName { get; set; }
        
        [NotMapped]
        public string BatchName { get; set; }
        [NotMapped]
        public DateTime? BatchExpiry { get; set; }
        [NotMapped]
        public string UnitString { get; set; }
        [NotMapped]
        public int ConversionUnit { get; set; }
        [NotMapped]
        public string DiscoutnTypeString { get; set; }
        [NotMapped]
        public string SalesTaxTypeString { get; set; }
    }



    public partial class PurchaseOrder
    {
        [NotMapped]
        public string SupplierName { get; set; }
    }




    public partial class PurchaseOrderItem
    {
        [NotMapped]
        public int ConversionUnit { get; set; }
        [NotMapped]
        public string ItemName { get; set; }
        [NotMapped]
        public string UnitString { get; set; }
    }
}
