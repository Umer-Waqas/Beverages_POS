

namespace Restaurant_MS_Infrastructure.Repository
{
    public class InvoiceRepository : Repository<Invoice>
    {
        AppDbContext cxt = null;
        public InvoiceRepository(AppDbContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }
        public long InsertInvoice(Invoice objInvoice)
        {
            cxt.Invoices.Add(objInvoice);
            cxt.SaveChanges();
            return objInvoice.InvoiceId;
        }

        public long GetInvoiceRefNo()
        {
            long invRefNo = 0;
            if (cxt.Invoices.FirstOrDefault() == null || cxt.Invoices.OrderByDescending(i => i.InvoiceId).FirstOrDefault().InvoiceRefNo == 0)
            {
                invRefNo = 100001;
            }
            else
            {
                invRefNo = cxt.Invoices.Max(n => n.InvoiceRefNo) + 1;
            }
            return invRefNo;
        }
        public InvoiceVM GetPatient_LastInvoiceDetail(long PatientId)
        {
            //return cxt.Invoices.Include("InvoiceItems").Where(i => i.Patient.PatientId == PatientId).OrderByDescending(i => i.InvoiceId).FirstOrDefault();
            InvoiceVM result =
                cxt.Invoices
                .Where(i => i.Patient.PatientId == PatientId)
           .Select(invoice => new InvoiceVM
           {
               InvoiceId = invoice.InvoiceId,
               CreatedAt = invoice.CreatedAt,
               //subt = order.Supplier.Name,
               SubTotal = invoice.SubTotal,
               GrandTotal = invoice.GrandTotal,
               TotalDiscount = invoice.TotalDiscount,
               //InvoiceItems = invoice.InvoiceItems.Select(i => new InvoiceItemVM
               //{
               //    ItemName = i.Item.ItemName,
               //    Amount = i.Amount
               //})
           }).OrderByDescending(i => i.InvoiceId).FirstOrDefault();
            return result;
        }

        public PatientVM GetPatienInvoices(long PatientId, int PageNo, int Pagesize)
        {
            PatientVM result =
                cxt.Patients.Where(p => p.PatientId == PatientId)
                .Select(p => new PatientVM
                {
                    PatientId = p.PatientId,
                    Name = p.Name,
                    Invoices = p.Invoices.Where(i => i.IsActive).Select(i => new InvoiceVM
                    {
                        InvoiceId = i.InvoiceId,
                        InvoiceRefNo = i.InvoiceRefNo,
                        SubTotal = i.SubTotal,
                        //InvoiceItems = i.InvoiceItems.Where(ii => ii.IsActive).Select(iItem => new InvoiceItemVM
                        //{
                        //    InvoiceItemId = iItem.InvoiceItemId,

                        //    ItemName = iItem.Item.ItemName
                        //}).ToList(),
                        InvoicePayments = i.InvoicePayments.Where(ip => ip.IsActive).Select(ip => new InvoicePaymentVM
                        {
                            Payment = ip.Payment,
                            PaymentDate = ip.PaymentDate
                        }).ToList(),
                        Due = i.Due,
                    }).ToList(),
                    Tags = p.Tags.ToList()
                }).FirstOrDefault();
            return result;
        }
        public EmployeeVM GetEmployeeInvoices(long EmployeeId, int PageNo, int Pagesize)
        {
            EmployeeVM result =
                cxt.Employees.Where(p => p.EmployeeId == EmployeeId)
                .Select(p => new EmployeeVM
                {
                    EmployeeId = p.EmployeeId,
                    Name = p.Name,
                    Invoices = p.Invoices.Where(i => i.IsActive).Select(i => new InvoiceVM
                    {
                        InvoiceId = i.InvoiceId,
                        InvoiceRefNo = i.InvoiceRefNo,
                        SubTotal = i.SubTotal,
                        //InvoiceItems = i.InvoiceItems.Where(ii => ii.IsActive).Select(iItem => new InvoiceItemVM
                        //{
                        //    InvoiceItemId = iItem.InvoiceItemId,

                        //    ItemName = iItem.Item.ItemName
                        //}).ToList(),
                        InvoicePayments = i.InvoicePayments.Where(ip => ip.IsActive).Select(ip => new InvoicePaymentVM
                        {
                            Payment = ip.Payment,
                            PaymentDate = ip.PaymentDate
                        }).ToList(),
                        Due = i.Due,
                    }).ToList()
                }).FirstOrDefault();
            return result;
        }
        public int GetHoldingInvoicesCount()
        {
            return cxt.Invoices.Where(i => i.IsHoldInvoice && i.IsActive).Count();
        }

