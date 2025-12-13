

namespace Restaurant_MS_Infrastructure.Repository
{
    public class ManufacturerRepository : Repository<Manufacturer>
    {
        AppDbContext cxt;
        public ManufacturerRepository(AppDbContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }

        public List<Manufacturer> GetAllActiveManufacturers()
        {
            return cxt.Manufacturers.Where(m => m.IsActive).ToList();
        }
        public IPagedList<Manufacturer> GetManufacturersByNameFilter(string FilterString, int PageNo, int PageSize)
        {          
            IPagedList<Manufacturer> r = cxt.Manufacturers
               .Where(i => i.IsActive)
               .Where(i => i.Name.ToLower().Contains(FilterString))
               .OrderByDescending(i => i.ManufacturerId).ToPagedList(PageNo, PageSize);
            return r;
        }

        public IPagedList<Manufacturer> GetManufacturers(int PageNo, int PageSize)
        {
            IPagedList<Manufacturer> r = cxt.Manufacturers
               .Where(i => i.IsActive)               
               .OrderByDescending(i => i.ManufacturerId).ToPagedList(PageNo, PageSize);
            return r;
        }

        public void SetManufacturerDataInactive(int ManufacturerId)
        {            
            string qry = @"Update Manufacturers set IsActive = 0, IsSynced = 0, DeletedAt =@DeletedAt  where manufacturerId= @ManufacturerId";
            cxt.Database.ExecuteSqlRaw(qry, new SqlParameter("@ManufacturerId", ManufacturerId), new SqlParameter("@DeletedAt", DateTime.Now));
        }

        public bool ManufacturerExists_New(string name)
        {
            return cxt.Manufacturers.Any(m => m.Name.ToLower() == name && m.IsActive);
        }

        public bool ManufacturerExists_Update(string name, int ManufacturerId)
        {
            return cxt.Manufacturers.Any(m => m.Name.ToLower() == name && m.ManufacturerId != ManufacturerId && m.IsActive);
        }

        public bool ManufacturerExist()
        {
            return cxt.Manufacturers.Any();
        }
        public Manufacturer GetManufByName(string ManufacName)
        {
            return cxt.Manufacturers.Where(sp => sp.Name.ToLower().Equals(ManufacName)).FirstOrDefault();
        }
    }
}