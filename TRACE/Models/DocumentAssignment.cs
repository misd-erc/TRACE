using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class DocumentAssignment
{
    public long DocumentAssignmentId { get; set; }

    public long DocumentId { get; set; }

    public string UserId { get; set; } = null!;

    public DateTime? DatetimeAssigned { get; set; }

    public string? AssignedBy { get; set; }

    public string? Remarks { get; set; }

    public DateOnly? DateCompleted { get; set; }

    public virtual Document Document { get; set; } = null!;

    public virtual ICollection<NeededAction> NeededActions { get; set; } = new List<NeededAction>();
}
