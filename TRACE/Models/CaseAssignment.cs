﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TRACE.Models;

public partial class CaseAssignment
{
    public long CaseAssignmentId { get; set; }

    [Display(Name = "Choose User")]
    public string UserId { get; set; } = null!;

    public long ErccaseId { get; set; }

    public DateOnly DateAssigned { get; set; }

    public string? AssignedBy { get; set; }

    public bool IsActive { get; set; }

    public DateOnly? DateRemoved { get; set; }

    [Display(Name = "Select Handling Officer Type")]
    public long HandlingOfficerTypeId { get; set; }

    public virtual Erccase Erccase { get; set; } = null!;

    public virtual HandlingOfficerType HandlingOfficerType { get; set; } = null!;
}
