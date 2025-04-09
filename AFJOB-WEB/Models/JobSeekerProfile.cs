using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AFJOB_WEB.Models
{
    public class JobSeekerProfile
    {
        public int JobSeekerProfileId { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public string ResumeFile { get; set; }

        public string Visibility { get; set; } // Internal or External

        public string ProfileImagePath { get; set; }


        public ICollection<ApplicationTable> Applications { get; set; }
    }
}
