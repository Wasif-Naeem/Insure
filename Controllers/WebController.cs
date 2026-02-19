using Microsoft.AspNetCore.Mvc;

namespace Health_Insurance.Controllers
{
    public class WebController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Service()
        {
            return View();
        }
        public IActionResult Feature()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }

    }
}
