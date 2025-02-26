using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class ErrorLog
{
    public long ErrorLogId { get; set; }

    public string Message { get; set; } = null!;

    public string? Source { get; set; }

    public string? StackTrace { get; set; }

    public DateTime ErrorDatetime { get; set; }

    public string? InvokedBy { get; set; }
}
