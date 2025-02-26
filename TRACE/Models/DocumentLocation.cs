using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class DocumentLocation
{
    public long DocumentLocationId { get; set; }

    public long DivisionId { get; set; }

    public long DocumentId { get; set; }

    public DateTime DatetimeReceived { get; set; }

    public string? ReceivedBy { get; set; }

    public bool? IsApproved { get; set; }

    public string? ApprovedBy { get; set; }

    public DateTime? DatetimeApproved { get; set; }

    public virtual Division Division { get; set; } = null!;

    public virtual Document Document { get; set; } = null!;
}
