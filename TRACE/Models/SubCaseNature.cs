using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class SubCaseNature
{
    public int SubNatureId { get; set; }

    public string SubNatureName { get; set; } = null!;

    public string? Description { get; set; }

    public long CaseNatureId { get; set; }

    public bool? IsInternal { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual CaseNature CaseNature { get; set; } = null!;
}
