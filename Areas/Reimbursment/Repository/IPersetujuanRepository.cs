using Microsoft.EntityFrameworkCore;
using NoiSys.Areas.Reimbursment.Models;
using NoiSys.Data;

namespace NoiSys.Areas.Reimbursment.Repository
{
    public class IPersetujuanRepository
    {
        private string _errors = "";
        private readonly ApplicationDbContext _context;

        public IPersetujuanRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public string GetErrors()
        {
            return _errors;
        }

        public Persetujuan Tambah(Persetujuan Persetujuan)
        {
            _context.Persetujuans.Add(Persetujuan);
            _context.SaveChanges();
            return Persetujuan;
        }

        public async Task<Persetujuan> GetPersetujuanById(Guid Id)
        {
            var Persetujuan = _context.Persetujuans
                .Where(i => i.PersetujuanId == Id)
                .Include(u => u.ApplicationUser)
                .Include(b => b.Bank)
                .Include(p => p.Pengajuan)
                .FirstOrDefault(p => p.PersetujuanId == Id);

            if (Persetujuan != null)
            {
                var PersetujuanDetail = new Persetujuan()
                {
                    PersetujuanId = Persetujuan.PersetujuanId,
                    PengajuanId = Persetujuan.PengajuanId,
                    PengajuanNumber = Persetujuan.PengajuanNumber,
                    UserId = Persetujuan.UserId,
                    ApproveDate = Persetujuan.ApproveDate,
                    ApproveBy = Persetujuan.ApproveBy,
                    BankId = Persetujuan.BankId,
                    NomorRekening = Persetujuan.NomorRekening,
                    AtasNama = Persetujuan.AtasNama,
                    Status = Persetujuan.Status,
                    Keterangan = Persetujuan.Keterangan,
                    QtyTotal = Persetujuan.QtyTotal,
                    GrandTotal = Persetujuan.GrandTotal
                };
                return PersetujuanDetail;
            }
            return Persetujuan;
        }

        public async Task<Persetujuan> GetPersetujuanByIdNoTracking(Guid Id)
        {
            return await _context.Persetujuans.AsNoTracking().FirstOrDefaultAsync(a => a.PersetujuanId == Id);
        }

        public async Task<List<Persetujuan>> GetPersetujuans()
        {
            return await _context.Persetujuans.OrderBy(p => p.CreateDateTime).Select(Persetujuan => new Persetujuan()
            {
                PersetujuanId = Persetujuan.PersetujuanId,
                PengajuanId = Persetujuan.PengajuanId,
                PengajuanNumber = Persetujuan.PengajuanNumber,
                UserId = Persetujuan.UserId,
                ApproveDate = Persetujuan.ApproveDate,
                ApproveBy = Persetujuan.ApproveBy,
                BankId = Persetujuan.BankId,
                NomorRekening = Persetujuan.NomorRekening,
                AtasNama = Persetujuan.AtasNama,
                Status = Persetujuan.Status,
                Keterangan = Persetujuan.Keterangan,
                QtyTotal = Persetujuan.QtyTotal,
                GrandTotal = Persetujuan.GrandTotal
            }).ToListAsync();
        }

        public IEnumerable<Persetujuan> GetAllPersetujuan()
        {
            return _context.Persetujuans
                .Include(u => u.ApplicationUser)
                .Include(b => b.Bank)
                .Include(p => p.Pengajuan)
                .ToList();
        }

        public async Task<Persetujuan> Update(Persetujuan update)
        {
            var Persetujuan = _context.Persetujuans.Attach(update);
            Persetujuan.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return update;
        }

        public Persetujuan Delete(Guid Id)
        {
            var Persetujuan = _context.Persetujuans.Find(Id);
            if (Persetujuan != null)
            {
                _context.Persetujuans.Remove(Persetujuan);
                _context.SaveChanges();
            }
            return Persetujuan;
        }
    }
}
