using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    [Authorize(Roles = "Diskon")]
    public class DiskonController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IDiskonRepository _diskonRepository;
        private readonly IProdukRepository _produkRepository;

        public DiskonController(
            ApplicationDbContext applicationDbContext,
            IDiskonRepository diskonRepository,
            IProdukRepository produkRepository
        )
        {
            _applicationDbContext = applicationDbContext;
            _diskonRepository = diskonRepository;
            _produkRepository = produkRepository;
        }
        [Authorize(Roles = "IndexDiskon")]
        public IActionResult Index()
        {
            var data = _diskonRepository.GetAllDiskon();
            return View(data);
        }

        [Authorize(Roles = "CreateDiskon")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ViewResult> CreateDiskon()
        {
            var Diskon = new DiskonViewModel();
            var dateNow = DateTimeOffset.Now;
            var setDateNow = DateTimeOffset.Now.ToString("yyMMdd");

            var lastCode = _diskonRepository.GetAllDiskon().Where(d => d.CreateDateTime.ToString("yyMMdd") == dateNow.ToString("yyMMdd")).OrderByDescending(k => k.KodeDiskon).FirstOrDefault();
            if (lastCode == null)
            {
                Diskon.KodeDiskon = "DSK" + setDateNow + "0001";
            }
            else
            {
                var lastCodeTrim = lastCode.KodeDiskon.Substring(3, 6);

                if (lastCodeTrim != setDateNow)
                {
                    Diskon.KodeDiskon = "DSK" + setDateNow + "0001";
                }
                else
                {
                    Diskon.KodeDiskon = "DSK" + setDateNow + (Convert.ToInt32(lastCode.KodeDiskon.Substring(9, lastCode.KodeDiskon.Length - 9)) + 1).ToString("D4");
                }
            }

            return View(Diskon);
        }

        [Authorize(Roles = "CreateDiskon")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateDiskon(DiskonViewModel vm)
        {
            var dateNow = DateTimeOffset.Now;
            var setDateNow = DateTimeOffset.Now.ToString("yyMMdd");

            var lastCode = _diskonRepository.GetAllDiskon().Where(d => d.CreateDateTime.ToString("yyMMdd") == dateNow.ToString("yyMMdd")).OrderByDescending(k => k.KodeDiskon).FirstOrDefault();
            if (lastCode == null)
            {
                vm.KodeDiskon = "DSK" + setDateNow + "0001";
            }
            else
            {
                var lastCodeTrim = lastCode.KodeDiskon.Substring(3, 6);

                if (lastCodeTrim != setDateNow)
                {
                    vm.KodeDiskon = "DSK" + setDateNow + "0001";
                }
                else
                {
                    vm.KodeDiskon = "DSK" + setDateNow + (Convert.ToInt32(lastCode.KodeDiskon.Substring(9, lastCode.KodeDiskon.Length - 9)) + 1).ToString("D4");
                }
            }

            if (ModelState.IsValid)
            {
                var Diskon = new Diskon
                {
                    CreateDateTime = DateTime.Now,
                    DiskonId = vm.DiskonId,
                    KodeDiskon = vm.KodeDiskon,
                    Nilai = vm.Nilai
                };

                var resultDiskon = _diskonRepository.GetAllDiskon().Where(c => c.Nilai == vm.Nilai).FirstOrDefault();

                if (resultDiskon == null)
                {
                    _diskonRepository.Tambah(Diskon);
                    TempData["SuccessMessage"] = "Diskon " + vm.Nilai + "% Berhasil Disimpan";
                    return RedirectToAction("Index", "Diskon");
                }
                else
                {
                    TempData["WarningMessage"] = "Nilai diskon " + vm.Nilai + "% sudah ada !!!";
                    //ModelState.AddModelError("", "Maaf, nama Diskon sudah ada !!!");
                    return View(vm);
                }
            }
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "DetailDiskon")]
        [AllowAnonymous]
        public async Task<IActionResult> DetailDiskon(Guid Id)
        {
            var Diskon = await _diskonRepository.GetDiskonById(Id);

            if (Diskon == null)
            {
                Response.StatusCode = 404;
                return View("DiskonNotFound", Id);
            }

            DetailDiskonViewModel viewModel = new DetailDiskonViewModel
            {
                DiskonId = Diskon.DiskonId,
                KodeDiskon = Diskon.KodeDiskon,
                Nilai = Diskon.Nilai
            };
            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "DetailDiskon")]
        [AllowAnonymous]
        public async Task<IActionResult> DetailDiskon(DetailDiskonViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Diskon diskon = await _diskonRepository.GetDiskonByIdNoTracking(viewModel.DiskonId);

                var check = _diskonRepository.GetAllDiskon().Where(d => d.KodeDiskon == viewModel.KodeDiskon).FirstOrDefault();

                if (check != null)
                {
                    diskon.UpdateDateTime = DateTime.Now;
                    diskon.KodeDiskon = viewModel.KodeDiskon;
                    diskon.Nilai = viewModel.Nilai;

                    _diskonRepository.Update(diskon);
                    _applicationDbContext.SaveChanges();

                    TempData["SuccessMessage"] = "Diskon " + viewModel.Nilai + " Berhasil Diubah";
                    return RedirectToAction("Index", "Diskon");
                }
                else
                {                    
                    TempData["WarningMessage"] = "Nilai diskon " + viewModel.Nilai + "% sudah ada !!!"; ;
                    return View(viewModel);
                }
            }
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "DeleteDiskon")]
        [AllowAnonymous]
        //[Authorize]
        public async Task<IActionResult> DeleteDiskon(Guid Id)
        {
            var Diskon = await _diskonRepository.GetDiskonById(Id);
            if (Diskon == null)
            {
                Response.StatusCode = 404;
                return View("DiskonNotFound", Id);
            }

            DetailDiskonViewModel vm = new DetailDiskonViewModel
            {
                DiskonId = Diskon.DiskonId,
                KodeDiskon = Diskon.KodeDiskon,
                Nilai = Diskon.Nilai
            };
            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = "DeleteDiskon")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteDiskon(DetailDiskonViewModel vm)
        {
            //Cek Relasi Principal dengan Produk
            var produk = _produkRepository.GetAllProduk().Where(p => p.DiskonId == vm.DiskonId).FirstOrDefault();
            if (produk == null)
            {
                //Hapus Data
                var Diskons = _applicationDbContext.Diskons.FirstOrDefault(x => x.DiskonId == vm.DiskonId);
                _applicationDbContext.Attach(Diskons);
                _applicationDbContext.Entry(Diskons).State = EntityState.Deleted;
                _applicationDbContext.SaveChanges();

                TempData["SuccessMessage"] = "Diskon " + vm.Nilai + " Berhasil Dihapus";
                return RedirectToAction("Index", "Diskon");
            }
            else
            {
                TempData["WarningMessage"] = "Diskon " + vm.Nilai + " terelasi dengan produk " + produk.NamaProduk;
                return View(vm);
            }            
        }
    }
}
