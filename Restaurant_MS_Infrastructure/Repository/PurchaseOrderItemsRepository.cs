

namespace Restaurant_MS_Infrastructure.Repository
{
    
    public class PurchaseOrderItemsRepository : Repository<PurchaseOrderItem>
    {
        AppDbContext cxt;

        public PurchaseOrderItemsRepository(AppDbContext cxt):base(cxt)
        {
            this.cxt = cxt;
        }

        public List<PurchaseOrderItem> GetItemsByPurchaseOrderNo(decimal PurchaseOrderNo)
        {
            return cxt.PurchaseOrderItems.Where(i => i.PurchaseOrderNo == PurchaseOrderNo).ToList();
        }     
        
        public void RemoveRange(List<PurchaseOrderItem> RemoveList)
        {
            cxt.PurchaseOrderItems.RemoveRange(RemoveList);
        }
    }
}