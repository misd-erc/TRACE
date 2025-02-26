using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class ExternalCaseCategory
{
    public long ExternalCaseCategoryId { get; set; }

    public string CaseCategory { get; set; } = null!;

    public virtual ICollection<ExternalCase> ExternalCases { get; set; } = new List<ExternalCase>();
}
