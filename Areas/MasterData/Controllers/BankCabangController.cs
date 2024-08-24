using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NoiSys.Areas.MasterData.Models;
using NoiSys.Areas.MasterData.Repository;
using NoiSys.Areas.MasterData.ViewModels;
using NoiSys.Data;

namespace NoiSys.Areas.MasterData.Controllers
{
    [Area("MasterData")]
    [Route("MasterData/[Controller]/[Action]")]
    [Authorize(Roles = "BankCabang")]
    public class BankCabangController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IBankRepository _bankRepository;
        private readonly IBankCabangRepository _bankCabangRepository;
        private readonly IPenggunaRepository _penggunaRepository;
        public BankCabangController(
            ApplicationDbContext applicationDbContext,
            IBankCabangRepository bankCabangRepository,
            IBankRepository bankRepository,
            IPenggunaRepository penggunaRepository
        )
        {
            _applicationDbContext = applicationDbContext;
            _bankCabangRepository = bankCabangRepository;
            _bankRepository = bankRepository;
            _penggunaRepository = penggunaRepository;
        }
        [Authorize(Roles = "IndexBankCabang")]
        public IActionResult Index()
        {
            var tampilkanData = _bankCabangRepository.GetAllBankCabang();
            return View(tampilkanData);
        }

        [Authorize(Roles = "CreateBankCabang")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ViewResult> CreateBankCabang()
        {
            ViewBag.Bank = new SelectList(await _bankRepository.GetBanks(), "BankId", "NamaBank", SortOrder.Ascending);

            var bankCabang = new BankCabangViewModel();
            var dateNow = DateTimeOffset.Now;
            var lastCodebank = _bankCabangRepository.GetAllBankCabang().Where(d => d.CreateDateTime.ToString("yyMMdd") == dateNow.ToString("yyMMdd")).OrderByDescending(c => c.KodeBankCabang).FirstOrDefault();
            var setDateNow = DateTimeOffset.Now.ToString("yyMMdd");

            if (lastCodebank == null)
            {
                bankCabang.KodeBankCabang = "BCB" + setDateNow + "0001";
            }
            else
            {
                var lastDateCabang = lastCodebank.KodeBankCabang.Substring(3, 6);

                if (lastDateCabang != setDateNow)
                {
                    bankCabang.KodeBankCabang = "BCB" + setDateNow + "0001";
                }
                else
                {
                    bankCabang.KodeBankCabang = "BCB" + setDateNow + (Convert.ToInt32(lastCodebank.KodeBankCabang.Substring(9, lastCodebank.KodeBankCabang.Length - 9)) + 1).ToString("D4");
                }
            }
            return View(bankCabang);
        }

        [Authorize(Roles = "CreateBankCabang")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateBankCabang(BankCabangViewModel vm)
        {
            ViewBag.Bank = new SelectList(await _bankRepository.GetBanks(), "BankId", "NamaBank", SortOrder.Ascending);

            var dateNow = DateTimeOffset.Now;
            var lastBank = _bankCabangRepository.GetAllBankCabang().Where(d => d.CreateDateTime.ToString("yyMMdd") == dateNow.ToString("yyMMdd")).OrderByDescending(c => c.KodeBankCabang).FirstOrDefault();
            var setDateNow = DateTimeOffset.Now.ToString("yyMMdd");

            if (lastBank == null)
            {
                vm.KodeBankCabang = "BCB" + setDateNow + "0001";
            }
            else
            {
                var lastDateBankCabang = lastBank.KodeBankCabang.Substring(3, 6);

                if (lastDateBankCabang != setDateNow)
                {
                    vm.KodeBankCabang = "BCB" + setDateNow + "0001";
                }
                else
                {
                    vm.KodeBankCabang = "BCB" + setDateNow + (Convert.ToInt32(lastBank.KodeBankCabang.Substring(9, lastBank.KodeBankCabang.Length - 9)) + 1).ToString("D4");
                }
            }

            var getUser = _penggunaRepository.GetAllUserLogin().Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            if (ModelState.IsValid)
            {
                var newBankCabang = new BankCabang
                {
                    CreateDateTime = DateTime.Now,
                    CreateBy = new Guid(getUser.Id),
                    BankId = vm.BankId,
                    KodeBankCabang = vm.KodeBankCabang,
                    NamaCabang = vm.NamaCabang,
                    NomorRekening = vm.NomorRekening,
                    AtasNama = vm.AtasNama,

                };

                var result = _bankCabangRepository.GetAllBankCabang().Where(c => c.NamaCabang == vm.NamaCabang).FirstOrDefault();

                if (result == null)
                {
                    _bankCabangRepository.Add(newBankCabang);
                    TempData["SuccessMessage"] = "Bank Cabang " + vm.NamaCabang + " Berhasil Disimpan";
                    return RedirectToAction("Index", "BankCabang");
                }
                else
                {
                    ViewBag.Bank = new SelectList(await _bankRepository.GetBanks(), "BankId", "NamaBank", SortOrder.Ascending);
                    ModelState.AddModelError("", "Maaf, Nama Bank Cabang sudah ada !!!");
                    return View(vm);
                }
            }
            ViewBag.Bank = new SelectList(await _bankRepository.GetBanks(), "BankId", "NamaBank", SortOrder.Ascending);
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "DetailBankCabang")]
        [AllowAnonymous]
        public async Task<IActionResult> DetailBankCabang(Guid Id)
        {
            ViewBag.Bank = new SelectList(await _bankRepository.GetBanks(), "BankId", "NamaBank", SortOrder.Ascending);

            var bankCabang = await _bankCabangRepository.GetBankCabangById(Id);

            if (bankCabang == null)
            {
                Response.StatusCode = 404;
                return View("BankNotFound", Id);
            }

            DetailBankCabangViewModel viewModel = new DetailBankCabangViewModel
            {
                BankCabangId = bankCabang.BankCabangId,
                BankId = bankCabang.BankId,
                KodeBankCabang = bankCabang.KodeBankCabang,
                NamaCabang = bankCabang.NamaCabang,
                NomorRekening = bankCabang.NomorRekening,
                AtasNama = bankCabang.AtasNama,
            };
            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "DetailBankCabang")]
        [AllowAnonymous]
        public async Task<IActionResult> DetailBankCabang(DetailBankCabangViewModel viewModel)
        {
            ViewBag.Bank = new SelectList(await _bankRepository.GetBanks(), "BankId", "NamaBank", SortOrder.Ascending);

            if (ModelState.IsValid)
            {
                ViewBag.Bank = new SelectList(await _bankRepository.GetBanks(), "BankId", "NamaBank", SortOrder.Ascending);

                BankCabang bankCabang = await _bankCabangRepository.GetBankCabangByIdNoTracking(viewModel.BankCabangId);

                var check = _bankCabangRepository.GetAllBankCabang().Where(d => d.KodeBankCabang == viewModel.KodeBankCabang).FirstOrDefault();

                if (check != null)
                {
                    bankCabang.UpdateDateTime = DateTime.Now;
                    bankCabang.KodeBankCabang = viewModel.KodeBankCabang;
                    bankCabang.BankId = viewModel.BankId;
                    bankCabang.NamaCabang = viewModel.NamaCabang;
                    bankCabang.NomorRekening = viewModel.NomorRekening;
                    bankCabang.AtasNama = viewModel.AtasNama;

                    _bankCabangRepository.Update(bankCabang);
                    _applicationDbContext.SaveChanges();

                    TempData["SuccessMessage"] = "Bank Cabang " + viewModel.NamaCabang + " Berhasil Diubah";
                    return RedirectToAction("Index", "BankCabang");
                }
                else
                {
                    ViewBag.Bank = new SelectList(await _bankRepository.GetBanks(), "BankId", "NamaBank", SortOrder.Ascending);
                    TempData["WarningMessage"] = "Bank Cabang " + viewModel.NamaCabang + " sudah ada !!!";
                    return View(viewModel);
                }
            }
            ViewBag.Bank = new SelectList(await _bankRepository.GetBanks(), "BankId", "NamaBank", SortOrder.Ascending);
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "DeleteBankCabang")]
        [AllowAnonymous]
        //[Authorize]
        public async Task<IActionResult> DeleteBankCabang(Guid Id)
        {
            var bankCabang = await _bankCabangRepository.GetBankCabangById(Id);
            if (bankCabang == null)
            {
                Response.StatusCode = 404;
                return View("BankCabangNotFound", Id);
            }

            DetailBankCabangViewModel vm = new DetailBankCabangViewModel
            {
                BankCabangId = bankCabang.BankCabangId,
                BankId = bankCabang.BankId,
                KodeBankCabang = bankCabang.KodeBankCabang,
                NamaCabang = bankCabang.NamaCabang,
                NomorRekening = bankCabang.NomorRekening,
                AtasNama = bankCabang.AtasNama,
            };
            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = "DeleteBankCabang")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteBankCabang(DetailBankCabangViewModel vm)
        {
            //Hapus Data
            var bankCabangs = _applicationDbContext.BankCabangs.FirstOrDefault(x => x.BankId == vm.BankId);
            _applicationDbContext.Attach(bankCabangs);
            _applicationDbContext.Entry(bankCabangs).State = EntityState.Deleted;
            _applicationDbContext.SaveChanges();

            TempData["SuccessMessage"] = "Bank Cabang " + vm.NamaCabang + " Berhasil Dihapus";
            return RedirectToAction("Index", "BankCabang");
        }
    }
}
