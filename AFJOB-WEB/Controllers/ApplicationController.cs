using Microsoft.AspNetCore.Mvc;

namespace AFJOB_WEB.Controllers
{
    public class ApplicationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
