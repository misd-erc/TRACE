using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class PreFilingLog
{
    public long PreFilingLogId { get; set; }

    public DateTime LogDatetime { get; set; }

    public string UserId { get; set; } = null!;

    public string ActionTaken { get; set; } = null!;

    public long PreFilingAssessmentId { get; set; }

    public virtual PreFilingAssessment PreFilingAssessment { get; set; } = null!;
}