        public int LagInvoiceId(int invoiceId)
        {
            //string qry = "select lag(invoiceid) from invoices where invoiceid = " + invoiceId;
            //int id = cxt.Database.SqlQuery<int>(qry);
            return 1;
        }
        public int LeadInvoiceId(int invoiceId)
        {
            return 0;
        }
        public Invoice GetInvoicceByIdWithDetails(long InvoiceId)
        {
            return cxt.Invoices
                .Where(i => i.InvoiceId == InvoiceId)
                .Include(i => i.InvoicePayments)
                .Include(i => i.Patient)
                .Where(i => i.InvoicePayments.Any(p => p.IsActive))
                //.Include(i => i.InvoiceItems)
                //.Where(i => i.InvoiceItems.Any(r => r.IsActive))
                .Include(i => i.StockConsumption)
                .Include(i => i.StockConsumption.StockConsumptionItems)
                .Include(i => i.StockConsumption.StockConsumptionItems.Select(sc => sc.Item))
                .Include(i => i.StockConsumption.StockConsumptionItems.Select(sc => sc.Batch))
                //.Include(i => i.StockConsumptions)
                //.Where(i => i.StockConsumptions.Any(c => c.IsActive))
                //.Include(i => i.StockConsumptions.Select(c => c.Item))
                //.Include(i => i.StockConsumptions.Select(c => c.Batch))
                .FirstOrDefault();
        }
        // alternate to above method, but it will require changes on POs screens as well
        public InvoiceVM GetInvoiceDetailById_Edit(long InvoiceId)
        {
            return cxt.Invoices
            .Where(i => i.InvoiceId == InvoiceId)
            .Select(i => new InvoiceVM
            {
                InvoiceId = i.InvoiceId,
                CreatedAt = i.CreatedAt,
                TotalAmount = i.TotalAmount,
                TotalDiscount = i.TotalDiscount,
                GrandTotal = i.GrandTotal,
                SubTotal = i.SubTotal,
                Due = i.Due,
                FirstPayment = i.InvoicePayments.OrderBy(p => p.InvoicePaymentId).Select(p => p.Payment).FirstOrDefault(),
                PaymentDate = i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault(),
                RefundDate = i.InvoicePayments.Where(p => p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault(),
                TotalPaid = i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0,
                TotalRefund = -1 * (i.InvoicePayments.Where(p => p.IsActive && p.Payment < 0).Sum(p => (double?)p.Payment) ?? 0),
                ObjPatient = i.Patient,
                //InvoiceItems = i.InvoiceItems.Where(ii => ii.IsActive).Select(item => new InvoiceItemVM
                //{
                //    Batch = item.Batch,
                //    Item = item.Item,
                //    ItemName = item.Item.ItemName,
                //    Rate = item.Rate,
                //    Quantity = item.Quantity,
                //    Amount = item.Amount
                //}).ToList(),
                InvoicePayments = i.InvoicePayments.Where(p => p.IsActive && p.Payment >= 0).OrderBy(p => p.InvoicePaymentId)
                .Select(p => new InvoicePaymentVM
                {
                    InvoicePaymentId = p.InvoicePaymentId,
                    PaymentType = p.PaymentType,
                    Payment = p.Payment,
                    PaymentDate = p.PaymentDate,
                    RefundReason = p.RefundReason,
                })
            }).FirstOrDefault();
        }
        public InvoiceVM GetInvoiceById_IncludeDetails(long InvoiceId)
        {
            return cxt.Invoices
            .Where(i => i.InvoiceId == InvoiceId)
            .Select(i => new InvoiceVM
            {
                InvoiceId = i.InvoiceId,
                CreatedAt = i.CreatedAt,
                TotalAmount = i.TotalAmount,
                TotalDiscount = i.TotalDiscount,
                GrandTotal = i.GrandTotal,
                SubTotal = i.SubTotal,
                Due = i.Due,
                TotalPaid = cxt.InvoicePayments.Where(p => p.Invoice.InvoiceId == InvoiceId).Sum(p => p.Payment),
                //InvoiceItems = i.InvoiceItems.Select(item => new InvoiceItemVM
                //{
                //    ItemName = item.Item.ItemName,
                //    Rate = item.Rate,
                //    Quantity = item.Quantity,
                //    Amount = item.Amount
                //}).ToList()
            }).FirstOrDefault();
        }
        public InvoiceVM GetInvoiceByIdDetails_IncPatient(long InvoiceId)
        {
            return cxt.Invoices
            .Where(i => i.InvoiceId == InvoiceId)
            .Select(i => new InvoiceVM
            {
                InvoiceId = i.InvoiceId,
                CreatedAt = i.CreatedAt,
                TotalAmount = i.TotalAmount,
                TotalDiscount = i.TotalDiscount,
                GrandTotal = i.GrandTotal,
                SubTotal = i.SubTotal,
                Due = i.Due,
                PaymentDate = i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault(),
                RefundDate = i.InvoicePayments.Where(p => p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault(),
                TotalPaid = i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0,
                PaymentType = i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentType).FirstOrDefault(),
                TotalRefund = -1 * (i.InvoicePayments.Where(p => p.IsActive && p.Payment < 0).Sum(p => (double?)p.Payment) ?? 0),
                Patient = new PatientVM
                {
                    Name = i.Patient.Name,
                    Phone = i.Patient.Phone,
                    DateOfBirth = i.Patient.DateOfBirth,
                    Gender = i.Patient.Gender,
                    AgeYears = i.Patient.AgeYears ?? 0,
                    AgeMonths = i.Patient.AgeMonths ?? 0,
                    AgeDays = i.Patient.AgeDays ?? 0
                },
                //InvoiceItems = i.InvoiceItems.Select(item => new InvoiceItemVM
                //{
                //    ItemName = item.Item.ItemName,
                //    Rate = item.Rate,
                //    Quantity = item.Quantity,
                //    Amount = item.Amount
                //}).ToList()
            }).FirstOrDefault();
        }
        public InvoiceVM GetPOSInvoiceDetailById(long InvoiceId)
        {
            return cxt.Invoices
            .Where(i => i.InvoiceId == InvoiceId)
            .Select(i => new InvoiceVM
            {
                InvoiceId = i.InvoiceId,
                InvoiceRefNo = i.InvoiceRefNo,
                CreatedAt = i.CreatedAt,
                TotalAmount = i.TotalAmount,
                TotalDiscount = i.TotalDiscount,
                DiscountType = i.DiscountType,
                GrandTotal = i.GrandTotal,
                SubTotal = i.SubTotal,
                Due = i.Due,
                FirstPayment = i.InvoicePayments.OrderBy(p => p.InvoicePaymentId).Select(p => p.Payment).FirstOrDefault(),
                PaymentDate = i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault(),
                RefundDate = i.InvoicePayments.Where(p => p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault(),
                TotalPaid = i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0,
                TotalRefund = -1 * (i.InvoicePayments.Where(p => p.IsActive && p.Payment < 0).Sum(p => (double?)p.Payment) ?? 0),
                ObjPatient = i.Patient,
                Note = i.Note,
                FBR_InvoiceNo = i.FBR_InvoiceNo,
                OrderType = i.OrderType,
                //InvoiceItems = i.InvoiceItems.Where(ii => ii.IsActive).Select(item => new InvoiceItemVM
                //{
                //    ItemName = item.Item.ItemName,
                //    BatchName = item.Batch != null ? item.Batch.BatchName : "",
                //    BatchExpiry = item.Batch != null ? item.Batch.Expiry.ToString() : "",
                //    Rate = item.Rate,
                //    Quantity = item.Unit == 1 ? item.Quantity : item.Quantity / item.Item.ConversionUnit,
                //    BonusQuantity = (double?)(item.Unit == 1 ? item.BonusQuantity ?? 0 : item.BonusQuantity / item.Item.ConversionUnit) ?? 0,
                //    Amount = item.Amount,
                //    DiscountString = item.Discount + (item.DiscountType == 1 ? "%" : ""),
                //    SalesTaxString = item.SalesTax + (item.SalesTaxType == 1 ? "%" : ""),
                //    NetAmount = item.NetAmount,
                //    UnitString = item.Unit == 1 ? "unit" : item.Item.Unit
                //}).ToList(),
                InvoicePayments = i.InvoicePayments.Where(p => p.IsActive && p.Payment >= 0).OrderBy(p => p.InvoicePaymentId)
                .Select(p => new InvoicePaymentVM
                {
                    PaymentDate = p.PaymentDate,
                    PaymentType = p.PaymentType,
                    PaymentString = "Rs. " + p.Payment + "/-"
                })
            }).FirstOrDefault();
        }
        public InvoiceVM GetPOSInvoiceById(long InvoiceId)
        {
            return cxt.Invoices
            .Where(i => i.InvoiceId == InvoiceId)
            .Select(i => new InvoiceVM
            {
                InvoiceId = i.InvoiceId,
                CreatedAt = i.CreatedAt,
                //TotalAmount = i.TotalAmount,
                //TotalDiscount = i.TotalDiscount,
                GrandTotal = i.GrandTotal,
                //SubTotal = i.SubTotal,
                Due = i.Due,
                //FirstPayment = i.InvoicePayments.OrderBy(p => p.InvoicePaymentId).Select(p => p.Payment).FirstOrDefault(),
                PaymentDate = i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault(),
                RefundDate = i.InvoicePayments.Where(p => p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault(),
                TotalPaid = i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0,
                TotalRefund = -1 * (i.InvoicePayments.Where(p => p.IsActive && p.Payment < 0).Sum(p => (double?)p.Payment) ?? 0),
                PaymentType = i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentType).FirstOrDefault(),
                ObjPatient = i.Patient,
                Note = i.Note,
                //InvoiceItems = i.InvoiceItems.Where(ii => ii.IsActive).Select(item => new InvoiceItemVM
                //{
                //    ItemName = item.Item.ItemName,
                //    //Rate = item.Rate,
                //    //Quantity = item.Quantity,
                //    //Amount = item.Amount
                //}).ToList()
                //InvoicePayments = (i.InvoicePayments.Where(p => p.IsActive && p.Payment >= 0).OrderBy(p => p.InvoicePaymentId)
                //.Select(p => new InvoicePaymentVM
                //{
                //    Date = p.Date,
                //    PaymentType = p.PaymentType,
                //    PaymentString = "Rs. " + p.Payment + "/-"
                //}))
            }).FirstOrDefault();
        }
        public InvoiceVM GetProcedureInvoiceDetailById(long InvoiceId)
        {
            return cxt.Invoices
            .Where(i => i.InvoiceId == InvoiceId)
            .Select(i => new InvoiceVM
            {
                InvoiceId = i.InvoiceId,
                CreatedAt = i.CreatedAt,
                TotalAmount = i.TotalAmount,
                TotalDiscount = i.TotalDiscount,
                GrandTotal = i.GrandTotal,
                SubTotal = i.SubTotal,
                Due = i.Due,
                FirstPayment = i.InvoicePayments.OrderBy(p => p.InvoicePaymentId).Select(p => p.Payment).FirstOrDefault(),
                PaymentDate = i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault(),
                RefundDate = i.InvoicePayments.Where(p => p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault(),
                TotalPaid = i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0,
                TotalRefund = -1 * (i.InvoicePayments.Where(p => p.IsActive && p.Payment < 0).Sum(p => (double?)p.Payment) ?? 0),
                ObjPatient = i.Patient,
                //InvoiceItems = i.InvoiceItems.Where(ii => ii.IsActive).Select(item => new InvoiceItemVM
                //{
                //    Rate = item.Rate,
                //    Quantity = item.Quantity,
                //    Amount = item.Amount
                //}).ToList(),
                InvoicePayments = i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderBy(p => p.InvoicePaymentId)
                .Select(p => new InvoicePaymentVM
                {
                    PaymentDate = p.PaymentDate,
                    PaymentType = p.PaymentType,
                    PaymentString = "Rs. " + p.Payment + "/-"
                })
            }).FirstOrDefault();
        }

        public IPagedList<InvoiceTransactionsVM> GetTransactions(DateTime DateFrom, DateTime DateTo, int PageNo, int PageSize)
        {
            return cxt.Invoices
            .Where(i => i.IsActive && !i.IsHoldInvoice)
            .Where(i => (i.CreatedAt.Date) >= (DateFrom.Date)
                && (i.CreatedAt.Date) <= (DateTo.Date)
            )
            .Select(i => new InvoiceTransactionsVM
            {
                InvoiceId = i.InvoiceId,
                InvoiceRefNo = i.InvoiceRefNo,
                CreatedAt = i.CreatedAt,
                TotalAmount = i.TotalAmount,
                TotalDiscount = i.TotalDiscount,
                DiscountType = i.DiscountType,
                GrandTotal = i.GrandTotal,
                SubTotal = i.SubTotal,
                Due = i.Due,
                //SaleQuantity = i.InvoiceItems.Sum(it => (double?)it.Quantity) ?? 0,
                //ReturnedQty = i.InvoiceItems.Sum(it => (double?)it.ReturnedQuantity) ?? 0,
                PaymentDate = i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault(),
                //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                TotalPaid = i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0,
                //TotalRefund = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)),
                Patient = i.Patient,
                PatientName = i.Patient.Name,
                IsProcedureInvoice = i.IsProcedureInvoice,
                UserName = i.User.UserName,
                PaymentStatus = i.PaymentStatus,
                EmployeeName = i.Employee.Name
            }).OrderByDescending(i => i.InvoiceId).ToPagedList(PageNo, PageSize);
        }
        public IPagedList<InvoiceTransactionsVM> GetTransactions(int PageNo, int PageSize)
        {
            return cxt.Invoices
            .Where(i => i.IsActive && !i.IsHoldInvoice)
            .Select(i => new InvoiceTransactionsVM
            {
                InvoiceId = i.InvoiceId,
                InvoiceRefNo = i.InvoiceRefNo,
                CreatedAt = i.CreatedAt,
                TotalAmount = i.TotalAmount,
                TotalDiscount = i.TotalDiscount,
                DiscountType = i.DiscountType,
                GrandTotal = i.GrandTotal,
                SubTotal = i.SubTotal,
                Due = i.Due,
                //SaleQuantity = i.InvoiceItems.Sum(it => (double?)it.Quantity) ?? 0,
                //ReturnedQty = i.InvoiceItems.Sum(it => (double?)it.ReturnedQuantity) ?? 0,
                PaymentDate = i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault(),
                //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                TotalPaid = i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0,
                //TotalRefund = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)),
                Patient = i.Patient,
                PatientName = i.Patient.Name,
                IsProcedureInvoice = i.IsProcedureInvoice,
                UserName = i.User.UserName,
                PaymentStatus = i.PaymentStatus,
                EmployeeName = i.Employee.Name
            }).OrderByDescending(i => i.InvoiceId).ToPagedList(PageNo, PageSize);
        }
        public IPagedList<InvoiceTransactionsVM> GetTransactions_Rpt(int PageNo, int PageSize)
        {
            //return cxt.Invoices
            //.Where(i => i.IsActive)
            //.Where(i => (i.CreatedAt.Date) >= (DateFrom.Date)
            //    && ((i.CreatedAt.Date) <= (DateTo.Date))
            //)
            //.Select(i => new InvoiceTransactionsVM
            //{
            //    InvoiceId = i.InvoiceId,
            //    CreatedAt = i.CreatedAt,
            //    TotalAmount = i.TotalAmount,
            //    TotalDiscount = i.TotalDiscount,
            //    DiscountType = i.DiscountType,
            //    GrandTotal = i.GrandTotal,
            //    SubTotal = i.SubTotal,
            //    Due = i.Due,
            //    Advance = i.Advance,
            //    SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
            //    ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
            //    PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
            //    //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
            //    TotalPaid = (double?)(i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment)) ?? 0,
            //    TotalRefund = (double?)(cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)) ?? 0,
            //    Patient = i.Patient,
            //    PatientName = i.Patient.Name,
            //    IsProcedureInvoice = i.IsProcedureInvoice,
            //    //InvoicePayments = i.InvoicePayments.ToList(),
            //}).OrderByDescending(i => i.InvoiceId).ToPagedList(PageNo, PageSize);  

            return
                (from i in cxt.Invoices
                 join p in cxt.InvoicePayments on i equals p.Invoice
                 where p.IsActive && i.IsActive
                 select new InvoiceTransactionsVM
                 {
                     InvoiceId = i.InvoiceId,
                     InvoiceRefNo = i.InvoiceRefNo,
                     CreatedAt = i.CreatedAt,
                     TotalAmount = i.TotalAmount,
                     TotalDiscount = i.TotalDiscount,
                     DiscountType = i.DiscountType,
                     GrandTotal = i.GrandTotal,
                     SubTotal = i.SubTotal,
                     Due = i.Due,
                     //SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                     //ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                     PaymentDate = p.PaymentDate,
                     PaymentCreatedAt = p.CreatedAt,
                     TotalPaid = p.Payment,
                     //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                     //TotalPaid = (double?)(i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment)) ?? 0,
                     //TotalRefund = (double?)(cxt.invoicePayments.Where(ip => ip.Invoice.InvoiceId == i.InvoiceId && ip.IsActive && ip.Payment < 0).Sum(pp => pp.Payment)) ?? 0,
                     Patient = i.Patient,
                     PatientName = i.Patient.Name,
                     IsProcedureInvoice = i.IsProcedureInvoice,
                     //InvoicePayments = i.InvoicePayments.ToList(),
                 }).OrderByDescending(i => i.PaymentDate).ToPagedList(PageNo, PageSize);

            // this query was being used to show data when multiple refund payments were being inserted and it was required to show single transaction for refund while multiple transactions for customer payments
            //return
            //    ((from i in cxt.Invoices
            //     join p in cxt.invoicePayments on i equals p.Invoice
            //     where p.IsActive
            //     && (p.Date) >= (DateFrom.Date)
            //     && ((p.Date) <= (DateTo.Date))
            //     && p.Payment > 0
            //     select new InvoiceTransactionsVM
            //     {
            //         InvoiceId = i.InvoiceId,
            //         CreatedAt = i.CreatedAt,
            //         TotalAmount = i.TotalAmount,
            //         TotalDiscount = i.TotalDiscount,
            //         DiscountType = i.DiscountType,
            //         GrandTotal = i.GrandTotal,
            //         SubTotal = i.SubTotal,
            //         Due = i.Due,
            //         Advance = i.Advance,
            //         SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
            //         ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
            //         PaymentDate = p.Date,
            //         TotalPaid = p.Payment,
            //         //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
            //         //TotalPaid = (double?)(i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment)) ?? 0,
            //         //TotalRefund = (double?)(cxt.invoicePayments.Where(ip => ip.Invoice.InvoiceId == i.InvoiceId && ip.IsActive && ip.Payment < 0).Sum(pp => pp.Payment)) ?? 0,
            //         Patient = i.Patient,
            //         PatientName = i.Patient.Name,
            //         IsProcedureInvoice = i.IsProcedureInvoice,
            //         //InvoicePayments = i.InvoicePayments.ToList(),
            //     })
            //     .Union
            //    (from i in cxt.Invoices
            //     join p in cxt.invoicePayments on i equals p.Invoice
            //     where p.IsActive
            //     && (p.Date) >= (DateFrom.Date)
            //     && ((p.Date) <= (DateTo.Date))
            //     && p.Payment < 0
            //     group i by i into g
            //     select new InvoiceTransactionsVM
            //     {
            //         InvoiceId = g.Key.InvoiceId,
            //         CreatedAt = g.Key.CreatedAt,
            //         TotalAmount = g.Key.TotalAmount,
            //         TotalDiscount = g.Key.TotalDiscount,
            //         DiscountType = g.Key.DiscountType,
            //         GrandTotal = g.Key.GrandTotal,
            //         SubTotal = g.Key.SubTotal,
            //         Due = g.Key.Due,
            //         Advance = g.Key.Advance,
            //         SaleQuantity = g.Key.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
            //         ReturnedQty = g.Key.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
            //         PaymentDate = g.Key.InvoicePayments.Where(ip => ip.Payment < 0).Select(r => r.Date).FirstOrDefault(),
            //         TotalPaid = (double?)g.Key.InvoicePayments.Where(ip=>ip.Payment<0 && ip.IsActive).Sum(ip => ip.Payment) ?? 0,
            //         //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
            //         //TotalPaid = (double?)(i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment)) ?? 0,
            //         //TotalRefund = (double?)(cxt.invoicePayments.Where(ip => ip.Invoice.InvoiceId == i.InvoiceId && ip.IsActive && ip.Payment < 0).Sum(pp => pp.Payment)) ?? 0,
            //         Patient = g.Key.Patient,
            //         PatientName = g.Key.Patient.Name,
            //         IsProcedureInvoice = g.Key.IsProcedureInvoice,
            //         //InvoicePayments = i.InvoicePayments.ToList(),
            //     })).OrderByDescending(i => i.InvoiceId).ToPagedList(PageNo, PageSize);
            ////return null;
        }
        public IPagedList<InvoiceTransactionsVM> GetTransactions_Rpt(DateTime DateFrom, DateTime DateTo, int PageNo, int PageSize)
        {
            //return cxt.Invoices
            //.Where(i => i.IsActive)
            //.Where(i => (i.CreatedAt.Date) >= (DateFrom.Date)
            //    && ((i.CreatedAt.Date) <= (DateTo.Date))
            //)
            //.Select(i => new InvoiceTransactionsVM
            //{
            //    InvoiceId = i.InvoiceId,
            //    CreatedAt = i.CreatedAt,
            //    TotalAmount = i.TotalAmount,
            //    TotalDiscount = i.TotalDiscount,
            //    DiscountType = i.DiscountType,
            //    GrandTotal = i.GrandTotal,
            //    SubTotal = i.SubTotal,
            //    Due = i.Due,
            //    Advance = i.Advance,
            //    SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
            //    ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
            //    PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
            //    //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
            //    TotalPaid = (double?)(i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment)) ?? 0,
            //    TotalRefund = (double?)(cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)) ?? 0,
            //    Patient = i.Patient,
            //    PatientName = i.Patient.Name,
            //    IsProcedureInvoice = i.IsProcedureInvoice,
            //    //InvoicePayments = i.InvoicePayments.ToList(),
            //}).OrderByDescending(i => i.InvoiceId).ToPagedList(PageNo, PageSize);  

            return
                (from i in cxt.Invoices
                 join p in cxt.InvoicePayments on i equals p.Invoice
                 where p.IsActive
                 && (p.UpdatedAt.Date) >= (DateFrom.Date)
                 && (p.UpdatedAt.Date) <= (DateTo.Date)
                 select new InvoiceTransactionsVM
                 {
                     InvoiceId = i.InvoiceId,
                     InvoiceRefNo = i.InvoiceRefNo,
                     CreatedAt = i.CreatedAt,
                     TotalAmount = i.TotalAmount,
                     TotalDiscount = i.TotalDiscount,
                     DiscountType = i.DiscountType,
                     GrandTotal = i.GrandTotal,
                     SubTotal = i.SubTotal,
                     Due = i.Due,
                     //SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                     //ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                     PaymentDate = p.PaymentDate,
                     PaymentCreatedAt = p.CreatedAt,
                     TotalPaid = p.Payment,
                     //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                     //TotalPaid = (double?)(i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment)) ?? 0,
                     //TotalRefund = (double?)(cxt.invoicePayments.Where(ip => ip.Invoice.InvoiceId == i.InvoiceId && ip.IsActive && ip.Payment < 0).Sum(pp => pp.Payment)) ?? 0,
                     Patient = i.Patient,
                     PatientName = i.Patient.Name,
                     IsProcedureInvoice = i.IsProcedureInvoice,
                     //InvoicePayments = i.InvoicePayments.ToList(),
                 }).OrderByDescending(i => i.PaymentDate).ToPagedList(PageNo, PageSize);

            // this query was being used to show data when multiple refund payments were being inserted and it was required to show single transaction for refund while multiple transactions for customer payments
            //return
            //    ((from i in cxt.Invoices
            //     join p in cxt.invoicePayments on i equals p.Invoice
            //     where p.IsActive
            //     && (p.Date) >= (DateFrom.Date)
            //     && ((p.Date) <= (DateTo.Date))
            //     && p.Payment > 0
            //     select new InvoiceTransactionsVM
            //     {
            //         InvoiceId = i.InvoiceId,
            //         CreatedAt = i.CreatedAt,
            //         TotalAmount = i.TotalAmount,
            //         TotalDiscount = i.TotalDiscount,
            //         DiscountType = i.DiscountType,
            //         GrandTotal = i.GrandTotal,
            //         SubTotal = i.SubTotal,
            //         Due = i.Due,
            //         Advance = i.Advance,
            //         SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
            //         ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
            //         PaymentDate = p.Date,
            //         TotalPaid = p.Payment,
            //         //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
            //         //TotalPaid = (double?)(i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment)) ?? 0,
            //         //TotalRefund = (double?)(cxt.invoicePayments.Where(ip => ip.Invoice.InvoiceId == i.InvoiceId && ip.IsActive && ip.Payment < 0).Sum(pp => pp.Payment)) ?? 0,
            //         Patient = i.Patient,
            //         PatientName = i.Patient.Name,
            //         IsProcedureInvoice = i.IsProcedureInvoice,
            //         //InvoicePayments = i.InvoicePayments.ToList(),
            //     })
            //     .Union
            //    (from i in cxt.Invoices
            //     join p in cxt.invoicePayments on i equals p.Invoice
            //     where p.IsActive
            //     && (p.Date) >= (DateFrom.Date)
            //     && ((p.Date) <= (DateTo.Date))
            //     && p.Payment < 0
            //     group i by i into g
            //     select new InvoiceTransactionsVM
            //     {
            //         InvoiceId = g.Key.InvoiceId,
            //         CreatedAt = g.Key.CreatedAt,
            //         TotalAmount = g.Key.TotalAmount,
            //         TotalDiscount = g.Key.TotalDiscount,
            //         DiscountType = g.Key.DiscountType,
            //         GrandTotal = g.Key.GrandTotal,
            //         SubTotal = g.Key.SubTotal,
            //         Due = g.Key.Due,
            //         Advance = g.Key.Advance,
            //         SaleQuantity = g.Key.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
            //         ReturnedQty = g.Key.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
            //         PaymentDate = g.Key.InvoicePayments.Where(ip => ip.Payment < 0).Select(r => r.Date).FirstOrDefault(),
            //         TotalPaid = (double?)g.Key.InvoicePayments.Where(ip=>ip.Payment<0 && ip.IsActive).Sum(ip => ip.Payment) ?? 0,
            //         //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
            //         //TotalPaid = (double?)(i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment)) ?? 0,
            //         //TotalRefund = (double?)(cxt.invoicePayments.Where(ip => ip.Invoice.InvoiceId == i.InvoiceId && ip.IsActive && ip.Payment < 0).Sum(pp => pp.Payment)) ?? 0,
            //         Patient = g.Key.Patient,
            //         PatientName = g.Key.Patient.Name,
            //         IsProcedureInvoice = g.Key.IsProcedureInvoice,
            //         //InvoicePayments = i.InvoicePayments.ToList(),
            //     })).OrderByDescending(i => i.InvoiceId).ToPagedList(PageNo, PageSize);
            ////return null;
        }
        public IPagedList<InvoiceTransactionsVM> GetTransactions_Rpt(long PatientId, int PageNo, int PageSize)
        {
            //return cxt.Invoices
            //.Where(i => i.IsActive)
            //.Where(i => (i.CreatedAt.Date) >= (DateFrom.Date)
            //    && ((i.CreatedAt.Date) <= (DateTo.Date))
            //)
            //.Select(i => new InvoiceTransactionsVM
            //{
            //    InvoiceId = i.InvoiceId,
            //    CreatedAt = i.CreatedAt,
            //    TotalAmount = i.TotalAmount,
            //    TotalDiscount = i.TotalDiscount,
            //    DiscountType = i.DiscountType,
            //    GrandTotal = i.GrandTotal,
            //    SubTotal = i.SubTotal,
            //    Due = i.Due,
            //    Advance = i.Advance,
            //    SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
            //    ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
            //    PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
            //    //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
            //    TotalPaid = (double?)(i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment)) ?? 0,
            //    TotalRefund = (double?)(cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)) ?? 0,
            //    Patient = i.Patient,
            //    PatientName = i.Patient.Name,
            //    IsProcedureInvoice = i.IsProcedureInvoice,
            //    //InvoicePayments = i.InvoicePayments.ToList(),
            //}).OrderByDescending(i => i.InvoiceId).ToPagedList(PageNo, PageSize);  

            return
                (from i in cxt.Invoices
                 join p in cxt.InvoicePayments on i equals p.Invoice
                 where p.IsActive && i.IsActive && i.Patient != null && i.Patient.PatientId == PatientId
                 select new InvoiceTransactionsVM
                 {
                     InvoiceId = i.InvoiceId,
                     InvoiceRefNo = i.InvoiceRefNo,
                     CreatedAt = i.CreatedAt,
                     TotalAmount = i.TotalAmount,
                     TotalDiscount = i.TotalDiscount,
                     DiscountType = i.DiscountType,
                     GrandTotal = i.GrandTotal,
                     SubTotal = i.SubTotal,
                     Due = i.Due,
                     //SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                     //ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                     PaymentDate = p.PaymentDate,
                     PaymentCreatedAt = p.CreatedAt,
                     TotalPaid = p.Payment,
                     //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                     //TotalPaid = (double?)(i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment)) ?? 0,
                     //TotalRefund = (double?)(cxt.invoicePayments.Where(ip => ip.Invoice.InvoiceId == i.InvoiceId && ip.IsActive && ip.Payment < 0).Sum(pp => pp.Payment)) ?? 0,
                     Patient = i.Patient,
                     PatientName = i.Patient.Name,
                     IsProcedureInvoice = i.IsProcedureInvoice,
                     //InvoicePayments = i.InvoicePayments.ToList(),
                 }).OrderByDescending(i => i.PaymentDate).ToPagedList(PageNo, PageSize);

            // this query was being used to show data when multiple refund payments were being inserted and it was required to show single transaction for refund while multiple transactions for customer payments
            //return
            //    ((from i in cxt.Invoices
            //     join p in cxt.invoicePayments on i equals p.Invoice
            //     where p.IsActive
            //     && (p.Date) >= (DateFrom.Date)
            //     && ((p.Date) <= (DateTo.Date))
            //     && p.Payment > 0
            //     select new InvoiceTransactionsVM
            //     {
            //         InvoiceId = i.InvoiceId,
            //         CreatedAt = i.CreatedAt,
            //         TotalAmount = i.TotalAmount,
            //         TotalDiscount = i.TotalDiscount,
            //         DiscountType = i.DiscountType,
            //         GrandTotal = i.GrandTotal,
            //         SubTotal = i.SubTotal,
            //         Due = i.Due,
            //         Advance = i.Advance,
            //         SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
            //         ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
            //         PaymentDate = p.Date,
            //         TotalPaid = p.Payment,
            //         //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
            //         //TotalPaid = (double?)(i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment)) ?? 0,
            //         //TotalRefund = (double?)(cxt.invoicePayments.Where(ip => ip.Invoice.InvoiceId == i.InvoiceId && ip.IsActive && ip.Payment < 0).Sum(pp => pp.Payment)) ?? 0,
            //         Patient = i.Patient,
            //         PatientName = i.Patient.Name,
            //         IsProcedureInvoice = i.IsProcedureInvoice,
            //         //InvoicePayments = i.InvoicePayments.ToList(),
            //     })
            //     .Union
            //    (from i in cxt.Invoices
            //     join p in cxt.invoicePayments on i equals p.Invoice
            //     where p.IsActive
            //     && (p.Date) >= (DateFrom.Date)
            //     && ((p.Date) <= (DateTo.Date))
            //     && p.Payment < 0
            //     group i by i into g
            //     select new InvoiceTransactionsVM
            //     {
            //         InvoiceId = g.Key.InvoiceId,
            //         CreatedAt = g.Key.CreatedAt,
            //         TotalAmount = g.Key.TotalAmount,
            //         TotalDiscount = g.Key.TotalDiscount,
            //         DiscountType = g.Key.DiscountType,
            //         GrandTotal = g.Key.GrandTotal,
            //         SubTotal = g.Key.SubTotal,
            //         Due = g.Key.Due,
            //         Advance = g.Key.Advance,
            //         SaleQuantity = g.Key.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
            //         ReturnedQty = g.Key.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
            //         PaymentDate = g.Key.InvoicePayments.Where(ip => ip.Payment < 0).Select(r => r.Date).FirstOrDefault(),
            //         TotalPaid = (double?)g.Key.InvoicePayments.Where(ip=>ip.Payment<0 && ip.IsActive).Sum(ip => ip.Payment) ?? 0,
            //         //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
            //         //TotalPaid = (double?)(i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment)) ?? 0,
            //         //TotalRefund = (double?)(cxt.invoicePayments.Where(ip => ip.Invoice.InvoiceId == i.InvoiceId && ip.IsActive && ip.Payment < 0).Sum(pp => pp.Payment)) ?? 0,
            //         Patient = g.Key.Patient,
            //         PatientName = g.Key.Patient.Name,
            //         IsProcedureInvoice = g.Key.IsProcedureInvoice,
            //         //InvoicePayments = i.InvoicePayments.ToList(),
            //     })).OrderByDescending(i => i.InvoiceId).ToPagedList(PageNo, PageSize);
            ////return null;
        }
        public List<InvoiceTransactionsVM> GetTransactions(long InvoiceId)
        {
            //return cxt.Invoices
            //.Where(i => i.IsActive)
            //.Where(i => (i.CreatedAt.Date) >= (DateFrom.Date)
            //    && ((i.CreatedAt.Date) <= (DateTo.Date))
            //)
            //.Select(i => new InvoiceTransactionsVM
            //{
            //    InvoiceId = i.InvoiceId,
            //    CreatedAt = i.CreatedAt,
            //    TotalAmount = i.TotalAmount,
            //    TotalDiscount = (i.DiscountType == 2 ? (i.TotalDiscount / 100) * i.SubTotal : i.TotalDiscount),
            //    DiscountType = i.DiscountType,
            //    GrandTotal = i.GrandTotal,
            //    SubTotal = i.SubTotal,
            //    Due = i.Due,
            //    Advance = i.Advance,
            //    SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
            //    ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
            //    PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
            //    //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
            //    TotalPaid = (double?)(i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment)) ?? 0,
            //    TotalRefund = (double?)(cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)) ?? 0,
            //    Patient = i.Patient,
            //    PatientName = i.Patient.Name,
            //    IsProcedureInvoice = i.IsProcedureInvoice,
            //}).OrderByDescending(i => i.InvoiceId).ToList();
            return
                (from i in cxt.Invoices
                 join p in cxt.InvoicePayments on i equals p.Invoice
                 where p.IsActive
               && i.InvoiceId == InvoiceId
                 select new InvoiceTransactionsVM
                 {
                     InvoiceId = i.InvoiceId,
                     InvoiceRefNo = i.InvoiceRefNo,
                     CreatedAt = i.CreatedAt,
                     TotalAmount = i.TotalAmount,
                     TotalDiscount = i.TotalDiscount,
                     DiscountType = i.DiscountType,
                     GrandTotal = i.GrandTotal,
                     SubTotal = i.SubTotal,
                     Due = i.Due,
                     //SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                     //ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                     PaymentDate = p.PaymentDate,
                     TotalPaid = p.Payment,
                     //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                     //TotalPaid = (double?)(i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment)) ?? 0,
                     //TotalRefund = (double?)(cxt.invoicePayments.Where(ip => ip.Invoice.InvoiceId == i.InvoiceId && ip.IsActive && ip.Payment < 0).Sum(pp => pp.Payment)) ?? 0,
                     Patient = i.Patient,
                     PatientName = i.Patient.Name,
                     IsProcedureInvoice = i.IsProcedureInvoice,
                     //InvoicePayments = i.InvoicePayments.ToList(),
                 }).OrderByDescending(i => i.InvoiceId).ToList();
        }
        public List<InvoiceTransactionsVM> GetTransactions(DateTime DateFrom, DateTime DateTo)
        {
            //return cxt.Invoices
            //.Where(i => i.IsActive)
            //.Where(i => (i.CreatedAt.Date) >= (DateFrom.Date)
            //    && ((i.CreatedAt.Date) <= (DateTo.Date))
            //)
            //.Select(i => new InvoiceTransactionsVM
            //{
            //    InvoiceId = i.InvoiceId,
            //    CreatedAt = i.CreatedAt,
            //    TotalAmount = i.TotalAmount,
            //    TotalDiscount = (i.DiscountType == 2 ? (i.TotalDiscount / 100) * i.SubTotal : i.TotalDiscount),
            //    DiscountType = i.DiscountType,
            //    GrandTotal = i.GrandTotal,
            //    SubTotal = i.SubTotal,
            //    Due = i.Due,
            //    Advance = i.Advance,
            //    SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
            //    ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
            //    PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
            //    //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
            //    TotalPaid = (double?)(i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment)) ?? 0,
            //    TotalRefund = (double?)(cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)) ?? 0,
            //    Patient = i.Patient,
            //    PatientName = i.Patient.Name,
            //    IsProcedureInvoice = i.IsProcedureInvoice,
            //}).OrderByDescending(i => i.InvoiceId).ToList();
            return
                (from i in cxt.Invoices
                 join p in cxt.InvoicePayments on i equals p.Invoice
                 where p.IsActive
                 && (p.UpdatedAt) >= (DateFrom.Date)
                 && (p.UpdatedAt) <= (DateTo.Date)
                 select new InvoiceTransactionsVM
                 {
                     InvoiceId = i.InvoiceId,
                     InvoiceRefNo = i.InvoiceRefNo,
                     CreatedAt = i.CreatedAt,
                     TotalAmount = i.TotalAmount,
                     TotalDiscount = i.TotalDiscount,
                     DiscountType = i.DiscountType,
                     GrandTotal = i.GrandTotal,
                     SubTotal = i.SubTotal,
                     Due = i.Due,
                     //SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                     //ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                     PaymentDate = p.PaymentDate,
                     TotalPaid = p.Payment,
                     //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                     //TotalPaid = (double?)(i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment)) ?? 0,
                     //TotalRefund = (double?)(cxt.invoicePayments.Where(ip => ip.Invoice.InvoiceId == i.InvoiceId && ip.IsActive && ip.Payment < 0).Sum(pp => pp.Payment)) ?? 0,
                     Patient = i.Patient,
                     PatientName = i.Patient.Name,
                     IsProcedureInvoice = i.IsProcedureInvoice,
                     //InvoicePayments = i.InvoicePayments.ToList(),
                 }).OrderByDescending(i => i.InvoiceId).ToList();
        }

        public IPagedList<InvoiceTransactionsVM> GetTransactions(long InvoiceNo, int PageNo, int PageSize)
        {
            return cxt.Invoices
            .Where(i => i.IsActive && i.InvoiceRefNo == InvoiceNo && !i.IsHoldInvoice)
            .Select(i => new InvoiceTransactionsVM
            {
                InvoiceId = i.InvoiceId,
                InvoiceRefNo = i.InvoiceRefNo,
                CreatedAt = i.CreatedAt,
                TotalAmount = i.TotalAmount,
                TotalDiscount = i.TotalDiscount,
                DiscountType = i.DiscountType,
                GrandTotal = i.GrandTotal,
                SubTotal = i.SubTotal,
                Due = i.Due,
                //SaleQuantity = i.InvoiceItems.Sum(it => (double?)it.Quantity) ?? 0,
                //ReturnedQty = i.InvoiceItems.Sum(it => (double?)it.ReturnedQuantity) ?? 0,
                PaymentDate = i.InvoicePayments.Where(p => p.IsActive).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault(),
                //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                TotalPaid = (double?)i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment) ?? 0,
                //TotalRefund = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)),
                Patient = i.Patient,
                PatientName = i.Patient.Name,
                IsProcedureInvoice = i.IsProcedureInvoice,
                UserName = i.User.UserName,
                PaymentStatus = i.PaymentStatus,
                EmployeeName = i.Employee.Name
            }).OrderByDescending(i => i.InvoiceId).ToPagedList(PageNo, PageSize);
        }
        public IPagedList<InvoiceTransactionsVM> GetTransactions(long EmployeeId, DateTime DateFrom, DateTime DateTo, int PageNo, int PageSize)
        {
            return cxt.Invoices
            .Where(i => i.IsActive && i.EmployeeId == EmployeeId && !i.IsHoldInvoice
            && (i.UpdatedAt.Date) >= (DateFrom.Date)
            && (i.UpdatedAt.Date) <= (DateTo.Date))
            .Select(i => new InvoiceTransactionsVM
            {
                InvoiceId = i.InvoiceId,
                InvoiceRefNo = i.InvoiceRefNo,
                CreatedAt = i.CreatedAt,
                TotalAmount = i.TotalAmount,
                TotalDiscount = i.TotalDiscount,
                DiscountType = i.DiscountType,
                GrandTotal = i.GrandTotal,
                SubTotal = i.SubTotal,
                Due = i.Due,
                //SaleQuantity = i.InvoiceItems.Sum(it => (double?)it.Quantity) ?? 0,
                //ReturnedQty = i.InvoiceItems.Sum(it => (double?)it.ReturnedQuantity) ?? 0,
                PaymentDate = i.InvoicePayments.Where(p => p.IsActive).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault(),
                //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                TotalPaid = (double?)i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment) ?? 0,
                //TotalRefund = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)),
                Patient = i.Patient,
                PatientName = i.Patient.Name,
                IsProcedureInvoice = i.IsProcedureInvoice,
                UserName = i.User.UserName,
                PaymentStatus = i.PaymentStatus,
                EmployeeName = i.Employee.Name
            }).OrderByDescending(i => i.InvoiceId).ToPagedList(PageNo, PageSize);
        }

        public IPagedList<InvoiceTransactionsVM> GetTransactions_rpt(long InvoiceNo, int PageNo, int PageSize)
        {
            return
               (from i in cxt.Invoices
                join p in cxt.InvoicePayments on i equals p.Invoice
                where p.IsActive
                && i.IsActive && i.InvoiceRefNo == InvoiceNo
                select new InvoiceTransactionsVM
                {
                    InvoiceId = i.InvoiceId,
                    InvoiceRefNo = i.InvoiceRefNo,
                    CreatedAt = i.CreatedAt,
                    TotalAmount = i.TotalAmount,
                    TotalDiscount = i.TotalDiscount,
                    DiscountType = i.DiscountType,
                    GrandTotal = i.GrandTotal,
                    SubTotal = i.SubTotal,
                    Due = i.Due,
                    //SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                    //ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                    PaymentDate = p.PaymentDate,
                    PaymentCreatedAt = p.CreatedAt,
                    TotalPaid = p.Payment,
                    //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                    //TotalPaid = (double?)(i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment)) ?? 0,
                    //TotalRefund = (double?)(cxt.invoicePayments.Where(ip => ip.Invoice.InvoiceId == i.InvoiceId && ip.IsActive && ip.Payment < 0).Sum(pp => pp.Payment)) ?? 0,
                    Patient = i.Patient,
                    PatientName = i.Patient.Name,
                    IsProcedureInvoice = i.IsProcedureInvoice,
                    //InvoicePayments = i.InvoicePayments.ToList(),
                }).OrderByDescending(i => i.PaymentDate).ToPagedList(PageNo, PageSize);
            //.Select(i => new InvoiceTransactionsVM
            //{
            //    InvoiceId = i.InvoiceId,
            //    CreatedAt = i.CreatedAt,
            //    TotalAmount = i.TotalAmount,
            //    TotalDiscount = i.TotalDiscount,
            //    DiscountType = i.DiscountType,
            //    GrandTotal = i.GrandTotal,
            //    SubTotal = i.SubTotal,
            //    Due = i.Due,
            //    Advance = i.Advance,
            //    SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
            //    ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
            //    PaymentDate = (i.InvoicePayments.Where(p => p.IsActive).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
            //    //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
            //    TotalPaid = (double?)(i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment)) ?? 0,
            //    TotalRefund = (double?)(cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)) ?? 0,
            //    Patient = i.Patient,
            //    PatientName = i.Patient.Name,
            //    IsProcedureInvoice = i.IsProcedureInvoice

            //}).OrderByDescending(i => i.InvoiceId).ToPagedList(PageNo, PageSize);
        }
        public IPagedList<InvoiceTransactionsVM> GetTransactions(DateTime DateFrom, DateTime DateTo, long? UserId, long? EmployeeId, string CustomerName, long? invoiceNumber, int PageNo, int PageSize)
        {
            return cxt.Invoices
                .Where(i => i.IsActive && !i.IsHoldInvoice)
                .Where(i => (i.CreatedAt.Date) >= (DateFrom.Date))
                .Where(i => (i.CreatedAt.Date) <= (DateTo.Date))
                .Where(i => UserId.HasValue && i.UserId == UserId.Value || !UserId.HasValue)
                .Where(i => EmployeeId.HasValue && i.Employee.EmployeeId == EmployeeId.Value || !EmployeeId.HasValue)
                .Where(i => !string.IsNullOrEmpty(CustomerName) && i.Patient.Name.Contains(CustomerName) || string.IsNullOrEmpty(CustomerName))
                .Where(i => invoiceNumber.HasValue && i.InvoiceRefNo == invoiceNumber || !invoiceNumber.HasValue)

            .Select(i => new InvoiceTransactionsVM
            {
                InvoiceId = i.InvoiceId,
                InvoiceRefNo = i.InvoiceRefNo,
                CreatedAt = i.CreatedAt,
                TotalAmount = i.TotalAmount,
                TotalDiscount = i.TotalDiscount,
                DiscountType = i.DiscountType,
                GrandTotal = i.GrandTotal,
                SubTotal = i.SubTotal,
                Due = i.Due,
                //SaleQuantity = i.InvoiceItems.Sum(it => (double?)it.Quantity) ?? 0,
                //ReturnedQty = i.InvoiceItems.Sum(it => (double?)it.ReturnedQuantity) ?? 0,
                PaymentDate = i.InvoicePayments.Where(p => p.IsActive).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault(),
                //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                TotalPaid = (double?)i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment) ?? 0,
                //TotalRefund = -1 * (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)),
                Patient = i.Patient,
                PatientName = i.Patient.Name,
                IsProcedureInvoice = i.IsProcedureInvoice,
                UserName = i.User.UserName,
                PaymentStatus = i.PaymentStatus,
                EmployeeName = i.Employee.Name
            }).OrderByDescending(i => i.InvoiceId).ToPagedList(PageNo, PageSize);
        }
        public IPagedList<InvoiceTransactionsVM> GetTransactions(DateTime DateFrom, DateTime DateTo, int UserId, int PageNo, int PageSize)
        {
            return cxt.Invoices
            .Where(i => i.IsActive && i.User != null && i.User.UserId == UserId && !i.IsHoldInvoice)
             .Where(i => (i.CreatedAt.Date) >= (DateFrom.Date)
                    && (i.CreatedAt.Date) <= (DateTo.Date))
            .Select(i => new InvoiceTransactionsVM
            {
                InvoiceId = i.InvoiceId,
                InvoiceRefNo = i.InvoiceRefNo,
                CreatedAt = i.CreatedAt,
                TotalAmount = i.TotalAmount,
                TotalDiscount = i.TotalDiscount,
                DiscountType = i.DiscountType,
                GrandTotal = i.GrandTotal,
                SubTotal = i.SubTotal,
                Due = i.Due,
                //SaleQuantity = i.InvoiceItems.Sum(it => (double?)it.Quantity) ?? 0,
                //ReturnedQty = i.InvoiceItems.Sum(it => (double?)it.ReturnedQuantity) ?? 0,
                PaymentDate = i.InvoicePayments.Where(p => p.IsActive).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault(),
                //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                TotalPaid = (double?)i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment) ?? 0,
                //TotalRefund = -1 * (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)),
                Patient = i.Patient,
                PatientName = i.Patient.Name,
                IsProcedureInvoice = i.IsProcedureInvoice,
                UserName = i.User.UserName,
                PaymentStatus = i.PaymentStatus,
                EmployeeName = i.Employee.Name
            }).OrderByDescending(i => i.InvoiceId).ToPagedList(PageNo, PageSize);
        }
        public IPagedList<InvoiceTransactionsVM> GetTransactions_Financial(DateTime DateFrom, DateTime DateTo, int PaymentMode, int PageNo, int PageSize)
        {
            if (PaymentMode == 0)
            {
                return cxt.Invoices
                .Where(i => i.IsActive)
                .Where(i => (i.CreatedAt.Date) >= (DateFrom.Date)
                    && (i.CreatedAt.Date) <= (DateTo.Date)
                )
                .Select(i => new InvoiceTransactionsVM
                {
                    InvoiceId = i.InvoiceId,
                    InvoiceRefNo = i.InvoiceRefNo,
                    CreatedAt = i.CreatedAt,
                    TotalAmount = i.TotalAmount,
                    TotalDiscount = i.TotalDiscount,
                    DiscountType = i.DiscountType,
                    GrandTotal = i.GrandTotal,
                    SubTotal = i.SubTotal,
                    Due = i.Due,
                    //SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                    //ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                    IsProcedureInvoice = i.IsProcedureInvoice,
                    //InvoiceItems = i.InvoiceItems.Where(ii => ii.IsActive).Select(item => new InvoiceItemVM
                    //{
                    //    //IsActive = item.IsActive,
                    //    //InvoiceItemId = item.InvoiceItemId,
                    //    //ItemId = item.Item.ItemId,
                    //    ItemName = item.Item.ItemName,
                    //    //Rate = item.Rate,
                    //    //Quantity = item.Quantity,
                    //    //Discount = item.Discount,
                    //    //DiscountType = item.DiscountType,
                    //    //DiscountTypeString = item.DiscountType == 1 ? "Value" : "%",
                    //    //Amount = item.Amount,
                    //    //BatchId = item.Batch.BatchId,
                    //    //BatchName = item.Batch.BatchName,
                    //    //ReturnedQuantity = item.ReturnedQuantity
                    //}).ToList(),
                    //PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                    //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                    //TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
                    //TotalRefund = -1 * (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)),
                    InvoicePayments = i.InvoicePayments.Where(p => p.IsActive).OrderByDescending(p => p.InvoicePaymentId).ToList(),
                    Patient = i.Patient,
                    PatientName = i.Patient.Name
                }).OrderByDescending(i => i.InvoiceId).ToPagedList(PageNo, PageSize);
            }
            else
            {
                return cxt.Invoices
                .Where(i => i.IsActive)
                .Where(i => (i.CreatedAt.Date) >= (DateFrom.Date)
                    && (i.CreatedAt.Date) <= (DateTo.Date)
                )
                .Select(i => new InvoiceTransactionsVM
                {
                    InvoiceId = i.InvoiceId,
                    CreatedAt = i.CreatedAt,
                    TotalAmount = i.TotalAmount,
                    TotalDiscount = i.TotalDiscount,
                    DiscountType = i.DiscountType,
                    GrandTotal = i.GrandTotal,
                    SubTotal = i.SubTotal,
                    Due = i.Due,
                    //SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                    //ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                    //InvoiceItems = i.InvoiceItems.Where(ii => ii.IsActive).Select(item => new InvoiceItemVM
                    //{
                    //    //IsActive = item.IsActive,
                    //    //InvoiceItemId = item.InvoiceItemId,
                    //    //ItemId = item.Item.ItemId,
                    //    ItemName = item.Item.ItemName,
                    //    //Rate = item.Rate,
                    //    //Quantity = item.Quantity,
                    //    //Discount = item.Discount,
                    //    //DiscountType = item.DiscountType,
                    //    //DiscountTypeString = item.DiscountType == 1 ? "Value" : "%",
                    //    //Amount = item.Amount,
                    //    //BatchId = item.Batch.BatchId,
                    //    //BatchName = item.Batch.BatchName,
                    //    //ReturnedQuantity = item.ReturnedQuantity
                    //}).ToList(),
                    //PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                    //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                    //TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
                    //TotalRefund = -1 * (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)),
                    InvoicePayments = i.InvoicePayments.Where(p => p.IsActive && p.PaymentType == PaymentMode).ToList(),
                    Patient = i.Patient,
                    PatientName = i.Patient.Name
                }).OrderByDescending(i => i.InvoiceId).ToPagedList(PageNo, PageSize);
            }
        }
        public IPagedList<InvoiceTransactionsVM> GetTransactionsByInvNo_Financial(long InvoiceNo, int PaymentMode, int PageNo, int PageSize)
        {
            if (PaymentMode == 0)
            {
                return cxt.Invoices
                .Where(i => i.IsActive && i.InvoiceId == InvoiceNo)
                .Select(i => new InvoiceTransactionsVM
                {
                    InvoiceId = i.InvoiceId,
                    InvoiceRefNo = i.InvoiceRefNo,
                    CreatedAt = i.CreatedAt,
                    TotalAmount = i.TotalAmount,
                    TotalDiscount = i.TotalDiscount,
                    DiscountType = i.DiscountType,
                    GrandTotal = i.GrandTotal,
                    SubTotal = i.SubTotal,
                    Due = i.Due,
                    //SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                    //ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                    IsProcedureInvoice = i.IsProcedureInvoice,
                    //InvoiceItems = i.InvoiceItems.Where(ii => ii.IsActive).Select(item => new InvoiceItemVM
                    //{
                    //    //IsActive = item.IsActive,
                    //    //InvoiceItemId = item.InvoiceItemId,
                    //    //ItemId = item.Item.ItemId,
                    //    ItemName = item.Item.ItemName,
                    //    //Rate = item.Rate,
                    //    //Quantity = item.Quantity,
                    //    //Discount = item.Discount,
                    //    //DiscountType = item.DiscountType,
                    //    //DiscountTypeString = item.DiscountType == 1 ? "Value" : "%",
                    //    //Amount = item.Amount,
                    //    //BatchId = item.Batch.BatchId,
                    //    //BatchName = item.Batch.BatchName,
                    //    //ReturnedQuantity = item.ReturnedQuantity
                    //}).ToList(),
                    //PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                    //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                    //TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
                    //TotalRefund = -1 * (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)),
                    InvoicePayments = i.InvoicePayments.Where(p => p.IsActive).OrderByDescending(p => p.InvoicePaymentId).ToList(),
                    Patient = i.Patient,
                    PatientName = i.Patient.Name
                }).OrderByDescending(i => i.InvoiceId).ToPagedList(PageNo, PageSize);
            }
            else
            {
                return cxt.Invoices
                .Where(i => i.IsActive && i.InvoiceId == InvoiceNo)
                .Select(i => new InvoiceTransactionsVM
                {
                    InvoiceId = i.InvoiceId,
                    CreatedAt = i.CreatedAt,
                    TotalAmount = i.TotalAmount,
                    TotalDiscount = i.TotalDiscount,
                    DiscountType = i.DiscountType,
                    GrandTotal = i.GrandTotal,
                    SubTotal = i.SubTotal,
                    Due = i.Due,
                    //SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                    //ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                    //InvoiceItems = i.InvoiceItems.Where(ii => ii.IsActive).Select(item => new InvoiceItemVM
                    //{
                    //    //IsActive = item.IsActive,
                    //    //InvoiceItemId = item.InvoiceItemId,
                    //    //ItemId = item.Item.ItemId,
                    //    ItemName = item.Item.ItemName,
                    //    //Rate = item.Rate,
                    //    //Quantity = item.Quantity,
                    //    //Discount = item.Discount,
                    //    //DiscountType = item.DiscountType,
                    //    //DiscountTypeString = item.DiscountType == 1 ? "Value" : "%",
                    //    //Amount = item.Amount,
                    //    //BatchId = item.Batch.BatchId,
                    //    //BatchName = item.Batch.BatchName,
                    //    //ReturnedQuantity = item.ReturnedQuantity
                    //}).ToList(),
                    //PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                    //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                    //TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
                    //TotalRefund = -1 * (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)),
                    InvoicePayments = i.InvoicePayments.Where(p => p.IsActive && p.PaymentType == PaymentMode).ToList(),
                    Patient = i.Patient,
                    PatientName = i.Patient.Name
                }).OrderByDescending(i => i.InvoiceId).ToPagedList(PageNo, PageSize);
            }
        }
        public List<SummationsVM> GetTransactions_Financial_Graph(DateTime DateFrom, DateTime DateTo, int PaymentMode)
        {
            List<SummationsVM> Result = new List<SummationsVM>();
            if (PaymentMode == 0)
            {
                Result = (from i in cxt.Invoices
                          where i.IsActive
                          && (i.CreatedAt.Date) >= (DateFrom.Date)
                          && (i.CreatedAt.Date) <= (DateTo.Date)
                          group i by new { Date = (i.CreatedAt.Date) } into g
                          select new SummationsVM
                          {
                              Date = g.Key.Date,
                              MaxMonthRevenue = g.Sum(p => p.InvoicePayments.Where(pmt => pmt.IsActive).Sum(pmt => pmt.Payment))
                          }
                              ).ToList();
            }
            else
            {
                Result = (from i in cxt.Invoices
                          where i.IsActive
                          && (i.CreatedAt.Date) >= (DateFrom.Date)
                          && (i.CreatedAt.Date) <= (DateTo.Date)
                          group i by new { Date = (i.CreatedAt.Date) } into g
                          select new SummationsVM
                          {
                              Date = g.Key.Date,
                              MaxMonthRevenue = g.Sum(p => p.InvoicePayments.Where(pmt => pmt.IsActive).Sum(pmt => pmt.Payment))
                          }
                              ).ToList();
            }
            return Result;
        }
        public List<SummationsVM> GetTransactionsByInvNo_Financial_Graph(long InvoiceNo, int PaymentMode)
        {
            List<SummationsVM> Result = new List<SummationsVM>();
            if (PaymentMode == 0)
            {
                Result = (from i in cxt.Invoices
                          where i.IsActive
                          && i.InvoiceId == InvoiceNo
                          group i by new { Date = (i.CreatedAt.Date) } into g
                          select new SummationsVM
                          {
                              Date = g.Key.Date,
                              MaxMonthRevenue = g.Sum(p => p.InvoicePayments.Where(pmt => pmt.IsActive).Sum(pmt => pmt.Payment))
                          }
                              ).ToList();
            }
            else
            {
                Result = (from i in cxt.Invoices
                          where i.IsActive
                          && i.InvoiceId == InvoiceNo
                          group i by new { Date = (i.CreatedAt.Date) } into g
                          select new SummationsVM
                          {
                              Date = g.Key.Date,
                              MaxMonthRevenue = g.Sum(p => p.InvoicePayments.Where(pmt => pmt.IsActive).Sum(pmt => pmt.Payment))
                          }
                              ).ToList();
            }
            return Result;
        }
        public List<InvoiceTransactionsVM> GetTransactions_Financial_Reports(DateTime DateFrom, DateTime DateTo, int PaymentMode)
        {
            if (PaymentMode == 0)
            {
                return cxt.Invoices
                .Where(i => i.IsActive)
                .Where(i => (i.CreatedAt.Date) >= (DateFrom.Date)
                    && (i.CreatedAt.Date) <= (DateTo.Date)
                )
                .Select(i => new InvoiceTransactionsVM
                {
                    InvoiceId = i.InvoiceId,
                    CreatedAt = i.CreatedAt,
                    TotalAmount = i.TotalAmount,
                    TotalDiscount = i.TotalDiscount,
                    DiscountType = i.DiscountType,
                    GrandTotal = i.GrandTotal,
                    SubTotal = i.SubTotal,
                    Due = i.Due,
                    //SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                    //ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                    IsProcedureInvoice = i.IsProcedureInvoice,
                    //InvoiceItems = i.InvoiceItems.Where(ii => ii.IsActive).Select(item => new InvoiceItemVM
                    //{
                    //    //IsActive = item.IsActive,
                    //    //InvoiceItemId = item.InvoiceItemId,
                    //    //ItemId = item.Item.ItemId,
                    //    ItemName = item.Item.ItemName,
                    //    //Rate = item.Rate,
                    //    //Quantity = item.Quantity,
                    //    //Discount = item.Discount,
                    //    //DiscountType = item.DiscountType,
                    //    //DiscountTypeString = item.DiscountType == 1 ? "Value" : "%",
                    //    //Amount = item.Amount,
                    //    //BatchId = item.Batch.BatchId,
                    //    //BatchName = item.Batch.BatchName,
                    //    //ReturnedQuantity = item.ReturnedQuantity
                    //}).ToList(),
                    //PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                    //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                    //TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
                    //TotalRefund = -1 * (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)),
                    InvoicePayments = i.InvoicePayments.Where(p => p.IsActive).ToList(),
                    Patient = i.Patient,
                    PatientName = i.Patient.Name
                }).OrderByDescending(i => i.InvoiceId).ToList();
            }
            else
            {
                return cxt.Invoices
                .Where(i => i.IsActive)
                .Where(i => (i.CreatedAt.Date) >= (DateFrom.Date)
                    && (i.CreatedAt.Date) <= (DateTo.Date)
                )
                .Select(i => new InvoiceTransactionsVM
                {
                    InvoiceId = i.InvoiceId,
                    CreatedAt = i.CreatedAt,
                    TotalAmount = i.TotalAmount,
                    TotalDiscount = i.TotalDiscount,
                    DiscountType = i.DiscountType,
                    GrandTotal = i.GrandTotal,
                    SubTotal = i.SubTotal,
                    Due = i.Due,
                    //SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                    //ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                    //InvoiceItems = i.InvoiceItems.Where(ii => ii.IsActive).Select(item => new InvoiceItemVM
                    //{
                    //    //IsActive = item.IsActive,
                    //    //InvoiceItemId = item.InvoiceItemId,
                    //    //ItemId = item.Item.ItemId,
                    //    ItemName = item.Item.ItemName,
                    //    //Rate = item.Rate,
                    //    //Quantity = item.Quantity,
                    //    //Discount = item.Discount,
                    //    //DiscountType = item.DiscountType,
                    //    //DiscountTypeString = item.DiscountType == 1 ? "Value" : "%",
                    //    //Amount = item.Amount,
                    //    //BatchId = item.Batch.BatchId,
                    //    //BatchName = item.Batch.BatchName,
                    //    //ReturnedQuantity = item.ReturnedQuantity
                    //}).ToList(),
                    //PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                    //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                    //TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
                    //TotalRefund = -1 * (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)),
                    InvoicePayments = i.InvoicePayments.Where(p => p.IsActive && p.PaymentType == PaymentMode).ToList(),
                    Patient = i.Patient,
                    PatientName = i.Patient.Name
                }).OrderByDescending(i => i.InvoiceId).ToList();
            }
        }
        public SummationsVM GetSums_Financial(DateTime DateFrom, DateTime DateTo)
        {
            SummationsVM Result = new SummationsVM();
            Result = (from i in cxt.Invoices
                      where i.IsActive
                      && (i.CreatedAt.Date) >= (DateFrom.Date)
                      && (i.CreatedAt.Date) <= (DateTo.Date)
                      group i by new { i.CreatedAt.Year, i.CreatedAt.Month } into g
                      select new SummationsVM
                      {
                          Year = g.Key.Year,
                          Month = g.Key.Month,
                          MaxMonthRevenue = g.Sum(p => p.InvoicePayments.Where(pmt => pmt.IsActive).Sum(pmt => pmt.Payment))
                      }
                          ).OrderByDescending(r => r.MaxMonthRevenue).FirstOrDefault();

            var Res = cxt.Invoices
                       .Where(i => i.IsActive
                        && (i.CreatedAt.Date) >= (DateFrom.Date)
                        && (i.CreatedAt.Date) <= (DateTo.Date)).ToList();
            if (Res.Any())
            {
                Result.TotalRevenue = cxt.Invoices
                        .Where(i => i.IsActive
                         && (i.CreatedAt.Date) >= (DateFrom.Date)
                         && (i.CreatedAt.Date) <= (DateTo.Date))
                        .Sum(r => r.InvoicePayments.DefaultIfEmpty().Where(p => p.IsActive).Sum(p => p.Payment));
            }
            return Result;
        }
        public SummationsVM GetSums_Financial(long InvoiceNo)
        {
            SummationsVM Result = new SummationsVM();
            //Result = (from i in cxt.Invoices
            //          where i.IsActive
            //          && i.InvoiceId == InvoiceNo
            //          group i by new { Year = i.CreatedAt.Year, Month = i.CreatedAt.Month } into g
            //          select new SummationsVM
            //          {
            //              Year = g.Key.Year,
            //              Month = g.Key.Month,
            //              MaxMonthRevenue = g.Sum(p => p.InvoicePayments.Where(pmt => pmt.IsActive).Sum(pmt => pmt.Payment))
            //          }
            //              ).OrderByDescending(r => r.MaxMonthRevenue).FirstOrDefault();

            Result.TotalRevenue = cxt.Invoices
                       .Where(i => i.IsActive
                        && i.InvoiceId == InvoiceNo)
                        .Sum(r => r.InvoicePayments.Where(p => p.IsActive).Sum(p => p.Payment));
            return Result;
        }
        public List<SummationsVM> GetPayment_Financial_Graph(DateTime DateFrom, DateTime DateTo, int PaymentMode)
        {
            List<SummationsVM> Result = new List<SummationsVM>();
            if (PaymentMode == 0)
            {
                Result = (from i in cxt.Invoices
                          join p in cxt.InvoicePayments on i equals p.Invoice
                          where i.IsActive
                          && (i.CreatedAt.Date) >= (DateFrom.Date)
                          && (i.CreatedAt.Date) <= (DateTo.Date)
                          group i by new { p.PaymentType } into g
                          select new SummationsVM
                          {
                              PaymentType = g.Key.PaymentType,
                              TotalRevenue = g.Sum(p => p.InvoicePayments.Where(pmt => pmt.IsActive).Sum(pmt => pmt.Payment))
                          }
                              ).ToList();
            }
            else
            {
                Result = (from i in cxt.Invoices
                          where i.IsActive
                          && (i.CreatedAt.Date) >= (DateFrom.Date)
                          && (i.CreatedAt.Date) <= (DateTo.Date)
                          group i by new { Date = (i.CreatedAt.Date) } into g
                          select new SummationsVM
                          {
                              Date = g.Key.Date,
                              MaxMonthRevenue = g.Sum(p => p.InvoicePayments.Where(pmt => pmt.IsActive).Sum(pmt => pmt.Payment))
                          }
                              ).ToList();
            }
            return Result;
        }

        public SummationsVM GetRetailValueOfAvailableStock()
        {
            SummationsVM res = new SummationsVM
            {
                RV_T_Stock = (from si in cxt.StockItems where si.IsActive select new { Quantity = si.Quantity + si.BonusQuantity, RetailPrice = (from rp in cxt.StockItems where rp.Item.ItemId == si.Item.ItemId where rp.IsActive orderby rp.StockItemId descending select rp.RetailPrice).FirstOrDefault() }).Sum(si => (double?)(si.Quantity * si.RetailPrice)) ?? 0,
                RV_T_C_Stock = (from sc in cxt.StockConsumptionItems where sc.IsActive select new { sc.Quantity, RetailPrice = (from si in cxt.StockItems where si.Item.ItemId == sc.Item.ItemId && si.IsActive orderby si.StockItemId descending select si.RetailPrice).FirstOrDefault() }).Sum(r => (double?)(r.Quantity * r.RetailPrice)) ?? 0,
                RV_T_A_Stock = (from ai in cxt.AdjustmentItems where ai.IsActive select new { ai.Quantity, RetailPrice = (from si in cxt.StockItems where si.Item.ItemId == ai.Item.ItemId && si.IsActive orderby si.StockItemId descending select si.RetailPrice).FirstOrDefault() }).Sum(r => (double?)(r.Quantity * r.RetailPrice)) ?? 0,
                //RV_T_E_Stock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && b.Expiry != null && (b.Expiry) < (DateTime.Now) orderby s.StockItemId descending select new { Quantity = s.Quantity + s.BonusQuantity, RetailPrice = (from si in cxt.StockItems where si.Item.ItemId == s.Item.ItemId && si.IsActive orderby s.StockItemId descending select si.RetailPrice).FirstOrDefault() }).Sum(r => (double?)(r.Quantity * r.RetailPrice)) ?? 0,
                //RV_T_C_E_Stock = (from s in cxt.StockConsumptionItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && b.Expiry != null && (b.Expiry) < (DateTime.Now) orderby s.StockConsumptionItemId descending select new { Quantity = s.Quantity, RetailPrice = (from si in cxt.StockItems where si.Item.ItemId == s.Item.ItemId && si.IsActive orderby si.StockItemId descending select si.RetailPrice).FirstOrDefault() }).Sum(r => (double?)(r.Quantity * r.RetailPrice)) ?? 0,

                //CV_T_Stock = (cxt.StockItems.Where(si => si.IsActive).Sum(si => ((int?)si.Quantity + si.BonusQuantity) * si.UnitCost) ?? 0),
                //CV_T_Stock = (from si in cxt.StockItems where si.IsActive select new { Quantity = si.Quantity + si.BonusQuantity, CostPrice = (from im in cxt.StockItems where im.IsActive && im.Item.ItemId == si.Item.ItemId orderby im.StockItemId descending select im.UnitCost).FirstOrDefault() }).Sum(si => (double?)(si.Quantity * si.CostPrice)) ?? 0,
                //CV_T_C_Stock = (from sc in cxt.StockConsumptionItems where sc.IsActive select new { Quantity = sc.Quantity, CostPrice = (from si in cxt.StockItems where si.IsActive && si.Item.ItemId == sc.Item.ItemId orderby si.StockItemId descending select si.UnitCost).FirstOrDefault() }).Sum(r => (double?)(r.Quantity * r.CostPrice)) ?? 0,
                //CV_T_A_Stock = (from ai in cxt.AdjustmentItems where ai.IsActive select new { Quantity = ai.Quantity, CostPrice = (from si in cxt.StockItems where si.IsActive && si.Item.ItemId == ai.Item.ItemId orderby si.StockItemId descending select si.UnitCost).FirstOrDefault() }).Sum(r => (double?)(r.Quantity * r.CostPrice)) ?? 0,
                //CV_T_E_Stock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && b.Expiry != null && (b.Expiry) < (DateTime.Now) orderby s.StockItemId descending select new { Quantity = s.Quantity + s.BonusQuantity, UnitCost = (from si in cxt.StockItems where si.IsActive && si.Item.ItemId == s.Item.ItemId orderby s.StockItemId descending select si.UnitCost).FirstOrDefault() }).Sum(r => (double?)(r.Quantity * r.UnitCost)) ?? 0,
                //CV_T_C_E_Stock = (from s in cxt.StockConsumptionItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && b.Expiry != null && (b.Expiry) < (DateTime.Now) orderby s.StockConsumptionItemId descending select new { Quantity = s.Quantity, UnitCost = (from si in cxt.StockItems where si.IsActive && si.Item.ItemId == s.Item.ItemId orderby si.StockItemId descending select si.UnitCost).FirstOrDefault() }).Sum(r => (double?)(r.Quantity * r.UnitCost)) ?? 0,
                //TotalRevenue = (cxt.InvoicePayments.Where(p => p.IsActive)).Sum(r => (double?)r.Payment) ?? 0
            };
            res.RetailValueOfAvailableStock = res.RV_T_Stock - res.RV_T_C_Stock + res.RV_T_A_Stock;// -(res.RV_T_E_Stock - res.RV_T_C_E_Stock);
            //res.CostOfAvailableStock = res.CV_T_Stock - res.CV_T_C_Stock + res.CV_T_A_Stock - (res.CV_T_E_Stock - res.CV_T_C_E_Stock);

            return res;
        }
        public SummationsVM GetSums()
        {
            SummationsVM res = new SummationsVM
            {
                //RV_T_Stock = (cxt.StockItems.Where(si => si.IsActive).Sum(si => ((int?)si.Quantity + si.BonusQuantity) * si.RetailPrice) ?? 0),
                //RV_T_Stock = (from si in cxt.StockItems where si.IsActive select new { Quantity = si.Quantity + si.BonusQuantity, RetailPrice = (from im in cxt.StockItems where im.IsActive && im.Item.ItemId == si.Item.ItemId && im.Batch.BatchId == si.Batch.BatchId orderby im.StockItemId descending select im.RetailPrice).FirstOrDefault() }).Sum(si => (double?)(si.Quantity * si.RetailPrice)) ?? 0,
                //RV_T_C_Stock = (from sc in cxt.StockConsumptionItems where sc.IsActive select new { Quantity = sc.Quantity, RetailPrice = (from si in cxt.StockItems where si.Item.ItemId == sc.Item.ItemId && si.Batch.BatchId == sc.Batch.BatchId orderby si.StockItemId descending select si.RetailPrice).FirstOrDefault() }).Sum(r => (double?)(r.Quantity * r.RetailPrice)) ?? 0,
                //RV_T_A_Stock = (from ai in cxt.AdjustmentItems where ai.IsActive select new { Quantity = ai.Quantity, RetailPrice = (from si in cxt.StockItems where si.Item.ItemId == ai.Item.ItemId && si.Batch.BatchId == ai.Batch.BatchId orderby si.StockItemId descending select si.RetailPrice).FirstOrDefault() }).Sum(r => (double?)(r.Quantity * r.RetailPrice)) ?? 0,
                //RV_T_E_Stock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && b.Expiry != null && (b.Expiry) < (DateTime.Now) orderby s.StockItemId descending select new { Quantity = s.Quantity + s.BonusQuantity, RetailPrice = (from si in cxt.StockItems where si.Item.ItemId == s.Item.ItemId && s.Batch.BatchId == si.Batch.BatchId orderby s.StockItemId descending select si.RetailPrice).FirstOrDefault() }).Sum(r => (double?)(r.Quantity * r.RetailPrice)) ?? 0,
                //RV_T_C_E_Stock =  (from s in cxt.StockConsumptionItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && b.Expiry != null && (b.Expiry) < (DateTime.Now) orderby s.StockConsumptionItemId descending select new { Quantity = s.Quantity, RetailPrice = (from si in cxt.StockItems where si.Item.ItemId == s.Item.ItemId && s.Batch.BatchId == si.Batch.BatchId orderby si.StockItemId descending select si.RetailPrice).FirstOrDefault() }).Sum(r => (double?)(r.Quantity * r.RetailPrice)) ?? 0,

                ////CV_T_Stock = (cxt.StockItems.Where(si => si.IsActive).Sum(si => ((int?)si.Quantity + si.BonusQuantity) * si.UnitCost) ?? 0),
                //CV_T_Stock = (from si in cxt.StockItems where si.IsActive select new { Quantity = si.Quantity + si.BonusQuantity, CostPrice = (from im in cxt.StockItems where im.IsActive && im.Item.ItemId == si.Item.ItemId && im.Batch.BatchId == si.Batch.BatchId orderby im.StockItemId descending select im.UnitCost).FirstOrDefault() }).Sum(si => (double?)(si.Quantity * si.CostPrice)) ?? 0,
                //CV_T_C_Stock = (from sc in cxt.StockConsumptionItems where sc.IsActive select new { Quantity = sc.Quantity, CostPrice = (from si in cxt.StockItems where si.Item.ItemId == sc.Item.ItemId && si.Batch.BatchId == sc.Batch.BatchId orderby si.StockItemId descending select si.UnitCost).FirstOrDefault() }).Sum(r => (double?)(r.Quantity * r.CostPrice)) ?? 0,
                //CV_T_A_Stock = (from ai in cxt.AdjustmentItems where ai.IsActive select new { Quantity = ai.Quantity, CostPrice = (from si in cxt.StockItems where si.Item.ItemId == ai.Item.ItemId && si.Batch.BatchId == ai.Batch.BatchId orderby si.StockItemId descending select si.UnitCost).FirstOrDefault() }).Sum(r => (double?)(r.Quantity * r.CostPrice)) ?? 0,
                //CV_T_E_Stock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && b.Expiry != null && (b.Expiry) < (DateTime.Now) orderby s.StockItemId descending select new { Quantity = s.Quantity + s.BonusQuantity, UnitCost = (from si in cxt.StockItems where si.Item.ItemId == s.Item.ItemId && s.Batch.BatchId == si.Batch.BatchId orderby s.StockItemId descending select si.UnitCost).FirstOrDefault() }).Sum(r => (double?)(r.Quantity * r.UnitCost)) ?? 0,
                //CV_T_C_E_Stock =(from s in cxt.StockConsumptionItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && b.Expiry != null && (b.Expiry) < (DateTime.Now) orderby s.StockConsumptionItemId descending select new { Quantity = s.Quantity, UnitCost = (from si in cxt.StockItems where si.Item.ItemId == s.Item.ItemId && s.Batch.BatchId == si.Batch.BatchId orderby si.StockItemId descending select si.UnitCost).FirstOrDefault() }).Sum(r => (double?)(r.Quantity * r.UnitCost)) ?? 0,
                //TotalRevenue = (cxt.invoicePayments.Where(p => p.IsActive)).Sum(r => (double?)r.Payment) ?? 0
                RV_T_Stock = (from si in cxt.StockItems where si.IsActive select new { Quantity = si.Quantity + si.BonusQuantity, RetailPrice = (from rp in cxt.StockItems where rp.Item.ItemId == si.Item.ItemId where rp.IsActive orderby rp.StockItemId descending select rp.RetailPrice).FirstOrDefault() }).Sum(si => (double?)(si.Quantity * si.RetailPrice)) ?? 0,
                RV_T_C_Stock = (from sc in cxt.StockConsumptionItems where sc.IsActive select new { sc.Quantity, RetailPrice = (from si in cxt.StockItems where si.Item.ItemId == sc.Item.ItemId && si.IsActive orderby si.StockItemId descending select si.RetailPrice).FirstOrDefault() }).Sum(r => (double?)(r.Quantity * r.RetailPrice)) ?? 0,
                RV_T_A_Stock = (from ai in cxt.AdjustmentItems where ai.IsActive select new { ai.Quantity, RetailPrice = (from si in cxt.StockItems where si.Item.ItemId == ai.Item.ItemId && si.IsActive orderby si.StockItemId descending select si.RetailPrice).FirstOrDefault() }).Sum(r => (double?)(r.Quantity * r.RetailPrice)) ?? 0,
                RV_T_E_Stock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && b.Expiry != null && (b.Expiry) < (DateTime.Now) orderby s.StockItemId descending select new { Quantity = s.Quantity + s.BonusQuantity, RetailPrice = (from si in cxt.StockItems where si.Item.ItemId == s.Item.ItemId && si.IsActive orderby s.StockItemId descending select si.RetailPrice).FirstOrDefault() }).Sum(r => (double?)(r.Quantity * r.RetailPrice)) ?? 0,
                RV_T_C_E_Stock = (from s in cxt.StockConsumptionItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && b.Expiry != null && (b.Expiry) < (DateTime.Now) orderby s.StockConsumptionItemId descending select new { s.Quantity, RetailPrice = (from si in cxt.StockItems where si.Item.ItemId == s.Item.ItemId && si.IsActive orderby si.StockItemId descending select si.RetailPrice).FirstOrDefault() }).Sum(r => (double?)(r.Quantity * r.RetailPrice)) ?? 0,

                //CV_T_Stock = (cxt.StockItems.Where(si => si.IsActive).Sum(si => ((int?)si.Quantity + si.BonusQuantity) * si.UnitCost) ?? 0),
                CV_T_Stock = (from si in cxt.StockItems where si.IsActive select new { Quantity = si.Quantity + si.BonusQuantity, CostPrice = (from im in cxt.StockItems where im.IsActive && im.Item.ItemId == si.Item.ItemId orderby im.StockItemId descending select im.UnitCost).FirstOrDefault() }).Sum(si => (double?)(si.Quantity * si.CostPrice)) ?? 0,
                CV_T_C_Stock = (from sc in cxt.StockConsumptionItems where sc.IsActive select new { sc.Quantity, CostPrice = (from si in cxt.StockItems where si.IsActive && si.Item.ItemId == sc.Item.ItemId orderby si.StockItemId descending select si.UnitCost).FirstOrDefault() }).Sum(r => (double?)(r.Quantity * r.CostPrice)) ?? 0,
                CV_T_A_Stock = (from ai in cxt.AdjustmentItems where ai.IsActive select new { ai.Quantity, CostPrice = (from si in cxt.StockItems where si.IsActive && si.Item.ItemId == ai.Item.ItemId orderby si.StockItemId descending select si.UnitCost).FirstOrDefault() }).Sum(r => (double?)(r.Quantity * r.CostPrice)) ?? 0,
                CV_T_E_Stock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && b.Expiry != null && (b.Expiry) < (DateTime.Now) orderby s.StockItemId descending select new { Quantity = s.Quantity + s.BonusQuantity, UnitCost = (from si in cxt.StockItems where si.IsActive && si.Item.ItemId == s.Item.ItemId orderby s.StockItemId descending select si.UnitCost).FirstOrDefault() }).Sum(r => (double?)(r.Quantity * r.UnitCost)) ?? 0,
                CV_T_C_E_Stock = (from s in cxt.StockConsumptionItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && b.Expiry != null && (b.Expiry) < (DateTime.Now) orderby s.StockConsumptionItemId descending select new { s.Quantity, UnitCost = (from si in cxt.StockItems where si.IsActive && si.Item.ItemId == s.Item.ItemId orderby si.StockItemId descending select si.UnitCost).FirstOrDefault() }).Sum(r => (double?)(r.Quantity * r.UnitCost)) ?? 0,
                TotalRevenue = cxt.InvoicePayments.Where(p => p.IsActive).Sum(r => (double?)r.Payment) ?? 0
            };
            res.RetailValueOfAvailableStock = res.RV_T_Stock - res.RV_T_C_Stock + res.RV_T_A_Stock - (res.RV_T_E_Stock - res.RV_T_C_E_Stock);
            res.CostOfAvailableStock = res.CV_T_Stock - res.CV_T_C_Stock + res.CV_T_A_Stock - (res.CV_T_E_Stock - res.CV_T_C_E_Stock);
            return res;
        }
        public SummationsVM GetSums(DateTime DateFrom, DateTime DateTo)
        {
            SummationsVM res = new SummationsVM
            {
                TotalRevenue = cxt.InvoicePayments.Where(p => p.IsActive && (p.UpdatedAt) >= (DateFrom.Date) && (p.UpdatedAt) <= (DateTo.Date)).Sum(r => (double?)r.Payment) ?? 0
            };
            return res;
        }
        public SummationsVM GetSums(long InvoiceNo)
        {
            SummationsVM res = new SummationsVM
            {
                TotalRevenue = cxt.InvoicePayments.Where(p => p.IsActive && p.Invoice.InvoiceRefNo == InvoiceNo).Sum(r => (double?)r.Payment) ?? 0
            };
            return res;
        }
        public IPagedList<InvoiceTransactionsVM> GetPendingPayments_Financial(DateTime DateFrom, DateTime DateTo, int PageNo, int PageSize)
        {
            return cxt.Invoices
            .Where(i => i.IsActive)
            .Where(i => (i.CreatedAt.Date) >= (DateFrom.Date)
                && (i.CreatedAt.Date) <= (DateTo.Date)
                && i.Due > 0
            )
            .Select(i => new InvoiceTransactionsVM
            {
                InvoiceId = i.InvoiceId,
                CreatedAt = i.CreatedAt,
                TotalAmount = i.TotalAmount,
                TotalDiscount = i.TotalDiscount,
                DiscountType = i.DiscountType,
                GrandTotal = i.GrandTotal,
                SubTotal = i.SubTotal,
                Due = i.Due,
                //SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                //ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                //IsProcedureInvoice = i.IsProcedureInvoice,
                //InvoiceItems = (i.InvoiceItems.Where(ii => ii.IsActive).Select(item => new InvoiceItemVM
                //{
                //IsActive = item.IsActive,
                //InvoiceItemId = item.InvoiceItemId,
                //ItemId = item.Item.ItemId,
                //ItemName = item.Item.ItemName,
                //Rate = item.Rate,
                //Quantity = item.Quantity,
                //Discount = item.Discount,
                //DiscountType = item.DiscountType,
                //DiscountTypeString = item.DiscountType == 1 ? "Value" : "%",
                //Amount = item.Amount,
                //BatchId = item.Batch.BatchId,
                //BatchName = item.Batch.BatchName,
                //ReturnedQuantity = item.ReturnedQuantity
                //})).ToList(),
                //PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                //TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
                //TotalRefund = -1 * (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)),
                //InvoicePayments = i.InvoicePayments.Where(p => p.IsActive).ToList(),
                Patient = i.Patient,
                PatientName = i.Patient.Name
            }).OrderByDescending(i => i.InvoiceId).ToPagedList(PageNo, PageSize);
        }
        public List<InvoiceTransactionsVM> GetPendingPayments_Financial_Reports(DateTime DateFrom, DateTime DateTo)
        {
            return cxt.Invoices
            .Where(i => i.IsActive)
            .Where(i => (i.CreatedAt.Date) >= (DateFrom.Date)
                && (i.CreatedAt.Date) <= (DateTo.Date)
            )
            .Select(i => new InvoiceTransactionsVM
            {
                InvoiceId = i.InvoiceId,
                CreatedAt = i.CreatedAt,
                TotalAmount = i.TotalAmount,
                TotalDiscount = i.TotalDiscount,
                DiscountType = i.DiscountType,
                GrandTotal = i.GrandTotal,
                SubTotal = i.SubTotal,
                Due = i.Due,
                //SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                //ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                IsProcedureInvoice = i.IsProcedureInvoice,
                //InvoiceItems = (i.InvoiceItems.Where(ii => ii.IsActive).Select(item => new InvoiceItemVM
                //{
                //IsActive = item.IsActive,
                //InvoiceItemId = item.InvoiceItemId,
                //ItemId = item.Item.ItemId,
                //ItemName = item.Item.ItemName,
                //Rate = item.Rate,
                //Quantity = item.Quantity,
                //Discount = item.Discount,
                //DiscountType = item.DiscountType,
                //DiscountTypeString = item.DiscountType == 1 ? "Value" : "%",
                //Amount = item.Amount,
                //BatchId = item.Batch.BatchId,
                //BatchName = item.Batch.BatchName,
                //ReturnedQuantity = item.ReturnedQuantity
                //})).ToList(),
                //PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                //TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
                //TotalRefund = -1 * (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)),
                //InvoicePayments = i.InvoicePayments.Where(p => p.IsActive).ToList(),
                Patient = i.Patient,
                PatientName = i.Patient.Name
            }).OrderByDescending(i => i.InvoiceId).ToList();
        }
        public SummationsVM GetSums_PendingPayments_Financial(DateTime DateFrom, DateTime DateTo)
        {
            SummationsVM Result = new SummationsVM();
            Result.TotalDues = (from i in cxt.Invoices
                                where i.IsActive
                                && (i.CreatedAt.Date) >= (DateFrom.Date)
                                && (i.CreatedAt.Date) <= (DateTo.Date)
                                select i.Due).Sum();
            return Result;
        }
        public IPagedList<InvoiceTransactionsVM> GetDeletedInvoices_Financial(DateTime DateFrom, DateTime DateTo, int PageNo, int PageSize)
        {
            return cxt.Invoices
            .Where(i => !i.IsActive)
            .Where(i => (i.CreatedAt.Date) >= (DateFrom.Date)
                && (i.CreatedAt.Date) <= (DateTo.Date)
            )
            .Select(i => new InvoiceTransactionsVM
            {
                InvoiceId = i.InvoiceId,
                CreatedAt = i.CreatedAt,
                TotalAmount = i.TotalAmount,
                TotalDiscount = i.TotalDiscount,
                DiscountType = i.DiscountType,
                GrandTotal = i.GrandTotal,
                SubTotal = i.SubTotal,
                Due = i.Due,
                //SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                //ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                User = i.User,
                DeletedAt = i.UpdatedAt,
                //InvoiceItems = i.InvoiceItems.Where(ii => ii.IsActive).Select(item => new InvoiceItemVM
                //{
                //    //IsActive = item.IsActive,
                //    //InvoiceItemId = item.InvoiceItemId,
                //    //ItemId = item.Item.ItemId,
                //    ItemName = item.Item.ItemName,
                //    //Rate = item.Rate,
                //    //Quantity = item.Quantity,
                //    //Discount = item.Discount,
                //    //DiscountType = item.DiscountType,
                //    //DiscountTypeString = item.DiscountType == 1 ? "Value" : "%",
                //    //Amount = item.Amount,
                //    //BatchId = item.Batch.BatchId,
                //    //BatchName = item.Batch.BatchName,
                //    //ReturnedQuantity = item.ReturnedQuantity
                //}).ToList(),
                //PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                TotalPaid = i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0,
                //TotalRefund = -1 * (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)),
                //InvoicePayments = i.InvoicePayments.Where(p => p.IsActive).ToList(),
                Patient = i.Patient,
                PatientName = i.Patient.Name
            }).OrderByDescending(i => i.InvoiceId).ToPagedList(PageNo, PageSize);
        }
        public List<InvoiceTransactionsVM> GetDeletedInvoices_Financial_Report(DateTime DateFrom, DateTime DateTo)
        {
            return cxt.Invoices
            .Where(i => i.IsActive)
            .Where(i => (i.CreatedAt.Date) >= (DateFrom.Date)
                && (i.CreatedAt.Date) <= (DateTo.Date)
            )
            .Select(i => new InvoiceTransactionsVM
            {
                InvoiceId = i.InvoiceId,
                CreatedAt = i.CreatedAt,
                TotalAmount = i.TotalAmount,
                TotalDiscount = i.TotalDiscount,
                DiscountType = i.DiscountType,
                GrandTotal = i.GrandTotal,
                SubTotal = i.SubTotal,
                Due = i.Due,
                //SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                //ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                User = i.User,
                DeletedAt = i.UpdatedAt,
                //InvoiceItems = i.InvoiceItems.Where(ii => ii.IsActive).Select(item => new InvoiceItemVM
                //{
                //    //IsActive = item.IsActive,
                //    //InvoiceItemId = item.InvoiceItemId,
                //    //ItemId = item.Item.ItemId,
                //    ItemName = item.Item.ItemName,
                //    //Rate = item.Rate,
                //    //Quantity = item.Quantity,
                //    //Discount = item.Discount,
                //    //DiscountType = item.DiscountType,
                //    //DiscountTypeString = item.DiscountType == 1 ? "Value" : "%",
                //    //Amount = item.Amount,
                //    //BatchId = item.Batch.BatchId,
                //    //BatchName = item.Batch.BatchName,
                //    //ReturnedQuantity = item.ReturnedQuantity
                //}).ToList(),
                //PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                TotalPaid = i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0,
                //TotalRefund = -1 * (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)),
                //InvoicePayments = i.InvoicePayments.Where(p => p.IsActive).ToList(),
                Patient = i.Patient,
                PatientName = i.Patient.Name
            }).OrderByDescending(i => i.InvoiceId).ToList();
        }
        public IPagedList<InvoiceTransactionsVM> GetRefundInvoices_Financial(DateTime DateFrom, DateTime DateTo, int PageNo, int PageSize)
        {
            return cxt.Invoices
            .Where(i => i.IsActive)
            //.Where(i => i.InvoiceItems.Any(ii => ii.ReturnedQuantity > 0))
            .Where(i => (i.CreatedAt.Date) >= (DateFrom.Date)
                && (i.CreatedAt.Date) <= (DateTo.Date)
            )
            .Select(i => new InvoiceTransactionsVM
            {
                InvoiceId = i.InvoiceId,
                CreatedAt = i.CreatedAt,
                TotalAmount = i.TotalAmount,
                TotalDiscount = i.TotalDiscount,
                DiscountType = i.DiscountType,
                GrandTotal = i.GrandTotal,
                SubTotal = i.SubTotal,
                Due = i.Due,
                //SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                //ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                User = i.User,
                DeletedAt = i.UpdatedAt,
                IsProcedureInvoice = i.IsProcedureInvoice,
                //InvoiceItems = i.InvoiceItems.Where(ii => ii.IsActive).Select(item => new InvoiceItemVM
                //{
                //    //IsActive = item.IsActive,
                //    //InvoiceItemId = item.InvoiceItemId,
                //    //ItemId = item.Item.ItemId,
                //    ItemName = item.Item.ItemName,
                //    //Rate = item.Rate,
                //    //Quantity = item.Quantity,
                //    //Discount = item.Discount,
                //    //DiscountType = item.DiscountType,
                //    //DiscountTypeString = item.DiscountType == 1 ? "Value" : "%",
                //    //Amount = item.Amount,
                //    //BatchId = item.Batch.BatchId,
                //    //BatchName = item.Batch.BatchName,
                //    //ReturnedQuantity = item.ReturnedQuantity
                //}).ToList(),
                //PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                RefundDate = cxt.InvoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault(),
                //TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
                TotalRefund = (double?)-1 * cxt.InvoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment) ?? 0,
                ReasonForRefund = cxt.InvoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(r => r.InvoicePaymentId).Select(r => r.RefundReason).FirstOrDefault(),
                //InvoicePayments = i.InvoicePayments.Where(p => p.IsActive).ToList(),
                Patient = i.Patient,
                PatientName = i.Patient.Name
            }).OrderByDescending(i => i.InvoiceId).ToPagedList(PageNo, PageSize);
        }
        public List<InvoiceTransactionsVM> GetRefundInvoices_Financial_Report(DateTime DateFrom, DateTime DateTo)
        {
            return cxt.Invoices
            .Where(i => i.IsActive)
            //.Where(i => i.InvoiceItems.Any(ii => ii.ReturnedQuantity > 0))
            .Where(i => (i.CreatedAt.Date) >= (DateFrom.Date)
                && (i.CreatedAt.Date) <= (DateTo.Date)
            )
            .Select(i => new InvoiceTransactionsVM
            {
                InvoiceId = i.InvoiceId,
                CreatedAt = i.CreatedAt,
                TotalAmount = i.TotalAmount,
                TotalDiscount = i.TotalDiscount,
                DiscountType = i.DiscountType,
                GrandTotal = i.GrandTotal,
                SubTotal = i.SubTotal,
                Due = i.Due,
                //SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                //ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                User = i.User,
                DeletedAt = i.UpdatedAt,
                IsProcedureInvoice = i.IsProcedureInvoice,
                //InvoiceItems = i.InvoiceItems.Where(ii => ii.IsActive).Select(item => new InvoiceItemVM
                //{
                //    //IsActive = item.IsActive,
                //    //InvoiceItemId = item.InvoiceItemId,
                //    //ItemId = item.Item.ItemId,
                //    ItemName = item.Item.ItemName,
                //    //Rate = item.Rate,
                //    //Quantity = item.Quantity,
                //    //Discount = item.Discount,
                //    //DiscountType = item.DiscountType,
                //    //DiscountTypeString = item.DiscountType == 1 ? "Value" : "%",
                //    //Amount = item.Amount,
                //    //BatchId = item.Batch.BatchId,
                //    //BatchName = item.Batch.BatchName,
                //    //ReturnedQuantity = item.ReturnedQuantity
                //}).ToList(),
                //PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                RefundDate = cxt.InvoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault(),
                //TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
                TotalRefund = -1 * cxt.InvoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment),
                ReasonForRefund = cxt.InvoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(r => r.InvoicePaymentId).Select(r => r.RefundReason).FirstOrDefault(),
                //InvoicePayments = i.InvoicePayments.Where(p => p.IsActive).ToList(),
                Patient = i.Patient,
                PatientName = i.Patient.Name
            }).OrderByDescending(i => i.InvoiceId).ToList();
        }
        public Invoice GetInvoiceById_Inc_Pmnts_cons(long InvoiceId)
        {
            //return cxt.Invoices
            //    .Where(i => i.InvoiceId == InvoiceId)
            //    .Include(i => i.InvoicePayments)
            //    .Include(i => i.InvoiceItems.Select(it => it.Item))
            //    .Include(i => i.InvoiceItems.Select(it => it.Batch))
            //    .Include(i => i.InvoiceItems.Select(it => it.Procedure))
            //    .Include(i => i.StockConsumptions.Select(sc => sc.Item)).FirstOrDefault();

            return cxt.Invoices
                .Where(i => i.InvoiceId == InvoiceId)
                .Include(i => i.InvoicePayments)
                .Include(i => i.Patient)
                //.Include(i => i.InvoiceItems.Select(it => it.Item))
                //.Include(i => i.InvoiceItems.Select(it => it.Batch))
                .Include(i => i.StockConsumption)
                //.Include(i => i.StockConsumption.StockConsumptionItems)
                .Include(i => i.StockConsumption.StockConsumptionItems.Select(sc => sc.Item))
                .Include(i => i.StockConsumption.StockConsumptionItems.Select(sc => sc.Batch))
                .FirstOrDefault();
        }
        public InvoiceVM GetInvoiceByInvoiceId(long InvoiceId)
        {
            InvoiceVM result =
                cxt.Invoices.Where(i => i.InvoiceId == InvoiceId)
                .Select(i => new InvoiceVM
                {
                    InvoiceId = i.InvoiceId,
                    EmployeeId = i.EmployeeId,
                    InvoiceRefNo = i.InvoiceRefNo,
                    CreatedAt = i.CreatedAt,
                    TotalAmount = i.TotalAmount,
                    TotalDiscount = i.TotalDiscount,
                    ModifiedDiscount = i.ModifiedDiscount,
                    DiscountType = i.DiscountType,
                    SubTotal = i.SubTotal,
                    GrandTotal = i.GrandTotal,
                    TotalPaid = (double?)i.InvoicePayments.Where(p => p.IsActive).Sum(p => p.Payment) ?? 0,
                    Due = i.Due,
                    OrderType = i.OrderType,
                    // SeatingTableId = i.SeatingTableId,
                    PaymentStatus = i.PaymentStatus,
                    //OrderStatus = i.OrderStatus,
                    Note = i.Note,
                    Patient = new PatientVM
                    {
                        PatientId = i.Patient == null ? 0 : i.Patient.PatientId,
                        Name = i.Patient == null ? "" : i.Patient.Name,
                        Phone = i.Patient == null ? "" : i.Patient.Phone,
                        Address = i.Patient == null ? "" : i.Patient.Address
                    },
                    //InvoiceItems = i.InvoiceItems.Select(item => new InvoiceItemVM
                    //{
                    //    IsActive = item.IsActive,
                    //    InvoiceItemId = item.InvoiceItemId,
                    //    Item = item.Item,
                    //    ItemId = item.Item.ItemId,
                    //    ItemName = item.Item.ItemName,
                    //    Unit = item.Unit,
                    //    UnitCost = item.PerUnitCostPrice,
                    //    Rate = item.Rate,
                    //    Quantity = item.Quantity,
                    //    BonusQuantity = item.BonusQuantity.HasValue ? item.BonusQuantity.Value : 0,
                    //    Discount = item.Discount,
                    //    DiscountType = item.DiscountType,
                    //    DiscountTypeString = item.DiscountType == 1 ? "%" : "Value",
                    //    SalesTax = item.SalesTax.HasValue ? item.SalesTax.Value : 0,
                    //    SalesTaxType = item.SalesTaxType.HasValue ? item.SalesTaxType.Value : 1,
                    //    SalesTaxTypeString = item.SalesTaxType.Value == 1 ? "%" : "Value",
                    //    Amount = item.Amount,
                    //    NetAmount = item.NetAmount,
                    //    BatchId = item.Batch == null ? 0 : item.Batch.BatchId,
                    //    BatchName = item.Batch.BatchName,
                    //    ReturnedQuantity = item.ReturnedQuantity,
                    //    AffectStock = item.AffectStock
                    //    //UnitCost = item.unit
                    //}),
                    InvoicePayments = i.InvoicePayments.Where(p => p.IsActive).Select(p => new InvoicePaymentVM
                    {
                        InvoicePaymentId = p.InvoicePaymentId,
                        PaymentType = p.PaymentType,
                        Payment = p.Payment,
                        ChequeNumber = p.ChequeNumber,
                        BankName = p.BankName,
                        ChequeStatus = p.ChequeStatus,
                        PaymentDate = p.PaymentDate,
                    })
                }).FirstOrDefault();
            return result;
        }
        public IPagedList<InvoiceVM> GetHoldingInvoices(int pageNo, int PageSize)
        {
            IPagedList<InvoiceVM> result =
                cxt.Invoices.Where(i => i.IsHoldInvoice && i.IsActive)
                .Select(i => new InvoiceVM
                {
                    InvoiceId = i.InvoiceId,
                    InvoiceRefNo = i.InvoiceRefNo,
                    CreatedAt = i.CreatedAt,
                    TotalAmount = i.TotalAmount,
                    TotalDiscount = i.TotalDiscount,
                    ModifiedDiscount = i.ModifiedDiscount,
                    DiscountType = i.DiscountType,
                    SubTotal = i.SubTotal,
                    GrandTotal = i.GrandTotal,
                    TotalPaid = (double?)i.InvoicePayments.Where(p => p.IsActive).Sum(p => p.Payment) ?? 0,
                    Due = i.Due,
                    Note = i.Note,
                    Patient = new PatientVM
                    {
                        PatientId = i.Patient == null ? 0 : i.Patient.PatientId,
                        Name = i.Patient == null ? "" : i.Patient.Name
                    },
                    //InvoiceItems = i.InvoiceItems.Select(item => new InvoiceItemVM
                    //{
                    //    //IsActive = item.IsActive,
                    //    //InvoiceItemId = item.InvoiceItemId,
                    //    Item = item.Item,
                    //    ItemId = item.Item.ItemId,
                    //    ItemName = item.Item.ItemName,
                    //    Unit = item.Unit,
                    //    UnitCost = item.PerUnitCostPrice,
                    //    Rate = item.Rate,
                    //    Rack = item.Item.Rack.Name,
                    //    Quantity = item.Quantity,
                    //    Discount = item.Discount,
                    //    DiscountType = item.DiscountType,
                    //    DiscountTypeString = item.DiscountType == 1 ? "Value" : "%",
                    //    Amount = item.Amount,
                    //    NetAmount = item.NetAmount,
                    //    BatchId = item.Batch.BatchId,
                    //    BatchName = item.Batch.BatchName,
                    //    ReturnedQuantity = item.ReturnedQuantity,
                    //    //UnitCost = item.unit
                    //}),
                    InvoicePayments = i.InvoicePayments.Where(p => p.IsActive).Select(p => new InvoicePaymentVM
                    {
                        InvoicePaymentId = p.InvoicePaymentId,
                        PaymentType = p.PaymentType,
                        Payment = p.Payment,
                        ChequeNumber = p.ChequeNumber,
                        BankName = p.BankName,
                        ChequeStatus = p.ChequeStatus,
                        PaymentDate = p.PaymentDate,
                    })
                })
                .OrderByDescending(i => i.InvoiceId)
                .ToPagedList(pageNo, PageSize);
            return result;
        }

