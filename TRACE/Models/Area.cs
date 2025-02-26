using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class Area
{
    public long AreaId { get; set; }

    public string? AreaName { get; set; }

    public long? ProvinceId { get; set; }

    public string? ZipCode { get; set; }

    public virtual ICollection<Contact> Contacts { get; set; } = new List<Contact>();

    public virtual ICollection<Employee> EmployeePermanentAreas { get; set; } = new List<Employee>();

    public virtual ICollection<Employee> EmployeeResidentialAreas { get; set; } = new List<Employee>();

    public virtual ICollection<Entity> Entities { get; set; } = new List<Entity>();

    public virtual Province? Province { get; set; }
}
