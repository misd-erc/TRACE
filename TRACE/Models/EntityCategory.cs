using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class EntityCategory
{
    public long EntityCategoryId { get; set; }

    public string? Ecategory { get; set; }

    public long? SectorId { get; set; }

    public virtual ICollection<Company> Companies { get; set; } = new List<Company>();

    public virtual ICollection<Entity> Entities { get; set; } = new List<Entity>();

    public virtual Sector? Sector { get; set; }
}