        public InvoiceVM GetProcedureInvoiceByInvoiceId(long InvoiceId)
        {
            InvoiceVM result =
                cxt.Invoices.Where(i => i.InvoiceId == InvoiceId)
                .Select(i => new InvoiceVM
                {
                    InvoiceId = i.InvoiceId,
                    CreatedAt = i.CreatedAt,
                    TotalAmount = i.TotalAmount,
                    TotalDiscount = i.TotalDiscount,
                    DiscountType = i.DiscountType,
                    SubTotal = i.SubTotal,
                    GrandTotal = i.GrandTotal,
                    Due = i.Due,
                    Patient = new PatientVM
                    {
                        PatientId = i.Patient == null ? 0 : i.Patient.PatientId,
                        Name = i.Patient == null ? "" : i.Patient.Name
                    },
                    //InvoiceItems = i.InvoiceItems.Select(item => new InvoiceItemVM
                    //{
                    //    IsActive = item.IsActive,
                    //    InvoiceItemId = item.InvoiceItemId,
                    //    ProcedureDescription = item.ProcedureDescription,
                    //    Rate = item.Rate,
                    //    Quantity = item.Quantity,
                    //    Discount = item.Discount,
                    //    DiscountType = item.DiscountType,
                    //    DiscountTypeString = item.DiscountType == 1 ? "Value" : "%",
                    //    Amount = item.Amount,
                    //    ReturnedQuantity = item.ReturnedQuantity
                    //}),
                    InvoicePayments = i.InvoicePayments.Where(p => p.IsActive).Select(p => new InvoicePaymentVM
                    {
                        InvoicePaymentId = p.InvoicePaymentId,
                        PaymentType = p.PaymentType,
                        Payment = p.Payment,
                        PaymentDate = p.PaymentDate,
                        ChequeNumber = p.ChequeNumber,
                        BankName = p.BankName,
                        ChequeStatus = p.ChequeStatus
                    })
                }).FirstOrDefault();
            return result;
        }

