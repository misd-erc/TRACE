using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class HearingOfficer
{
    public long HearingOfficerId { get; set; }

    public long HearingId { get; set; }

    public string UserId { get; set; } = null!;

    public long HearingOfficerTypeId { get; set; }

    public virtual Hearing Hearing { get; set; } = null!;

    public virtual HearingOfficerType HearingOfficerType { get; set; } = null!;
}
