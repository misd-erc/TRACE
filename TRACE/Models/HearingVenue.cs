using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class HearingVenue
{
    public long HearingVenueId { get; set; }

    public string VenueName { get; set; } = null!;

    public string? StreetAddress { get; set; }

    public long? CityId { get; set; }

    public virtual City? City { get; set; }

    public virtual ICollection<Hearing> Hearings { get; set; } = new List<Hearing>();
}
