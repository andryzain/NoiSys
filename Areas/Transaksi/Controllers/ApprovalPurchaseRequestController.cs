using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NoiSys.Areas.Identity.Data;
using NoiSys.Areas.MasterData.Models;
using NoiSys.Areas.MasterData.Repository;
using NoiSys.Areas.MasterData.ViewModels;
using NoiSys.Areas.Transaksi.Models;
using NoiSys.Areas.Transaksi.Repository;
using NoiSys.Areas.Transaksi.ViewModels;
using NoiSys.Data;
using System.Data;

namespace NoiSys.Areas.Transaksi.Controllers
{
    [Area("Transaksi")]
    [Route("Transaksi/[Controller]/[Action]")]
    [Authorize(Roles = "ApprovalPurchaseRequest")]
    public class ApprovalPurchaseRequestController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IApprovalPurchaseRequestRepository _approvalPurchaseRequestRepository;
        private readonly IPenggunaRepository _penggunaRepository;
        private readonly IPurchaseRequestRepository _purchaseRequestRepository;
        private readonly IBengkelRepository _bengkelRepository;

        public ApprovalPurchaseRequestController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext applicationDbContext,
            IApprovalPurchaseRequestRepository approvalPurchaseRequestRepository,
            IPenggunaRepository penggunaRepository,
            IPurchaseRequestRepository purchaseRequestRepository,
            IBengkelRepository bengkelRepository
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _applicationDbContext = applicationDbContext;
            _approvalPurchaseRequestRepository = approvalPurchaseRequestRepository;
            _penggunaRepository = penggunaRepository;
            _purchaseRequestRepository = purchaseRequestRepository;
            _bengkelRepository = bengkelRepository;
        }
        [Authorize(Roles = "IndexApprovalPurchaseRequest")]
        public IActionResult Index()
        {
            var data = _approvalPurchaseRequestRepository.GetAllApprovalPurchaseRequest();
            return View(data);
        }

        [Authorize(Roles = "DetailApprovalPurchaseRequest")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ViewResult> ApprovalPurchaseRequest(Guid Id)
        {
            ViewBag.Pengguna = new SelectList(await _penggunaRepository.GetPenggunas(), "PenggunaId", "NamaLengkap", SortOrder.Ascending);
            ViewBag.Bengkel = new SelectList(await _bengkelRepository.GetBengkels(), "BengkelId", "NamaBengkel", SortOrder.Ascending);
            ViewBag.Users = new SelectList(_userManager.Users, nameof(ApplicationUser.Id), nameof(ApplicationUser.NamaLengkap), SortOrder.Ascending);

            _signInManager.IsSignedIn(User);

            var getUser = _penggunaRepository.GetAllUserLogin().Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            var approval = await _approvalPurchaseRequestRepository.GetApprovalPurchaseRequestById(Id);

            var approvalViewModel = new ApprovalPurchaseRequestViewModel()
            {
                ApprovalId = approval.ApprovalId,
                PurchaseRequestId = approval.PurchaseRequestId,
                PurchaseRequestNumber = approval.PurchaseRequestNumber,
                UserId = approval.UserId,
                PenggunaId = approval.PenggunaId,
                BengkelId = approval.BengkelId,
                ApproveDate = approval.ApproveDate,
                ApproveBy = getUser.NamaLengkap,
                Status = approval.Status,
                Keterangan = approval.Keterangan
            };

            var getPrNumber = _purchaseRequestRepository.GetAllPurchaseRequest().Where(pr => pr.PurchaseRequestNumber == approvalViewModel.PurchaseRequestNumber).FirstOrDefault();
            
            approvalViewModel.QtyTotal = getPrNumber.QtyTotal;
            approvalViewModel.GrandTotal = Math.Truncate(getPrNumber.GrandTotal);

            var ItemsList = new List<PurchaseRequestDetail>();

            foreach (var item in getPrNumber.PurchaseRequestDetails)
            {
                ItemsList.Add(new PurchaseRequestDetail
                {
                    KodeProduk = item.KodeProduk,
                    NamaProduk = item.NamaProduk,
                    Principal = item.Principal,
                    Satuan = item.Satuan,
                    Qty = item.Qty,
                    Price = Math.Truncate(item.Price),
                    Diskon = item.Diskon,
                    SubTotal = Math.Truncate(item.SubTotal)
                });
            }

            approvalViewModel.PurchaseRequestDetails = ItemsList;

            return View(approvalViewModel);
        }

        [Authorize(Roles = "DetailApprovalPurchaseRequest")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ApprovalPurchaseRequest(ApprovalPurchaseRequestViewModel vm)
        {
            if (ModelState.IsValid)
            {
                ApprovalPurchaseRequest approval = await _approvalPurchaseRequestRepository.GetApprovalPurchaseRequestByIdNoTracking(vm.ApprovalId);
                
                approval.Status = vm.Status;
                approval.Keterangan = vm.Keterangan;

                if (approval.Status == "Disetujui" || approval.Status == "Ditolak")
                {
                    approval.ApproveDate = DateTime.Now;
                    approval.ApproveBy = vm.ApproveBy;
                }

                var result = _purchaseRequestRepository.GetAllPurchaseRequest().Where(c => c.PurchaseRequestNumber == vm.PurchaseRequestNumber).FirstOrDefault();
                if (result != null)
                {
                    result.Status = vm.Status;
                    result.Keterangan = vm.Keterangan;

                    _applicationDbContext.Entry(result).State = EntityState.Modified;                    
                }
                _approvalPurchaseRequestRepository.Update(approval);

                if (approval.Status == "Belum Disetujui")
                {
                    TempData["SuccessMessage"] = "No. Purchase Request " + vm.PurchaseRequestNumber + " belum disetujui";
                }
                else if (approval.Status == "Disetujui")
                {
                    TempData["SuccessMessage"] = "No. Purchase Request " + vm.PurchaseRequestNumber + " disetujui";
                }
                else if (approval.Status == "Ditolak")
                {
                    TempData["SuccessMessage"] = "No. Purchase Request " + vm.PurchaseRequestNumber + " ditolak";
                }
                return RedirectToAction("Index", "ApprovalPurchaseRequest");
            }

            return View();
        }        
    }
}
