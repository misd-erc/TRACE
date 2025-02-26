using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class ExternalCaseStatus
{
    public long ExternalCaseStatusId { get; set; }

    public string CaseStatus { get; set; } = null!;

    public virtual ICollection<ExternalCase> ExternalCases { get; set; } = new List<ExternalCase>();
}
