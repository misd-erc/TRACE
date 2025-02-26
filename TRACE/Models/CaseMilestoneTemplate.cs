using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class CaseMilestoneTemplate
{
    public long CaseMilestoneTemplateId { get; set; }

    public string TemplateName { get; set; } = null!;

    public long? CaseCategoryId { get; set; }

    public virtual CaseCategory? CaseCategory { get; set; }

    public virtual ICollection<CaseMilestone> CaseMilestones { get; set; } = new List<CaseMilestone>();
}
