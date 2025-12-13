using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class InvoiceWholeSaleVM
    {
        public double Quantity { get; set; }
        public double BonusQuantity { get; set; }
        public string ItemName { get; set; }
        public string Pack { get; set; }
        public string Batch { get; set; }
        public double Rate { get; set; }
        public double Disc { get; set; }
        public double SalesTax { get; set; }
        public double NetValue { get; set; }
    }
}
