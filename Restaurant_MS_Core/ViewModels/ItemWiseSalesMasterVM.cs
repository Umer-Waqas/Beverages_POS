using Restaurant_MS_Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Core.ViewModels
{
    public class ItemWiseSalesMasterVM
    {
        public double? GrandSalesTotalQuantity { get; set; }
        public double GrandSaleRevenue { get; set; }
        public IPagedList<ItemWiseSaleVM> ItemWiseSales { get; set; }
    }
}