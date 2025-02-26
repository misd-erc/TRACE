using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class SharepointMapping
{
    public long SharpointMappingId { get; set; }

    public string SharepointLocation { get; set; } = null!;

    public long DivisionId { get; set; }

    public virtual Division Division { get; set; } = null!;
}
