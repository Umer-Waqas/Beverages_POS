
namespace Restaurant_MS_Infrastructure.Repository
{
    public class POSCashReceiveRepository : Repository<POSReceivedCash>
    {
        AppDbContext cxt;
        public POSCashReceiveRepository(AppDbContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }

        public List<POSReceivedCash> GetCR_CurrSess(int userId)
        {
            //return cxt.POSReceivedCash
            //    .Where(c => c.IsActive
            //        && c.UserId == userId
            //        && c.CreatedAt >= c.User.Shift.StartTime
            //        && c.CreatedAt <= c.User.Shift.EndTime)                    
            //        .ToList();

            DateTime last = (from h in cxt.LoginHistories
                             where h.UserId == userId
                             orderby h.LoginHistoryId descending
                             select h.CreatedAt)
                    .Skip(1).Take(1).FirstOrDefault();
            return cxt.POSReceivedCashes.Where(c => c.IsActive && c.UserId == userId && c.CreatedAt >= last).OrderByDescending(r => r.POSReceivedCashId).ToList();
        }
        public double GetCRSum_CurrSess(int userId)
        {
            //return cxt.POSReceivedCash
            //    .Where(c => c.IsActive
            //        && c.UserId == userId
            //        && c.CreatedAt >= c.User.Shift.StartTime
            //        && c.CreatedAt <= c.User.Shift.EndTime)                    
            //        .ToList();

            //DateTime last = (from h in cxt.LoginHistory
            //                 where h.UserId == userId
            //                 orderby h.LoginHistoryId descending
            //                 select h.CreatedAt)
            //        .Skip(1).Take(1).FirstOrDefault();
            //return (double?)cxt.POSReceivedCash.Where(c => c.IsActive && c.UserId == userId && c.CreatedAt >= last).Sum(r => (double?)r.Cash) ?? 0;
            return (double?)cxt.POSReceivedCashes
                .Where(c => c.IsActive
                    && c.UserId == userId
                    && c.CreatedAt >= (from p in cxt.POSClosings where p.IsActive orderby p.POSClosingId descending select p.ClosingDate)
                       .FirstOrDefault()
                    ).Sum(r => (double?)r.Cash) ?? 0;
        }
    }
}