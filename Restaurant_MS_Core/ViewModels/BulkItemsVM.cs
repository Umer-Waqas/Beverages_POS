using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Core.ViewModels
{
    public class BulkItemsVM
    {
        public long ItemId { get; set; }
        public string ItemName { get; set; }
        /// <summary>
        /// in db its saved as ChecmicalNAme
        /// </summary>

        public string Unit { get; set; }
        public string GenericName { get; set; }
        public string Manufacturer { get; set; }
        public string Barcode { get; set; }
        public double RetailPrice { get; set; }
        public double CostPrice { get; set; }
    }
}
