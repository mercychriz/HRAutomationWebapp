using AFJOB_WEB.Models;
using AFJOB_WEB.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AFJOB_WEB.Controllers
{
    [Authorize(Roles = "Recruiter")]
    public class JobController : Controller
    {
        private readonly AfjobWebContext _context;
        private readonly UserManager<User> _userManager;

        public JobController(AfjobWebContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ✅ List Jobs for Current Recruiter
        public async Task<IActionResult> List()
        {
            var user = await _userManager.GetUserAsync(User);

            var employer = await _context.Employers.FirstOrDefaultAsync(e => e.UserId == user.Id);

            if (employer == null)
            {
                TempData["Error"] = "You need to create your employer profile before listing jobs.";
                return RedirectToAction("CreateEmployerProfile", "Recruiter");
            }

            var jobs = await _context.Jobs
                .Where(j => j.EmployerId == user.Id)
                .ToListAsync();

            return View(jobs);
        }

        // ✅ GET: Create Job
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(User);
            var employerProfile = await _context.Employers.FirstOrDefaultAsync(e => e.UserId == user.Id);

            if (employerProfile == null)
            {
                TempData["Error"] = "You need to create your employer profile before posting a job.";
                return RedirectToAction("CreateEmployerProfile", "Recruiter");
            }

            ViewBag.CompanyName = employerProfile.CompanyName;
            return View(new JobViewModel());
        }

        // ✅ POST: Create Job
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(JobViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            var employerProfile = await _context.Employers.FirstOrDefaultAsync(e => e.UserId == user.Id);

            if (employerProfile == null)
            {
                TempData["Error"] = "You need to create your employer profile before posting a job.";
                return RedirectToAction("CreateEmployerProfile", "Recruiter");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.CompanyName = employerProfile.CompanyName;
                return View(model);
            }

            var job = new Job
            {
                Title = model.Title,
                Description = model.Description,
                Location = model.Location,
                Salary = model.Salary,
                ExpiryDate = model.ExpiryDate,
                EmployerId = user.Id,
                CreatedAt = DateTime.Now,
                Visibility = model.Visibility // ✅ FIXED
            };

            _context.Jobs.Add(job);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Job posted successfully!";
            return RedirectToAction("List");
        }

        // ✅ GET: Edit Job
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var job = await _context.Jobs.FirstOrDefaultAsync(j => j.JobId == id && j.EmployerId == user.Id);

            if (job == null)
            {
                TempData["Error"] = "Job not found or access denied.";
                return RedirectToAction("List");
            }

            var model = new JobViewModel
            {
                JobId = job.JobId,
                Title = job.Title,
                Description = job.Description,
                Location = job.Location,
                Salary = job.Salary,
                ExpiryDate = job.ExpiryDate,
                Visibility = job.Visibility // ✅ FIXED
            };

            return View(model);
        }

        // ✅ POST: Edit Job
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, JobViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            var job = await _context.Jobs.FirstOrDefaultAsync(j => j.JobId == id && j.EmployerId == user.Id);

            if (job == null)
            {
                TempData["Error"] = "Job not found or access denied.";
                return RedirectToAction("List");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            job.Title = model.Title;
            job.Description = model.Description;
            job.Location = model.Location;
            job.Salary = model.Salary;
            job.ExpiryDate = model.ExpiryDate;
            job.Visibility = model.Visibility; // ✅ FIXED

            _context.Update(job);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Job updated successfully!";
            return RedirectToAction("List");
        }

        // ✅ POST: Delete Job
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var job = await _context.Jobs.FirstOrDefaultAsync(j => j.JobId == id && j.EmployerId == user.Id);

            if (job == null)
            {
                TempData["Error"] = "Job not found or access denied.";
                return RedirectToAction("List");
            }

            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Job deleted successfully!";
            return RedirectToAction("List");
        }

        // ✅ NEW: Job Analysis
        [HttpGet]
        public async Task<IActionResult> JobAnalysis()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToAction("Login", "User");

            var jobs = await _context.Jobs
                .Where(j => j.EmployerId == user.Id)
                .ToListAsync();

            var jobApplications = await _context.ApplicationTables
                .Include(a => a.Job)
                .Where(a => a.Job.EmployerId == user.Id)
                .ToListAsync();

            var totalJobs = jobs.Count;
            var totalApplications = jobApplications.Count;

            var applicationsPerJob = jobs.Select(job => new ApplicationsPerJob
            {
                JobTitle = job.Title,
                ApplicationCount = jobApplications.Count(a => a.JobId == job.JobId)
            }).ToList();

            var model = new JobAnalysisViewModel
            {
                TotalJobs = totalJobs,
                TotalApplications = totalApplications,
                ApplicationsPerJob = applicationsPerJob
            };

            return View(model);
        }
    }
}
