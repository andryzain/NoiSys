using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.Language.CodeGeneration;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NoiSys.Areas.Identity.Data;
using NoiSys.Areas.MasterData.Models;
using NoiSys.Areas.MasterData.Repository;
using NoiSys.Areas.MasterData.ViewModels;
using NoiSys.Data;
using NoiSys.Repository;
using System.Drawing;
using System.Security.Principal;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace NoiSys.Areas.MasterData.Controllers
{
    [Area("MasterData")]
    [Route("MasterData/[Controller]/[Action]")]
    [Authorize(Roles = "Pengguna")]
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IPenggunaRepository _penggunaRepository;
        private readonly ILevelPenggunaRepository _levelPenggunaRepository;

        public UserController(
            IUnitOfWork unitOfWork,
            IHostingEnvironment hostingEnvironment,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext applicationDbContext,
            IPenggunaRepository penggunaRepository,
            ILevelPenggunaRepository levelPenggunaRepository
        )
        {
            _unitOfWork = unitOfWork;
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
            _signInManager = signInManager;
            _applicationDbContext = applicationDbContext;
            _penggunaRepository = penggunaRepository;
            _levelPenggunaRepository = levelPenggunaRepository;
        }

        [Authorize(Roles = "IndexPengguna")]
        public IActionResult Index()
        {
            var data = _penggunaRepository.GetAllPengguna();
            return View(data);
        }

        [Authorize(Roles = "CreatePengguna")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ViewResult> CreateUser()
        {
            ViewBag.LevelPengguna = new SelectList(await _levelPenggunaRepository.GetLevelPenggunas(), "LevelId", "NamaLevel", SortOrder.Ascending);

            var user = new PenggunaViewModel();
            var dateNow = DateTimeOffset.Now;
            var setDateNow = DateTimeOffset.Now.ToString("yyMMdd");

            var lastCode = _penggunaRepository.GetAllPengguna().Where(d => d.CreateDateTime.ToString("yyMMdd") == dateNow.ToString("yyMMdd")).OrderByDescending(k => k.KodePengguna).FirstOrDefault();
            if (lastCode == null)
            {
                user.KodePengguna = "USR" + setDateNow + "0001";
            }
            else
            {
                var lastCodeTrim = lastCode.KodePengguna.Substring(3, 6);

                if (lastCodeTrim != setDateNow)
                {
                    user.KodePengguna = "USR" + setDateNow + "0001";
                }
                else
                {
                    user.KodePengguna = "USR" + setDateNow + (Convert.ToInt32(lastCode.KodePengguna.Substring(9, lastCode.KodePengguna.Length - 9)) + 1).ToString("D4");
                }
            }

            return View(user);
        }

        [Authorize(Roles = "CreatePengguna")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateUser(PenggunaViewModel vm)
        {
            ViewBag.LevelPengguna = new SelectList(await _levelPenggunaRepository.GetLevelPenggunas(), "LevelId", "NamaLevel", SortOrder.Ascending);

            var dateNow = DateTimeOffset.Now;
            var setDateNow = DateTimeOffset.Now.ToString("yyMMdd");

            var lastCode = _penggunaRepository.GetAllPengguna().Where(d => d.CreateDateTime.ToString("yyMMdd") == dateNow.ToString("yyMMdd")).OrderByDescending(k => k.KodePengguna).FirstOrDefault();
            if (lastCode == null)
            {
                vm.KodePengguna = "USR" + setDateNow + "0001";
            }
            else
            {
                var lastCodeTrim = lastCode.KodePengguna.Substring(3, 6);

                if (lastCodeTrim != setDateNow)
                {
                    vm.KodePengguna = "USR" + setDateNow + "0001";
                }
                else
                {
                    vm.KodePengguna = "USR" + setDateNow + (Convert.ToInt32(lastCode.KodePengguna.Substring(9, lastCode.KodePengguna.Length - 9)) + 1).ToString("D4");
                }
            }            

            if (ModelState.IsValid)
            {
                string uniqueFileName = ProcessUploadFile(vm);

                var userLogin = new ApplicationUser
                {                    
                    KodePengguna = vm.KodePengguna,
                    NamaLengkap = vm.NamaLengkap,
                    LevelId = vm.LevelId,
                    Email = vm.Email,
                    UserName = vm.Email
                };

                var user = new Pengguna
                {
                    CreateDateTime = DateTime.Now,
                    PenggunaId = vm.PenggunaId,
                    KodePengguna = vm.KodePengguna,
                    NamaLengkap = vm.NamaLengkap,
                    NomorIdentitas = vm.NomorIdentitas,
                    LevelId = vm.LevelId,
                    TempatLahir = vm.TempatLahir,
                    TanggalLahir = vm.TanggalLahir,
                    JenisKelamin = vm.JenisKelamin,
                    AlamatLengkap = vm.AlamatLengkap,
                    NomorHandphone = vm.NomorHandphone,
                    Email = vm.Email,
                    Foto = uniqueFileName
                };

                var passTglLahir = vm.TanggalLahir.ToString("ddMMMyyyy");

                var resultPengguna = _penggunaRepository.GetAllPengguna().Where(c => c.NamaLengkap == vm.NamaLengkap).FirstOrDefault();
                var resultLogin = await _userManager.CreateAsync(userLogin, passTglLahir);

                if (resultPengguna == null)
                {
                    if (resultLogin.Succeeded)
                    {
                        _penggunaRepository.Tambah(user);
                        TempData["SuccessMessage"] = "Akun " + vm.NamaLengkap + " Berhasil Disimpan";
                        return RedirectToAction("Index", "User");
                    }
                    else
                    {
                        TempData["WarningMessage"] = "Akun " + vm.NamaLengkap + " sudah ada !!!";
                        return View(vm);
                    }
                }
                else
                {
                    ViewBag.LevelPengguna = new SelectList(await _levelPenggunaRepository.GetLevelPenggunas(), "LevelId", "NamaLevel", SortOrder.Ascending);
                    TempData["WarningMessage"] = "Akun " + vm.NamaLengkap + " sudah ada !!!";
                    return View(vm);
                }

            }
            ViewBag.LevelPengguna = new SelectList(await _levelPenggunaRepository.GetLevelPenggunas(), "LevelId", "NamaLevel", SortOrder.Ascending);
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "DetailPengguna")]
        [AllowAnonymous]
        public async Task<IActionResult> DetailUser(Guid Id)
        {
            ViewBag.LevelPengguna = new SelectList(await _levelPenggunaRepository.GetLevelPenggunas(), "LevelId", "NamaLevel", SortOrder.Ascending);

            var pengguna = await _penggunaRepository.GetPenggunaById(Id);            

            if (pengguna == null)
            {
                Response.StatusCode = 404;
                return View("PenggunaNotFound", Id);
            }

            DetailPenggunaViewModel viewModel = new DetailPenggunaViewModel
            {
                PenggunaId = pengguna.PenggunaId,
                KodePengguna = pengguna.KodePengguna,
                NamaLengkap = pengguna.NamaLengkap,
                NomorIdentitas = pengguna.NomorIdentitas,
                LevelId = pengguna.LevelId,
                TempatLahir = pengguna.TempatLahir,
                TanggalLahir = pengguna.TanggalLahir,
                JenisKelamin = pengguna.JenisKelamin,
                AlamatLengkap = pengguna.AlamatLengkap,
                NomorHandphone = pengguna.NomorHandphone,
                Email = pengguna.Email,
                PenggunaPhotoPath = pengguna.Foto
            };            
            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "DetailPengguna")]
        [AllowAnonymous]
        public async Task<IActionResult> DetailUser(DetailPenggunaViewModel viewModel)
        {
            ViewBag.LevelPengguna = new SelectList(await _levelPenggunaRepository.GetLevelPenggunas(), "LevelId", "NamaLevel", SortOrder.Ascending);

            if (ModelState.IsValid)
            {
                var pengguna = await _penggunaRepository.GetPenggunaByIdNoTracking(viewModel.PenggunaId);
                var userLoginLevel = await _userManager.FindByNameAsync(viewModel.Email);

                var check = _penggunaRepository.GetAllPengguna().Where(d => d.KodePengguna == viewModel.KodePengguna).FirstOrDefault();

                if (check != null)
                {
                    if (userLoginLevel != null)
                    { 
                        userLoginLevel.LevelId = viewModel.LevelId;

                        pengguna.UpdateDateTime = DateTime.Now;
                        pengguna.KodePengguna = viewModel.KodePengguna;
                        pengguna.NamaLengkap = viewModel.NamaLengkap;
                        pengguna.NomorIdentitas = viewModel.NomorIdentitas;
                        pengguna.LevelId = viewModel.LevelId;
                        pengguna.TempatLahir = viewModel.TempatLahir;
                        pengguna.TanggalLahir = viewModel.TanggalLahir;
                        pengguna.JenisKelamin = viewModel.JenisKelamin;
                        pengguna.AlamatLengkap = viewModel.AlamatLengkap;
                        pengguna.NomorHandphone = viewModel.NomorHandphone;
                        pengguna.Email = viewModel.Email;

                        if (viewModel.Foto != null)
                        {
                            if (viewModel.PenggunaPhotoPath != null)
                            {
                                string filePath = Path.Combine(_hostingEnvironment.WebRootPath,
                                    "FotoPengguna", viewModel.PenggunaPhotoPath);
                                System.IO.File.Delete(filePath);
                            }
                            pengguna.Foto = ProcessUploadFile(viewModel);
                        }

                        var result = await _userManager.UpdateAsync(userLoginLevel);
                        if (result.Succeeded)
                        {
                            _penggunaRepository.Update(pengguna);
                            _applicationDbContext.SaveChanges();

                            TempData["SuccessMessage"] = "Akun " + viewModel.NamaLengkap + " Berhasil Diubah";
                            return RedirectToAction("Index", "User");
                        }                        
                    }
                    else
                    {
                        TempData["WarningMessage"] = "Akun " + viewModel.NamaLengkap + " Level Pengguna tidak ditemukan !!!";
                        return View(viewModel);
                    }                                                           
                }
                else
                {
                    ViewBag.LevelPengguna = new SelectList(await _levelPenggunaRepository.GetLevelPenggunas(), "LevelId", "NamaLevel", SortOrder.Ascending);
                    TempData["WarningMessage"] = "Akun " + viewModel.NamaLengkap + "% sudah ada !!!";
                    return View(viewModel);
                }
            }
            ViewBag.LevelPengguna = new SelectList(await _levelPenggunaRepository.GetLevelPenggunas(), "LevelId", "NamaLevel", SortOrder.Ascending);
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "DeletePengguna")]
        [AllowAnonymous]
        //[Authorize]
        public async Task<IActionResult> DeleteUser(Guid Id)
        {
            var pengguna = await _penggunaRepository.GetPenggunaById(Id);
            if (pengguna == null)
            {
                Response.StatusCode = 404;
                return View("PenggunaNotFound", Id);
            }

            DetailPenggunaViewModel vm = new DetailPenggunaViewModel
            {
                PenggunaId = pengguna.PenggunaId,
                KodePengguna = pengguna.KodePengguna,
                NamaLengkap = pengguna.NamaLengkap,
                PenggunaPhotoPath = pengguna.Foto,
            };            
            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = "DeletePengguna")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteUser(DetailPenggunaViewModel vm)
        {
            //Hapus Akun Login
            var akunpengguna = _signInManager.UserManager.Users.FirstOrDefault(s => s.KodePengguna == vm.KodePengguna);
            _applicationDbContext.Attach(akunpengguna);
            _applicationDbContext.Entry(akunpengguna).State = EntityState.Deleted;
            _applicationDbContext.SaveChanges();

            //Hapus Data Profil
            var penggunas = _applicationDbContext.Penggunas.FirstOrDefault(x => x.PenggunaId == vm.PenggunaId);
            _applicationDbContext.Attach(penggunas);
            _applicationDbContext.Entry(penggunas).State = EntityState.Deleted;
            _applicationDbContext.SaveChanges();

            if (vm.PenggunaPhotoPath != null)
            {
                string filePath = Path.Combine(_hostingEnvironment.WebRootPath,
                    "FotoPengguna", vm.PenggunaPhotoPath);
                System.IO.File.Delete(filePath);
            }

            penggunas.Foto = ProcessUploadFile(vm);

            TempData["SuccessMessage"] = "Akun " + vm.NamaLengkap + " Berhasil Dihapus";

            return RedirectToAction("Index", "User");
        }

        private string ProcessUploadFile(PenggunaViewModel model)
        {
            string uniqueFileName = null;
            if (model.Foto != null)
            {
                string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "FotoPengguna");
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.NamaLengkap + "_" + model.Foto.FileName;
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
