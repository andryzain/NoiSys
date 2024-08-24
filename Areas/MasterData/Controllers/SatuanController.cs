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
    [Authorize(Roles = "Satuan")]
    public class SatuanController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly ISatuanRepository _satuanRepository;
        private readonly IProdukRepository _produkRepository;

        public SatuanController(
            ApplicationDbContext applicationDbContext,
            ISatuanRepository satuanRepository,
            IProdukRepository produkRepository
        )
        {
            _applicationDbContext = applicationDbContext;
            _satuanRepository = satuanRepository;
            _produkRepository = produkRepository;
        }
        [Authorize(Roles = "IndexSatuan")]
        public IActionResult Index()
        {
            var data = _satuanRepository.GetAllSatuan();
            return View(data);
        }

        [Authorize(Roles = "CreateSatuan")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ViewResult> CreateSatuan()
        {
            var Satuan = new SatuanViewModel();
            var dateNow = DateTimeOffset.Now;
            var setDateNow = DateTimeOffset.Now.ToString("yyMMdd");

            var lastCode = _satuanRepository.GetAllSatuan().Where(d => d.CreateDateTime.ToString("yyMMdd") == dateNow.ToString("yyMMdd")).OrderByDescending(k => k.KodeSatuan).FirstOrDefault();
            if (lastCode == null)
            {
                Satuan.KodeSatuan = "STN" + setDateNow + "0001";
            }
            else
            {
                var lastCodeTrim = lastCode.KodeSatuan.Substring(3, 6);

                if (lastCodeTrim != setDateNow)
                {
                    Satuan.KodeSatuan = "STN" + setDateNow + "0001";
                }
                else
                {
                    Satuan.KodeSatuan = "STN" + setDateNow + (Convert.ToInt32(lastCode.KodeSatuan.Substring(9, lastCode.KodeSatuan.Length - 9)) + 1).ToString("D4");
                }
            }

            return View(Satuan);
        }

        [Authorize(Roles = "CreateSatuan")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateSatuan(SatuanViewModel vm)
        {
            var dateNow = DateTimeOffset.Now;
            var setDateNow = DateTimeOffset.Now.ToString("yyMMdd");

            var lastCode = _satuanRepository.GetAllSatuan().Where(d => d.CreateDateTime.ToString("yyMMdd") == dateNow.ToString("yyMMdd")).OrderByDescending(k => k.KodeSatuan).FirstOrDefault();
            if (lastCode == null)
            {
                vm.KodeSatuan = "STN" + setDateNow + "0001";
            }
            else
            {
                var lastCodeTrim = lastCode.KodeSatuan.Substring(3, 6);

                if (lastCodeTrim != setDateNow)
                {
                    vm.KodeSatuan = "STN" + setDateNow + "0001";
                }
                else
                {
                    vm.KodeSatuan = "STN" + setDateNow + (Convert.ToInt32(lastCode.KodeSatuan.Substring(9, lastCode.KodeSatuan.Length - 9)) + 1).ToString("D4");
                }
            }

            if (ModelState.IsValid)
            {
                var Satuan = new Satuan
                {
                    CreateDateTime = DateTime.Now,
                    SatuanId = vm.SatuanId,
                    KodeSatuan = vm.KodeSatuan,
                    NamaSatuan = vm.NamaSatuan                    
                };

                var resultSatuan = _satuanRepository.GetAllSatuan().Where(c => c.NamaSatuan == vm.NamaSatuan).FirstOrDefault();

                if (resultSatuan == null)
                {
                    _satuanRepository.Tambah(Satuan);
                    TempData["SuccessMessage"] = "Satuan " + vm.NamaSatuan + " Berhasil Disimpan";
                    return RedirectToAction("Index", "Satuan");
                }
                else
                {
                    TempData["WarningMessage"] = "Satuan " + vm.NamaSatuan + " sudah ada !!!";
                    return View(vm);
                }
            }
            return View();
        }

        [Authorize(Roles = "DetailSatuan")]
        [AllowAnonymous]
        public async Task<IActionResult> DetailSatuan(Guid Id)
        {
            var Satuan = await _satuanRepository.GetSatuanById(Id);

            if (Satuan == null)
            {
                Response.StatusCode = 404;
                return View("SatuanNotFound", Id);
            }

            DetailSatuanViewModel viewModel = new DetailSatuanViewModel
            {
                SatuanId = Satuan.SatuanId,
                KodeSatuan = Satuan.KodeSatuan,
                NamaSatuan = Satuan.NamaSatuan
            };
            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "DetailSatuan")]
        [AllowAnonymous]
        public async Task<IActionResult> DetailSatuan(DetailSatuanViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Satuan Satuan = await _satuanRepository.GetSatuanByIdNoTracking(viewModel.SatuanId);

                var check = _satuanRepository.GetAllSatuan().Where(d => d.KodeSatuan == viewModel.KodeSatuan).FirstOrDefault();

                if (check != null)
                {
                    Satuan.UpdateDateTime = DateTime.Now;
                    Satuan.KodeSatuan = viewModel.KodeSatuan;
                    Satuan.NamaSatuan = viewModel.NamaSatuan;

                    _satuanRepository.Update(Satuan);
                    _applicationDbContext.SaveChanges();

                    TempData["SuccessMessage"] = "Satuan " + viewModel.NamaSatuan + " Berhasil Diubah";
                    return RedirectToAction("Index", "Satuan");
                }
                else
                {
                    TempData["WarningMessage"] = "Satuan " + viewModel.NamaSatuan + " sudah ada !!!";
                    return View(viewModel);
                }
            }
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "DeleteSatuan")]
        [AllowAnonymous]
        //[Authorize]
        public async Task<IActionResult> DeleteSatuan(Guid Id)
        {
            var Satuan = await _satuanRepository.GetSatuanById(Id);
            if (Satuan == null)
            {
                Response.StatusCode = 404;
                return View("SatuanNotFound", Id);
            }

            DetailSatuanViewModel vm = new DetailSatuanViewModel
            {
                SatuanId = Satuan.SatuanId,
                KodeSatuan = Satuan.KodeSatuan,
                NamaSatuan = Satuan.NamaSatuan
            };
            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = "DeleteSatuan")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteSatuan(DetailSatuanViewModel vm)
        {
            //Cek Relasi Principal dengan Produk
            var produk = _produkRepository.GetAllProduk().Where(p => p.SatuanId == vm.SatuanId).FirstOrDefault();
            if (produk == null)
            {
                //Hapus Data
                var Satuans = _applicationDbContext.Satuans.FirstOrDefault(x => x.SatuanId == vm.SatuanId);
                _applicationDbContext.Attach(Satuans);
                _applicationDbContext.Entry(Satuans).State = EntityState.Deleted;
                _applicationDbContext.SaveChanges();

                TempData["SuccessMessage"] = "Satuan " + vm.NamaSatuan + " Berhasil Dihapus";
                return RedirectToAction("Index", "Satuan");
            }
            else
            {
                TempData["WarningMessage"] = "Satuan " + vm.NamaSatuan + " terelasi dengan produk " + produk.NamaProduk;
                return View(vm);
            }            
        }
    }
}
