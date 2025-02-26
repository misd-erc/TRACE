using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class Employee
{
    public long EmployeeId { get; set; }

    public string? EmployeeNo { get; set; }

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public string MiddleName { get; set; } = null!;

    public string? NameExtension { get; set; }

    public string? Salutation { get; set; }

    public int GenderId { get; set; }

    public int CivilStatusId { get; set; }

    public DateOnly Birthdate { get; set; }

    public string Citizenship { get; set; } = null!;

    public string? PermanentStreetAddress { get; set; }

    public long? PermanentAreaId { get; set; }

    public string? ResidentialStreetAddress { get; set; }

    public long? ResidentialAreaId { get; set; }

    public string? EmailAddress { get; set; }

    public string? TelephoneNo { get; set; }

    public string? MobileNo { get; set; }

    public int? MeterHeight { get; set; }

    public decimal? Kgweight { get; set; }

    public string? BloodType { get; set; }

    public string? Gsisid { get; set; }

    public string? Pagibigid { get; set; }

    public string? PhilhealthNo { get; set; }

    public string? Sssno { get; set; }

    public string? Tin { get; set; }

    public string? UserId { get; set; }

    public virtual CivilStatus CivilStatus { get; set; } = null!;

    public virtual Gender Gender { get; set; } = null!;

    public virtual Area? PermanentArea { get; set; }

    public virtual Area? ResidentialArea { get; set; }
}
