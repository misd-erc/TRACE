using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TRACE.Models;

public partial class MilestonesAchieved
{
    public long MilestoneAchievedId { get; set; }

    public long ErccaseId { get; set; }

    [Display(Name = "Case Milestone")]
    public long CaseMilestoneId { get; set; }

    public DateTime DatetimeAchieved { get; set; }

    public decimal? PercentAchieved { get; set; }

    public virtual CaseMilestone CaseMilestone { get; set; } = null!;

    public virtual Erccase Erccase { get; set; } = null!;
}
