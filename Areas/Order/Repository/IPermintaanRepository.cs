using Microsoft.EntityFrameworkCore;
using NoiSys.Areas.Order.Models;
using NoiSys.Areas.Transaksi.Models;
using NoiSys.Data;

namespace NoiSys.Areas.Order.Repository
{
    public class IPermintaanRepository
    {
        private string _errors = "";
        private readonly ApplicationDbContext _context;

        public IPermintaanRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public string GetErrors()
        {
            return _errors;
        }

        public Permintaan Tambah(Permintaan Permintaan)
        {
            _context.Permintaans.Add(Permintaan);
            _context.SaveChanges();
            return Permintaan;
        }

        public async Task<Permintaan> GetPermintaanById(Guid Id)
        {
            var Permintaan = _context.Permintaans
                .Where(i => i.PermintaanId == Id)
                .Include(d => d.PermintaanDetails)
                .Include(u => u.ApplicationUser)
                .Include(p => p.Pengguna)
                .Include(s => s.DisetujuiOleh)
                .FirstOrDefault(p => p.PermintaanId == Id);

            if (Permintaan != null)
            {
                var PermintaanDetail = new Permintaan()
                {
                    PermintaanId = Permintaan.PermintaanId,
                    PermintaanNumber = Permintaan.PermintaanNumber,
                    UserId = Permintaan.UserId,
                    ApplicationUser = Permintaan.ApplicationUser,
                    DisetujuiOlehId = Permintaan.DisetujuiOlehId,
                    DisetujuiOleh = Permintaan.DisetujuiOleh,
                    PenggunaId = Permintaan.PenggunaId,
                    Pengguna = Permintaan.Pengguna,
                    Termin = Permintaan.Termin,
                    Status = Permintaan.Status,
                    QtyTotal = Permintaan.QtyTotal,
                    GrandTotal = Permintaan.GrandTotal,
                    Keterangan = Permintaan.Keterangan,
                    PermintaanDetails = Permintaan.PermintaanDetails,
                };
                return PermintaanDetail;
            }
            return Permintaan;
        }

        public async Task<Permintaan> GetPermintaanByIdNoTracking(Guid Id)
        {
            return await _context.Permintaans.AsNoTracking()
                .Where(i => i.PermintaanId == Id)
                .Include(d => d.PermintaanDetails)
                .Include(u => u.ApplicationUser)
                .Include(p => p.Pengguna)
                .Include(s => s.DisetujuiOleh)
                .FirstOrDefaultAsync(a => a.PermintaanId == Id);
        }

        public async Task<List<Permintaan>> GetPermintaans()
        {
            return await _context.Permintaans.OrderBy(p => p.CreateDateTime).Select(Permintaan => new Permintaan()
            {
                PermintaanId = Permintaan.PermintaanId,
                PermintaanNumber = Permintaan.PermintaanNumber,
                UserId = Permintaan.UserId,
                DisetujuiOlehId = Permintaan.DisetujuiOlehId,
                PenggunaId = Permintaan.PenggunaId,
                Termin = Permintaan.Termin,
                Status = Permintaan.Status,
                QtyTotal = Permintaan.QtyTotal,
                GrandTotal = Permintaan.GrandTotal,
                Keterangan = Permintaan.Keterangan,
                PermintaanDetails = Permintaan.PermintaanDetails,
            }).ToListAsync();
        }

        public IEnumerable<Permintaan> GetAllPermintaan()
        {
            return _context.Permintaans
                .Include(d => d.PermintaanDetails)
                .Include(u => u.ApplicationUser)
                .Include(p => p.Pengguna)
                .Include(s => s.DisetujuiOleh)
                .ToList();
        }

        public async Task<Permintaan> Update(Permintaan update)
        {
            List<PermintaanDetail> PermintaanDetails = _context.PermintaanDetails.Where(d => d.PermintaanId == update.PermintaanId).ToList();
            _context.PermintaanDetails.RemoveRange(PermintaanDetails);
            _context.SaveChanges();

            var Permintaan = _context.Permintaans.Attach(update);
            Permintaan.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.PermintaanDetails.AddRangeAsync(update.PermintaanDetails);
            _context.SaveChanges();
            return update;
        }

        public Permintaan Delete(Guid Id)
        {
            var Permintaan = _context.Permintaans.Find(Id);
            if (Permintaan != null)
            {
                _context.Permintaans.Remove(Permintaan);
                _context.SaveChanges();
            }
            return Permintaan;
        }
    }
}
