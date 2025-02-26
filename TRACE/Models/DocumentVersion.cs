using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class DocumentVersion
{
    public long DocumentVersionId { get; set; }

    public string FileVersion { get; set; } = null!;

    public DateTime UploadedDatetime { get; set; }

    public DateTime? ModifiedDatetime { get; set; }

    public string UploadedBy { get; set; } = null!;

    public long VersionStatusId { get; set; }

    public long DocumentId { get; set; }

    public string? FileExtension { get; set; }

    public string? SharepointLocation { get; set; }

    public virtual Document Document { get; set; } = null!;

    public virtual VersionStatus VersionStatus { get; set; } = null!;
}
