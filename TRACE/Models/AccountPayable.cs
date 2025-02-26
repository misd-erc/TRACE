using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class AccountPayable
{
    public long AccountPayableId { get; set; }

    public long AdviceId { get; set; }

    public string? ObligationRequestNo { get; set; }

    public string? AllotmentClass { get; set; }

    public decimal? WithholdingTax { get; set; }

    public bool? IsCurrentYear { get; set; }

    public long? AccountId { get; set; }

    public virtual Account? Account { get; set; }

    public virtual Advice Advice { get; set; } = null!;

    public virtual ICollection<PaymentDetail> PaymentDetails { get; set; } = new List<PaymentDetail>();
}
