﻿using System;
using System.Collections.Generic;

namespace JwtCrud.Models;

public partial class Employee
{
    public int EmpId { get; set; }

    public string? EmpName { get; set; }

    public string? EmpDept { get; set; }
}
