using Microsoft.AspNetCore.Mvc;

namespace NoiSys.Areas.Order.Controllers
{
    [Area("Order")]
    [Route("Order/[Controller]/[Action]")]
    //[Authorize(Roles = "Order")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
