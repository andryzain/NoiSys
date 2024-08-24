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
using NoiSys.Areas.Identity.Data;
using NoiSys.Areas.MasterData.Repository;
using NoiSys.Areas.Order.Models;
using NoiSys.Areas.Reimbursment.Models;
using NoiSys.Areas.Reimbursment.Repository;
using NoiSys.Areas.Reimbursment.ViewModels;
using NoiSys.Areas.Transaksi.Models;
using NoiSys.Areas.Transaksi.Repository;
using NoiSys.Data;
using System.Data;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace NoiSys.Areas.Reimbursment.Controllers
{
    [Area("Reimbursment")]
    [Route("Reimbursment/[Controller]/[Action]")]
    [Authorize(Roles = "Pengajuan")]
    public class PengajuanController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IPenggunaRepository _penggunaRepository;
        private readonly IPengajuanRepository _pengajuanRepository;
        private readonly IItemReimbursmentRepository _itemReimbursmentRepository;
        private readonly IBankRepository _bankRepository;
        private readonly IPersetujuanRepository _persetujuanRepository;

        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        public PengajuanController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext applicationDbContext,
            IPenggunaRepository penggunaRepository,
            IPengajuanRepository pengajuanRepository,
            IItemReimbursmentRepository itemReimbursmentRepository,
            IBankRepository bankRepository,
            IPersetujuanRepository persetujuanRepository,

            IHostingEnvironment hostingEnvironment,
            IWebHostEnvironment webHostEnvironment,
            IConfiguration configuration
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _applicationDbContext = applicationDbContext;
            _penggunaRepository = penggunaRepository;
            _pengajuanRepository = pengajuanRepository;
            _itemReimbursmentRepository = itemReimbursmentRepository;
            _bankRepository = bankRepository;
            _persetujuanRepository = persetujuanRepository;

            _hostingEnvironment = hostingEnvironment;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }

        public JsonResult LoadItemReimbursment(Guid Id)
        {
            var item = _applicationDbContext.ItemReimbursments.Where(p => p.ItemReimbursmentId == Id).FirstOrDefault();
            return new JsonResult(item);
        }

        [Authorize(Roles = "IndexPengajuan")]
        public IActionResult Index()
        {
            var data = _pengajuanRepository.GetAllPengajuan();
            return View(data);
        }

        [Authorize(Roles = "CreatePengajuan")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> CreatePengajuan()
        {
            ViewBag.Item = new SelectList(await _itemReimbursmentRepository.GetItemReimbursments(), "ItemReimbursmentId", "NamaItemReimbursment", SortOrder.Ascending);
            ViewBag.Users = new SelectList(_userManager.Users, nameof(ApplicationUser.Id), nameof(ApplicationUser.NamaLengkap), SortOrder.Ascending);
            ViewBag.Bank = new SelectList(await _bankRepository.GetBanks(), "BankId", "NamaBank", SortOrder.Ascending);

            _signInManager.IsSignedIn(User);
            var getUser = _penggunaRepository.GetAllUserLogin().Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            Pengajuan pengajuan = new Pengajuan()
            {
                UserId = getUser.Id,
            };
            pengajuan.PengajuanDetails.Add(new PengajuanDetail() { PengajuanDetailId = Guid.NewGuid() });

            var dateNow = DateTimeOffset.Now;
            var setDateNow = DateTimeOffset.Now.ToString("yyMMdd");

            var lastCode = _pengajuanRepository.GetAllPengajuan().Where(d => d.CreateDateTime.ToString("yyMMdd") == dateNow.ToString("yyMMdd")).OrderByDescending(k => k.PengajuanNumber).FirstOrDefault();
            if (lastCode == null)
            {
                pengajuan.PengajuanNumber = "RBS" + setDateNow + "0001";
            }
            else
            {
                var lastCodeTrim = lastCode.PengajuanNumber.Substring(3, 6);

                if (lastCodeTrim != setDateNow)
                {
                    pengajuan.PengajuanNumber = "RBS" + setDateNow + "0001";
                }
                else
                {
                    pengajuan.PengajuanNumber = "RBS" + setDateNow + (Convert.ToInt32(lastCode.PengajuanNumber.Substring(9, lastCode.PengajuanNumber.Length - 9)) + 1).ToString("D4");
                }
            }

            return View(pengajuan);
        }

        [Authorize(Roles = "CreatePengajuan")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreatePengajuan(Pengajuan model)
        {
            ViewBag.Item = new SelectList(await _itemReimbursmentRepository.GetItemReimbursments(), "ItemReimbursmentId", "NamaItemReimbursment", SortOrder.Ascending);
            ViewBag.Users = new SelectList(_userManager.Users, nameof(ApplicationUser.Id), nameof(ApplicationUser.NamaLengkap), SortOrder.Ascending);
            ViewBag.Bank = new SelectList(await _bankRepository.GetBanks(), "BankId", "NamaBank", SortOrder.Ascending);

            var dateNow = DateTimeOffset.Now;
            var setDateNow = DateTimeOffset.Now.ToString("yyMMdd");

            var lastCode = _pengajuanRepository.GetAllPengajuan().Where(d => d.CreateDateTime.ToString("yyMMdd") == dateNow.ToString("yyMMdd")).OrderByDescending(k => k.PengajuanNumber).FirstOrDefault();
            if (lastCode == null)
            {
                model.PengajuanNumber = "RBS" + setDateNow + "0001";
            }
            else
            {
                var lastCodeTrim = lastCode.PengajuanNumber.Substring(3, 6);

                if (lastCodeTrim != setDateNow)
                {
                    model.PengajuanNumber = "RBS" + setDateNow + "0001";
                }
                else
                {
                    model.PengajuanNumber = "RBS" + setDateNow + (Convert.ToInt32(lastCode.PengajuanNumber.Substring(9, lastCode.PengajuanNumber.Length - 9)) + 1).ToString("D4");
                }
            }

            var getUser = _penggunaRepository.GetAllUserLogin().Where(u => u.UserName == User.Identity.Name).FirstOrDefault();            

            if (ModelState.IsValid)
            {
                var pengajuan = new Pengajuan
                {
                    CreateDateTime = DateTime.Now,
                    CreateBy = new Guid(getUser.Id), //Convert Guid to String
                    PengajuanId = model.PengajuanId,
                    PengajuanNumber = model.PengajuanNumber,
                    UserId = getUser.Id,
                    BankId = model.BankId,
                    NomorRekening = model.NomorRekening,
                    AtasNama = model.AtasNama,
                    Status = model.Status,
                    QtyTotal = model.QtyTotal,
                    GrandTotal = model.GrandTotal,
                    PengajuanDetails = model.PengajuanDetails,
                };

                var ItemsList = new List<PengajuanDetail>();

                foreach (var item in model.PengajuanDetails)
                {
                    ItemsList.Add(new PengajuanDetail
                    {
                        CreateDateTime = DateTime.Now,
                        CreateBy = new Guid(getUser.Id),
                        KodeItem = item.KodeItem,
                        NamaItem = item.NamaItem,
                        Qty = item.Qty,
                        Nominal = item.Nominal,
                        SubTotal = item.SubTotal,
                        Catatan = item.Catatan,
                    });
                }

                pengajuan.PengajuanDetails = ItemsList;
                _pengajuanRepository.Tambah(pengajuan);

                var approve = new Persetujuan
                {
                    CreateDateTime = DateTime.Now,
                    CreateBy = new Guid(getUser.Id),
                    PengajuanId = pengajuan.PengajuanId,
                    PengajuanNumber = pengajuan.PengajuanNumber,
                    UserId = getUser.Id.ToString(),
                    ApproveDate = DateTime.MinValue,
                    ApproveBy = "",
                    BankId = pengajuan.BankId, 
                    NomorRekening = pengajuan.NomorRekening,
                    AtasNama = pengajuan.AtasNama,
                    Status = pengajuan.Status,
                    Keterangan = "",
                };
                _persetujuanRepository.Tambah(approve);

                TempData["SuccessMessage"] = "No. Pengajuan " + model.PengajuanNumber + " Berhasil Disimpan";                
                return Json(new { redirectToUrl = Url.Action("Index", "Pengajuan") });
            }
            else
            {
                ViewBag.Item = new SelectList(await _itemReimbursmentRepository.GetItemReimbursments(), "ItemReimbursmentId", "NamaItemReimbursment", SortOrder.Ascending);
                ViewBag.Users = new SelectList(_userManager.Users, nameof(ApplicationUser.Id), nameof(ApplicationUser.NamaLengkap), SortOrder.Ascending);
                ViewBag.Bank = new SelectList(await _bankRepository.GetBanks(), "BankId", "NamaBank", SortOrder.Ascending);
                TempData["WarningMessage"] = "Terdapat data yang masih kosong !!!";
                return View(model);
            }
        }

        [HttpGet]
        [Authorize(Roles = "DetailPengajuan")]
        [AllowAnonymous]
        public async Task<IActionResult> DetailPengajuan(Guid Id)
        {
            ViewBag.Item = new SelectList(await _itemReimbursmentRepository.GetItemReimbursments(), "ItemReimbursmentId", "NamaItemReimbursment", SortOrder.Ascending);
            ViewBag.Users = new SelectList(_userManager.Users, nameof(ApplicationUser.Id), nameof(ApplicationUser.NamaLengkap), SortOrder.Ascending);
            ViewBag.Bank = new SelectList(await _bankRepository.GetBanks(), "BankId", "NamaBank", SortOrder.Ascending);

            var pengajuan = await _pengajuanRepository.GetPengajuanById(Id);

            if (pengajuan == null)
            {
                Response.StatusCode = 404;
                return View("PengajuanNotFound", Id);
            }

            var getUser = _penggunaRepository.GetAllUserLogin().Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            var model = new Pengajuan
            {
                PengajuanId = pengajuan.PengajuanId,
                PengajuanNumber = pengajuan.PengajuanNumber,
                UserId = getUser.Id,
                BankId = pengajuan.BankId,
                NomorRekening = pengajuan.NomorRekening,
                AtasNama = pengajuan.AtasNama,
                Status = pengajuan.Status,
                QtyTotal = pengajuan.QtyTotal,
                GrandTotal = Math.Truncate(pengajuan.GrandTotal),
                PengajuanDetails = pengajuan.PengajuanDetails,
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "DetailPengajuan")]
        [AllowAnonymous]
        public async Task<IActionResult> DetailPengajuan(Pengajuan model)
        {
            ViewBag.Item = new SelectList(await _itemReimbursmentRepository.GetItemReimbursments(), "ItemReimbursmentId", "NamaItemReimbursment", SortOrder.Ascending);
            ViewBag.Users = new SelectList(_userManager.Users, nameof(ApplicationUser.Id), nameof(ApplicationUser.NamaLengkap), SortOrder.Ascending);
            ViewBag.Bank = new SelectList(await _bankRepository.GetBanks(), "BankId", "NamaBank", SortOrder.Ascending);

            var getUser = _penggunaRepository.GetAllUserLogin().Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            if (ModelState.IsValid)
            {
                var pengajuan =  _pengajuanRepository.GetAllPengajuan().Where(p => p.PengajuanNumber == model.PengajuanNumber).FirstOrDefault();                

                if (pengajuan != null)
                {
                    pengajuan.UpdateDateTime = DateTime.Now;
                    pengajuan.UpdateBy = new Guid(getUser.Id);
                    pengajuan.BankId = model.BankId;
                    pengajuan.NomorRekening = model.NomorRekening;
                    pengajuan.AtasNama = model.AtasNama;
                    pengajuan.QtyTotal = model.QtyTotal;
                    pengajuan.GrandTotal = model.GrandTotal;
                    pengajuan.PengajuanDetails = model.PengajuanDetails;
                    _pengajuanRepository.Update(pengajuan);

                    TempData["SuccessMessage"] = "No. Pengajuan " + model.PengajuanNumber + " Berhasil Diubah";
                    return Json(new { redirectToUrl = Url.Action("Index", "Pengajuan") });
                }
                else
                {
                    ViewBag.Item = new SelectList(await _itemReimbursmentRepository.GetItemReimbursments(), "ItemReimbursmentId", "NamaItemReimbursment", SortOrder.Ascending);
                    ViewBag.Users = new SelectList(_userManager.Users, nameof(ApplicationUser.Id), nameof(ApplicationUser.NamaLengkap), SortOrder.Ascending);
                    ViewBag.Bank = new SelectList(await _bankRepository.GetBanks(), "BankId", "NamaBank", SortOrder.Ascending);
                    TempData["WarningMessage"] = "No. Pengajuan " + model.PengajuanNumber + " sudah ada !!!";
                    return View(model);
                }
            }
            ViewBag.Item = new SelectList(await _itemReimbursmentRepository.GetItemReimbursments(), "ItemReimbursmentId", "NamaItemReimbursment", SortOrder.Ascending);
            ViewBag.Users = new SelectList(_userManager.Users, nameof(ApplicationUser.Id), nameof(ApplicationUser.NamaLengkap), SortOrder.Ascending);
            ViewBag.Bank = new SelectList(await _bankRepository.GetBanks(), "BankId", "NamaBank", SortOrder.Ascending);
            TempData["WarningMessage"] = "No. Pengajuan " + model.PengajuanNumber + " Gagal Disimpan";
            return Json(new { redirectToUrl = Url.Action("Index", "Pengajuan") });
        }

        public async Task<IActionResult> PrintPengajuan(Guid Id)
        {
            var pengajuan = await _pengajuanRepository.GetPengajuanById(Id);            

            var getRbsNumber = pengajuan.PengajuanNumber;
            var getDateNow = DateTime.Now.ToString("dd MMMM yyyy");
            var getBank = pengajuan.Bank.NamaBank;
            var getNoRek = pengajuan.NomorRekening;
            var getAtasNama = pengajuan.AtasNama;
            var getGrandTotal = pengajuan.GrandTotal;
            var getDibuatOleh = pengajuan.ApplicationUser.NamaLengkap;

            WebReport web = new WebReport();
            var path = $"{_webHostEnvironment.WebRootPath}\\Reporting\\Reimbursment.frx";
            web.Report.Load(path);

            var msSqlDataConnection = new MsSqlDataConnection();
            msSqlDataConnection.ConnectionString = _configuration.GetConnectionString("DefaultConnection");
            var Conn = msSqlDataConnection.ConnectionString;

            web.Report.SetParameterValue("Conn", Conn);
            web.Report.SetParameterValue("PengajuanId", Id.ToString());
            web.Report.SetParameterValue("PengajuanNumber", getRbsNumber);
            web.Report.SetParameterValue("RbsDateNow", getDateNow);
            web.Report.SetParameterValue("Bank", getBank);
            web.Report.SetParameterValue("NoRekening", getNoRek);
            web.Report.SetParameterValue("AtasNama", getAtasNama);
            web.Report.SetParameterValue("GrandTotal", getGrandTotal);
            web.Report.SetParameterValue("DibuatOleh", getDibuatOleh);

            web.Report.Prepare();
            Stream stream = new MemoryStream();
            web.Report.Export(new PDFSimpleExport(), stream);
            stream.Position = 0;
            return File(stream, "application/zip", (getRbsNumber + ".pdf"));
        }
    }
}
