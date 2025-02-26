using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class Gender
{
    public int GenderId { get; set; }

    public string GenderValue { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
