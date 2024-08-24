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
    [Authorize(Roles = "LevelPengguna")]
    public class LevelPenggunaController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly ILevelPenggunaRepository _levelPenggunaRepository;
        private readonly IPenggunaRepository _penggunaRepository;

        public LevelPenggunaController(
            ApplicationDbContext applicationDbContext,
            ILevelPenggunaRepository LevelPenggunaRepository,
            IPenggunaRepository penggunaRepository
        )
        {
            _applicationDbContext = applicationDbContext;
            _levelPenggunaRepository = LevelPenggunaRepository;
            _penggunaRepository = penggunaRepository;
        }
        [Authorize(Roles = "IndexLevelPengguna")]
        public IActionResult Index()
        {
            var data = _levelPenggunaRepository.GetAllLevelPengguna();
            return View(data);
        }

        [Authorize(Roles = "CreateLevelPengguna")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ViewResult> CreateLevelPengguna()
        {
            var LevelPengguna = new LevelPenggunaViewModel();
            var dateNow = DateTimeOffset.Now;
            var setDateNow = DateTimeOffset.Now.ToString("yyMMdd");

            var lastCode = _levelPenggunaRepository.GetAllLevelPengguna().Where(d => d.CreateDateTime.ToString("yyMMdd") == dateNow.ToString("yyMMdd")).OrderByDescending(k => k.KodeLevel).FirstOrDefault();
            if (lastCode == null)
            {
                LevelPengguna.KodeLevel = "LVL" + setDateNow + "0001";
            }
            else
            {
                var lastCodeTrim = lastCode.KodeLevel.Substring(3, 6);

                if (lastCodeTrim != setDateNow)
                {
                    LevelPengguna.KodeLevel = "LVL" + setDateNow + "0001";
                }
                else
                {
                    LevelPengguna.KodeLevel = "LVL" + setDateNow + (Convert.ToInt32(lastCode.KodeLevel.Substring(9, lastCode.KodeLevel.Length - 9)) + 1).ToString("D4");
                }
            }

            return View(LevelPengguna);
        }

        [Authorize(Roles = "CreateLevelPengguna")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateLevelPengguna(LevelPenggunaViewModel vm)
        {
            var dateNow = DateTimeOffset.Now;
            var setDateNow = DateTimeOffset.Now.ToString("yyMMdd");

            var lastCode = _levelPenggunaRepository.GetAllLevelPengguna().Where(d => d.CreateDateTime.ToString("yyMMdd") == dateNow.ToString("yyMMdd")).OrderByDescending(k => k.KodeLevel).FirstOrDefault();
            if (lastCode == null)
            {
                vm.KodeLevel = "LVL" + setDateNow + "0001";
            }
            else
            {
                var lastCodeTrim = lastCode.KodeLevel.Substring(3, 6);

                if (lastCodeTrim != setDateNow)
                {
                    vm.KodeLevel = "LVL" + setDateNow + "0001";
                }
                else
                {
                    vm.KodeLevel = "LVL" + setDateNow + (Convert.ToInt32(lastCode.KodeLevel.Substring(9, lastCode.KodeLevel.Length - 9)) + 1).ToString("D4");
                }
            }

            if (ModelState.IsValid)
            {
                var level = new LevelPengguna
                {
                    CreateDateTime = DateTime.Now,
                    LevelId = vm.LevelId,
                    KodeLevel = vm.KodeLevel,
                    NamaLevel = vm.NamaLevel,
                    Persentase = vm.Persentase,
                    Keterangan = vm.Keterangan,
                };

                var resultLevelPengguna = _levelPenggunaRepository.GetAllLevelPengguna().Where(c => c.NamaLevel == vm.NamaLevel).FirstOrDefault();

                if (resultLevelPengguna == null)
                {
                    _levelPenggunaRepository.Tambah(level);
                    TempData["SuccessMessage"] = "LevelPengguna " + vm.NamaLevel + " Berhasil Disimpan";
                    return RedirectToAction("Index", "LevelPengguna");
                }
                else
                {
                    TempData["WarningMessage"] = "LevelPengguna " + vm.NamaLevel + " sudah ada !!!";
                    return View(vm);
                }
            }
            return View();
        }

        [Authorize(Roles = "DetailLevelPengguna")]
        [AllowAnonymous]
        public async Task<IActionResult> DetailLevelPengguna(Guid Id)
        {
            var level = await _levelPenggunaRepository.GetLevelPenggunaById(Id);

            if (level == null)
            {
                Response.StatusCode = 404;
                return View("LevelPenggunaNotFound", Id);
            }

            DetailLevelPenggunaViewModel viewModel = new DetailLevelPenggunaViewModel
            {
                LevelId = level.LevelId,
                KodeLevel = level.KodeLevel,
                NamaLevel = level.NamaLevel,
                Persentase = level.Persentase,
                Keterangan = level.Keterangan,
            };
            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "DetailLevelPengguna")]
        [AllowAnonymous]
        public async Task<IActionResult> DetailLevelPengguna(DetailLevelPenggunaViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                LevelPengguna level = await _levelPenggunaRepository.GetLevelPenggunaByIdNoTracking(viewModel.LevelId);

                var check = _levelPenggunaRepository.GetAllLevelPengguna().Where(d => d.KodeLevel == viewModel.KodeLevel).FirstOrDefault();

                if (check != null)
                {
                    level.UpdateDateTime = DateTime.Now;
                    level.KodeLevel = viewModel.KodeLevel;
                    level.NamaLevel = viewModel.NamaLevel;
                    level.Persentase = viewModel.Persentase;
                    level.Keterangan = viewModel.Keterangan;

                    _levelPenggunaRepository.Update(level);
                    _applicationDbContext.SaveChanges();

                    TempData["SuccessMessage"] = "Level " + viewModel.NamaLevel + " Berhasil Diubah";
                    return RedirectToAction("Index", "LevelPengguna");
                }
                else
                {
                    TempData["WarningMessage"] = "Level " + viewModel.NamaLevel + " sudah ada !!!";
                    return View(viewModel);
                }
            }
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "DeleteLevelPengguna")]
        [AllowAnonymous]
        //[Authorize]
        public async Task<IActionResult> DeleteLevelPengguna(Guid Id)
        {
            var level = await _levelPenggunaRepository.GetLevelPenggunaById(Id);
            if (level == null)
            {
                Response.StatusCode = 404;
                return View("LevelPenggunaNotFound", Id);
            }

            DetailLevelPenggunaViewModel vm = new DetailLevelPenggunaViewModel
            {
                LevelId = level.LevelId,
                KodeLevel = level.KodeLevel,
                NamaLevel = level.NamaLevel,
                Persentase = level.Persentase,
                Keterangan = level.Keterangan,
        };
            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = "DeleteLevelPengguna")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteLevelPengguna(DetailLevelPenggunaViewModel vm)
        {
            //Cek Relasi Principal dengan Produk
            var pengguna = _penggunaRepository.GetAllPengguna().Where(p => p.LevelId == vm.LevelId).FirstOrDefault();
            if (pengguna == null)
            {
                //Hapus Data
                var level = _applicationDbContext.LevelPenggunas.FirstOrDefault(x => x.LevelId == vm.LevelId);
                _applicationDbContext.Attach(level);
                _applicationDbContext.Entry(level).State = EntityState.Deleted;
                _applicationDbContext.SaveChanges();

                TempData["SuccessMessage"] = "Level Pengguna " + vm.NamaLevel + " Berhasil Dihapus";

                return RedirectToAction("Index", "Level Pengguna");
            }
            else
            {
                TempData["WarningMessage"] = "Level Pengguna " + vm.NamaLevel + " terelasi dengan pengguna " + pengguna.NamaLengkap;
                return View(vm);
            }            
        }
    }
}
