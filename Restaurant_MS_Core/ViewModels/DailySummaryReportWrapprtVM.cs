using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Core.ViewModels
{
    public class DailySummaryReportWrapprtVM
    {
        public List<DailySummaryRptVM> CetagoryWiseSummary { get; set; }
        public List<DailySummaryReport_ExpenseVM> Expenses { get; set; }
        public List<DailySummaryReport_AdjustmentVM> Adjustments { get; set; }
        public List<StoreClosingVM> StoreClosingSummary { get; set; }

        public double LocalPurchase { get; set; }
        public double HOPurchase { get; set; }
        public double TotalDiscount { get; set; }
        public double OpeningBalance { get; set; }

        public double ClosingBalance { get; set; }
    }
}
