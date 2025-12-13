

using System.Data;
using static Restaurant_MS_Core.Enums;

namespace Restaurant_MS_Infrastructure.Repository
{

    public class StockConsumptionRepository : Repository<StockConsumption>
    {
        AppDbContext cxt;
        public StockConsumptionRepository(AppDbContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }

        public IPagedList<StockConsumptionVM> GetStockConsumptions(DateTime StartDate, DateTime EndDate, int PageNo, int PageSize)
        {
            return cxt.StockConsumptions
            .Where(s => s.CreatedAt.Date >= StartDate.Date)
            .Where(s => s.CreatedAt.Date <= EndDate.Date)
            .Where(s => s.IsActive)
            .Select(s => new StockConsumptionVM
            {
                StockConsumptionId = s.StockConsumptionId,
                //Invoice = s.Invoice,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt,
                Comment = s.Comment,
                UserName = s.User.UserName,
                StockConsumptionsList = s.StockConsumptionItems.Select(l => new StockConsumptionItemVM
                {
                    ItemId = l.Item.ItemId,
                    StockConsumptionItemId = l.StockConsumptionItemId,
                    UnitString = l.Unit == 0 ? l.Item.Unit : "Units",
                    ConversionUnit = l.Item.ConversionUnit,
                    ItemName = l.Item.ItemName,
                    Quantity = l.Unit == 0 ? l.Quantity / l.Item.ConversionUnit : l.Quantity,
                    BatchName = l.Batch.BatchName,
                    ConsumptionType = l.ConsumptionType,
                    ConsumptionTypeString = ((ConsumptionType)l.ConsumptionType).ToString(),
                    TotalCost = cxt.StockItems.Where(i => i.Item.ItemId == l.Item.ItemId && i.Batch.BatchId == l.Batch.BatchId).OrderByDescending(item => item.StockItemId).Select(item => item.UnitCost).FirstOrDefault() * l.Quantity,
                    CreatedAt = l.CreatedAt,
                    IsActive = l.IsActive,
                    User = l.User
                }).ToList()
            }).OrderByDescending(r => r.StockConsumptionId).ToPagedList(PageNo, PageSize);
        }
        public IPagedList<StockConsumptionVM> GetStockConsumptions(DateTime StartDate, DateTime EndDate, int UserId, int PageNo, int PageSize)
        {
            return cxt.StockConsumptions
            .Where(s => s.CreatedAt.Date >= StartDate.Date)
            .Where(s => s.CreatedAt.Date <= EndDate.Date)
            .Where(s => s.IsActive)
            .Where(s => s.User.UserId == UserId)
            .Select(s => new StockConsumptionVM
            {
                StockConsumptionId = s.StockConsumptionId,
                //Invoice = s.Invoice,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt,
                Comment = s.Comment,
                UserName = s.User.UserName,
                StockConsumptionsList = s.StockConsumptionItems.Select(l => new StockConsumptionItemVM
                {
                    ItemId = l.Item.ItemId,
                    StockConsumptionItemId = l.StockConsumptionItemId,
                    UnitString = l.Unit == 0 ? l.Item.Unit : "Units",
                    ConversionUnit = l.Item.ConversionUnit,
                    ItemName = l.Item.ItemName,
                    Quantity = l.Unit == 0 ? l.Quantity / l.Item.ConversionUnit : l.Quantity,
                    BatchName = l.Batch.BatchName,
                    ConsumptionType = l.ConsumptionType,
                    ConsumptionTypeString = ((ConsumptionType)l.ConsumptionType).ToString(),
                    TotalCost = cxt.StockItems.Where(i => i.Item.ItemId == l.Item.ItemId && i.Batch.BatchId == l.Batch.BatchId).OrderByDescending(item => item.StockItemId).Select(item => item.UnitCost).FirstOrDefault() * l.Quantity,
                    CreatedAt = l.CreatedAt,
                    IsActive = l.IsActive,
                    User = l.User
                }).ToList()
            }).OrderByDescending(r => r.StockConsumptionId).ToPagedList(PageNo, PageSize);
        }
        public IPagedList<StockConsumptionVM> GetStockConsumptions(int ItemId, int PageNo, int PageSize)
        {
            return cxt.StockConsumptions
            .Where(s => s.IsActive && s.StockConsumptionItems.Any(si => si.IsActive && si.Item.ItemId == ItemId))
            .Select(s => new StockConsumptionVM
            {
                StockConsumptionId = s.StockConsumptionId,
                //Invoice = s.Invoice,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt,
                Comment = s.Comment,
                UserName = s.User.UserName,
                StockConsumptionsList = s.StockConsumptionItems.Select(l => new StockConsumptionItemVM
                {
                    ItemId = l.Item.ItemId,
                    StockConsumptionItemId = l.StockConsumptionItemId,
                    UnitString = l.Unit == 0 ? l.Item.Unit : "Units",
                    ConversionUnit = l.Item.ConversionUnit,
                    ItemName = l.Item.ItemName,
                    Quantity = l.Unit == 0 ? l.Quantity / l.Item.ConversionUnit : l.Quantity,
                    BatchName = l.Batch.BatchName,
                    ConsumptionType = l.ConsumptionType,
                    ConsumptionTypeString = ((ConsumptionType)l.ConsumptionType).ToString(),
                    TotalCost = cxt.StockItems.Where(i => i.Item.ItemId == l.Item.ItemId && i.Batch.BatchId == l.Batch.BatchId).OrderByDescending(item => item.StockItemId).Select(item => item.UnitCost).FirstOrDefault() * l.Quantity,
                    CreatedAt = l.CreatedAt,
                    IsActive = l.IsActive,
                    User = l.User
                }).ToList()
            }).OrderByDescending(r => r.StockConsumptionId).ToPagedList(PageNo, PageSize);
        }
        public IPagedList<StockConsumptionVM> GetStockConsumptions(int PageNo, int PageSize)
        {
            return cxt.StockConsumptions
            .Where(s => s.IsActive)
            .Select(s => new StockConsumptionVM
            {
                StockConsumptionId = s.StockConsumptionId,
                //Invoice = s.Invoice,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt,
                Comment = s.Comment,
                UserName = s.User.UserName,
                StockConsumptionsList = s.StockConsumptionItems.Select(l => new StockConsumptionItemVM
                {
                    ItemId = l.Item.ItemId,
                    StockConsumptionItemId = l.StockConsumptionItemId,
                    ItemName = l.Item.ItemName,
                    UnitString = l.Unit == 0 ? l.Item.Unit : "Units",
                    ConversionUnit = l.Item.ConversionUnit,
                    Quantity = l.Unit == 0 ? l.Quantity / l.Item.ConversionUnit : l.Quantity,
                    BatchName = l.Batch.BatchName,
                    ConsumptionType = l.ConsumptionType,
                    ConsumptionTypeString = ((ConsumptionType)l.ConsumptionType).ToString(),
                    TotalCost = cxt.StockItems.Where(i => i.Item.ItemId == l.Item.ItemId && i.Batch.BatchId == l.Batch.BatchId).OrderByDescending(item => item.StockItemId).Select(item => item.UnitCost).FirstOrDefault() * l.Quantity,
                    CreatedAt = l.CreatedAt,
                    IsActive = l.IsActive,
                    User = l.User
                }).ToList()
            }).OrderByDescending(r => r.StockConsumptionId).ToPagedList(PageNo, PageSize);
        }
        //public void BulkInsert(DataTable dtStockConsumption)
        //{
        //    try
        //    {
        //        SqlBulkCopy objbulk = new SqlBulkCopy(cxt.Database.Connection.ConnectionString);
        //        objbulk.DestinationTableName = "StockConsumptions";
        //        objbulk.ColumnMappings.Add("Quantity", "Quantity");
        //        objbulk.ColumnMappings.Add("ConsumptionType", "ConsumptionType");
        //        objbulk.ColumnMappings.Add("Comment", "Comment");
        //        objbulk.ColumnMappings.Add("Batch_BatchId", "Batch_BatchId");
        //        objbulk.ColumnMappings.Add("Item_ItemID", "Item_ItemID");
        //        objbulk.ColumnMappings.Add("Invoice_InvoiceId", "Invoice_InvoiceId");
        //        objbulk.ColumnMappings.Add("Date", "Date");
        //        objbulk.WriteToServer(dtStockConsumption);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public List<StockConsumptionItem> GetConsumptionsByItemId(int ItemId)
        {
            return cxt.StockConsumptionItems
                .Where(sc => sc.IsActive)
                .Where(s => s.Item.ItemId == ItemId)
                .Include(s => s.Batch).ToList();
        }

