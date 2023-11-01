using System;
using System.Collections.Generic;

namespace JwtCrud.Models;

public partial class Employee
{
    public int EmpId { get; set; }

    public string? EmpName { get; set; }

    public string? EmpDept { get; set; }

    public string? Password { get; set; }

    public string? RoleName { get; set; } = "User";
}
