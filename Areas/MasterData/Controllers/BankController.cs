using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NoiSys.Areas.MasterData.Models;
using NoiSys.Areas.MasterData.Repository;
using NoiSys.Areas.MasterData.ViewModels;
using NoiSys.Data;
using System.Data;

namespace NoiSys.Areas.MasterData.Controllers
{
    [Area("MasterData")]
    [Route("MasterData/[Controller]/[Action]")]
    [Authorize(Roles = "Bank")]
    public class BankController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IBankRepository _bankRepository;
        private readonly IPenggunaRepository _penggunaRepository;
        private readonly IBankCabangRepository _bankCabangRepository;

        public BankController(
            ApplicationDbContext applicationDbContext,
            IBankRepository bankRepository,
            IPenggunaRepository penggunaRepository,
            IBankCabangRepository bankCabangRepository
        )
        {
            _applicationDbContext = applicationDbContext;
            _bankRepository = bankRepository;
            _penggunaRepository = penggunaRepository;
            _bankCabangRepository = bankCabangRepository;
        }

        [Authorize(Roles = "IndexBank")]
        public IActionResult Index()
        {
            var tampilkanData = _bankRepository.GetAllBank();
            return View(tampilkanData);
        }

        [Authorize(Roles = "CreateBank")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ViewResult> CreateBank()
        {
            var bank = new BankViewModel();
            var dateNow = DateTimeOffset.Now;
            var lastCodebank = _bankRepository.GetAllBank().Where(d => d.CreateDateTime.ToString("yyMMdd") == dateNow.ToString("yyMMdd")).OrderByDescending(c => c.KodeBank).FirstOrDefault();
            var setDateNow = DateTimeOffset.Now.ToString("yyMMdd");

            if (lastCodebank == null)
            {
                bank.KodeBank = "BNK" + setDateNow + "0001";
            }
            else
            {
                var lastDateEdu = lastCodebank.KodeBank.Substring(3, 6);

                if (lastDateEdu != setDateNow)
                {
                    bank.KodeBank = "BNK" + setDateNow + "0001";
                }
                else
                {
                    bank.KodeBank = "BNK" + setDateNow + (Convert.ToInt32(lastCodebank.KodeBank.Substring(9, lastCodebank.KodeBank.Length - 9)) + 1).ToString("D4");
                }
            }
            return View(bank);
        }

        [Authorize(Roles = "CreateBank")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateBank(BankViewModel vm)
        {
            var dateNow = DateTimeOffset.Now;
            var lastBank = _bankRepository.GetAllBank().Where(d => d.CreateDateTime.ToString("yyMMdd") == dateNow.ToString("yyMMdd")).OrderByDescending(c => c.KodeBank).FirstOrDefault();
            var setDateNow = DateTimeOffset.Now.ToString("yyMMdd");

            if (lastBank == null)
            {
                vm.KodeBank = "BNK" + setDateNow + "0001";
            }
            else
            {
                var lastDateBank = lastBank.KodeBank.Substring(3, 6);

                if (lastDateBank != setDateNow)
                {
                    vm.KodeBank = "BNK" + setDateNow + "0001";
                }
                else
                {
                    vm.KodeBank = "BNK" + setDateNow + (Convert.ToInt32(lastBank.KodeBank.Substring(9, lastBank.KodeBank.Length - 9)) + 1).ToString("D4");
                }
            }

            var getUser = _penggunaRepository.GetAllUserLogin().Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            if (ModelState.IsValid)
            {
                var newBank = new Bank
                {
                    CreateDateTime = DateTime.Now,
                    CreateBy = new Guid(getUser.Id),
                    BankId = vm.BankId,
                    KodeBank = vm.KodeBank,
                    NamaBank = vm.NamaBank
                };

                var result = _bankRepository.GetAllBank().Where(c => c.NamaBank == vm.NamaBank).FirstOrDefault();

                if (result == null)
                {
                    _bankRepository.Add(newBank);
                    TempData["SuccessMessage"] = "Bank " + vm.NamaBank + " Berhasil Disimpan";
                    return RedirectToAction("Index", "Bank");
                }
                else
                {
                    ModelState.AddModelError("", "Maaf, Nama Bank sudah ada !!!");
                    return View(vm);
                }
            }
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "DetailBank")]
        [AllowAnonymous]
        public async Task<IActionResult> DetailBank(Guid Id)
        {
            ViewBag.Pengguna = new SelectList(await _penggunaRepository.GetPenggunas(), "PenggunaId", "NamaLengkap", SortOrder.Ascending);

            var bank = await _bankRepository.GetBankById(Id);

            if (bank == null)
            {
                Response.StatusCode = 404;
                return View("BankNotFound", Id);
            }

            DetailBankViewModel viewModel = new DetailBankViewModel
            {
                BankId = bank.BankId,
                KodeBank = bank.KodeBank,
                NamaBank = bank.NamaBank
            };
            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "DetailBank")]
        [AllowAnonymous]
        public async Task<IActionResult> DetailBank(DetailBankViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Pengguna = new SelectList(await _penggunaRepository.GetPenggunas(), "PenggunaId", "NamaLengkap", SortOrder.Ascending);

                Bank bank = await _bankRepository.GetBankByIdNoTracking(viewModel.BankId);

                var check = _bankRepository.GetAllBank().Where(d => d.KodeBank == viewModel.KodeBank).FirstOrDefault();

                if (check != null)
                {
                    bank.UpdateDateTime = DateTime.Now;
                    bank.KodeBank = viewModel.KodeBank;
                    bank.NamaBank = viewModel.NamaBank;

                    _bankRepository.Update(bank);
                    _applicationDbContext.SaveChanges();

                    TempData["SuccessMessage"] = "Bank " + viewModel.NamaBank + " Berhasil Diubah";
                    return RedirectToAction("Index", "Bank");
                }
                else
                {
                    ViewBag.Pengguna = new SelectList(await _penggunaRepository.GetPenggunas(), "PenggunaId", "NamaLengkap", SortOrder.Ascending);
                    TempData["WarningMessage"] = "Bank " + viewModel.NamaBank + " sudah ada !!!";
                    return View(viewModel);
                }
            }
            ViewBag.Pengguna = new SelectList(await _penggunaRepository.GetPenggunas(), "PenggunaId", "NamaLengkap", SortOrder.Ascending);
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "DeleteBank")]
        [AllowAnonymous]
        //[Authorize]
        public async Task<IActionResult> DeleteBank(Guid Id)
        {
            var Bank = await _bankRepository.GetBankById(Id);
            if (Bank == null)
            {
                Response.StatusCode = 404;
                return View("BankNotFound", Id);
            }

            DetailBankViewModel vm = new DetailBankViewModel
            {
                BankId = Bank.BankId,
                KodeBank = Bank.KodeBank,
                NamaBank = Bank.NamaBank,
            };
            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = "DeleteBank")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteBank(DetailBankViewModel vm)
        {
            //Cek Relasi Bank dengan Bank Cabang
            var bangCabang = _bankCabangRepository.GetAllBankCabang().Where(p => p.BankId == vm.BankId).FirstOrDefault();
            if (bangCabang == null)
            {
                //Hapus Data
                var banks = _applicationDbContext.Banks.FirstOrDefault(x => x.BankId == vm.BankId);
                _applicationDbContext.Attach(banks);
                _applicationDbContext.Entry(banks).State = EntityState.Deleted;
                _applicationDbContext.SaveChanges();

                TempData["SuccessMessage"] = "Nama " + vm.NamaBank + " Berhasil Dihapus";
                return RedirectToAction("Index", "Bank");
            }
            else
            {
                TempData["WarningMessage"] = "Nama " + vm.NamaBank + " terelasi dengan bank cabang " + bangCabang.NamaCabang;
                return View(vm);
            }            
        }     
    }
}
