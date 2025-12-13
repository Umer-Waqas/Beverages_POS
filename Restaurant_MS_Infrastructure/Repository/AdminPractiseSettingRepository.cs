using Restaurant_MS_Core.Entities;
using Restaurant_MS_Core.Entities;
using Restaurant_MS_Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Infrastructure.Repository
{
    public class AdminPractiseSettingRepository : Repository<Restaurant_MS_Core.Entities.AdminPractiseSetting>
    {
        AppDbContext cxt = null;
        public AdminPractiseSettingRepository(AppDbContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }

        public int GetPracticeId()
        {
            return cxt.AdminPractiseSettings.FirstOrDefault().PracticeId;
        }

        public bool IsUserDeletedDefaultData()
        {
            return cxt.AdminPractiseSettings.FirstOrDefault().UserDeletedDefaultData;
        }

        public AdminPractiseSetting? getAdminPracStting()
        {
            return cxt.AdminPractiseSettings.FirstOrDefault();
        }
    }
}