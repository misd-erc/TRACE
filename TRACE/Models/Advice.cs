using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class Advice
{
    public long AdviceId { get; set; }

    public string AdviceNo { get; set; } = null!;

    public DateTime DateTimeCreated { get; set; }

    public long AdviceStatusId { get; set; }

    public string? Remarks { get; set; }

    public string? Ncano { get; set; }

    public string? FundCode { get; set; }

    public DateOnly? DateIssued { get; set; }

    public virtual ICollection<AccountPayable> AccountPayables { get; set; } = new List<AccountPayable>();

    public virtual AdviceStatus AdviceStatus { get; set; } = null!;
}
