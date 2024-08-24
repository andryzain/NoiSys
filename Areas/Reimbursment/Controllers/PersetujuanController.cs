using Microsoft.AspNetCore.Authorization;
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
using NoiSys.Areas.Pengiriman.Repository;
using NoiSys.Areas.Reimbursment.Models;
using NoiSys.Areas.Reimbursment.Repository;
using NoiSys.Areas.Reimbursment.ViewModels;
using NoiSys.Areas.Transaksi.Models;
using NoiSys.Areas.Transaksi.Repository;
using NoiSys.Areas.Transaksi.ViewModels;
using NoiSys.Data;
using System.Data;

namespace NoiSys.Areas.Reimbursment.Controllers
{
    [Area("Reimbursment")]
    [Route("Reimbursment/[Controller]/[Action]")]
    [Authorize(Roles = "Persetujuan")]
    public class PersetujuanController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IPenggunaRepository _penggunaRepository;
        private readonly IPengajuanRepository _pengajuanRepository;
        private readonly IItemReimbursmentRepository _itemReimbursmentRepository;
        private readonly IBankRepository _bankRepository;
        private readonly IPersetujuanRepository _persetujuanRepository;
        private readonly IPembayaranReimbursmentRepository _pembayaranReimbursmentRepository;
        private readonly IHutangUsahaRepository _hutangUsahaRepository;

