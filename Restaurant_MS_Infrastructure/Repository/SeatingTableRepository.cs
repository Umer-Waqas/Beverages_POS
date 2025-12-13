
namespace Restaurant_MS_Infrastructure.Repository
{
    public class SeatingTableRepository : Repository<Invoice>
    {
        AppDbContext cxt = null;
        public SeatingTableRepository(AppDbContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }
    }
}
