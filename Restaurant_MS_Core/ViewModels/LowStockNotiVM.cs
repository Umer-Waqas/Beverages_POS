using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Core.ViewModels
{
    public class LowStockNotiVM
    {
        public long ItemId { get; set; }
        public string ItemName { get; set; }
        public int ReorderingLevel { get; set; }
        public double TotalStock { get; set; }
        public double ConsumedStock { get; set; }
        public double AdjustedStock { get; set; }
        public double ExpiredStock { get; set; }
        public double ExpiredConsumedStock { get; set; }
        public double HoldStock { get; set; }

        /// <summary>
        /// AvailableStock = TotalStock + adjustedStock - consumedStock - expiredStock- ExpiredConsumedStock - (HoldStock) : hold stock is conditional
        /// </summary>
        public double AvailableStock { get; set; }
        public string Unit { get; set; }
        public int ConversionUnit { get; set; }
    }
}