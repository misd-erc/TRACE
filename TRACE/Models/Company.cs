using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class Company
{
    public long CompanyId { get; set; }

    public string CompanyName { get; set; } = null!;

    public string AddressLine1 { get; set; } = null!;

    public string? AddressLine2 { get; set; }

    public long? CityId { get; set; }

    public string? ZipCode { get; set; }

    public long EntityCategoryId { get; set; }

    public string? ShortName { get; set; }

    public string? Region { get; set; }

    public virtual ICollection<CaseApplicant> CaseApplicants { get; set; } = new List<CaseApplicant>();

    public virtual ICollection<CaseRespondent> CaseRespondents { get; set; } = new List<CaseRespondent>();

    public virtual City? City { get; set; }

    public virtual ICollection<Correspondent> Correspondents { get; set; } = new List<Correspondent>();

    public virtual ICollection<DistributionListMember> DistributionListMembers { get; set; } = new List<DistributionListMember>();

    public virtual ICollection<DocumentCorrespondent> DocumentCorrespondents { get; set; } = new List<DocumentCorrespondent>();

    public virtual EntityCategory EntityCategory { get; set; } = null!;

    public virtual ICollection<OutgoingRecipient> OutgoingRecipients { get; set; } = new List<OutgoingRecipient>();

    public virtual ICollection<PreFilingCorrespondent> PreFilingCorrespondents { get; set; } = new List<PreFilingCorrespondent>();

}
