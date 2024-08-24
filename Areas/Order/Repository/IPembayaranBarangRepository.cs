using Microsoft.EntityFrameworkCore;
using NoiSys.Areas.Order.Models;
using NoiSys.Data;

namespace NoiSys.Areas.Order.Repository
{
    public class IPembayaranBarangRepository
    {
        private string _errors = "";
        private readonly ApplicationDbContext _context;

        public IPembayaranBarangRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public string GetErrors()
        {
            return _errors;
        }

        public PembayaranBarang Tambah(PembayaranBarang PembayaranBarang)
        {
            _context.PembayaranBarangs.Add(PembayaranBarang);
            _context.SaveChanges();
            return PembayaranBarang;
        }

        public async Task<PembayaranBarang> GetPembayaranBarangById(Guid Id)
        {
            var PembayaranBarang = _context.PembayaranBarangs
                .Where(i => i.PaymentId == Id)
                .Include(u => u.ApplicationUser)
                .Include(p => p.Pengguna)
                .Include(s => s.DisetujuiOleh)
                .Include(b => b.Bank)
                .Include(l => l.Pembelian)
                .FirstOrDefault(p => p.PaymentId == Id);

            if (PembayaranBarang != null)
            {
                var PembayaranBarangDetail = new PembayaranBarang()
                {
                    PaymentId = PembayaranBarang.PaymentId,
                    PaymentNumber = PembayaranBarang.PaymentNumber,
                    PembelianId = PembayaranBarang.PembelianId,
                    PembelianNumber = PembayaranBarang.PembelianNumber,
                    UserId = PembayaranBarang.UserId,
                    PenggunaId = PembayaranBarang.PenggunaId,
                    DisetujuiOlehId = PembayaranBarang.DisetujuiOlehId,
                    Termin = PembayaranBarang.Termin,
                    Status = PembayaranBarang.Status,
                    GrandTotal = PembayaranBarang.GrandTotal,
                    Keterangan = PembayaranBarang.Keterangan,                    
                };
                return PembayaranBarangDetail;
            }
            return PembayaranBarang;
        }

        public async Task<PembayaranBarang> GetPembayaranBarangByIdNoTracking(Guid Id)
        {
            return await _context.PembayaranBarangs.AsNoTracking().Where(i => i.PaymentId == Id).FirstOrDefaultAsync(a => a.PaymentId == Id);
        }

        public async Task<List<PembayaranBarang>> GetPembayaranBarangs()
        {
            return await _context.PembayaranBarangs.OrderBy(p => p.CreateDateTime).Select(PembayaranBarang => new PembayaranBarang()
            {
                PaymentId = PembayaranBarang.PaymentId,
                PaymentNumber = PembayaranBarang.PaymentNumber,
                PembelianId = PembayaranBarang.PembelianId,
                PembelianNumber = PembayaranBarang.PembelianNumber,
                UserId = PembayaranBarang.UserId,
                PenggunaId = PembayaranBarang.PenggunaId,
                DisetujuiOlehId = PembayaranBarang.DisetujuiOlehId,
                Termin = PembayaranBarang.Termin,
                Status = PembayaranBarang.Status,
                GrandTotal = PembayaranBarang.GrandTotal,
                Keterangan = PembayaranBarang.Keterangan,
            }).ToListAsync();
        }

        public IEnumerable<PembayaranBarang> GetAllPembayaranBarang()
        {
            return _context.PembayaranBarangs
                .Include(u => u.ApplicationUser)
                .Include(p => p.Pengguna)
                .Include(s => s.DisetujuiOleh)
                .Include(b => b.Bank)
                .Include(l => l.Pembelian)
                .ToList();
        }        
    }
}
