using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NoiSys.Areas.Identity.Data;
using NoiSys.Areas.MasterData.Repository;
using NoiSys.Areas.Order.Repository;
using NoiSys.Areas.Penerimaan.Models;
using NoiSys.Areas.Penerimaan.Repository;
using NoiSys.Areas.Transaksi.Models;
using NoiSys.Areas.Transaksi.Repository;
using NoiSys.Data;
using System.Data;

namespace NoiSys.Areas.Penerimaan.Controllers
{
    [Area("Penerimaan")]
    [Route("Penerimaan/[Controller]/[Action]")]
    [Authorize(Roles = "ReceiveOrder")]
    public class ReceiveOrderController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IReceiveOrderRepository _receiveOrderRepository;
        private readonly IPenggunaRepository _penggunaRepository;        
        private readonly IProdukRepository _produkRepository;
        private readonly IMetodePembayaranRepository _metodePembayaranRepository;
        private readonly IBengkelRepository _bengkelRepository;
        private readonly IPembelianRepository _pembelianRepository;
        private readonly IPermintaanRepository _permintaanRepository;


        public ReceiveOrderController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext applicationDbContext,
            IReceiveOrderRepository receiveOrderRepository,
            IPenggunaRepository penggunaRepository,
            IProdukRepository produkRepository,
            IMetodePembayaranRepository metodePembayaranRepository,
            IBengkelRepository bengkelRepository,
            IPembelianRepository pembelianRepository,
            IPermintaanRepository permintaanRepository
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _applicationDbContext = applicationDbContext;
            _receiveOrderRepository = receiveOrderRepository;
            _penggunaRepository = penggunaRepository;
            _produkRepository = produkRepository;
            _metodePembayaranRepository = metodePembayaranRepository;
            _bengkelRepository = bengkelRepository;
            _pembelianRepository = pembelianRepository;
            _permintaanRepository = permintaanRepository;
        }

        public JsonResult LoadProduk(Guid Id)
        {
            var produk = _applicationDbContext.Produks.Include(p => p.Principal).Include(s => s.Satuan).Include(d => d.Diskon).Where(p => p.ProdukId == Id).FirstOrDefault();
            return new JsonResult(produk);
        }
        public JsonResult LoadPembelian(Guid Id)
        {
            var podetail = _applicationDbContext.Pembelians
                .Include(p => p.PembelianDetails)
                .Where(p => p.PembelianId == Id).FirstOrDefault();
            return new JsonResult(podetail);
        }
        public JsonResult LoadPembelianDetail(Guid Id)
        {
            var podetail = _applicationDbContext.PembelianDetails
                .Where(p => p.PembelianDetailId == Id).FirstOrDefault();
            return new JsonResult(podetail);
        }

        [AllowAnonymous]
        [Authorize(Roles = "IndexReceiveOrder")]
        public IActionResult Index()
        {            
            var data = _receiveOrderRepository.GetAllReceiveOrder();
            return View(data);
        }       

        [Authorize(Roles = "CreateReceiveOrder")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> CreateReceiveOrder(string poList)
        {
            //Pembelian Pembelian = await _pembelianRepository.GetAllPembelian().Where(p => p.PembelianNumber == );

            _signInManager.IsSignedIn(User);
            var getUser = _penggunaRepository.GetAllUserLogin().Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            ViewBag.Produk = new SelectList(await _produkRepository.GetProduks(), "ProdukId", "NamaProduk", SortOrder.Ascending);            
            ViewBag.Approval = new SelectList(await _penggunaRepository.GetPenggunas(), "PenggunaId", "NamaLengkap", SortOrder.Ascending);
            ViewBag.PembelianFilter = new SelectList(await _pembelianRepository.GetPembelianFilters(), "PembelianId", "PembelianNumber", SortOrder.Ascending);

            ReceiveOrder ReceiveOrder = new ReceiveOrder()
            {
                ReceiveById = getUser.Id,
            };
            //ReceiveOrder.ReceiveOrderDetails.Add(new ReceiveOrderDetail() { ReceiveOrderDetailId = Guid.NewGuid() });

            var dateNow = DateTimeOffset.Now;
            var setDateNow = DateTimeOffset.Now.ToString("yyMMdd");

            var lastCode = _receiveOrderRepository.GetAllReceiveOrder().Where(d => d.CreateDateTime.ToString("yyMMdd") == dateNow.ToString("yyMMdd")).OrderByDescending(k => k.ReceiveOrderNumber).FirstOrDefault();
            if (lastCode == null)
            {
                ReceiveOrder.ReceiveOrderNumber = "RO" + setDateNow + "0001";
            }
            else
            {
                var lastCodeTrim = lastCode.ReceiveOrderNumber.Substring(2, 6);

                if (lastCodeTrim != setDateNow)
                {
                    ReceiveOrder.ReceiveOrderNumber = "RO" + setDateNow + "0001";
                }
                else
                {
                    ReceiveOrder.ReceiveOrderNumber = "RO" + setDateNow + (Convert.ToInt32(lastCode.ReceiveOrderNumber.Substring(9, lastCode.ReceiveOrderNumber.Length - 9)) + 1).ToString("D4");
                }
            }

            return View(ReceiveOrder);
        }

        [Authorize(Roles = "CreateReceiveOrder")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateReceiveOrder(ReceiveOrder model)
        {
            ViewBag.Produk = new SelectList(await _produkRepository.GetProduks(), "ProdukId", "NamaProduk", SortOrder.Ascending);
            ViewBag.Approval = new SelectList(await _penggunaRepository.GetPenggunas(), "PenggunaId", "NamaLengkap", SortOrder.Ascending);
            ViewBag.PembelianFilter = new SelectList(await _pembelianRepository.GetPembelianFilters(), "PembelianId", "PembelianNumber", SortOrder.Ascending);

            var dateNow = DateTimeOffset.Now;
            var setDateNow = DateTimeOffset.Now.ToString("yyMMdd");

            var lastCode = _receiveOrderRepository.GetAllReceiveOrder().Where(d => d.CreateDateTime.ToString("yyMMdd") == dateNow.ToString("yyMMdd")).OrderByDescending(k => k.ReceiveOrderNumber).FirstOrDefault();
            if (lastCode == null)
            {
                model.ReceiveOrderNumber = "RO" + setDateNow + "0001";
            }
            else
            {
                var lastCodeTrim = lastCode.ReceiveOrderNumber.Substring(2, 6);

                if (lastCodeTrim != setDateNow)
                {
                    model.ReceiveOrderNumber = "RO" + setDateNow + "0001";
                }
                else
                {
                    model.ReceiveOrderNumber = "RO" + setDateNow + (Convert.ToInt32(lastCode.ReceiveOrderNumber.Substring(9, lastCode.ReceiveOrderNumber.Length - 9)) + 1).ToString("D4");
                }
            }

            var getUser = _penggunaRepository.GetAllUserLogin().Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            //var getApproval = _penggunaRepository.GetAllUserLogin().Where(n => n.Id == model.Approval).FirstOrDefault();   

            if (ModelState.IsValid)
            {
                var receiveOrder = new ReceiveOrder
                {
                    CreateDateTime = DateTime.Now,
                    CreateBy = new Guid(getUser.Id), //Convert Guid to String
                    ReceiveOrderId = model.ReceiveOrderId,
                    ReceiveOrderNumber = model.ReceiveOrderNumber,
                    PembelianId = model.PembelianId,
                    ReceiveById = getUser.Id,
                    Status = model.Status,
                    Catatan = model.Catatan,
                    ReceiveOrderDetails = model.ReceiveOrderDetails,
                };                

                var updateStatusPembelian = _pembelianRepository.GetAllPembelian().Where(c => c.PembelianId == model.PembelianId).FirstOrDefault();
                if (updateStatusPembelian != null)
                {
                    updateStatusPembelian.UpdateDateTime = DateTime.Now;
                    updateStatusPembelian.UpdateBy = new Guid(getUser.Id);
                    updateStatusPembelian.Status = model.ReceiveOrderNumber;

                    _applicationDbContext.Entry(updateStatusPembelian).State = EntityState.Modified;
                }                

                foreach (var item in receiveOrder.ReceiveOrderDetails)
                {
                    var updateProduk = _produkRepository.GetAllProduk().Where(c => c.KodeProduk == item.KodeProduk).FirstOrDefault();
                    if (updateProduk != null)
                    {
                        updateProduk.UpdateDateTime = DateTime.Now;
                        updateProduk.UpdateBy = new Guid(getUser.Id);
                        updateProduk.JumlahStok = updateProduk.JumlahStok + item.QtyDiTerima;

                        _applicationDbContext.Entry(updateProduk).State = EntityState.Modified;
                    }
                }

                _receiveOrderRepository.Tambah(receiveOrder);
                TempData["SuccessMessage"] = "No. Receive Order " + model.ReceiveOrderNumber + " Berhasil Disimpan";
                //return RedirectToAction("Index", "ReceiveOrder");
                return Json(new { redirectToUrl = Url.Action("Index", "ReceiveOrder") });
            }
            else
            {
                ViewBag.Produk = new SelectList(await _produkRepository.GetProduks(), "ProdukId", "NamaProduk", SortOrder.Ascending);
                ViewBag.Approval = new SelectList(await _penggunaRepository.GetPenggunas(), "PenggunaId", "NamaLengkap", SortOrder.Ascending);
                ViewBag.Pembelian = new SelectList(await _pembelianRepository.GetPembelians(), "PembelianId", "PembelianNumber", SortOrder.Ascending);
                TempData["WarningMessage"] = "Terdapat data yang masih kosong !!!";
                return View(model);
            }
        }

        [HttpGet]
        [Authorize(Roles = "DetailReceiveOrder")]
        [AllowAnonymous]
        public async Task<IActionResult> DetailReceiveOrder(Guid Id)
        {
            ViewBag.Pembelian = new SelectList(await _pembelianRepository.GetPembelians(), "PembelianId", "PembelianNumber", SortOrder.Ascending);
            ViewBag.Users = new SelectList(_userManager.Users, nameof(ApplicationUser.Id), nameof(ApplicationUser.NamaLengkap), SortOrder.Ascending);

            var receiveOrder = await _receiveOrderRepository.GetReceiveOrderById(Id);

            if (receiveOrder == null)
            {
                Response.StatusCode = 404;
                return View("ReceiveOrderNotFound", Id);
            }

            ReceiveOrder model = new ReceiveOrder
            {
                ReceiveOrderId = receiveOrder.ReceiveOrderId,
                ReceiveOrderNumber = receiveOrder.ReceiveOrderNumber,
                PembelianId = receiveOrder.PembelianId,
                ReceiveById = receiveOrder.ReceiveById,                
                Status = receiveOrder.Status,
                Catatan = receiveOrder.Catatan,
                ReceiveOrderDetails = receiveOrder.ReceiveOrderDetails,
            };
            return View(model);
        }        
    }
}
