using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class ContactEntryValue
{
    public long ContactEntryValueId { get; set; }

    public long? ContactEntryId { get; set; }

    public long? ContactId { get; set; }

    public string? EntryValue { get; set; }

    public virtual Contact? Contact { get; set; }

    public virtual ContactEntry? ContactEntry { get; set; }
}
