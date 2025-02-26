using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class PaymentDetail
{
    public long PaymentDetailId { get; set; }

    public string? Particular { get; set; }

    public decimal? Amount { get; set; }

    public long? AccountPayableId { get; set; }

    public virtual AccountPayable? AccountPayable { get; set; }
}
