using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class PreFilingCorrespondent
{
    public long PreFilingCorrespondentId { get; set; }

    public long? CorrespondentId { get; set; }

    public long? CompanyId { get; set; }

    public long PreFilingAssessmentId { get; set; }

    public virtual Company? Company { get; set; }

    public virtual Correspondent? Correspondent { get; set; }

    public virtual PreFilingAssessment PreFilingAssessment { get; set; } = null!;
}
