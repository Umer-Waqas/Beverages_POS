
using Restaurant_MS_Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Core.ViewModels
{
    public class ItemWisePurchaseMasterVM
    {
        public long? GrandPurchaseTotalQuantity { get; set; }
        public IPagedList<ItemWisePurchaseVM> ItemWisePurchases { get; set; }
    }
}
