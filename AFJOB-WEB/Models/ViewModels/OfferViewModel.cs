using System.ComponentModel.DataAnnotations;

namespace AFJOB_WEB.Models.ViewModels
{
    public class OfferViewModel
    {
        public int ApplicationId { get; set; }

        [Required(ErrorMessage = "Offer details are required.")]
        public string OfferDetails { get; set; }

        public DateTime OfferDate { get; set; } = DateTime.Now;
    }
}
