using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class HearingOfficerType
{
    public long HearingOfficerTypeId { get; set; }

    public string OfficerType { get; set; } = null!;
    public string description { get; set; } = "No Description Needed";

    public virtual ICollection<HearingOfficer> HearingOfficers { get; set; } = new List<HearingOfficer>();
}
