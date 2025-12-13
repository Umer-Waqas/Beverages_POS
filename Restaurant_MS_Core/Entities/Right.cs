using System;
using System.Collections.Generic;

namespace Restaurant_MS_Core.Entities;

public partial class Right
{
    public int RightId { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
