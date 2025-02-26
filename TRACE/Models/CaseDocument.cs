using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class CaseDocument
{
    public long CaseDocumentId { get; set; }

    public long ErccaseId { get; set; }

    public long DocumentId { get; set; }

    public virtual Document Document { get; set; } = null!;

    public virtual Erccase Erccase { get; set; } = null!;
}
