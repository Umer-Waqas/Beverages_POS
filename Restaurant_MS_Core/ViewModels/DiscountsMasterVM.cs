using Restaurant_MS_Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Core.ViewModels
{
    public class DiscountsMasterVM
    {
        public double TotalDiscount { get; set; }
        public IPagedList<DiscountVM> Discounts { get; set; }
    }
}
