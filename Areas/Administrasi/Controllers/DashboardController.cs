using Microsoft.AspNetCore.Mvc;

namespace NoiSys.Areas.Administrasi.Controllers
{
    [Area("Administrasi")]
    [Route("Administrasi/[Controller]/[Action]")]
    //[Authorize(Roles = "Administrasi")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
