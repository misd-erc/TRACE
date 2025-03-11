using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class CaseEventType
{
    public long CaseEventTypeId { get; set; }

    public string EventType { get; set; } = null!;

    public virtual ICollection<CaseEvent> CaseEvents { get; set; } = new List<CaseEvent>();
}
