using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AFJOB_WEB.Models
{
    public class ApplicationTable
    {
        [Key]
        public int ApplicationId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int JobId { get; set; }

        public DateTime ApplicationDate { get; set; }

        public string Status { get; set; } = "Pending";

        public int? JobSeekerProfileId { get; set; }

        public bool IsSelected { get; set; } = false;
        public DateTime? OfferDate { get; set; }
        public string OfferDetails { get; set; }

        // Navigation properties
        public User User { get; set; }
        public Job Job { get; set; }

        // ✅ Navigation to interviews
        public ICollection<Interview> Interviews { get; set; }
        public JobSeekerProfile JobSeekerProfile { get; set; }
    }
}
