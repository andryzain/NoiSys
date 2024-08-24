using Microsoft.AspNetCore.Mvc;

namespace NoiSys.Areas.Keuangan.Controllers
{
    [Area("Keuangan")]
    [Route("Keuangan/[Controller]/[Action]")]
    //[Authorize(Roles = "Keuangan")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
