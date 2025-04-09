namespace AFJOB_WEB.Models
{
    public class ResetViewModel
    {
        public string Email { get; set; } // ✅ Add this line
        public string Token { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
