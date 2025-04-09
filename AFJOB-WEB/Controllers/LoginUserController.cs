using AFJOB_WEB.Models;
using AFJOB_WEB.Models.ViewModels;
using AFJOB_WEB.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace AFJOB_WEB.Controllers
{
    public class LoginUserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly AfjobWebContext _context;

        public LoginUserController(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration, IEmailService emailService, AfjobWebContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _emailService = emailService;
            _context = context;
        }

        // GET: Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginUser model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }

            // ✅ Role-based redirection
            if (await _userManager.IsInRoleAsync(user, "Recruiter"))
            {
                return RedirectToAction("Index", "Recruiter");
            }

            if (await _userManager.IsInRoleAsync(user, "JobSeeker"))
            {
                // 🔹 Inject your database context (AfjobWebContext) at the top of your controller!
                // Check if the job seeker has a profile:
                var hasProfile = await _context.JobSeekerProfiles
                    .AnyAsync(p => p.Email == user.Email);

                if (!hasProfile)
                {
                    TempData["Info"] = "Please complete your profile to start applying for jobs!";
                    return RedirectToAction("CreateProfile", "JobSeeker");
                }

                // If they have a profile, proceed to job listings/dashboard
                return RedirectToAction("Index", "JobSeeker");
            }

            // If user has no role, log out and show error
            await _signInManager.SignOutAsync();
            ModelState.AddModelError(string.Empty, "You do not have a valid role assigned.");
            return View(model);
        }

        // LOGOUT
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        // Optional: Forgot Password, Reset Password, etc.
    }
}
