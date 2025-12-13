

namespace Restaurant_MS_Infrastructure.Repository
{
    public class AccTransactionsRepository:Repository<AccTransaction>
    {
        AppDbContext cxt;
        public AccTransactionsRepository(AppDbContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }

        public List<AccTransaction> GetAllActiveTransactions()
        {
            return cxt.AccTransactions.Where(t => t.IsActive).ToList();
        }
    }
}