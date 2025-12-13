using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Core.ViewModels
{
   public class StoreOpenRespVM
    {
        public bool? OpenSuccess { get; set; }
        public bool? IsPreviousDayClosed { get; set; }
        public bool? IsOpenTimeReached { get; set; }
    }
}
