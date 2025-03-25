using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TRACE.Models;

public partial class User
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string? UserCategory { get; set; }

    [Display(Name = "Allow Email Notifications?")]
    public bool? IsEmailNotif { get; set; }

    public string Fullname { get; set; } = null!;

    public string? Designation { get; set; }

    public string? Department { get; set; }

    [Display(Name = "Allow System Notifications?")]
    public bool? IsSystemNotif { get; set; }

    [Display(Name = "Archive this user?")]
    public bool? IsArchive { get; set; }

    public string Username { get; set; } = null!;
}
