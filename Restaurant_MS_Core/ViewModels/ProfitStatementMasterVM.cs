using Restaurant_MS_Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Core.ViewModels
{
    public class ProfitStatementMasterVM
    {
        public double TotalSales { get; set; }
        public double NetTotalSales { get; set; }
        public double TotalCostOfSales { get; set; }
        public double TotalDiscount { get; set; }
        public double TotalRevenue { get; set; }
        public double Profit { get; set; }
        public IPagedList<ProfitStatementVM> ProfitStatementPaged { get; set; }
    }
}