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
using NoiSys.Areas.Order.Models;
using NoiSys.Areas.Order.Repository;
using NoiSys.Areas.Pengiriman.Repository;
using NoiSys.Areas.Transaksi.Repository;
using NoiSys.Data;
using System.Data;

namespace NoiSys.Areas.Order.Controllers
{
    [Area("Order")]
    [Route("Order/[Controller]/[Action]")]
    [Authorize(Roles = "PembayaranBarang")]
    public class PembayaranBarangController : Controller
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
        private readonly IPembayaranBarangRepository _pembayaranBarangRepository;
        private readonly IBankRepository _bankRepository;
        private readonly IBankCabangRepository _bankCabangRepository;

        public PembayaranBarangController(
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
            IPembayaranBarangRepository pembayaranBarangRepository,
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
            _pembayaranBarangRepository = pembayaranBarangRepository;
            _bankRepository = bankRepository;
            _bankCabangRepository = bankCabangRepository;
        }


        [Authorize(Roles = "IndexPembayaranBarang")]
        public IActionResult Index()
        {
            var data = _pembayaranBarangRepository.GetAllPembayaranBarang();
            return View(data);
        }

        [HttpGet]
        [Authorize(Roles = "DetailPembayaranBarang")]
        [AllowAnonymous]
        public async Task<IActionResult> DetailPembayaran(Guid Id)
        {
            ViewBag.Users = new SelectList(_userManager.Users, nameof(ApplicationUser.Id), nameof(ApplicationUser.NamaLengkap), SortOrder.Ascending);
            ViewBag.Pengguna = new SelectList(await _penggunaRepository.GetPenggunas(), "PenggunaId", "NamaLengkap", SortOrder.Ascending);
            ViewBag.Termin = new SelectList(await _metodePembayaranRepository.GetMetodePembayarans(), "MetodePembayaranId", "NamaMetodePembayaran", SortOrder.Ascending);
            ViewBag.Mengetahui = new SelectList(await _penggunaRepository.GetPenggunas(), "PenggunaId", "NamaLengkap", SortOrder.Ascending);            
            ViewBag.Bank = new SelectList(await _bankRepository.GetBanks(), "BankId", "NamaBank", SortOrder.Ascending);            

            var payment = await _pembayaranBarangRepository.GetPembayaranBarangById(Id);

            if (payment == null)
            {
                Response.StatusCode = 404;
                return View("PembayaranNotFound", Id);
            }

            PembayaranBarang model = new PembayaranBarang
            {
                PaymentId = payment.PaymentId,
                PaymentNumber = payment.PaymentNumber,
                PembelianId = payment.PembelianId,
                PembelianNumber = payment.PembelianNumber,
                BankId = payment.BankId,
                UserId = payment.UserId,
                PenggunaId = payment.PenggunaId,
                DisetujuiOlehId = payment.DisetujuiOlehId,
                Termin = payment.Termin,
                Status = payment.Status,
                GrandTotal = Math.Truncate(payment.GrandTotal),
                Keterangan = payment.Keterangan,
            };
            return View(model);
        }
    }
}
