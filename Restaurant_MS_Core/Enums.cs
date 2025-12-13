using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Core
{
    public static class Enums
    {
        public enum DiscountType { Value = 0, Percent = 1 }
        public enum SalesTaxType { Value = 0, Percent = 1 }
        public enum ItemAdjustmentType { Adjustment = 0, Expired = 1, Damage = 2, Exceed = 3 }
        public enum AdjustmentType { Adjustment = 1, HOReturn = 2 }
        public enum PendingOrdersViewType { All = 1, Pending = 2, Completed = 3 };
        public enum ConsumptionType { Services = 1, Sales = 2, Damages = 3, Returned = 4 };
        public enum Gender { Male = 1, Female = 2, Other = 3 }
        // public enum AdjustmentItemType { HOReturn = 1 };
    }
}