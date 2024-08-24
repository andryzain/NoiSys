using FastReport.Data;
using FastReport.Export.PdfSimple;
using FastReport.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NoiSys.Areas.Identity.Data;
using NoiSys.Areas.Keuangan.Models;
using NoiSys.Areas.Keuangan.Repository;
using NoiSys.Areas.MasterData.Repository;
using NoiSys.Areas.Order.Models;
using NoiSys.Areas.Order.Repository;
using NoiSys.Areas.Order.ViewModels;
using NoiSys.Areas.Transaksi.Models;
using NoiSys.Data;
using System.Data;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace NoiSys.Areas.Order.Controllers
{
    [Area("Order")]
    [Route("Order/[Controller]/[Action]")]
    [Authorize(Roles = "Pembelian")]
    public class PembelianController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _applicationDbContext;        
        private readonly IPenggunaRepository _penggunaRepository;        
        private readonly IProdukRepository _produkRepository;
        private readonly IMetodePembayaranRepository _metodePembayaranRepository;
        private readonly IPermintaanRepository _permintaanRepository;
        private readonly IPembelianRepository _pembelianRepository;
        private readonly IBankRepository _bankRepository;
        private readonly IPembayaranBarangRepository _pembayaranBarangRepository;
        private readonly IHutangUsahaRepository _hutangUsahaRepository;

        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        public PembelianController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext applicationDbContext,            
            IPenggunaRepository penggunaRepository,           
            IProdukRepository produkRepository,
            IMetodePembayaranRepository metodePembayaranRepository,
            IPermintaanRepository permintaanRepository,
            IPembelianRepository pembelianRepository,
            IBankRepository bankRepository,
            IPembayaranBarangRepository pembayaranBarangRepository,
            IHutangUsahaRepository hutangUsahaRepository,

            IHostingEnvironment hostingEnvironment,
            IWebHostEnvironment webHostEnvironment,
            IConfiguration configuration
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _applicationDbContext = applicationDbContext;
            _penggunaRepository = penggunaRepository;            
            _produkRepository = produkRepository;
            _metodePembayaranRepository = metodePembayaranRepository;
            _permintaanRepository = permintaanRepository;
            _pembelianRepository = pembelianRepository;
            _bankRepository = bankRepository;
            _pembayaranBarangRepository = pembayaranBarangRepository;
            _hutangUsahaRepository = hutangUsahaRepository;

            _hostingEnvironment = hostingEnvironment;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }

        [Authorize(Roles = "IndexPembelian")]
        public IActionResult Index()
        {
            var data = _pembelianRepository.GetAllPembelian();
            return View(data);
        }

        [HttpGet]
        [Authorize(Roles = "DetailPembelian")]
        [AllowAnonymous]
        public async Task<IActionResult> DetailPembelian(Guid Id)
        {
            ViewBag.Permintaan = new SelectList(await _permintaanRepository.GetPermintaans(), "PermintaanId", "PermintaanNumber", SortOrder.Ascending);
            ViewBag.Users = new SelectList(_userManager.Users, nameof(ApplicationUser.Id), nameof(ApplicationUser.NamaLengkap), SortOrder.Ascending);
            ViewBag.Termin = new SelectList(await _metodePembayaranRepository.GetMetodePembayarans(), "MetodePembayaranId", "NamaMetodePembayaran", SortOrder.Ascending);
            ViewBag.Mengetahui = new SelectList(await _penggunaRepository.GetPenggunas(), "PenggunaId", "NamaLengkap", SortOrder.Ascending);            

            var pembelian = await _pembelianRepository.GetPembelianById(Id);

            if (pembelian == null)
            {
                Response.StatusCode = 404;
                return View("PembelianNotFound", Id);
            }

            Pembelian model = new Pembelian
            {
                PembelianId = pembelian.PembelianId,
                PembelianNumber = pembelian.PembelianNumber,
                PermintaanId = pembelian.PermintaanId,
                PermintaanNumber = pembelian.PermintaanNumber,
                UserId = pembelian.UserId,
                PenggunaId = pembelian.PenggunaId,
                DisetujuiOlehId = pembelian.DisetujuiOlehId,
                Termin = pembelian.Termin,
                Status = pembelian.Status,
                QtyTotal = pembelian.QtyTotal,
                GrandTotal = Math.Truncate(pembelian.GrandTotal),
                Keterangan = pembelian.Keterangan,
                PembelianDetails = pembelian.PembelianDetails,
            };

            var ItemsList = new List<PembelianDetail>();

            foreach (var item in pembelian.PembelianDetails)
            {
                ItemsList.Add(new PembelianDetail
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

            model.PembelianDetails = ItemsList;
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "GeneratePayment-Pembelian")]
        [AllowAnonymous]
        public async Task<IActionResult> GeneratePayment(Guid Id)
        {
            ViewBag.Bank = new SelectList(await _bankRepository.GetBanks(), "BankId", "NamaBank", SortOrder.Ascending);
            ViewBag.Termin = new SelectList(await _metodePembayaranRepository.GetMetodePembayarans(), "MetodePembayaranId", "NamaMetodePembayaran", SortOrder.Ascending);
            ViewBag.Mengetahui = new SelectList(await _penggunaRepository.GetPenggunas(), "PenggunaId", "NamaLengkap", SortOrder.Ascending);            

            Pembelian pembelian = _applicationDbContext.Pembelians
                .Include(u => u.ApplicationUser)
                .Include(r => r.Permintaan)
                .Include(p => p.Pengguna)
                .Where(p => p.PembelianId == Id).FirstOrDefault();

            _signInManager.IsSignedIn(User);

            var getUser = _penggunaRepository.GetAllUserLogin().Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            PembayaranBarang payment = new PembayaranBarang();

            var dateNow = DateTimeOffset.Now;
            var setDateNow = DateTimeOffset.Now.ToString("yyMMdd");

            var lastCode = _pembayaranBarangRepository.GetAllPembayaranBarang().Where(d => d.CreateDateTime.ToString("yyMMdd") == dateNow.ToString("yyMMdd")).OrderByDescending(k => k.PaymentNumber).FirstOrDefault();
            if (lastCode == null)
            {
                payment.PaymentNumber = "PBR" + setDateNow + "0001";
            }
            else
            {
                var lastCodeTrim = lastCode.PaymentNumber.Substring(3, 6);

                if (lastCodeTrim != setDateNow)
                {
                    payment.PaymentNumber = "PBR" + setDateNow + "0001";
                }
                else
                {
                    payment.PaymentNumber = "PBR" + setDateNow + (Convert.ToInt32(lastCode.PaymentNumber.Substring(9, lastCode.PaymentNumber.Length - 9)) + 1).ToString("D4");
                }
            }

            ViewBag.PaymentNumber = payment.PaymentNumber;

            var getPmbVM = new PembelianViewModel()
            {
                PembelianId = pembelian.PembelianId,
                PembelianNumber = pembelian.PembelianNumber,
                PermintaanId = pembelian.PermintaanId,
                PermintaanNumber = pembelian.PermintaanNumber,
                UserId = pembelian.UserId,
                PenggunaId = pembelian.PenggunaId,
                DisetujuiOlehId = pembelian.DisetujuiOlehId,
                Termin = pembelian.Termin,
                Status = pembelian.Status,
                QtyTotal = pembelian.QtyTotal,
                GrandTotal = Math.Truncate(pembelian.GrandTotal),
                Keterangan = pembelian.Keterangan,
            };

            return View(getPmbVM);
        }

        [HttpPost]
        [Authorize(Roles = "GeneratePayment-Invoice")]
        [AllowAnonymous]
        public async Task<IActionResult> GeneratePayment(Pembelian model, PembelianViewModel vm)
        {
            ViewBag.Bank = new SelectList(await _bankRepository.GetBanks(), "BankId", "NamaBank", SortOrder.Ascending);            

            Pembelian pembelian = await _pembelianRepository.GetPembelianByIdNoTracking(model.PembelianId);

            _signInManager.IsSignedIn(User);

            var getUser = _penggunaRepository.GetAllUserLogin().Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            string getPaymentNumber = Request.Form["PBRNumber"];

            var updatePembelian = _pembelianRepository.GetAllPembelian().Where(c => c.PembelianId == model.PembelianId).FirstOrDefault();
            if (updatePembelian != null)
            {
                {
                    updatePembelian.Status = "Selesai";
                };
                _applicationDbContext.Entry(updatePembelian).State = EntityState.Modified;
            }           

            // Generate ke Table Pembayaran
            var newPayment = new PembayaranBarang
            {
                CreateDateTime = DateTime.Now,
                CreateBy = new Guid(getUser.Id),
                PembelianId = pembelian.PembelianId,
                PembelianNumber = pembelian.PembelianNumber,
                BankId = vm.BankId,
                UserId = getUser.Id.ToString(),
                PenggunaId = pembelian.PenggunaId,
                DisetujuiOlehId = pembelian.DisetujuiOlehId,
                Termin = pembelian.Termin,
                Status = "Lunas",
                GrandTotal = pembelian.GrandTotal,
                Keterangan = pembelian.Keterangan,
            };

            newPayment.PaymentNumber = getPaymentNumber;

            _pembayaranBarangRepository.Tambah(newPayment);

            // Generate ke Table Hutang Usaha            
            var newHutang = new HutangUsaha
            {
                CreateDateTime = DateTime.Now,
                CreateBy = new Guid(getUser.Id),
                TransaksiId = pembelian.PembelianId,
                TransaksiNumber = pembelian.PembelianNumber,
                BankId = vm.BankId,
                UserId = getUser.Id.ToString(),
                Nominal = pembelian.GrandTotal
            };

            var dateNow = DateTimeOffset.Now;
            var setDateNow = DateTimeOffset.Now.ToString("yyMMdd");

            var lastCode = _hutangUsahaRepository.GetAllHutangUsaha().Where(d => d.CreateDateTime.ToString("yyMMdd") == dateNow.ToString("yyMMdd")).OrderByDescending(k => k.HutangNumber).FirstOrDefault();
            if (lastCode == null)
            {
                newHutang.HutangNumber = "HTG" + setDateNow + "0001";
            }
            else
            {
                var lastCodeTrim = lastCode.HutangNumber.Substring(3, 6);

                if (lastCodeTrim != setDateNow)
                {
                    newHutang.HutangNumber = "HTG" + setDateNow + "0001";
                }
                else
                {
                    newHutang.HutangNumber = "HTG" + setDateNow + (Convert.ToInt32(lastCode.HutangNumber.Substring(9, lastCode.HutangNumber.Length - 9)) + 1).ToString("D4");
                }
            }

            _hutangUsahaRepository.Tambah(newHutang);

            ViewBag.Bank = new SelectList(await _bankRepository.GetBanks(), "BankId", "NamaBank", SortOrder.Ascending);           
            TempData["SuccessMessage"] = "No. Pembayaran " + newPayment.PaymentNumber + " Berhasil Disimpan";
            return RedirectToAction("Index", "Pembelian");
        }

        public async Task<IActionResult> PrintPembelian(Guid Id)
        {
            var pembelian = await _pembelianRepository.GetPembelianById(Id);
            var term = _metodePembayaranRepository.GetAllMetodePembayaran().Where(t => t.MetodePembayaranId == new Guid(pembelian.Termin)).FirstOrDefault();
            var termin = term.NamaMetodePembayaran;

            var getPbrNumber = pembelian.PembelianNumber;
            var getPmNumber = pembelian.PermintaanNumber;
            var getTermin = termin;
            var getDateNow = DateTime.Now.ToString("dd MMMM yyyy");
            var getGrandTotal = pembelian.GrandTotal;
            var getTax = (getGrandTotal / 100) * 11;
            var getGrandTotalAfterTax = (getGrandTotal + getTax);
            var getMengetahui = pembelian.Pengguna.NamaLengkap;
            var getDibuatOleh = pembelian.ApplicationUser.NamaLengkap;
            var getDisetujuiOleh = pembelian.DisetujuiOleh.NamaLengkap;

            WebReport web = new WebReport();
            var path = $"{_webHostEnvironment.WebRootPath}\\Reporting\\Pembelian.frx";
            web.Report.Load(path);

            var msSqlDataConnection = new MsSqlDataConnection();
            msSqlDataConnection.ConnectionString = _configuration.GetConnectionString("DefaultConnection");
            var Conn = msSqlDataConnection.ConnectionString;

            web.Report.SetParameterValue("Conn", Conn);
            web.Report.SetParameterValue("PembelianId", Id.ToString());
            web.Report.SetParameterValue("PembelianNumber", getPbrNumber);
            web.Report.SetParameterValue("PermintaanNumber", getPmNumber);
            web.Report.SetParameterValue("PbrDateNow", getDateNow);
            web.Report.SetParameterValue("Termin", getTermin);
            web.Report.SetParameterValue("GrandTotal", getGrandTotal);
            web.Report.SetParameterValue("Tax", getTax);
            web.Report.SetParameterValue("GrandTotalAfterTax", getGrandTotalAfterTax);
            web.Report.SetParameterValue("Mengetahui", getMengetahui);
            web.Report.SetParameterValue("DibuatOleh", getDibuatOleh);
            web.Report.SetParameterValue("DisetujuiOleh", getDisetujuiOleh);

            web.Report.Prepare();
            Stream stream = new MemoryStream();
            web.Report.Export(new PDFSimpleExport(), stream);
            stream.Position = 0;
            return File(stream, "application/zip", (getPbrNumber + ".pdf"));
        }
    }
}
