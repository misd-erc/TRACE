using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class AdviceStatus
{
    public long AdviceStatusId { get; set; }

    public string? Status { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Advice> Advices { get; set; } = new List<Advice>();
}
