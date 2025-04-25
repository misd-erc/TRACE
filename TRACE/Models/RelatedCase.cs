using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class RelatedCase
{
    public long RelatedCaseId { get; set; }

    public long ErccaseId { get; set; }

    public long ErccaseRelatedId { get; set; }

    public string RelatedBy { get; set; } = null!;

    public DateTime DatetimeRelated { get; set; }

    public virtual Erccase Erccase { get; set; } = null!;

    public virtual Erccase ErccaseRelated { get; set; } = null!;
}
