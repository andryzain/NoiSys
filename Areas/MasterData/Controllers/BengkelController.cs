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
    [Authorize(Roles = "Bengkel")]
    public class BengkelController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IBengkelRepository _bengkelRepository;
        private readonly IPenggunaRepository _penggunaRepository;
        private readonly IMekanikRepository _mekanikRepository;

        public BengkelController(
            IHostingEnvironment hostingEnvironment,
            ApplicationDbContext applicationDbContext,
            IBengkelRepository bengkelRepository,
            IPenggunaRepository penggunaRepository,
            IMekanikRepository mekanikRepository
        )
        {
            _hostingEnvironment = hostingEnvironment;
            _applicationDbContext = applicationDbContext;
            _bengkelRepository = bengkelRepository;
            _penggunaRepository = penggunaRepository;
            _mekanikRepository = mekanikRepository;
        }
        
        [Authorize(Roles = "IndexBengkel")]
        public IActionResult Index()
        {
            var data = _bengkelRepository.GetAllBengkel();
            return View(data);
        }

        [Authorize(Roles = "CreateBengkel")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ViewResult> CreateBengkel()
        {
            ViewBag.Pengguna = new SelectList(await _penggunaRepository.GetPenggunas(), "PenggunaId", "NamaLengkap", SortOrder.Ascending);

            var bengkel = new BengkelViewModel();
            var dateNow = DateTimeOffset.Now;
            var setDateNow = DateTimeOffset.Now.ToString("yyMMdd");

            var lastCode = _bengkelRepository.GetAllBengkel().Where(d => d.CreateDateTime.ToString("yyMMdd") == dateNow.ToString("yyMMdd")).OrderByDescending(k => k.KodeBengkel).FirstOrDefault();
            if (lastCode == null)
            {
                bengkel.KodeBengkel = "BKL" + setDateNow + "0001";
            }
            else
            {
                var lastCodeTrim = lastCode.KodeBengkel.Substring(3, 6);

                if (lastCodeTrim != setDateNow)
                {
                    bengkel.KodeBengkel = "BKL" + setDateNow + "0001";
                }
                else
                {
                    bengkel.KodeBengkel = "BKL" + setDateNow + (Convert.ToInt32(lastCode.KodeBengkel.Substring(9, lastCode.KodeBengkel.Length - 9)) + 1).ToString("D4");
                }
            }

            return View(bengkel);
        }

        [Authorize(Roles = "CreateBengkel")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateBengkel(BengkelViewModel vm)
        {
            ViewBag.Pengguna = new SelectList(await _penggunaRepository.GetPenggunas(), "PenggunaId", "NamaLengkap", SortOrder.Ascending);

            var dateNow = DateTimeOffset.Now;
            var setDateNow = DateTimeOffset.Now.ToString("yyMMdd");

            var lastCode = _bengkelRepository.GetAllBengkel().Where(d => d.CreateDateTime.ToString("yyMMdd") == dateNow.ToString("yyMMdd")).OrderByDescending(k => k.KodeBengkel).FirstOrDefault();
            if (lastCode == null)
            {
                vm.KodeBengkel = "BKL" + setDateNow + "0001";
            }
            else
            {
                var lastCodeTrim = lastCode.KodeBengkel.Substring(3, 6);

                if (lastCodeTrim != setDateNow)
                {
                    vm.KodeBengkel = "BKL" + setDateNow + "0001";
                }
                else
                {
                    vm.KodeBengkel = "BKL" + setDateNow + (Convert.ToInt32(lastCode.KodeBengkel.Substring(9, lastCode.KodeBengkel.Length - 9)) + 1).ToString("D4");
                }
            }

            if (ModelState.IsValid)
            {
                string uniqueFileName = ProcessUploadFile(vm);

                var bengkel = new Bengkel
                {
                    CreateDateTime = DateTime.Now,
                    BengkelId = vm.BengkelId,
                    KodeBengkel = vm.KodeBengkel,
                    NamaBengkel = vm.NamaBengkel,
                    PenanggungJawab = vm.PenanggungJawab,
                    NamaPemilik = vm.NamaPemilik,
                    Alamat = vm.Alamat,
                    NomorTelepon = vm.NomorTelepon,
                    Email = vm.Email,
                    Keterangan = vm.Keterangan,
                    Foto = uniqueFileName
                };

                var resultBengkel = _bengkelRepository.GetAllBengkel().Where(c => c.NamaBengkel == vm.NamaBengkel).FirstOrDefault();

                if (resultBengkel == null)
                {
                    _bengkelRepository.Tambah(bengkel);
                    TempData["SuccessMessage"] = "Bengkel " + vm.NamaBengkel + " Berhasil Disimpan";
                    return RedirectToAction("Index", "Bengkel");
                }
                else
                {
                    ViewBag.Pengguna = new SelectList(await _penggunaRepository.GetPenggunas(), "PenggunaId", "NamaLengkap", SortOrder.Ascending);
                    TempData["WarningMessage"] = "Bengkel " + vm.NamaBengkel + " sudah ada !!!";
                    return View(vm);
                }
            }
            ViewBag.Pengguna = new SelectList(await _penggunaRepository.GetPenggunas(), "PenggunaId", "NamaLengkap", SortOrder.Ascending);
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "DetailBengkel")]
        [AllowAnonymous]
        public async Task<IActionResult> DetailBengkel(Guid Id)
        {
            ViewBag.Pengguna = new SelectList(await _penggunaRepository.GetPenggunas(), "PenggunaId", "NamaLengkap", SortOrder.Ascending);

            var bengkel = await _bengkelRepository.GetBengkelById(Id);

            if (bengkel == null)
            {
                Response.StatusCode = 404;
                return View("BengkelNotFound", Id);
            }

            DetailBengkelViewModel viewModel = new DetailBengkelViewModel
            {
                BengkelId = bengkel.BengkelId,
                KodeBengkel = bengkel.KodeBengkel,
                NamaBengkel = bengkel.NamaBengkel,
                PenanggungJawab = bengkel.PenanggungJawab,
                NamaPemilik = bengkel.NamaPemilik,
                Alamat = bengkel.Alamat,
                NomorTelepon = bengkel.NomorTelepon,
                Email = bengkel.Email,
                Keterangan = bengkel.Keterangan,
                BengkelPhotoPath = bengkel.Foto,
            };
            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "DetailBengkel")]
        [AllowAnonymous]
        public async Task<IActionResult> DetailBengkel(DetailBengkelViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Pengguna = new SelectList(await _penggunaRepository.GetPenggunas(), "PenggunaId", "NamaLengkap", SortOrder.Ascending);

                Bengkel bengkel = await _bengkelRepository.GetBengkelByIdNoTracking(viewModel.BengkelId);

                var check = _bengkelRepository.GetAllBengkel().Where(d => d.KodeBengkel == viewModel.KodeBengkel).FirstOrDefault();

                if (check != null)
                {
                    bengkel.UpdateDateTime = DateTime.Now;
                    bengkel.KodeBengkel = viewModel.KodeBengkel;
                    bengkel.NamaBengkel = viewModel.NamaBengkel;
                    bengkel.PenanggungJawab = viewModel.PenanggungJawab;
                    bengkel.NamaPemilik = viewModel.NamaPemilik;
                    bengkel.Alamat = viewModel.Alamat;
                    bengkel.NomorTelepon = viewModel.NomorTelepon;
                    bengkel.Email = viewModel.Email;
                    bengkel.Keterangan = viewModel.Keterangan;

                    if (viewModel.Foto != null)
                    {
                        if (viewModel.BengkelPhotoPath != null)
                        {
                            string filePath = Path.Combine(_hostingEnvironment.WebRootPath,
                                "FotoBengkel", viewModel.BengkelPhotoPath);
                            System.IO.File.Delete(filePath);
                        }
                        bengkel.Foto = ProcessUploadFile(viewModel);
                    }

                    _bengkelRepository.Update(bengkel);
                    _applicationDbContext.SaveChanges();

                    TempData["SuccessMessage"] = "Bengkel " + viewModel.NamaBengkel + " Berhasil Diubah";
                    return RedirectToAction("Index", "Bengkel");
                }
                else
                {
                    ViewBag.Pengguna = new SelectList(await _penggunaRepository.GetPenggunas(), "PenggunaId", "NamaLengkap", SortOrder.Ascending);
                    TempData["WarningMessage"] = "Bengkel " + viewModel.NamaBengkel + " sudah ada !!!";
                    return View(viewModel);
                }
            }
            ViewBag.Pengguna = new SelectList(await _penggunaRepository.GetPenggunas(), "PenggunaId", "NamaLengkap", SortOrder.Ascending);
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "DeleteBengkel")]
        [AllowAnonymous]
        //[Authorize]
        public async Task<IActionResult> DeleteBengkel(Guid Id)
        {
            var bengkel = await _bengkelRepository.GetBengkelById(Id);
            if (bengkel == null)
            {
                Response.StatusCode = 404;
                return View("BengkelNotFound", Id);
            }

            DetailBengkelViewModel vm = new DetailBengkelViewModel
            {
                BengkelId = bengkel.BengkelId,
                KodeBengkel = bengkel.KodeBengkel,
                NamaBengkel = bengkel.NamaBengkel,
                BengkelPhotoPath = bengkel.Foto,
            };
            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = "DeleteBengkel")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteBengkel(DetailBengkelViewModel vm)
        {
            //Cek Relasi Bengkel dengan Mekanik
            var mekanik = _mekanikRepository.GetAllMekanik().Where(p => p.BengkelId == vm.BengkelId).FirstOrDefault();
            if (mekanik == null)
            {
                //Hapus Data
                var bengkels = _applicationDbContext.Bengkels.FirstOrDefault(x => x.BengkelId == vm.BengkelId);
                _applicationDbContext.Attach(bengkels);
                _applicationDbContext.Entry(bengkels).State = EntityState.Deleted;
                _applicationDbContext.SaveChanges();

                if (vm.BengkelPhotoPath != null)
                {
                    string filePath = Path.Combine(_hostingEnvironment.WebRootPath,
                        "FotoPengguna", vm.BengkelPhotoPath);
                    System.IO.File.Delete(filePath);
                }

                bengkels.Foto = ProcessUploadFile(vm);

                TempData["SuccessMessage"] = "Bengkel " + vm.NamaBengkel + " Berhasil Dihapus";
                return RedirectToAction("Index", "Bengkel");
            }
            else
            {
                TempData["WarningMessage"] = "Bengkel " + vm.NamaBengkel + " terelasi dengan mekanik " + mekanik.NamaMekanik;
                return View(vm);
            }            
        }

        private string ProcessUploadFile(BengkelViewModel model)
        {
            string uniqueFileName = null;
            if (model.Foto != null)
            {
                string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "FotoBengkel");
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.NamaBengkel + "_" + model.Foto.FileName;
                string filePath = Path.Combine(uploadFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Foto.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }
    }
}
