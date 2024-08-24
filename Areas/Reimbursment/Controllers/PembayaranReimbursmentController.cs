using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using NoiSys.Areas.Administrasi.Models;
using NoiSys.Areas.Administrasi.Repository;
using NoiSys.Areas.Identity.Data;
using NoiSys.Areas.MasterData.Repository;
using NoiSys.Areas.Pengiriman.Repository;
using NoiSys.Areas.Reimbursment.Models;
using NoiSys.Areas.Reimbursment.Repository;
using NoiSys.Areas.Transaksi.Repository;
using NoiSys.Data;
using System.Data;

namespace NoiSys.Areas.Reimbursment.Controllers
{
    [Area("Reimbursment")]
    [Route("Reimbursment/[Controller]/[Action]")]
    [Authorize(Roles = "PembayaranReimbursment")]
    public class PembayaranReimbursmentController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IPurchaseRequestRepository _purchaseRequestRepository;
        private readonly IPenggunaRepository _penggunaRepository;
        private readonly IApprovalPurchaseRequestRepository _approvalPurchaseRequestRepository;
        private readonly IProdukRepository _produkRepository;
        private readonly IBengkelRepository _bengkelRepository;
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;
        private readonly IDeliveryOrderRepository _deliveryOrderRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IPembayaranReimbursmentRepository _pembayaranReimbursmentRepository;
        private readonly IBankRepository _bankRepository;
        private readonly IBankCabangRepository _bankCabangRepository;

        public PembayaranReimbursmentController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext applicationDbContext,
            IPurchaseRequestRepository purchaseRequestRepository,
            IPenggunaRepository penggunaRepository,
            IApprovalPurchaseRequestRepository approvalPurchaseRequestRepository,
            IProdukRepository produkRepository,            
            IBengkelRepository bengkelRepository,
            IPurchaseOrderRepository purchaseOrderRepository,
            IDeliveryOrderRepository deliveryOrderRepository,
            IInvoiceRepository invoiceRepository,
            IPembayaranReimbursmentRepository PembayaranReimbursmentRepository,
            IBankRepository bankRepository,
            IBankCabangRepository bankCabangRepository
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _applicationDbContext = applicationDbContext;
            _purchaseRequestRepository = purchaseRequestRepository;
            _penggunaRepository = penggunaRepository;
            _approvalPurchaseRequestRepository = approvalPurchaseRequestRepository;
            _produkRepository = produkRepository;           
            _bengkelRepository = bengkelRepository;
            _purchaseOrderRepository = purchaseOrderRepository;
            _deliveryOrderRepository = deliveryOrderRepository;
            _invoiceRepository = invoiceRepository;
            _pembayaranReimbursmentRepository = PembayaranReimbursmentRepository;
            _bankRepository = bankRepository;
            _bankCabangRepository = bankCabangRepository;
        }


        [Authorize(Roles = "IndexPembayaranReimbursment")]
        public IActionResult Index()
        {
            var data = _pembayaranReimbursmentRepository.GetAllPembayaran();
            return View(data);
        }

        [HttpGet]
        [Authorize(Roles = "DetailPembayaranReimbursment")]
        [AllowAnonymous]
        public async Task<IActionResult> DetailPembayaranReimbursment(Guid Id)
        {
            ViewBag.Users = new SelectList(_userManager.Users, nameof(ApplicationUser.Id), nameof(ApplicationUser.NamaLengkap), SortOrder.Ascending);            
            ViewBag.Bank = new SelectList(await _bankRepository.GetBanks(), "BankId", "NamaBank", SortOrder.Ascending);            

            var payment = await _pembayaranReimbursmentRepository.GetPembayaranById(Id);

            if (payment == null)
            {
                Response.StatusCode = 404;
                return View("PembayaranNotFound", Id);
            }

            PembayaranReimbursment model = new PembayaranReimbursment
            {
                PaymentId = payment.PaymentId,
                PaymentNumber = payment.PaymentNumber,
                PengajuanId = payment.PengajuanId,
                PengajuanNumber = payment.PengajuanNumber,                
                UserId = payment.UserId,
                BankId = payment.BankId,
                NomorRekening = payment.NomorRekening,
                AtasNama = payment.AtasNama,
                Status = payment.Status,
                GrandTotal = Math.Truncate(payment.GrandTotal),
                Keterangan = payment.Keterangan,
            };
            return View(model);
        }
    }
}
