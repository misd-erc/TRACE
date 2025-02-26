using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class OutgoingRecipient
{
    public long OutgoingRecipientId { get; set; }

    public long DocumentId { get; set; }

    public long? CorrespondentId { get; set; }

    public DateOnly DateDelivered { get; set; }

    public long? DeliveryMethodId { get; set; }

    public long? CompanyId { get; set; }

    public virtual Company? Company { get; set; }

    public virtual Correspondent? Correspondent { get; set; }

    public virtual DeliveryMethod? DeliveryMethod { get; set; }

    public virtual Document Document { get; set; } = null!;
}
