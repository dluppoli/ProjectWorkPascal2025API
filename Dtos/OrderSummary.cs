public class OrderSummary
{
    public int TableId { get; set; }
    public int Occupants { get; set; }

    public double TotalPrice { get; set; }
    public List<OrderDto> Orders { get; set; } = new List<OrderDto>();
}