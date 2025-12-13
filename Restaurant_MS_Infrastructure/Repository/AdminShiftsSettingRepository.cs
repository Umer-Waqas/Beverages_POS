

namespace Restaurant_MS_Infrastructure.Repository
{
    public class AdminShiftsSettingRepository : Repository<AdminShiftSetting>
    {
        AppDbContext cxt;
        public AdminShiftsSettingRepository(AppDbContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }

        public List<AdminShiftSetting> GetActiveShifts()
        {
            return cxt.AdminShiftSettings.Where(s => s.IsActive).ToList();
        }
    }
}