        public IPagedList<InvoiceReturnVM> GetInvoiceReturns(DateTime FromDate, DateTime Dateto, int PageNo, int Pagesize)
        {
            return cxt.Invoices
                .Where(i => i.IsActive)
                .Where(i => (i.CreatedAt.Date) >= (FromDate)
                    && (i.CreatedAt.Date) <= (Dateto.Date))
                    //.Where(i => i.InvoiceItems.Any(it => it.ReturnedQuantity > 0))
                    .Select(ir => new InvoiceReturnVM
                    {
                        InvoiceId = ir.InvoiceId,
                        InvoiceRefNo = ir.InvoiceRefNo,
                        IsProcedureInvoice = ir.IsProcedureInvoice,
                        Patient = ir.Patient,
                        Total = ir.SubTotal,
                        Paid = ir.InvoicePayments.Where(p => p.Invoice.InvoiceId == ir.InvoiceId).Sum(p => p.Payment),
                        PaymentDate = ir.InvoicePayments.OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault(),
                        CreatedAt = ir.CreatedAt,
                        UserName = ir.User.UserName
                    }).OrderByDescending(i => i.InvoiceId).ToPagedList(PageNo, Pagesize);
        }
        public IPagedList<InvoiceReturnVM> GetInvoiceReturns(int PageNo, int Pagesize)
        {
            return cxt.Invoices
                .Where(i => i.IsActive)
                    //.Where(i => i.InvoiceItems.Any(it => it.ReturnedQuantity > 0))
                    .Select(ir => new InvoiceReturnVM
                    {
                        InvoiceId = ir.InvoiceId,
                        InvoiceRefNo = ir.InvoiceRefNo,
                        IsProcedureInvoice = ir.IsProcedureInvoice,
                        Patient = ir.Patient,
                        Total = ir.SubTotal,
                        Paid = ir.InvoicePayments.Where(p => p.Invoice.InvoiceId == ir.InvoiceId).Sum(p => p.Payment),
                        PaymentDate = ir.InvoicePayments.OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault(),
                        CreatedAt = ir.CreatedAt,
                        UserName = ir.User.UserName

                    }).OrderByDescending(i => i.InvoiceId).ToPagedList(PageNo, Pagesize);
        }
        public IPagedList<InvoiceReturnVM> GetInvoiceReturns(DateTime FromDate, DateTime Dateto, long invoiceRefNo, int PageNo, int Pagesize)
        {
            return cxt.Invoices
                    .Where(i => i.InvoiceRefNo == invoiceRefNo)
                    //.Where(i => i.InvoiceItems.Any(it => it.ReturnedQuantity > 0))
                    .Select(ir => new InvoiceReturnVM
                    {
                        InvoiceId = ir.InvoiceId,
                        InvoiceRefNo = ir.InvoiceRefNo,
                        IsProcedureInvoice = ir.IsProcedureInvoice,
                        Patient = ir.Patient,
                        Total = ir.SubTotal,
                        Paid = ir.InvoicePayments.Where(p => p.Invoice.InvoiceId == ir.InvoiceId).Sum(p => p.Payment),
                        PaymentDate = ir.InvoicePayments.OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault(),
                        CreatedAt = ir.CreatedAt,
                        UserName = ir.User.UserName
                    }).OrderByDescending(i => i.InvoiceId).ToPagedList(PageNo, Pagesize);
        }
        public IPagedList<InvoiceReturnVM> GetInvoiceReturns(DateTime FromDate, DateTime Dateto, int UserID, int PageNo, int Pagesize)
        {
            return cxt.Invoices
                    .Where(i => i.IsActive && i.User != null && i.User.UserId == UserID)
                      //.Where(i => i.InvoiceItems.Any(it => it.ReturnedQuantity > 0))
                      .Where(i => (i.CreatedAt.Date) >= (FromDate)
                    && (i.CreatedAt.Date) <= (Dateto.Date))
                    .Select(ir => new InvoiceReturnVM
                    {
                        InvoiceId = ir.InvoiceId,
                        InvoiceRefNo = ir.InvoiceRefNo,
                        IsProcedureInvoice = ir.IsProcedureInvoice,
                        Patient = ir.Patient,
                        Total = ir.SubTotal,
                        Paid = ir.InvoicePayments.Where(p => p.Invoice.InvoiceId == ir.InvoiceId).Sum(p => p.Payment),
                        PaymentDate = ir.InvoicePayments.OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault(),
                        CreatedAt = ir.CreatedAt,
                        UserName = ir.User.UserName
                    }).OrderByDescending(i => i.InvoiceId).ToPagedList(PageNo, Pagesize);
        }
        public IPagedList<InvoiceReturnVM> GetInvoiceReturns(DateTime FromDate, DateTime Dateto, string SearchString, int PageNo, int Pagesize)
        {
            return cxt.Invoices
                .Where(i => i.IsActive)
                .Where(i => (i.CreatedAt.Date) >= (FromDate)
                    && (i.CreatedAt.Date) <= (Dateto.Date))
                    //.Where(i => i.InvoiceItems.Any(it => it.ReturnedQuantity > 0))
                    .Where(i => i.Patient.Name.ToLower().Contains(SearchString))
                    .Select(ir => new InvoiceReturnVM
                    {
                        InvoiceId = ir.InvoiceId,
                        InvoiceRefNo = ir.InvoiceRefNo,
                        IsProcedureInvoice = ir.IsProcedureInvoice,
                        Patient = ir.Patient,
                        Total = ir.SubTotal,
                        Paid = ir.InvoicePayments.Where(p => p.Invoice.InvoiceId == ir.InvoiceId).Sum(p => p.Payment),
                        PaymentDate = ir.InvoicePayments.OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault(),
                        CreatedAt = ir.CreatedAt,
                        UserName = ir.User.UserName
                    }).OrderByDescending(i => i.InvoiceId).ToPagedList(PageNo, Pagesize);
        }


