using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Infrastructure.Repository
{
    public class AdminProcedureInvoiceSettingRepository : Repository<AdminProcedureInvoiceSetting>
    {
        AppDbContext cxt;
        public AdminProcedureInvoiceSettingRepository(AppDbContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }
    }
}
