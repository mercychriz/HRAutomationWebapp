using System;
using System.Collections.Generic;

namespace AFJOB_WEB.Models;

public partial class Interview
{
    public int InterviewId { get; set; }

    public int? ApplicationId { get; set; }

    public DateTime? ScheduledDate { get; set; }

    public string? Status { get; set; }
}
