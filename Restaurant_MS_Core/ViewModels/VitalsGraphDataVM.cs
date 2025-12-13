using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Core.ViewModels
{
    public class VitalsGraphDataVM
    {
        public DateTime Date { get; set; }
        public double Temprarture { get; set; }
        public double Weight { get; set; }
        public double BloodPressure { get; set; }
        public double BloodSugar { get; set; }
        public int Test { get; set; }
        public double RespiratoryRate { get; set; }
        public double OxygenSaturation { get; set; }
    }
}
