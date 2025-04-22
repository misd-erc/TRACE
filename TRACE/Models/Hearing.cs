using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class Hearing
{
    public long HearingId { get; set; }

    public long ErccaseId { get; set; }

    public DateOnly HearingDate { get; set; }

    public string Time { get; set; } = null!;

    public long HearingVenueId { get; set; }

    public string? Remarks { get; set; }

    public long HearingCategoryId { get; set; }

    public bool? IsApproved { get; set; }

    public string? ApprovedBy { get; set; }

    public DateTime? DatetimeApproved { get; set; }

    public string? OtherVenue { get; set; }

    public virtual Erccase Erccase { get; set; } = null!;

    public virtual HearingCategory HearingCategory { get; set; } = null!;

    public virtual ICollection<HearingOfficer> HearingOfficers { get; set; } = new List<HearingOfficer>();

    public virtual HearingVenue HearingVenue { get; set; } = null!;

    public virtual ICollection<HearingType> HearingTypes { get; set; } = new List<HearingType>();

    public virtual ICollection<HearingsInHearingType> HearingsInHearingTypes { get; set; } = new List<HearingsInHearingType>();

}
