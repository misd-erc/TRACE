using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class Document
{
    public long DocumentId { get; set; }

    public long DocumentFilingTypeId { get; set; }

    public DateOnly DocumentDate { get; set; }

    public long? DocumentTypeId { get; set; }

    public string? Subject { get; set; }

    public DateTime DatetimeFiled { get; set; }

    public string? BarcodeNo { get; set; }

    public long? DocumentCategoryId { get; set; }

    public string? InternalAuthor { get; set; }

    public bool? IsFiled { get; set; }

    public string? CustomerBarcode { get; set; }

    public DateTime? DatetimeInitiated { get; set; }

    public long? OriginatingDivisionId { get; set; }

    public string? AttachedEvidences { get; set; }

    public bool? SealedDocuments { get; set; }

    public bool? IsApproved { get; set; }

    public string? ApprovedBy { get; set; }

    public DateTime? DatetimeApproved { get; set; }

    public bool? IsDraft { get; set; }

    public long? DeliveryMethodId { get; set; }

    public long OfficeId { get; set; }

    public DateTime? LastActivityDatetime { get; set; }

    public virtual ICollection<CaseDocument> CaseDocuments { get; set; } = new List<CaseDocument>();

    public virtual ICollection<CaseTask> CaseTasks { get; set; } = new List<CaseTask>();

    public virtual DeliveryMethod? DeliveryMethod { get; set; }

    public virtual ICollection<DocumentAssignment> DocumentAssignments { get; set; } = new List<DocumentAssignment>();

    public virtual DocumentCategory? DocumentCategory { get; set; }

    public virtual ICollection<DocumentCopy> DocumentCopies { get; set; } = new List<DocumentCopy>();

    public virtual ICollection<DocumentCorrespondent> DocumentCorrespondents { get; set; } = new List<DocumentCorrespondent>();

    public virtual DocumentFilingType DocumentFilingType { get; set; } = null!;

    public virtual ICollection<DocumentForward> DocumentForwards { get; set; } = new List<DocumentForward>();

    public virtual ICollection<DocumentLocation> DocumentLocations { get; set; } = new List<DocumentLocation>();

    public virtual ICollection<DocumentLog> DocumentLogs { get; set; } = new List<DocumentLog>();

    public virtual ICollection<DocumentReply> DocumentReplyDocuments { get; set; } = new List<DocumentReply>();

    public virtual ICollection<DocumentReply> DocumentReplyRepliedDocuments { get; set; } = new List<DocumentReply>();

    public virtual DocumentType? DocumentType { get; set; }

    public virtual ICollection<DocumentVersion> DocumentVersions { get; set; } = new List<DocumentVersion>();

    public virtual ICollection<ExternalCaseDocument> ExternalCaseDocuments { get; set; } = new List<ExternalCaseDocument>();

    public virtual ICollection<InternalDocument> InternalDocuments { get; set; } = new List<InternalDocument>();

    public virtual Office Office { get; set; } = null!;

    public virtual Division? OriginatingDivision { get; set; }

    public virtual ICollection<OutgoingRecipient> OutgoingRecipients { get; set; } = new List<OutgoingRecipient>();

    public virtual ICollection<UserAction> UserActions { get; set; } = new List<UserAction>();

    public virtual ICollection<UserComment> UserComments { get; set; } = new List<UserComment>();
}
