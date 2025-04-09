using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AFJOB_WEB.Models;
using AFJOB_WEB.Models.ViewModels;

// Alias to resolve ambiguity with InterviewViewModel
using InterviewVM = AFJOB_WEB.Models.ViewModels.InterviewViewModel;

namespace AFJOB_WEB.Controllers
{
    [Authorize(Roles = "Recruiter")]
    [Route("[controller]/[action]")] // This ensures the URL path maps to controller/action
    public class CandidateController : Controller
    {
        private readonly AfjobWebContext _context;
        private readonly UserManager<User> _userManager;

        public CandidateController(AfjobWebContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ✅ LIST CANDIDATES
        [HttpGet]
        public async Task<IActionResult> ListCandidate()
        {
            var recruiter = await _userManager.GetUserAsync(User);

            if (recruiter == null)
            {
                TempData["Error"] = "Recruiter not found!";
                return RedirectToAction("Login", "User");
            }

            var applications = await _context.ApplicationTables
                .Include(a => a.Job)
                .Include(a => a.User)
                .Include(a => a.JobSeekerProfile)
                .Where(a => a.Job.EmployerId == recruiter.Id)
                .Select(a => new JobApplicationViewModel
                {
                    ApplicationId = a.ApplicationId,
                    JobTitle = a.Job.Title,
                    CandidateName = $"{a.User.FirstName} {a.User.LastName}",
                    CandidateEmail = a.User.Email,
                    ResumePath = a.JobSeekerProfile.ResumeFile,
  


                    ApplicationDate = a.ApplicationDate,
                    Status = a.Status
                })
                .ToListAsync();

            return View(applications);
        }

        // ✅ GET: SCHEDULE INTERVIEW
        [HttpGet]
        public async Task<IActionResult> ScheduleInterview(int applicationId)
        {
            var application = await _context.ApplicationTables
                .Include(a => a.User)
                .Include(a => a.Job)
                .FirstOrDefaultAsync(a => a.ApplicationId == applicationId);

            if (application == null)
            {
                TempData["Error"] = "Application not found.";
                return RedirectToAction(nameof(ListCandidate));
            }

            var model = new InterviewVM
            {
                ApplicationId = application.ApplicationId,
                CandidateName = $"{application.User.FirstName} {application.User.LastName}",
                JobTitle = application.Job.Title,
                InterviewDate = DateTime.Now.AddDays(1)
            };

            return View(model);
        }

        // ✅ POST: SCHEDULE INTERVIEW
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ScheduleInterview(InterviewVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please fix the errors before submitting!";
                return View(model);
            }

            var interview = new Interview
            {
                ApplicationId = model.ApplicationId,
                InterviewDate = model.InterviewDate,
                InterviewType = model.InterviewType,
                Notes = model.Notes,
                InterviewStatus = "Scheduled"
            };

            _context.Interviews.Add(interview);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Interview scheduled successfully!";
            return RedirectToAction(nameof(ListCandidate));
        }
        [HttpGet]
        public async Task<IActionResult> MakeOffer(int applicationId)
        {
            var application = await _context.ApplicationTables
                .Include(a => a.User)
                .Include(a => a.Job)
                .FirstOrDefaultAsync(a => a.ApplicationId == applicationId);

            if (application == null)
                return NotFound();

            var model = new OfferViewModel
            {
                ApplicationId = applicationId,
                OfferDetails = $"Dear {application.User.FirstName}, we are pleased to offer you the position of {application.Job.Title}."
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MakeOffer(OfferViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var application = await _context.ApplicationTables.FindAsync(model.ApplicationId);
            if (application == null)
                return NotFound();

            application.IsSelected = true;
            application.OfferDetails = model.OfferDetails;
            application.OfferDate = model.OfferDate;
            application.Status = "Offered";

            _context.Update(application);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Offer made successfully!";
            return RedirectToAction(nameof(ListCandidate));
        }

    }
}
