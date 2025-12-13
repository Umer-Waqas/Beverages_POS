using Restaurant_MS_Core.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Core.ViewModels
{
    public class InvoiceItemVM
    {
        public long InvoiceItemId { get; set; }
        public long ItemId { get; set; }
        public string ItemName { get; set; }
        public long BatchId { get; set; } // long was nullable()long, but removed as nullable
        public string BatchName { get; set; }
        public string BatchExpiry { get; set; }
        public Item Item { get; set; }
        public Batch Batch { get; set; }
        public string Rack { get; set; }
        public double Rate { get; set; }
        public double Quantity { get; set; }
        public double BonusQuantity { get; set; }
        public double Amount { get; set; }
        public double NetAmount { get; set; }
        public double Discount { get; set; }
        public string DiscountString { get; set; }
        public int DiscountType { get; set; }
        public string DiscountTypeString { get; set; }
        public double SalesTax { get; set; }
        public string SalesTaxString { get; set; }
        public int SalesTaxType { get; set; }
        public string SalesTaxTypeString { get; set; }
        public double ReturnedQuantity { get; set; }
        public bool IsActive { get; set; }
        public string ProcedureDescription { get; set; }
        public long ProcedureId { get; set; }
        public string ProcedureName { get; set; }
        public double Revenue { get; set; }
        public double UnitCost { get; set; }
        public bool AffectStock { get; set; }

        public int Unit { get; set; }
        public string UnitString { get; set; }
        public bool IsOptionalBatchNo { get; set; }
    }
}