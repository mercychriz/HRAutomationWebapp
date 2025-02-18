using AFJOB_WEB.Models;
using AFJOB_WEB.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace AFJOB_WEB.Controllers
{
    public class UserController : Controller
    {
        private readonly AfjobWebContext _context;

        public UserController(AfjobWebContext context)
        {
            _context = context;
        }

        // GET: User/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: User/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(UserRegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); // Return view with errors
            }

            // Check if the email is already in use
            if (_context.Users.Any(u => u.Email == model.Email))
            {
                ModelState.AddModelError("Email", "This email is already registered.");
                return View(model);
            }

            // Hash password
            var passwordHasher = new PasswordHasher<User>();
            string hashedPassword = passwordHasher.HashPassword(null, model.Password);

            // Create user object
            var newUser = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PasswordHash = hashedPassword,
                RoleId = model.RoleId,
                CreatedAt = DateTime.UtcNow
            };

            // Save to database
            _context.Users.Add(newUser);
            _context.SaveChanges();

            // Redirect based on role
            return model.RoleId == 1 ? RedirectToAction("Index", "JobSeeker") : RedirectToAction("Index", "Recruiter");
        }

        // GET: User/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: User/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View();
            }

            // Verify password
            var passwordHasher = new PasswordHasher<User>();
            var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);

            if (result != PasswordVerificationResult.Success)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View();
            }

            // TODO: Implement session or authentication logic here

            return RedirectToAction("Dashboard", "Home");
        }
    }
}
