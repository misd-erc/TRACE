using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class Entity
{
    public long EntityId { get; set; }

    public string? Acronym { get; set; }

    public string? EntityName { get; set; }

    public long? EntityCategoryId { get; set; }

    public string? EntityCode { get; set; }

    public string? StreetAddress { get; set; }

    public long? AreaId { get; set; }

    public virtual Area? Area { get; set; }

    public virtual ICollection<Contact> Contacts { get; set; } = new List<Contact>();

    public virtual EntityCategory? EntityCategory { get; set; }
}
