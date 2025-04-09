using System.ComponentModel.DataAnnotations;

namespace AFJOB_WEB.Models.ViewModels
{
    public class JobViewModel
    {
        public int JobId { get; set; }

        [Required(ErrorMessage = "Job title is required.")]
        [StringLength(100)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        [StringLength(100)]
        public string Location { get; set; }

        [Required(ErrorMessage = "Salary is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Salary must be a positive number.")]
        public decimal Salary { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ExpiryDate { get; set; }

        [Required(ErrorMessage = "Visibility is required.")]
        public string Visibility { get; set; }
    }
}
