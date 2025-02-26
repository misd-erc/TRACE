using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class DocumentFilingType
{
    public long DocumentFilingTypeId { get; set; }

    public string FillingType { get; set; } = null!;

    public virtual ICollection<DocumentType> DocumentTypes { get; set; } = new List<DocumentType>();

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();
}
