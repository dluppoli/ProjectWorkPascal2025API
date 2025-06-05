public class OrderDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }

    public string Name { get; set; } = "";

    public int Qty { get; set; }

    public string Category { get; set; } = null!;

    public double Price { get; set; }

    public DateTime OrderDate { get; set; }

    public DateTime? CompletionDate { get; set; } = null;
}