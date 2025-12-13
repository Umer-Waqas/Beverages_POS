using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Core.ViewModels
{
    public class DailySummaryRptVM
    {
        public long CategoryId { get; set; }
        public string CategoryName { get; set; }
        public double TotalPurchase { get; set; }
        public double TotalSale { get; set; }
        public double TotalHoReturn { get; set; }
        public double TotalAdjustment { get; set; }

    }
}
