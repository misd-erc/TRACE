using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class DocumentType
{
    public long DocumentTypeId { get; set; }

    public string TypeDescription { get; set; } = null!;

    public string? TypeContents { get; set; }

    public bool? IsInternal { get; set; }

    public long? DocumentFilingTypeId { get; set; }

    public virtual DocumentFilingType? DocumentFilingType { get; set; }

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();

    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}
