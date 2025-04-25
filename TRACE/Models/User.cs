using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class User
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string? UserCategory { get; set; }

    public bool? IsEmailNotif { get; set; }

    public string Fullname { get; set; } = null!;

    public string? Designation { get; set; }

    public string? Department { get; set; }

    public bool? IsSystemNotif { get; set; }

    public bool? IsArchive { get; set; }

    public string Username { get; set; } = null!;
}