        // this is date range method
        public DiscountsMasterVM GetDiscounts(DateTime DateFrom, DateTime DateTo, int PageNo, int PageSize)
        {
            int count = 0;
            count = cxt.Invoices
            .Where(i => i.IsActive)
            .Where(i => i.ModifiedDiscount > 0
            && (i.CreatedAt.Date) >= (DateFrom.Date)
                   && (i.CreatedAt.Date) <= (DateTo.Date))
                   .Count();
            if (count <= 0)
            {
                return new DiscountsMasterVM { Discounts = new List<DiscountVM>().ToPagedList(1, 1) };
            }
            IPagedList<DiscountVM> Discounts = cxt.Invoices
            .Where(i => i.IsActive)
            .Where(i => i.ModifiedDiscount > 0
            && (i.CreatedAt.Date) >= (DateFrom.Date)
                   && (i.CreatedAt.Date) <= (DateTo.Date))
            .Select(i => new DiscountVM
            {
                InvoiceId = i.InvoiceId,
                InvoiceRefNo = i.InvoiceRefNo,
                SubTotal = i.SubTotal,
                GrandTotal = i.GrandTotal,
                DiscountType = i.DiscountType,
                //ModifiedDiscount = i.DiscountType == 1 ? i.ModifiedDiscount : ((i.ModifiedDiscount / 100) * i.SubTotal),
                ModifiedDiscount = i.ModifiedDiscount,
                //ModifiedDiscount = i
                PaymentDate = i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault(),
                TotalPaid = (double?)(i.InvoicePayments.Where(p => p.IsActive).Sum(p => (double?)p.Payment) ?? 0) ?? 0,
                Patient = i.Patient,
                //InvoiceItems = i.InvoiceItems.Select(item => new InvoiceItemVM
                //{
                //    ItemName = item.Item.ItemName
                //}).ToList()
            }).OrderByDescending(r => r.InvoiceId).ToPagedList(PageNo, PageSize);


            double totalDiscount = cxt.Invoices
            .Where(i => i.IsActive
            && i.ModifiedDiscount > 0
            && (i.CreatedAt.Date) >= (DateFrom.Date)
            && (i.CreatedAt.Date) <= (DateTo.Date))
            .Sum(i => (double?)i.ModifiedDiscount ?? 0);
            //.Select(i => new DiscountVM
            //{
            //    ModifiedDiscount = (double?)i.ModifiedDiscount ?? 0,

            //}).Sum(r => (double?)r.ModifiedDiscount ?? 0) ?? 0;

            //double TotalDiscount = 0;
            //if (PageNo == 1)
            //{
            //    TotalDiscount = cxt.Invoices
            //                              .Where(i => i.IsActive && i.TotalDiscount > 0)
            //                              .Sum(i => (double?)i.TotalDiscount) ?? 0;
            //}
            return new DiscountsMasterVM
            {
                TotalDiscount = totalDiscount,
                Discounts = Discounts
            };
        }
        public DiscountsMasterVM GetDiscountByInvNo(long InvNo, int PageNo, int PageSize)
        {
            IPagedList<DiscountVM> Discounts = cxt.Invoices
            .Where(i => i.InvoiceRefNo == InvNo)
            .Where(i => i.ModifiedDiscount > 0)
            .Select(i => new DiscountVM
            {
                InvoiceId = i.InvoiceId,
                SubTotal = i.SubTotal,
                GrandTotal = i.GrandTotal,
                DiscountType = i.DiscountType,
                //ModifiedDiscount = i.DiscountType == 1 ? i.ModifiedDiscount : ((i.ModifiedDiscount / 100) * i.SubTotal),
                ModifiedDiscount = i.ModifiedDiscount,
                //ModifiedDiscount = i
                PaymentDate = i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault(),
                TotalPaid = (double?)i.InvoicePayments.Where(p => p.IsActive).Sum(p => (double?)p.Payment) ?? 0,
                Patient = i.Patient,
                //InvoiceItems = i.InvoiceItems.Select(item => new InvoiceItemVM
                //{
                //    ItemName = item.Item.ItemName
                //}).ToList()
            }).OrderByDescending(r => r.InvoiceId).ToPagedList(PageNo, PageSize);


            double totalDiscount = (double?)cxt.Invoices
            .Where(i => i.InvoiceRefNo == InvNo)
            .Where(i => i.ModifiedDiscount > 0)
            .Select(i => i.ModifiedDiscount)
            .DefaultIfEmpty(0)
            .Sum() ?? 0;
            //.Select(i => new DiscountVM
            //{
            //    ModifiedDiscount = (double?)i.ModifiedDiscount ?? 0,

            //}).Sum(r => (double?)r.ModifiedDiscount ?? 0)) ?? 0;
            //double TotalDiscount = 0;
            //if (PageNo == 1)
            //{
            //    TotalDiscount = cxt.Invoices
            //                              .Where(i => i.IsActive && i.TotalDiscount > 0)
            //                              .Sum(i => (double?)i.TotalDiscount) ?? 0;
            //}
            //return new DiscountsMasterVM
            //{
            //    TotalDiscount = totalDiscount,
            //    Discounts = Discounts
            //};

            return new DiscountsMasterVM
            {
                TotalDiscount = 0,
                Discounts = Discounts
            };
        }
        public DiscountsMasterVM GetDiscountByPatient(long PatientId, int PageNo, int PageSize)
        {
            IPagedList<DiscountVM> Discounts = cxt.Invoices
                .Where(i => i.Patient != null && i.Patient.PatientId == PatientId)
            .Where(i => i.ModifiedDiscount > 0)
            .Select(i => new DiscountVM
            {
                InvoiceId = i.InvoiceId,
                SubTotal = i.SubTotal,
                GrandTotal = i.GrandTotal,
                DiscountType = i.DiscountType,
                //ModifiedDiscount = i.DiscountType == 1 ? i.ModifiedDiscount : ((i.ModifiedDiscount / 100) * i.SubTotal),
                ModifiedDiscount = i.ModifiedDiscount,
                //ModifiedDiscount = i
                PaymentDate = i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault(),
                TotalPaid = (double?)i.InvoicePayments.Where(p => p.IsActive).Sum(p => (double?)p.Payment) ?? 0,
                Patient = i.Patient,
                //InvoiceItems = i.InvoiceItems.Select(item => new InvoiceItemVM
                //{
                //    ItemName = item.Item.ItemName
                //}).ToList()
            }).OrderByDescending(r => r.InvoiceId).ToPagedList(PageNo, PageSize);


            double totalDiscount = (double?)cxt.Invoices
            .Where(i => i.Patient != null && i.Patient.PatientId == PatientId)
            .Where(i => i.ModifiedDiscount > 0)
            .Select(i => i.ModifiedDiscount)
            .DefaultIfEmpty(0)
            .Sum() ?? 0;
            //.Select(i => new DiscountVM
            //{
            //    ModifiedDiscount = (double?)i.ModifiedDiscount ?? 0,

            //}).Sum(r => (double?)r.ModifiedDiscount ?? 0)) ?? 0;
            //double TotalDiscount = 0;
            //if (PageNo == 1)
            //{
            //    TotalDiscount = cxt.Invoices
            //                              .Where(i => i.IsActive && i.TotalDiscount > 0)
            //                              .Sum(i => (double?)i.TotalDiscount) ?? 0;
            //}
            //return new DiscountsMasterVM
            //{
            //    TotalDiscount = totalDiscount,
            //    Discounts = Discounts
            //};

            return new DiscountsMasterVM
            {
                TotalDiscount = 0,
                Discounts = Discounts
            };
        }
        public DiscountsMasterVM GetDiscounts(DateTime DateFrom, DateTime DateTo, bool GetFullDiscountSales, int PageNo, int PageSize)
        {
            IPagedList<DiscountVM> Discounts = cxt.Invoices
            .Where(i => i.IsActive)
            .Where(i => i.ModifiedDiscount > 0)
            .Where(i => i.DiscountType == 2 && i.TotalDiscount == 100 || i.DiscountType == 1 && i.TotalDiscount == i.SubTotal)
            .Select(i => new DiscountVM
            {
                InvoiceId = i.InvoiceId,
                InvoiceRefNo = i.InvoiceRefNo,
                SubTotal = i.SubTotal,
                GrandTotal = i.GrandTotal,
                DiscountType = i.DiscountType,
                //ModifiedDiscount = i.DiscountType == 1 ? i.ModifiedDiscount : ((i.ModifiedDiscount / 100) * i.SubTotal),
                ModifiedDiscount = i.ModifiedDiscount,
                PaymentDate = i.InvoicePayments.Where(p => p.IsActive).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault(),
                TotalPaid = i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0,
                Patient = i.Patient,
                //InvoiceItems = i.InvoiceItems.Select(item => new InvoiceItemVM
                //{
                //    ItemName = item.Item.ItemName
                //}).ToList()
            }).OrderByDescending(r => r.InvoiceId).ToPagedList(PageNo, PageSize);
            double TotalDiscount = 0;
            if (PageNo == 1) // this will avoid getting total while each new page is fetched.
            {
                TotalDiscount = cxt.Invoices
                                          .Where(i => i.IsActive && i.TotalDiscount > 0)
                                          .Sum(i => (double?)i.ModifiedDiscount) ?? 0;
            }
            return new DiscountsMasterVM
            {
                //TotalDiscount = TotalDiscount,
                Discounts = Discounts
            };
        }
        // this is report method
        public List<DiscountVM> GetDiscounts_Rpt(DateTime DateFrom, DateTime DateTo)
        {
            List<DiscountVM> discounts = cxt.Invoices
            .Where(i => i.IsActive)
            .Where(i => i.ModifiedDiscount > 0
                && (i.CreatedAt.Date) >= (DateFrom.Date)
                   && (i.CreatedAt.Date) <= (DateTo.Date)
                   )
            .Select(i => new DiscountVM
            {
                InvoiceId = i.InvoiceId,
                SubTotal = i.SubTotal,
                GrandTotal = i.GrandTotal,
                DiscountType = i.DiscountType,
                //Discount = i.DiscountType == 1 ? i.ModifiedDiscount : ((i.ModifiedDiscount / 100) * i.SubTotal), // this field is being use din reports
                Discount = i.ModifiedDiscount, // this field is being use din reports
                //ModifiedDiscount = i.DiscountType == 1 ? i.ModifiedDiscount : ((i.ModifiedDiscount / 100) * i.SubTotal),
                PaymentDate = i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault(),
                TotalPaid = i.InvoicePayments.Where(p => p.IsActive).Sum(p => (double?)p.Payment) ?? 0,
                Patient = i.Patient,
                //InvoiceItems = i.InvoiceItems.Select(item => new InvoiceItemVM
                //{
                //    ItemName = item.Item.ItemName
                //}).ToList()
            }).OrderByDescending(r => r.InvoiceId).ToList();
            foreach (DiscountVM d in discounts)
            {
                if (d.Patient == null)
                {
                    d.PatientName = "";
                    d.MrNo = 0;
                }
                else
                {
                    d.PatientName = d.Patient.Name;
                    d.MrNo = 0;
                }
                foreach (InvoiceItemVM i in d.InvoiceItems)
                {
                    d.Description += i.ItemName + ", ";
                }
                d.Description = string.IsNullOrEmpty(d.Description) ? "" : d.Description.Trim().TrimEnd(',');
            }
            return discounts;
        }
        public List<DiscountVM> GetDiscounts_Rpt(bool GetFullDiscountSales, DateTime DateFrom, DateTime DateTo)
        {
            List<DiscountVM> discounts = cxt.Invoices
            .Where(i => i.IsActive)
            .Where(i => i.ModifiedDiscount > 0)
            .Where(i => i.DiscountType == 2 && i.ModifiedDiscount == 100 || i.DiscountType == 1 && i.TotalDiscount == i.SubTotal
            && (i.CreatedAt.Date) >= (DateFrom.Date)
                   && (i.CreatedAt.Date) <= (DateTo.Date))
            .Select(i => new DiscountVM
            {
                InvoiceId = i.InvoiceId,
                SubTotal = i.SubTotal,
                GrandTotal = i.GrandTotal,
                DiscountType = i.DiscountType,
                //Discount = i.DiscountType == 1 ? i.ModifiedDiscount : ((i.ModifiedDiscount / 100) * i.SubTotal), // this property is being used in reports
                Discount = i.ModifiedDiscount, // this property is being used in reports
                //ModifiedDiscount = i.DiscountType == 1 ? i.ModifiedDiscount : ((i.ModifiedDiscount / 100) * i.SubTotal),
                PaymentDate = i.InvoicePayments.Where(p => p.IsActive).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault(),
                TotalPaid = i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0,
                Patient = i.Patient,
                //InvoiceItems = i.InvoiceItems.Select(item => new InvoiceItemVM
                //{
                //    ItemName = item.Item.ItemName
                //}).ToList()
            }).OrderByDescending(r => r.InvoiceId).ToList();
            //foreach (DiscountVM d in discounts)
            //{
            //    if (d.Patient == null)
            //    {
            //        d.PatientName = "";
            //        d.MrNo = 0;
            //    }
            //    else
            //    {
            //        d.PatientName = d.Patient.Name;
            //        d.MrNo = (decimal)d.Patient.MRNo;
            //    }
            //    foreach (InvoiceItemVM i in d.InvoiceItems)
            //    {
            //        d.Description += i.ItemName + ", ";
            //    }
            //    d.Description = string.IsNullOrEmpty(d.Description) ? "" : d.Description.Trim().TrimEnd(',');
            //}
            return discounts;
        }

