using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class Office
{
    public long OfficeId { get; set; }

    public string? OfficeName { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
}
