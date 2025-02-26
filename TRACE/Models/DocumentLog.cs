using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class DocumentLog
{
    public long DocumentLogId { get; set; }

    public DateTime LogDatetime { get; set; }

    public string EventLog { get; set; } = null!;

    public string? EventLogDescription { get; set; }

    public string UserId { get; set; } = null!;

    public long DocumentId { get; set; }

    public virtual Document Document { get; set; } = null!;
}
