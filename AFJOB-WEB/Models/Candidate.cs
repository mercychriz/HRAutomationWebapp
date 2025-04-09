using System;
using System.ComponentModel.DataAnnotations;

namespace AFJOB_WEB.Models
{
    public class Candidate
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Display(Name = "Resume File Path")]
        public string ResumeFile { get; set; }

        [Required(ErrorMessage = "Application source is required")]
        [StringLength(50, ErrorMessage = "Application source cannot exceed 50 characters")]
        public string ApplicationSource { get; set; } // E.g., Job Board, Referral

        [Display(Name = "Applied Date")]
        public DateTime AppliedDate { get; set; } = DateTime.Now;

        [Display(Name = "Phone Number")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; }

        [StringLength(200, ErrorMessage = "Notes cannot exceed 200 characters")]
        public string Notes { get; set; } 
    }
}

