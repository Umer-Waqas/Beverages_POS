using Restaurant_MS_Core.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Infrastructure.Repository
{

    public class AdminShiftMasterSettingRepository : Repository<AdminShiftMasterSetting>
    {
        AppDbContext cxt;
        public AdminShiftMasterSettingRepository(AppDbContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }
    }
}