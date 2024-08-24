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
    [Authorize(Roles = "ItemReimbursment")]
    public class ItemReimbursmentController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IItemReimbursmentRepository _itemReimbursmentRepository;

        public ItemReimbursmentController(
            ApplicationDbContext applicationDbContext,
            IItemReimbursmentRepository ItemReimbursmentRepository
        )
        {
            _applicationDbContext = applicationDbContext;
            _itemReimbursmentRepository = ItemReimbursmentRepository;
        }
        [Authorize(Roles = "IndexItemReimbursment")]
        public IActionResult Index()
        {
            var data = _itemReimbursmentRepository.GetAllItemReimbursment();
            return View(data);
        }

        [Authorize(Roles = "CreateItemReimbursment")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ViewResult> CreateItemReimbursment()
        {
            var ItemReimbursment = new ItemReimbursmentViewModel();
            var dateNow = DateTimeOffset.Now;
            var setDateNow = DateTimeOffset.Now.ToString("yyMMdd");

            var lastCode = _itemReimbursmentRepository.GetAllItemReimbursment().Where(d => d.CreateDateTime.ToString("yyMMdd") == dateNow.ToString("yyMMdd")).OrderByDescending(k => k.KodeItemReimbursment).FirstOrDefault();
            if (lastCode == null)
            {
                ItemReimbursment.KodeItemReimbursment = "ITM" + setDateNow + "0001";
            }
            else
            {
                var lastCodeTrim = lastCode.KodeItemReimbursment.Substring(3, 6);

                if (lastCodeTrim != setDateNow)
                {
                    ItemReimbursment.KodeItemReimbursment = "ITM" + setDateNow + "0001";
                }
                else
                {
                    ItemReimbursment.KodeItemReimbursment = "ITM" + setDateNow + (Convert.ToInt32(lastCode.KodeItemReimbursment.Substring(9, lastCode.KodeItemReimbursment.Length - 9)) + 1).ToString("D4");
                }
            }

            return View(ItemReimbursment);
        }

        [Authorize(Roles = "CreateItemReimbursment")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateItemReimbursment(ItemReimbursmentViewModel vm)
        {
            var dateNow = DateTimeOffset.Now;
            var setDateNow = DateTimeOffset.Now.ToString("yyMMdd");

            var lastCode = _itemReimbursmentRepository.GetAllItemReimbursment().Where(d => d.CreateDateTime.ToString("yyMMdd") == dateNow.ToString("yyMMdd")).OrderByDescending(k => k.KodeItemReimbursment).FirstOrDefault();
            if (lastCode == null)
            {
                vm.KodeItemReimbursment = "ITM" + setDateNow + "0001";
            }
            else
            {
                var lastCodeTrim = lastCode.KodeItemReimbursment.Substring(3, 6);

                if (lastCodeTrim != setDateNow)
                {
                    vm.KodeItemReimbursment = "ITM" + setDateNow + "0001";
                }
                else
                {
                    vm.KodeItemReimbursment = "ITM" + setDateNow + (Convert.ToInt32(lastCode.KodeItemReimbursment.Substring(9, lastCode.KodeItemReimbursment.Length - 9)) + 1).ToString("D4");
                }
            }

            if (ModelState.IsValid)
            {
                var itemReimbursment = new ItemReimbursment
                {
                    CreateDateTime = DateTime.Now,
                    ItemReimbursmentId = vm.ItemReimbursmentId,
                    KodeItemReimbursment = vm.KodeItemReimbursment,
                    NamaItemReimbursment = vm.NamaItemReimbursment
                };

                var resultItemReimbursment = _itemReimbursmentRepository.GetAllItemReimbursment().Where(c => c.NamaItemReimbursment == vm.NamaItemReimbursment).FirstOrDefault();

                if (resultItemReimbursment == null)
                {
                    _itemReimbursmentRepository.Tambah(itemReimbursment);
                    TempData["SuccessMessage"] = "Item " + vm.NamaItemReimbursment + " Berhasil Disimpan";
                    return RedirectToAction("Index", "ItemReimbursment");
                }
                else
                {
                    TempData["WarningMessage"] = "Item " + vm.NamaItemReimbursment + " sudah ada !!!";
                    return View(vm);
                }
            }
            return View();
        }

        [Authorize(Roles = "DetailItemReimbursment")]
        [AllowAnonymous]
        public async Task<IActionResult> DetailItemReimbursment(Guid Id)
        {
            var ItemReimbursment = await _itemReimbursmentRepository.GetItemReimbursmentById(Id);

            if (ItemReimbursment == null)
            {
                Response.StatusCode = 404;
                return View("ItemReimbursmentNotFound", Id);
            }

            ItemReimbursmentViewModel viewModel = new ItemReimbursmentViewModel
            {
                ItemReimbursmentId = ItemReimbursment.ItemReimbursmentId,
                KodeItemReimbursment = ItemReimbursment.KodeItemReimbursment,
                NamaItemReimbursment = ItemReimbursment.NamaItemReimbursment
            };
            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "DetailItemReimbursment")]
        [AllowAnonymous]
        public async Task<IActionResult> DetailItemReimbursment(ItemReimbursmentViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                ItemReimbursment ItemReimbursment = await _itemReimbursmentRepository.GetItemReimbursmentByIdNoTracking(viewModel.ItemReimbursmentId);

                var check = _itemReimbursmentRepository.GetAllItemReimbursment().Where(d => d.KodeItemReimbursment == viewModel.KodeItemReimbursment).FirstOrDefault();

                if (check != null)
                {
                    ItemReimbursment.UpdateDateTime = DateTime.Now;
                    ItemReimbursment.KodeItemReimbursment = viewModel.KodeItemReimbursment;
                    ItemReimbursment.NamaItemReimbursment = viewModel.NamaItemReimbursment;

                    _itemReimbursmentRepository.Update(ItemReimbursment);
                    _applicationDbContext.SaveChanges();

                    TempData["SuccessMessage"] = "Item " + viewModel.NamaItemReimbursment + " Berhasil Diubah";
                    return RedirectToAction("Index", "ItemReimbursment");
                }
                else
                {
                    TempData["WarningMessage"] = "Item " + viewModel.NamaItemReimbursment + " sudah ada !!!";
                    return View(viewModel);
                }
            }
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "DeleteItemReimbursment")]
        [AllowAnonymous]
        //[Authorize]
        public async Task<IActionResult> DeleteItemReimbursment(Guid Id)
        {
            var ItemReimbursment = await _itemReimbursmentRepository.GetItemReimbursmentById(Id);
            if (ItemReimbursment == null)
            {
                Response.StatusCode = 404;
                return View("ItemReimbursmentNotFound", Id);
            }

            ItemReimbursmentViewModel vm = new ItemReimbursmentViewModel
            {
                ItemReimbursmentId = ItemReimbursment.ItemReimbursmentId,
                KodeItemReimbursment = ItemReimbursment.KodeItemReimbursment,
                NamaItemReimbursment = ItemReimbursment.NamaItemReimbursment
            };
            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = "DeleteItemReimbursment")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteItemReimbursment(ItemReimbursmentViewModel vm)
        {
            //Hapus Data
            var ItemReimbursments = _applicationDbContext.ItemReimbursments.FirstOrDefault(x => x.ItemReimbursmentId == vm.ItemReimbursmentId);
            _applicationDbContext.Attach(ItemReimbursments);
            _applicationDbContext.Entry(ItemReimbursments).State = EntityState.Deleted;
            _applicationDbContext.SaveChanges();

            TempData["SuccessMessage"] = "Item " + vm.NamaItemReimbursment + " Berhasil Dihapus";

            return RedirectToAction("Index", "ItemReimbursment");
        }
    }
}
