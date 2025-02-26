using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class Service
{
    public long ServiceId { get; set; }

    public string? ServiceName { get; set; }

    public string? Description { get; set; }

    public long? OfficeId { get; set; }

    public string? Secretary { get; set; }

    public virtual ICollection<Division> Divisions { get; set; } = new List<Division>();

    public virtual ICollection<InternalDocument> InternalDocuments { get; set; } = new List<InternalDocument>();

    public virtual Office? Office { get; set; }
}
