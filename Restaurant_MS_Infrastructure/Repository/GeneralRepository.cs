
namespace Restaurant_MS_Infrastructure.Repository
{
    public class GeneralRepository
    {
        private AppDbContext cxt = null;
        public GeneralRepository(AppDbContext cxt)
        {
            this.cxt = cxt;
        }

        public IncomeStatementVM GetIncomeStatement(DateTime DateFrom, DateTime DateTo)
        {
            IncomeStatementVM Result = new IncomeStatementVM
            {
                TotalStockPayments = cxt.Stocks
                                      .Where(s => s.IsActive
                                      && s.CreatedAt.Date >= DateFrom.Date
                                      && s.CreatedAt.Date <= DateTo.Date
                                      )
                                     .Sum(inv => (double?)inv.Expenses.Where(p => p.IsActive)
                                                        .Sum(p => (double?)p.Amount ?? 0)) ?? 0,

                TotalRevenue = cxt.Invoices
                                               .Where(i => i.IsActive
                                                   && (i.CreatedAt.Date) >= (DateFrom.Date) &&
                                                   (i.CreatedAt.Date) <= (DateTo.Date))
                                                    .Sum(inv => (double?)inv.InvoicePayments.Where(p => p.IsActive)
                                                        .Sum(p => (double?)p.Payment ?? 0)) ?? 0
            };
            return Result;
        }
        public TodaysReportVM GetTodyasReportForDashboard()
        {
            return new TodaysReportVM
            {

                SalesTotal = cxt.Invoices.Where(i => i.IsActive && (i.CreatedAt) == (DateTime.Now.Date))
                            .Sum(i => (double?)i.GrandTotal) ?? 0,
                PurchaseOrdersTotal = cxt.PurchaseOrders.Where(po => po.IsActive && (po.CreatedAt.Date) == (DateTime.Now.Date))
                                      .Sum(po => (double?)po.TotalAmount) ?? 0,
                CashReceivedTotal = cxt.Invoices.Where(i => i.IsActive && (i.CreatedAt.Date) == (DateTime.Now.Date))
                                    .Sum(i => i.InvoicePayments.Where(ip => ip.IsActive
                                                                    && (ip.PaymentDate) == (DateTime.Now)
                                                                    && ip.PaymentType == 1
                                                                    ).Sum(ip => (double?)ip.Payment)) ?? 0,

                ChecquePaymentsTotal = cxt.Invoices.Where(i => i.IsActive && (i.CreatedAt.Date.Date) == (DateTime.Now.Date))
                                    .Sum(i => i.InvoicePayments.Where(ip => ip.IsActive
                                                                    && (ip.PaymentDate.Date) == (DateTime.Now.Date)
                                                                    && ip.PaymentType == 2
                                                                    ).Sum(ip => (double?)ip.Payment)) ?? 0,

                DebitCreditTotal = cxt.Invoices.Where(i => i.IsActive && (i.CreatedAt.Date) == (DateTime.Now.Date))
                                    .Sum(i => i.InvoicePayments.Where(ip => ip.IsActive
                                                                    && (ip.PaymentDate.Date) == (DateTime.Now.Date)
                                                                    && ip.PaymentType == 3
                                                                    ).Sum(ip => (double?)ip.Payment)) ?? 0,

                OnlinePaymentsTotal = cxt.Invoices.Where(i => i.IsActive && (i.CreatedAt.Date) == (DateTime.Now.Date))
                                    .Sum(i => i.InvoicePayments.Where(ip => ip.IsActive
                                                                    && (ip.PaymentDate.Date) == (DateTime.Now.Date)
                                                                    && ip.PaymentType == 4
                                                                    ).Sum(ip => (double?)ip.Payment)) ?? 0
            };
        }

