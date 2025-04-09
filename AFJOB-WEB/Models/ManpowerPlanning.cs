using System.ComponentModel.DataAnnotations;

namespace AFJOB_WEB.Models
{
    public class ManpowerPlanning
    {
        [Key]
        public int ManpowerId { get; set; }

        [Required(ErrorMessage = "Position is required")]
        public string Position { get; set; }

        [Required(ErrorMessage = "Department is required")]
        public string Department { get; set; }

        [Required(ErrorMessage = "Current Headcount is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Headcount must be positive")]
        public int CurrentHeadcount { get; set; }

        [Required(ErrorMessage = "Required Headcount is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Headcount must be positive")]
        public int RequiredHeadcount { get; set; }

        [Required(ErrorMessage = "Forecasted Growth is required")]
        [Range(0, 100, ErrorMessage = "Growth must be between 0 and 100%")]
        public double ForecastedGrowth { get; set; }
    }
}
