

namespace Restaurant_MS_Infrastructure.Repository
{
    public class LoginHistoryRepository:Repository<LoginHistory>
    {
        AppDbContext cxt;
        public LoginHistoryRepository(AppDbContext cxt) : base(cxt)
        {
            this.cxt = cxt;
        }
    }
}