using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace AFJOB_WEB.Models.ViewModels
{
    public class JobSeekerProfileViewModel
    {
        public int JobSeekerProfileId { get; set; }

        [Required(ErrorMessage = "Full name is required.")]
        public string FullName { get; set; }

        // Email can be populated from the logged-in user, so you don't need to have it on the form.
        public string? Email { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        [Required(ErrorMessage = "Please select visibility.")]
        public string Visibility { get; set; }

        public IFormFile? ResumeFile { get; set; }

        public IFormFile? ProfileImage { get; set; }


        // No need to make this required, it's a display field!
        public string? ExistingResumePath { get; set; }

        public string? ExistingProfileImagePath { get; set; }
    }
}
