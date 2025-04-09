using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AFJOB_WEB.Models
{
    public class Interview
    {
        [Key]
        public int InterviewId { get; set; }

        public int ApplicationId { get; set; }

        [ForeignKey("ApplicationId")]
        public ApplicationTable Application { get; set; }

        [Required]
        public DateTime InterviewDate { get; set; }

        [Required]
        public string InterviewType { get; set; }

        public string Notes { get; set; }

        public string InterviewStatus { get; set; } = "Scheduled";
    }
}
