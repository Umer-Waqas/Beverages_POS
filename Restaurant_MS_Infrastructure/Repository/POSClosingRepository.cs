

namespace Restaurant_MS_Infrastructure.Repository
{
    public class POSClosingRepository:Repository<POSClosing>
    {
        AppDbContext cxt;
        public POSClosingRepository(AppDbContext cxt) :base(cxt)
        {
            this.cxt = cxt;
        }

        public DateTime GetLastPOSClosingDate()
        {
            return cxt.POSClosings.Where(c => c.IsActive).OrderByDescending(r => r.POSClosingId).Select(r=>r.ClosingDate).FirstOrDefault();
        }
        public double GetLastCashClosingVal()
        {
            return (double?)cxt.POSClosings.Where(c => c.IsActive).OrderByDescending(c => c.POSClosingId).Select(c => (double?)c.CashTillIn).FirstOrDefault() ?? 0;
        }

        public int GetNewClosingCode()
        {
            return cxt.POSClosings.Where(c => c.IsActive).OrderByDescending(c => c.POSClosingId).Select(c => c.POSCode).FirstOrDefault() + 1;
        }
    }
}
