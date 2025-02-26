using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class ExternalCaseDocument
{
    public long ExternalCaseDocumentId { get; set; }

    public long ExternalCaseId { get; set; }

    public long? DocumentId { get; set; }

    public string? Resolution { get; set; }

    public string? Pleadings { get; set; }

    public string? DocumentRefNo { get; set; }

    public DateOnly? ReceiptDate { get; set; }

    public string? DocumentTitle { get; set; }

    public DateOnly? DocumentDate { get; set; }

    public virtual Document? Document { get; set; }

    public virtual ExternalCase ExternalCase { get; set; } = null!;
}
