

namespace Restaurant_MS_Infrastructure.Repository
{
    public class MissedsalesRepository : Repository<MissedSale>
    {
        AppDbContext cxt = null;
        public MissedsalesRepository(AppDbContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }

        public IPagedList<MissedSale> GetAllActiveMissedSale(int pageNo, int pagesize)
        {
            return cxt.MissedSales.Where(s => s.IsActive).Include(s => s.Item).Include(s => s.User).OrderByDescending(s => s.MissedSaleId).ToPagedList(pageNo, pagesize);
        }
        public List<MissedSale> GetAllActiveMissedSale()
        {
            return cxt.MissedSales.Where(s => s.IsActive)
                .Include(s => s.Item)
                .OrderByDescending(s => s.MissedSaleId).ToList();
        }

        public List<long> GetMissedSalesItemsIds()
        {
            return cxt.MissedSales.Where(s => s.IsActive)
                .Select(s => s.Item.ItemId)
                .ToList();
        }

        public List<MissedSale> GetAllActiveMissedSalesByItem(int ItemId)
        {
            return cxt.MissedSales.Where(s => s.IsActive && s.Item.ItemId == ItemId).Include(s => s.Item).Include(s => s.User).OrderByDescending(s => s.MissedSaleId).ToList();
        }

        public IPagedList<MissedSale> GetMissedSalesBWDate(DateTime DateFrom, DateTime DateTo, int pageNo, int pagesize)
        {
            return cxt.MissedSales
                .Where(s => s.IsActive)
                .Where(s => (s.CreatedAt) >= (DateFrom) && (s.CreatedAt) <= (DateTo))
                .Include(s => s.Item)
                .Include(s => s.User)
                .OrderByDescending(s => s.MissedSaleId).ToPagedList(pageNo, pagesize);
        }
        public IPagedList<MissedSale> GetMissedSalesByItem(int ItemId)
        {
            return cxt.MissedSales
                .Where(s => s.IsActive && s.Item.ItemId == ItemId)
                .Include(s => s.Item)
                    .Include(s => s.User)
                .OrderByDescending(s => s.MissedSaleId).ToPagedList(1, 1);
        }

        public bool ItemMissedSaleExists(int ItemId)
        {
            return cxt.MissedSales.Any(m => m.IsActive && m.Item.ItemId == ItemId);
        }

        public List<MissedSale> GetMissedSalesByItemIds(List<long> itemIds)
        {
            return cxt.MissedSales.Where(s => s.IsActive && itemIds.Contains(s.Item.ItemId)).ToList();
        }
    }
}