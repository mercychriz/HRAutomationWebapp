using System.ComponentModel.DataAnnotations;

namespace AFJOB_WEB.Models
{
    public class JobAnalysis
    {
        [Key]
        public int JobAnalysisId { get; set; }
        public int JobId { get; set; }  // Foreign Key to Job
        public string Duties { get; set; }
        public string Responsibilities { get; set; }
        public string RequiredQualifications { get; set; }
        public string SkillsRequired { get; set; }

        public Job Job { get; set; } // Navigation property to the Job
    }
}
