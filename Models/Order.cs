using System;
using System.Collections.Generic;

namespace ProjectWorkAPI.Models;

public partial class Order
{
    public int Id { get; set; }

    public int TableId { get; set; }

    public string TableKey { get; set; } = null!;

    public int ProductId { get; set; }

    public int Qty { get; set; }

    public double Price { get; set; }

    public DateTime OrderDate { get; set; }

    public DateTime? CompletionDate { get; set; }

    public virtual Product Product { get; set; } = null!;
}
