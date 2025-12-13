

namespace Restaurant_MS_Infrastructure.Repository
{
    public class StockRepository : Repository<Stock>, IStock
    {
        AppDbContext cxt;
        public StockRepository(AppDbContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }

        public decimal GetNewDocumentNumber()
        {
            decimal NewDocNo = 0;
            if (cxt.Stocks.FirstOrDefault() == null)
            {
                NewDocNo = 100001;
            }
            else
            {
                NewDocNo = cxt.Stocks.Max(n => n.DocumentNo) + 1;
            }
            return Math.Round(NewDocNo);
        }

        public void AddStockItem()
        {

        }

        public void InsertStock(Stock objStock)
        {
            cxt.Stocks.Add(objStock);
            cxt.SaveChanges();
        }

        //public IPagedList<StockVM> GetStocks(int pageNo, int PageSize)
        //{
        //    //IPagedList<Stock> result = cxt.Stocks
        //    //    .Where(s => s.IsActive == true)
        //    //    .Include(s => s.StockItems.Select(i => i.Item))
        //    //    .Include(s => s.StockItems.Select(i => i.Batch))
        //    //    .Include(s => s.Supplier)
        //    //    .OrderByDescending(s => s.StockId)
        //    //    .ToPagedList<Stock>(pageNo, PageSize);
        //    //return result;

        //    IPagedList<StockVM> result = cxt.Stocks
        //        .Where(s => s.IsActive)
        //        .Select(s => new StockVM
        //        {
        //            StockId = s.StockId,
        //            DocumentNo = s.DocumentNo,
        //            SupplierInvoiceNo = s.SupplierInvoiceNo,
        //            ImagePath = s.ImagePath,
        //            CreatedAt = s.CreatedAt,
        //            UpdatedAt = s.UpdatedAt,
        //            IsActive = s.IsActive,
        //            SupplierIvoiceDate = s.SupplierIvoiceDate,
        //            StockItems = (s.StockItems.Select(si => new StockItemVM
        //            {
        //                IsActive = si.IsActive,
        //                Item = si.Item,
        //                Batch = si.Batch,
        //                Quantity = si.Quantity,
        //                UnitCost = si.UnitCost,
        //                RetailPrice = si.RetailPrice,
        //                TotalCost = si.TotalCost,
        //                Discount = si.Discount,
        //                SalesTax = si.SalesTax,
        //                NetValue = si.NetValue,
        //                DiscountType = si.DiscountType,
        //                SalesTaxType = si.SalesTaxType,
        //                CreatedAt = si.CreatedAt,
        //                StockConsumed = (cxt.StockConsumptionItems.Where(c => c.Item == si.Item && c.Batch == si.Batch && c.IsActive && c.Quantity > 0).Select(c => c.Quantity).FirstOrDefault())
        //            })).ToList(),
        //            Supplier = s.Supplier
        //        })
        //        .OrderByDescending(s => s.StockId)
        //        .ToPagedList(pageNo, PageSize);
        //    return result;
        //}

