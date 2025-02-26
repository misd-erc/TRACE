using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class ContactEntry
{
    public long ContactEntryId { get; set; }

    public string? Entry { get; set; }

    public virtual ICollection<ContactEntryValue> ContactEntryValues { get; set; } = new List<ContactEntryValue>();
}
