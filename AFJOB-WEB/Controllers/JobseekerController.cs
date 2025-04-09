using AFJOB_WEB.Models;
using AFJOB_WEB.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AFJOB_WEB.Controllers
{
    [Authorize(Roles = "JobSeeker")]
    public class JobSeekerController : Controller
    {
        private readonly AfjobWebContext _context;
        private readonly UserManager<User> _userManager;

        public JobSeekerController(AfjobWebContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "LoginUser");

            var profile = await _context.JobSeekerProfiles.FirstOrDefaultAsync(p => p.Email == user.Email);
            if (profile == null) return RedirectToAction("CreateProfile");

            var jobs = await _context.Jobs
                .Where(j => j.Visibility == profile.Visibility)
                .OrderByDescending(j => j.CreatedAt)
                .ToListAsync();

            var viewModel = new JobSeekerDashboardViewModel
            {
                Profile = profile,
                Jobs = jobs
            };

            return View(viewModel);
        }
        [HttpGet]
        public async Task<IActionResult> MyApplications()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "LoginUser");

            var applications = await _context.ApplicationTables
                .Include(a => a.Job)
                .Where(a => a.UserId == user.Id)
                .Select(a => new JobSeekerApplicationViewModel
                {
                    ApplicationId = a.ApplicationId,
                    JobTitle = a.Job.Title,
                    ApplicationDate = a.ApplicationDate,
                    Status = a.Status,
                    OfferDetails = a.OfferDetails,
                    OfferDate = a.OfferDate
                })
                .ToListAsync();

            return View(applications);
        }

        [HttpGet]
        public async Task<IActionResult> Apply(int jobId)
        {
            var job = await _context.Jobs.FirstOrDefaultAsync(j => j.JobId == jobId);
            if (job == null)
            {
                TempData["Error"] = "Job not found.";
                return RedirectToAction("Index");
            }

            var user = await _userManager.GetUserAsync(User);
            var viewModel = new JobApplicationViewModel
            {
                JobId = job.JobId,
                JobTitle = job.Title,
                CandidateName = user.FirstName,
                CandidateEmail = user.Email,
                ApplicationDate = DateTime.Now
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Apply(JobApplicationViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Index");

            model.Status = "Pending";

            var job = await _context.Jobs.FirstOrDefaultAsync(j => j.JobId == model.JobId);
            if (job == null)
            {
                TempData["Error"] = "Job not found!";
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid || model.ResumeFile == null || model.ResumeFile.Length == 0)
            {
                TempData["Error"] = "Please fill all fields and upload resume.";
                return View(model);
            }

            var resumeFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "resumes");
            if (!Directory.Exists(resumeFolder)) Directory.CreateDirectory(resumeFolder);

            var resumeFileName = Guid.NewGuid() + Path.GetExtension(model.ResumeFile.FileName);
            var resumeFilePath = Path.Combine(resumeFolder, resumeFileName);

            using (var stream = new FileStream(resumeFilePath, FileMode.Create))
            {
                await model.ResumeFile.CopyToAsync(stream);
            }

            var application = new ApplicationTable
            {
                UserId = user.Id,
                JobId = model.JobId,
                ApplicationDate = DateTime.Now,
                Status = model.Status,
                OfferDetails = "" // Default value to prevent SQL null error
            };

            _context.ApplicationTables.Add(application);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Application submitted successfully!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult CreateProfile()
        {
            return View(new JobSeekerProfileViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProfile(JobSeekerProfileViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "LoginUser");

            var profile = new JobSeekerProfile
            {
                FullName = model.FullName,
                Email = user.Email,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                Visibility = model.Visibility
            };

            if (model.ResumeFile != null)
            {
                var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "resumes");
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                var fileName = Guid.NewGuid() + Path.GetExtension(model.ResumeFile.FileName);
                var filePath = Path.Combine(folder, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await model.ResumeFile.CopyToAsync(stream);

                profile.ResumeFile = "/resumes/" + fileName;
            }

            if (model.ProfileImage != null)
            {
                var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "profileimages");
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                var fileName = Guid.NewGuid() + Path.GetExtension(model.ProfileImage.FileName);
                var filePath = Path.Combine(folder, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await model.ProfileImage.CopyToAsync(stream);

                profile.ProfileImagePath = "/profileimages/" + fileName;
            }

            _context.JobSeekerProfiles.Add(profile);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Profile created successfully!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Index");

            var profile = await _context.JobSeekerProfiles.FirstOrDefaultAsync(p => p.Email == user.Email);
            if (profile == null) return RedirectToAction("Index");

            var model = new JobSeekerProfileViewModel
            {
                JobSeekerProfileId = profile.JobSeekerProfileId,
                FullName = profile.FullName,
                Email = profile.Email,
                PhoneNumber = profile.PhoneNumber,
                Address = profile.Address,
                Visibility = profile.Visibility,
                ExistingResumePath = profile.ResumeFile,
                ExistingProfileImagePath = profile.ProfileImagePath
            };

            return View(model);
        }

        // JobSeekerController.cs (Updated EditProfile POST Action)

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(JobSeekerProfileViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "LoginUser");

            var profile = await _context.JobSeekerProfiles.FirstOrDefaultAsync(p => p.Email == user.Email);
            if (profile == null) return RedirectToAction("Index");

            profile.FullName = model.FullName;
            profile.Email = user.Email; // force update with logged-in email
            profile.PhoneNumber = model.PhoneNumber;
            profile.Address = model.Address;
            profile.Visibility = model.Visibility;

            // Resume upload
            if (model.ResumeFile != null && model.ResumeFile.Length > 0)
            {
                var resumeFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "resumes");
                Directory.CreateDirectory(resumeFolder);

                var resumeFileName = Guid.NewGuid() + Path.GetExtension(model.ResumeFile.FileName);
                var resumePath = Path.Combine(resumeFolder, resumeFileName);

                using var resumeStream = new FileStream(resumePath, FileMode.Create);
                await model.ResumeFile.CopyToAsync(resumeStream);

                profile.ResumeFile = "/resumes/" + resumeFileName;
            }

            // Profile image upload
            if (model.ProfileImage != null && model.ProfileImage.Length > 0)
            {
                var imageFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "profileimages");
                Directory.CreateDirectory(imageFolder);

                var imageFileName = Guid.NewGuid() + Path.GetExtension(model.ProfileImage.FileName);
                var imagePath = Path.Combine(imageFolder, imageFileName);

                using var imageStream = new FileStream(imagePath, FileMode.Create);
                await model.ProfileImage.CopyToAsync(imageStream);

                profile.ProfileImagePath = "/profileimages/" + imageFileName;
            }

            _context.JobSeekerProfiles.Update(profile);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Profile updated successfully!";
            return RedirectToAction("Index");
        }



        [HttpGet]
        public async Task<IActionResult> ProfileDetails()
        {
            var user = await _userManager.GetUserAsync(User);
            var profile = await _context.JobSeekerProfiles.FirstOrDefaultAsync(p => p.Email == user.Email);
            if (profile == null) return NotFound();

            return View(profile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            var profile = await _context.JobSeekerProfiles.FirstOrDefaultAsync(p => p.Email == user.Email);
            if (profile == null)
            {
                TempData["Error"] = "Profile not found.";
                return RedirectToAction("Index");
            }

            _context.JobSeekerProfiles.Remove(profile);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Profile deleted successfully!";
            return RedirectToAction("Logout", "Account");
        }
    }
}
