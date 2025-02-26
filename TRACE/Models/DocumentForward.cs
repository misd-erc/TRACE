using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class DocumentForward
{
    public long DocumentForwardId { get; set; }

    public long DocumentId { get; set; }

    public long DivisionId { get; set; }

    public DateTime? DatetimeForwarded { get; set; }

    public string? ForwardedBy { get; set; }

    public string? Remarks { get; set; }

    public long? FromDivisionId { get; set; }

    public bool? IsApproved { get; set; }

    public string? ApprovedBy { get; set; }

    public DateTime? DatetimeApproved { get; set; }

    public virtual Division Division { get; set; } = null!;

    public virtual Document Document { get; set; } = null!;

    public virtual Division? FromDivision { get; set; }

    public virtual ICollection<NeededAction> NeededActions { get; set; } = new List<NeededAction>();
}
