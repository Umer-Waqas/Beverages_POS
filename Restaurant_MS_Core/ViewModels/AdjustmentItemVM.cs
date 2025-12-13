using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Core.ViewModels
{
    public class AdjustmentItemVM
    {
        public long AdjustmentItemId { get; set; }
        public double Quantity { get; set; }
        public string Reason { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsNew { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsSynced { get; set; }
               
        public string ItemName { get; set; }        
        public string BatchName { get; set; }
    }
}
