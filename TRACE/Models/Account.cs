using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class Account
{
    public long AccountId { get; set; }

    public string? AccountName { get; set; }

    public string? AccountNo { get; set; }

    public string? ContactPerson { get; set; }

    public string? AccountType { get; set; }

    public string? BankName { get; set; }

    public string? Branch { get; set; }

    public string? PrimaryMobileNo { get; set; }

    public string? EmailAddress { get; set; }

    public long? AccountCategoryId { get; set; }

    public string? AlternateEmailAddress { get; set; }

    public string? SecondaryMobileNo { get; set; }

    public string? LastName { get; set; }

    public string? FirstName { get; set; }

    public string? MiddleInitial { get; set; }

    public virtual AccountCategory? AccountCategory { get; set; }

    public virtual ICollection<AccountPayable> AccountPayables { get; set; } = new List<AccountPayable>();

    public virtual ICollection<AccountGroup> AccountGroups { get; set; } = new List<AccountGroup>();
}
