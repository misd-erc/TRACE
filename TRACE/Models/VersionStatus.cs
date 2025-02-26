using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class VersionStatus
{
    public long VersionStatusId { get; set; }

    public string Status { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<DocumentVersion> DocumentVersions { get; set; } = new List<DocumentVersion>();
}
