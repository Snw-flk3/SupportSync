using Microsoft.AspNetCore.Mvc;

namespace HopeWorldWide.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
