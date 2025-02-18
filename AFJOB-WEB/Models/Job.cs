using System;
using System.Collections.Generic;

namespace AFJOB_WEB.Models;

public partial class Job
{
    public int JobId { get; set; }

    public int? EmployerId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? Location { get; set; }

    public decimal? Salary { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? ExpiryDate { get; set; }
}
