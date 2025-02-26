using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class DocumentCopy
{
    public long DocumentCopyId { get; set; }

    public long DocumentId { get; set; }

    public long DivisionId { get; set; }

    public DateTime DatetimeSent { get; set; }

    public string? SentBy { get; set; }

    public virtual Division Division { get; set; } = null!;

    public virtual Document Document { get; set; } = null!;
}
