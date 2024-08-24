using Microsoft.EntityFrameworkCore;
using NoiSys.Areas.Reimbursment.Models;
using NoiSys.Areas.Transaksi.Models;
using NoiSys.Data;

namespace NoiSys.Areas.Reimbursment.Repository
{
    public class IPengajuanRepository
    {
        private string _errors = "";
        private readonly ApplicationDbContext _context;

        public IPengajuanRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public string GetErrors()
        {
            return _errors;
        }

        public Pengajuan Tambah(Pengajuan Pengajuan)
        {
            _context.Pengajuans.Add(Pengajuan);
            _context.SaveChanges();
            return Pengajuan;
        }

        public async Task<Pengajuan> GetPengajuanById(Guid Id)
        {
            var Pengajuan = _context.Pengajuans
                .Where(i => i.PengajuanId == Id)
                .Include(d => d.PengajuanDetails)
                .Include(u => u.ApplicationUser)
                .Include(b => b.Bank)
                .FirstOrDefault(p => p.PengajuanId == Id);

            if (Pengajuan != null)
            {
                var PengajuanDetail = new Pengajuan()
                {
                    PengajuanId = Pengajuan.PengajuanId,
                    PengajuanNumber = Pengajuan.PengajuanNumber,
                    UserId = Pengajuan.UserId,
                    ApplicationUser = Pengajuan.ApplicationUser,
                    BankId = Pengajuan.BankId,
                    Bank = Pengajuan.Bank,
                    NomorRekening = Pengajuan.NomorRekening,
                    AtasNama = Pengajuan.AtasNama,
                    Status = Pengajuan.Status,
                    QtyTotal = Pengajuan.QtyTotal,
                    GrandTotal = Pengajuan.GrandTotal,
                    PengajuanDetails = Pengajuan.PengajuanDetails,
                };
                return PengajuanDetail;
            }
            return Pengajuan;
        }

        public async Task<Pengajuan> GetPengajuanByIdNoTracking(Guid Id)
        {
            return await _context.Pengajuans.AsNoTracking()
                .Where(i => i.PengajuanId == Id)
                .FirstOrDefaultAsync(a => a.PengajuanId == Id);
        }

        public async Task<List<Pengajuan>> GetPengajuans()
        {
            return await _context.Pengajuans.OrderBy(p => p.CreateDateTime).Select(Pengajuan => new Pengajuan()
            {
                PengajuanId = Pengajuan.PengajuanId,
                PengajuanNumber = Pengajuan.PengajuanNumber,
                UserId = Pengajuan.UserId,
                BankId = Pengajuan.BankId,
                NomorRekening = Pengajuan.NomorRekening,
                AtasNama = Pengajuan.AtasNama,
                Status = Pengajuan.Status,
                QtyTotal = Pengajuan.QtyTotal,
                GrandTotal = Pengajuan.GrandTotal,
                PengajuanDetails = Pengajuan.PengajuanDetails,
            }).ToListAsync();
        }

        public IEnumerable<Pengajuan> GetAllPengajuan()
        {
            return _context.Pengajuans
                .Include(d => d.PengajuanDetails)
                .Include(u => u.ApplicationUser)
                .Include(b => b.Bank)
                .ToList();
        }

        public async Task<Pengajuan> Update(Pengajuan update)
        {
            List<PengajuanDetail> PengajuanDetails = _context.PengajuanDetails.Where(d => d.PengajuanId == update.PengajuanId).ToList();
            _context.PengajuanDetails.RemoveRange(PengajuanDetails);
            _context.SaveChanges();

            var Pengajuan = _context.Pengajuans.Attach(update);
            Pengajuan.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.PengajuanDetails.AddRangeAsync(update.PengajuanDetails);
            _context.SaveChanges();
            return update;
        }

        public Pengajuan Delete(Guid Id)
        {
            var Pengajuan = _context.Pengajuans.Find(Id);
            if (Pengajuan != null)
            {
                _context.Pengajuans.Remove(Pengajuan);
                _context.SaveChanges();
            }
            return Pengajuan;
        }
    }
}
