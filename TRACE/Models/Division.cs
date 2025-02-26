using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class Division
{
    public long DivisionId { get; set; }

    public string? DivisionName { get; set; }

    public string? Description { get; set; }

    public long? ServiceId { get; set; }

    public string? Secretary { get; set; }

    public virtual ICollection<DocumentCopy> DocumentCopies { get; set; } = new List<DocumentCopy>();

    public virtual ICollection<DocumentForward> DocumentForwardDivisions { get; set; } = new List<DocumentForward>();

    public virtual ICollection<DocumentForward> DocumentForwardFromDivisions { get; set; } = new List<DocumentForward>();

    public virtual ICollection<DocumentLocation> DocumentLocations { get; set; } = new List<DocumentLocation>();

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();

    public virtual ICollection<InternalDocument> InternalDocumentDeliveryTos { get; set; } = new List<InternalDocument>();

    public virtual ICollection<InternalDocument> InternalDocumentRecievedFroms { get; set; } = new List<InternalDocument>();

    public virtual Service? Service { get; set; }

    public virtual ICollection<SharepointMapping> SharepointMappings { get; set; } = new List<SharepointMapping>();

    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}
