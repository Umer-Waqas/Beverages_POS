namespace Restaurant_MS_Infrastructure.Repository
{
    public class AdminPrintFormatSettingRepository : Repository<AdminPrintFormatSetting>
    {
        AppDbContext cxt = null;
        public AdminPrintFormatSettingRepository(AppDbContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }
    }
}