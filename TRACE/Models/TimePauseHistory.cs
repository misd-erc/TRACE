using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class TimePauseHistory
{
    public int Id { get; set; }

    public string? DateUpdated { get; set; }

    public string? Status { get; set; }

    public int? ErcId { get; set; }

    public int? UserId { get; set; }

    public string? Remarks { get; set; }
}