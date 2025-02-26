using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class CorrespondentCategory
{
    public long CorrespondentCategoryId { get; set; }

    public string Category { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<DocumentCorrespondent> DocumentCorrespondents { get; set; } = new List<DocumentCorrespondent>();
}
