using System.ComponentModel.DataAnnotations;

namespace AFJOB_WEB.Models.ViewModels
{
    public class LoginUser
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; } // Optional: include in your form
    }
}
