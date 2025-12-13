using Restaurant_MS_Core.Entities;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Core.ViewModels
{
    public class StockAuditVM
    {
        public double TotalDifference { get; set; }
        public long StockAuditId { get; set; }
        public string Note { get; set; }

        public DateTime StockAuditDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ICollection<StockAuditDetail> StockAuditDetail { get; set; }
        public ICollection<StockAuditDetailVM> StockAuditDetailVM { get; set; }
        public User User { get; set; }
        public bool IsNew { get; set; }
        public bool IsActive { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsSynced { get; set; }
    }
}
