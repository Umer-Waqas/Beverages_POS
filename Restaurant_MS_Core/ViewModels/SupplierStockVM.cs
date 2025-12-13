using Restaurant_MS_Core.Entities;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Core.ViewModels
{
    public class SupplierStockVM
    {
        public Supplier Supplier { get; set; }
        public List<StockBillVM> StockPayments { get; set; }
    }
}