        public IPagedList<StockVM> GetStocks(long SupplierId, int pageNo, int PageSize)
        {
            //IPagedList<Stock> result = cxt.Stocks
            //.Where(s => s.IsActive == true)
            //.Where(s => s.Supplier.SupplierID == SupplierId)
            //.Include(s => s.StockItems.Select(i => i.Item))
            //.Include(s => s.StockItems.Select(i => i.Batch))
            //.Include(s => s.Supplier)
            //.OrderByDescending(s => s.StockId)
            //.ToPagedList<Stock>(pageNo, PageSize);
            //return result;

            IPagedList<StockVM> result = cxt.Stocks
              .Where(s => s.IsActive)
              .Where(s => s.Supplier.SupplierID == SupplierId)
              .Where(s => s.StockItems.Any(si => si.IsActive && si.Quantity > 0))
              .Select(s => new StockVM
              {
                  StockId = s.StockId,
                  DocumentNo = s.DocumentNo,
                  SupplierInvoiceNo = s.SupplierInvoiceNo,
                  ImagePath = s.ImagePath,
                  CreatedAt = s.CreatedAt,
                  UpdatedAt = s.UpdatedAt,
                  IsActive = s.IsActive,
                  SupplierIvoiceDate = s.SupplierIvoiceDate,
                  UserName = s.User.UserName,
                  IsAutoInsertedStock = s.IsAutoInsertedStock,
                  StockItems = s.StockItems.Where(si => si.IsActive && si.Quantity > 0).Select(si => new StockItemVM
                  {
                      IsActive = si.IsActive,
                      Item = si.Item,
                      Unit = si.Unit,
                      UnitString = si.Unit == 0 ? si.Item.Unit : "Units",
                      ConversionUnit = si.Item.ConversionUnit,
                      Batch = si.Batch,
                      Quantity = si.Quantity,
                      BonusQuantity = si.BonusQuantity,
                      UnitCost = si.UnitCost,
                      RetailPrice = si.RetailPrice,
                      TotalCost = si.TotalCost,
                      Discount = si.Discount,
                      SalesTax = si.SalesTax,
                      NetValue = si.NetValue,
                      DiscountType = si.DiscountType,
                      SalesTaxType = si.SalesTaxType,
                      CreatedAt = si.CreatedAt,
                      StockConsumed = cxt.StockConsumptionItems.Where(c => c.Item == si.Item && c.Batch == si.Batch && c.IsActive && c.Quantity > 0).Select(c => c.Quantity).FirstOrDefault()
                  }).ToList(),
                  Supplier = s.Supplier
              })
              .OrderByDescending(s => s.StockId)
              .ToPagedList(pageNo, PageSize);

            return result;
        }
        public IPagedList<StockVM> GetStocks(DateTime FromDate, DateTime ToDate, int pageNo, int PageSize)
        {
            //IPagedList<Stock> result = cxt.Stocks
            //.Where(s => s.IsActive == true)
            //.Where(s => (s.CreatedAt) >= (FromDate))
            //.Where(s => (s.CreatedAt) <= (ToDate))
            //.Include(s => s.StockItems.Select(i => i.Item))
            //.Include(s => s.StockItems.Select(i => i.Batch))
            //.Include(s => s.Supplier)
            //.OrderByDescending(s => s.StockId)
            //.ToPagedList<Stock>(pageNo, PageSize);
            //return result;

            IPagedList<StockVM> result = cxt.Stocks
              .Where(s => s.IsActive)
              .Where(s => (s.CreatedAt) >= (FromDate))
              .Where(s => (s.CreatedAt) <= (ToDate))
              .Where(s => s.StockItems.Any(si => si.IsActive && si.Quantity > 0))
              .Select(s => new StockVM
              {
                  StockId = s.StockId,
                  DocumentNo = s.DocumentNo,
                  SupplierInvoiceNo = s.SupplierInvoiceNo,
                  ImagePath = s.ImagePath,
                  CreatedAt = s.CreatedAt,
                  UpdatedAt = s.UpdatedAt,
                  IsActive = s.IsActive,
                  SupplierIvoiceDate = s.SupplierIvoiceDate,
                  UserName = s.User.UserName,
                  IsAutoInsertedStock = s.IsAutoInsertedStock,
                  StockItems = s.StockItems.Where(si => si.IsActive && si.Quantity > 0).Select(si => new StockItemVM
                  {
                      IsActive = si.IsActive,
                      Item = si.Item,
                      Unit = si.Unit,
                      UnitString = si.Unit == 0 ? si.Item.Unit : "Units",
                      ConversionUnit = si.Item.ConversionUnit,
                      Batch = si.Batch,
                      Quantity = si.Quantity,
                      BonusQuantity = si.BonusQuantity,
                      UnitCost = si.UnitCost,
                      RetailPrice = si.RetailPrice,
                      TotalCost = si.TotalCost,
                      Discount = si.Discount,
                      SalesTax = si.SalesTax,
                      NetValue = si.NetValue,
                      DiscountType = si.DiscountType,
                      SalesTaxType = si.SalesTaxType,
                      CreatedAt = si.CreatedAt,
                      StockConsumed = cxt.StockConsumptionItems.Where(c => c.Item == si.Item && c.Batch == si.Batch && c.IsActive && c.Quantity > 0).Select(c => c.Quantity).FirstOrDefault()
                  }).ToList(),
                  Supplier = s.Supplier
              })
              .OrderByDescending(s => s.StockId)
              .ToPagedList(pageNo, PageSize);

            return result;
        }
        public IPagedList<StockVM> GetStocks(int pageNo, int PageSize)
        {
            //IPagedList<Stock> result = cxt.Stocks
            //.Where(s => s.IsActive == true)
            //.Where(s => (s.CreatedAt) >= (FromDate))
            //.Where(s => (s.CreatedAt) <= (ToDate))
            //.Include(s => s.StockItems.Select(i => i.Item))
            //.Include(s => s.StockItems.Select(i => i.Batch))
            //.Include(s => s.Supplier)
            //.OrderByDescending(s => s.StockId)
            //.ToPagedList<Stock>(pageNo, PageSize);
            //return result;

            IPagedList<StockVM> result = cxt.Stocks
              .Where(s => s.IsActive)
              .Where(s => s.StockItems.Any(si => si.IsActive && si.Quantity > 0))
              .Select(s => new StockVM
              {
                  StockId = s.StockId,
                  DocumentNo = s.DocumentNo,
                  SupplierInvoiceNo = s.SupplierInvoiceNo,
                  ImagePath = s.ImagePath,
                  CreatedAt = s.CreatedAt,
                  UpdatedAt = s.UpdatedAt,
                  IsActive = s.IsActive,
                  SupplierIvoiceDate = s.SupplierIvoiceDate,
                  UserName = s.User.UserName,
                  IsAutoInsertedStock = s.IsAutoInsertedStock,
                  StockItems = s.StockItems.Where(si => si.IsActive && si.Quantity > 0).Select(si => new StockItemVM
                  {
                      IsActive = si.IsActive,
                      Item = si.Item,
                      Unit = si.Unit,
                      UnitString = si.Unit == 0 ? si.Item.Unit : "Units",
                      ConversionUnit = si.Item.ConversionUnit,
                      Batch = si.Batch,
                      Quantity = si.Quantity,
                      BonusQuantity = si.BonusQuantity,
                      UnitCost = si.UnitCost,
                      RetailPrice = si.RetailPrice,
                      TotalCost = si.TotalCost,
                      Discount = si.Discount,
                      SalesTax = si.SalesTax,
                      NetValue = si.NetValue,
                      DiscountType = si.DiscountType,
                      SalesTaxType = si.SalesTaxType,
                      CreatedAt = si.CreatedAt,
                      StockConsumed = cxt.StockConsumptionItems.Where(c => c.Item == si.Item && c.Batch == si.Batch && c.IsActive && c.Quantity > 0).Select(c => c.Quantity).FirstOrDefault()
                  }).ToList(),
                  Supplier = s.Supplier
              })
              .OrderByDescending(s => s.StockId)
              .ToPagedList(pageNo, PageSize);

            return result;
        }
        public IPagedList<StockVM> GetStocks(DateTime FromDate, DateTime ToDate, string NameFilter, int pageNo, int PageSize)
        {
            //IPagedList<Stock> result = cxt.Stocks
            //.Where(s => s.IsActive == true)
            //.Where(s => (s.CreatedAt) >= (FromDate))
            //.Where(s => (s.CreatedAt) <= (ToDate))
            //.Where(s => s.StockItems.Any(i => i.Item.ItemName.ToLower().Contains(NameFilter)))
            //.Include(s => s.StockItems.Select(i => i.Item))
            //.Include(s => s.StockItems.Select(i => i.Batch))
            //.Include(s => s.Supplier)
            //.OrderByDescending(s => s.StockId)
            //.ToPagedList<Stock>(pageNo, PageSize);
            //return result;
            IPagedList<StockVM> result = cxt.Stocks
              .Where(s => s.IsActive)
              .Where(s => (s.CreatedAt) >= (FromDate))
              .Where(s => (s.CreatedAt) <= (ToDate))
              .Where(s => s.StockItems.Any(i => i.IsActive && i.Quantity > 0 && i.Item.ItemName.ToLower().Contains(NameFilter)))
              .Select(s => new StockVM
              {
                  StockId = s.StockId,
                  DocumentNo = s.DocumentNo,
                  SupplierInvoiceNo = s.SupplierInvoiceNo,
                  ImagePath = s.ImagePath,
                  CreatedAt = s.CreatedAt,
                  UpdatedAt = s.UpdatedAt,
                  IsActive = s.IsActive,
                  SupplierIvoiceDate = s.SupplierIvoiceDate,
                  UserName = s.User.UserName,
                  IsAutoInsertedStock = s.IsAutoInsertedStock,
                  StockItems = s.StockItems.Where(si => si.IsActive && si.Quantity > 0).Select(si => new StockItemVM
                  {
                      IsActive = si.IsActive,
                      Item = si.Item,
                      Unit = si.Unit,
                      UnitString = si.Unit == 0 ? si.Item.Unit : "Units",
                      ConversionUnit = si.Item.ConversionUnit,
                      Batch = si.Batch,
                      Quantity = si.Quantity,
                      UnitCost = si.UnitCost,
                      RetailPrice = si.RetailPrice,
                      TotalCost = si.TotalCost,
                      Discount = si.Discount,
                      SalesTax = si.SalesTax,
                      NetValue = si.NetValue,
                      DiscountType = si.DiscountType,
                      SalesTaxType = si.SalesTaxType,
                      CreatedAt = si.CreatedAt,
                      StockConsumed = cxt.StockConsumptionItems.Where(c => c.Item == si.Item && c.Batch == si.Batch && c.IsActive && c.Quantity > 0).Select(c => c.Quantity).FirstOrDefault()
                  }).ToList(),
                  Supplier = s.Supplier
              })
              .OrderByDescending(s => s.StockId)
              .ToPagedList(pageNo, PageSize);

            return result;
        }
        public List<StocksReportVM> GetStocks_Print(DateTime FromDate, DateTime ToDate, long SupplierId)
        {
            //IPagedList<Stock> result = cxt.Stocks
            //.Where(s => s.IsActive == true)
            //.Where(s => (s.CreatedAt) >= (FromDate))
            //.Where(s => (s.CreatedAt) <= (ToDate))
            //.Where(s => s.StockItems.Any(i => i.Item.ItemName.ToLower().Contains(NameFilter)))
            //.Include(s => s.StockItems.Select(i => i.Item))
            //.Include(s => s.StockItems.Select(i => i.Batch))
            //.Include(s => s.Supplier)
            //.OrderByDescending(s => s.StockId)
            //.ToPagedList<Stock>(pageNo, PageSize);
            //return result;
            List<StocksReportVM> Result = (from s in cxt.Stocks
                                           join si in cxt.StockItems on s equals si.Stock
                                           where s.IsActive
                                           && s.Supplier.SupplierID == SupplierId
                                           && (s.CreatedAt) >= (FromDate)
                                           && (s.CreatedAt) <= (ToDate)
                                           && si.IsActive
                                           && si.Quantity > 0
                                           select new StocksReportVM
                                           {
                                               StockId = s.StockId,
                                               DocumentNo = s.DocumentNo,
                                               SupplierIvoiceDate = s.SupplierIvoiceDate,
                                               CreatedAt = s.CreatedAt,
                                               ItemName = si.Item.ItemName,
                                               Quantity = si.Quantity,
                                               UnitCost = si.UnitCost,
                                               TotalCost = si.TotalCost,
                                               RetailPrice = si.RetailPrice,
                                               BatchName = si.Batch.BatchName,
                                               Discount = si.Discount,
                                               DiscountType = si.DiscountType,
                                               SalesTax = si.SalesTax,
                                               NetValue = si.NetValue,
                                               ItemCreatedAt = si.CreatedAt
                                           }).OrderByDescending(s => s.StockId).ToList();
            return Result;
        }
        public List<StocksReportVM> GetStocks_Print(DateTime FromDate, DateTime ToDate, bool ApplyDateFilter)
        {
            //IPagedList<Stock> result = cxt.Stocks
            //.Where(s => s.IsActive == true)
            //.Where(s => (s.CreatedAt) >= (FromDate))
            //.Where(s => (s.CreatedAt) <= (ToDate))
            //.Where(s => s.StockItems.Any(i => i.Item.ItemName.ToLower().Contains(NameFilter)))
            //.Include(s => s.StockItems.Select(i => i.Item))
            //.Include(s => s.StockItems.Select(i => i.Batch))
            //.Include(s => s.Supplier)
            //.OrderByDescending(s => s.StockId)
            //.ToPagedList<Stock>(pageNo, PageSize);
            //return result;
            List<StocksReportVM> Result = new List<StocksReportVM>();
            if (ApplyDateFilter)
            {
                Result = (from s in cxt.Stocks
                          join si in cxt.StockItems on s equals si.Stock
                          where s.IsActive
                          && (s.CreatedAt) >= (FromDate)
                          && (s.CreatedAt) <= (ToDate)
                          && si.IsActive
                          && si.Quantity > 0
                          select new StocksReportVM
                          {
                              StockId = s.StockId,
                              DocumentNo = s.DocumentNo,
                              SupplierIvoiceDate = s.SupplierIvoiceDate,
                              CreatedAt = s.CreatedAt,
                              ItemName = si.Item.ItemName,
                              Quantity = si.Quantity,
                              UnitCost = si.UnitCost,
                              TotalCost = si.TotalCost,
                              RetailPrice = si.RetailPrice,
                              BatchName = si.Batch.BatchName,
                              Discount = si.Discount,
                              DiscountType = si.DiscountType,
                              SalesTax = si.SalesTax,
                              NetValue = si.NetValue,
                              ItemCreatedAt = si.CreatedAt
                          }).OrderByDescending(s => s.StockId).ToList();
            }
            else
            {
                Result = (from s in cxt.Stocks
                          join si in cxt.StockItems on s equals si.Stock
                          where s.IsActive
                          && si.IsActive
                          && si.Quantity > 0
                          select new StocksReportVM
                          {
                              StockId = s.StockId,
                              DocumentNo = s.DocumentNo,
                              SupplierIvoiceDate = s.SupplierIvoiceDate,
                              CreatedAt = s.CreatedAt,
                              ItemName = si.Item.ItemName,
                              Quantity = si.Quantity,
                              UnitCost = si.UnitCost,
                              TotalCost = si.TotalCost,
                              RetailPrice = si.RetailPrice,
                              BatchName = si.Batch.BatchName,
                              Discount = si.Discount,
                              DiscountType = si.DiscountType,
                              SalesTax = si.SalesTax,
                              NetValue = si.NetValue,
                              ItemCreatedAt = si.CreatedAt
                          }).OrderByDescending(s => s.StockId).ToList();
            }
            return Result;
        }
        public List<StocksReportVM> GetStocks_Print(DateTime FromDate, DateTime ToDate, string ItemName)
        {
            //IPagedList<Stock> result = cxt.Stocks
            //.Where(s => s.IsActive == true)
            //.Where(s => (s.CreatedAt) >= (FromDate))
            //.Where(s => (s.CreatedAt) <= (ToDate))
            //.Where(s => s.StockItems.Any(i => i.Item.ItemName.ToLower().Contains(NameFilter)))
            //.Include(s => s.StockItems.Select(i => i.Item))
            //.Include(s => s.StockItems.Select(i => i.Batch))
            //.Include(s => s.Supplier)
            //.OrderByDescending(s => s.StockId)
            //.ToPagedList<Stock>(pageNo, PageSize);
            //return result;
            List<StocksReportVM> Result = (from s in cxt.Stocks
                                           join si in cxt.StockItems on s equals si.Stock
                                           where s.IsActive
                                           && (s.CreatedAt) >= (FromDate)
                                           && (s.CreatedAt) <= (ToDate)
                                           && si.IsActive
                                           && si.Quantity > 0
                                           && si.Item.ItemName.ToLower().Contains(ItemName)
                                           select new StocksReportVM
                                           {
                                               StockId = s.StockId,
                                               DocumentNo = s.DocumentNo,
                                               SupplierIvoiceDate = s.SupplierIvoiceDate,
                                               CreatedAt = s.CreatedAt,
                                               ItemName = si.Item.ItemName,
                                               Quantity = si.Quantity,
                                               UnitCost = si.UnitCost,
                                               TotalCost = si.TotalCost,
                                               RetailPrice = si.RetailPrice,
                                               BatchName = si.Batch.BatchName,
                                               Discount = si.Discount,
                                               DiscountType = si.DiscountType,
                                               SalesTax = si.SalesTax,
                                               NetValue = si.NetValue,
                                               ItemCreatedAt = si.CreatedAt
                                           }).OrderByDescending(s => s.StockId).ToList();
            return Result;
        }
        public StockVM GetStockDetailById(long StockId)
        {
            //IPagedList<Stock> result = cxt.Stocks
            //.Where(s => s.IsActive == true)
            //.Where(s => (s.CreatedAt) >= (FromDate))
            //.Where(s => (s.CreatedAt) <= (ToDate))
            //.Include(s => s.StockItems.Select(i => i.Item))
            //.Include(s => s.StockItems.Select(i => i.Batch))
            //.Include(s => s.Supplier)
            //.OrderByDescending(s => s.StockId)
            //.ToPagedList<Stock>(pageNo, PageSize);
            //return result;

            StockVM result = cxt.Stocks
              .Where(s => s.StockId == StockId)
              .Where(s => s.StockItems.Any(si => si.IsActive && si.Quantity > 0))
              .Select(s => new StockVM
              {
                  StockId = s.StockId,
                  DocumentNo = s.DocumentNo,
                  SupplierInvoiceNo = s.SupplierInvoiceNo,
                  ImagePath = s.ImagePath,
                  CreatedAt = s.CreatedAt,
                  UpdatedAt = s.UpdatedAt,
                  IsActive = s.IsActive,
                  SupplierIvoiceDate = s.SupplierIvoiceDate,
                  StockItems = s.StockItems.Where(si => si.IsActive && si.Quantity > 0).Select(si => new StockItemVM
                  {
                      IsActive = si.IsActive,
                      ItemName = si.Item.ItemName,
                      BatchName = si.Batch.BatchName,
                      Quantity = si.Quantity,
                      UnitCost = si.UnitCost,
                      RetailPrice = si.RetailPrice,
                      TotalCost = si.TotalCost,
                      Discount = si.Discount,
                      SalesTax = si.SalesTax,
                      NetValue = si.NetValue,
                      DiscountType = si.DiscountType,
                      SalesTaxType = si.SalesTaxType,
                      CreatedAt = si.CreatedAt,
                      StockConsumed = cxt.StockConsumptionItems.Where(c => c.Item == si.Item && c.Batch == si.Batch && c.IsActive && c.Quantity > 0).Select(c => c.Quantity).FirstOrDefault()
                  }).ToList(),
                  Supplier = s.Supplier
              })
              .OrderByDescending(s => s.StockId).FirstOrDefault();

            return result;
        }
        public StockVM GetStockById_Show(long StockId)
        {
            //Stock result = cxt.Stocks
            //    .Where(s => s.StockId == StockId)
            //    .Include(s => s.StockItems.Select(i => i.Item))
            //    .Include(s => s.StockItems.Select(i => i.Batch))
            //    .Include(s => s.Supplier)
            //    .OrderByDescending(s => s.StockId).FirstOrDefault();
            //return result;

            StockVM result = cxt.Stocks
                .Where(s => s.StockId == StockId)
              .Select(s => new StockVM
              {
                  StockId = s.StockId,
                  DocumentNo = s.DocumentNo,
                  SupplierInvoiceNo = s.SupplierInvoiceNo,
                  ImagePath = s.ImagePath,
                  CreatedAt = s.CreatedAt,
                  UpdatedAt = s.UpdatedAt,
                  IsActive = s.IsActive,
                  SupplierIvoiceDate = s.SupplierIvoiceDate,
                  StockItems = s.StockItems.Where(si => si.IsActive).Select(si => new StockItemVM
                  {
                      IsActive = si.IsActive,
                      StockItemId = si.StockItemId,
                      Item = si.Item,
                      Unit = si.Unit,
                      UnitString = si.Unit == 0 ? si.Item.Unit : "Units",
                      ConversionUnit = si.Item.ConversionUnit,
                      Batch = si.Batch,
                      Quantity = si.Quantity,
                      BonusQuantity = si.BonusQuantity,
                      UnitCost = si.UnitCost,
                      RetailPrice = si.RetailPrice,
                      TotalCost = si.TotalCost,
                      Discount = si.Discount,
                      BonusDiscount = si.BonusDiscount,
                      SalesTax = si.SalesTax,
                      NetValue = si.NetValue,
                      DiscountType = si.DiscountType,
                      DiscountVal = si.DiscountValue,
                      PercDiscType = si.PercDiscType,
                      SalesTaxType = si.SalesTaxType,
                      SalesTaxVal = si.SalesTaxValue,
                      PercSalesTaxType = si.PercSalesTaxType,
                      CreatedAt = si.CreatedAt,
                      StockConsumed = cxt.StockConsumptionItems.Where(c => c.Item == si.Item && c.Batch == si.Batch && c.IsActive && c.Quantity > 0).Select(c => c.Quantity).FirstOrDefault()
                  }).ToList(),
                  Supplier = s.Supplier
              }).FirstOrDefault();

            return result;
        }

