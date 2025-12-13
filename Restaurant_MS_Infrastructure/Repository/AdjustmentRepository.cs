

namespace Restaurant_MS_Infrastructure.Repository
{
    public class AdjustmentRepository : Repository<Adjustment>
    {
        AppDbContext cxt;
        public AdjustmentRepository(AppDbContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }

        public IPagedList<Adjustment> GetAdjustments(int PageNo, int PageSize, Enums.AdjustmentType AdjustmentType)
        {
            return cxt.Adjustments
                .Where(a => a.IsActive == true
                        && a.AdjustmentType == (int) AdjustmentType)
                //.Where(a=> a.AdjustmentItems.Any(i=>i.IsActive && i.Quantity > 0))
                .Include(a => a.AdjustmentItems)
                .Where(a => a.AdjustmentItems.Any(i => i.IsActive))
                .Include(a => a.AdjustmentItems.Select(i => i.Batch))
                .Include(a => a.AdjustmentItems.Select(i => i.Item))
                .Include(a => a.User)
                .OrderByDescending(a => a.AdjustmentId).ToPagedList(PageNo, PageSize);
        }

        public IPagedList<Adjustment> GetAdjustments(DateTime FromDate, DateTime Dateto, int PageNo, int PageSize, Enums.AdjustmentType AdjustmentType)
        {

            return cxt.Adjustments
                    .Where(a => a.IsActive == true && a.AdjustmentType == (int)AdjustmentType)
                    .Where(a => (a.CreatedAt.Date) >= (FromDate.Date))

                    .Where(a => (a.CreatedAt.Date) <= (Dateto.Date))
                    .Include(a => a.AdjustmentItems)
                    .Where(a => a.AdjustmentItems.Any(i => i.IsActive))
                    .Include(a => a.AdjustmentItems.Select(i => i.Batch))
                    .Include(a => a.AdjustmentItems.Select(i => i.Item))
                    .Include(a => a.User)
                    .OrderByDescending(a => a.AdjustmentId).ToPagedList(PageNo, PageSize);

        }
        public IPagedList<Adjustment> GetAdjustments(int ItemId, int PageNo, int PageSize, Enums.AdjustmentType AdjustmentType)
        {

            return cxt.Adjustments
                    .Where(a => a.IsActive
                        && a.AdjustmentType == (int)AdjustmentType
                        && a.AdjustmentItems.Any(ai => ai.IsActive && ai.Item.ItemId == ItemId))
                    .Include(a => a.AdjustmentItems)
                    .Where(a => a.AdjustmentItems.Any(i => i.IsActive))
                    .Include(a => a.AdjustmentItems.Select(i => i.Batch))
                    .Include(a => a.AdjustmentItems.Select(i => i.Item))
                    .Include(a => a.User)
                    .OrderByDescending(a => a.AdjustmentId).ToPagedList(PageNo, PageSize);
        }
        //public AdjustmentsVM GetAdjustments_Rpt(long AdjustmentID)
        //{
        //    return cxt.Adjustments
        //            .Where(a => a.AdjustmentId == AdjustmentID)
        //            .Where(a => a.IsActive == true)
        //            .Select(ad => new AdjustmentsVM
        //            {
        //                AdjustmentId = ad.AdjustmentId,
        //                AdjustmentItems = ad.AdjustmentItems.Where(i => i.IsActive).Select(i => new AdjustmentItemVM
        //                {
        //                    ItemName = i.Item.ItemName,
        //                    BatchName = i.Batch.BatchName,
        //                    Quantity = i.Quantity,
        //                    Reason = i.Reason,
        //                    CreatedAt = i.CreatedAt
        //                }).ToList()
        //            }).FirstOrDefault();
        //}

        public Adjustment GetAdjustmentById(long AdjustmentId)
        {
            return cxt.Adjustments
                .Where(a => a.AdjustmentId == AdjustmentId)
                .Include(a => a.AdjustmentItems.Select(i => i.Item))
                .Include(a => a.AdjustmentItems.Select(i => i.Batch)).FirstOrDefault();
        }

        public void SetInactive(long AdjustmentId)
        {
            var sql = @"
                UPDATE Adjustments 
                SET IsActive = 0, IsSynced = 0 
                WHERE AdjustmentId = @AdjustmentID;

                UPDATE AdjustmentItems 
                SET IsActive = 0 
                WHERE AdjustmentId = @AdjustmentID;
            ";

            cxt.Database.ExecuteSqlRaw(sql, new SqlParameter("@AdjustmentID", AdjustmentId));
        }

        public List<AdjustmentItem> GetStockAdjustmentsByItemId(int ItemId)
        {
            return cxt.AdjustmentItems
                .Where(a => a.IsActive)
                .Include(ai => ai.Batch)
                .Where(ai => ai.Item.ItemId == ItemId).ToList();
        }
        public bool AnyActiveAdjExists()
        {
            return cxt.Adjustments.Any(a => a.IsActive);
        }
    }
}