        public ProfitStatementMasterVM GetProfitStatements(DateTime DateFrom, DateTime DateTo, int PageNo, int PageSize)
        {
            ProfitStatementMasterVM vm = new ProfitStatementMasterVM();

            vm.ProfitStatementPaged = (from i in cxt.Invoices
                                       where (i.CreatedAt.Date) >= (DateFrom.Date)
                                       where (i.CreatedAt.Date) <= (DateTo.Date)
                                       where i.IsActive
                                       group i by (i.CreatedAt.Date) into g
                                       select new ProfitStatementVM
                                       {
                                           Date = g.Key.ToString(),
                                           TotalSales = g.Sum(r => (double?)r.SubTotal) ?? 0,
                                           //NetTotalSale = (double?)g.Sum(r => r.InvoiceItems.Where(p => p.IsActive).Sum(ii => ii.Amount - ii.Amount / ii.Quantity * ii.ReturnedQuantity)) ?? 0,
                                           TotalRevenue = (double?)g.Sum(r => r.InvoicePayments.Where(p => p.IsActive).Sum(p => p.Payment)) ?? 0,
                                           GrandTotals = g.Sum(r => (double?)r.GrandTotal) ?? 0,
                                           //CostOfSales = g.Sum(r => (double?)r.InvoiceItems.Where(ii => ii.IsActive).Sum(ii => (double?)ii.TotalCostPrice)) ?? 0,
                                           Discount = g.Sum(r => (double?)r.ModifiedDiscount) ?? 0
                                           //Discount = g.Sum(r => r.DiscountType == 1 ? r.ModifiedDiscount : ((r.ModifiedDiscount / 100) * r.SubTotal))
                                       }).OrderByDescending(r => r.Date).ToPagedList(PageNo, PageSize);


            vm.TotalSales = (from i in cxt.Invoices
                             where (i.CreatedAt.Date) >= (DateFrom.Date)
                             where (i.CreatedAt.Date) <= (DateTo.Date)
                             where i.IsActive
                             select i.SubTotal
                                      ).Sum(r => (double?)r) ?? 0;

            //vm.NetTotalSales = (double?)(from i in cxt.Invoices
            //                             where (i.CreatedAt.Date) >= (DateFrom.Date)
            //                             where (i.CreatedAt.Date) <= (DateTo.Date)
            //                             where i.IsActive
            //                             select i
            //                          ).Sum(r => (double?)r.InvoiceItems.Where(ii => ii.IsActive).Sum(ii => (double?)ii.Amount - ii.Amount / ii.Quantity * ii.ReturnedQuantity)) ?? 0;

            //vm.TotalRevenue = (double?)(from i in cxt.Invoices
            //                            where (i.CreatedAt.Date) >= (DateFrom.Date)
            //                            where (i.CreatedAt.Date) <= (DateTo.Date)
            //                            where i.IsActive
            //                            select i
            //         ).Sum(r => (double?)r.InvoicePayments.Where(p => p.IsActive).Sum(p => (double?)p.Payment)) ?? 0;

            //vm.TotalCostOfSales = (double?)(from i in cxt.Invoices
            //                                where (i.CreatedAt.Date) >= (DateFrom.Date)
            //                                where (i.CreatedAt.Date) <= (DateTo.Date)
            //                                where i.IsActive
            //                                select i
            //         ).Sum(r => (double?)r.InvoiceItems.Where(ii => ii.IsActive).Sum(ii => (double?)ii.TotalCostPrice)) ?? 0;

            vm.TotalDiscount = (double?)(from i in cxt.Invoices
                                         where (i.CreatedAt.Date) >= (DateFrom.Date)
                                         where (i.CreatedAt.Date) <= (DateTo.Date)
                                         where i.IsActive
                                         select i
                     ).Sum(r => (double?)r.ModifiedDiscount) ?? 0;
            //Sum(r => r.DiscountType == 1 ? r.ModifiedDiscount : ((r.ModifiedDiscount / 100) * r.SubTotal)) ?? 0;

            return vm;
        }

