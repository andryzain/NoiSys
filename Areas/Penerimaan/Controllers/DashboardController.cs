using Microsoft.AspNetCore.Mvc;

namespace NoiSys.Areas.Penerimaan.Controllers
{
    [Area("Penerimaan")]
    [Route("Penerimaan/[Controller]/[Action]")]
    //[Authorize(Roles = "Penerimaan")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
