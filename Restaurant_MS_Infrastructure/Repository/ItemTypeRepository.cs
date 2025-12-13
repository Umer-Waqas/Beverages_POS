
namespace Restaurant_MS_Infrastructure.Repository
{
    public class ItemTypeRepository : Repository<Category>
    {
        AppDbContext cxt;
        public ItemTypeRepository(AppDbContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }

        public List<SelectListVM> GetItemTypeActiveItems(int ItemTypeId)
        {
            return cxt.Items
                .Where(i => i.ItemTypeId == ItemTypeId && i.IsActive)
                .Select(i => new SelectListVM
                {
                    Value = i.ItemId,
                    Text = i.ItemName
                }).ToList();
        }

        public List<ItemType> GetAllActiveItemTypes()
        {
            return cxt.ItemTypes.Where(c => c.IsActive).ToList();
        }

        public ItemType GetItemTypeByItemId(long ItemId)
        {
            return cxt.Items.Where(i => i.ItemId == ItemId).Select(i => i.ItemType).FirstOrDefault();
        }
        public ItemType GetItemTypeByName(string TypeName)
        {
            return cxt.ItemTypes.Where(c => c.Name.ToLower() == TypeName).FirstOrDefault();
        }

        public long GetItemTypeIdByName(string TypeName)
        {
            return cxt.ItemTypes.Where(c => c.Name.ToLower() == TypeName).Select(c => c.ItemTypeId).FirstOrDefault();
        }

        public List<SelectListVM> GetBakedItemsSelectList()
        {
            return cxt.Items.Where(c => c.ItemType.Name == "Baked Item")
                 .Select(i => new SelectListVM
                 {
                     Value = i.ItemId,
                     Text = i.ItemName
                 }).ToList();
        }

        public List<SelectListVM> DealsSelectList()
        {
            return cxt.Items.Where(c => c.ItemType.Name == "Deal Item")
                 .Select(i => new SelectListVM
                 {
                     Value = i.ItemId,
                     Text = i.ItemName
                 }).ToList();
        }

        public List<SelectListVM> GetItemTypesSelectList()
        {
            return cxt.ItemTypes.Where(c => c.IsActive)
                .Select(c => new SelectListVM
                {
                    Value = c.ItemTypeId,
                    Text = c.Name
                }).ToList();
        }
    }
}
