using Microsoft.EntityFrameworkCore;
using NoiSys.Areas.Administrasi.Models;
using NoiSys.Data;

namespace NoiSys.Areas.Administrasi.Repository
{
    public class IPembayaranRepository
    {
        private string _errors = "";
        private readonly ApplicationDbContext _context;

        public IPembayaranRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public string GetErrors()
        {
            return _errors;
        }

        public Pembayaran Tambah(Pembayaran Pembayaran)
        {
            _context.Pembayarans.Add(Pembayaran);
            _context.SaveChanges();
            return Pembayaran;
        }

        public async Task<Pembayaran> GetPembayaranById(Guid Id)
        {
            var pembayaran = _context.Pembayarans
                .Where(i => i.PaymentId == Id)
                .Include(i => i.Invoice)
                .Include(u => u.ApplicationUser)
                .Include(p => p.Pengguna)
                .Include(b => b.Bengkel)
                .Include(n => n.Bank)
                .FirstOrDefault(p => p.PaymentId == Id);

            if (pembayaran != null)
            {
                var pembayaranDetail = new Pembayaran()
                {
                    PaymentId = pembayaran.PaymentId,
                    PaymentNumber = pembayaran.PaymentNumber,
                    InvoiceId = pembayaran.InvoiceId,
                    InvoiceNumber = pembayaran.InvoiceNumber,
                    BankId = pembayaran.BankId,
                    UserId = pembayaran.UserId,
                    PenggunaId = pembayaran.PenggunaId,
                    Termin = pembayaran.Termin,
                    BengkelId = pembayaran.BengkelId,
                    Status = pembayaran.Status,
                    GrandTotal = pembayaran.GrandTotal,
                    Keterangan = pembayaran.Keterangan,
                };
                return pembayaranDetail;
            }
            return pembayaran;
        }

        public async Task<Pembayaran> GetPembayaranByIdNoTracking(Guid Id)
        {
            return await _context.Pembayarans.AsNoTracking()
                .Where(i => i.PaymentId == Id)
                .Include(i => i.Invoice)
                .Include(u => u.ApplicationUser)
                .Include(p => p.Pengguna)
                .Include(b => b.Bengkel)
                .Include(n => n.Bank)
                .FirstOrDefaultAsync(a => a.PaymentId == Id);
        }

        public async Task<List<Pembayaran>> GetPembayarans()
        {
            return await _context.Pembayarans.OrderBy(p => p.CreateDateTime).Select(pembayaran => new Pembayaran()
            {
                PaymentId = pembayaran.PaymentId,
                PaymentNumber = pembayaran.PaymentNumber,
                InvoiceId = pembayaran.InvoiceId,
                InvoiceNumber = pembayaran.InvoiceNumber,
                BankId = pembayaran.BankId,
                UserId = pembayaran.UserId,
                PenggunaId = pembayaran.PenggunaId,
                Termin = pembayaran.Termin,
                BengkelId = pembayaran.BengkelId,
                Status = pembayaran.Status,
                GrandTotal = pembayaran.GrandTotal,
                Keterangan = pembayaran.Keterangan,
            }).ToListAsync();
        }
        
        public IEnumerable<Pembayaran> GetAllPembayaran()
        {
            return _context.Pembayarans
                .Include(i => i.Invoice)
                .Include(u => u.ApplicationUser)
                .Include(p => p.Pengguna)
                .Include(b => b.Bengkel)
                .Include(n => n.Bank)
                .ToList();
        }
    }
}
