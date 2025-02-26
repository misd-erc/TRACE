using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class ActionType
{
    public long ActionTypeId { get; set; }

    public string Type { get; set; } = null!;

    public virtual ICollection<UserAction> UserActions { get; set; } = new List<UserAction>();
}
