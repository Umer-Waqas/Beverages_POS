using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Core.ViewModels
{
    public class PatientSearchVM
    {
        public long PatientId { get; set; }
        public string PatientName { get; set; }
        public string Phone { get; set; }
        public Decimal MrNo { get; set; }
    }
}
