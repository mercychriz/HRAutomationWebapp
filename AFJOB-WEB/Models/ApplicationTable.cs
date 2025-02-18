using System;
using System.Collections.Generic;

namespace AFJOB_WEB.Models;

public partial class ApplicationTable
{
    public int ApplicationId { get; set; }

    public int? UserId { get; set; }

    public int? JobId { get; set; }

    public string? Status { get; set; }

    public DateTime? AppliedAt { get; set; }
}