        public Stock GetStockById_Edit(long StockId)
        {
            Stock result = cxt.Stocks
                .Where(s => s.StockId == StockId)
                .Include(s => s.StockItems.Select(i => i.Item))
                .Include(s => s.StockItems.Select(i => i.Batch))
                .Include(s => s.Supplier)
                .OrderByDescending(s => s.StockId).FirstOrDefault();
            return result;
        }

        public List<Stock> GetActiveStocksParentOnly()
        {
            return cxt.Stocks.Where(s => s.IsActive == true).Include(s => s.Supplier).ToList();
        }

        public List<StockItemVM> GetActiveStockItems()
        {
            //return cxt.StockItems.Where(i => i.IsActive == true).Include(i=>i.Stock).Include(i=>i.Item).ToList();

            return cxt.StockItems.Where(i => i.IsActive == true)
                  .Select(item => new StockItemVM
                  {
                      ItemName = item.Item.ItemName,
                      Quantity = item.Quantity,
                      UnitCost = item.UnitCost,
                      TotalCost = item.TotalCost,
                      RetailPrice = item.RetailPrice,
                      BatchName = item.Batch.BatchName,
                      Discount = item.Discount,
                      SalesTax = item.SalesTax,
                      NetValue = item.NetValue,
                      CreatedAt = item.CreatedAt,
                      StockId = item.Stock.StockId
                  }).ToList();

        }

