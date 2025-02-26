using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class CaseNote
{
    public long CaseNoteId { get; set; }

    public string? Notes { get; set; }

    public long ErccaseId { get; set; }

    public DateTime DatetimeCreated { get; set; }

    public string NotedBy { get; set; } = null!;

    public virtual Erccase Erccase { get; set; } = null!;
}
