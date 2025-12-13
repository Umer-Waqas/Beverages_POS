

namespace Restaurant_MS_Infrastructure.Repository
{
    public class TemplateRepository : Repository<Template>
    {
        AppDbContext cxt;
        public TemplateRepository(AppDbContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }

        public TemplateVM getTemplateById(int templateId)
        {
            return cxt.Templates
                .Where(t=>t.TemplateId == templateId)
                .Select(t => new TemplateVM
                {
                    Name = t.Name,
                    //TemplateItems = t.TemplateItems.Where(ti => ti.IsActive).Select(ti => new TemplateItemVm
                    //{
                    //    TemplateItemId = ti.TemplateItemId,
                    //    Item = ti.Item,
                    //    Quantity = ti.Quantity
                    //}).ToList()
                }).FirstOrDefault();
        }
        public Template getTemplateWithDetailsById(int templateId)
        {
            return cxt.Templates
                .Where(t => t.TemplateId == templateId)
                //.Include(t => t.TemplateItems)                
                //.Where(t => t.TemplateItems.Any(ti => ti.IsActive))
                .Include(t=>t.User)
                .FirstOrDefault();
        }
        public List<Template> GetAllActiveTemplates()
        {
            return cxt.Templates.Where(t => t.IsActive).ToList();
        }
        private IPagedList<Template> GetActiveTemplates(int pageNo, int pageSize)
        {
            return cxt.Templates.Where(t => t.IsActive).ToPagedList(pageNo, pageSize);
        }
        public IPagedList<TemplateVM> getTemplates_filter(string search, int pageNo, int pageSize)
        {
            return cxt.Templates
                 .Where(t => t.IsActive
                     && t.Name.ToLower().Contains(search))
                 .Select(tvm => new TemplateVM
                 {
                     TemplateId = tvm.TemplateId,
                     Name = tvm.Name
                     //TemplateItems = tvm.TemplateItems.Where(ti => ti.IsActive).Select(ivm => new TemplateItemVm
                     //{
                     //    Item = ivm.Item,
                     //    Quantity = ivm.Quantity
                     //}).ToList()
                 })
                 .OrderByDescending(tvm => tvm.TemplateId)
                 .ToPagedList(pageNo, pageSize);
        }
        public IPagedList<TemplateVM> getTemplates(int pageNo, int pageSize)
        {
            return cxt.Templates
                 .Where(t => t.IsActive)              
                 .Select(tvm => new TemplateVM
                 {
                     TemplateId = tvm.TemplateId,
                     Name = tvm.Name,
                     //TemplateItems = tvm.TemplateItems.Where(ti => ti.IsActive).Select(ivm => new TemplateItemVm
                     //{
                     //    Item = ivm.Item,
                     //    Quantity = ivm.Quantity
                     //}).ToList()
                 })
                 .OrderByDescending(tvm => tvm.TemplateId)
                 .ToPagedList(pageNo, pageSize);
        }
        public void SetTemplateInactive(int TemplateId)
        {
            //            string qry = @"Update Items set IsActive = 0, IsUpdate = 1, IsSynced = 0, UpdatedAt = @UpdatedAt where ItemId = @ItemId 
            //                            Update StockItems Set IsActive = 0, IsUpdate = 1, IsSynced = 0, UpdatedAt = @UpdatedAt where ItemId = @ItemId 
            //                            Update AdjustmentItems Set IsActive = 0, IsUpdate = 1, IsSynced = 0, UpdatedAt = @UpdatedAt where ItemId = @ItemId";

            string qry = @"Update Templates set IsActive = 0, IsSynced = 0, DeletedAt =@DeletedAt  where TemplateId = @TemplateId";
            cxt.Database.ExecuteSqlRaw(qry, new SqlParameter("@TemplateId", TemplateId), new SqlParameter("@DeletedAt", DateTime.Now));
        }
        public bool TemplateExists_New(string name)
        {
            return cxt.Templates.Any(m => m.Name.ToLower() == name && m.IsActive);
        }

        public bool TemplateExists_Update(string name, int TemplateId)
        {
            return cxt.Templates.Any(m => m.Name.ToLower() == name && m.TemplateId!= TemplateId && m.IsActive);
        }
    }
}