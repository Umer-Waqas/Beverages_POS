

namespace Restaurant_MS_Infrastructure.Repository
{
    public class StockAuditRepository : Repository<StockAudit>
    {
        AppDbContext cxt;
        public StockAuditRepository(AppDbContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }

        public IPagedList<StockAuditsScreenVM> getActiveAudits(int pageNo, int PageSize)
        {
            return cxt.StockAudits
                 .Where(sa => sa.IsActive)
                 .Select(sa => new StockAuditsScreenVM
                 {
                     StockAuditId = sa.StockAuditId,
                     TotalDifference = sa.TotalDifference,
                     AuditDate = sa.StockAuditDate,
                     CreatedAt = sa.CreatedAt
                 }).OrderByDescending(sa => sa.StockAuditId)
                 .ToPagedList(pageNo, PageSize);
        }
        public IPagedList<StockAuditVM> getAudits(DateTime from, DateTime to, int pageNo, int PageSize)
        {
            cxt.StockAudits
                .Where(sa => (sa.StockAuditDate) >= (from)
                    && (sa.StockAuditDate) <= (to))
                .Select(sa => new StockAuditVM
                {
                    StockAuditId = sa.StockAuditId,
                    TotalDifference = sa.TotalDifference,
                    CreatedAt = sa.CreatedAt
                }).OrderByDescending(sa => sa.StockAuditId)
                .ToPagedList(pageNo, PageSize);
            return null;
        }
        public StockAudit GetAuditWithDetailById(int StockAuditId)
        {

            return cxt.StockAudits
                .Where(sa => sa.StockAuditId == StockAuditId)
                .Include(sa => sa.StockAuditDetails)
                .Include(sa => sa.StockAuditDetails.Select(sd => sd.Item))
                .FirstOrDefault();
        }

        public StockAuditVM GetAuditDetailVMById(int StockAuditId)
        {
            return cxt.StockAudits
                .Where(a => a.StockAuditId == StockAuditId)
                .Select(a => new StockAuditVM
                {
                    StockAuditId = a.StockAuditId,
                    StockAuditDate = a.StockAuditDate,
                    User = a.User,
                    StockAuditDetailVM = a.StockAuditDetails.Where(d => d.IsActive).Select(vm => new StockAuditDetailVM
                    {
                        StockAuditDetailId = vm.StockAuditDetailId,
                        ItemId = vm.Item.ItemId,
                        ItemName = vm.Item.ItemName,
                        ConversionUnit = vm.Item.ConversionUnit,
                        ItemUnit = vm.Item.Unit,
                        AuditUnit = vm.Unit,

                        SystemQuantity = vm.SystemQuantity,
                        CurrentAdjustedQuantity = vm.CurrentAdjustedQuantity,
                        Differnce =vm.Differnce,
                        AmountDifference = vm.AmountDifference,
                        PhysicalQuantity = vm.PhysicalQuantity,

                        // query variables to get avaolable stock
                        TotalStock = cxt.StockItems.Where(si => si.IsActive && si.Item.ItemId == vm.Item.ItemId).Sum(si => (int?)si.Quantity + si.BonusQuantity) ?? 0,
                        ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemId == vm.Item.ItemId select s).Sum(s => (int?)s.Quantity) ?? 0,
                        ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemId == vm.Item.ItemId && (b.Expiry) < (DateTime.Now) select s).Sum(s => (int?)s.Quantity) ?? 0,
                        AdjustedStock = (from ai in cxt.AdjustmentItems join ad in cxt.Adjustments on ai.Adjustment.AdjustmentId equals ad.AdjustmentId where ai.IsActive && ai.Item.ItemId == vm.Item.ItemId select ai).Sum(ai => (int?)ai.Quantity) ?? 0,
                        ExpiredConsumedStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == vm.Item.ItemId && (b_.Expiry) < (DateTime.Now) select new { es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                        HoldStock = (from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemId == vm.Item.ItemId select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                    }).ToList()
                }).FirstOrDefault();
        }

        public StockAuditDetail getAuditDetailById(int detailId)
        {
            return cxt.StockAuditDetails.Where(d => d.StockAuditDetailId == detailId).FirstOrDefault();
        }
    }
}