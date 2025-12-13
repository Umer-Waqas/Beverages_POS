using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Core.ViewModels
{
    public class StoreClosingVM
    {
        public int StoreClosingId { get; set; }
        public double OpeningCash { get; set; }
        public double TotalInflow { get; set; }
        public double TotalOutFlow { get; set; }
        public double SystemCash { get; set; }
        public double PhysicalCash { get; set; }
        public double CashDiff { get; set; }
        //public DateTime? ClosingDate { get; set; }
        //public DateTime? OpenDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public long CreatedBy { get; set; }
        //public long? StoreClosedBy { get; set; }
        //public long? StoreOpenedBy { get; set; }

        // cash submission attrs
        public double CashSubmittedToBank { get; set; }
        public double CashSubmittedToHO { get; set; }
        public double TotalCashSubmitted { get; set; }
        public double ClosingCash { get; set; }
    }
}