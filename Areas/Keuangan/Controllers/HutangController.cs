using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using NoiSys.Areas.Identity.Data;
using NoiSys.Areas.Keuangan.Models;
using NoiSys.Areas.Keuangan.Repository;
using NoiSys.Areas.MasterData.Repository;
using NoiSys.Data;
using System.Data;

namespace NoiSys.Areas.Keuangan.Controllers
{
    [Area("Keuangan")]
    [Route("Keuangan/[Controller]/[Action]")]
    [Authorize(Roles = "HutangUsaha")]
    public class HutangController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IHutangUsahaRepository _hutangUsahaRepository;
        private readonly IBankRepository _bankRepository;

        public HutangController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext applicationDbContext,
            IHutangUsahaRepository hutangUsahaRepository,
            IBankRepository bankRepository
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _applicationDbContext = applicationDbContext;
            _hutangUsahaRepository = hutangUsahaRepository;
            _bankRepository = bankRepository;
        }

        [Authorize(Roles = "IndexHutangUsaha")]
        public IActionResult Index()
        {
            var data = _hutangUsahaRepository.GetAllHutangUsaha();
            return View(data);
        }

        [HttpGet]
        [Authorize(Roles = "DetailHutangUsaha")]
        [AllowAnonymous]
        public async Task<IActionResult> DetailHutangUsaha(Guid Id)
        {
            ViewBag.Users = new SelectList(_userManager.Users, nameof(ApplicationUser.Id), nameof(ApplicationUser.NamaLengkap), SortOrder.Ascending);
            ViewBag.Bank = new SelectList(await _bankRepository.GetBanks(), "BankId", "NamaBank", SortOrder.Ascending);

            var Hutang = await _hutangUsahaRepository.GetHutangUsahaById(Id);

            if (Hutang == null)
            {
                Response.StatusCode = 404;
                return View("HutangNotFound", Id);
            }

            HutangUsaha model = new HutangUsaha
            {
                HutangId = Hutang.HutangId,
                HutangNumber = Hutang.HutangNumber,
                TransaksiId = Hutang.TransaksiId,
                TransaksiNumber = Hutang.TransaksiNumber,
                UserId = Hutang.UserId,
                BankId = Hutang.BankId,
                Nominal = Math.Truncate(Hutang.Nominal),
            };
            return View(model);
        }
    }
}
