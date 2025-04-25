using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class CaseBlobDocument
{
    public int DocumentId { get; set; }

    public string AttachmentName { get; set; } = null!;

    public int Ercid { get; set; }

    public string AttachmentLink { get; set; } = null!;

    public DateTime? UploadedAt { get; set; }
}
