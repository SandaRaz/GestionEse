using Microsoft.AspNetCore.Mvc;

namespace SIProject.Controllers.Entreprises
{
    public class ModuleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Commerce()
        {
            return View();
        }
    }
}
