using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class UserAction
{
    public long UserActionId { get; set; }

    public string ActionTaken { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public DateTime DatetimeCreated { get; set; }

    public long DocumentId { get; set; }

    public long? ActionTypeId { get; set; }

    public bool? IsApproved { get; set; }

    public string? ApprovedBy { get; set; }

    public DateTime? DatetimeApproved { get; set; }

    public virtual ActionType? ActionType { get; set; }

    public virtual Document Document { get; set; } = null!;
}
