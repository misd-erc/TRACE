using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class CaseNature
{
    public long CaseNatureId { get; set; }

    public string Nature { get; set; } = null!;

    public string? Description { get; set; }

    public long? CaseCategoryId { get; set; }

    public bool? Active { get; set; }

    public virtual CaseCategory? CaseCategory { get; set; }

    public virtual ICollection<Erccase> Erccases { get; set; } = new List<Erccase>();

    public virtual ICollection<SubCaseNature> SubCaseNatures { get; set; } = new List<SubCaseNature>();
}
