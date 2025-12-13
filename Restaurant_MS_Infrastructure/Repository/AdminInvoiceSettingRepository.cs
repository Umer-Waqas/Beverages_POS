
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Infrastructure.Repository
{

    public class AdminInvoiceSettingRepository : Repository<AdminInvoiceSetting>
    {
        AppDbContext cxt;
        public AdminInvoiceSettingRepository(AppDbContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }
    }
}