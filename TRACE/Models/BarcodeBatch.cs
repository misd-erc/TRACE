using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class BarcodeBatch
{
    public long BarcodeBatchId { get; set; }

    public bool IsPrinted { get; set; }

    public DateOnly DateGenerated { get; set; }

    public long? DocumentCategoryId { get; set; }

    public virtual ICollection<Barcode> Barcodes { get; set; } = new List<Barcode>();

    public virtual DocumentCategory? DocumentCategory { get; set; }
}
