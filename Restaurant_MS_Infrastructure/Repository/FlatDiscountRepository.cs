
using Microsoft.EntityFrameworkCore;

namespace Restaurant_MS_Infrastructure.Repository
{
    public class FlatDiscountRepository : Repository<FlatDiscount>
    {
        private AppDbContext cxt;
        public FlatDiscountRepository(AppDbContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }

        public FlatDiscount GetDiscountDet(int FlatDiscountId)
        {
            return cxt.FlatDiscounts
                 .Where(d => d.FlatDiscountId == FlatDiscountId)
                 //.Include(d => d.DiscountItems)
                 //.Include(d => d.DiscountItems.Select(di => di.Item))
                 .FirstOrDefault();
        }
        public FlatDiscountVM GetDiscountDet_VM(int FlatDiscountId)
        {
            return cxt.FlatDiscounts
                 .Where(d => d.FlatDiscountId == FlatDiscountId)
                 .Select(d => new FlatDiscountVM
                 {
                     FlatDiscountId = d.FlatDiscountId,
                     Name = d.Name,

                     Code = d.Code,
                     Discount = d.Discount,
                     DiscountType = d.DiscountType,

                     IsAllDays = d.IsAllDays,
                     IsAllTimes = d.IsAllTimes,
                     SelectedDays = d.SelectedDays,
                     IsAllItems = d.IsAllItems,

                     StartDate = d.StartDate,
                     EndDate = d.EndDate,
                     StartTime = d.StartTime,
                     EndTime = d.EndTime,

                     CreatedAt = d.CreatedAt,
                     UpdatedAt = d.UpdatedAt,
                     User = d.User,
                     //DiscountItems = d.DiscountItems.Where(di => di.IsActive).Select(di => new DiscountItemVM
                     //{
                     //    DiscountItemId = di.DiscountItemId,
                     //    Item = di.Item
                     //}).ToList()
                 }).FirstOrDefault();
        }

        public IPagedList<FlatDiscount> GetAllACtiveDiscounts(int pageNo, int pageSize)
        {
            return cxt.FlatDiscounts
                    .Where(d => d.IsActive)
                    .OrderByDescending(d => d.FlatDiscountId)
                    .ToPagedList(pageNo, pageSize);
        }

        public void SetInActive(int DiscountId)
        {
            var discountIdParam = new SqlParameter("@DiscountId", DiscountId);
            var deletedAtParam = new SqlParameter("@DeletedAt", DateTime.Now);

            cxt.Database
                .ExecuteSqlRaw($"Update FlatDiscounts Set IsActive = 0, issynced = 0, DeletedAt = @DeletedAt WHERE flatdiscountId = @DiscountId",
                    discountIdParam, deletedAtParam);
        }

        public bool DiscNameExists_new(string name)
        {
            return cxt.FlatDiscounts.Any(m => m.Name.ToLower() == name && m.IsActive);
        }
        public bool DiscCodeExists_new(string name)
        {
            return cxt.FlatDiscounts.Any(m => m.Code.ToLower() == name && m.IsActive);
        }

        public bool DiscNameExists_Update(string name, int DiscountId)
        {
            return cxt.FlatDiscounts.Any(m => m.Name.ToLower() == name && m.FlatDiscountId != DiscountId && m.IsActive);
        }
        public bool DiscCodeExists_Update(string code, int DiscountId)
        {
            return cxt.FlatDiscounts.Any(m => m.Code.ToLower() == code && m.FlatDiscountId != DiscountId && m.IsActive);
        }
        public List<FlatDiscount> getTodayDiscounts()
        {
            return cxt.FlatDiscounts
                .Where(d => d.IsActive
                    && DateTime.Now.Date >= (d.StartTime.Date)
                    && DateTime.Now.Date <= (d.EndDate.Date)
                    )
                    //.Include(d => d.DiscountItems)
                    //.Include(d => d.DiscountItems.Select(di => di.Item))
                    .ToList();
            //cxt.FlatDiscount
            //    .Where(d=> d.IsActive
            //        && (DateTime.Now) >= (d.StartTime)
            //        && (DateTime.Now) <= (d.EndDate)
            //        && 
            //        (
            //            d.IsAllTimes 
            //            || (DbFunctions.CreateTime(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)>= DbFunctions.CreateTime(d.StartTime.Hour, d.StartTime.Minute, d.StartTime.Second)
            //                &&
            //                DbFunctions.CreateTime(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)>= DbFunctions.CreateTime(d.EndTime.Hour, d.EndTime.Minute, d.EndTime.Second)
            //                )
            //        )
            //        &&
            //        (
            //            d.IsAllDays
            //            ||
            //            (

            //            )
            //        )



            //            )
        }

        public IPagedList<FlatDiscountVM> GetAllItemDiscounts(int pageNo, int pageSize)
        {
            return cxt.FlatDiscounts
                .Where(d => d.IsActive
                    )
                    .Select(d => new FlatDiscountVM
                    {
                        FlatDiscountId = d.FlatDiscountId,
                        Name = d.Name,
                        Code = d.Code,
                        Discount = d.Discount,
                        DiscountType = d.DiscountType,
                        IsAllItems = d.IsAllItems
                    })
                    .OrderByDescending(r => r.FlatDiscountId).ToPagedList(pageNo, pageSize);
        }
        public IPagedList<FlatDiscountVM> GetAllItemDiscounts(DateTime DateFrom, DateTime DateTo, int pageNo, int pageSize)
        {
            return cxt.FlatDiscounts
                .Where(d => d.IsActive
                    && (d.StartDate) >= (DateFrom)
                    && (d.EndDate) <= (DateTo)
                    )
                    .Select(d=> new FlatDiscountVM {
                        FlatDiscountId = d.FlatDiscountId,
                        Name = d.Name,
                        Code = d.Code,
                        Discount = d.Discount,
                        DiscountType = d.DiscountType                        
                    })
                    .OrderByDescending(r => r.FlatDiscountId).ToPagedList(pageNo, pageSize);
        }

        public List<FlatDiscountVM> GetAllItemDiscounts_rpt()
        {
            return cxt.FlatDiscounts
                .Where(d => d.IsActive
                    && d.IsAllItems
                    )
                    .Select(d => new FlatDiscountVM
                    {
                        FlatDiscountId = d.FlatDiscountId,
                        Name = d.Name,
                        Code = d.Code,
                        Discount = d.Discount,
                        DiscTypeString = d.DiscountType ==0 ? "%" : "Value"
                    })
                    .OrderByDescending(r => r.FlatDiscountId).ToList();
        }
        public List<FlatDiscountVM> GetAllItemDiscounts_rpt(DateTime DateFrom, DateTime DateTo)
        {
            return cxt.FlatDiscounts
                .Where(d => d.IsActive
                    && d.IsAllItems
                    && (d.StartDate) >= (DateFrom)
                    && (d.EndDate) <= (DateTo)
                    )
                    .Select(d => new FlatDiscountVM
                    {         
                        FlatDiscountId = d.FlatDiscountId,
                        Name = d.Name,
                        Code = d.Code,
                        Discount = d.Discount,
                        DiscTypeString = d.DiscountType == 0 ? "%" : "Value"
                    })
                    .OrderByDescending(r => r.FlatDiscountId).ToList();
        }
    }
}