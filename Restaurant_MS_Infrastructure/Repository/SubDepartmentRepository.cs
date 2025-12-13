

namespace Restaurant_MS_Infrastructure.Repository
{
    public class SubDepartmentRepository : Repository<SubDepartment>
    {
        AppDbContext cxt;
        public SubDepartmentRepository(AppDbContext cxt) : base(cxt)
        {
            this.cxt = cxt;
        }
    }
}
