

namespace Restaurant_MS_Infrastructure.Repository
{
    public class HwDataRepository : Repository<HwData>
    {
        AppDbContext cxt;
        public HwDataRepository(AppDbContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }

        public string GetSystemExpiryDate()
        {
            return cxt.HwDatas.Select(r => r.SystemExpiry).FirstOrDefault();
        }
    }
}
