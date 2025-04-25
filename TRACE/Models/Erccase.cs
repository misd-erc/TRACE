using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TRACE.Models;

public partial class Erccase
{
    public long ErccaseId { get; set; }

    public string CaseNo { get; set; } = null!;

    [Display(Name = "Choose Case Category")]
    public long CaseCategoryId { get; set; }

    public string? Title { get; set; }

    [Display(Name = "Choose Case Nature")]
    public long? CaseNatureId { get; set; }

    public DateOnly DateFiled { get; set; }

    public DateOnly? DateDocketed { get; set; }

    public string? DocketedBy { get; set; }

    public long CaseStatusId { get; set; }

    public string? Synopsis { get; set; }

    public int? NoOfFolders { get; set; }

    public string? MeterSin { get; set; }

    public decimal? AmountClaimed { get; set; }

    public decimal? AmountSettled { get; set; }

    public bool? IsArchived { get; set; }

    public DateOnly? TargetPaissuance { get; set; }

    public DateOnly? ActualPaissuance { get; set; }

    public DateOnly? TargetFaissuance { get; set; }

    public DateOnly? ActualFaissuance { get; set; }

    public DateOnly? SubmittedForResolution { get; set; }

    public bool? PrayedForPa { get; set; }

    public bool? IsApproved { get; set; }

    public string? ApprovedBy { get; set; }

    public DateTime? DatetimeApproved { get; set; }

    public string? CaseBoxLocation { get; set; }

    public DateOnly? PadeliberationDate { get; set; }

    public DateOnly? FadeliberationDate { get; set; }

    public DateOnly? PatargetOrder { get; set; }

    public DateOnly? FatargetOrder { get; set; }

    public string? AssignedTo { get; set; }

    public virtual ICollection<CaseApplicant> CaseApplicants { get; set; } = new List<CaseApplicant>();

    public virtual ICollection<CaseAssignment> CaseAssignments { get; set; } = new List<CaseAssignment>();

    public virtual CaseCategory CaseCategory { get; set; } = null!;

    public virtual ICollection<CaseDocument> CaseDocuments { get; set; } = new List<CaseDocument>();

    public virtual ICollection<CaseEvent> CaseEvents { get; set; } = new List<CaseEvent>();

    public virtual CaseNature? CaseNature { get; set; }

    public virtual ICollection<CaseNote> CaseNotes { get; set; } = new List<CaseNote>();

    public virtual ICollection<CaseRespondent> CaseRespondents { get; set; } = new List<CaseRespondent>();

    public virtual CaseStatus CaseStatus { get; set; } = null!;

    public virtual ICollection<CaseTask> CaseTasks { get; set; } = new List<CaseTask>();

    public virtual ICollection<Hearing> Hearings { get; set; } = new List<Hearing>();

    public virtual ICollection<MilestonesAchieved> MilestonesAchieveds { get; set; } = new List<MilestonesAchieved>();

    public virtual ICollection<RelatedCase> RelatedCaseErccaseRelateds { get; set; } = new List<RelatedCase>();

    public virtual ICollection<RelatedCase> RelatedCaseErccases { get; set; } = new List<RelatedCase>();

    public virtual ICollection<ExternalCase> ExternalCases { get; set; } = new List<ExternalCase>();
}
