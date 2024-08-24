using Microsoft.EntityFrameworkCore;
using NoiSys.Areas.Order.Models;
using NoiSys.Data;

namespace NoiSys.Areas.Order.Repository
{
    public class IPembelianRepository
    {
        private string _errors = "";
        private readonly ApplicationDbContext _context;

        public IPembelianRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public string GetErrors()
        {
            return _errors;
        }

        public Pembelian Tambah(Pembelian Pembelian)
        {
            _context.Pembelians.Add(Pembelian);
            _context.SaveChanges();
            return Pembelian;
        }

        public async Task<Pembelian> GetPembelianById(Guid Id)
        {
            var Pembelian = _context.Pembelians
                .Where(i => i.PembelianId == Id)
                .Include(d => d.PembelianDetails)
                .Include(u => u.ApplicationUser)
                .Include(p => p.Pengguna)
                .Include(p => p.DisetujuiOleh)
                .FirstOrDefault(p => p.PembelianId == Id);

            if (Pembelian != null)
            {
                var PembelianDetail = new Pembelian()
                {
                    PembelianId = Pembelian.PembelianId,
                    PembelianNumber = Pembelian.PembelianNumber,
                    PermintaanId = Pembelian.PermintaanId,
                    PermintaanNumber = Pembelian.PermintaanNumber,
                    UserId = Pembelian.UserId,
                    ApplicationUser = Pembelian.ApplicationUser,
                    PenggunaId = Pembelian.PenggunaId,
                    Pengguna = Pembelian.Pengguna,
                    DisetujuiOlehId = Pembelian.DisetujuiOlehId,
                    DisetujuiOleh = Pembelian.DisetujuiOleh,
                    Termin = Pembelian.Termin,
                    Status = Pembelian.Status,
                    QtyTotal = Pembelian.QtyTotal,
                    GrandTotal = Pembelian.GrandTotal,
                    Keterangan = Pembelian.Keterangan,
                    PembelianDetails = Pembelian.PembelianDetails,
                };
                return PembelianDetail;
            }
            return Pembelian;
        }

        public async Task<Pembelian> GetPembelianByIdNoTracking(Guid Id)
        {
            return await _context.Pembelians.AsNoTracking().Where(i => i.PembelianId == Id).FirstOrDefaultAsync(a => a.PembelianId == Id);
        }

        public async Task<List<Pembelian>> GetPembelians()
        {
            return await _context.Pembelians.OrderBy(p => p.CreateDateTime).Select(Pembelian => new Pembelian()
            {
                PembelianId = Pembelian.PembelianId,
                PembelianNumber = Pembelian.PembelianNumber,
                PermintaanId = Pembelian.PermintaanId,
                PermintaanNumber = Pembelian.PermintaanNumber,
                UserId = Pembelian.UserId,
                PenggunaId = Pembelian.PenggunaId,
                DisetujuiOlehId = Pembelian.DisetujuiOlehId,
                Termin = Pembelian.Termin,
                Status = Pembelian.Status,
                QtyTotal = Pembelian.QtyTotal,
                GrandTotal = Pembelian.GrandTotal,
                Keterangan = Pembelian.Keterangan,
                PembelianDetails = Pembelian.PembelianDetails,
            }).ToListAsync();
        }

        public async Task<List<Pembelian>> GetPembelianFilters()
        {
            return await _context.Pembelians.Where(p => p.Status == "Diproses").OrderBy(p => p.CreateDateTime).Select(Pembelian => new Pembelian()
            {
                PembelianId = Pembelian.PembelianId,
                PembelianNumber = Pembelian.PembelianNumber,
                PermintaanId = Pembelian.PermintaanId,
                PermintaanNumber = Pembelian.PermintaanNumber,
                UserId = Pembelian.UserId,
                PenggunaId = Pembelian.PenggunaId,
                DisetujuiOlehId = Pembelian.DisetujuiOlehId,
                Termin = Pembelian.Termin,
                Status = Pembelian.Status,
                QtyTotal = Pembelian.QtyTotal,
                GrandTotal = Pembelian.GrandTotal,
                Keterangan = Pembelian.Keterangan,
                PembelianDetails = Pembelian.PembelianDetails,
            }).ToListAsync();
        }

        public IEnumerable<Pembelian> GetAllPembelian()
        {
            return _context.Pembelians
                .Include(d => d.PembelianDetails)
                .Include(u => u.ApplicationUser)
                .Include(p => p.Pengguna)
                .Include(p => p.DisetujuiOleh)
                .ToList();
        }

        public async Task<Pembelian> Update(Pembelian update)
        {
            List<PembelianDetail> PembelianDetails = _context.PembelianDetails.Where(d => d.PembelianId == update.PembelianId).ToList();
            _context.PembelianDetails.RemoveRange(PembelianDetails);
            _context.SaveChanges();

            var Pembelian = _context.Pembelians.Attach(update);
            Pembelian.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.PembelianDetails.AddRangeAsync(update.PembelianDetails);
            _context.SaveChanges();
            return update;
        }

        public Pembelian Delete(Guid Id)
        {
            var Pembelian = _context.Pembelians.Find(Id);
            if (Pembelian != null)
            {
                _context.Pembelians.Remove(Pembelian);
                _context.SaveChanges();
            }
            return Pembelian;
        }
    }
}
