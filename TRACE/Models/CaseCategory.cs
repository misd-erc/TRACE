using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class CaseCategory
{
    public long CaseCategoryId { get; set; }

    public string Category { get; set; } = null!;

    public string? Description { get; set; }

    public bool? IsInternal { get; set; }

    public virtual ICollection<CaseMilestoneTemplate> CaseMilestoneTemplates { get; set; } = new List<CaseMilestoneTemplate>();

    public virtual ICollection<CaseNature> CaseNatures { get; set; } = new List<CaseNature>();

    public virtual ICollection<Erccase> Erccases { get; set; } = new List<Erccase>();
}
