using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class Barcode
{
    public long BarcodeId { get; set; }

    public long BarcodeBatchId { get; set; }

    public string BarcodeNo { get; set; } = null!;

    public virtual BarcodeBatch BarcodeBatch { get; set; } = null!;
}
