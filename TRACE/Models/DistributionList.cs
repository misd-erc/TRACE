using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class DistributionList
{
    public long DistributionListId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<DistributionListMember> DistributionListMembers { get; set; } = new List<DistributionListMember>();
}
