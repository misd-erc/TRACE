using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class EventLog
{
    public long EventLogId { get; set; }

    public string? Event { get; set; }

    public string? UserId { get; set; }

    public DateTime? EventDatetime { get; set; }

    public string? Source { get; set; }

    public string? Category { get; set; }
}
