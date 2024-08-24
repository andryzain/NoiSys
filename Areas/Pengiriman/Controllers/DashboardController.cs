using Microsoft.AspNetCore.Mvc;

namespace NoiSys.Areas.Pengiriman.Controllers
{
    [Area("Pengiriman")]
    [Route("Pengiriman/[Controller]/[Action]")]
    //[Authorize(Roles = "Pengiriman")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
