using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class City
{
    public long CityId { get; set; }

    public string CityName { get; set; } = null!;

    public long StateId { get; set; }

    public virtual ICollection<Company> Companies { get; set; } = new List<Company>();

    public virtual ICollection<Correspondent> Correspondents { get; set; } = new List<Correspondent>();

    public virtual ICollection<HearingVenue> HearingVenues { get; set; } = new List<HearingVenue>();

    public virtual State State { get; set; } = null!;
}
