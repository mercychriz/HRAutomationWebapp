
using AFJOB_WEB.Models;
using Microsoft.AspNetCore.Mvc;

namespace AFJOB_WEB.Controllers
{
    public class ManpowerPlanningController : Controller
    {
        private readonly AfjobWebContext _context;

        public ManpowerPlanningController(AfjobWebContext context)
        {
            _context = context;
        }

        // GET: /ManpowerPlanning
        public IActionResult Index()
        {
            var plans = _context.ManpowerPlannings.ToList();
            return View(plans);
        }

        // GET: /ManpowerPlanning/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View(new ManpowerPlanning());
        }

        // POST: /ManpowerPlanning/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ManpowerPlanning manpowerPlanning)
        {
            if (!ModelState.IsValid)
            {
                return View(manpowerPlanning);
            }

            _context.ManpowerPlannings.Add(manpowerPlanning);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }

}
