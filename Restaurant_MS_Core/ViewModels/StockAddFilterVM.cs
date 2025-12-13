using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Core.ViewModels
{
    public class StockAddFilterVM
    {
        public long ItemId { get; set; }
        public string ItemName { get; set; }
        public string Unit { get; set; }
        public int ConversionUnit { get; set; }
        public double UnitCost { get; set; }
        public double RetailPrice { get; set; }
        public double AvailableStock { get; set; }
        public double TotalStock { get; set; }
        public double ConsumedStock { get; set; }
        public double AdjustedStock { get; set; }
        /// <summary>
        /// sum of total stock that has been expired
        /// </summary>
        public double ExpiredStock { get; set; }
        /// <summary>
        /// sum of consumed stock whom batches expired now
        /// </summary>
        public double ExpiredConsStock { get; set; }
        /// <summary>
        /// this contains stock consumption quantity for on hold invoices
        /// </summary>
        public double HoldStock { get; set; }
        public double Quantity { get; set; }
    }
}