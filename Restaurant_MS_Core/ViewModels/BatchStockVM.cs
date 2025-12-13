using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Core.ViewModels
{
    public class BatchStockVM
    {
        public long? BatchId { get; set; }
        public string BatchName { get; set; }
        public double TotalStock { get; set; }
        public double AvailableStock { get; set; }
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
        public DateTime? Expiry { get; set; }
        public DateTime CreatedAt { get; set; }
        public double RetailPrice { get; set; }
        public double CostPrice { get; set; }
    }
}