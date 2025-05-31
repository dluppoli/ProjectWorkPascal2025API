using System;
using System.Collections.Generic;

namespace ProjectWorkAPI.Dtos;

public partial class CategoryDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Image { get; set; } = null!;

    public int OrderIndex { get; set; }
}
