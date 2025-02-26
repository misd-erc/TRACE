using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class NeededAction
{
    public long NeededActionId { get; set; }

    public string Action { get; set; } = null!;

    public virtual ICollection<DocumentAssignment> DocumentAssignments { get; set; } = new List<DocumentAssignment>();

    public virtual ICollection<DocumentForward> DocumentForwards { get; set; } = new List<DocumentForward>();
}
