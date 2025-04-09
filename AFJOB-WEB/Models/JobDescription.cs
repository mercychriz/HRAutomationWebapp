namespace AFJOB_WEB.Models
{
    public class JobDescription
    {
        public int JobDescriptionId { get; set; }
        public int JobId { get; set; }  // Foreign Key to Job
        public string JobTitle { get; set; }
        public string Department { get; set; }
        public string JobSummary { get; set; }
        public string Qualifications { get; set; }
        public string Skills { get; set; }

        public Job Job { get; set; } 
    }
}
