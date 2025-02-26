using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class CaseEvent
{
    public long CaseEventId { get; set; }

    public long ErccaseId { get; set; }

    public DateTime EventDatetime { get; set; }

    public string? UserId { get; set; }

    public string EventDescription { get; set; } = null!;

    public bool? IsUserAction { get; set; }

    public long? CaseEventTypeId { get; set; }

    public virtual CaseEventType? CaseEventType { get; set; }

    public virtual Erccase Erccase { get; set; } = null!;
}
