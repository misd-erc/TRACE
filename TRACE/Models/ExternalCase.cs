using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class ExternalCase
{
    public long ExternalCaseId { get; set; }

    public string CaseRefNo { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string? SubjectMatter { get; set; }

    public long ExternalCaseCategoryId { get; set; }

    public long ExternalCaseStatusId { get; set; }

    public long? ParentCaseId { get; set; }

    public virtual ExternalCaseCategory ExternalCaseCategory { get; set; } = null!;

    public virtual ICollection<ExternalCaseDocument> ExternalCaseDocuments { get; set; } = new List<ExternalCaseDocument>();

    public virtual ExternalCaseStatus ExternalCaseStatus { get; set; } = null!;

    public virtual ICollection<ExternalCase> InverseParentCase { get; set; } = new List<ExternalCase>();

    public virtual ExternalCase? ParentCase { get; set; }

    public virtual ICollection<Erccase> Erccases { get; set; } = new List<Erccase>();
}
