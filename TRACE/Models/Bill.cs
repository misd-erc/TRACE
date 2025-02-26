using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class Bill
{
    public long BillingId { get; set; }

    public string? BillNo { get; set; }

    public long BillTypeId { get; set; }

    public string AssignedStaff { get; set; } = null!;

    public DateOnly DateIssued { get; set; }

    public DateOnly? DueDate { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime DatetimeCreated { get; set; }

    public bool IsApproved { get; set; }

    public DateTime? DatetimeApproved { get; set; }
}
