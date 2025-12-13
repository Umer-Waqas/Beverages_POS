using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Core.ViewModels
{
    public class ExpenseVM
    {
        public long ExpenseId { get; set; }
        public string ExpenseCategory { get; set; }
        public string description { get; set; }
        public double Amount { get; set; }
        public System.DateTime Date { get; set; }
        public string PaymentMode { get; set; }
        public bool AutoAdded { get; set; }
        public System.DateTime CreatedAt { get; set; }
    }
}
