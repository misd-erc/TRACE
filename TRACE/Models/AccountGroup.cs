using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class AccountGroup
{
    public long AccountGroupId { get; set; }

    public string? GroupName { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}