        public List<ProfitStatementVM> GetProfitStatements_Reports(DateTime DateFrom, DateTime DateTo)
        {
            return (from i in cxt.Invoices
                    where (i.CreatedAt.Date) >= (DateFrom.Date)
                    where (i.CreatedAt.Date) <= (DateTo.Date)
                    where i.IsActive
                    group i by (i.CreatedAt.Date) into g
                    select new ProfitStatementVM
                    {
                        Date = g.Key.ToString(),
                        TotalSales = g.Sum(r => r.SubTotal),
                        //NetTotalSale = (double?)g.Sum(r => r.InvoiceItems.Where(p => p.IsActive).Sum(ii => ii.Amount - ii.Amount / ii.Quantity * ii.ReturnedQuantity)) ?? 0,
                        TotalRevenue = (double?)g.Sum(r => r.InvoicePayments.Sum(p => p.Payment)) ?? 0,
                        GrandTotals = g.Sum(r => r.GrandTotal),
                        //CostOfSales = g.Sum(r => r.InvoiceItems.Where(ii => ii.IsActive).Sum(ii => ii.TotalCostPrice)),
                        //Discount = g.Sum(r => r.DiscountType == 1 ? r.ModifiedDiscount : ((r.ModifiedDiscount / 100) * r.SubTotal))
                        Discount = g.Sum(r => r.ModifiedDiscount)
                    }).OrderByDescending(r => r.Date).ToList();
        }

