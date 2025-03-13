using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class User
{
    public int Id { get; set; }

    public string? Email { get; set; }

    public string? Department { get; set; }

    public string? Username { get; set; }
    public string? Fullname { get; set; }
    public string? Designation { get; set; }
    public bool IsSystemNotif { get; set; }
    public bool? IsArchive { get; set; }



}
