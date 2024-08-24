using Microsoft.EntityFrameworkCore;
using NoiSys.Areas.Keuangan.Models;
using NoiSys.Data;

namespace NoiSys.Areas.Keuangan.Repository
{
    public class IPiutangUsahaRepository
    {
        private string _errors = "";
        private readonly ApplicationDbContext _context;

        public IPiutangUsahaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public string GetErrors()
        {
            return _errors;
        }

        public PiutangUsaha Tambah(PiutangUsaha PiutangUsaha)
        {
            _context.PiutangUsahas.Add(PiutangUsaha);
            _context.SaveChanges();
            return PiutangUsaha;
        }

        public async Task<PiutangUsaha> GetPiutangUsahaById(Guid Id)
        {
            var PiutangUsaha = _context.PiutangUsahas
                .Where(i => i.PiutangId == Id)
                .Include(u => u.ApplicationUser)
                .Include(b => b.Bank)
                .FirstOrDefault(p => p.PiutangId == Id);

            if (PiutangUsaha != null)
            {
                var PiutangUsahaDetail = new PiutangUsaha()
                {
                    PiutangId = PiutangUsaha.PiutangId,
                    PiutangNumber = PiutangUsaha.PiutangNumber,
                    TransaksiId = PiutangUsaha.TransaksiId,
                    TransaksiNumber = PiutangUsaha.TransaksiNumber,
                    BankId = PiutangUsaha.BankId,
                    UserId = PiutangUsaha.UserId,
                    Nominal = PiutangUsaha.Nominal,
                };
                return PiutangUsahaDetail;
            }
            return PiutangUsaha;
        }

        public async Task<PiutangUsaha> GetPiutangUsahaByIdNoTracking(Guid Id)
        {
            return await _context.PiutangUsahas.AsNoTracking()
                .Where(i => i.PiutangId == Id)
                .FirstOrDefaultAsync(a => a.PiutangId == Id);
        }

        public async Task<List<PiutangUsaha>> GetPiutangUsahas()
        {
            return await _context.PiutangUsahas.OrderBy(p => p.CreateDateTime).Select(PiutangUsaha => new PiutangUsaha()
            {
                PiutangId = PiutangUsaha.PiutangId,
                PiutangNumber = PiutangUsaha.PiutangNumber,
                TransaksiId = PiutangUsaha.TransaksiId,
                TransaksiNumber = PiutangUsaha.TransaksiNumber,
                BankId = PiutangUsaha.BankId,
                UserId = PiutangUsaha.UserId,
                Nominal = PiutangUsaha.Nominal,
            }).ToListAsync();
        }

        public async Task<List<PiutangUsaha>> GetPiutangUsahaDetails()
        {
            return await _context.PiutangUsahas.OrderBy(p => p.CreateDateTime).Select(PiutangUsaha => new PiutangUsaha()
            {
                PiutangId = PiutangUsaha.PiutangId,
                PiutangNumber = PiutangUsaha.PiutangNumber,
                TransaksiId = PiutangUsaha.TransaksiId,
                TransaksiNumber = PiutangUsaha.TransaksiNumber,
                BankId = PiutangUsaha.BankId,
                UserId = PiutangUsaha.UserId,
                Nominal = PiutangUsaha.Nominal,
            }).ToListAsync();
        }

        public IEnumerable<PiutangUsaha> GetAllPiutangUsaha()
        {
            return _context.PiutangUsahas
                .Include(u => u.ApplicationUser)
                .Include(b => b.Bank)
                .ToList();
        }
    }
}
