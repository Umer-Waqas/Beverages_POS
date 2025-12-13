using System;
using System.Collections.Generic;

namespace Restaurant_MS_Core.Entities;

public partial class Department
{
    public long DepartmentId { get; set; }

    public string? Name { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? SyncedAt { get; set; }

    public bool IsActive { get; set; }

    public bool IsNew { get; set; }

    public bool IsUpdate { get; set; }

    public bool IsSynced { get; set; }

    public int UserId { get; set; }

    public virtual ICollection<SubDepartment> SubDepartments { get; set; } = new List<SubDepartment>();

    public virtual User User { get; set; } = null!;
}
