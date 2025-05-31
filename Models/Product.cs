using System;
using System.Collections.Generic;

namespace ProjectWorkAPI.Models;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public double Price { get; set; }

    public string Image { get; set; } = null!;

    public int IdCategory { get; set; }

    public int IdPostazionePreparazione { get; set; }

    public virtual Category IdCategoryNavigation { get; set; } = null!;

    public virtual ProductPrepStation IdPostazionePreparazioneNavigation { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
