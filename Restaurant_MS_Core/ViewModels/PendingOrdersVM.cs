using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Core.ViewModels
{
    public class PendingOrdersVM
    {
        public long InvoiceId { get; set; }
        public long CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerAddress { get; set; }
        public double TotalAmount { get; set; }
        public double DiscountAmount { get; set; }
        public double GrandTotal { get; set; }
        public int OrderType { get; set; }
        public string OrderTypeString { get; set; }
        public int PaymentStatus { get; set; }
        public string PaymentStatusString { get; set; }
        public int OrderStatus { get; set; }
        public string OrderStatusString { get; set; }
        public int PaymentMethod { get; set; }
        public string PaymentMethodString { get; set; }
        public DateTime OrderStartTime { get; set; }
    }
}