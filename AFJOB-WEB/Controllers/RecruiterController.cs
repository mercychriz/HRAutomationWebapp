using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AFJOB_WEB.Models;
using Microsoft.EntityFrameworkCore;
using AFJOB_WEB.Models.ViewModels;

namespace AFJOB_WEB.Controllers
{
    [Authorize(Roles = "Recruiter")]
    public class RecruiterController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly AfjobWebContext _context;

        public RecruiterController(UserManager<User> userManager, AfjobWebContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // Recruiter Landing Page (After Login)
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "User");

            var employer = await _context.Employers.FirstOrDefaultAsync(e => e.UserId == user.Id);

            if (employer == null)
                return RedirectToAction("CreateEmployerProfile");

            ViewBag.FirstName = user.FirstName;

            ViewBag.CompanyName = employer.CompanyName;
            ViewBag.Industry = employer.Industry;
            ViewBag.Location = employer.Location;

            return View();
        }

        // Recruiter Dashboard
        public async Task<IActionResult> Dashboard()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "User");

            var employer = await _context.Employers.FirstOrDefaultAsync(e => e.UserId == user.Id);
            if (employer == null)
                return RedirectToAction("CreateEmployerProfile");

         

            // Basic Stats
            ViewBag.TotalJobs = await _context.Jobs.CountAsync(j => j.EmployerId == user.Id);
            ViewBag.TotalApplications = await _context.ApplicationTables
                .Include(a => a.Job)
                .Where(a => a.Job.EmployerId == user.Id)
                .CountAsync();
            ViewBag.TotalInterviews = await _context.Interviews
                .Include(i => i.Application)
                .ThenInclude(a => a.Job)
                .Where(i => i.Application.Job.EmployerId == user.Id)
                .CountAsync();

            // Chart 1: Applications Over Time (Line Chart)
            ViewBag.Months = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun" };
            ViewBag.ApplicationCounts = new[] { 10, 20, 15, 25, 30, 22 };

            // Chart 2: Jobs Per Department (Bar Chart)
            var departments = await _context.Jobs
                .Where(j => j.EmployerId == user.Id)
                .GroupBy(j => j.Location) // Treating Location as Department for simplicity
                .Select(g => new { Department = g.Key, Count = g.Count() })
                .ToListAsync();

            ViewBag.Departments = departments.Select(d => d.Department).ToList();
            ViewBag.JobCounts = departments.Select(d => d.Count).ToList();

            // Chart 3: Applications by Status (Pie Chart)
            var statuses = await _context.ApplicationTables
                .Where(a => a.Job.EmployerId == user.Id)
                .GroupBy(a => a.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToListAsync();

            ViewBag.Statuses = statuses.Select(s => s.Status).ToList();
            ViewBag.StatusCounts = statuses.Select(s => s.Count).ToList();

            // Chart 4: Applications by Location (Doughnut Chart)
            var locations = await _context.Jobs
                .Where(j => j.EmployerId == user.Id)
                .GroupBy(j => j.Location)
                .Select(g => new { Location = g.Key, Count = g.Sum(j => _context.ApplicationTables.Count(a => a.JobId == j.JobId)) })
                .ToListAsync();

            ViewBag.Locations = locations.Select(l => l.Location).ToList();
            ViewBag.LocationCounts = locations.Select(l => l.Count).ToList();

            // Chart 5: Salaries per Job (Horizontal Bar Chart)
            var salaries = await _context.Jobs
                .Where(j => j.EmployerId == user.Id)
                .Select(j => new { j.Title, j.Salary })
                .ToListAsync();

            ViewBag.JobTitles = salaries.Select(s => s.Title).ToList();
            ViewBag.Salaries = salaries.Select(s => s.Salary).ToList();

            return View();
        }

        // ✅ View Employer Profile (New!)
        [HttpGet]
        public async Task<IActionResult> EmployerProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "User");

            var employer = await _context.Employers.FirstOrDefaultAsync(e => e.UserId == user.Id);
            if (employer == null)
                return RedirectToAction("CreateEmployerProfile");

            return View(employer);
        }

        // Create Employer Profile - GET
        [HttpGet]
        public async Task<IActionResult> CreateEmployerProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "User");

            var exists = await _context.Employers.AnyAsync(e => e.UserId == user.Id);
            if (exists)
                return RedirectToAction("EditEmployerProfile");

            return View(new EmployerViewModel());
        }


        // Create Employer Profile - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEmployerProfile(EmployerViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "User");

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please fix the validation errors.";
                return View(model);
            }

            var employer = new Employer
            {
                CompanyName = model.CompanyName,
                CompanyWebsite = model.CompanyWebsite,
                Industry = model.Industry,
                Location = model.Location,
                Description = model.Description,
                UserId = user.Id
            };

            _context.Employers.Add(employer);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Employer profile created successfully!";
            return RedirectToAction("Index");
        }




        // Edit Employer Profile - GET
        [HttpGet]
        public async Task<IActionResult> EditEmployerProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "User");

            var employer = await _context.Employers.FirstOrDefaultAsync(e => e.UserId == user.Id);
            if (employer == null)
                return RedirectToAction("CreateEmployerProfile");

            return View(employer);
        }

        // Edit Employer Profile - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEmployerProfile(Employer employer)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "User");

            var existingEmployer = await _context.Employers.FirstOrDefaultAsync(e => e.UserId == user.Id);
            if (existingEmployer == null)
                return NotFound();

            if (!ModelState.IsValid)
                return View(employer);

            existingEmployer.CompanyName = employer.CompanyName;
            existingEmployer.CompanyWebsite = employer.CompanyWebsite;
            existingEmployer.Industry = employer.Industry;
            existingEmployer.Location = employer.Location;
            existingEmployer.Description = employer.Description;

            _context.Update(existingEmployer);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Profile updated successfully!";
            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteEmployerProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "User");

            var employer = await _context.Employers.FirstOrDefaultAsync(e => e.UserId == user.Id);
            if (employer != null)
            {
                _context.Employers.Remove(employer);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Employer profile deleted successfully!";
            }

            return RedirectToAction("Index");
        }
    }
}
