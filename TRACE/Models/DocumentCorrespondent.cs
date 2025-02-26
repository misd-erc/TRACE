using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class DocumentCorrespondent
{
    public long DocumentCorrespondentId { get; set; }

    public long? CorrespondentId { get; set; }

    public long? CompanyId { get; set; }

    public long DocumentId { get; set; }

    public long? CorrespondentCategoryId { get; set; }

    public virtual Company? Company { get; set; }

    public virtual Correspondent? Correspondent { get; set; }

    public virtual CorrespondentCategory? CorrespondentCategory { get; set; }

    public virtual Document Document { get; set; } = null!;
}
