using System;
using System.Collections.Generic;

namespace ProjectWorkAPI.Models;

public partial class Table
{
    public int Id { get; set; }

    public bool Occupied { get; set; }

    public DateTime? OccupancyDate { get; set; }

    public int? Occupants { get; set; }

    public string? TableKey { get; set; }
}
