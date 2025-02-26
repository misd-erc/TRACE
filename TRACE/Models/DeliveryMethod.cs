using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class DeliveryMethod
{
    public long DeliveryMethodId { get; set; }

    public string Method { get; set; } = null!;

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();

    public virtual ICollection<OutgoingRecipient> OutgoingRecipients { get; set; } = new List<OutgoingRecipient>();
}
