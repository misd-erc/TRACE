using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class PreFilingCategory
{
    public long PreFilingCategoryId { get; set; }

    public string Category { get; set; } = null!;

    public virtual ICollection<PreFilingAssessment> PreFilingAssessments { get; set; } = new List<PreFilingAssessment>();
}
