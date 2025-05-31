using System;
using System.Collections.Generic;

namespace ProjectWorkAPI.Models;

public partial class RevokedToken
{
    public string Token { get; set; } = null!;

    public DateTime? Expire { get; set; }
}