        public StockConsumptionItem GetConsumptionById(long StockConsumptionId)
        {
            return cxt.StockConsumptionItems
                .Where(s => s.StockConsumptionItemId == StockConsumptionId)
                .Include(s => s.Item)
                .Include(s => s.Batch).FirstOrDefault();
        }
        public StockConsumptionVM GetStockConsumptionById(long StockConsumptionId)
        {

            return cxt.StockConsumptions
          .Where(s => s.StockConsumptionId == StockConsumptionId)
          .Select(s => new StockConsumptionVM
          {
              StockConsumptionId = s.StockConsumptionId,
              CreatedAt = s.CreatedAt,
              UpdatedAt = s.UpdatedAt,
              User = s.User,
              StockConsumptionsList =
              s.StockConsumptionItems.Select(sc => new StockConsumptionItemVM
              {
                  StockConsumptionItemId = sc.StockConsumptionItemId,
                  Item = sc.Item,
                  Batch = sc.Batch,
                  UnitString = sc.Unit == 0 ? sc.Item.Unit : "Units",
                  ItemName = sc.Item.ItemName,
                  BatchName = sc.Batch.BatchName,
                  Quantity = sc.Quantity,
                  Comment = sc.Comment,
                  ConsumptionType = sc.ConsumptionType,
                  CreatedAt = sc.CreatedAt,
                  UpdatedAt = sc.UpdatedAt,
                  IsActive = sc.IsActive
              }).ToList()
          }).FirstOrDefault();
        }
        public StockConsumption GetStockConumptionById_(long StockConsumptionId)
        {
            return cxt.StockConsumptions
                .Include(c => c.StockConsumptionItems)
                .Include(c => c.StockConsumptionItems.Select(sc => sc.Item))
                .Include(c => c.StockConsumptionItems.Select(sc => sc.Batch))
                .Where(c => c.StockConsumptionId == StockConsumptionId)
                .FirstOrDefault();
        }

