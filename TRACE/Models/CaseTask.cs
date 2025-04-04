using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TRACE.Models;

public partial class CaseTask
{
    public long CaseTaskId { get; set; }

    public long ErccaseId { get; set; }

    [Display(Name = "Choose User")]
    public string UserId { get; set; } = null!;

    public string Task { get; set; } = null!;

    public string TaskedBy { get; set; } = null!;

    public DateTime DatetimeCreated { get; set; }

    [Display(Name = "Target Completion Date")]
    public DateOnly? TargetCompletionDate { get; set; }

    public DateOnly? ActualCompletionDate { get; set; }

    [Display(Name = "Select Document")]
    public long? DocumentId { get; set; }

    public virtual Document? Document { get; set; }

    public virtual Erccase Erccase { get; set; } = null!;
}
