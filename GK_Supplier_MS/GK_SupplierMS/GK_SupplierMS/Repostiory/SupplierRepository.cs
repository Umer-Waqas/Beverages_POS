using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GK.Shared.Core.Entities;
using System.Data.Entity;
using PagedList;
using System.Data.SqlClient;
using GK.Shared.Core.ViewModels;
using GK.Shared.Core.EntityModel;
using GK_SupplierMS.Repostiory;

namespace GK_SupplierMS.Repostiory
{
    public class SupplierRepository : Repository<Supplier>
    {
        GKPOS_Entities cxt = null;
        public SupplierRepository(GKPOS_Entities cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }

        public IPagedList<Supplier> GetActiveSuppliers(int PageNo, int PageSize)
        {
            return cxt.Suppliers
                .Where(s => s.IsActive == true)
                .OrderByDescending(s => s.SupplierID)
                .ToPagedList(PageNo, PageSize);
        }
        public IPagedList<Supplier> GetActiveSuppliers(long SupplierId)
        {
            return cxt.Suppliers
                .Where(s => s.IsActive && s.SupplierID == SupplierId)
                .OrderByDescending(s => s.SupplierID)
                .ToPagedList(1, 1);
        }

        public async Task<List<SelectListVM>> GetSelectList()
        {
            return await cxt.Suppliers
                .Where(s => s.IsActive == true)
                .OrderByDescending(s => s.SupplierID)
                .Select(s => new SelectListVM
                {
                    Value = s.SupplierID,
                    Text = s.Name
                }).ToListAsync();
        }
        public List<Supplier> GetActiveSuppliers()
        {
            return cxt.Suppliers
                .Where(s => s.IsActive == true)
                .OrderByDescending(s => s.SupplierID).ToList();
        }

        public SupplierVM GetSupplierWithItems(long SupplierId, int PageNo, int PageSize)
        {
            //return cxt.Database.SqlQuery<Item>("select i.ItemName, i.IsNarcotic FROM Items i inner join SupplierItem si on i.ItemId = si.Item_ItemIs WHERE si.Supplier_SupplierId = @SupplierId ", new SqlParameter("@SupplierId", SupplierId)).OrderByDescending(i => i.ItemId).ToPagedList(PageNo, PageSize);            
            return cxt.Suppliers
                .Where(s => s.SupplierID == SupplierId)
                .Select(s => new SupplierVM()
                {
                    SupplierID = s.SupplierID,
                    IsHOSupplier = s.IsHoSupplier,
                    Name = s.Name,
                    Phone = s.Phone,
                    Address = s.Address,
                    PrimaryContactPersonName = s.PrimaryContactPersonName,
                    PrimaryContactPersonPhone = s.PrimaryContactPersonPhone,
                    OpeningBalance = s.OpeningBalance,
                    Items = s.Items.Where(i => i.IsActive).ToList()
                }).FirstOrDefault();
        }

        public SupplierDetailsVM GetSupplierWithDataDetails(long SupplierID)
        {
            //return cxt.Suppliers
            //    .Where(s => s.SupplierID == SupplierID)
            //    .Where(s => s.IsActive)
            //    .Include(s => s.Items)
            //    .Include(s => s.Stocks.Select(si => si.StockItems.Select(item => item.Item)))
            //    .FirstOrDefault();

            return cxt.Suppliers
                 .Where(s => s.SupplierID == SupplierID && s.IsActive)
                 .Select(d => new SupplierDetailsVM
                 {
                     Supplier = d,
                     Items = d.Items.ToList(),
                     Stocks = (d.Stocks.Where(s => s.GrandTotal > 0).Select(st => new StockVM
                     {
                         IsActive = st.IsActive,
                         StockId = st.StockId,
                         DocumentNo = st.DocumentNo,
                         SupplierInvoiceNo = st.SupplierInvoiceNo,
                         SupplierIvoiceDate = st.SupplierIvoiceDate,
                         ImagePath = st.ImagePath,
                         CreatedAt = st.CreatedAt,
                         UpdatedAt = st.UpdatedAt,
                         TotalAmount = st.GrandTotal,// (double?)(st.StockItems.Where(si => si.IsActive).Sum(si => si.TotalCost)) ?? 0,
                         TotalPaid = (double?)(st.Expenses.Sum(ip => ip.Amount)) ?? 0,
                         StockItems = (st.StockItems.Where(si => si.IsActive).Select(si => new StockItemVM
                         {
                             Item = si.Item,
                             Quantity = si.Quantity,
                             TotalCost = si.TotalCost,
                         })).ToList(),
                         StockPayments = (st.Expenses.Select(sp => new StockPaymentVM // can be added where caluse in future to fetch only active payments
                         {
                             Payment = sp.Amount ?? 0,
                             PaymentType = sp.PaymentMode,
                             CreatedAt = sp.Date
                         })).ToList()
                     })).ToList()
                 }).FirstOrDefault();
        }
        public IPagedList<StockVM> GetBillPayments_Report(int PageNo, int PageSize)
        {
            return cxt.Stocks
                .Where(s => s.Supplier != null)
                .Select(st => new StockVM()
                {
                    Supplier = st.Supplier,
                    IsActive = st.IsActive,
                    StockId = st.StockId,
                    DocumentNo = st.DocumentNo,
                    SupplierInvoiceNo = st.SupplierInvoiceNo,
                    SupplierIvoiceDate = st.SupplierIvoiceDate,
                    ImagePath = st.ImagePath,
                    CreatedAt = st.CreatedAt,
                    UpdatedAt = st.UpdatedAt,
                    TotalAmount = st.GrandTotal,// (st.StockItems.Where(si => si.IsActive).Sum(si => si.TotalCost)),
                    TotalPaid = (st.Expenses.Sum(ip => (double?)ip.Amount) ?? 0)
                    //StockPayments = (st.InvoicePayments.Select(sp => new StockPaymentVM // can be added where caluse in future to fetch only active payments
                    //{
                    //    Payment = sp.Payment,
                    //    CreatedAt = sp.Date
                    //})).ToList()
                }).OrderBy(r => r.StockId).ToPagedList(1, 1);
        }

