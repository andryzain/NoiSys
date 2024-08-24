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
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Hosting;
using NoiSys.Areas.Identity.Data;
using NoiSys.Areas.MasterData.Models;
using NoiSys.Areas.MasterData.Repository;
using NoiSys.Areas.Order.Models;
using NoiSys.Areas.Transaksi.Models;
using NoiSys.Areas.Transaksi.Repository;
using NoiSys.Areas.Transaksi.ViewModels;
using NoiSys.Data;
using System.Data;
using System.Reflection.Metadata.Ecma335;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace NoiSys.Areas.Transaksi.Controllers
{
    [Area("Transaksi")]
    [Route("Transaksi/[Controller]/[Action]")]
    [Authorize(Roles = "PurchaseRequest")]
    public class PurchaseRequestController : Controller
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

        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;


        public PurchaseRequestController(
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

            _hostingEnvironment = hostingEnvironment;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }

        public JsonResult LoadProduk(Guid Id)
        {
            var produk = _applicationDbContext.Produks.Include(p => p.Principal).Include(s => s.Satuan).Include(d => d.Diskon).Where(p => p.ProdukId == Id).FirstOrDefault();
            return new JsonResult(produk);
        }

        [Authorize(Roles = "IndexPurchaseRequest")]
        public IActionResult Index()
        {
            var data = _purchaseRequestRepository.GetAllPurchaseRequest();
            return View(data);
        }

        [Authorize(Roles = "CreatePurchaseRequest")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> CreatePurchaseRequest()
        {
            _signInManager.IsSignedIn(User);
            var getUser = _penggunaRepository.GetAllUserLogin().Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            ViewBag.Produk = new SelectList(await _produkRepository.GetProduks(), "ProdukId", "NamaProduk", SortOrder.Ascending);
            ViewBag.Termin = new SelectList(await _metodePembayaranRepository.GetMetodePembayarans(), "MetodePembayaranId", "NamaMetodePembayaran", SortOrder.Ascending);
            ViewBag.Approval = new SelectList(await _penggunaRepository.GetPenggunas(), "PenggunaId", "NamaLengkap", SortOrder.Ascending);
            ViewBag.Bengkel = new SelectList(await _bengkelRepository.GetBengkels(), "BengkelId", "NamaBengkel", SortOrder.Ascending);

            PurchaseRequest purchaseRequest = new PurchaseRequest()
            {
                UserId = getUser.Id,
            };
            purchaseRequest.PurchaseRequestDetails.Add(new PurchaseRequestDetail() { PurchaseRequestDetailId = Guid.NewGuid() });

            var dateNow = DateTimeOffset.Now;
            var setDateNow = DateTimeOffset.Now.ToString("yyMMdd");

            var lastCode = _purchaseRequestRepository.GetAllPurchaseRequest().Where(d => d.CreateDateTime.ToString("yyMMdd") == dateNow.ToString("yyMMdd")).OrderByDescending(k => k.PurchaseRequestNumber).FirstOrDefault();
            if (lastCode == null)
            {
                purchaseRequest.PurchaseRequestNumber = "PR" + setDateNow + "0001";
            }
            else
            {
                var lastCodeTrim = lastCode.PurchaseRequestNumber.Substring(2, 6);

                if (lastCodeTrim != setDateNow)
                {
                    purchaseRequest.PurchaseRequestNumber = "PR" + setDateNow + "0001";
                }
                else
                {
                    purchaseRequest.PurchaseRequestNumber = "PR" + setDateNow + (Convert.ToInt32(lastCode.PurchaseRequestNumber.Substring(9, lastCode.PurchaseRequestNumber.Length - 9)) + 1).ToString("D4");
                }
            }

            return View(purchaseRequest);
        }

        [Authorize(Roles = "CreatePurchaseRequest")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreatePurchaseRequest(PurchaseRequest model)
        {
            ViewBag.Produk = new SelectList(await _produkRepository.GetProduks(), "ProdukId", "NamaProduk", SortOrder.Ascending);
            ViewBag.Termin = new SelectList(await _metodePembayaranRepository.GetMetodePembayarans(), "MetodePembayaranId", "NamaMetodePembayaran", SortOrder.Ascending);
            ViewBag.Approval = new SelectList(await _penggunaRepository.GetPenggunas(), "PenggunaId", "NamaLengkap", SortOrder.Ascending);
            ViewBag.Bengkel = new SelectList(await _bengkelRepository.GetBengkels(), "BengkelId", "NamaBengkel", SortOrder.Ascending);

            var dateNow = DateTimeOffset.Now;
            var setDateNow = DateTimeOffset.Now.ToString("yyMMdd");

            var lastCode = _purchaseRequestRepository.GetAllPurchaseRequest().Where(d => d.CreateDateTime.ToString("yyMMdd") == dateNow.ToString("yyMMdd")).OrderByDescending(k => k.PurchaseRequestNumber).FirstOrDefault();
            if (lastCode == null)
            {
                model.PurchaseRequestNumber = "PR" + setDateNow + "0001";
            }
            else
            {
                var lastCodeTrim = lastCode.PurchaseRequestNumber.Substring(2, 6);

                if (lastCodeTrim != setDateNow)
                {
                    model.PurchaseRequestNumber = "PR" + setDateNow + "0001";
                }
                else
                {
                    model.PurchaseRequestNumber = "PR" + setDateNow + (Convert.ToInt32(lastCode.PurchaseRequestNumber.Substring(9, lastCode.PurchaseRequestNumber.Length - 9)) + 1).ToString("D4");
                }
            }

            var getUser = _penggunaRepository.GetAllUserLogin().Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            //var getApproval = _penggunaRepository.GetAllUserLogin().Where(n => n.Id == model.Approval).FirstOrDefault();

            if (ModelState.IsValid)
            {
                var purchaseRequest = new PurchaseRequest
                {
                    CreateDateTime = DateTime.Now,
                    CreateBy = new Guid(getUser.Id), //Convert Guid to String
                    PurchaseRequestId = model.PurchaseRequestId,
                    PurchaseRequestNumber = model.PurchaseRequestNumber,
                    UserId = getUser.Id,
                    PenggunaId = model.PenggunaId,
                    Termin = model.Termin,
                    BengkelId = model.BengkelId,
                    Status = model.Status,
                    QtyTotal = model.QtyTotal,
                    GrandTotal = Math.Truncate(model.GrandTotal),
                    Keterangan = model.Keterangan,                    
                };

                var ItemsList = new List<PurchaseRequestDetail>();

                foreach (var item in model.PurchaseRequestDetails)
                {
                    ItemsList.Add(new PurchaseRequestDetail
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

                purchaseRequest.PurchaseRequestDetails = ItemsList;
                _purchaseRequestRepository.Tambah(purchaseRequest);

                var approval = new ApprovalPurchaseRequest
                {
                    CreateDateTime = DateTime.Now,
                    CreateBy = new Guid(getUser.Id),
                    PurchaseRequestId = purchaseRequest.PurchaseRequestId,
                    PurchaseRequestNumber = purchaseRequest.PurchaseRequestNumber,
                    UserId = getUser.Id.ToString(),
                    PenggunaId = purchaseRequest.PenggunaId,
                    BengkelId = purchaseRequest.BengkelId,
                    ApproveDate = DateTime.MinValue,
                    ApproveBy = "",                    
                    Status = purchaseRequest.Status,
                    Keterangan = purchaseRequest.Keterangan,
                };
                _approvalPurchaseRequestRepository.Tambah(approval);                

                TempData["SuccessMessage"] = "No. Purchase Request " + model.PurchaseRequestNumber + " Berhasil Disimpan";              
                return Json(new { redirectToUrl = Url.Action("Index", "PurchaseRequest") });                
            }
            else
            {
                ViewBag.Produk = new SelectList(await _produkRepository.GetProduks(), "ProdukId", "NamaProduk", SortOrder.Ascending);
                ViewBag.Termin = new SelectList(await _metodePembayaranRepository.GetMetodePembayarans(), "MetodePembayaranId", "NamaMetodePembayaran", SortOrder.Ascending);
                ViewBag.Approval = new SelectList(await _penggunaRepository.GetPenggunas(), "PenggunaId", "NamaLengkap", SortOrder.Ascending);
                ViewBag.Bengkel = new SelectList(await _bengkelRepository.GetBengkels(), "BengkelId", "NamaBengkel", SortOrder.Ascending);
                TempData["WarningMessage"] = "Terdapat data yang masih kosong !!!";
                return View(model);
            }
        }

        [HttpGet]
        [Authorize(Roles = "DetailPurchaseRequest")]
        [AllowAnonymous]
        public async Task<IActionResult> DetailPurchaseRequest(Guid Id)
        {
            ViewBag.Produk = new SelectList(await _produkRepository.GetProduks(), "ProdukId", "NamaProduk", SortOrder.Ascending);
            ViewBag.Termin = new SelectList(await _metodePembayaranRepository.GetMetodePembayarans(), "MetodePembayaranId", "NamaMetodePembayaran", SortOrder.Ascending);
            ViewBag.Approval = new SelectList(await _penggunaRepository.GetPenggunas(), "PenggunaId", "NamaLengkap", SortOrder.Ascending);
            ViewBag.Bengkel = new SelectList(await _bengkelRepository.GetBengkels(), "BengkelId", "NamaBengkel", SortOrder.Ascending);

            var purchaseRequest = await _purchaseRequestRepository.GetPurchaseRequestById(Id);

            if (purchaseRequest == null)
            {
                Response.StatusCode = 404;
                return View("PurchaseRequestNotFound", Id);
            }

            PurchaseRequest model = new PurchaseRequest
            {
                PurchaseRequestId = purchaseRequest.PurchaseRequestId,
                PurchaseRequestNumber = purchaseRequest.PurchaseRequestNumber,
                UserId = purchaseRequest.UserId,
                PenggunaId = purchaseRequest.PenggunaId,
                Termin = purchaseRequest.Termin,
                BengkelId = purchaseRequest.BengkelId,
                Status = purchaseRequest.Status,
                QtyTotal = purchaseRequest.QtyTotal,
                GrandTotal = Math.Truncate(purchaseRequest.GrandTotal),
                Keterangan = purchaseRequest.Keterangan                
            };

            var ItemsList = new List<PurchaseRequestDetail>();

            foreach (var item in purchaseRequest.PurchaseRequestDetails)
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

            model.PurchaseRequestDetails = ItemsList;
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "DetailPurchaseRequest")]
        [AllowAnonymous]
        public async Task<IActionResult> DetailPurchaseRequest(PurchaseRequest model)
        {
            var getUser = _penggunaRepository.GetAllUserLogin().Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            if (ModelState.IsValid)
            {
                PurchaseRequest purchaseRequest = _purchaseRequestRepository.GetAllPurchaseRequest().Where(p => p.PurchaseRequestNumber == model.PurchaseRequestNumber).FirstOrDefault();
                ApprovalPurchaseRequest approval = _approvalPurchaseRequestRepository.GetAllApprovalPurchaseRequest().Where(p => p.PurchaseRequestNumber == model.PurchaseRequestNumber).FirstOrDefault();

                if (purchaseRequest != null)
                {
                    if (approval != null)
                    {
                        purchaseRequest.UpdateDateTime = DateTime.Now;
                        purchaseRequest.UpdateBy = new Guid(getUser.Id);
                        purchaseRequest.Termin = model.Termin;
                        purchaseRequest.BengkelId = model.BengkelId;
                        purchaseRequest.PenggunaId = model.PenggunaId;
                        purchaseRequest.Keterangan = model.Keterangan;
                        purchaseRequest.PurchaseRequestDetails = model.PurchaseRequestDetails;
                        purchaseRequest.QtyTotal = model.QtyTotal;
                        purchaseRequest.GrandTotal = model.GrandTotal;

                        approval.ApproveDate = DateTime.MinValue;
                        approval.PenggunaId = model.PenggunaId;
                        approval.BengkelId = model.BengkelId;
                        approval.Keterangan = model.Keterangan;                        

                        _applicationDbContext.Entry(approval).State = EntityState.Modified;
                        _purchaseRequestRepository.Update(purchaseRequest);

                        TempData["SuccessMessage"] = "No. Purchase Request " + model.PurchaseRequestNumber + " Berhasil Diubah";
                        return Json(new { redirectToUrl = Url.Action("Index", "PurchaseRequest") });
                    }
                    else
                    {
                        TempData["WarningMessage"] = "No. Purchase Request " + model.PurchaseRequestNumber + " tidak ditemukan !!!";
                        return View(model);
                    }                    
                }
                else
                {
                    TempData["WarningMessage"] = "No. Purchase Request " + model.PurchaseRequestNumber + " sudah ada !!!";
                    return View(model);
                }
            }
            TempData["WarningMessage"] = "No. Purchase Request " + model.PurchaseRequestNumber + " Gagal Disimpan";
            return Json(new { redirectToUrl = Url.Action("Index", "PurchaseRequest") });            
        }

        [HttpGet]
        [Authorize(Roles = "GeneratePurchaseOrder-PurchaseRequest")]
        [AllowAnonymous]
        public async Task<IActionResult> GeneratePurchaseOrder(Guid Id)
        {
            ViewBag.Produk = new SelectList(await _produkRepository.GetProduks(), "ProdukId", "NamaProduk", SortOrder.Ascending);
            ViewBag.Termin = new SelectList(await _metodePembayaranRepository.GetMetodePembayarans(), "MetodePembayaranId", "NamaMetodePembayaran", SortOrder.Ascending);
            ViewBag.Approval = new SelectList(await _penggunaRepository.GetPenggunas(), "PenggunaId", "NamaLengkap", SortOrder.Ascending);
            ViewBag.Bengkel = new SelectList(await _bengkelRepository.GetBengkels(), "BengkelId", "NamaBengkel", SortOrder.Ascending);

            PurchaseRequest purchaseRequest = _applicationDbContext.PurchaseRequests
                .Include(d => d.PurchaseRequestDetails)
                .Include(u => u.ApplicationUser)
                .Include(p => p.Pengguna)
                .Include(b => b.Bengkel)
                .Where(p => p.PurchaseRequestId == Id).FirstOrDefault();

            _signInManager.IsSignedIn(User);

            var getUser = _penggunaRepository.GetAllUserLogin().Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            PurchaseOrder po = new PurchaseOrder();

            var dateNow = DateTimeOffset.Now;
            var setDateNow = DateTimeOffset.Now.ToString("yyMMdd");

            var lastCode = _purchaseOrderRepository.GetAllPurchaseOrder().Where(d => d.CreateDateTime.ToString("yyMMdd") == dateNow.ToString("yyMMdd")).OrderByDescending(k => k.PurchaseOrderNumber).FirstOrDefault();
            if (lastCode == null)
            {
                po.PurchaseOrderNumber = "PO" + setDateNow + "0001";
            }
            else
            {
                var lastCodeTrim = lastCode.PurchaseOrderNumber.Substring(2, 6);

                if (lastCodeTrim != setDateNow)
                {
                    po.PurchaseOrderNumber = "PO" + setDateNow + "0001";
                }
                else
                {
                    po.PurchaseOrderNumber = "PO" + setDateNow + (Convert.ToInt32(lastCode.PurchaseRequestNumber.Substring(9, lastCode.PurchaseRequestNumber.Length - 9)) + 1).ToString("D4");
                }
            }

            ViewBag.PurchaseOrderNumber = po.PurchaseOrderNumber;

            var getPr = new PurchaseRequest()
            {
                PurchaseRequestId = purchaseRequest.PurchaseRequestId,
                PurchaseRequestNumber = purchaseRequest.PurchaseRequestNumber,
                UserId = purchaseRequest.UserId,
                PenggunaId = purchaseRequest.PenggunaId,
                Termin = purchaseRequest.Termin,
                BengkelId = purchaseRequest.BengkelId,
                Status = purchaseRequest.Status,
                QtyTotal = purchaseRequest.QtyTotal,
                GrandTotal = Math.Truncate(purchaseRequest.GrandTotal),
                Keterangan = purchaseRequest.Keterangan
            };

            var ItemsList = new List<PurchaseRequestDetail>();

            foreach (var item in purchaseRequest.PurchaseRequestDetails)
            {
                ItemsList.Add(new PurchaseRequestDetail
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

            getPr.PurchaseRequestDetails = ItemsList;

            return View(getPr);
        }

        [HttpPost]
        [Authorize(Roles = "GeneratePurchaseOrder-PurchaseRequest")]
        [AllowAnonymous]
        public async Task<IActionResult> GeneratePurchaseOrder(PurchaseRequest model)
        {
            PurchaseRequest purchaseRequest = await _purchaseRequestRepository.GetPurchaseRequestByIdNoTracking(model.PurchaseRequestId);            

            _signInManager.IsSignedIn(User);

            var getUser = _penggunaRepository.GetAllUserLogin().Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            
            string getPurchaseOrderNumber = Request.Form["PONumber"];

            var updatePurchaseRequest = _purchaseRequestRepository.GetAllPurchaseRequest().Where(c => c.PurchaseRequestId == model.PurchaseRequestId).FirstOrDefault();
            if (updatePurchaseRequest != null) {             
                {
                    updatePurchaseRequest.Status = getPurchaseOrderNumber;
                };
                _applicationDbContext.Entry(updatePurchaseRequest).State = EntityState.Modified;
            }

            var newPurchaseOrder = new PurchaseOrder
            {
                CreateDateTime = DateTime.Now,
                CreateBy = new Guid(getUser.Id),
                PurchaseRequestId = purchaseRequest.PurchaseRequestId,
                PurchaseRequestNumber = purchaseRequest.PurchaseRequestNumber,
                UserId = getUser.Id.ToString(),
                PenggunaId = purchaseRequest.PenggunaId,
                Termin = purchaseRequest.Termin,
                BengkelId = purchaseRequest.BengkelId,                
                Status = "Diproses",
                QtyTotal = purchaseRequest.QtyTotal,
                GrandTotal = Math.Truncate(purchaseRequest.GrandTotal),
                Keterangan = purchaseRequest.Keterangan,
            };

            newPurchaseOrder.PurchaseOrderNumber = getPurchaseOrderNumber;            

            var ItemsList = new List<PurchaseOrderDetail>();

            foreach (var item in purchaseRequest.PurchaseRequestDetails)
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

            newPurchaseOrder.PurchaseOrderDetails = ItemsList;
            
            _purchaseOrderRepository.Tambah(newPurchaseOrder);
            
            TempData["SuccessMessage"] = "No. " + newPurchaseOrder.PurchaseOrderNumber + " Berhasil Disimpan";
            return RedirectToAction("Index", "PurchaseOrder");
        }

        public async Task<IActionResult> PrintPurchaseRequest(Guid Id)
        {
            var purchaseRequest = await _purchaseRequestRepository.GetPurchaseRequestById(Id);
            var term = _metodePembayaranRepository.GetAllMetodePembayaran().Where(t => t.MetodePembayaranId == new Guid (purchaseRequest.Termin)).FirstOrDefault();
            var termin = term.NamaMetodePembayaran;

            var getPrNumber = purchaseRequest.PurchaseRequestNumber;
            var getBengkel = purchaseRequest.Bengkel.NamaBengkel;
            var getTermin = termin;
            var getDateNow = DateTime.Now.ToString("dd MMMM yyyy");
            var getGrandTotal = purchaseRequest.GrandTotal;
            var getTax = (getGrandTotal / 100) * 11;
            var getGrandTotalAfterTax = (getGrandTotal + getTax);
            var getDibuatOleh = purchaseRequest.ApplicationUser.NamaLengkap;
            var getDisetujuiOleh = purchaseRequest.Pengguna.NamaLengkap;

            WebReport web = new WebReport();
            var path = $"{_webHostEnvironment.WebRootPath}\\Reporting\\PurchaseRequest.frx";
            web.Report.Load(path);

            var msSqlDataConnection = new MsSqlDataConnection();
            msSqlDataConnection.ConnectionString = _configuration.GetConnectionString("DefaultConnection");
            var Conn = msSqlDataConnection.ConnectionString;

            web.Report.SetParameterValue("Conn", Conn);
            web.Report.SetParameterValue("PurchaseRequestId", Id.ToString());
            web.Report.SetParameterValue("PrNumber", getPrNumber);
            web.Report.SetParameterValue("PrDateNow", getDateNow);
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
            return File(stream, "application/zip", (getPrNumber + ".pdf"));
        }
    }    
}