using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class HandlingOfficerType
{
    public long HandlingOfficerTypeId { get; set; }

    public string OfficerType { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<CaseAssignment> CaseAssignments { get; set; } = new List<CaseAssignment>();
}
