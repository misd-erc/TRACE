using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class Correspondent
{
    public long CorrespondentId { get; set; }

    public string? Salutation { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string? Designation { get; set; }

    public string AddressLine1 { get; set; } = null!;

    public string? AddressLine2 { get; set; }

    public long? CityId { get; set; }

    public string? ZipCode { get; set; }

    public string? ContactNos { get; set; }

    public string? EmailAddress { get; set; }

    public long? CompanyId { get; set; }

    public virtual ICollection<CaseApplicant> CaseApplicants { get; set; } = new List<CaseApplicant>();

    public virtual ICollection<CaseRespondent> CaseRespondents { get; set; } = new List<CaseRespondent>();

    public virtual City? City { get; set; }

    public virtual Company? Company { get; set; }

    public virtual ICollection<DistributionListMember> DistributionListMembers { get; set; } = new List<DistributionListMember>();

    public virtual ICollection<DocumentCorrespondent> DocumentCorrespondents { get; set; } = new List<DocumentCorrespondent>();

    public virtual ICollection<OutgoingRecipient> OutgoingRecipients { get; set; } = new List<OutgoingRecipient>();

    public virtual ICollection<PreFilingCorrespondent> PreFilingCorrespondents { get; set; } = new List<PreFilingCorrespondent>();
}
