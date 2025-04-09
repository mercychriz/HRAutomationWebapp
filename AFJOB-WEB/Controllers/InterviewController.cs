using AFJOB_WEB.Models;
using AFJOB_WEB.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AFJOB_WEB.Controllers
{
    public class InterviewController : Controller
    {
        private readonly AfjobWebContext _context;

        public InterviewController(AfjobWebContext context)
        {
            _context = context;
        }

        // GET: Interview/Index
        public IActionResult Index()
        {
            var interviews = _context.Interviews
                .Include(i => i.Application)
                    .ThenInclude(a => a.User)
                .Include(i => i.Application)
                    .ThenInclude(a => a.Job)
                .Select(i => new InterviewViewModel
                {
                    InterviewId = i.InterviewId,
                    ApplicationId = i.ApplicationId,
                    CandidateName = $"{i.Application.User.FirstName} {i.Application.User.LastName}",
                    CandidateEmail = i.Application.User.Email,
                    JobTitle = i.Application.Job.Title,
                    InterviewDate = i.InterviewDate,
                    InterviewType = i.InterviewType,
                    Notes = i.Notes,
                    InterviewStatus = i.InterviewStatus
                }).ToList();

            return View(interviews);
        }

       
        [HttpGet]
        public IActionResult Details(int interviewId)
        {
            var interview = _context.Interviews
                .Include(i => i.Application)
                    .ThenInclude(a => a.User)
                .Include(i => i.Application)
                    .ThenInclude(a => a.Job)
                .FirstOrDefault(i => i.InterviewId == interviewId);

            if (interview == null)
            {
                return NotFound();
            }

            var interviewVM = new InterviewViewModel
            {
                InterviewId = interview.InterviewId,
                ApplicationId = interview.ApplicationId,
                CandidateName = $"{interview.Application.User.FirstName} {interview.Application.User.LastName}",
                CandidateEmail = interview.Application.User.Email,
                JobTitle = interview.Application.Job.Title,
                InterviewDate = interview.InterviewDate,
                InterviewType = interview.InterviewType,
                Notes = interview.Notes,
                InterviewStatus = interview.InterviewStatus
            };

            return View(interviewVM);
        }



   
        // GET: Interview/Edit/{interviewId}
        [HttpGet]
        public IActionResult Edit(int interviewId)
        {
            var interview = _context.Interviews
                .Include(i => i.Application)
                    .ThenInclude(a => a.User)
                .Include(i => i.Application)
                    .ThenInclude(a => a.Job)
                .FirstOrDefault(i => i.InterviewId == interviewId);

            if (interview == null)
            {
                return NotFound();
            }

            var interviewVM = new InterviewViewModel
            {
                InterviewId = interview.InterviewId,
                ApplicationId = interview.ApplicationId,
                CandidateName = $"{interview.Application.User.FirstName} {interview.Application.User.LastName}",
                CandidateEmail = interview.Application.User.Email,
                JobTitle = interview.Application.Job.Title,
                InterviewDate = interview.InterviewDate,
                InterviewType = interview.InterviewType,
                Notes = interview.Notes,
                InterviewStatus = interview.InterviewStatus
            };

            return View(interviewVM);
        }

        // POST: Interview/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(InterviewViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var interview = _context.Interviews.FirstOrDefault(i => i.InterviewId == model.InterviewId);

            if (interview == null)
            {
                return NotFound();
            }

            // ✅ Update properties
            interview.InterviewDate = model.InterviewDate;
            interview.InterviewType = model.InterviewType;
            interview.Notes = model.Notes;
            interview.InterviewStatus = model.InterviewStatus;

            _context.Interviews.Update(interview);
            _context.SaveChanges();

            TempData["Success"] = "Interview updated successfully!";
            return RedirectToAction("Index");
        }



        // GET: Interview/Create/{applicationId}
        [HttpGet]
        public IActionResult Create(int applicationId)
        {
            var application = _context.ApplicationTables
                .Include(a => a.User)
                .Include(a => a.Job)
                .FirstOrDefault(a => a.ApplicationId == applicationId);

            if (application == null)
            {
                return NotFound();
            }

            var interviewVM = new InterviewViewModel
            {
                ApplicationId = application.ApplicationId,
                CandidateName = $"{application.User.FirstName} {application.User.LastName}",
                CandidateEmail = application.User.Email,
                JobTitle = application.Job.Title,
                InterviewDate = DateTime.Now // Pre-fill valid date
            };

            return View(interviewVM);
        }

        // POST: Interview/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(InterviewViewModel model)
        {
            if (!ModelState.IsValid)
            {
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
            _context.SaveChanges();

            TempData["Success"] = "Interview scheduled successfully!";
            return RedirectToAction("Index");
        }

    }
}
