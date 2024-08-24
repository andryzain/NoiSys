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
    [Authorize(Roles = "MetodePembayaran")]
    public class MetodePembayaranController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMetodePembayaranRepository _MetodePembayaranRepository;

        public MetodePembayaranController(
            ApplicationDbContext applicationDbContext,
            IMetodePembayaranRepository MetodePembayaranRepository
        )
        {
            _applicationDbContext = applicationDbContext;
            _MetodePembayaranRepository = MetodePembayaranRepository;
        }
        [Authorize(Roles = "IndexMetodePembayaran")]
        public IActionResult Index()
        {
            var data = _MetodePembayaranRepository.GetAllMetodePembayaran();
            return View(data);
        }

        [Authorize(Roles = "CreateMetodePembayaran")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ViewResult> CreateMetodePembayaran()
        {
            var MetodePembayaran = new MetodePembayaranViewModel();
            var dateNow = DateTimeOffset.Now;
            var setDateNow = DateTimeOffset.Now.ToString("yyMMdd");

            var lastCode = _MetodePembayaranRepository.GetAllMetodePembayaran().Where(d => d.CreateDateTime.ToString("yyMMdd") == dateNow.ToString("yyMMdd")).OrderByDescending(k => k.KodeMetodePembayaran).FirstOrDefault();
            if (lastCode == null)
            {
                MetodePembayaran.KodeMetodePembayaran = "TOP" + setDateNow + "0001";
            }
            else
            {
                var lastCodeTrim = lastCode.KodeMetodePembayaran.Substring(3, 6);

                if (lastCodeTrim != setDateNow)
                {
                    MetodePembayaran.KodeMetodePembayaran = "TOP" + setDateNow + "0001";
                }
                else
                {
                    MetodePembayaran.KodeMetodePembayaran = "TOP" + setDateNow + (Convert.ToInt32(lastCode.KodeMetodePembayaran.Substring(9, lastCode.KodeMetodePembayaran.Length - 9)) + 1).ToString("D4");
                }
            }

            return View(MetodePembayaran);
        }

        [Authorize(Roles = "CreateMetodePembayaran")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateMetodePembayaran(MetodePembayaranViewModel vm)
        {
            var dateNow = DateTimeOffset.Now;
            var setDateNow = DateTimeOffset.Now.ToString("yyMMdd");

            var lastCode = _MetodePembayaranRepository.GetAllMetodePembayaran().Where(d => d.CreateDateTime.ToString("yyMMdd") == dateNow.ToString("yyMMdd")).OrderByDescending(k => k.KodeMetodePembayaran).FirstOrDefault();
            if (lastCode == null)
            {
                vm.KodeMetodePembayaran = "TOP" + setDateNow + "0001";
            }
            else
            {
                var lastCodeTrim = lastCode.KodeMetodePembayaran.Substring(3, 6);

                if (lastCodeTrim != setDateNow)
                {
                    vm.KodeMetodePembayaran = "TOP" + setDateNow + "0001";
                }
                else
                {
                    vm.KodeMetodePembayaran = "TOP" + setDateNow + (Convert.ToInt32(lastCode.KodeMetodePembayaran.Substring(9, lastCode.KodeMetodePembayaran.Length - 9)) + 1).ToString("D4");
                }
            }

            if (ModelState.IsValid)
            {
                var MetodePembayaran = new MetodePembayaran
                {
                    CreateDateTime = DateTime.Now,
                    MetodePembayaranId = vm.MetodePembayaranId,
                    KodeMetodePembayaran = vm.KodeMetodePembayaran,
                    NamaMetodePembayaran = vm.NamaMetodePembayaran
                };

                var resultMetodePembayaran = _MetodePembayaranRepository.GetAllMetodePembayaran().Where(c => c.NamaMetodePembayaran == vm.NamaMetodePembayaran).FirstOrDefault();

                if (resultMetodePembayaran == null)
                {
                    _MetodePembayaranRepository.Tambah(MetodePembayaran);
                    TempData["SuccessMessage"] = "MetodePembayaran " + vm.NamaMetodePembayaran + " Berhasil Disimpan";
                    return RedirectToAction("Index", "MetodePembayaran");
                }
                else
                {
                    TempData["WarningMessage"] = "MetodePembayaran " + vm.NamaMetodePembayaran + " sudah ada !!!";
                    return View(vm);
                }
            }
            return View();
        }

        [Authorize(Roles = "DetailMetodePembayaran")]
        [AllowAnonymous]
        public async Task<IActionResult> DetailMetodePembayaran(Guid Id)
        {
            var MetodePembayaran = await _MetodePembayaranRepository.GetMetodePembayaranById(Id);

            if (MetodePembayaran == null)
            {
                Response.StatusCode = 404;
                return View("MetodePembayaranNotFound", Id);
            }

            DetailMetodePembayaranViewModel viewModel = new DetailMetodePembayaranViewModel
            {
                MetodePembayaranId = MetodePembayaran.MetodePembayaranId,
                KodeMetodePembayaran = MetodePembayaran.KodeMetodePembayaran,
                NamaMetodePembayaran = MetodePembayaran.NamaMetodePembayaran
            };
            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "DetailMetodePembayaran")]
        [AllowAnonymous]
        public async Task<IActionResult> DetailMetodePembayaran(DetailMetodePembayaranViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                MetodePembayaran MetodePembayaran = await _MetodePembayaranRepository.GetMetodePembayaranByIdNoTracking(viewModel.MetodePembayaranId);

                var check = _MetodePembayaranRepository.GetAllMetodePembayaran().Where(d => d.KodeMetodePembayaran == viewModel.KodeMetodePembayaran).FirstOrDefault();

                if (check != null)
                {
                    MetodePembayaran.UpdateDateTime = DateTime.Now;
                    MetodePembayaran.KodeMetodePembayaran = viewModel.KodeMetodePembayaran;
                    MetodePembayaran.NamaMetodePembayaran = viewModel.NamaMetodePembayaran;

                    _MetodePembayaranRepository.Update(MetodePembayaran);
                    _applicationDbContext.SaveChanges();

                    TempData["SuccessMessage"] = "MetodePembayaran " + viewModel.NamaMetodePembayaran + " Berhasil Diubah";
                    return RedirectToAction("Index", "MetodePembayaran");
                }
                else
                {
                    TempData["WarningMessage"] = "MetodePembayaran " + viewModel.NamaMetodePembayaran + " sudah ada !!!";
                    return View(viewModel);
                }
            }
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "DeleteMetodePembayaran")]
        [AllowAnonymous]
        //[Authorize]
        public async Task<IActionResult> DeleteMetodePembayaran(Guid Id)
        {
            var MetodePembayaran = await _MetodePembayaranRepository.GetMetodePembayaranById(Id);
            if (MetodePembayaran == null)
            {
                Response.StatusCode = 404;
                return View("MetodePembayaranNotFound", Id);
            }

            DetailMetodePembayaranViewModel vm = new DetailMetodePembayaranViewModel
            {
                MetodePembayaranId = MetodePembayaran.MetodePembayaranId,
                KodeMetodePembayaran = MetodePembayaran.KodeMetodePembayaran,
                NamaMetodePembayaran = MetodePembayaran.NamaMetodePembayaran
            };
            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = "DeleteMetodePembayaran")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteMetodePembayaran(DetailMetodePembayaranViewModel vm)
        {
            //Hapus Data
            var MetodePembayarans = _applicationDbContext.MetodePembayarans.FirstOrDefault(x => x.MetodePembayaranId == vm.MetodePembayaranId);
            _applicationDbContext.Attach(MetodePembayarans);
            _applicationDbContext.Entry(MetodePembayarans).State = EntityState.Deleted;
            _applicationDbContext.SaveChanges();

            TempData["SuccessMessage"] = "MetodePembayaran " + vm.NamaMetodePembayaran + " Berhasil Dihapus";

            return RedirectToAction("Index", "MetodePembayaran");
        }
    }
}
