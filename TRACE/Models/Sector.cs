using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class Sector
{
    public long SectorId { get; set; }

    public string? SectorName { get; set; }

    public virtual ICollection<EntityCategory> EntityCategories { get; set; } = new List<EntityCategory>();
}
