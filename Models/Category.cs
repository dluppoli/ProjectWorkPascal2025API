using System;
using System.Collections.Generic;

namespace ProjectWorkAPI.Models;

public partial class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Image { get; set; } = null!;

    public int OrderIndex { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
