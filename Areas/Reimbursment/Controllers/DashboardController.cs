using Microsoft.AspNetCore.Mvc;

namespace NoiSys.Areas.Reimbursment.Controllers
{
    [Area("Reimbursment")]
    [Route("Reimbursment/[Controller]/[Action]")]
    //[Authorize(Roles = "Reimbursment")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
