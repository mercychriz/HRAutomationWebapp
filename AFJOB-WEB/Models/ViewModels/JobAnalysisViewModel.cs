namespace AFJOB_WEB.Models.ViewModels
{
    public class JobAnalysisViewModel
    {
        public int TotalJobs { get; set; }
        public int TotalApplications { get; set; }

        public List<ApplicationsPerJob> ApplicationsPerJob { get; set; }
    }

    public class ApplicationsPerJob
    {
        public string JobTitle { get; set; }
        public int ApplicationCount { get; set; }
    }
}
