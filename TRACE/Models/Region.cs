using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class Region
{
    public long RegionId { get; set; }

    public string? RegionName { get; set; }

    public long? GridId { get; set; }

    public virtual Grid? Grid { get; set; }

    public virtual ICollection<Province> Provinces { get; set; } = new List<Province>();
}
