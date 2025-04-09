using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AFJOB_WEB.Models.ViewModels
{
    public class JobApplicationViewModel
    {
        public int JobId { get; set; }
        public int ApplicationId { get; set; }

        public string? JobTitle { get; set; }
        public string Status { get; set; } = "Pending";

        [Required(ErrorMessage = "Candidate name is required")]
        public string CandidateName { get; set; }

        [Required(ErrorMessage = "Candidate email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string CandidateEmail { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        [Display(Name = "Resume Path")]
        public string? ResumePath { get; set; } // ✅ use this to show/download resume

        [NotMapped]
        [Required(ErrorMessage = "Please upload your resume")]
        public IFormFile? ResumeFile { get; set; } // ✅ only for uploading

        public DateTime ApplicationDate { get; set; }
    }
}
