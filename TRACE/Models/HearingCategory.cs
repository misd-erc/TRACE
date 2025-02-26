using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class HearingCategory
{
    public long HearingCategoryId { get; set; }

    public string Category { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Hearing> Hearings { get; set; } = new List<Hearing>();
}