        public PersetujuanController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext applicationDbContext,
            IPenggunaRepository penggunaRepository,
            IPengajuanRepository pengajuanRepository,
            IItemReimbursmentRepository itemReimbursmentRepository,
            IBankRepository bankRepository,
            IPersetujuanRepository persetujuanRepository,
            IPembayaranReimbursmentRepository pembayaranReimbursmentRepository,
            IHutangUsahaRepository hutangUsahaRepository
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
            _pembayaranReimbursmentRepository = pembayaranReimbursmentRepository;
            _hutangUsahaRepository = hutangUsahaRepository;
        }

        [Authorize(Roles = "IndexPersetujuan")]
        public IActionResult Index()
        {
            var data = _persetujuanRepository.GetAllPersetujuan();
            return View(data);
        }

        [Authorize(Roles = "DetailPersetujuan")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ViewResult> DetailPersetujuan(Guid Id)
        {            
            ViewBag.Users = new SelectList(_userManager.Users, nameof(ApplicationUser.Id), nameof(ApplicationUser.NamaLengkap), SortOrder.Ascending);
            ViewBag.Bank = new SelectList(await _bankRepository.GetBanks(), "BankId", "NamaBank", SortOrder.Ascending);

            _signInManager.IsSignedIn(User);

            var getUser = _penggunaRepository.GetAllUserLogin().Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            var approval = await _persetujuanRepository.GetPersetujuanById(Id);

            var approvalVm = new PersetujuanViewModel()
            {
                PersetujuanId = approval.PersetujuanId,
                PengajuanId = approval.PengajuanId,
                PengajuanNumber = approval.PengajuanNumber,
                UserId = approval.UserId,
                ApproveDate = approval.ApproveDate,
                ApproveBy = getUser.NamaLengkap,
                BankId = approval.BankId,
                NomorRekening = approval.NomorRekening,
                AtasNama = approval.AtasNama,
                Status = approval.Status,
                Keterangan = approval.Keterangan,
            };

            var getPengajuan = _pengajuanRepository.GetAllPengajuan().Where(a => a.PengajuanNumber == approval.PengajuanNumber).FirstOrDefault();

            approvalVm.QtyTotal = getPengajuan.QtyTotal;
            approvalVm.GrandTotal = Math.Truncate(getPengajuan.GrandTotal);
            approvalVm.PengajuanDetails = getPengajuan.PengajuanDetails;

            return View(approvalVm);
        }

        [Authorize(Roles = "DetailPersetujuan")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> DetailPersetujuan(PersetujuanViewModel vm)
        {            
            ViewBag.Bank = new SelectList(await _bankRepository.GetBanks(), "BankId", "NamaBank", SortOrder.Ascending);

            if (ModelState.IsValid)
            {
                Persetujuan approval = await _persetujuanRepository.GetPersetujuanByIdNoTracking(vm.PersetujuanId);

                approval.Status = vm.Status;
                approval.Keterangan = vm.Keterangan;

                if (approval.Status == "Diproses" || approval.Status == "Ditolak")
                {
                    approval.CreateBy = new Guid(vm.UserId);
                    approval.CreateDateTime = DateTime.Now;
                    approval.ApproveDate = DateTime.Now;
                    approval.ApproveBy = vm.ApproveBy;
                }

                var result = _pengajuanRepository.GetAllPengajuan().Where(c => c.PengajuanNumber == vm.PengajuanNumber).FirstOrDefault();
                if (result != null)
                {
                    result.Status = vm.Status;

                    _applicationDbContext.Entry(result).State = EntityState.Modified;
                }
                _persetujuanRepository.Update(approval);

                if (approval.Status == "Belum Disetujui")
                {
                    TempData["SuccessMessage"] = "No. Pengajuan " + vm.PengajuanNumber + " belum disetujui";
                }
                else if (approval.Status == "Diproses")
                {
                    TempData["SuccessMessage"] = "No. Pengajuan " + vm.PengajuanNumber + " diproses";
                }
                else if (approval.Status == "Ditolak")
                {
                    TempData["SuccessMessage"] = "No. Pengajuan " + vm.PengajuanNumber + " ditolak";
                }
                return RedirectToAction("Index", "Persetujuan");
            }

            return View();
        }

        [HttpGet]
        [Authorize(Roles = "GeneratePayment-Persetujuan")]
        [AllowAnonymous]
        public async Task<IActionResult> GeneratePayment(Guid Id)
        {
            ViewBag.Users = new SelectList(_userManager.Users, nameof(ApplicationUser.Id), nameof(ApplicationUser.NamaLengkap), SortOrder.Ascending);
            ViewBag.Bank = new SelectList(await _bankRepository.GetBanks(), "BankId", "NamaBank", SortOrder.Ascending);            

            Persetujuan approve = _applicationDbContext.Persetujuans
                .Include(u => u.ApplicationUser)
                .Include(b => b.Bank)
                .Where(p => p.PersetujuanId == Id).FirstOrDefault();

            _signInManager.IsSignedIn(User);

            var getUser = _penggunaRepository.GetAllUserLogin().Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            
            var payment = new PembayaranReimbursment();

            var dateNow = DateTimeOffset.Now;
            var setDateNow = DateTimeOffset.Now.ToString("yyMMdd");

            var lastCode = _pembayaranReimbursmentRepository.GetAllPembayaran().Where(d => d.CreateDateTime.ToString("yyMMdd") == dateNow.ToString("yyMMdd")).OrderByDescending(k => k.PaymentNumber).FirstOrDefault();
            if (lastCode == null)
            {
                payment.PaymentNumber = "PRB" + setDateNow + "0001";
            }
            else
            {
                var lastCodeTrim = lastCode.PaymentNumber.Substring(3, 6);

                if (lastCodeTrim != setDateNow)
                {
                    payment.PaymentNumber = "PRB" + setDateNow + "0001";
                }
                else
                {
                    payment.PaymentNumber = "PRB" + setDateNow + (Convert.ToInt32(lastCode.PaymentNumber.Substring(9, lastCode.PaymentNumber.Length - 9)) + 1).ToString("D4");
                }
            }

            ViewBag.PaymentNumber = payment.PaymentNumber;

            var getPersetujuanVM = new PersetujuanViewModel()
            {
                PersetujuanId = approve.PersetujuanId,
                PengajuanId = approve.PengajuanId,
                PengajuanNumber = approve.PengajuanNumber,
                UserId = approve.UserId,
                NomorRekening = approve.NomorRekening,
                AtasNama = approve.AtasNama,
                Status = approve.Status,
                GrandTotal = Math.Truncate(approve.GrandTotal),
                Keterangan = approve.Keterangan,
            };

            return View(getPersetujuanVM);
        }

        [HttpPost]
        [Authorize(Roles = "GeneratePayment-Persetujuan")]
        [AllowAnonymous]
        public async Task<IActionResult> GeneratePayment(Persetujuan model, PersetujuanViewModel vm)
        {
            ViewBag.Users = new SelectList(_userManager.Users, nameof(ApplicationUser.Id), nameof(ApplicationUser.NamaLengkap), SortOrder.Ascending);
            ViewBag.Bank = new SelectList(await _bankRepository.GetBanks(), "BankId", "NamaBank", SortOrder.Ascending);            

            Persetujuan approve = await _persetujuanRepository.GetPersetujuanByIdNoTracking(model.PersetujuanId);
            _signInManager.IsSignedIn(User);

            var getUser = _penggunaRepository.GetAllUserLogin().Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            string getPaymentNumber = Request.Form["PAYReimburstNumber"];

            var updatePersetujuan = _persetujuanRepository.GetAllPersetujuan().Where(c => c.PersetujuanId == model.PersetujuanId).FirstOrDefault();
            if (updatePersetujuan != null)
            {
                {
                    updatePersetujuan.Status = "Selesai";
                };
                _applicationDbContext.Entry(updatePersetujuan).State = EntityState.Modified;
            }

            var updatePengajuan = _pengajuanRepository.GetAllPengajuan().Where(c => c.PengajuanId == vm.PengajuanId).FirstOrDefault();
            if (updatePengajuan != null)
            {
                {
                    updatePengajuan.Status = "Selesai";
                };
                _applicationDbContext.Entry(updatePengajuan).State = EntityState.Modified;
            }

            // Generate ke Table Pembayaran
            var newPayment = new PembayaranReimbursment
            {
                CreateDateTime = DateTime.Now,
                CreateBy = new Guid(getUser.Id),
                PengajuanId = approve.PengajuanId,
                PengajuanNumber = approve.PengajuanNumber,
                UserId = getUser.Id.ToString(),
                BankId = vm.BankId,
                NomorRekening = approve.NomorRekening,
                AtasNama = approve.AtasNama,
                Status = "Lunas",
                GrandTotal = vm.GrandTotal,
                Keterangan = approve.Keterangan,
            };

            newPayment.PaymentNumber = getPaymentNumber;

            _pembayaranReimbursmentRepository.Tambah(newPayment);

            // Generate ke Table Piutang Usaha            
            var newHutang = new HutangUsaha
            {
                CreateDateTime = DateTime.Now,
                CreateBy = new Guid(getUser.Id),
                TransaksiId = approve.PengajuanId,
                TransaksiNumber = approve.PengajuanNumber,
                BankId = vm.BankId,
                UserId = getUser.Id.ToString(),
                Nominal = vm.GrandTotal
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
            return RedirectToAction("Index", "Persetujuan");
        }
    }
}
