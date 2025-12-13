using System;
using System.Collections.Generic;

namespace Restaurant_MS_Core.Entities;

public partial class RecipeItem
{
    public int RecipeItemId { get; set; }

    public int RecipeId { get; set; }

    public long IngredientId { get; set; }

    public decimal Quantity { get; set; }

    public string? Unit { get; set; }

    public virtual Item Ingredient { get; set; } = null!;

    public virtual Recipe Recipe { get; set; } = null!;
}
