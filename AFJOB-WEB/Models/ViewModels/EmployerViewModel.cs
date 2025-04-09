using System.ComponentModel.DataAnnotations;

namespace AFJOB_WEB.Models.ViewModels
{
    public class EmployerViewModel
    {
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
    }
}
