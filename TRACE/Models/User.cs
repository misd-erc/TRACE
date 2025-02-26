using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class User
{
    public int Id { get; set; }

    public string? Email { get; set; }

    public string? Department { get; set; }

    public string? Username { get; set; }
}
