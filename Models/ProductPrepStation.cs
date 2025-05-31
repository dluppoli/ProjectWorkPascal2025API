using System;
using System.Collections.Generic;

namespace ProjectWorkAPI.Models;

public partial class ProductPrepStation
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
