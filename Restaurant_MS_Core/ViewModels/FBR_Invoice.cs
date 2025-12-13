using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Core.ViewModels
{
    public class FBR_Invoice
    {
        public FBR_Invoice()
        {
            Items = new List<FBR_Invoice_Items>();
        }
        public string InvoiceNumber { get; set; }
        public string POSID { get; set; }
        public string USIN { get; set; }
        public string RefUSIN { get; set; }
        public DateTime DateTime { get; set; }
        public string BuyerName { get; set; }
        public string BuyerNTN { get; set; }
        public string BuyerCNIC { get; set; }
        public string BuyerPhoneNumber { get; set; }
        public double TotalSaleValue { get; set; }
        public double TotalTaxCharged { get; set; }
        public double TotalQuantity { get; set; }
        public double Discount { get; set; }
        public double FurtherTax { get; set; }
        public double TotalBillAmount { get; set; }
        public int PaymentMode { get; set; }
        public int InvoiceType { get; set; }
        public List<FBR_Invoice_Items> Items { get; set; }
    }

    public class FBR_Invoice_Items
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string PCTCode { get; set; }
        public double Quantity { get; set; }
        public float TaxRate { get; set; }
        public double SaleValue { get; set; }
        public double Discount { get; set; }
        public double FurtherTax { get; set; }
        public double TaxCharged { get; set; }
        public double TotalAmount { get; set; }
        public int InvoiceType { get; set; }
        public string RefUSIN { get; set; }
    }
}