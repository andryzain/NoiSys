using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using NoiSys.Areas.Administrasi.Models;
using NoiSys.Areas.Administrasi.Repository;
using NoiSys.Areas.Identity.Data;
using NoiSys.Areas.MasterData.Models;
using NoiSys.Areas.MasterData.Repository;
using NoiSys.Areas.Pengiriman.Repository;
using NoiSys.Areas.Transaksi.Repository;
using NoiSys.Data;
using System.Data;

namespace NoiSys.Areas.Administrasi.Controllers
{
    [Area("Administrasi")]
    [Route("Administrasi/[Controller]/[Action]")]
    [Authorize(Roles = "Pembayaran")]
    public class PembayaranController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IPurchaseRequestRepository _purchaseRequestRepository;
        private readonly IPenggunaRepository _penggunaRepository;
        private readonly IApprovalPurchaseRequestRepository _approvalPurchaseRequestRepository;
        private readonly IProdukRepository _produkRepository;
        private readonly IMetodePembayaranRepository _metodePembayaranRepository;
        private readonly IBengkelRepository _bengkelRepository;
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;
        private readonly IDeliveryOrderRepository _deliveryOrderRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IPembayaranRepository _pembayaranRepository;
        private readonly IBankRepository _bankRepository;
        private readonly IBankCabangRepository _bankCabangRepository;

        public PembayaranController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext applicationDbContext,
            IPurchaseRequestRepository purchaseRequestRepository,
            IPenggunaRepository penggunaRepository,
            IApprovalPurchaseRequestRepository approvalPurchaseRequestRepository,
            IProdukRepository produkRepository,
            IMetodePembayaranRepository metodePembayaranRepository,
            IBengkelRepository bengkelRepository,
            IPurchaseOrderRepository purchaseOrderRepository,
            IDeliveryOrderRepository deliveryOrderRepository,
            IInvoiceRepository invoiceRepository,
            IPembayaranRepository pembayaranRepository,
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
            _metodePembayaranRepository = metodePembayaranRepository;
            _bengkelRepository = bengkelRepository;
            _purchaseOrderRepository = purchaseOrderRepository;
            _deliveryOrderRepository = deliveryOrderRepository;
            _invoiceRepository = invoiceRepository;
            _pembayaranRepository = pembayaranRepository;
            _bankRepository = bankRepository;
            _bankCabangRepository = bankCabangRepository;
        }


        [Authorize(Roles = "IndexPembayaran")]
        public IActionResult Index()
        {
            var data = _pembayaranRepository.GetAllPembayaran();
            return View(data);
        }

        [HttpGet]
        [Authorize(Roles = "DetailPembayaran")]
        [AllowAnonymous]
        public async Task<IActionResult> DetailPembayaran(Guid Id)
        {
            ViewBag.Users = new SelectList(_userManager.Users, nameof(ApplicationUser.Id), nameof(ApplicationUser.NamaLengkap), SortOrder.Ascending);
            ViewBag.Pengguna = new SelectList(await _penggunaRepository.GetPenggunas(), "PenggunaId", "NamaLengkap", SortOrder.Ascending);
            ViewBag.Termin = new SelectList(await _metodePembayaranRepository.GetMetodePembayarans(), "MetodePembayaranId", "NamaMetodePembayaran", SortOrder.Ascending);
            ViewBag.Approval = new SelectList(await _penggunaRepository.GetPenggunas(), "PenggunaId", "NamaLengkap", SortOrder.Ascending);
            ViewBag.Bengkel = new SelectList(await _bengkelRepository.GetBengkels(), "BengkelId", "NamaBengkel", SortOrder.Ascending);
            ViewBag.Bank = new SelectList(await _bankRepository.GetBanks(), "BankId", "NamaBank", SortOrder.Ascending);
            ViewBag.BankCabang = new SelectList(await _bankCabangRepository.GetBankCabangs(), "BankCabangId", "NamaCabang", SortOrder.Ascending);

            var payment = await _pembayaranRepository.GetPembayaranById(Id);

            if (payment == null)
            {
                Response.StatusCode = 404;
                return View("PembayaranNotFound", Id);
            }

            Pembayaran model = new Pembayaran
            {
                PaymentId = payment.PaymentId,
                PaymentNumber = payment.PaymentNumber,
                InvoiceId = payment.InvoiceId,
                InvoiceNumber = payment.InvoiceNumber,
                BankId = payment.BankId,
                UserId = payment.UserId,
                PenggunaId = payment.PenggunaId,
                Termin = payment.Termin,
                BengkelId = payment.BengkelId,
                Status = payment.Status,
                GrandTotal = Math.Truncate(payment.GrandTotal),
                Keterangan = payment.Keterangan,
            };
            return View(model);
        }
    }
}
