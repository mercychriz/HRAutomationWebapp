using AFJOB_WEB.Models;

namespace AFJOB_WEB.Models.ViewModels
{
    public class JobSeekerDashboardViewModel
    {
        public JobSeekerProfile Profile { get; set; }
        public List<Job> Jobs { get; set; }
    }
}