        public void SetInactive(long StockId, DateTime date)
        {
            cxt.Database.ExecuteSqlRaw("Update Stocks Set IsActive = 0, issynced = 0, IsUpdate = 1, UpdatedAt = @UpdatedAt WHERE StockId = @StockId Update StockItems Set IsActive = 0 WHERE StockId = @StockId", new SqlParameter("@StockId", StockId), new SqlParameter("@UpdatedAt", date));
        }

        public void InsertStockPayments(List<Expense> PaymentsList)
        {
            //            string qry = "";                
            //            foreach (InvoicePayment pmnt in PaymentsList)
            //            {
            //                qry = @"insert into  Invoicepayments (paymentType, payment, Date, RefundReason,  IsActive, IsNew, IsUpdate, IsSynced, UpdatedAt, StockId) 
            //                        values(" + pmnt.PaymentType + ", " + pmnt.Payment + ", '" + pmnt.Date.ToString("yyyy-MM-dd hh:mm:ss tt")  + "', " + "''" + ", " + 1 + ", " + 1 + ", " + 0 + ", " + 0 + ",'" + pmnt.UpdatedAt.ToString("yyyy-MM-dd hh:mm:ss tt") + "', " + pmnt.StockId + ")";
            //            }
            //            cxt.Database.ExecuteSqlCommand(qry);

            string qry = "";
            foreach (Expense pmnt in PaymentsList)
            {
                qry = @"insert into  Expenses (Description, PaymentMode, Date, CreatedAt,  UpdatedAt, PracticeId, VoucherNo, AutoAdded, Amount, StockId, Supplier_SupplierId, IsActive, IsNew, IsUpdate, IsSynced) 
                        values('"
                                  + pmnt.description + "', "
                                  + pmnt.PaymentMode + ", '"
                                  + pmnt.Date.ToString("yyyy-MM-dd hh:mm:ss tt") + "', '"
                                  + pmnt.Date.ToString("yyyy-MM-dd hh:mm:ss tt") + "', '"
                                  + pmnt.Date.ToString("yyyy-MM-dd hh:mm:ss tt") + "', "
                                  + pmnt.PracticeId + ", "
                                  + pmnt.VoucherNo + ", ";
                qry += pmnt.AutoAdded ? "1," : "0,";
                qry += pmnt.Amount + ", "
                + pmnt.StockId + ", "
                + pmnt.SupplierId + ", ";
                qry += pmnt.IsActive ? "1," : "0,";
                qry += pmnt.IsNew ? "1," : "0,";
                qry += pmnt.IsUpdate ? "1," : "0,";
                qry += pmnt.IsSynced ? "1" : "0";
                qry += ")";
            }
            cxt.Database.ExecuteSqlRaw(qry);
        }

        public PurchaseReportsMasterVM GetAllPurchases(DateTime DateFrom, DateTime DateTo, int PageNo, int PageSize)
        {
            double TotalPurchaseAMount = cxt.Stocks
            .Where(s => s.IsActive
                    && (s.CreatedAt) >= (DateFrom)
                    && (s.CreatedAt) <= (DateTo)
                    && s.Supplier != null
                    )
                .Sum(s => (double?)s.StockItems
                        .Where(item => item.IsActive)
                        .Sum(item => (double?)item.NetValue)) ?? 0;
            IPagedList<PurchaseReportVM> PurchaseReports = cxt.Stocks
                 .Where(s => s.IsActive
                         && (s.CreatedAt) >= (DateFrom)
                         && (s.CreatedAt) <= (DateTo)
                         && s.Supplier != null)
                         .Select(s => new PurchaseReportVM
                         {
                             StockId = s.StockId,
                             DocumentNo = s.DocumentNo,
                             Supplier = s.Supplier,
                             StockDate = s.CreatedAt,
                             SupplierInvoiceNo = s.SupplierInvoiceNo,
                             SupplierInvoiceDate = s.SupplierIvoiceDate,
                             Amount = s.GrandTotal, // s.StockItems.Where(i => i.IsActive).Sum(i => (double?)i.NetValue) ?? 0,
                             Paid = s.Expenses.Where(p => p.IsActive).Sum(p => (double?)p.Amount) ?? 0
                             //LastPayment = s.Expenses.Where(p => p.IsActive).OrderByDescending(p => p.ExpenseId).FirstOrDefault()
                         }).OrderByDescending(r => r.StockId).ToPagedList(PageNo, PageSize);
            return new PurchaseReportsMasterVM
            {
                TotalPurchaseAmount = TotalPurchaseAMount,
                //PurchaseReports = PurchaseReports
            };
        }
        public PurchaseReportsMasterVM GetAllPurchases(long SupplierId, int PageNo, int PageSize)
        {
            double TotalPurchaseAMount = cxt.Stocks
            .Where(s => s.IsActive
                    && s.Supplier.SupplierID == SupplierId
                    )
                .Sum(s => (double?)s.StockItems
                        .Where(item => item.IsActive)
                        .Sum(item => (double?)item.NetValue)) ?? 0;
            IPagedList<PurchaseReportVM> PurchaseReports = cxt.Stocks
                 .Where(s => s.IsActive
                         && s.Supplier.SupplierID == SupplierId
                         && s.Supplier != null)
                         .Select(s => new PurchaseReportVM
                         {
                             StockId = s.StockId,
                             DocumentNo = s.DocumentNo,
                             Supplier = s.Supplier,
                             StockDate = s.CreatedAt,
                             SupplierInvoiceNo = s.SupplierInvoiceNo,
                             SupplierInvoiceDate = s.SupplierIvoiceDate,
                             Amount = s.GrandTotal,// s.StockItems.Where(i => i.IsActive).Sum(i => (double?)i.NetValue) ?? 0,
                             Paid = s.Expenses.Where(p => p.IsActive).Sum(p => (double?)p.Amount) ?? 0,
                             //LastPayment = s.Expenses.Where(p => p.IsActive).OrderByDescending(p => p.ExpenseId).FirstOrDefault()
                         }).OrderByDescending(r => r.StockId).ToPagedList(PageNo, PageSize);
            return new PurchaseReportsMasterVM
            {
                TotalPurchaseAmount = TotalPurchaseAMount,
                //PurchaseReports = PurchaseReports
            };
        }
        public List<PurchaseReportVM> GetAllPurchases_Rpt(DateTime DateFrom, DateTime DateTo)
        {
            List<PurchaseReportVM> Reports = cxt.Stocks
                 .Where(s => s.IsActive
                         && (s.CreatedAt) >= (DateFrom)
                         && (s.CreatedAt) <= (DateTo)
                         && s.Supplier != null)
                         .Select(s => new PurchaseReportVM
                         {
                             StockId = s.StockId,
                             DocumentNo = s.DocumentNo,
                             SupplierName = s.Supplier.Name,
                             StockDate = s.CreatedAt,
                             SupplierInvoiceNo = s.SupplierInvoiceNo,
                             SupplierInvoiceDate = s.SupplierIvoiceDate,
                             Amount = s.GrandTotal,// s.StockItems.Where(i => i.IsActive).Sum(i => (double?)i.NetValue) ?? 0,
                             Paid = s.Expenses.Where(p => p.IsActive).Sum(p => (double?)p.Amount) ?? 0,
                             //LastPayment = s.Expenses.Where(p => p.IsActive).OrderByDescending(p => p.ExpenseId).FirstOrDefault()
                         }).OrderByDescending(r => r.StockId).ToList();
            foreach (PurchaseReportVM p in Reports)
            {
                p.Due = p.Amount - p.Paid;
            }
            return Reports;
        }
        public List<PurchaseReportVM> GetAllPurchases_Rpt(long SupplierId)
        {
            List<PurchaseReportVM> Reports = cxt.Stocks
                .Where(s => s.IsActive
                        && s.Supplier.SupplierID == SupplierId
                        && s.Supplier != null)
                        .Select(s => new PurchaseReportVM
                        {
                            StockId = s.StockId,
                            DocumentNo = s.DocumentNo,
                            Supplier = s.Supplier,
                            StockDate = s.CreatedAt,
                            SupplierInvoiceNo = s.SupplierInvoiceNo,
                            SupplierInvoiceDate = s.SupplierIvoiceDate,
                            Amount = s.GrandTotal, // s.StockItems.Where(i => i.IsActive).Sum(i => (double?)i.NetValue) ?? 0,
                            Paid = s.Expenses.Where(p => p.IsActive).Sum(p => (double?)p.Amount) ?? 0
                            //LastPayment = s.Expenses.Where(p => p.IsActive).OrderByDescending(p => p.ExpenseId).FirstOrDefault()
                        }).OrderByDescending(r => r.StockId).ToList();
            foreach (PurchaseReportVM p in Reports)
            {
                p.Due = p.Amount - p.Paid;
            }
            return Reports;
        }
        public StockStatisticsParentVM GetStockStatistics(DateTime DateFrom, DateTime DateTo)
        {
            StockStatisticsParentVM Result = new StockStatisticsParentVM();
            //Result.StockStatisticsList = (from s in cxt.Stocks
            //                              join si in cxt.StockItems on s equals si.Stock
            //                              where s.IsActive
            //                              && (s.CreatedAt) >= (DateFrom)
            //                              && (s.CreatedAt) <= (DateTo)
            //                              group s by (s.CreatedAt) into g
            //                              select new StockStatisticsVM
            //                              {
            //                                  MonthDay = g.FirstOrDefault().CreatedAt.Day,
            //                                  TotalRetailValue = g.Sum(r => r.StockItems.Sum(sit => sit.RetailPrice * sit.Quantity)),
            //                                  TotalCostValue = g.Sum(r => r.StockItems.Sum(si => si.TotalCost)),

            //                              }).ToList();

            SummationsVM res = new SummationsVM
            {
                //RV_T_Stock = (cxt.StockItems.Where(si => si.IsActive).Sum(si => ((int?)si.Quantity + si.BonusQuantity) * si.RetailPrice) ?? 0),
                RV_T_Stock = (from s in cxt.Stocks join si in cxt.StockItems on s.StockId equals si.Stock.StockId where (s.StockDate) >= (DateFrom) && (s.StockDate) <= (DateTo) && si.IsActive select new { Quantity = si.Quantity + si.BonusQuantity, RetailPrice = (from im in cxt.StockItems where im.IsActive && im.Item.ItemId == si.Item.ItemId && im.Batch.BatchId == si.Batch.BatchId orderby im.StockItemId descending select im.RetailPrice).FirstOrDefault() }).Sum(si => (double?)(si.Quantity * si.RetailPrice)) ?? 0,
                RV_T_C_Stock = (from sc in cxt.StockConsumptionItems where (sc.CreatedAt) >= (DateFrom) && (sc.CreatedAt) <= (DateTo) && sc.IsActive select new { sc.Quantity, RetailPrice = (from si in cxt.StockItems where si.Item.ItemId == sc.Item.ItemId && si.Batch.BatchId == sc.Batch.BatchId orderby si.StockItemId descending select si.RetailPrice).FirstOrDefault() }).Sum(r => (double?)r.Quantity * r.RetailPrice) ?? 0,
                RV_T_A_Stock = (from a in cxt.Adjustments join ai in cxt.AdjustmentItems on a.AdjustmentId equals ai.Adjustment.AdjustmentId where (a.CreatedAt) >= (DateFrom) && (a.CreatedAt) <= (DateTo) && ai.IsActive select new { ai.Quantity, RetailPrice = (from si in cxt.StockItems where si.Item.ItemId == ai.Item.ItemId && si.Batch.BatchId == ai.Batch.BatchId orderby si.StockItemId descending select si.RetailPrice).FirstOrDefault() }).Sum(r => (double?)r.Quantity * r.RetailPrice) ?? 0,
                RV_T_E_Stock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && b.Expiry != null && (b.Expiry) < (DateTime.Now) orderby s.StockItemId descending select new { Quantity = s.Quantity + s.BonusQuantity, RetailPrice = (from si in cxt.StockItems where si.Item.ItemId == s.Item.ItemId && s.Batch.BatchId == si.Batch.BatchId orderby s.StockItemId descending select si.RetailPrice).FirstOrDefault() }).Sum(r => (double?)(r.Quantity * r.RetailPrice)) ?? 0,
                RV_T_C_E_Stock = (from s in cxt.StockConsumptionItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && b.Expiry != null && (b.Expiry) < (DateTime.Now) orderby s.StockConsumptionItemId descending select new { s.Quantity, RetailPrice = (from si in cxt.StockItems where si.Item.ItemId == s.Item.ItemId && s.Batch.BatchId == si.Batch.BatchId orderby si.StockItemId descending select si.RetailPrice).FirstOrDefault() }).Sum(r => (double?)(r.Quantity * r.RetailPrice)) ?? 0,
                //CV_T_Stock = (cxt.StockItems.Where(si => si.IsActive).Sum(si => ((int?)si.Quantity + si.BonusQuantity) * si.UnitCost) ?? 0),
                CV_T_Stock = (from s in cxt.Stocks join si in cxt.StockItems on s.StockId equals si.Stock.StockId where (s.CreatedAt) >= (DateFrom) && (s.CreatedAt) <= (DateTo) && si.IsActive select new { Quantity = si.Quantity + si.BonusQuantity, CostPrice = (from im in cxt.StockItems where im.IsActive && im.Item.ItemId == si.Item.ItemId && im.Batch.BatchId == si.Batch.BatchId orderby im.StockItemId descending select im.UnitCost).FirstOrDefault() }).Sum(si => (double?)(si.Quantity * si.CostPrice)) ?? 0,
                CV_T_C_Stock = (from sc in cxt.StockConsumptionItems where (sc.CreatedAt) >= (DateFrom) && (sc.CreatedAt) <= (DateTo) && sc.IsActive select new { sc.Quantity, CostPrice = (from si in cxt.StockItems where si.Item.ItemId == sc.Item.ItemId && si.Batch.BatchId == sc.Batch.BatchId orderby si.StockItemId descending select si.UnitCost).FirstOrDefault() }).Sum(r => (double?)r.Quantity * r.CostPrice) ?? 0,
                CV_T_A_Stock = (from a in cxt.Adjustments join ai in cxt.AdjustmentItems on a.AdjustmentId equals ai.Adjustment.AdjustmentId where (a.CreatedAt) >= (DateFrom) && (a.CreatedAt) <= (DateTo) && ai.IsActive select new { ai.Quantity, CostPrice = (from si in cxt.StockItems where si.Item.ItemId == ai.Item.ItemId && si.Batch.BatchId == ai.Batch.BatchId orderby si.StockItemId descending select si.UnitCost).FirstOrDefault() }).Sum(r => (double?)r.Quantity * r.CostPrice) ?? 0,
                CV_T_E_Stock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && b.Expiry != null && (b.Expiry) < (DateTime.Now) orderby s.StockItemId descending select new { Quantity = s.Quantity + s.BonusQuantity, UnitCost = (from si in cxt.StockItems where si.Item.ItemId == s.Item.ItemId && s.Batch.BatchId == si.Batch.BatchId orderby s.StockItemId descending select si.UnitCost).FirstOrDefault() }).Sum(r => (double?)(r.Quantity * r.UnitCost)) ?? 0,
                CV_T_C_E_Stock = (from s in cxt.StockConsumptionItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && b.Expiry != null && (b.Expiry) < (DateTime.Now) orderby s.StockConsumptionItemId descending select new { s.Quantity, UnitCost = (from si in cxt.StockItems where si.Item.ItemId == s.Item.ItemId && s.Batch.BatchId == si.Batch.BatchId orderby si.StockItemId descending select si.UnitCost).FirstOrDefault() }).Sum(r => (double?)(r.Quantity * r.UnitCost)) ?? 0,
            };
            if (res != null)
            {
                Result.TotalRetValOfAvailableStock = res.RV_T_Stock - res.RV_T_C_Stock + res.RV_T_A_Stock - (res.RV_T_E_Stock - res.RV_T_C_E_Stock);
                Result.TotalCostValOfAvailableStock = res.CV_T_Stock - res.CV_T_C_Stock + res.CV_T_A_Stock - (res.CV_T_E_Stock - res.CV_T_C_E_Stock);
            }
            return Result;
        }
        public void TestMethod(DateTime DateFrom, DateTime DateTo)
        {
            var Result = from s in cxt.Stocks
                          join si in cxt.StockItems on s equals si.Stock
                          where s.IsActive
                          && (s.CreatedAt) >= (DateFrom)
                          && (s.CreatedAt) <= (DateTo)
                          group s by (s.CreatedAt) into g
                          select g.ToList();
        }

