
using Restaurant_MS_Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Restaurant_MS_Core.ViewModels
{
    public class ItemDetailVM
    {

        public Item Item { get; set; }
        public double AvailableStock { get; set; }
        public double RetailPrice { get; set; }
        public ICollection<BatchStockVM> BatchStockList { get; set; }
    }
}