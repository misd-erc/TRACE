using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class Country
{
    public long CountryId { get; set; }

    public string CountryName { get; set; } = null!;

    public string? Acronym { get; set; }

    public virtual ICollection<State> States { get; set; } = new List<State>();
}
