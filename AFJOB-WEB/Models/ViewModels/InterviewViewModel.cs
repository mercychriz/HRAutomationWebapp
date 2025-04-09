using System;
using System.ComponentModel.DataAnnotations;

namespace AFJOB_WEB.Models.ViewModels
{
    public class InterviewViewModel
    {
        public int InterviewId { get; set; }

        public int ApplicationId { get; set; }

        // Read-only candidate info
        public string CandidateName { get; set; }
        public string CandidateEmail { get; set; }
        public string JobTitle { get; set; }

        // Form inputs
        [Required(ErrorMessage = "Interview Date is required.")]
        public DateTime InterviewDate { get; set; }

        [Required(ErrorMessage = "Interview Type is required.")]
        public string InterviewType { get; set; }

        public string Notes { get; set; }

        public string InterviewStatus { get; set; } = "Scheduled";
    }
}