        public bool ClearDefaultData()
        {
            try
            {
                cxt.Database.ExecuteSqlRaw(@"
                                                    begin transaction
                                                    go
                                                    truncate table stockitems
                                                    DELETE FROM Stocks
                                                    DBCC CHECKIDENT ('Pharmacy.dbo.Stocks',RESEED, 0)

                                                    --delete invoicepayments
                                                    truncate table invoicepayments

                                                    --delete invoices
                                                    DELETE FROM InvoiceItems
                                                    DBCC CHECKIDENT ('Pharmacy.dbo.InvoiceItems',RESEED, 0)
                                                    DELETE FROM invoices
                                                    DBCC CHECKIDENT ('Pharmacy.dbo.invoices',RESEED, 0)


                                                    --delete stock consumptions
                                                    DELETE FROM StockConsumptionItems
                                                    DBCC CHECKIDENT ('Pharmacy.dbo.StockConsumptionItems',RESEED, 0)
                                                    DELETE FROM StockConsumptions
                                                    DBCC CHECKIDENT ('Pharmacy.dbo.StockConsumptions',RESEED, 0)


                                                    --delete stock adjustments
                                                    DELETE FROM AdjustmentItems
                                                    DBCC CHECKIDENT ('Pharmacy.dbo.AdjustmentItems',RESEED, 0)
                                                    DELETE FROM Adjustments
                                                    DBCC CHECKIDENT ('Pharmacy.dbo.Adjustments',RESEED, 0)

                                                    --delete discounts data related to items
                                                    DELETE FROM DiscountItems
                                                    DBCC CHECKIDENT ('Pharmacy.dbo.DiscountItems',RESEED, 0)
                                                    DELETE FROM FlatDiscounts
                                                    DBCC CHECKIDENT ('Pharmacy.dbo.FlatDiscounts',RESEED, 0)

                                                    --delete purchase orders
                                                    DELETE FROM PurchaseOrderItems
                                                    DBCC CHECKIDENT ('Pharmacy.dbo.PurchaseOrderItems',RESEED, 0)
                                                    DELETE FROM PurchaseOrders
                                                    DBCC CHECKIDENT ('Pharmacy.dbo.PurchaseOrders',RESEED, 0)

                                                    --delete suppliers
                                                    DELETE FROM Suppliers
                                                    DBCC CHECKIDENT ('Pharmacy.dbo.suppliers',RESEED, 0)


                                                    --delete missed sales
                                                    DELETE FROM MissedSales
                                                    DBCC CHECKIDENT ('Pharmacy.dbo.MissedSales',RESEED, 0)

                                                    --delete templateitems
                                                    DELETE FROM TemplateItems
                                                    DBCC CHECKIDENT ('Pharmacy.dbo.TemplateItems',RESEED, 0)

                                                    --delete items
                                                    DELETE FROM Items
                                                    DBCC CHECKIDENT ('Pharmacy.dbo.items',RESEED, 0)

                                                    --delete categories : must be dropped after items are dropped
                                                    DELETE FROM Categories
                                                    DBCC CHECKIDENT ('Pharmacy.dbo.Categories',RESEED, 0)

                                                    --delete manufacturers : must be dropped after items are dropped
                                                    DELETE FROM Manufacturers
                                                    DBCC CHECKIDENT ('Pharmacy.dbo.Manufacturers',RESEED, 0)


                                                    --drop expenses
                                                    DELETE FROM expenses
                                                    DBCC CHECKIDENT ('Pharmacy.dbo.expenses',RESEED, 0)

                                                    commit

                                                ");
                cxt.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<AccTransactionsVM> GetAccountTransactions()
        {
            return (from p in cxt.InvoicePayments
                    where p.IsActive
                    orderby p.InvoicePaymentId descending
                    select new AccTransactionsVM { TrDate = p.PaymentDate, Type = p.PaymentType, Amount = p.Payment, TotalAmount = p.Invoice.GrandTotal, Comments = "Sale Trans" })
                    .Union( // supplier payments
                            from e in cxt.Expenses
                            where e.IsActive
                            orderby e.ExpenseId descending
                            select new AccTransactionsVM { TrDate = e.Date, Type = e.PaymentMode, Amount = e.Amount ?? 0, TotalAmount = e.Stock.GrandTotal, Comments = "Supplier Payments" }
                             )
                    .Union(
                            from a in cxt.AccTransactions
                            where a.IsActive
                            orderby a.AccTransactionId descending
                            select new AccTransactionsVM { TrDate = a.TrDate, Type = 0, Amount = a.Amount, TotalAmount = a.Amount, Comments = a.Description }
                    ).ToList();
        }

        public double GetTodayBalanceSummary(DateTime dayCloseDate)
        {
            var ActionTime = dayCloseDate;
            var openingBalance = cxt.StoreClosings.Where(c => c.IsActive && (c.OpenDate) == (ActionTime)).FirstOrDefault().OpeningBalance;
            var stocks = cxt.Stocks.Where(s => s.IsActive && (s.StockDate) == (ActionTime)).Sum(s => (double?)s.GrandTotal_RetailPrice) ?? 0;
            var HoReturn = cxt.Adjustments.Where(a => a.IsActive && (a.CreatedAt) == (ActionTime) && a.AdjustmentType == (int)Enums.AdjustmentType.HOReturn).Sum(a => (double?)a.GrandTotalRetailPrice) ?? 0;
            var adj = cxt.Adjustments.Where(a => a.IsActive && (a.CreatedAt) == (ActionTime) && a.AdjustmentType != (int)Enums.AdjustmentType.HOReturn).Sum(a => (double?)a.GrandTotalRetailPrice) ?? 0;
            var sale = cxt.Invoices.Where(i => i.IsActive && (i.CreatedAt) == (ActionTime)).Sum(a => (double?)a.GrandTotal) ?? 0;
            var t = (double)(stocks + adj - sale - HoReturn + openingBalance);
            return t;
        }

        public DailySummaryReportWrapprtVM GetDailySummaryReport(DateTime summaryDate)
        {
            var purchase = (from s in cxt.Stocks
                            join si in cxt.StockItems on s.StockId equals si.StockId
                            where (s.CreatedAt) == (summaryDate)
                            && si.Item.Category.ParentId == null
                            group si by si.Item.Category into grpReault
                            select new
                            {
                                CategoryId = (long?)grpReault.FirstOrDefault().Item.Category.CategoryId,
                                CategoryName = grpReault.FirstOrDefault().Item.Category.Name,
                                Amount = grpReault.Sum(r => (double?)(r.RetailPrice * r.Quantity)) ?? 0
                            }).ToList();

            var HoReturn = (from si in cxt.AdjustmentItems
                            where (si.CreatedAt) == (summaryDate)
                            && si.AdjustmentType != (int)Enums.ItemAdjustmentType.Adjustment

                            group si by si.Item.Category into grpReault
                            select new
                            {
                                CategoryId = (long?)grpReault.FirstOrDefault().Item.Category.CategoryId,
                                CategoryName = grpReault.FirstOrDefault().Item.Category.Name,
                                Amount = grpReault.Sum(r => (double?)(r.RetailPrice * r.Quantity * -1)) ?? 0
                            }).ToList();
            var adjustments = (from si in cxt.AdjustmentItems

                               where (si.CreatedAt) == (summaryDate)
                               && si.AdjustmentType == (int)Enums.ItemAdjustmentType.Adjustment
                               group si by si.Item.Category into grpReault
                               select new
                               {
                                   CategoryId = (long?)grpReault.FirstOrDefault().Item.Category.CategoryId,
                                   CategoryName = grpReault.FirstOrDefault().Item.Category.Name,
                                   Amount = grpReault.Sum(r => (double?)r.TotalRetailPrice) ?? 0
                               }).ToList();

            var sales = (from i in cxt.Invoices
                         join si in cxt.InvoiceItems on i.InvoiceId equals si.InvoiceId
                         where (i.CreatedAt) == (summaryDate)
                         && si.Item.Category.ParentId == null
                         group si by si.Item.Category into grpReault
                         select new
                         {
                             CategoryId = (long?)grpReault.FirstOrDefault().Item.Category.CategoryId,
                             CategoryName = grpReault.FirstOrDefault().Item.Category.Name,
                             Amount = grpReault.Sum(r => (double?)r.CalculatedNetAmount) ?? 0
                         }).ToList();

            var expenses = (from e in cxt.Expenses
                            where e.IsActive
                            && (e.CreatedAt) == (summaryDate)
                            && e.ExpenseCategory != null
                            group e by e.ExpenseCategory into grp
                            select new DailySummaryReport_ExpenseVM
                            {
                                Category = grp.FirstOrDefault().ExpenseCategory.Title,
                                Expense = grp.Sum(g => (double?)g.Amount) ?? 0
                            }).ToList();


            DailySummaryReportWrapprtVM summary = new DailySummaryReportWrapprtVM();
            List<DailySummaryRptVM> result = new List<DailySummaryRptVM>();
            foreach (var c in purchase)
            {
                result.Add(new DailySummaryRptVM { CategoryId = c.CategoryId.Value, CategoryName = c.CategoryName, TotalPurchase = c.Amount });
            }

            var found = false;
            foreach (var s in sales)
            {
                found = false;
                for (int a = 0; a < result.Count; a++)
                {
                    if (s.CategoryId == result[a].CategoryId)
                    {
                        result[a].TotalSale = s.Amount;
                        found = true;
                    }
                }
                if (!found)
                {
                    result.Add(new DailySummaryRptVM { CategoryId = s.CategoryId.Value, CategoryName = s.CategoryName, TotalSale = s.Amount });
                }
            }


            foreach (var s in HoReturn)
            {
                found = false;
                for (int a = 0; a < result.Count; a++)
                {
                    if (s.CategoryId == result[a].CategoryId)
                    {
                        result[a].TotalHoReturn = s.Amount;
                        found = true;
                    }
                }
                if (!found)
                {
                    result.Add(new DailySummaryRptVM { CategoryId = (long)s.CategoryId, CategoryName = s.CategoryName, TotalHoReturn = s.Amount });
                }
            }


            foreach (var s in adjustments)
            {
                found = false;
                for (int a = 0; a < result.Count; a++)
                {
                    if (s.CategoryId == result[a].CategoryId)
                    {
                        result[a].TotalAdjustment = s.Amount;
                        found = true;
                    }
                }
                if (!found)
                {
                    result.Add(new DailySummaryRptVM { CategoryId = (long)s.CategoryId, CategoryName = s.CategoryName, TotalHoReturn = s.Amount });
                }
            }




            //total purchases from suppliers
            var localPurchase = cxt.Stocks
                .Where(s => s.IsActive
                    && (s.CreatedAt) == (summaryDate)
                    && !s.Supplier.IsHoSupplier.Value
                ).Sum(r => (double?)r.GrandTotal_RetailPrice) ?? 0;

            var HOPurchase = cxt.Stocks
                .Where(s => s.IsActive
                    && (s.CreatedAt) == (summaryDate)
                    && s.Supplier.IsHoSupplier.Value
                ).Sum(r => (double?)r.GrandTotal_RetailPrice) ?? 0;

            var supplierPayments = (from e in cxt.Expenses
                                    where e.IsActive && e.ExpenseCategory == null
                                    && (e.CreatedAt) == (summaryDate)
                                    group e by e.ExpenseCategory into grp
                                    select new DailySummaryReport_ExpenseVM
                                    {
                                        Category = "Supplier Payments",
                                        Expense = grp.Sum(e => e.Amount) ?? 0
                                    }).FirstOrDefault();

            //var totalPurchase = result.Sum(r => r.TotalPurchase);
            //var totalSale = result.Sum(r => r.TotalSale);
            var totalDiscount = cxt.Invoices.Where(i => i.IsActive
                                    && (i.CreatedAt)
                                    == (summaryDate))
                                    .Sum(r => (double?)(r.SubTotal - r.GrandTotal)) ?? 0;

            var closingSummary = cxt.StoreClosings
                .Where(c => c.ClosingDate.HasValue && (c.ClosingDate) == (summaryDate))
                .Select(c => new StoreClosingVM
                {
                    StoreClosingId = c.StoreClosingId,
                    OpeningCash = c.OpeningCash,
                    TotalInflow = c.TotalInflow,
                    TotalOutFlow = c.TotalOutFlow,
                    SystemCash = c.SystemCash,
                    PhysicalCash = c.PhysicalCash,
                    CashDiff = c.CashDiff,
                    //ClosingDate = c.ClosingDate,
                    //OpenDate =c.OpenDate,
                    //CreatedAt =c.CreatedAt
                    //CreatedBy =c.cre
                    //StoreClosedBy =
                    //StoreOpenedBy =

                    // cash submission attrs
                    CashSubmittedToBank = c.CashSubmittedToBank,
                    CashSubmittedToHO = c.CashSubmittedToHO,
                    TotalCashSubmitted = c.CashSubmittedToHO,
                    ClosingCash = c.ClosingCash
                }).OrderByDescending(c => c.StoreClosingId)
                .ToList();
            var closingDate = InfraUtilityFunctions.GetDateAccordingToDayCloseSetting();
            var closing = cxt.StoreClosings.Where(s => s.IsActive && (s.OpenDate) == (closingDate)).FirstOrDefault();
            summary.OpeningBalance = 0;
            if (closing != null)
            {
                summary.OpeningBalance = (double)closing.OpeningBalance;
            }
            summary.CetagoryWiseSummary = result;
            summary.LocalPurchase = localPurchase;
            summary.HOPurchase = HOPurchase;
            summary.Expenses = expenses;
            summary.Expenses.Add(supplierPayments);
            summary.TotalDiscount = totalDiscount;
            summary.StoreClosingSummary = closingSummary;
            //summary.Adjustments = adjustments;
            return summary;
        }
    }
}