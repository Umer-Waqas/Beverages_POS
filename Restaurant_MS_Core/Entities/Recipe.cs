using System;
using System.Collections.Generic;

namespace Restaurant_MS_Core.Entities;

public partial class Recipe
{
    public int RecipeId { get; set; }

    public long ItemId { get; set; }

    public string Name { get; set; } = null!;

    public virtual Item Item { get; set; } = null!;

    public virtual ICollection<RecipeItem> RecipeItems { get; set; } = new List<RecipeItem>();
}
