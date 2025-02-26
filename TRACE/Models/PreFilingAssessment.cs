using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class PreFilingAssessment
{
    public long PreFilingAssessmentId { get; set; }

    public string AssessedBy { get; set; } = null!;

    public DateTime DatetimeAssessed { get; set; }

    public string SubjectMatter { get; set; } = null!;

    public string? Remarks { get; set; }

    public long? PreFilingStatusId { get; set; }

    public string AssessmentNo { get; set; } = null!;

    public long PreFilingCategoryId { get; set; }

    public virtual PreFilingCategory PreFilingCategory { get; set; } = null!;

    public virtual ICollection<PreFilingCorrespondent> PreFilingCorrespondents { get; set; } = new List<PreFilingCorrespondent>();

    public virtual ICollection<PreFilingLog> PreFilingLogs { get; set; } = new List<PreFilingLog>();

    public virtual PreFilingStatus? PreFilingStatus { get; set; }
}
