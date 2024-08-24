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
    [Authorize(Roles = "Produk")]
    public class ProdukController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IProdukRepository _produkRepository;
        private readonly IPrincipalRepository _principalRepository;
        private readonly IKategoriRepository _kategoriRepository;
        private readonly ISatuanRepository _satuanRepository;
        private readonly IDiskonRepository _diskonRepository;
        private readonly IPenggunaRepository _penggunaRepository;

        public ProdukController(
            ApplicationDbContext applicationDbContext,
            IProdukRepository produkRepository,
            IPrincipalRepository principalRepository,
            IKategoriRepository kategoriRepository,
            ISatuanRepository satuanRepository,
            IDiskonRepository diskonRepository,
            IPenggunaRepository penggunaRepository
        )
        {
            _applicationDbContext = applicationDbContext;
            _produkRepository = produkRepository;
            _principalRepository = principalRepository;
            _kategoriRepository = kategoriRepository;
            _satuanRepository = satuanRepository;
            _diskonRepository = diskonRepository;
            _penggunaRepository = penggunaRepository;
        }
        [Authorize(Roles = "IndexProduk")]
        public IActionResult Index()
        {
            var data = _produkRepository.GetAllProduk();
            return View(data);
        }

        [Authorize(Roles = "CreateProduk")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ViewResult> CreateProduk()
        {
            ViewBag.Principal = new SelectList(await _principalRepository.GetPrincipals(), "PrincipalId", "NamaPrincipal", SortOrder.Ascending);
            ViewBag.Kategori = new SelectList(await _kategoriRepository.GetKategoris(), "KategoriId", "NamaKategori", SortOrder.Ascending);
            ViewBag.Satuan = new SelectList(await _satuanRepository.GetSatuans(), "SatuanId", "NamaSatuan", SortOrder.Ascending);
            ViewBag.Diskon = new SelectList(await _diskonRepository.GetDiskons(), "DiskonId", "Nilai", SortOrder.Ascending);

            var produk = new ProdukViewModel();

            var lastCode = _produkRepository.GetAllProduk().OrderByDescending(c => c.KodeProduk).FirstOrDefault();

            if (lastCode == null)
            {
                produk.KodeProduk = "PRD0001";
            }
            else
            {
                produk.KodeProduk = "PRD" + (Convert.ToInt32(lastCode.KodeProduk.Substring(3, lastCode.KodeProduk.Length - 3)) + 1).ToString("D4");
            }

            return View(produk);
        }

        [Authorize(Roles = "CreateProduk")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateProduk(ProdukViewModel vm)
        {
            ViewBag.Principal = new SelectList(await _principalRepository.GetPrincipals(), "PrincipalId", "NamaPrincipal", SortOrder.Ascending);
            ViewBag.Kategori = new SelectList(await _kategoriRepository.GetKategoris(), "KategoriId", "NamaKategori", SortOrder.Ascending);
            ViewBag.Satuan = new SelectList(await _satuanRepository.GetSatuans(), "SatuanId", "NamaSatuan", SortOrder.Ascending);
            ViewBag.Diskon = new SelectList(await _diskonRepository.GetDiskons(), "DiskonId", "Nilai", SortOrder.Ascending);

            var lastCode = _produkRepository.GetAllProduk().OrderByDescending(c => c.KodeProduk).FirstOrDefault();

            if (lastCode == null)
            {
                vm.KodeProduk = "PRD0001";
            }
            else
            {
                vm.KodeProduk = "PRD" + (Convert.ToInt32(lastCode.KodeProduk.Substring(3, lastCode.KodeProduk.Length - 3)) + 1).ToString("D4");
            }

            var getUser = _penggunaRepository.GetAllUserLogin().Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            if (ModelState.IsValid)
            {
                var Produk = new Produk
                {
                    CreateDateTime = DateTime.Now,
                    CreateBy = new Guid(getUser.Id),
                    ProdukId = vm.ProdukId,
                    KodeProduk = vm.KodeProduk,
                    NamaProduk = vm.NamaProduk,
                    PrincipalId = vm.PrincipalId,
                    KategoriId = vm.KategoriId,
                    JumlahStok = vm.JumlahStok,
                    SatuanId = vm.SatuanId,
                    HargaBeli = vm.HargaBeli,
                    HargaJual = vm.HargaJual,
                    Cogs = vm.Cogs,
                    DiskonId = vm.DiskonId,
                    Catatan = vm.Catatan,
                };

                var resultProduk = _produkRepository.GetAllProduk().Where(c => c.NamaProduk == vm.NamaProduk && c.KategoriId == vm.KategoriId).FirstOrDefault();

                if (resultProduk == null)
                {
                    _produkRepository.Tambah(Produk);
                    ViewBag.Principal = new SelectList(await _principalRepository.GetPrincipals(), "PrincipalId", "NamaPrincipal", SortOrder.Ascending);
                    ViewBag.Kategori = new SelectList(await _kategoriRepository.GetKategoris(), "KategoriId", "NamaKategori", SortOrder.Ascending);
                    ViewBag.Satuan = new SelectList(await _satuanRepository.GetSatuans(), "SatuanId", "NamaSatuan", SortOrder.Ascending);
                    ViewBag.Diskon = new SelectList(await _diskonRepository.GetDiskons(), "DiskonId", "Nilai", SortOrder.Ascending);
                    TempData["SuccessMessage"] = "Produk " + vm.NamaProduk + " Berhasil Disimpan";
                    return RedirectToAction("Index", "Produk");
                }
                else
                {
                    TempData["WarningMessage"] = "Produk " + vm.NamaProduk + " sudah ada !!!";
                    return View(vm);
                }
            }
            ViewBag.Principal = new SelectList(await _principalRepository.GetPrincipals(), "PrincipalId", "NamaPrincipal", SortOrder.Ascending);
            ViewBag.Kategori = new SelectList(await _kategoriRepository.GetKategoris(), "KategoriId", "NamaKategori", SortOrder.Ascending);
            ViewBag.Satuan = new SelectList(await _satuanRepository.GetSatuans(), "SatuanId", "NamaSatuan", SortOrder.Ascending);
            ViewBag.Diskon = new SelectList(await _diskonRepository.GetDiskons(), "DiskonId", "Nilai", SortOrder.Ascending);
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "DetailProduk")]
        [AllowAnonymous]
        public async Task<IActionResult> DetailProduk(Guid Id)
        {
            ViewBag.Principal = new SelectList(await _principalRepository.GetPrincipals(), "PrincipalId", "NamaPrincipal", SortOrder.Ascending);
            ViewBag.Kategori = new SelectList(await _kategoriRepository.GetKategoris(), "KategoriId", "NamaKategori", SortOrder.Ascending);
            ViewBag.Satuan = new SelectList(await _satuanRepository.GetSatuans(), "SatuanId", "NamaSatuan", SortOrder.Ascending);
            ViewBag.Diskon = new SelectList(await _diskonRepository.GetDiskons(), "DiskonId", "Nilai", SortOrder.Ascending);

            var produk = await _produkRepository.GetProdukById(Id);

            if (produk == null)
            {
                Response.StatusCode = 404;
                return View("ProdukNotFound", Id);
            }

            ProdukViewModel viewModel = new ProdukViewModel
            {
                ProdukId = produk.ProdukId,
                KodeProduk = produk.KodeProduk,
                NamaProduk = produk.NamaProduk,
                PrincipalId = produk.PrincipalId,
                KategoriId = produk.KategoriId,
                JumlahStok = produk.JumlahStok,
                SatuanId = produk.SatuanId,
                HargaBeli = Math.Truncate(produk.HargaBeli),
                HargaJual = Math.Truncate(produk.HargaJual),
                Cogs = Math.Truncate(produk.Cogs),
                DiskonId = produk.DiskonId,
                Catatan = produk.Catatan,
            };
            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "DetailProduk")]
        [AllowAnonymous]
        public async Task<IActionResult> DetailProduk(ProdukViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Produk Produk = await _produkRepository.GetProdukByIdNoTracking(viewModel.ProdukId);

                var getUser = _penggunaRepository.GetAllUserLogin().Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
                var check = _produkRepository.GetAllProduk().Where(d => d.KodeProduk == viewModel.KodeProduk).FirstOrDefault();

                if (check != null)
                {
                    Produk.UpdateDateTime = DateTime.Now;
                    Produk.UpdateBy = new Guid(getUser.Id);
                    Produk.KodeProduk = viewModel.KodeProduk;
                    Produk.NamaProduk = viewModel.NamaProduk;
                    Produk.PrincipalId = viewModel.PrincipalId;
                    Produk.KategoriId = viewModel.KategoriId;
                    Produk.JumlahStok = viewModel.JumlahStok;
                    Produk.SatuanId = viewModel.SatuanId;
                    Produk.HargaBeli = viewModel.HargaBeli;
                    Produk.HargaJual = viewModel.HargaJual;
                    Produk.Cogs = viewModel.Cogs;
                    Produk.DiskonId = viewModel.DiskonId;
                    Produk.Catatan = viewModel.Catatan;

                    _produkRepository.Update(Produk);
                    _applicationDbContext.SaveChanges();

                    TempData["SuccessMessage"] = "Produk " + viewModel.NamaProduk + " Berhasil Diubah";
                    return RedirectToAction("Index", "Produk");
                }
                else
                {
                    TempData["WarningMessage"] = "Produk " + viewModel.NamaProduk + " sudah ada !!!";
                    ViewBag.Principal = new SelectList(await _principalRepository.GetPrincipals(), "PrincipalId", "NamaPrincipal", SortOrder.Ascending);
                    ViewBag.Kategori = new SelectList(await _kategoriRepository.GetKategoris(), "KategoriId", "NamaKategori", SortOrder.Ascending);
                    ViewBag.Satuan = new SelectList(await _satuanRepository.GetSatuans(), "SatuanId", "NamaSatuan", SortOrder.Ascending);
                    ViewBag.Diskon = new SelectList(await _diskonRepository.GetDiskons(), "DiskonId", "Nilai", SortOrder.Ascending);
                    return View(viewModel);
                }
            }
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "DeleteProduk")]
        [AllowAnonymous]
        //[Authorize]
        public async Task<IActionResult> DeleteProduk(Guid Id)
        {
            var produk = await _produkRepository.GetProdukById(Id);
            if (produk == null)
            {
                Response.StatusCode = 404;
                return View("ProdukNotFound", Id);
            }

            DetailProdukViewModel vm = new DetailProdukViewModel
            {
                ProdukId = produk.ProdukId,
                KodeProduk = produk.KodeProduk,
                NamaProduk = produk.NamaProduk
            };
            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = "DeleteProduk")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteProduk(DetailProdukViewModel vm)
        {
            //Hapus Data
            var Produks = _applicationDbContext.Produks.FirstOrDefault(x => x.ProdukId == vm.ProdukId);
            _applicationDbContext.Attach(Produks);
            _applicationDbContext.Entry(Produks).State = EntityState.Deleted;
            _applicationDbContext.SaveChanges();

            TempData["SuccessMessage"] = "Produk " + vm.NamaProduk + " Berhasil Dihapus";

            return RedirectToAction("Index", "Produk");
        }
    }
}
