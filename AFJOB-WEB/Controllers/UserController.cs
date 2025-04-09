using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AFJOB_WEB.Models;
using AFJOB_WEB.Models.ViewModels;
using System;
using System.Threading.Tasks;

namespace AFJOB_WEB.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: Register
        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterViewModel model)
        {
            Console.WriteLine("POST Register hit");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("Invalid ModelState");
                // This will be returned if validation fails.
                return View(model);
            }

            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                RoleID = model.RoleId,
                CreatedAt = DateTime.Now
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                Console.WriteLine("Failed to create user");
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"CreateAsync Error: {error.Description}");
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model); // shows errors in the view if you added the validation summary
            }

            var roleName = model.RoleId == 1 ? "JobSeeker" : "Recruiter";
            var roleResult = await _userManager.AddToRoleAsync(user, roleName);

            if (!roleResult.Succeeded)
            {
                Console.WriteLine("Failed to add to role");
                foreach (var error in roleResult.Errors)
                {
                    Console.WriteLine($"Role Error: {error.Description}");
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            Console.WriteLine("Redirecting to Login...");
            return RedirectToAction("Login", "User");
        }



        // GET: Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUser model)
        {
            if (!ModelState.IsValid)
                return View(model);

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

            // ✅ Redirect based on role
            if (await _userManager.IsInRoleAsync(user, "Recruiter"))
            {
                return RedirectToAction("Index", "Recruiter");
            }
            else if (await _userManager.IsInRoleAsync(user, "JobSeeker"))
            {
                return RedirectToAction("Index", "JobSeeker");
            }

            // ❌ No valid role
            await _signInManager.SignOutAsync();
            ModelState.AddModelError("", "You do not have a valid role assigned.");
            return View(model);
        }


        // Logout
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "User");
        }
    }
}
