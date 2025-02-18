using System;
using System.Collections.Generic;

namespace AFJOB_WEB.Models;

public partial class Resume
{
    public int ResumeId { get; set; }

    public int? UserId { get; set; }

    public string? FilePath { get; set; }

    public string? ParsedData { get; set; }
}