        public IPagedList<FinancialTransactionsVM> GetFinancialTrancation(DateTime DateFrom, DateTime DateTo, int PageNo, int PageSize)
        {
            return cxt.Invoices
            .Where(i => i.IsActive)
            .Where(i => (i.CreatedAt.Date) >= (DateFrom.Date)
                   && (i.CreatedAt.Date) <= (DateTo.Date))
            .Select(i => new FinancialTransactionsVM
            {
                InvoiceId = i.InvoiceId,
                CreatedAt = i.CreatedAt,
                TotalAmount = i.TotalAmount,
                TotalDiscount = i.TotalDiscount,
                GrandTotal = i.GrandTotal,
                SubTotal = i.SubTotal,
                Due = i.Due,
                PaymentDate = i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault(),
                RefundDate = i.InvoicePayments.Where(p => p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault(),
                TotalPaid = i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0,
                TotalRefund = -1 * (i.InvoicePayments.Where(p => p.IsActive && p.Payment < 0).Sum(p => (double?)p.Payment) ?? 0),
                Patient = i.Patient,
                //InvoiceItems = i.InvoiceItems.Where(ii => ii.IsActive).Select(ii => new InvoiceItemVM
                //{
                //    Item = ii.Item
                //}).ToList()
            }).OrderByDescending(i => i.InvoiceId).ToPagedList(PageNo, PageSize);
        }

        public List<ShiftCollectionVM> ShiftCollection(DateTime DateFrom, DateTime DateTo, int Shift)
        {
            bool GetAllShiftsData = false;
            if (Shift == 0)
            {
                GetAllShiftsData = true;
            }
            //shift 1 : morning
            //shift 2 : afternoon
            //shift 3 : evening
            DateTime ShiftStart;
            DateTime ShiftEnd;
            List<ShiftCollectionVM> CollectionList = new List<ShiftCollectionVM>();
            if (Shift == 1 || GetAllShiftsData && Shift == 0)
            {
                ShiftStart = new DateTime(DateFrom.Year, DateFrom.Month, DateFrom.Day, 0, 0, 0);
                ShiftEnd = new DateTime(DateTo.Year, DateTo.Month, DateTo.Day, 7, 59, 59);
                ShiftCollectionVM Collection =
                    new ShiftCollectionVM
                    {
                        Shift = "Morning(12am - 8am)",
                        Collection = cxt.InvoicePayments
                        .Where(p => p.IsActive
                            && p.Payment > 0
                            && (p.PaymentDate) >= (DateFrom.Date)
                            && (p.PaymentDate) <= (DateTo.Date)
                            && p.PaymentDate.TimeOfDay >= ShiftStart.TimeOfDay
                            && p.PaymentDate.TimeOfDay <= ShiftEnd.TimeOfDay
                            )//where ends
                            .Sum(p => (double?)p.Payment) ?? 0
                    };
                CollectionList.Add(Collection);
            }
            if (Shift == 2 || GetAllShiftsData && Shift == 0)
            {
                ShiftStart = new DateTime(DateFrom.Year, DateFrom.Month, DateFrom.Day, 8, 0, 0);
                ShiftEnd = new DateTime(DateTo.Year, DateTo.Month, DateTo.Day, 15, 59, 59);
                ShiftCollectionVM Collection =
                    new ShiftCollectionVM
                    {
                        Shift = "Afternoon(8am - 4pm)",
                        Collection = cxt.InvoicePayments
                        .Where(p => p.IsActive
                                && p.Payment > 0
                                && (p.PaymentDate) >= (DateFrom.Date)
                                && (p.PaymentDate) <= (DateTo.Date)
                                && p.PaymentDate.TimeOfDay >= ShiftStart.TimeOfDay
                                && p.PaymentDate.TimeOfDay <= ShiftEnd.TimeOfDay
                            )// where ends
                            .Sum(p => (double?)p.Payment) ?? 0
                        //Collection = (double?)cxt.invoicePayments.Where(p => p.IsActive && p.Payment > 0 && (p.Date) >= (ShiftStart) && (p.Date) <= (ShiftEnd)).Sum(p => p.Payment) ?? 0
                        //Collection = (double?)c.InvoicePayments.Sum(p => p.Payment) ?? 0
                    };
                CollectionList.Add(Collection);
            }
            if (Shift == 3 || GetAllShiftsData && Shift == 0)
            {
                ShiftStart = new DateTime(DateFrom.Year, DateFrom.Month, DateFrom.Day, 16, 0, 0);
                ShiftEnd = new DateTime(DateTo.Year, DateTo.Month, DateTo.Day, 23, 59, 59);
                ShiftCollectionVM Collection =
                    new ShiftCollectionVM
                    {
                        Shift = "Evening(4pm - 12am)",
                        Collection = cxt.InvoicePayments
                        .Where(p => p.IsActive
                                && p.Payment > 0
                                && (p.PaymentDate) >= (DateFrom.Date)
                                && (p.PaymentDate) <= (DateTo.Date)
                               && p.PaymentDate.TimeOfDay >= ShiftStart.TimeOfDay
                               && p.PaymentDate.TimeOfDay <= ShiftEnd.TimeOfDay)// where ends
                            .Sum(p => (double?)p.Payment) ?? 0
                    };
                CollectionList.Add(Collection);
            }
            return CollectionList;
        }

        public int GetTotalInvoiceCount()
        {
            return cxt.Invoices.Where(i => i.IsActive).Count();
        }

        public int GetCurrentDateInvoiceCount()
        {
            return cxt.Invoices.Where(i => i.IsActive && i.CreatedAt.Date == DateTime.Now.Date).Count();
        }

        public List<SaleGraphVM> GetMonthlySalesGraph(DateTime DateFrom, DateTime DateTo)
        {
            List<SaleGraphVM> Result = new List<SaleGraphVM>();
            Result = (from i in cxt.Invoices
                      where i.IsActive
                      && (i.CreatedAt.Date) >= (DateFrom.Date)
                      && (i.CreatedAt.Date) <= (DateTo.Date)
                      group i by (i.CreatedAt.Date) into g
                      select new SaleGraphVM
                      {
                          MonthDay = g.FirstOrDefault().CreatedAt.Day,
                          GrandTotal = g.Sum(r => r.GrandTotal),
                      }).ToList();
            return Result;
        }

        public List<SaleGraphVM> GetMonthlyRevenueGraph(DateTime DateFrom, DateTime DateTo)
        {
            List<SaleGraphVM> Result = new List<SaleGraphVM>();
            Result = (from i in cxt.Invoices
                      where i.IsActive
                      && (i.CreatedAt.Date) >= (DateFrom.Date)
                      && (i.CreatedAt.Date) <= (DateTo.Date)
                      group i by (i.CreatedAt.Date) into g
                      select new SaleGraphVM
                      {
                          MonthDay = g.FirstOrDefault().CreatedAt.Day,
                          GrandTotal = g.Sum(r => r.InvoicePayments.Sum(ip => ip.Payment)),
                      }).ToList();
            return Result;
        }

        public Invoice DeleteHoldingInvoice(int invoiceId)
        {
            return cxt.Invoices
                .Where(i => i.InvoiceId == invoiceId)
                .Include(i => i.InvoicePayments)
                .Include(i => i.StockConsumption)
                .Include(i => i.StockConsumption.StockConsumptionItems)
                //.Include(i => i.InvoiceItems)
                .FirstOrDefault();
        }
        public int getInvoiceIdByRefNo(int invoiceRefNo)
        {
            Invoice inv = cxt.Invoices.Where(i => i.InvoiceRefNo == invoiceRefNo)
                .FirstOrDefault();
            if (inv != null)
            {
                return (int)inv.InvoiceId;
            }
            else
            {
                return 0;
            }
        }
        public int getMaxInvoiceId()
        {
            return (int)cxt.Invoices.Max(i => i.InvoiceId);
        }
        public double GetInvoicesBalanceByPatientId(long patientId)
        {
            return cxt.Invoices
                .Where(i => i.IsActive && i.Patient.PatientId == patientId)
                .Sum(i => i.Due);
        }

        public List<XRptVM> getXRpt(int UserId)
        {
            return cxt.InvoicePayments
                .Where(r => r.User.UserId == UserId
                    && r.PaymentDate > (from c in cxt.POSClosings where c.IsActive orderby c.POSClosingId descending select c.ClosingDate).FirstOrDefault()
                    )
                    .Select(p => new XRptVM
                    {
                        InvoiceId = p.Invoice.InvoiceId,
                        InvoiceRef = p.Invoice.InvoiceRefNo,
                        TrTime = p.PaymentDate,
                        Amount = p.PaymentType == 1 ? p.Payment : 0
                    }).ToList();
        }
        public List<StockAddFilterVM> LoadPreviouslySoldItems(DateTime DateFrom, DateTime DateTo)
        {
            try
            {
                return cxt.InvoiceItems
                    .Where(i => (i.CreatedAt.Date) >= (DateFrom.Date))
                    .Where(i => (i.CreatedAt.Date) <= (DateTo.Date))
                    .GroupBy(i => new { i.Item })
                    .Select(i => new StockAddFilterVM
                    {
                        ItemId = i.Key.Item.ItemId,
                        ItemName = i.Key.Item.ItemName,
                        Unit = i.Key.Item.Unit,
                        ConversionUnit = i.Key.Item.ConversionUnit,
                        UnitCost =
                        SharedVariables.AdminPharmacySetting.IsItemConumptionFifo ?
                            (from si in cxt.StockItems join s in cxt.Stocks on si.Stock.StockId equals s.StockId where s.IsActive && si.IsActive && si.Item.ItemId == i.Key.Item.ItemId orderby si.StockItemId ascending select si.UnitCost).FirstOrDefault()
                            :
                            (from si in cxt.StockItems join s in cxt.Stocks on si.Stock.StockId equals s.StockId where s.IsActive && si.IsActive && si.Item.ItemId == i.Key.Item.ItemId orderby si.StockItemId descending select si.UnitCost).FirstOrDefault(),
                        RetailPrice =
                        SharedVariables.AdminPharmacySetting.IsItemConumptionFifo ?
                        (from si in cxt.StockItems join s in cxt.Stocks on si.Stock.StockId equals s.StockId where s.IsActive && si.IsActive && si.Item.ItemId == i.Key.Item.ItemId orderby si.StockItemId ascending select si.RetailPrice).FirstOrDefault()
                        :
                        (from si in cxt.StockItems join s in cxt.Stocks on si.Stock.StockId equals s.StockId where s.IsActive && si.IsActive && si.Item.ItemId == i.Key.Item.ItemId orderby si.StockItemId descending select si.RetailPrice).FirstOrDefault(),

                        Quantity = i.Sum(gp => gp.Quantity - gp.ReturnedQuantity),
                        TotalStock = cxt.StockItems.Where(si => si.IsActive && si.Item.ItemId == i.Key.Item.ItemId).Sum(si => (int?)si.Quantity + si.BonusQuantity) ?? 0,
                        HoldStock = (from cs in cxt.StockConsumptionItems join inv in cxt.Invoices on cs.InvoiceId equals inv.InvoiceId where inv.IsHoldInvoice && cs.IsActive && cs.Item.ItemId == i.Key.Item.ItemId select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                        ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemId == i.Key.Item.ItemId select new { qty = s.Quantity }).Sum(s => (int?)s.qty) ?? 0,
                        ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemId == i.Key.Item.ItemId && (b.Expiry) < (DateTime.Now) select new { qty = s.Quantity, BQty = s.BonusQuantity }).Sum(s => (int?)(s.qty + s.BQty)) ?? 0,
                        ExpiredConsStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == i.Key.Item.ItemId && (b_.Expiry) < (DateTime.Now) select new { es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                        AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemId == i.Key.Item.ItemId select new { qty = ai.Quantity }).Sum(ai => (int?)ai.qty) ?? 0,
                    }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<StockAddFilterVM> LoadPreviouslySoldItems(DateTime DateFrom, DateTime DateTo, long SupplierId)
        {
            try
            {
                Supplier sp = cxt.Suppliers.Where(s => s.SupplierID == SupplierId).FirstOrDefault();

                var query = cxt.InvoiceItems
                    .Where(i => (i.CreatedAt.Date) >= (DateFrom.Date))
                    .Where(i => (i.CreatedAt.Date) <= (DateTo.Date))
                    .Where(i => i.Item.Suppliers.Contains(sp))
                    .GroupBy(i => new { i.Item })
                    .Select(i => new StockAddFilterVM
                    {
                        ItemId = i.Key.Item.ItemId,
                        ItemName = i.Key.Item.ItemName,
                        Unit = i.Key.Item.Unit,
                        ConversionUnit = i.Key.Item.ConversionUnit,
                        UnitCost =
                        SharedVariables.AdminPharmacySetting.IsItemConumptionFifo ?
                            (from si in cxt.StockItems join s in cxt.Stocks on si.Stock.StockId equals s.StockId where s.IsActive && si.IsActive && si.Item.ItemId == i.Key.Item.ItemId orderby si.StockItemId ascending select si.UnitCost).FirstOrDefault()
                            :
                            (from si in cxt.StockItems join s in cxt.Stocks on si.Stock.StockId equals s.StockId where s.IsActive && si.IsActive && si.Item.ItemId == i.Key.Item.ItemId orderby si.StockItemId descending select si.UnitCost).FirstOrDefault(),
                        RetailPrice =
                        SharedVariables.AdminPharmacySetting.IsItemConumptionFifo ?
                        (from si in cxt.StockItems join s in cxt.Stocks on si.Stock.StockId equals s.StockId where s.IsActive && si.IsActive && si.Item.ItemId == i.Key.Item.ItemId orderby si.StockItemId ascending select si.RetailPrice).FirstOrDefault()
                        :
                        (from si in cxt.StockItems join s in cxt.Stocks on si.Stock.StockId equals s.StockId where s.IsActive && si.IsActive && si.Item.ItemId == i.Key.Item.ItemId orderby si.StockItemId descending select si.RetailPrice).FirstOrDefault(),

                        Quantity = i.Sum(gp => gp.Quantity - gp.ReturnedQuantity),
                        TotalStock = cxt.StockItems.Where(si => si.IsActive && si.Item.ItemId == i.Key.Item.ItemId).Sum(si => (int?)si.Quantity + si.BonusQuantity) ?? 0,
                        HoldStock = (from cs in cxt.StockConsumptionItems join inv in cxt.Invoices on cs.InvoiceId equals inv.InvoiceId where inv.IsHoldInvoice && cs.IsActive && cs.Item.ItemId == i.Key.Item.ItemId select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                        ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemId == i.Key.Item.ItemId select new { qty = s.Quantity }).Sum(s => (int?)s.qty) ?? 0,
                        ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemId == i.Key.Item.ItemId && (b.Expiry) < (DateTime.Now) select new { qty = s.Quantity, BQty = s.BonusQuantity }).Sum(s => (int?)(s.qty + s.BQty)) ?? 0,
                        ExpiredConsStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == i.Key.Item.ItemId && (b_.Expiry) < (DateTime.Now) select new { es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                        AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemId == i.Key.Item.ItemId select new { qty = ai.Quantity }).Sum(ai => (int?)ai.qty) ?? 0,
                    });

                return cxt.InvoiceItems
                    .Where(i => (i.CreatedAt.Date) >= (DateFrom.Date))
                    .Where(i => (i.CreatedAt.Date) <= (DateTo.Date))
                    .Where(i => i.Item.Suppliers.Contains(sp))
                    .GroupBy(i => new { i.Item })
                    .Select(i => new StockAddFilterVM
                    {
                        ItemId = i.Key.Item.ItemId,
                        ItemName = i.Key.Item.ItemName,
                        Unit = i.Key.Item.Unit,
                        ConversionUnit = i.Key.Item.ConversionUnit,
                        UnitCost =
                        SharedVariables.AdminPharmacySetting.IsItemConumptionFifo ?
                            (from si in cxt.StockItems join s in cxt.Stocks on si.Stock.StockId equals s.StockId where s.IsActive && si.IsActive && si.Item.ItemId == i.Key.Item.ItemId orderby si.StockItemId ascending select si.UnitCost).FirstOrDefault()
                            :
                            (from si in cxt.StockItems join s in cxt.Stocks on si.Stock.StockId equals s.StockId where s.IsActive && si.IsActive && si.Item.ItemId == i.Key.Item.ItemId orderby si.StockItemId descending select si.UnitCost).FirstOrDefault(),
                        RetailPrice =
                        SharedVariables.AdminPharmacySetting.IsItemConumptionFifo ?
                        (from si in cxt.StockItems join s in cxt.Stocks on si.Stock.StockId equals s.StockId where s.IsActive && si.IsActive && si.Item.ItemId == i.Key.Item.ItemId orderby si.StockItemId ascending select si.RetailPrice).FirstOrDefault()
                        :
                        (from si in cxt.StockItems join s in cxt.Stocks on si.Stock.StockId equals s.StockId where s.IsActive && si.IsActive && si.Item.ItemId == i.Key.Item.ItemId orderby si.StockItemId descending select si.RetailPrice).FirstOrDefault(),

                        Quantity = i.Sum(gp => gp.Quantity - gp.ReturnedQuantity),
                        TotalStock = cxt.StockItems.Where(si => si.IsActive && si.Item.ItemId == i.Key.Item.ItemId).Sum(si => (int?)si.Quantity + si.BonusQuantity) ?? 0,
                        HoldStock = (from cs in cxt.StockConsumptionItems join inv in cxt.Invoices on cs.InvoiceId equals inv.InvoiceId where inv.IsHoldInvoice && cs.IsActive && cs.Item.ItemId == i.Key.Item.ItemId select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                        ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemId == i.Key.Item.ItemId select new { qty = s.Quantity }).Sum(s => (int?)s.qty) ?? 0,
                        ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemId == i.Key.Item.ItemId && (b.Expiry) < (DateTime.Now) select new { qty = s.Quantity, BQty = s.BonusQuantity }).Sum(s => (int?)(s.qty + s.BQty)) ?? 0,
                        ExpiredConsStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == i.Key.Item.ItemId && (b_.Expiry) < (DateTime.Now) select new { es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                        AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemId == i.Key.Item.ItemId select new { qty = ai.Quantity }).Sum(ai => (int?)ai.qty) ?? 0,
                    }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<PendingOrdersVM> GetPendingOrders(Enums.PendingOrdersViewType OrderViewType)
        {
            var pendingOrders = cxt.Invoices.Where(i => i.IsActive)
                .Where(o => OrderViewType == Enums.PendingOrdersViewType.Pending && o.OrderStatus == 1 || OrderViewType != Enums.PendingOrdersViewType.Pending)
                .Where(o => OrderViewType == Enums.PendingOrdersViewType.Completed && o.OrderStatus == 2 || OrderViewType != Enums.PendingOrdersViewType.Completed)
                .Select(i => new PendingOrdersVM
                {
                    InvoiceId = i.InvoiceId,
                    CustomerName = i.Patient == null ? "" : i.Patient.Name,
                    CustomerPhone = i.Patient == null ? "" : i.Patient.Phone,
                    CustomerAddress = i.Patient == null ? "" : i.Patient.Address,
                    TotalAmount = i.TotalAmount,
                    DiscountAmount = i.SubTotal - i.GrandTotal,
                    GrandTotal = i.GrandTotal,
                    OrderType = i.OrderType,
                    PaymentMethod = i.PaymentMethod,
                    PaymentStatus = i.PaymentStatus,
                    OrderStatusString = ((Enums.PendingOrdersViewType)i.OrderStatus).ToString(),
                    OrderStartTime = i.CreatedAt
                }).OrderByDescending(i => i.InvoiceId);

            List<PendingOrdersVM> resultData = new List<PendingOrdersVM>();
            if (OrderViewType == Enums.PendingOrdersViewType.All)
            {
                resultData = pendingOrders.Take(200)
                .ToList();
            }
            else
            {
                resultData = pendingOrders.Take(50)
                .ToList();
            }
            return resultData;
        }
        public double GetTodayCollection(DateTime from, DateTime To)
        {
            try
            {
                var res = cxt.Invoices.Where(i => i.IsActive && (i.CreatedAt.Date) >= (from)
                                                      && (i.CreatedAt.Date) <= (To)
                                                      ).Sum(r => (double?)r.SubTotal) ?? 0;
                return res;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public bool SetInvoiceStatusToPaid(long invoiceId)
        {
            try
            {
                var invoice = cxt.Invoices.Where(i => i.InvoiceId == invoiceId).Include(i => i.InvoicePayments).FirstOrDefault();
                if (invoice == null) { return false; }

                //invoice.PaymentStatus = 1;
                invoice.TotalPaid = invoice.GrandTotal;
                invoice.Due = 0;
                var invoicepayment = invoice.InvoicePayments.FirstOrDefault();
                invoicepayment.Payment = invoice.GrandTotal;
                invoicepayment.PaymentDate = DateTime.Now;
                invoicepayment.PaymentType = 1;
                invoicepayment.MethodType = 0;
                cxt.InvoicePayments.Attach(invoicepayment);
                cxt.Entry(invoicepayment).State = EntityState.Modified;
                cxt.Entry(invoice).State = EntityState.Modified;
                cxt.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool MarkBillReceieving(long invoiceId)
        {
            try
            {
                var invoice = cxt.Invoices.Where(i => i.InvoiceId == invoiceId).FirstOrDefault();
                if (invoice == null) { return false; }
                invoice.PaymentStatus = 2;
                cxt.Entry(invoice).State = EntityState.Modified;
                cxt.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool SetInvoiceORderStatusToCompleted(long invoiceId)
        {
            try
            {
                var invoice = cxt.Invoices.Where(i => i.InvoiceId == invoiceId).FirstOrDefault();
                if (invoice == null) { return false; }

                invoice.OrderStatus = 2;
                //invoice.OrderCompletedAt = DateTime.Now;
                cxt.Entry(invoice).State = EntityState.Modified;
                cxt.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}