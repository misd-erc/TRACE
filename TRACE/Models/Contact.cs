using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class Contact
{
    public long ContactId { get; set; }

    public string? Lastname { get; set; }

    public string? Firstname { get; set; }

    public string? Middlename { get; set; }

    public string? StreetAddress { get; set; }

    public long? AreaId { get; set; }

    public string? Salutation { get; set; }

    public string? ContactNos { get; set; }

    public string? EmailAddress { get; set; }

    public long? EntityId { get; set; }

    public string? Designation { get; set; }

    public virtual Area? Area { get; set; }

    public virtual ICollection<ContactEntryValue> ContactEntryValues { get; set; } = new List<ContactEntryValue>();

    public virtual Entity? Entity { get; set; }
}
