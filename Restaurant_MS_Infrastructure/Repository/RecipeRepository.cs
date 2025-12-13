

namespace Restaurant_MS_Infrastructure.Repository
{

    public class RecipeRepository : Repository<RecipeItem>
    {
        private readonly AppDbContext _context;

        public RecipeRepository(AppDbContext context)
            : base(context)
        {
            _context = context;
        }

        public Recipe GetRecipe(long ItemId)
        {
            return _context.Recipes
                           .Where(r => r.ItemId == ItemId)
                           .Include("Items")
                           .Include("recipeItems")
                           .FirstOrDefault();
        }
    }
}