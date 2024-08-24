using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoiSys.Areas.Identity.Data;
using NoiSys.Areas.MasterData.Models;
using NoiSys.Areas.MasterData.Repository;
using NoiSys.Areas.MasterData.ViewModels;
using NoiSys.Data;
using NoiSys.Repository;
using System.Data;

namespace NoiSys.Areas.MasterData.Controllers
{
    [Area("MasterData")]
    [Route("MasterData/[Controller]/[Action]")]
    [Authorize(Roles = "Principal")]
    public class PrincipalController : Controller
    {        
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IPrincipalRepository _principalRepository;
        private readonly IProdukRepository _produkRepository;

        public PrincipalController(            
            ApplicationDbContext applicationDbContext,
            IPrincipalRepository principalRepository,
            IProdukRepository produkRepository
        )
        {            
            _applicationDbContext = applicationDbContext;
            _principalRepository = principalRepository;
            _produkRepository = produkRepository;
        }
        [Authorize(Roles = "IndexPrincipal")]
        public IActionResult Index()
        {
            var data = _principalRepository.GetAllPrincipal();
            return View(data);
        }

        [Authorize(Roles = "CreatePrincipal")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ViewResult> CreatePrincipal()
        {
            var principal = new PrincipalViewModel();
            var dateNow = DateTimeOffset.Now;
            var setDateNow = DateTimeOffset.Now.ToString("yyMMdd");

            var lastCode = _principalRepository.GetAllPrincipal().Where(d => d.CreateDateTime.ToString("yyMMdd") == dateNow.ToString("yyMMdd")).OrderByDescending(k => k.KodePrincipal).FirstOrDefault();
            if (lastCode == null)
            {
                principal.KodePrincipal = "PRC" + setDateNow + "0001";
            }
            else
            {
                var lastCodeTrim = lastCode.KodePrincipal.Substring(3, 6);

                if (lastCodeTrim != setDateNow)
                {
                    principal.KodePrincipal = "PRC" + setDateNow + "0001";
                }
                else
                {
                    principal.KodePrincipal = "PRC" + setDateNow + (Convert.ToInt32(lastCode.KodePrincipal.Substring(9, lastCode.KodePrincipal.Length - 9)) + 1).ToString("D4");
                }
            }

            return View(principal);
        }

        [Authorize(Roles = "CreatePrincipal")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreatePrincipal(PrincipalViewModel vm)
        {
            var dateNow = DateTimeOffset.Now;
            var setDateNow = DateTimeOffset.Now.ToString("yyMMdd");

            var lastCode = _principalRepository.GetAllPrincipal().Where(d => d.CreateDateTime.ToString("yyMMdd") == dateNow.ToString("yyMMdd")).OrderByDescending(k => k.KodePrincipal).FirstOrDefault();
            if (lastCode == null)
            {
                vm.KodePrincipal = "PRC" + setDateNow + "0001";
            }
            else
            {
                var lastCodeTrim = lastCode.KodePrincipal.Substring(3, 6);

                if (lastCodeTrim != setDateNow)
                {
                    vm.KodePrincipal = "PRC" + setDateNow + "0001";
                }
                else
                {
                    vm.KodePrincipal = "PRC" + setDateNow + (Convert.ToInt32(lastCode.KodePrincipal.Substring(9, lastCode.KodePrincipal.Length - 9)) + 1).ToString("D4");
                }
            }

            if (ModelState.IsValid)
            {
                var Principal = new Principal
                {
                    CreateDateTime = DateTime.Now,
                    PrincipalId = vm.PrincipalId,
                    KodePrincipal = vm.KodePrincipal,
                    NamaPrincipal = vm.NamaPrincipal,
                    Alamat = vm.Alamat,
                    NomorTelepon = vm.NomorTelepon,
                    Email = vm.Email,
                    Keterangan = vm.Keterangan,                   
                };
               
                var resultPrincipal = _principalRepository.GetAllPrincipal().Where(c => c.NamaPrincipal == vm.NamaPrincipal).FirstOrDefault();

                if (resultPrincipal == null)
                {
                    _principalRepository.Tambah(Principal);
                    TempData["SuccessMessage"] = "Principal " + vm.NamaPrincipal + " Berhasil Disimpan";
                    return RedirectToAction("Index", "Principal");
                }
                else
                {
                    TempData["WarningMessage"] = "Nama " + vm.NamaPrincipal + " sudah ada !!!";
                    return View(vm);
                }                
            }
            return View();
        }

        [Authorize(Roles = "DetailPrincipal")]
        [AllowAnonymous]
        public async Task<IActionResult> DetailPrincipal(Guid Id)
        {
            var principal = await _principalRepository.GetPrincipalById(Id);

            if (principal == null)
            {
                Response.StatusCode = 404;
                return View("PrincipalNotFound", Id);
            }

            DetailPrincipalViewModel viewModel = new DetailPrincipalViewModel
            {
                PrincipalId = principal.PrincipalId,
                KodePrincipal = principal.KodePrincipal,
                NamaPrincipal = principal.NamaPrincipal,
                Alamat = principal.Alamat,
                NomorTelepon = principal.NomorTelepon,
                Email = principal.Email,
                Keterangan = principal.Keterangan,
            };
            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "DetailPrincipal")]
        [AllowAnonymous]
        public async Task<IActionResult> DetailPrincipal(DetailPrincipalViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Principal principal = await _principalRepository.GetPrincipalByIdNoTracking(viewModel.PrincipalId);

                var check = _principalRepository.GetAllPrincipal().Where(d => d.KodePrincipal == viewModel.KodePrincipal).FirstOrDefault();

                if (check != null)
                {
                    principal.UpdateDateTime = DateTime.Now;
                    principal.KodePrincipal = viewModel.KodePrincipal;
                    principal.NamaPrincipal = viewModel.NamaPrincipal;
                    principal.Alamat = viewModel.Alamat;
                    principal.NomorTelepon = viewModel.NomorTelepon;
                    principal.Email = viewModel.Email;
                    principal.Keterangan = viewModel.Keterangan;

                    _principalRepository.Update(principal);
                    _applicationDbContext.SaveChanges();

                    TempData["SuccessMessage"] = "Principal " + viewModel.NamaPrincipal + " Berhasil Diubah";
                    return RedirectToAction("Index", "Principal");
                }
                else
                {
                    TempData["WarningMessage"] = "Nama " + viewModel.NamaPrincipal + " sudah ada !!!";
                    return View(viewModel);
                }
            }
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "DeletePrincipal")]
        [AllowAnonymous]
        //[Authorize]
        public async Task<IActionResult> DeletePrincipal(Guid Id)
        {
            var Principal = await _principalRepository.GetPrincipalById(Id);
            if (Principal == null)
            {
                Response.StatusCode = 404;
                return View("PrincipalNotFound", Id);
            }

            DetailPrincipalViewModel vm = new DetailPrincipalViewModel
            {
                PrincipalId = Principal.PrincipalId,
                KodePrincipal = Principal.KodePrincipal,
                NamaPrincipal = Principal.NamaPrincipal
            };
            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = "DeletePrincipal")]
        [AllowAnonymous]
        public async Task<IActionResult> DeletePrincipal(DetailPrincipalViewModel vm)
        {

            //Cek Relasi Principal dengan Produk
            var produk = _produkRepository.GetAllProduk().Where(p => p.PrincipalId == vm.PrincipalId).FirstOrDefault();
            if (produk == null)
            {
                //Hapus Data
                var Principals = _applicationDbContext.Principals.FirstOrDefault(x => x.PrincipalId == vm.PrincipalId);
                _applicationDbContext.Attach(Principals);
                _applicationDbContext.Entry(Principals).State = EntityState.Deleted;
                _applicationDbContext.SaveChanges();

                TempData["SuccessMessage"] = "Principal " + vm.NamaPrincipal + " Berhasil Dihapus";
                return RedirectToAction("Index", "Principal");
            }
            else {
                TempData["WarningMessage"] = "Principal " + vm.NamaPrincipal + " terelasi dengan produk " + produk.NamaProduk;
                return View(vm);
            }
        }
    }
}
