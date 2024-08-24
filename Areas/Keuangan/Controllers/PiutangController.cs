using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using NoiSys.Areas.Identity.Data;
using NoiSys.Areas.Keuangan.Models;
using NoiSys.Areas.Keuangan.Repository;
using NoiSys.Areas.MasterData.Repository;
using NoiSys.Areas.Reimbursment.Models;
using NoiSys.Areas.Reimbursment.Repository;
using NoiSys.Data;
using System.Data;

namespace NoiSys.Areas.Keuangan.Controllers
{
    [Area("Keuangan")]
    [Route("Keuangan/[Controller]/[Action]")]
    [Authorize(Roles = "PiutangUsaha")]
    public class PiutangController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IPiutangUsahaRepository _piutangUsahaRepository;
        private readonly IBankRepository _bankRepository;

        public PiutangController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext applicationDbContext,
            IPiutangUsahaRepository piutangUsahaRepository,
            IBankRepository bankRepository
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _applicationDbContext = applicationDbContext;
            _piutangUsahaRepository = piutangUsahaRepository;
            _bankRepository = bankRepository;
        }

        [Authorize(Roles = "IndexPiutangUsaha")]
        public IActionResult Index()
        {
            var data = _piutangUsahaRepository.GetAllPiutangUsaha();
            return View(data);
        }

        [HttpGet]
        [Authorize(Roles = "DetailPiutangUsaha")]
        [AllowAnonymous]
        public async Task<IActionResult> DetailPiutangUsaha(Guid Id)
        {
            ViewBag.Users = new SelectList(_userManager.Users, nameof(ApplicationUser.Id), nameof(ApplicationUser.NamaLengkap), SortOrder.Ascending);
            ViewBag.Bank = new SelectList(await _bankRepository.GetBanks(), "BankId", "NamaBank", SortOrder.Ascending);

            var Piutang = await _piutangUsahaRepository.GetPiutangUsahaById(Id);

            if (Piutang == null)
            {
                Response.StatusCode = 404;
                return View("PiutangNotFound", Id);
            }

            PiutangUsaha model = new PiutangUsaha
            {
                PiutangId = Piutang.PiutangId,
                PiutangNumber = Piutang.PiutangNumber,
                TransaksiId = Piutang.TransaksiId,
                TransaksiNumber = Piutang.TransaksiNumber,
                UserId = Piutang.UserId,
                BankId = Piutang.BankId,
                Nominal = Math.Truncate(Piutang.Nominal)
            };
            return View(model);
        }
    }
}
