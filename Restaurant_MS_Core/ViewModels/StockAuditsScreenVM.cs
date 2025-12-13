using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Core.ViewModels
{
    public class StockAuditsScreenVM
    {
        public long StockAuditId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime AuditDate { get; set; }
        public double TotalDifference { get; set; }     
    }
}
