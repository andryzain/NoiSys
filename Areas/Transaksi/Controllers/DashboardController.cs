using Microsoft.AspNetCore.Mvc;

namespace NoiSys.Areas.Transaksi.Controllers
{
    [Area("Transaksi")]
    [Route("Transaksi/[Controller]/[Action]")]
    //[Authorize(Roles = "Transaksi")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
