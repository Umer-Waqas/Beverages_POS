

namespace Restaurant_MS_Infrastructure.Repository
{
    public class LowStockMailRepository : Repository<LowStockMail>
    {
        AppDbContext cxt;
        public LowStockMailRepository(AppDbContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }
        public DateTime GetLastMailSentDate()
        {
            return cxt.LowStockMails.OrderByDescending(r=>r.CreatedAt).FirstOrDefault().LastSent;
        }

        public void AddLastMailSentDate(LowStockMail objLowStockMail)
        {
            cxt.LowStockMails.Add(objLowStockMail);
        }
    }
}