        public SupplierStockVM GetSupplierStockAndPayments(long SupplierId)
        {
            return cxt.Suppliers
                .Where(s => s.SupplierID == SupplierId)
                .Select(s => new SupplierStockVM
                {
                    Supplier = s,
                    StockPayments = (s.Stocks.Select(stp => new StockBillVM
                    {
                        StockId = stp.StockId,
                        DocumentNo = stp.DocumentNo,
                        SupplierInvoiceNo = stp.SupplierInvoiceNo,
                        SupplierInvoiceDate = stp.SupplierIvoiceDate,
                        TotalAmount = stp.GrandTotal,// (double?)(stp.StockItems.Where(si => si.IsActive).Sum(si => si.TotalCost)) ?? 0,
                        TotalPaid = (double?)(stp.Expenses.Sum(ip => ip.Amount)) ?? 0
                    })).ToList()
                }).FirstOrDefault();
            //return cxt.Stocks
            //    .Where(s => s.Supplier.SupplierID == SupplierId)
            //    .Select(s => new SupplierStockVM
            //    {
            //        Supplier = s.Supplier,
            //        StockPayments = (new StockPaymentsVM(){
            //            StockId = s.StockId,
            //            DocumentNo = s.DocumentNo,
            //            SupplierInvoiceNo = s.SupplierInvoiceNo,
            //            SupplierInvoiceDate = s.SupplierIvoiceDate,
            //            TotalAmount = (s.StockItems.Where(si => si.IsActive).Sum(si => si.TotalCost)),
            //            TotalPaid = (s.InvoicePayments.Sum(ip => ip.Payment))
            //        })

            //    }).ToList();
        }

        public Supplier GetSupplierByName(string SupplierName)
        {
            return cxt.Suppliers.Where(sp => sp.Name.ToLower().Equals(SupplierName)).FirstOrDefault();
        }

        public bool SuppliersExist()
        {
            return cxt.Suppliers.Any();
        }

        public SupplierItemsVM getSupplierItems(long SupplierID)
        {
            //return cxt.Suppliers
            //    .Where(s => s.SupplierID == SupplierID)
            //    .Where(s => s.IsActive)
            //    .Include(s => s.Items)
            //    .Include(s => s.Stocks.Select(si => si.StockItems.Select(item => item.Item)))
            //    .FirstOrDefault();
            return cxt.Suppliers
             .Where(s => s.SupplierID == SupplierID)
             .Select(d => new SupplierItemsVM
             {
                 SupplierId = d.SupplierID,
                 Items = d.Items.Where(i => i.IsActive).Select(ci => new ItemsComboVM
                 {
                     ItemId = ci.ItemId,
                     //ItemName = ci.ItemName
                 }).ToList()
             }).FirstOrDefault();
        }

        public List<SuppliersComboVM> getSuppliersForCombo()
        {
            return cxt.Suppliers.Where(s => s.IsActive)
                .Select(s => new SuppliersComboVM
                {
                    SupplierId = s.SupplierID,
                    Name = s.Name
                }).ToList();
        }
    }
}