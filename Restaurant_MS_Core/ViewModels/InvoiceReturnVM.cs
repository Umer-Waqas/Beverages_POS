using Restaurant_MS_Core.Entities;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Core.ViewModels
{
    public class InvoiceReturnVM
    {
        public long InvoiceId { get; set; }
        public long InvoiceRefNo { get; set; }
        public Patient Patient { get; set; }
        public double Total { get; set; }
        public double Paid { get; set; }
        public string DepartmentRevenue { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsProcedureInvoice { get; set; }
        public string UserName { get; set; }
    }
}
