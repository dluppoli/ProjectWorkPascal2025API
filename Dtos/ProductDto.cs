namespace ProjectWorkAPI.Dtos;

public partial class ProductDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public double Price { get; set; }

    public string Image { get; set; } = null!;

    public string Category { get; set; } = null!;
}
