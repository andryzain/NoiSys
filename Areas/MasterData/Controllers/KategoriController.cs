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
    [Authorize(Roles = "Kategori")]
    public class KategoriController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IKategoriRepository _kategoriRepository;
        private readonly IProdukRepository _produkRepository;

        public KategoriController(
            ApplicationDbContext applicationDbContext,
            IKategoriRepository kategoriRepository,
            IProdukRepository produkRepository
        )
        {
            _applicationDbContext = applicationDbContext;
            _kategoriRepository = kategoriRepository;
            _produkRepository = produkRepository;
        }
        [Authorize(Roles = "IndexKategori")]
        public IActionResult Index()
        {
            var data = _kategoriRepository.GetAllKategori();
            return View(data);
        }

        [Authorize(Roles = "CreateKategori")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ViewResult> CreateKategori()
        {
            var Kategori = new KategoriViewModel();
            var dateNow = DateTimeOffset.Now;
            var setDateNow = DateTimeOffset.Now.ToString("yyMMdd");

            var lastCode = _kategoriRepository.GetAllKategori().Where(d => d.CreateDateTime.ToString("yyMMdd") == dateNow.ToString("yyMMdd")).OrderByDescending(k => k.KodeKategori).FirstOrDefault();
            if (lastCode == null)
            {
                Kategori.KodeKategori = "KTG" + setDateNow + "0001";
            }
            else
            {
                var lastCodeTrim = lastCode.KodeKategori.Substring(3, 6);

                if (lastCodeTrim != setDateNow)
                {
                    Kategori.KodeKategori = "KTG" + setDateNow + "0001";
                }
                else
                {
                    Kategori.KodeKategori = "KTG" + setDateNow + (Convert.ToInt32(lastCode.KodeKategori.Substring(9, lastCode.KodeKategori.Length - 9)) + 1).ToString("D4");
                }
            }

            return View(Kategori);
        }

        [Authorize(Roles = "CreateKategori")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateKategori(KategoriViewModel vm)
        {
            var dateNow = DateTimeOffset.Now;
            var setDateNow = DateTimeOffset.Now.ToString("yyMMdd");

            var lastCode = _kategoriRepository.GetAllKategori().Where(d => d.CreateDateTime.ToString("yyMMdd") == dateNow.ToString("yyMMdd")).OrderByDescending(k => k.KodeKategori).FirstOrDefault();
            if (lastCode == null)
            {
                vm.KodeKategori = "KTG" + setDateNow + "0001";
            }
            else
            {
                var lastCodeTrim = lastCode.KodeKategori.Substring(3, 6);

                if (lastCodeTrim != setDateNow)
                {
                    vm.KodeKategori = "KTG" + setDateNow + "0001";
                }
                else
                {
                    vm.KodeKategori = "KTG" + setDateNow + (Convert.ToInt32(lastCode.KodeKategori.Substring(9, lastCode.KodeKategori.Length - 9)) + 1).ToString("D4");
                }
            }

            if (ModelState.IsValid)
            {
                var Kategori = new Kategori
                {
                    CreateDateTime = DateTime.Now,
                    KategoriId = vm.KategoriId,
                    KodeKategori = vm.KodeKategori,
                    NamaKategori = vm.NamaKategori
                };

                var resultKategori = _kategoriRepository.GetAllKategori().Where(c => c.NamaKategori == vm.NamaKategori).FirstOrDefault();

                if (resultKategori == null)
                {
                    _kategoriRepository.Tambah(Kategori);
                    TempData["SuccessMessage"] = "Kategori " + vm.NamaKategori + " Berhasil Disimpan";
                    return RedirectToAction("Index", "Kategori");
                }
                else
                {
                    TempData["WarningMessage"] = "Kategori " + vm.NamaKategori + " sudah ada !!!";
                    return View(vm);
                }
            }
            return View();
        }

        [Authorize(Roles = "DetailKategori")]
        [AllowAnonymous]
        public async Task<IActionResult> DetailKategori(Guid Id)
        {
            var Kategori = await _kategoriRepository.GetKategoriById(Id);

            if (Kategori == null)
            {
                Response.StatusCode = 404;
                return View("KategoriNotFound", Id);
            }

            DetailKategoriViewModel viewModel = new DetailKategoriViewModel
            {
                KategoriId = Kategori.KategoriId,
                KodeKategori = Kategori.KodeKategori,
                NamaKategori = Kategori.NamaKategori
            };
            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "DetailKategori")]
        [AllowAnonymous]
        public async Task<IActionResult> DetailKategori(DetailKategoriViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Kategori Kategori = await _kategoriRepository.GetKategoriByIdNoTracking(viewModel.KategoriId);

                var check = _kategoriRepository.GetAllKategori().Where(d => d.KodeKategori == viewModel.KodeKategori).FirstOrDefault();

                if (check != null)
                {
                    Kategori.UpdateDateTime = DateTime.Now;
                    Kategori.KodeKategori = viewModel.KodeKategori;
                    Kategori.NamaKategori = viewModel.NamaKategori;

                    _kategoriRepository.Update(Kategori);
                    _applicationDbContext.SaveChanges();

                    TempData["SuccessMessage"] = "Kategori " + viewModel.NamaKategori + " Berhasil Diubah";
                    return RedirectToAction("Index", "Kategori");
                }
                else
                {
                    TempData["WarningMessage"] = "Kategori " + viewModel.NamaKategori + " sudah ada !!!";
                    return View(viewModel);
                }
            }
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "DeleteKategori")]
        [AllowAnonymous]
        //[Authorize]
        public async Task<IActionResult> DeleteKategori(Guid Id)
        {
            var Kategori = await _kategoriRepository.GetKategoriById(Id);
            if (Kategori == null)
            {
                Response.StatusCode = 404;
                return View("KategoriNotFound", Id);
            }

            DetailKategoriViewModel vm = new DetailKategoriViewModel
            {
                KategoriId = Kategori.KategoriId,
                KodeKategori = Kategori.KodeKategori,
                NamaKategori = Kategori.NamaKategori
            };
            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = "DeleteKategori")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteKategori(DetailKategoriViewModel vm)
        {
            //Cek Relasi Principal dengan Produk
            var produk = _produkRepository.GetAllProduk().Where(p => p.KategoriId == vm.KategoriId).FirstOrDefault();
            if (produk == null)
            {
                //Hapus Data
                var Kategoris = _applicationDbContext.Kategoris.FirstOrDefault(x => x.KategoriId == vm.KategoriId);
                _applicationDbContext.Attach(Kategoris);
                _applicationDbContext.Entry(Kategoris).State = EntityState.Deleted;
                _applicationDbContext.SaveChanges();

                TempData["SuccessMessage"] = "Kategori " + vm.NamaKategori + " Berhasil Dihapus";
                return RedirectToAction("Index", "Kategori");
            }
            else
            {
                TempData["WarningMessage"] = "Kategori " + vm.NamaKategori + " terelasi dengan produk " + produk.NamaProduk;
                return View(vm);
            }            
        }
    }
}
