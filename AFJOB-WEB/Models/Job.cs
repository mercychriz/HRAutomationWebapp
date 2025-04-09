using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AFJOB_WEB.Models
{
    public class Job
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-increment
        public int JobId { get; set; }

        [Required]
        [StringLength(450)] // Matches Identity GUID of the recruiter!
        public string EmployerId { get; set; }

        [Required]
        public string Visibility { get; set; } // "Internal" or "External"

        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [StringLength(100)]
        public string Location { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Salary { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? ExpiryDate { get; set; }

        // Optional navigation properties
        public JobAnalysis JobAnalysis { get; set; }
        public JobDescription JobDescription { get; set; }
    }
}
