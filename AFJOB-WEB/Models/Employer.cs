using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;           // Add this!
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation; // Add this!

namespace AFJOB_WEB.Models
{
    public class Employer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployerId { get; set; }

        [Required]
        [StringLength(450)]
        [BindNever] // ✅ Prevent binding from form input
        public string UserId { get; set; }

        [Required(ErrorMessage = "Company Name is required.")]
        [StringLength(100)]
        public string CompanyName { get; set; }

        [Url(ErrorMessage = "Please enter a valid URL.")]
        [StringLength(200)]
        public string? CompanyWebsite { get; set; }

        [Required(ErrorMessage = "Industry is required.")]
        [StringLength(50)]
        public string Industry { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        [StringLength(100)]
        public string Location { get; set; }

        [StringLength(250)]
        public string? Description { get; set; }

        [ForeignKey("UserId")]
        [ValidateNever] // ✅ Skip validation for this navigation property
        public User User { get; set; }
    }
}