        public List<SaleStatisticsVM> GetSalesStatistics(DateTime? startDate, DateTime endDate)
        {
            return (from i in cxt.Invoices
                    where i.IsActive
                    && (i.CreatedAt) >= (startDate)
                    && (i.CreatedAt) <= (endDate)
                    group i by (i.CreatedAt) into g
                    select new SaleStatisticsVM
                    {
                        MonthDay = g.FirstOrDefault().CreatedAt.Day,
                        TotalSales = g.Sum(i => i.GrandTotal)
                    }).ToList();
        }

        public List<SaleStatisticsVM> GetRevenuStatistics(DateTime? startDate, DateTime endDate)
        {
            //return (from i in cxt.Invoices
            //        where i.IsActive
            //        && (i.CreatedAt) >= (startDate)
            //        && (i.CreatedAt) <= (endDate)
            //        group i by (i.CreatedAt) into g
            //        select new SaleStatisticsVM
            //        {
            //            MonthDay = g.FirstOrDefault().CreatedAt.Day,
            //            TotalSales = g.Sum(i => i.InvoicePayments.Sum())
            //        }).ToList();
            return null;
        }
        public List<StockGraphVM> GetRetailValueOfAvailableStock(DateTime DateFrom, DateTime DateTo)
        {
            List<StockGraphVM> Result = new List<StockGraphVM>();
            Result = (from s in cxt.Stocks
                      join si in cxt.StockItems on s equals si.Stock
                      where s.IsActive
                      && (s.CreatedAt) >= (DateFrom)
                      && (s.CreatedAt) <= (DateTo)
                      group s by (s.CreatedAt) into g
                      select new StockGraphVM
                      {
                          MonthDay = g.FirstOrDefault().CreatedAt.Day,
                          GrandTotal = g.Sum(r => r.StockItems.Sum(sit => sit.RetailPrice * sit.Quantity)),
                      }).ToList();
            return Result;
        }
        public List<StockGraphVM> GetCostValueOfAvailableStock(DateTime DateFrom, DateTime DateTo)
        {
            List<StockGraphVM> Result = new List<StockGraphVM>();
            Result = (from s in cxt.Stocks
                      join si in cxt.StockItems on s equals si.Stock
                      where s.IsActive
                      && (s.CreatedAt) >= (DateFrom)
                      && (s.CreatedAt) <= (DateTo)
                      group s by (s.CreatedAt) into g
                      select new StockGraphVM
                      {
                          MonthDay = g.FirstOrDefault().CreatedAt.Day,
                          GrandTotal = g.Sum(r => r.StockItems.Sum(sit => sit.TotalCost)),
                      }).ToList();
            return Result;
        }

