using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TRACE.Models;

public partial class CaseMilestoneTemplate
{
    public long CaseMilestoneTemplateId { get; set; }

    [Display(Name = "Template Name")]
    public string TemplateName { get; set; } = null!;

    [Display(Name = "Case Category")]
    public long? CaseCategoryId { get; set; }

    public virtual CaseCategory? CaseCategory { get; set; }

    public virtual ICollection<CaseMilestone> CaseMilestones { get; set; } = new List<CaseMilestone>();
    public ICollection<CaseMilestoneTemplateMember> CaseMilestoneTemplateMembers { get; set; }

}
