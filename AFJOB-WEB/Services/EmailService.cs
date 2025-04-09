using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace AFJOB_WEB.Services  // ✅ Update namespace to match your project structure
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendResetToken(string toEmail, string resetLink)
        {
            var fromEmail = _configuration["SenderEmail"];
            var fromPassword = _configuration["SenderPassword"];

            var subject = "Password Reset Request";
            var body = $"Click the link to reset your password: {resetLink}";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                Credentials = new NetworkCredential(fromEmail, fromPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true // Ensure HTML support if needed
            })
            {
                // Async method to send the email
                await smtp.SendMailAsync(message);
            }
        }
    }
    public interface IEmailService
    {
        Task SendResetToken(string email, string resetLink);
    }


}
