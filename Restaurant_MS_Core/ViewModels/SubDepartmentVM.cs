

using Restaurant_MS_Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK.Shared.Core.Entities
{
    public class SubDepartmentVM
    {
        public long SubDepartmentId { get; set; }
        public string Name { get; set; }
        public User User { get; set; }
        public Department Department { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsNew { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsSynced { get; set; }
    }
}
