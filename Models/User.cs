using System;
using System.Collections.Generic;

namespace ProjectWorkAPI.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Salt { get; set; } = null!;

    public DateTime? LastLogin { get; set; }

    public DateTime? LastLogout { get; set; }

    public string? SessionToken { get; set; }
}
