using Microsoft.EntityFrameworkCore;
using NoiSys.Areas.Reimbursment.Models;
using NoiSys.Data;

namespace NoiSys.Areas.Reimbursment.Repository
{
    public class IPembayaranReimbursmentRepository
    {
        private string _errors = "";
        private readonly ApplicationDbContext _context;

        public IPembayaranReimbursmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public string GetErrors()
        {
            return _errors;
        }

        public PembayaranReimbursment Tambah(PembayaranReimbursment pembayaran)
        {
            _context.PembayaranReimbursments.Add(pembayaran);
            _context.SaveChanges();
            return pembayaran;
        }

        public async Task<PembayaranReimbursment> GetPembayaranById(Guid Id)
        {
            var Pembayaran = _context.PembayaranReimbursments
                .Where(i => i.PaymentId == Id)
                .Include(u => u.ApplicationUser)
                .Include(b => b.Bank)
                .Include(p => p.Pengajuan)
                .FirstOrDefault(p => p.PaymentId == Id);

            if (Pembayaran != null)
            {
                var PembayaranDetail = new PembayaranReimbursment()
                {
                    PaymentId = Pembayaran.PaymentId,
                    PaymentNumber = Pembayaran.PaymentNumber,
                    PengajuanId = Pembayaran.PengajuanId,
                    PengajuanNumber = Pembayaran.PengajuanNumber,
                    UserId = Pembayaran.UserId,                    
                    BankId = Pembayaran.BankId,
                    NomorRekening = Pembayaran.NomorRekening,
                    AtasNama = Pembayaran.AtasNama,
                    Status = Pembayaran.Status,
                    GrandTotal = Pembayaran.GrandTotal,
                    Keterangan = Pembayaran.Keterangan,
                };
                return PembayaranDetail;
            }
            return Pembayaran;
        }

        public async Task<PembayaranReimbursment> GetPembayaranByIdNoTracking(Guid Id)
        {
            return await _context.PembayaranReimbursments.AsNoTracking().FirstOrDefaultAsync(a => a.PaymentId == Id);
        }

        public async Task<List<PembayaranReimbursment>> GetPembayarans()
        {
            return await _context.PembayaranReimbursments.OrderBy(p => p.CreateDateTime).Select(Pembayaran => new PembayaranReimbursment()
            {
                PaymentId = Pembayaran.PaymentId,
                PaymentNumber = Pembayaran.PaymentNumber,
                PengajuanId = Pembayaran.PengajuanId,
                PengajuanNumber = Pembayaran.PengajuanNumber,
                UserId = Pembayaran.UserId,
                BankId = Pembayaran.BankId,
                NomorRekening = Pembayaran.NomorRekening,
                AtasNama = Pembayaran.AtasNama,
                Status = Pembayaran.Status,
                GrandTotal = Pembayaran.GrandTotal,
                Keterangan = Pembayaran.Keterangan,
            }).ToListAsync();
        }

        public IEnumerable<PembayaranReimbursment> GetAllPembayaran()
        {
            return _context.PembayaranReimbursments
                .Include(u => u.ApplicationUser)
                .Include(b => b.Bank)
                .Include(p => p.Pengajuan)
                .ToList();
        }

        public async Task<PembayaranReimbursment> Update(PembayaranReimbursment update)
        {
            var Pembayaran = _context.PembayaranReimbursments.Attach(update);
            Pembayaran.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return update;
        }

        public PembayaranReimbursment Delete(Guid Id)
        {
            var Pembayaran = _context.PembayaranReimbursments.Find(Id);
            if (Pembayaran != null)
            {
                _context.PembayaranReimbursments.Remove(Pembayaran);
                _context.SaveChanges();
            }
            return Pembayaran;
        }
    }
}
