using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class CaseMilestone
{
    public long CaseMilestoneId { get; set; }

    public string Milestone { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<MilestonesAchieved> MilestonesAchieveds { get; set; } = new List<MilestonesAchieved>();

    public virtual ICollection<CaseMilestoneTemplate> CaseMilestoneTemplates { get; set; } = new List<CaseMilestoneTemplate>();
}
