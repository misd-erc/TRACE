using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class PreFilingStatus
{
    public long PreFilingStatusId { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<PreFilingAssessment> PreFilingAssessments { get; set; } = new List<PreFilingAssessment>();
}