        //public IPagedList<StockConsumptionVM> GetStockConsumptions(DateTime StartDate, DateTime EndDate, string NameFilter, string BatchName, int PageNo, int PageSize)
        //{
        //    if (!string.IsNullOrEmpty(NameFilter) && !string.IsNullOrEmpty(BatchName))
        //    {
        //        return cxt.StockConsumptions
        //      .Where(s => DbFunctions.TruncateTime(s.CreatedAt) >= DbFunctions.TruncateTime(StartDate))
        //      .Where(s => DbFunctions.TruncateTime(s.CreatedAt) <= DbFunctions.TruncateTime(EndDate))
        //      .Where(s => s.Item.ItemName.Contains(NameFilter))
        //      .Where(s => s.Batch.BatchName.ToLower() == BatchName)
        //      .Where(s => s.IsActive)
        //      .Select(s => new StockConsumptionVM
        //      {
        //          StockConsumptionId = s.StockConsumptionId,
        //          //InvoiceId = s.Invoice == null ? 0 : s.Invoice.InvoiceId,
        //          ItemName = s.Item.ItemName,
        //          BatchName = s.Batch.BatchName,
        //          Quantity = s.Quantity,
        //          ConsumptionType = s.ConsumptionType,
        //          ConsumptionTypeString = ((GlobalSharing.ConsumptionType)s.ConsumptionType).ToString(),
        //          Comment = s.Comment,
        //          TotalCost = (cxt.StockItems.Where(i => i.Item.ItemId == s.Item.ItemId && i.Batch.BatchId == s.Batch.BatchId).OrderByDescending(item => item.StockItemId).Select(item => item.UnitCost).FirstOrDefault()) * s.Quantity,
        //          CreatedAt = s.CreatedAt
        //      }).OrderByDescending(s => s.StockConsumptionId).ToPagedList(PageNo, PageSize);
        //    }
        //    else if (!string.IsNullOrEmpty(NameFilter))
        //    {
        //        return cxt.StockConsumptions
        //            .Where(s => DbFunctions.TruncateTime(s.CreatedAt) >= DbFunctions.TruncateTime(StartDate))
        //            .Where(s => DbFunctions.TruncateTime(s.CreatedAt) <= DbFunctions.TruncateTime(EndDate))
        //            .Where(s => s.Item.ItemName.Contains(NameFilter))
        //            .Where(s => s.IsActive)
        //            .Select(s => new StockConsumptionVM
        //            {
        //                StockConsumptionId = s.StockConsumptionId,
        //                InvoiceId = s.Invoice == null ? 0 : s.Invoice.InvoiceId,
        //                ItemName = s.Item.ItemName,
        //                BatchName = s.Batch.BatchName,
        //                Quantity = s.Quantity,
        //                ConsumptionType = s.ConsumptionType,
        //                ConsumptionTypeString = ((GlobalSharing.ConsumptionType)s.ConsumptionType).ToString(),
        //                Comment = s.Comment,
        //                TotalCost = (cxt.StockItems.Where(i => i.Item.ItemId == s.Item.ItemId && i.Batch.BatchId == s.Batch.BatchId).OrderByDescending(item => item.StockItemId).Select(item => item.UnitCost).FirstOrDefault()) * s.Quantity,
        //                CreatedAt = s.CreatedAt
        //            }).OrderByDescending(s => s.StockConsumptionId).ToPagedList(PageNo, PageSize);
        //    }
        //    else if (!string.IsNullOrEmpty(BatchName))
        //    {
        //        return cxt.StockConsumptions
        //        .Where(s => DbFunctions.TruncateTime(s.CreatedAt) >= DbFunctions.TruncateTime(StartDate))
        //        .Where(s => DbFunctions.TruncateTime(s.CreatedAt) <= DbFunctions.TruncateTime(EndDate))
        //        .Where(s => s.Batch.BatchName.ToLower() == BatchName)
        //        .Where(s => s.IsActive)
        //        .Select(s => new StockConsumptionVM
        //        {
        //            StockConsumptionId = s.StockConsumptionId,
        //            InvoiceId = s.Invoice == null ? 0 : s.Invoice.InvoiceId,
        //            ItemName = s.Item.ItemName,
        //            BatchName = s.Batch.BatchName,
        //            Quantity = s.Quantity,
        //            ConsumptionType = s.ConsumptionType,
        //            ConsumptionTypeString = ((GlobalSharing.ConsumptionType)s.ConsumptionType).ToString(),
        //            Comment = s.Comment,
        //            TotalCost = (cxt.StockItems.Where(i => i.Item.ItemId == s.Item.ItemId && i.Batch.BatchId == s.Batch.BatchId).OrderByDescending(item => item.StockItemId).Select(item => item.UnitCost).FirstOrDefault()) * s.Quantity,
        //            CreatedAt = s.CreatedAt
        //        }).OrderByDescending(s => s.StockConsumptionId).ToPagedList(PageNo, PageSize);
        //    }
        //    else
        //    {
        //        return cxt.StockConsumptions
        //             .Where(s => DbFunctions.TruncateTime(s.CreatedAt) >= DbFunctions.TruncateTime(StartDate))
        //             .Where(s => DbFunctions.TruncateTime(s.CreatedAt) <= DbFunctions.TruncateTime(EndDate))
        //             .Where(s => s.IsActive)
        //             .Select(s => new StockConsumptionVM
        //             {
        //                 StockConsumptionId = s.StockConsumptionId,
        //                 InvoiceId = s.Invoice == null ? 0 : s.Invoice.InvoiceId,
        //                 ItemName = s.Item.ItemName,
        //                 BatchName = s.Batch.BatchName,
        //                 Quantity = s.Quantity,
        //                 ConsumptionType = s.ConsumptionType,
        //                 ConsumptionTypeString = ((GlobalSharing.ConsumptionType)s.ConsumptionType).ToString(),
        //                 Comment = s.Comment,
        //                 TotalCost = (cxt.StockItems.Where(i => i.Item.ItemId == s.Item.ItemId && i.Batch.BatchId == s.Batch.BatchId).OrderByDescending(item => item.StockItemId).Select(item => item.UnitCost).FirstOrDefault()) * s.Quantity,
        //                 CreatedAt = s.CreatedAt
        //             }).OrderByDescending(s => s.StockConsumptionId).ToPagedList(PageNo, PageSize);
        //    }
        //}

        //public void SetInActive(long StockConsumptionId)
        //{
        //    cxt.Database.ExecuteSqlCommand("Update StockConsumptions Set IsActive = 0 WHERE StockConsumptionId = @StockConsumptionId", new SqlParameter("@StockConsumptionId", StockConsumptionId));
        //}

        public bool AnyActiveConsExists()
        {
            return cxt.StockConsumptions.Any(c => c.IsActive);
        }

        public List<StockConsumptionItem> GetConsStockByBatchId(int BatchId)
        {
            try
            {
                return (from c in cxt.StockConsumptions
                        join ci in cxt.StockConsumptionItems on c.StockConsumptionId equals ci.StockConsumptionItemId
                        where c.IsActive && ci.IsActive && ci.Batch.BatchId == BatchId
                        select ci
                        )
                        .Include(r => r.Batch)
                        .ToList();

            }
            catch (Exception)
            {
                throw;
            }
        }

        
    }
}