using FastReport.Data;
using FastReport.Export.PdfSimple;
using FastReport.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NoiSys.Areas.Administrasi.Models;
using NoiSys.Areas.Administrasi.Repository;
using NoiSys.Areas.Identity.Data;
using NoiSys.Areas.MasterData.Repository;
using NoiSys.Areas.MasterData.ViewModels;
using NoiSys.Areas.Pengiriman.Models;
using NoiSys.Areas.Pengiriman.Repository;
using NoiSys.Areas.Transaksi.Models;
using NoiSys.Areas.Transaksi.Repository;
using NoiSys.Areas.Transaksi.ViewModels;
using NoiSys.Data;
using System.Data;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace NoiSys.Areas.Transaksi.Controllers
{
    [Area("Transaksi")]
    [Route("Transaksi/[Controller]/[Action]")]
    [Authorize(Roles = "PurchaseOrder")]
    public class PurchaseOrderController : Controller
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

        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        public PurchaseOrderController(
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
            _invoiceRepository = invoiceRepository;

            _hostingEnvironment = hostingEnvironment;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }

        [Authorize(Roles = "IndexPurchaseOrder")]
        public IActionResult Index()
        {
            var data = _purchaseOrderRepository.GetAllPurchaseOrder();
            return View(data);
        }

        [HttpGet]
        [Authorize(Roles = "DetailPurchaseOrder")]
        [AllowAnonymous]
        public async Task<IActionResult> DetailPurchaseOrder(Guid Id)
        {
            ViewBag.Pr = new SelectList(await _purchaseRequestRepository.GetPurchaseRequests(), "PurchaseRequestId", "PurchaseRequestNumber", SortOrder.Ascending);
            ViewBag.Users = new SelectList(_userManager.Users, nameof(ApplicationUser.Id), nameof(ApplicationUser.NamaLengkap), SortOrder.Ascending);
            ViewBag.Termin = new SelectList(await _metodePembayaranRepository.GetMetodePembayarans(), "MetodePembayaranId", "NamaMetodePembayaran", SortOrder.Ascending);
            ViewBag.Approval = new SelectList(await _penggunaRepository.GetPenggunas(), "PenggunaId", "NamaLengkap", SortOrder.Ascending);
            ViewBag.Bengkel = new SelectList(await _bengkelRepository.GetBengkels(), "BengkelId", "NamaBengkel", SortOrder.Ascending);

            var purchaseOrder = await _purchaseOrderRepository.GetPurchaseOrderById(Id);

            if (purchaseOrder == null)
            {
                Response.StatusCode = 404;
                return View("PurchaseOrderNotFound", Id);
            }

            PurchaseOrder model = new PurchaseOrder
            {
                PurchaseOrderId = purchaseOrder.PurchaseOrderId,
                PurchaseOrderNumber = purchaseOrder.PurchaseOrderNumber,
                PurchaseRequestId = purchaseOrder.PurchaseRequestId,
                PurchaseRequestNumber = purchaseOrder.PurchaseRequestNumber,
                UserId = purchaseOrder.UserId,
                PenggunaId = purchaseOrder.PenggunaId,
                Termin = purchaseOrder.Termin,
                BengkelId = purchaseOrder.BengkelId,
                Status = purchaseOrder.Status,
                QtyTotal = purchaseOrder.QtyTotal,
                GrandTotal = Math.Truncate(purchaseOrder.GrandTotal),
                Keterangan = purchaseOrder.Keterangan,
                PurchaseOrderDetails = purchaseOrder.PurchaseOrderDetails,
            };

            var ItemsList = new List<PurchaseOrderDetail>();

            foreach (var item in purchaseOrder.PurchaseOrderDetails)
            {
                ItemsList.Add(new PurchaseOrderDetail
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

            model.PurchaseOrderDetails = ItemsList;

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "GenerateDeliveryOrder-PurchaseOrder")]
        [AllowAnonymous]
        public async Task<IActionResult> GenerateDeliveryOrder(Guid Id)
        {
            ViewBag.Produk = new SelectList(await _produkRepository.GetProduks(), "ProdukId", "NamaProduk", SortOrder.Ascending);
            ViewBag.Termin = new SelectList(await _metodePembayaranRepository.GetMetodePembayarans(), "MetodePembayaranId", "NamaMetodePembayaran", SortOrder.Ascending);
            ViewBag.Approval = new SelectList(await _penggunaRepository.GetPenggunas(), "PenggunaId", "NamaLengkap", SortOrder.Ascending);
            ViewBag.Bengkel = new SelectList(await _bengkelRepository.GetBengkels(), "BengkelId", "NamaBengkel", SortOrder.Ascending);            

            PurchaseOrder purchaseOrder = _applicationDbContext.PurchaseOrders
                .Include(d => d.PurchaseOrderDetails)
                .Include(u => u.ApplicationUser)
                .Include(p => p.Pengguna)
                .Include(b => b.Bengkel)
                .Where(p => p.PurchaseOrderId == Id).FirstOrDefault();   

            _signInManager.IsSignedIn(User);

            var getUser = _penggunaRepository.GetAllUserLogin().Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            DeliveryOrder dlvOrder = new DeliveryOrder();

            var dateNow = DateTimeOffset.Now;
            var setDateNow = DateTimeOffset.Now.ToString("yyMMdd");

            var lastCode = _deliveryOrderRepository.GetAllDeliveryOrder().Where(d => d.CreateDateTime.ToString("yyMMdd") == dateNow.ToString("yyMMdd")).OrderByDescending(k => k.DeliveryOrderNumber).FirstOrDefault();
            if (lastCode == null)
            {
                dlvOrder.DeliveryOrderNumber = "DO" + setDateNow + "0001";
            }
            else
            {
                var lastCodeTrim = lastCode.DeliveryOrderNumber.Substring(2, 6);

                if (lastCodeTrim != setDateNow)
                {
                    dlvOrder.DeliveryOrderNumber = "DO" + setDateNow + "0001";
                }
                else
                {
                    dlvOrder.DeliveryOrderNumber = "DO" + setDateNow + (Convert.ToInt32(lastCode.DeliveryOrderNumber.Substring(9, lastCode.DeliveryOrderNumber.Length - 9)) + 1).ToString("D4");
                }
            }

            ViewBag.DeliveryOrderNumber = dlvOrder.DeliveryOrderNumber;

            var getPoVm = new PurchaseOrderViewModel()
            {
                PurchaseOrderId = purchaseOrder.PurchaseOrderId,
                PurchaseOrderNumber = purchaseOrder.PurchaseOrderNumber,
                UserId = purchaseOrder.UserId,
                PenggunaId = purchaseOrder.PenggunaId,
                Termin = purchaseOrder.Termin,
                BengkelId = purchaseOrder.BengkelId,
                Status = purchaseOrder.Status,
                QtyTotal = purchaseOrder.QtyTotal,
                GrandTotal = Math.Truncate(purchaseOrder.GrandTotal),
                Keterangan = purchaseOrder.Keterangan,                
            };

            var ItemsList = new List<PurchaseOrderDetail>();

            foreach (var item in purchaseOrder.PurchaseOrderDetails)
            {
                ItemsList.Add(new PurchaseOrderDetail
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
                    SubTotal = Math.Truncate(item.SubTotal)
                });
            }

            getPoVm.PurchaseOrderDetails = ItemsList;

            return View(getPoVm);
        }

        [HttpPost]
        [Authorize(Roles = "GenerateDeliveryOrder-PurchaseOrder")]
        [AllowAnonymous]
        public async Task<IActionResult> GenerateDeliveryOrder(PurchaseOrder model, PurchaseOrderViewModel vm)
        {            
            PurchaseOrder purchaseOrder = await _purchaseOrderRepository.GetPurchaseOrderByIdNoTracking(model.PurchaseOrderId);

            _signInManager.IsSignedIn(User);

            var getUser = _penggunaRepository.GetAllUserLogin().Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            string getDeliveryOrderNumber = Request.Form["DONumber"];

            var updatePurchaseOrder = _purchaseOrderRepository.GetAllPurchaseOrder().Where(c => c.PurchaseOrderId == model.PurchaseOrderId).FirstOrDefault();
            if (updatePurchaseOrder != null)
            {
                {
                    updatePurchaseOrder.Status = "Selesai";
                };
                _applicationDbContext.Entry(updatePurchaseOrder).State = EntityState.Modified;
            }

            var newDeliveryOrder = new DeliveryOrder
            {
                CreateDateTime = DateTime.Now,
                CreateBy = new Guid(getUser.Id),
                PurchaseOrderId = purchaseOrder.PurchaseOrderId,
                PurchaseOrderNumber = purchaseOrder.PurchaseOrderNumber,
                UserId = getUser.Id.ToString(),
                PenggunaId = purchaseOrder.PenggunaId,
                Termin = purchaseOrder.Termin,
                BengkelId = purchaseOrder.BengkelId,
                Status = "Dikirim",
                QtyTotal = purchaseOrder.QtyTotal,
                GrandTotal = Math.Truncate(purchaseOrder.GrandTotal),
                Keterangan = purchaseOrder.Keterangan,
            };

            newDeliveryOrder.DeliveryOrderNumber = getDeliveryOrderNumber;

            var ItemsList = new List<DeliveryOrderDetail>();

            foreach (var item in vm.PurchaseOrderDetails)
            {
                //Saat proses buat surat jalan stok barang di gudang akan berkurang
                var updateProduk = _produkRepository.GetAllProduk().Where(c => c.KodeProduk == item.KodeProduk).FirstOrDefault();
                if (updatePurchaseOrder != null)
                {
                    {
                        updateProduk.JumlahStok = updateProduk.JumlahStok - item.Qty;
                    };
                    _applicationDbContext.Entry(updateProduk).State = EntityState.Modified;
                }

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
                    Checked = item.Checked
                });
            }

            newDeliveryOrder.DeliveryOrderDetails = ItemsList;

            _deliveryOrderRepository.Tambah(newDeliveryOrder);

            //Generate Invoice            

            var invoice = new Invoice()
            {
                CreateDateTime = DateTime.Now,
                CreateBy = new Guid(getUser.Id),
                PurchaseOrderId = purchaseOrder.PurchaseOrderId,
                PurchaseOrderNumber = purchaseOrder.PurchaseOrderNumber,
                UserId = getUser.Id.ToString(),
                PenggunaId = purchaseOrder.PenggunaId,
                Termin = purchaseOrder.Termin,
                BengkelId = purchaseOrder.BengkelId,
                Status = "Menunggu Pembayaran",
                QtyTotal = purchaseOrder.QtyTotal,
                GrandTotal = Math.Truncate(purchaseOrder.GrandTotal),
                Keterangan = purchaseOrder.Keterangan,
            };

            var dateNow = DateTimeOffset.Now;
            var setDateNow = DateTimeOffset.Now.ToString("yyMMdd");

            var lastCode = _invoiceRepository.GetAllInvoice().Where(d => d.CreateDateTime.ToString("yyMMdd") == dateNow.ToString("yyMMdd")).OrderByDescending(k => k.InvoiceNumber).FirstOrDefault();
            if (lastCode == null)
            {
                invoice.InvoiceNumber = "INV" + setDateNow + "0001";
            }
            else
            {
                var lastCodeTrim = lastCode.InvoiceNumber.Substring(3, 6);

                if (lastCodeTrim != setDateNow)
                {
                    invoice.InvoiceNumber = "INV" + setDateNow + "0001";
                }
                else
                {
                    invoice.InvoiceNumber = "INV" + setDateNow + (Convert.ToInt32(lastCode.InvoiceNumber.Substring(9, lastCode.InvoiceNumber.Length - 9)) + 1).ToString("D4");
                }
            }

            var ItemsListInvoice = new List<InvoiceDetail>();

            foreach (var item in vm.PurchaseOrderDetails)
            {
                ItemsListInvoice.Add(new InvoiceDetail
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
                    SubTotal = Math.Truncate(item.SubTotal)
                });
            }

            invoice.InvoiceDetails = ItemsListInvoice;

            _invoiceRepository.Tambah(invoice);

            TempData["SuccessMessage"] = "No. DO " + newDeliveryOrder.DeliveryOrderNumber + " Berhasil Disimpan";
            return RedirectToAction("Index", "PurchaseOrder");
        }

        public async Task<IActionResult> PrintPurchaseOrder(Guid Id)
        {
            var purchaseOrder = await _purchaseOrderRepository.GetPurchaseOrderById(Id);
            var term = _metodePembayaranRepository.GetAllMetodePembayaran().Where(t => t.MetodePembayaranId == new Guid(purchaseOrder.Termin)).FirstOrDefault();
            var termin = term.NamaMetodePembayaran;

            var getPoNumber = purchaseOrder.PurchaseOrderNumber;
            var getPrNumber = purchaseOrder.PurchaseRequestNumber;
            var getBengkel = purchaseOrder.Bengkel.NamaBengkel;
            var getTermin = termin;
            var getDateNow = DateTime.Now.ToString("dd MMMM yyyy");
            var getGrandTotal = purchaseOrder.GrandTotal;
            var getTax = (getGrandTotal / 100) * 11;
            var getGrandTotalAfterTax = (getGrandTotal + getTax);
            var getDibuatOleh = purchaseOrder.ApplicationUser.NamaLengkap;
            var getDisetujuiOleh = purchaseOrder.Pengguna.NamaLengkap;

            WebReport web = new WebReport();
            var path = $"{_webHostEnvironment.WebRootPath}\\Reporting\\PurchaseOrder.frx";
            web.Report.Load(path);

            var msSqlDataConnection = new MsSqlDataConnection();
            msSqlDataConnection.ConnectionString = _configuration.GetConnectionString("DefaultConnection");
            var Conn = msSqlDataConnection.ConnectionString;

            web.Report.SetParameterValue("Conn", Conn);
            web.Report.SetParameterValue("PurchaseOrderId", Id.ToString());
            web.Report.SetParameterValue("PoNumber", getPoNumber);
            web.Report.SetParameterValue("PrNumber", getPrNumber);
            web.Report.SetParameterValue("PoDateNow", getDateNow);
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
            return File(stream, "application/zip", (getPoNumber + ".pdf"));
        }
    }
}
