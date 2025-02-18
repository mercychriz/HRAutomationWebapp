using System;
using System.Collections.Generic;

namespace AFJOB_WEB.Models;

public partial class Employer
{
    public int EmployerId { get; set; }

    public int? UserId { get; set; }

    public string? CompanyName { get; set; }

    public string? CompanyWebsite { get; set; }

    public string? Industry { get; set; }

    public string? Location { get; set; }
}
