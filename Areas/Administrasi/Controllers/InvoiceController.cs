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
using NoiSys.Areas.Administrasi.ViewModels;
using NoiSys.Areas.Identity.Data;
using NoiSys.Areas.Keuangan.Models;
using NoiSys.Areas.Keuangan.Repository;
using NoiSys.Areas.MasterData.Repository;
using NoiSys.Areas.Pengiriman.Models;
using NoiSys.Areas.Pengiriman.Repository;
using NoiSys.Areas.Transaksi.Models;
using NoiSys.Areas.Transaksi.Repository;
using NoiSys.Areas.Transaksi.ViewModels;
using NoiSys.Data;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace NoiSys.Areas.Administrasi.Controllers
{
    [Area("Administrasi")]
    [Route("Administrasi/[Controller]/[Action]")]
    [Authorize(Roles = "Invoice")]
    public class InvoiceController : Controller
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
        private readonly IPiutangUsahaRepository _piutangUsahaRepository;

        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        public InvoiceController(
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
            IBankCabangRepository bankCabangRepository,
            IPiutangUsahaRepository piutangUsahaRepository,

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
            _pembayaranRepository = pembayaranRepository;
            _bankRepository = bankRepository;
            _bankCabangRepository = bankCabangRepository;
            _piutangUsahaRepository = piutangUsahaRepository;

            _hostingEnvironment = hostingEnvironment;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }

        public JsonResult LoadBankCabang(Guid Id)
        {
            var bankCabang = _applicationDbContext.BankCabangs.Where(p => p.BankId == Id).ToList();
            return Json(new SelectList(bankCabang, "BankCabangId", "NamaCabang"));
        }

        [Authorize(Roles = "IndexInvoice")]
        public IActionResult Index()
        {
            var data = _invoiceRepository.GetAllInvoice();
            return View(data);
        }

        [HttpGet]
        [Authorize(Roles = "DetailInvoice")]
        [AllowAnonymous]
        public async Task<IActionResult> DetailInvoice(Guid Id)
        {            
            ViewBag.Users = new SelectList(_userManager.Users, nameof(ApplicationUser.Id), nameof(ApplicationUser.NamaLengkap), SortOrder.Ascending);
            ViewBag.Termin = new SelectList(await _metodePembayaranRepository.GetMetodePembayarans(), "MetodePembayaranId", "NamaMetodePembayaran", SortOrder.Ascending);
            ViewBag.Approval = new SelectList(await _penggunaRepository.GetPenggunas(), "PenggunaId", "NamaLengkap", SortOrder.Ascending);
            ViewBag.Bengkel = new SelectList(await _bengkelRepository.GetBengkels(), "BengkelId", "NamaBengkel", SortOrder.Ascending);

            var invoice = await _invoiceRepository.GetInvoiceById(Id);

            if (invoice == null)
            {
                Response.StatusCode = 404;
                return View("InvoiceNotFound", Id);
            }

            Invoice model = new Invoice
            {
                InvoiceId = invoice.InvoiceId,
                InvoiceNumber = invoice.InvoiceNumber,
                PurchaseOrderId = invoice.PurchaseOrderId,
                PurchaseOrderNumber = invoice.PurchaseOrderNumber,                
                UserId = invoice.UserId,
                PenggunaId = invoice.PenggunaId,
                Termin = invoice.Termin,
                BengkelId = invoice.BengkelId,
                Status = invoice.Status,
                QtyTotal = invoice.QtyTotal,
                GrandTotal = Math.Truncate(invoice.GrandTotal),
                Keterangan = invoice.Keterangan
            };

            var ItemsList = new List<InvoiceDetail>();

            foreach (var item in invoice.InvoiceDetails)
            {
                ItemsList.Add(new InvoiceDetail
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

            model.InvoiceDetails = ItemsList;

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "GeneratePayment-Invoice")]
        [AllowAnonymous]
        public async Task<IActionResult> GeneratePayment(Guid Id)
        {
            ViewBag.Bank = new SelectList(await _bankRepository.GetBanks(), "BankId", "NamaBank", SortOrder.Ascending);
            ViewBag.Termin = new SelectList(await _metodePembayaranRepository.GetMetodePembayarans(), "MetodePembayaranId", "NamaMetodePembayaran", SortOrder.Ascending);
            ViewBag.Approval = new SelectList(await _penggunaRepository.GetPenggunas(), "PenggunaId", "NamaLengkap", SortOrder.Ascending);
            ViewBag.Bengkel = new SelectList(await _bengkelRepository.GetBengkels(), "BengkelId", "NamaBengkel", SortOrder.Ascending);

            Invoice invoice = _applicationDbContext.Invoices
                .Include(u => u.ApplicationUser)
                .Include(r => r.PurchaseOrder)
                .Include(p => p.Pengguna)
                .Include(b => b.Bengkel)
                .Where(p => p.InvoiceId == Id).FirstOrDefault();

            _signInManager.IsSignedIn(User);

            var getUser = _penggunaRepository.GetAllUserLogin().Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            Pembayaran payment = new Pembayaran();

            var dateNow = DateTimeOffset.Now;
            var setDateNow = DateTimeOffset.Now.ToString("yyMMdd");

            var lastCode = _pembayaranRepository.GetAllPembayaran().Where(d => d.CreateDateTime.ToString("yyMMdd") == dateNow.ToString("yyMMdd")).OrderByDescending(k => k.PaymentNumber).FirstOrDefault();
            if (lastCode == null)
            {
                payment.PaymentNumber = "PAY" + setDateNow + "0001";
            }
            else
            {
                var lastCodeTrim = lastCode.PaymentNumber.Substring(3, 6);

                if (lastCodeTrim != setDateNow)
                {
                    payment.PaymentNumber = "PAY" + setDateNow + "0001";
                }
                else
                {
                    payment.PaymentNumber = "PAY" + setDateNow + (Convert.ToInt32(lastCode.PaymentNumber.Substring(9, lastCode.PaymentNumber.Length - 9)) + 1).ToString("D4");
                }
            }

            ViewBag.PaymentNumber = payment.PaymentNumber;

            var getInvVM = new InvoiceViewModel()
            {
                InvoiceId = invoice.InvoiceId,
                InvoiceNumber = invoice.InvoiceNumber,
                PurchaseOrderId = invoice.PurchaseOrderId,
                PurchaseOrderNumber = invoice.PurchaseOrderNumber,
                UserId = invoice.UserId,
                PenggunaId = invoice.PenggunaId,
                Termin = invoice.Termin,
                BengkelId = invoice.BengkelId,
                Status = invoice.Status,
                QtyTotal = invoice.QtyTotal,
                GrandTotal = Math.Truncate(invoice.GrandTotal),
                Keterangan = invoice.Keterangan,
            };

            return View(getInvVM);
        }

        [HttpPost]
        [Authorize(Roles = "GeneratePayment-Invoice")]
        [AllowAnonymous]
        public async Task<IActionResult> GeneratePayment(Invoice model, InvoiceViewModel vm)
        {
            ViewBag.Bank = new SelectList(await _bankRepository.GetBanks(), "BankId", "NamaBank", SortOrder.Ascending);            

            Invoice invoice = await _invoiceRepository.GetInvoiceByIdNoTracking(model.InvoiceId);

            _signInManager.IsSignedIn(User);

            var getUser = _penggunaRepository.GetAllUserLogin().Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            string getPaymentNumber = Request.Form["PAYNumber"];

            var updateInvoice = _invoiceRepository.GetAllInvoice().Where(c => c.InvoiceId == model.InvoiceId).FirstOrDefault();
            if (updateInvoice != null)
            {
                {
                    updateInvoice.Status = "Selesai";
                };
                _applicationDbContext.Entry(updateInvoice).State = EntityState.Modified;
            }

            var updateDeliveryOrder = _deliveryOrderRepository.GetAllDeliveryOrder().Where(c => c.PurchaseOrderId == vm.PurchaseOrderId).FirstOrDefault();
            if (updateDeliveryOrder != null)
            {
                {
                    updateDeliveryOrder.Status = "Selesai";
                };
                _applicationDbContext.Entry(updateDeliveryOrder).State = EntityState.Modified;
            }

            // Generate ke Table Pembayaran
            var newPayment = new Pembayaran
            {
                CreateDateTime = DateTime.Now,
                CreateBy = new Guid(getUser.Id),
                InvoiceId = invoice.InvoiceId,
                InvoiceNumber = invoice.InvoiceNumber,
                BankId = vm.BankId,
                UserId = getUser.Id.ToString(),
                PenggunaId = invoice.PenggunaId,
                Termin = invoice.Termin,
                BengkelId = invoice.BengkelId,
                Status = "Lunas",
                GrandTotal = invoice.GrandTotal,
                Keterangan = invoice.Keterangan,
            };

            newPayment.PaymentNumber = getPaymentNumber;
            
            _pembayaranRepository.Tambah(newPayment);

            // Generate ke Table Piutang Usaha            
            var newPiutang = new PiutangUsaha
            {
                CreateDateTime = DateTime.Now,
                CreateBy = new Guid(getUser.Id),                
                TransaksiId = invoice.InvoiceId, 
                TransaksiNumber = invoice.InvoiceNumber,
                BankId = vm.BankId,
                UserId = getUser.Id.ToString(),
                Nominal = invoice.GrandTotal
            };

            var dateNow = DateTimeOffset.Now;
            var setDateNow = DateTimeOffset.Now.ToString("yyMMdd");

            var lastCode = _piutangUsahaRepository.GetAllPiutangUsaha().Where(d => d.CreateDateTime.ToString("yyMMdd") == dateNow.ToString("yyMMdd")).OrderByDescending(k => k.PiutangNumber).FirstOrDefault();
            if (lastCode == null)
            {
                newPiutang.PiutangNumber = "PTG" + setDateNow + "0001";
            }
            else
            {
                var lastCodeTrim = lastCode.PiutangNumber.Substring(3, 6);

                if (lastCodeTrim != setDateNow)
                {
                    newPiutang.PiutangNumber = "PTG" + setDateNow + "0001";
                }
                else
                {
                    newPiutang.PiutangNumber = "PTG" + setDateNow + (Convert.ToInt32(lastCode.PiutangNumber.Substring(9, lastCode.PiutangNumber.Length - 9)) + 1).ToString("D4");
                }
            }

            _piutangUsahaRepository.Tambah(newPiutang);

            ViewBag.Bank = new SelectList(await _bankRepository.GetBanks(), "BankId", "NamaBank", SortOrder.Ascending);
            TempData["SuccessMessage"] = "No. Pembayaran " + newPayment.PaymentNumber + " Berhasil Disimpan";
            return RedirectToAction("Index", "Invoice");
        }

        public async Task<IActionResult> PrintInvoice(Guid Id)
        {
            var invoice = await _invoiceRepository.GetInvoiceById(Id);
            var term = _metodePembayaranRepository.GetAllMetodePembayaran().Where(t => t.MetodePembayaranId == new Guid(invoice.Termin)).FirstOrDefault();
            var termin = term.NamaMetodePembayaran;

            var getInvNumber = invoice.InvoiceNumber;
            var getPoNumber = invoice.PurchaseOrderNumber;
            var getBengkel = invoice.Bengkel.NamaBengkel;
            var getTermin = termin;
            var getDateNow = DateTime.Now.ToString("dd MMMM yyyy");
            var getGrandTotal = invoice.GrandTotal;
            var getTax = (getGrandTotal / 100) * 11;
            var getGrandTotalAfterTax = (getGrandTotal + getTax);
            var getDibuatOleh = invoice.ApplicationUser.NamaLengkap;
            var getDisetujuiOleh = invoice.Pengguna.NamaLengkap;

            WebReport web = new WebReport();
            var path = $"{_webHostEnvironment.WebRootPath}\\Reporting\\Invoice.frx";
            web.Report.Load(path);

            var msSqlDataConnection = new MsSqlDataConnection();
            msSqlDataConnection.ConnectionString = _configuration.GetConnectionString("DefaultConnection");
            var Conn = msSqlDataConnection.ConnectionString;

            web.Report.SetParameterValue("Conn", Conn);
            web.Report.SetParameterValue("InvoiceId", Id.ToString());
            web.Report.SetParameterValue("InvNumber", getInvNumber);
            web.Report.SetParameterValue("PoNumber", getPoNumber);
            web.Report.SetParameterValue("InvDateNow", getDateNow);
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
            return File(stream, "application/zip", (getInvNumber + ".pdf"));
        }
    }
}
