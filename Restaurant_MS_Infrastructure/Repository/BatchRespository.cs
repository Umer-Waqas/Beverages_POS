
namespace Restaurant_MS_Infrastructure.Repository
{
    public class BatchRepository : Repository<Batch>
    {
        AppDbContext cxt;
        public BatchRepository(AppDbContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }

        public Batch FindBatchByName(string BatchName)
        {
            return cxt.Batches.Where(b => b.BatchName.ToLower() == BatchName.ToLower()).FirstOrDefault();
        }

        public Batch FindItemBatch(string BatchName, int ItemId)
        {
            return (from s in cxt.StockItems
                    join b in cxt.Batches
                    on s.Batch.BatchId equals b.BatchId
                    where s.Item.ItemId == ItemId && b.BatchName.ToLower().Equals(BatchName)
                    select b).FirstOrDefault();
            //return cxt.Batches.Where(b => b.BatchName.ToLower() == BatchName.ToLower()).FirstOrDefault();
        }

        public Batch InsertBatch(Batch objBatch)
        {
            cxt.Batches.Add(objBatch);
            cxt.SaveChanges();
            return objBatch;
        }

        public List<string> GetAllUniqueBatches()
        {
            return cxt.Batches.Select(b => b.BatchName).Distinct().ToList();
        }

        public List<SelectListVM> GetItemBatches(int ItemId)
        {
            var res = (from si in cxt.StockItems
                       join s in cxt.Stocks on si.Stock.StockId equals s.StockId
                       join b in cxt.Batches on si.Batch.BatchId equals b.BatchId
                       where si.Item.ItemId == ItemId
                       select new SelectListVM
                       {
                           Value = b.BatchId,
                           Text = b.BatchName
                       }).ToList();
            return res;
        }
    }
}