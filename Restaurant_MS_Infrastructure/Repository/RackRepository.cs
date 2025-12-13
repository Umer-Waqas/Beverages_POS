

namespace Restaurant_MS_Infrastructure.Repository
{
    public class RackRepository : Repository<Rack>
    {
        AppDbContext cxt;
        public RackRepository(AppDbContext cxt) : base(cxt)
        {
            this.cxt = cxt;
        }
        public List<Rack> GetAllActiveRacks()
        {
            return cxt.Racks.Where(r => r.IsActive).ToList();
        }
        public IPagedList<Rack> GetActiveRacks(int pageNo, int pageSize)
        {
            return cxt.Racks.Where(r => r.IsActive)
                .OrderByDescending(r => r.RackId)
                .ToPagedList(pageNo, pageSize);
        }

        public IPagedList<IGrouping<Rack, Item>> getRackView(int pageNo, int pageSize)
        {
            var r2 = (from i in cxt.Items
                      .Where(i=>i.IsActive && !i.IsDefault)
                     group i by i.Rack)
                    .OrderByDescending(r => r.Key.RackId)
                    .ToPagedList(pageNo, pageSize);
            return r2;
            //Console.WriteLine("nothing");

        }
        public bool RackExists_New(string name)
        {
            return cxt.Racks.Any(m => m.Name.ToLower() == name && m.IsActive);
        }

        public bool RackExists_Update(string name, int RackId)
        {
            return cxt.Racks.Any(m => m.Name.ToLower() == name && m.RackId != RackId && m.IsActive);
        }
    }
}
