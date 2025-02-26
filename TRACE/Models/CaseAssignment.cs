using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class CaseAssignment
{
    public long CaseAssignmentId { get; set; }

    public string UserId { get; set; } = null!;

    public long ErccaseId { get; set; }

    public DateOnly DateAssigned { get; set; }

    public string? AssignedBy { get; set; }

    public long HandlingOfficerTypeId { get; set; }

    public virtual Erccase Erccase { get; set; } = null!;

    public virtual HandlingOfficerType HandlingOfficerType { get; set; } = null!;
}
