using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurant_MS_Core.Entities;
using Restaurant_MS_Core.ViewModels;



namespace Restaurant_MS_Core.Interfaces
{
    public interface IPurchaseOrder
    {
        List<PurchaseOrderVM> GetPurchaseOrdersWithDetailByDateRange(DateTime datefrom, DateTime dateTo);
        PurchaseOrderVM GetPODetailByOrderNo(decimal PurchaseOrderNo);
        PurchaseOrder GetPODetailByOrderNo_Origional(decimal PurchaseOrderNo);
        List<PurchaseOrderVM> GetPurchaseOrderSummaryByDateRange(DateTime datefrom, DateTime dateTo);
        PurchaseOrderVM GetPurchaseOrdersWithDetailByOrderNo(decimal PurchaseOrderNo);
        decimal GetNewPurchaseOrderNumber();
        void SetOrderDeleted(decimal PurchaseOrderNo);
    }
}