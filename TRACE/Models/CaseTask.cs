using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class CaseTask
{
    public long CaseTaskId { get; set; }

    public long ErccaseId { get; set; }

    public string UserId { get; set; } = null!;

    public string Task { get; set; } = null!;

    public string TaskedBy { get; set; } = null!;

    public DateTime DatetimeCreated { get; set; }

    public DateOnly? TargetCompletionDate { get; set; }

    public DateOnly? ActualCompletionDate { get; set; }

    public long? DocumentId { get; set; }

    public virtual Document? Document { get; set; }

    public virtual Erccase Erccase { get; set; } = null!;
}
