using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class Province
{
    public long ProvinceId { get; set; }

    public string? ProvinceName { get; set; }

    public long? RegionId { get; set; }

    public virtual ICollection<Area> Areas { get; set; } = new List<Area>();

    public virtual Region? Region { get; set; }
}
