using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class Subscription
{
    public long SubscriptionId { get; set; }

    public long DocumentTypeId { get; set; }

    public long DivisionId { get; set; }

    public bool? EnableNotification { get; set; }

    public virtual Division Division { get; set; } = null!;

    public virtual DocumentType DocumentType { get; set; } = null!;
}