        public Batch GetRequiredBatch(int ItemId)
        {
            if (SharedVariables.AdminPharmacySetting.IsItemConumptionFifo)
            {
                return cxt.StockItems.Where(i => i.Item.ItemId == ItemId && i.IsActive).OrderBy(i => i.StockItemId).Select(r => r.Batch).FirstOrDefault();
            }
            else
            {
                return cxt.StockItems.Where(i => i.Item.ItemId == ItemId && i.IsActive).OrderByDescending(i => i.StockItemId).Select(r => r.Batch).FirstOrDefault();
            }
        }

        public bool anyActiveStockExists()
        {
            return cxt.Stocks.Any(s => s.IsActive);
        }

        public void deleteStocks()
        {
        }

        public StockPaymentWithItemDetails GetStockPaymentPrint(long StockId)
        {
            //IPagedList<Stock> result = cxt.Stocks
            //.Where(s => s.IsActive == true)
            //.Where(s => (s.CreatedAt) >= (FromDate))
            //.Where(s => (s.CreatedAt) <= (ToDate))
            //.Where(s => s.StockItems.Any(i => i.Item.ItemName.ToLower().Contains(NameFilter)))
            //.Include(s => s.StockItems.Select(i => i.Item))
            //.Include(s => s.StockItems.Select(i => i.Batch))
            //.Include(s => s.Supplier)
            //.OrderByDescending(s => s.StockId)
            //.ToPagedList<Stock>(pageNo, PageSize);
            //return result;
            StockPaymentWithItemDetails Result = cxt.Stocks
                                .Where(s => s.StockId == StockId)
                                .Select(s => new StockPaymentWithItemDetails
                                           {
                                               DocumentNo = s.DocumentNo,
                                               SupplierIvoiceDate = s.SupplierIvoiceDate,
                                               StockDate = s.StockDate,
                                               SupplierInvoiceNo = s.SupplierInvoiceNo,
                                               SupplierName = s.Supplier != null ? s.Supplier.Name : "",
                                               GrandTotal = s.GrandTotal,
                                               TotalPaid = s.TotalPaid,
                                               UserName = s.User.UserName,
                                               Items = s.StockItems.Where(si => si.IsActive)
                                               .Select(si => new StockPayment_ItemsVM
                                               {
                                                   ItemName = si.Item.ItemName,
                                                   Quantity = si.Quantity,
                                                   UnitCost = si.UnitCost,
                                                   TotalCost = si.TotalCost,
                                                   RetailPrice = si.RetailPrice,
                                                   DiscountVal = si.DiscountValue,
                                                   NetValue = si.NetValue,
                                               }).ToList()
                                           }).FirstOrDefault();
            return Result;
        }

        public Tuple<double, double> GetStockPaymentTotals(long StockId)
        {
            //IPagedList<Stock> result = cxt.Stocks
            //.Where(s => s.IsActive == true)
            //.Where(s => (s.CreatedAt) >= (FromDate))
            //.Where(s => (s.CreatedAt) <= (ToDate))
            //.Where(s => s.StockItems.Any(i => i.Item.ItemName.ToLower().Contains(NameFilter)))
            //.Include(s => s.StockItems.Select(i => i.Item))
            //.Include(s => s.StockItems.Select(i => i.Batch))
            //.Include(s => s.Supplier)
            //.OrderByDescending(s => s.StockId)
            //.ToPagedList<Stock>(pageNo, PageSize);
            //return result;
            var Result = cxt.Stocks
                                .Where(s => s.StockId == StockId)
                                .Select(s => new
                                {
                                    TotalAmount = s.GrandTotal,
                                    TotalPaid = (from e in cxt.Expenses where e.StockId == StockId && e.IsActive select e.Amount).Sum(r => (double?)r) ?? 0
                                }).FirstOrDefault();
            return new Tuple<double, double>(Result.TotalAmount, Result.TotalPaid);
        }
    }
}