using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class Grid
{
    public long GridId { get; set; }

    public string? GridName { get; set; }

    public virtual ICollection<Region> Regions { get; set; } = new List<Region>();
}
