
namespace Restaurant_MS_Infrastructure.Repository
{
    public class ExpenseCategoryRepository : Repository<ExpenseCategory>
    {
        AppDbContext cxt;
        public ExpenseCategoryRepository(AppDbContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }

        public List<SelectListVM> GetSelectList()
        {
            return cxt.ExpenseCategories.Where(c => c.IsActive && c.DisplayInDropDown)
               .Select(c => new SelectListVM
               {
                   Value = c.ExpenseCategoryId,
                   Text = c.Title
               }).ToList();
        }



        public bool alreadyExists_Insert(string categoryName)
        {
            return cxt.ExpenseCategories.Any(c => c.IsActive && c.Title.ToLower().Equals(categoryName.ToLower()));
        }


        public bool alreadyExists_Update(string categoryName, long categoryId)
        {
            return cxt.ExpenseCategories.Any(c => c.IsActive && c.ExpenseCategoryId != categoryId && c.Title.ToLower().Equals(categoryName.ToLower()));
        }
    }
}
