namespace AFJOB_WEB.Models.ViewModels
{
    public class JobSeekerApplicationViewModel
    {
        public int ApplicationId { get; set; }
        public string JobTitle { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string Status { get; set; }
        public string? OfferDetails { get; set; }
        public DateTime? OfferDate { get; set; }
    }

}
