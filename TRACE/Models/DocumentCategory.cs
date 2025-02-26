using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class DocumentCategory
{
    public long DocumentCategoryId { get; set; }

    public string Category { get; set; } = null!;

    public virtual ICollection<BarcodeBatch> BarcodeBatches { get; set; } = new List<BarcodeBatch>();

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();
}
