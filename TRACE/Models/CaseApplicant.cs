using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class CaseApplicant
{
    public long CaseApplicantId { get; set; }

    public long ErccaseId { get; set; }

    public string? Remarks { get; set; }

    public long? CorrespondentId { get; set; }

    public long? CompanyId { get; set; }

    public virtual Company? Company { get; set; }

    public virtual Correspondent? Correspondent { get; set; }

    public virtual Erccase Erccase { get; set; } = null!;
}
