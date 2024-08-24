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
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace NoiSys.Areas.MasterData.Controllers
{
    [Area("MasterData")]
    [Route("MasterData/[Controller]/[Action]")]
    [Authorize(Roles = "Mekanik")]
    public class MekanikController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMekanikRepository _mekanikRepository;
        private readonly IBengkelRepository _bengkelRepository;

        public MekanikController(
            IHostingEnvironment hostingEnvironment,
            ApplicationDbContext applicationDbContext,
            IMekanikRepository mekanikRepository,
            IBengkelRepository bengkelRepository
        )
        {
            _hostingEnvironment = hostingEnvironment;
            _applicationDbContext = applicationDbContext;
            _mekanikRepository = mekanikRepository;
            _bengkelRepository = bengkelRepository;
        }
        [Authorize(Roles = "IndexMekanik")]
        public IActionResult Index()
        {
            var data = _mekanikRepository.GetAllMekanik();
            return View(data);
        }

        [Authorize(Roles = "CreateMekanik")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ViewResult> CreateMekanik()
        {
            ViewBag.Bengkel = new SelectList(await _bengkelRepository.GetBengkels(), "BengkelId", "NamaBengkel", SortOrder.Ascending);

            var Mekanik = new MekanikViewModel();
            var dateNow = DateTimeOffset.Now;
            var setDateNow = DateTimeOffset.Now.ToString("yyMMdd");

            var lastCode = _mekanikRepository.GetAllMekanik().Where(d => d.CreateDateTime.ToString("yyMMdd") == dateNow.ToString("yyMMdd")).OrderByDescending(k => k.KodeMekanik).FirstOrDefault();
            if (lastCode == null)
            {
                Mekanik.KodeMekanik = "MKN" + setDateNow + "0001";
            }
            else
            {
                var lastCodeTrim = lastCode.KodeMekanik.Substring(3, 6);

                if (lastCodeTrim != setDateNow)
                {
                    Mekanik.KodeMekanik = "MKN" + setDateNow + "0001";
                }
                else
                {
                    Mekanik.KodeMekanik = "MKN" + setDateNow + (Convert.ToInt32(lastCode.KodeMekanik.Substring(9, lastCode.KodeMekanik.Length - 9)) + 1).ToString("D4");
                }
            }

            return View(Mekanik);
        }

        [Authorize(Roles = "CreateMekanik")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateMekanik(MekanikViewModel vm)
        {
            ViewBag.Bengkel = new SelectList(await _bengkelRepository.GetBengkels(), "BengkelId", "NamaBengkel", SortOrder.Ascending);

            var dateNow = DateTimeOffset.Now;
            var setDateNow = DateTimeOffset.Now.ToString("yyMMdd");

            var lastCode = _mekanikRepository.GetAllMekanik().Where(d => d.CreateDateTime.ToString("yyMMdd") == dateNow.ToString("yyMMdd")).OrderByDescending(k => k.KodeMekanik).FirstOrDefault();
            if (lastCode == null)
            {
                vm.KodeMekanik = "MKN" + setDateNow + "0001";
            }
            else
            {
                var lastCodeTrim = lastCode.KodeMekanik.Substring(3, 6);

                if (lastCodeTrim != setDateNow)
                {
                    vm.KodeMekanik = "MKN" + setDateNow + "0001";
                }
                else
                {
                    vm.KodeMekanik = "MKN" + setDateNow + (Convert.ToInt32(lastCode.KodeMekanik.Substring(9, lastCode.KodeMekanik.Length - 9)) + 1).ToString("D4");
                }
            }

            if (ModelState.IsValid)
            {
                var Mekanik = new Mekanik
                {
                    CreateDateTime = DateTime.Now,
                    MekanikId = vm.MekanikId,
                    KodeMekanik = vm.KodeMekanik,
                    NamaMekanik = vm.NamaMekanik,
                    BengkelId = vm.BengkelId,
                };

                var resultMekanik = _mekanikRepository.GetAllMekanik().Where(c => c.NamaMekanik == vm.NamaMekanik).FirstOrDefault();

                if (resultMekanik == null)
                {
                    _mekanikRepository.Tambah(Mekanik);
                    TempData["SuccessMessage"] = "Mekanik " + vm.NamaMekanik + " Berhasil Disimpan";
                    return RedirectToAction("Index", "Mekanik");
                }
                else
                {
                    ViewBag.Bengkel = new SelectList(await _bengkelRepository.GetBengkels(), "BengkelId", "NamaBengkel", SortOrder.Ascending);
                    TempData["WarningMessage"] = "Mekanik " + vm.NamaMekanik + " sudah ada !!!";
                    return View(vm);
                }
            }
            ViewBag.Bengkel = new SelectList(await _bengkelRepository.GetBengkels(), "BengkelId", "NamaBengkel", SortOrder.Ascending);
            return View();
        }

        [Authorize(Roles = "DetailMekanik")]
        [AllowAnonymous]
        public async Task<IActionResult> DetailMekanik(Guid Id)
        {
            var Mekanik = await _mekanikRepository.GetMekanikById(Id);

            if (Mekanik == null)
            {
                Response.StatusCode = 404;
                return View("MekanikNotFound", Id);
            }

            DetailMekanikViewModel viewModel = new DetailMekanikViewModel
            {
                MekanikId = Mekanik.MekanikId,
                KodeMekanik = Mekanik.KodeMekanik,
                NamaMekanik = Mekanik.NamaMekanik,
                BengkelId = Mekanik.BengkelId,
            };
            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "DetailMekanik")]
        [AllowAnonymous]
        public async Task<IActionResult> DetailMekanik(DetailMekanikViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Mekanik Mekanik = await _mekanikRepository.GetMekanikByIdNoTracking(viewModel.MekanikId);

                var check = _mekanikRepository.GetAllMekanik().Where(d => d.KodeMekanik == viewModel.KodeMekanik).FirstOrDefault();

                if (check != null)
                {
                    Mekanik.UpdateDateTime = DateTime.Now;
                    Mekanik.KodeMekanik = viewModel.KodeMekanik;
                    Mekanik.NamaMekanik = viewModel.NamaMekanik;
                    Mekanik.BengkelId = viewModel.BengkelId;                    

                    _mekanikRepository.Update(Mekanik);
                    _applicationDbContext.SaveChanges();

                    TempData["SuccessMessage"] = "Mekanik " + viewModel.NamaMekanik + " Berhasil Diubah";
                    return RedirectToAction("Index", "Mekanik");
                }
                else
                {
                    TempData["WarningMessage"] = "Mekanik " + viewModel.NamaMekanik + " sudah ada !!!";
                    return View(viewModel);
                }
            }
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "DeleteMekanik")]
        [AllowAnonymous]
        //[Authorize]
        public async Task<IActionResult> DeleteMekanik(Guid Id)
        {
            var Mekanik = await _mekanikRepository.GetMekanikById(Id);
            if (Mekanik == null)
            {
                Response.StatusCode = 404;
                return View("MekanikNotFound", Id);
            }

            DetailMekanikViewModel vm = new DetailMekanikViewModel
            {
                MekanikId = Mekanik.MekanikId,
                KodeMekanik = Mekanik.KodeMekanik,
                NamaMekanik = Mekanik.NamaMekanik,
                BengkelId = Mekanik.BengkelId,
            };
            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = "DeleteMekanik")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteMekanik(DetailMekanikViewModel vm)
        {
            //Hapus Data
            var Mekaniks = _applicationDbContext.Mekaniks.FirstOrDefault(x => x.MekanikId == vm.MekanikId);
            _applicationDbContext.Attach(Mekaniks);
            _applicationDbContext.Entry(Mekaniks).State = EntityState.Deleted;
            _applicationDbContext.SaveChanges();            

            TempData["SuccessMessage"] = "Mekanik " + vm.NamaMekanik + " Berhasil Dihapus";

            return RedirectToAction("Index", "Mekanik");
        }        
    }
}
