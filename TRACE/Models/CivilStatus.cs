using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class CivilStatus
{
    public int CivilStatusId { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
