using Microsoft.AspNetCore.Mvc;

namespace AFJOB_WEB.Controllers
{
    public class JobseekerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
