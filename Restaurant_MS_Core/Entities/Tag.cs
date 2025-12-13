using System;
using System.Collections.Generic;

namespace Restaurant_MS_Core.Entities;

public partial class Tag
{
    public long TagId { get; set; }

    public string? TagName { get; set; }

    public virtual ICollection<Patient> Patients { get; set; } = new List<Patient>();
}
