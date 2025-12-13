

namespace Restaurant_MS_Infrastructure.Repository
{
    public class CategoryRepository : Repository<Category>
    {
        AppDbContext cxt;
        public CategoryRepository(AppDbContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }

        public List<SelectListVM> GetCategoryActiveItems(int categiryId)
        {
            //return cxt.Categories.Where(c => c.CategoryId == categiryId)
            //    .Select(c => new CategoryVM()
            //    {
            //        CategoryId = c.CategoryId,
            //        Items = (c.Items.Where(i => i.IsActive).Select(i => new selecte()
            //        {
            //            ItemId = i.ItemId,
            //            ItemName = i.ItemName
            //        })).ToList()
            //    }).FirstOrDefault();
            return cxt.Items
                .Where(i => i.CategoryId == categiryId && i.IsActive)
                .Select(i => new SelectListVM
                {
                    Value = i.ItemId,
                    Text = i.ItemName
                }).ToList();
        }

        public List<Category> GetAllActiveCategories()
        {
            return cxt.Categories.Where(c => c.IsActive && c.ParentId == null).ToList();
        }

        public List<Category> GetAllActiveSubCategories(long ParentId)
        {
            return cxt.Categories.Where(c => c.IsActive && c.ParentId == ParentId).ToList();
        }

        public Category GetCategoryByItemId(long ItemId)
        {
            return cxt.Items.Where(i => i.ItemId == ItemId).Select(i => i.Category).FirstOrDefault();
        }
        public Category GetCategoryByName(string catName)
        {
            return cxt.Categories.Where(c => c.Name.ToLower() == catName).FirstOrDefault();
        }

        public long GetCategoryIdByName(string catName)
        {
            return cxt.Categories.Where(c => c.Name.ToLower() == catName).Select(c => c.CategoryId).FirstOrDefault();
        }

        public List<SelectListVM> GetCategoriesSelectList()
        {
            return cxt.Categories.Where(c => c.IsActive && !c.IsSystemCategory.Value)
                .Select(c => new SelectListVM
                {
                    Value = c.CategoryId,
                    Text = c.Name
                }).ToList();
        }

        public List<SelectListVM> GetSubCategoriesSelectList(long ParentCategoryId)
        {
            return cxt.Categories.Where(c => c.IsActive && c.ParentId == ParentCategoryId)
                .Select(c => new SelectListVM
                {
                    Value = c.CategoryId,
                    Text = c.Name
                }).ToList();
        }
    }
}
