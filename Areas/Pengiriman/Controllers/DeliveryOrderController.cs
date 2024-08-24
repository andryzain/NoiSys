using FastReport.Data;
using FastReport.Export.PdfSimple;
using FastReport.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using NoiSys.Areas.Identity.Data;
using NoiSys.Areas.MasterData.Repository;
using NoiSys.Areas.Pengiriman.Models;
using NoiSys.Areas.Pengiriman.Repository;
using NoiSys.Areas.Transaksi.Models;
using NoiSys.Areas.Transaksi.Repository;
using NoiSys.Data;
using System.Data;
using System.Diagnostics.Metrics;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace NoiSys.Areas.Pengiriman.Controllers
{
    [Area("Pengiriman")]
    [Route("Pengiriman/[Controller]/[Action]")]
    [Authorize(Roles = "DeliveryOrder")]
    public class DeliveryOrderController : Controller
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

        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        public DeliveryOrderController(
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

            IHostingEnvironment hostingEnvironment,
            IWebHostEnvironment webHostEnvironment,
            IConfiguration configuration
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

            _hostingEnvironment = hostingEnvironment;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }

        [Authorize(Roles = "IndexDeliveryOrder")]
        public IActionResult Index()
        {
            var data = _deliveryOrderRepository.GetAllDeliveryOrder();
            return View(data);
        }

        [HttpGet]
        [Authorize(Roles = "DetailDeliveryOrder")]
        [AllowAnonymous]
        public async Task<IActionResult> DetailDeliveryOrder(Guid Id)
        {
            ViewBag.Po = new SelectList(await _purchaseOrderRepository.GetPurchaseOrders(), "PurchaseOrderId", "PurchaseOrderNumber", SortOrder.Ascending);
            ViewBag.Users = new SelectList(_userManager.Users, nameof(ApplicationUser.Id), nameof(ApplicationUser.NamaLengkap), SortOrder.Ascending);
            ViewBag.Termin = new SelectList(await _metodePembayaranRepository.GetMetodePembayarans(), "MetodePembayaranId", "NamaMetodePembayaran", SortOrder.Ascending);
            ViewBag.Approval = new SelectList(await _penggunaRepository.GetPenggunas(), "PenggunaId", "NamaLengkap", SortOrder.Ascending);
            ViewBag.Bengkel = new SelectList(await _bengkelRepository.GetBengkels(), "BengkelId", "NamaBengkel", SortOrder.Ascending);

            var deliveryOrder = await _deliveryOrderRepository.GetDeliveryOrderById(Id);

            if (deliveryOrder == null)
            {
                Response.StatusCode = 404;
                return View("PurchaseOrderNotFound", Id);
            }

            var getUser = _penggunaRepository.GetAllUserLogin().Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            DeliveryOrder model = new DeliveryOrder
            {
                DeliveryOrderId = deliveryOrder.DeliveryOrderId,
                DeliveryOrderNumber = deliveryOrder.DeliveryOrderNumber,
                PurchaseOrderId = deliveryOrder.PurchaseOrderId,
                PurchaseOrderNumber = deliveryOrder.PurchaseOrderNumber,                
                UserId = deliveryOrder.UserId,
                PenggunaId = deliveryOrder.PenggunaId,
                Termin = deliveryOrder.Termin,
                BengkelId = deliveryOrder.BengkelId,
                Status = deliveryOrder.Status,
                QtyTotal = deliveryOrder.QtyTotal,
                GrandTotal = Math.Truncate(deliveryOrder.GrandTotal),
                Keterangan = deliveryOrder.Keterangan,                
            };

            var ItemsList = new List<DeliveryOrderDetail>();

            foreach (var item in deliveryOrder.DeliveryOrderDetails)
            {
                ItemsList.Add(new DeliveryOrderDetail
                {
                    CreateDateTime = DateTime.Now,
                    CreateBy = new Guid(getUser.Id),
                    KodeProduk = item.KodeProduk,
                    NamaProduk = item.NamaProduk,
                    Principal = item.Principal,
                    Satuan = item.Satuan,
                    Qty = item.Qty,
                    Price = Math.Truncate(item.Price),
                    Diskon = item.Diskon,
                    SubTotal = Math.Truncate(item.SubTotal),
                    Checked = item.Checked,
                });
            }

            model.DeliveryOrderDetails = ItemsList;

            return View(model);
        }

        public async Task<IActionResult> PrintDeliveryOrder(Guid Id)
        {
            var deliveryOrder = await _deliveryOrderRepository.GetDeliveryOrderById(Id);
            var term = _metodePembayaranRepository.GetAllMetodePembayaran().Where(t => t.MetodePembayaranId == new Guid(deliveryOrder.Termin)).FirstOrDefault();
            var termin = term.NamaMetodePembayaran;

            var getDoNumber = deliveryOrder.DeliveryOrderNumber;
            var getPoNumber = deliveryOrder.PurchaseOrderNumber;
            var getBengkel = deliveryOrder.Bengkel.NamaBengkel;
            var getTermin = termin;
            var getDateNow = DateTime.Now.ToString("dd MMMM yyyy");
            var getGrandTotal = deliveryOrder.GrandTotal;
            var getTax = (getGrandTotal / 100) * 11;
            var getGrandTotalAfterTax = (getGrandTotal + getTax);
            var getDibuatOleh = deliveryOrder.ApplicationUser.NamaLengkap;
            var getDisetujuiOleh = deliveryOrder.Pengguna.NamaLengkap;

            WebReport web = new WebReport();
            var path = $"{_webHostEnvironment.WebRootPath}\\Reporting\\SuratJalan.frx";
            web.Report.Load(path);

            var msSqlDataConnection = new MsSqlDataConnection();
            msSqlDataConnection.ConnectionString = _configuration.GetConnectionString("DefaultConnection");
            var Conn = msSqlDataConnection.ConnectionString;

            web.Report.SetParameterValue("Conn", Conn);
            web.Report.SetParameterValue("DeliveryOrderId", Id.ToString());
            web.Report.SetParameterValue("DoNumber", getDoNumber);
            web.Report.SetParameterValue("PoNumber", getPoNumber);
            web.Report.SetParameterValue("DoDateNow", getDateNow);
            web.Report.SetParameterValue("Bengkel", getBengkel);
            web.Report.SetParameterValue("Termin", getTermin);
            web.Report.SetParameterValue("GrandTotal", getGrandTotal);
            web.Report.SetParameterValue("Tax", getTax);
            web.Report.SetParameterValue("GrandTotalAfterTax", getGrandTotalAfterTax);
            web.Report.SetParameterValue("DibuatOleh", getDibuatOleh);
            web.Report.SetParameterValue("DisetujuiOleh", getDisetujuiOleh);

            web.Report.Prepare();
            Stream stream = new MemoryStream();
            web.Report.Export(new PDFSimpleExport(), stream);
            stream.Position = 0;
            return File(stream, "application/zip", (getDoNumber + ".pdf"));
        }
    }
}
