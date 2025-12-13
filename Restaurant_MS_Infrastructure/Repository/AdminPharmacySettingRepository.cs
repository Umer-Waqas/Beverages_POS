

namespace Restaurant_MS_Infrastructure.Repository
{
    public class AdminPharmacySettingRepository : Repository<AdminPharmacySetting>
    {
        AppDbContext cxt;
        public AdminPharmacySettingRepository(AppDbContext cxt) : base(cxt)
        {
            this.cxt = cxt;
        }
    }
}