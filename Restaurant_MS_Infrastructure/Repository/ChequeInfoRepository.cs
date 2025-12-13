


namespace Restaurant_MS_Infrastructure.Repository
{

    public class ChequeInfoRepository : Repository<ChequeInfo>, IChequeInfoRepository
    {
        AppDbContext cxt = null;
        public ChequeInfoRepository(AppDbContext cxt) : base(cxt)
        {
            this.cxt = cxt;
        }

        public long InsertChequeInfo(ChequeInfo objChequInfo)
        {
            cxt.ChequeInfoes.Add(objChequInfo);
            cxt.SaveChanges();
            return objChequInfo.ChequeInfoId;
        }
    }
}