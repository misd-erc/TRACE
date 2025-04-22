using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class HearingType
{
    public long HearingTypeId { get; set; }

    public string TypeOfHearing { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Hearing> Hearings { get; set; } = new List<Hearing>();

    public virtual ICollection<HearingsInHearingType> HearingsInHearingTypes { get; set; } = new List<HearingsInHearingType>();

}
