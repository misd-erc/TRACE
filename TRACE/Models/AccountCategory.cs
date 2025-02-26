using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class AccountCategory
{
    public long AccountCategoryId { get; set; }

    public string? Category { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}
