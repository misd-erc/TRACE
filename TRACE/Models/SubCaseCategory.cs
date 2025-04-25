using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class SubCaseCategory
{
    public int SubCategoryId { get; set; }

    public string SubCategoryName { get; set; } = null!;

    public string? Description { get; set; }

    public long CaseCategoryId { get; set; }

    public bool? IsInternal { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual CaseCategory CaseCategory { get; set; } = null!;
}
