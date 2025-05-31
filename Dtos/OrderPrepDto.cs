public class OrderPrepDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }

    public int TableId { get; set; }

    public string Name { get; set; } = "";

    public int Qty { get; set; }

    public double Price { get; set; }

    public DateTime OrderDate { get; set; }

    public DateTime? CompletionDate { get; set; } = null;
}