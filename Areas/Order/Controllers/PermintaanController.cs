using FastReport.Data;
using FastReport.Export.PdfSimple;
using FastReport.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NoiSys.Areas.Administrasi.Repository;
using NoiSys.Areas.Identity.Data;
using NoiSys.Areas.MasterData.Repository;
using NoiSys.Areas.Order.Models;
using NoiSys.Areas.Order.Repository;
using NoiSys.Areas.Pengiriman.Models;
using NoiSys.Areas.Transaksi.Repository;
using NoiSys.Data;
using System.Data;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace NoiSys.Areas.Order.Controllers
{
    [Area("Order")]
    [Route("Order/[Controller]/[Action]")]
    [Authorize(Roles = "Permintaan")]
    public class PermintaanController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IPermintaanRepository _permintaanDetails;
        private readonly IPenggunaRepository _penggunaRepository;        
        private readonly IProdukRepository _produkRepository;
        private readonly IMetodePembayaranRepository _metodePembayaranRepository;
        private readonly IBengkelRepository _bengkelRepository;
        private readonly IPembelianRepository _pembelianRepository;

        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        public PermintaanController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext applicationDbContext,
            IPermintaanRepository PermintaanRepository,
            IPenggunaRepository penggunaRepository,
            IProdukRepository produkRepository,
            IMetodePembayaranRepository metodePembayaranRepository,
            IBengkelRepository bengkelRepository,
            IPembelianRepository PembelianRepository,

            IHostingEnvironment hostingEnvironment,
            IWebHostEnvironment webHostEnvironment,
            IConfiguration configuration
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _applicationDbContext = applicationDbContext;
            _permintaanDetails = PermintaanRepository;
            _penggunaRepository = penggunaRepository;
            _produkRepository = produkRepository;
            _metodePembayaranRepository = metodePembayaranRepository;
            _bengkelRepository = bengkelRepository;
            _pembelianRepository = PembelianRepository;

            _hostingEnvironment = hostingEnvironment;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }

        public JsonResult LoadProduk(Guid Id)
        {
            var produk = _applicationDbContext.Produks.Include(p => p.Principal).Include(s => s.Satuan).Include(d => d.Diskon).Where(p => p.ProdukId == Id).FirstOrDefault();
            return new JsonResult(produk);
        }

        [Authorize(Roles = "IndexPermintaan")]
        public IActionResult Index()
        {
            var data = _permintaanDetails.GetAllPermintaan();
            return View(data);
        }

        [Authorize(Roles = "CreatePermintaan")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> CreatePermintaan()
        {
            _signInManager.IsSignedIn(User);
            var getUser = _penggunaRepository.GetAllUserLogin().Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            ViewBag.Produk = new SelectList(await _produkRepository.GetProduks(), "ProdukId", "NamaProduk", SortOrder.Ascending);
            ViewBag.Termin = new SelectList(await _metodePembayaranRepository.GetMetodePembayarans(), "MetodePembayaranId", "NamaMetodePembayaran", SortOrder.Ascending);
            ViewBag.Mengetahui = new SelectList(await _penggunaRepository.GetPenggunas(), "PenggunaId", "NamaLengkap", SortOrder.Ascending);
            ViewBag.Bengkel = new SelectList(await _bengkelRepository.GetBengkels(), "BengkelId", "NamaBengkel", SortOrder.Ascending);

            Permintaan Permintaan = new Permintaan()
            {
                UserId = getUser.Id,
            };
            Permintaan.PermintaanDetails.Add(new PermintaanDetail() { PermintaanDetailId = Guid.NewGuid() });

            var dateNow = DateTimeOffset.Now;
            var setDateNow = DateTimeOffset.Now.ToString("yyMMdd");

            var lastCode = _permintaanDetails.GetAllPermintaan().Where(d => d.CreateDateTime.ToString("yyMMdd") == dateNow.ToString("yyMMdd")).OrderByDescending(k => k.PermintaanNumber).FirstOrDefault();
            if (lastCode == null)
            {
                Permintaan.PermintaanNumber = "PM" + setDateNow + "0001";
            }
            else
            {
                var lastCodeTrim = lastCode.PermintaanNumber.Substring(2, 6);

                if (lastCodeTrim != setDateNow)
                {
                    Permintaan.PermintaanNumber = "PM" + setDateNow + "0001";
                }
                else
                {
                    Permintaan.PermintaanNumber = "PM" + setDateNow + (Convert.ToInt32(lastCode.PermintaanNumber.Substring(9, lastCode.PermintaanNumber.Length - 9)) + 1).ToString("D4");
                }
            }

            return View(Permintaan);
        }

        [Authorize(Roles = "CreatePermintaan")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreatePermintaan(Permintaan model)
        {
            ViewBag.Produk = new SelectList(await _produkRepository.GetProduks(), "ProdukId", "NamaProduk", SortOrder.Ascending);
            ViewBag.Termin = new SelectList(await _metodePembayaranRepository.GetMetodePembayarans(), "MetodePembayaranId", "NamaMetodePembayaran", SortOrder.Ascending);
            ViewBag.Mengetahui = new SelectList(await _penggunaRepository.GetPenggunas(), "PenggunaId", "NamaLengkap", SortOrder.Ascending);
            ViewBag.Bengkel = new SelectList(await _bengkelRepository.GetBengkels(), "BengkelId", "NamaBengkel", SortOrder.Ascending);

            var dateNow = DateTimeOffset.Now;
            var setDateNow = DateTimeOffset.Now.ToString("yyMMdd");

            var lastCode = _permintaanDetails.GetAllPermintaan().Where(d => d.CreateDateTime.ToString("yyMMdd") == dateNow.ToString("yyMMdd")).OrderByDescending(k => k.PermintaanNumber).FirstOrDefault();
            if (lastCode == null)
            {
                model.PermintaanNumber = "PM" + setDateNow + "0001";
            }
            else
            {
                var lastCodeTrim = lastCode.PermintaanNumber.Substring(2, 6);

                if (lastCodeTrim != setDateNow)
                {
                    model.PermintaanNumber = "PM" + setDateNow + "0001";
                }
                else
                {
                    model.PermintaanNumber = "PM" + setDateNow + (Convert.ToInt32(lastCode.PermintaanNumber.Substring(9, lastCode.PermintaanNumber.Length - 9)) + 1).ToString("D4");
                }
            }

            var getUser = _penggunaRepository.GetAllUserLogin().Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            //var getApproval = _penggunaRepository.GetAllUserLogin().Where(n => n.Id == model.Approval).FirstOrDefault();

            if (ModelState.IsValid)
            {
                var permintaan = new Permintaan
                {
                    CreateDateTime = DateTime.Now,
                    CreateBy = new Guid(getUser.Id), //Convert Guid to String
                    PermintaanId = model.PermintaanId,
                    PermintaanNumber = model.PermintaanNumber,
                    UserId = getUser.Id,
                    DisetujuiOlehId = model.DisetujuiOlehId,
                    PenggunaId = model.PenggunaId,
                    Termin = model.Termin,
                    Status = model.Status,
                    QtyTotal = model.QtyTotal,
                    GrandTotal = model.GrandTotal,
                    Keterangan = model.Keterangan,
                };

                var ItemsList = new List<PermintaanDetail>();

                foreach (var item in model.PermintaanDetails)
                {
                    ItemsList.Add(new PermintaanDetail
                    {
                        CreateDateTime = DateTime.Now,
                        CreateBy = new Guid(getUser.Id),
                        KodeProduk = item.KodeProduk,
                        NamaProduk = item.NamaProduk,
                        Principal = item.Principal,
                        Satuan = item.Satuan,
                        Qty = item.Qty,
                        Price = item.Price,
                        Diskon = item.Diskon,
                        SubTotal = item.SubTotal
                    });
                }

                permintaan.PermintaanDetails = ItemsList;
                _permintaanDetails.Tambah(permintaan);

                TempData["SuccessMessage"] = "No. Permintaan " + model.PermintaanNumber + " Berhasil Disimpan";
                return Json(new { redirectToUrl = Url.Action("Index", "Permintaan") });
            }
            else
            {
                ViewBag.Produk = new SelectList(await _produkRepository.GetProduks(), "ProdukId", "NamaProduk", SortOrder.Ascending);
                ViewBag.Termin = new SelectList(await _metodePembayaranRepository.GetMetodePembayarans(), "MetodePembayaranId", "NamaMetodePembayaran", SortOrder.Ascending);
                ViewBag.Mengetahui = new SelectList(await _penggunaRepository.GetPenggunas(), "PenggunaId", "NamaLengkap", SortOrder.Ascending);
                ViewBag.Bengkel = new SelectList(await _bengkelRepository.GetBengkels(), "BengkelId", "NamaBengkel", SortOrder.Ascending);
                TempData["WarningMessage"] = "Terdapat data yang masih kosong !!!";
                return View(model);
            }
        }

        [HttpGet]
        [Authorize(Roles = "DetailPermintaan")]
        [AllowAnonymous]
        public async Task<IActionResult> DetailPermintaan(Guid Id)
        {
            ViewBag.Produk = new SelectList(await _produkRepository.GetProduks(), "ProdukId", "NamaProduk", SortOrder.Ascending);
            ViewBag.Termin = new SelectList(await _metodePembayaranRepository.GetMetodePembayarans(), "MetodePembayaranId", "NamaMetodePembayaran", SortOrder.Ascending);
            ViewBag.Mengetahui = new SelectList(await _penggunaRepository.GetPenggunas(), "PenggunaId", "NamaLengkap", SortOrder.Ascending);
            ViewBag.Bengkel = new SelectList(await _bengkelRepository.GetBengkels(), "BengkelId", "NamaBengkel", SortOrder.Ascending);

            var Permintaan = await _permintaanDetails.GetPermintaanById(Id);

            if (Permintaan == null)
            {
                Response.StatusCode = 404;
                return View("PermintaanNotFound", Id);
            }           

            Permintaan model = new Permintaan
            {
                PermintaanId = Permintaan.PermintaanId,
                PermintaanNumber = Permintaan.PermintaanNumber,
                UserId = Permintaan.UserId,
                DisetujuiOlehId = Permintaan.DisetujuiOlehId,
                PenggunaId = Permintaan.PenggunaId,
                Termin = Permintaan.Termin,
                Status = Permintaan.Status,
                QtyTotal = Permintaan.QtyTotal,
                GrandTotal = Math.Truncate(Permintaan.GrandTotal),
                Keterangan = Permintaan.Keterangan
            };

            var ItemsList = new List<PermintaanDetail>();

            foreach (var item in Permintaan.PermintaanDetails)
            {
                ItemsList.Add(new PermintaanDetail
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

            model.PermintaanDetails = ItemsList;
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "DetailPermintaan")]
        [AllowAnonymous]
        public async Task<IActionResult> DetailPermintaan(Permintaan model)
        {
            var getUser = _penggunaRepository.GetAllUserLogin().Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            if (ModelState.IsValid)
            {
                Permintaan Permintaan = _permintaanDetails.GetAllPermintaan().Where(p => p.PermintaanNumber == model.PermintaanNumber).FirstOrDefault();                

                if (Permintaan != null)
                {
                    Permintaan.UpdateDateTime = DateTime.Now;
                    Permintaan.UpdateBy = new Guid(getUser.Id);
                    Permintaan.Termin = model.Termin;
                    Permintaan.PenggunaId = model.PenggunaId;
                    Permintaan.DisetujuiOlehId = model.DisetujuiOlehId;
                    Permintaan.Status = "Diproses";
                    Permintaan.Keterangan = model.Keterangan;
                    Permintaan.PermintaanDetails = model.PermintaanDetails;
                    Permintaan.QtyTotal = model.QtyTotal;
                    Permintaan.GrandTotal = model.GrandTotal;

                    if (Permintaan.DisetujuiOlehId == null)
                    {
                        TempData["SuccessMessage"] = "No. Permintaan " + model.PermintaanNumber + " Berhasil Diubah";
                        return Json(new { redirectToUrl = Url.Action("Index", "Permintaan") });
                    }
                    else {
                        _permintaanDetails.Update(Permintaan);
                        TempData["SuccessMessage"] = "No. Permintaan " + model.PermintaanNumber + " Berhasil Diubah";
                        return Json(new { redirectToUrl = Url.Action("Index", "Permintaan") });
                    }                    
                }
                else
                {
                    TempData["WarningMessage"] = "No. Permintaan " + model.PermintaanNumber + " sudah ada !!!";
                    return View(model);
                }
            }
            TempData["WarningMessage"] = "No. Permintaan " + model.PermintaanNumber + " Gagal Disimpan";
            return Json(new { redirectToUrl = Url.Action("Index", "Permintaan") });
        }

        [HttpGet]
        [Authorize(Roles = "GeneratePembelian-Permintaan")]
        [AllowAnonymous]
        public async Task<IActionResult> GeneratePembelian(Guid Id)
        {
            ViewBag.Produk = new SelectList(await _produkRepository.GetProduks(), "ProdukId", "NamaProduk", SortOrder.Ascending);
            ViewBag.Termin = new SelectList(await _metodePembayaranRepository.GetMetodePembayarans(), "MetodePembayaranId", "NamaMetodePembayaran", SortOrder.Ascending);
            ViewBag.Mengetahui = new SelectList(await _penggunaRepository.GetPenggunas(), "PenggunaId", "NamaLengkap", SortOrder.Ascending);
            ViewBag.Bengkel = new SelectList(await _bengkelRepository.GetBengkels(), "BengkelId", "NamaBengkel", SortOrder.Ascending);

            Permintaan Permintaan = _applicationDbContext.Permintaans
                .Include(d => d.PermintaanDetails)
                .Include(u => u.ApplicationUser)
                .Include(p => p.Pengguna)
                .Where(p => p.PermintaanId == Id).FirstOrDefault();

            _signInManager.IsSignedIn(User);

            var getUser = _penggunaRepository.GetAllUserLogin().Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            Pembelian po = new Pembelian();

            var dateNow = DateTimeOffset.Now;
            var setDateNow = DateTimeOffset.Now.ToString("yyMMdd");

            var lastCode = _pembelianRepository.GetAllPembelian().Where(d => d.CreateDateTime.ToString("yyMMdd") == dateNow.ToString("yyMMdd")).OrderByDescending(k => k.PembelianNumber).FirstOrDefault();
            if (lastCode == null)
            {
                po.PembelianNumber = "ORD" + setDateNow + "0001";
            }
            else
            {
                var lastCodeTrim = lastCode.PembelianNumber.Substring(3, 6);

                if (lastCodeTrim != setDateNow)
                {
                    po.PembelianNumber = "ORD" + setDateNow + "0001";
                }
                else
                {
                    po.PembelianNumber = "ORD" + setDateNow + (Convert.ToInt32(lastCode.PermintaanNumber.Substring(9, lastCode.PermintaanNumber.Length - 9)) + 1).ToString("D4");
                }
            }

            ViewBag.PembelianNumber = po.PembelianNumber;

            var getPr = new Permintaan()
            {
                PermintaanId = Permintaan.PermintaanId,
                PermintaanNumber = Permintaan.PermintaanNumber,
                UserId = Permintaan.UserId,
                PenggunaId = Permintaan.PenggunaId,
                DisetujuiOlehId = Permintaan.DisetujuiOlehId,
                Termin = Permintaan.Termin,
                Status = Permintaan.Status,
                QtyTotal = Permintaan.QtyTotal,
                GrandTotal = Permintaan.GrandTotal,
                Keterangan = Permintaan.Keterangan,
                PermintaanDetails = Permintaan.PermintaanDetails,
            };
            return View(getPr);
        }

        [HttpPost]
        [Authorize(Roles = "GeneratePembelian-Permintaan")]
        [AllowAnonymous]
        public async Task<IActionResult> GeneratePembelian(Permintaan model)
        {
            Permintaan Permintaan = await _permintaanDetails.GetPermintaanByIdNoTracking(model.PermintaanId);

            _signInManager.IsSignedIn(User);

            var getUser = _penggunaRepository.GetAllUserLogin().Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            string getPembelianNumber = Request.Form["PMBNumber"];

            var updatePermintaan = _permintaanDetails.GetAllPermintaan().Where(c => c.PermintaanId == model.PermintaanId).FirstOrDefault();
            if (updatePermintaan != null)
            {
                {
                    updatePermintaan.Status = getPembelianNumber;
                };
                _applicationDbContext.Entry(updatePermintaan).State = EntityState.Modified;
            }

            var newPembelian = new Pembelian
            {
                CreateDateTime = DateTime.Now,
                CreateBy = new Guid(getUser.Id),
                PermintaanId = Permintaan.PermintaanId,
                PermintaanNumber = Permintaan.PermintaanNumber,
                UserId = getUser.Id.ToString(),
                PenggunaId = Permintaan.PenggunaId,
                DisetujuiOlehId = Permintaan.DisetujuiOlehId,
                Termin = Permintaan.Termin,
                Status = "Diproses",
                QtyTotal = Permintaan.QtyTotal,
                GrandTotal = Permintaan.GrandTotal,
                Keterangan = Permintaan.Keterangan,
            };

            newPembelian.PembelianNumber = getPembelianNumber;

            var ItemsList = new List<PembelianDetail>();

            foreach (var item in Permintaan.PermintaanDetails)
            {
                ItemsList.Add(new PembelianDetail
                {
                    CreateDateTime = DateTime.Now,
                    CreateBy = new Guid(getUser.Id),
                    KodeProduk = item.KodeProduk,
                    NamaProduk = item.NamaProduk,
                    Principal = item.Principal,
                    Satuan = item.Satuan,
                    Qty = item.Qty,
                    Price = item.Price,
                    Diskon = item.Diskon,
                    SubTotal = item.SubTotal
                });
            }

            newPembelian.PembelianDetails = ItemsList;

            _pembelianRepository.Tambah(newPembelian);

            TempData["SuccessMessage"] = "No. Pembelian " + newPembelian.PembelianNumber + " Berhasil Disimpan";
            return RedirectToAction("Index", "Pembelian");
        }

        public async Task<IActionResult> PrintPermintaan(Guid Id)
        {
            var permintaan = await _permintaanDetails.GetPermintaanById(Id);
            var term = _metodePembayaranRepository.GetAllMetodePembayaran().Where(t => t.MetodePembayaranId == new Guid(permintaan.Termin)).FirstOrDefault();
            var termin = term.NamaMetodePembayaran;

            var getPmNumber = permintaan.PermintaanNumber;
            //var getPoNumber = permintaan.PurchaseOrderNumber;
            var getTermin = termin;
            var getDateNow = DateTime.Now.ToString("dd MMMM yyyy");
            var getGrandTotal = permintaan.GrandTotal;
            var getTax = (getGrandTotal / 100) * 11;
            var getGrandTotalAfterTax = (getGrandTotal + getTax);
            var getMengetahui = permintaan.Pengguna.NamaLengkap;
            var getDibuatOleh = permintaan.ApplicationUser.NamaLengkap;
            var getDisetujuiOleh = permintaan.DisetujuiOleh.NamaLengkap;

            WebReport web = new WebReport();
            var path = $"{_webHostEnvironment.WebRootPath}\\Reporting\\Permintaan.frx";
            web.Report.Load(path);

            var msSqlDataConnection = new MsSqlDataConnection();
            msSqlDataConnection.ConnectionString = _configuration.GetConnectionString("DefaultConnection");
            var Conn = msSqlDataConnection.ConnectionString;

            web.Report.SetParameterValue("Conn", Conn);
            web.Report.SetParameterValue("PermintaanId", Id.ToString());
            web.Report.SetParameterValue("PermintaanNumber", getPmNumber);
            //web.Report.SetParameterValue("PoNumber", getPoNumber);
            web.Report.SetParameterValue("PmDateNow", getDateNow);
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
            return File(stream, "application/zip", (getPmNumber + ".pdf"));
        }
    }
}
