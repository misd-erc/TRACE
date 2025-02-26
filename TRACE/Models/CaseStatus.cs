using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class CaseStatus
{
    public long CaseStatusId { get; set; }

    public string Status { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Erccase> Erccases { get; set; } = new List<Erccase>();
}
