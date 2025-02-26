using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class DistributionListMember
{
    public long DistributionListMemberId { get; set; }

    public long? CorrespondentId { get; set; }

    public long DistributionListId { get; set; }

    public long? CompanyId { get; set; }

    public virtual Company? Company { get; set; }

    public virtual Correspondent? Correspondent { get; set; }

    public virtual DistributionList DistributionList { get; set; } = null!;
}
