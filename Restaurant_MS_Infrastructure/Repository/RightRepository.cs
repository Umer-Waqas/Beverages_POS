

namespace Restaurant_MS_Infrastructure.Repository
{
    public class RightRepository : Repository<Right>
    {
        AppDbContext cxt;
        public RightRepository(AppDbContext cxt) : base(cxt)
        {
            this.cxt = cxt;
        }
    }
}
