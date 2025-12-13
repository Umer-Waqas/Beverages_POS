
using Restaurant_MS_Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class getUsersVM
    {
        public int userId { get; set; }
        public string userName { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public UserRole UserRole { get; set; }
    }
}
