using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class InternalDocument
{
    public long InternalDocumentId { get; set; }

    public long ServiceId { get; set; }

    public string? RefNo { get; set; }

    public string Particular { get; set; } = null!;

    public DateTime? InDatetime { get; set; }

    public DateTime? OutDatetime { get; set; }

    public long? DocumentId { get; set; }

    public long? RecievedFromId { get; set; }

    public long? DeliveryToId { get; set; }

    public string? Remarks { get; set; }

    public virtual Division? DeliveryTo { get; set; }

    public virtual Document? Document { get; set; }

    public virtual Division? RecievedFrom { get; set; }

    public virtual Service Service { get; set; } = null!;
}
