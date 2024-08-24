using Microsoft.EntityFrameworkCore;
using NoiSys.Areas.Administrasi.Models;
using NoiSys.Areas.Keuangan.Models;
using NoiSys.Data;

namespace NoiSys.Areas.Keuangan.Repository
{
    public class IHutangUsahaRepository
    {
        private string _errors = "";
        private readonly ApplicationDbContext _context;

        public IHutangUsahaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public string GetErrors()
        {
            return _errors;
        }

        public HutangUsaha Tambah(HutangUsaha HutangUsaha)
        {
            _context.HutangUsahas.Add(HutangUsaha);
            _context.SaveChanges();
            return HutangUsaha;
        }

        public async Task<HutangUsaha> GetHutangUsahaById(Guid Id)
        {
            var HutangUsaha = _context.HutangUsahas
                .Where(i => i.HutangId == Id)
                .Include(u => u.ApplicationUser)
                .Include(b => b.Bank)
                .FirstOrDefault(p => p.HutangId == Id);

            if (HutangUsaha != null)
            {
                var HutangUsahaDetail = new HutangUsaha()
                {
                    HutangId = HutangUsaha.HutangId,
                    HutangNumber = HutangUsaha.HutangNumber,
                    TransaksiId = HutangUsaha.TransaksiId,
                    TransaksiNumber = HutangUsaha.TransaksiNumber,
                    BankId = HutangUsaha.BankId,
                    UserId = HutangUsaha.UserId,
                    Nominal = HutangUsaha.Nominal,                    
                };
                return HutangUsahaDetail;
            }
            return HutangUsaha;
        }

        public async Task<HutangUsaha> GetHutangUsahaByIdNoTracking(Guid Id)
        {
            return await _context.HutangUsahas.AsNoTracking()
                .Where(i => i.HutangId == Id)                
                .FirstOrDefaultAsync(a => a.HutangId == Id);
        }

        public async Task<List<HutangUsaha>> GetHutangUsahas()
        {
            return await _context.HutangUsahas.OrderBy(p => p.CreateDateTime).Select(HutangUsaha => new HutangUsaha()
            {
                HutangId = HutangUsaha.HutangId,
                HutangNumber = HutangUsaha.HutangNumber,
                TransaksiId = HutangUsaha.TransaksiId,
                TransaksiNumber = HutangUsaha.TransaksiNumber,
                BankId = HutangUsaha.BankId,
                UserId = HutangUsaha.UserId,
                Nominal = HutangUsaha.Nominal,
            }).ToListAsync();
        }

        public async Task<List<HutangUsaha>> GetHutangUsahaDetails()
        {
            return await _context.HutangUsahas.OrderBy(p => p.CreateDateTime).Select(HutangUsaha => new HutangUsaha()
            {
                HutangId = HutangUsaha.HutangId,
                HutangNumber = HutangUsaha.HutangNumber,
                TransaksiId = HutangUsaha.TransaksiId,
                TransaksiNumber = HutangUsaha.TransaksiNumber,
                BankId = HutangUsaha.BankId,
                UserId = HutangUsaha.UserId,
                Nominal = HutangUsaha.Nominal,
            }).ToListAsync();
        }

        public IEnumerable<HutangUsaha> GetAllHutangUsaha()
        {
            return _context.HutangUsahas
                .Include(u => u.ApplicationUser)
                .Include(b => b.Bank)
                .ToList();
        }
    }
}
