using Microsoft.EntityFrameworkCore;
using NoiSys.Areas.MasterData.Models;
using NoiSys.Data;

namespace NoiSys.Areas.MasterData.Repository
{
    public class ISatuanRepository
    {
        private readonly ApplicationDbContext _context;

        public ISatuanRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Satuan Tambah(Satuan Satuan)
        {
            _context.Satuans.Add(Satuan);
            _context.SaveChanges();
            return Satuan;
        }

        public async Task<Satuan> GetSatuanById(Guid Id)
        {
            var Satuan = await _context.Satuans.FindAsync(Id);

            if (Satuan != null)
            {
                var SatuanDetail = new Satuan()
                {
                    SatuanId = Satuan.SatuanId,
                    KodeSatuan = Satuan.KodeSatuan,
                    NamaSatuan = Satuan.NamaSatuan                    
                };
                return SatuanDetail;
            }
            return null;
        }

        public async Task<Satuan> GetSatuanByIdNoTracking(Guid Id)
        {
            return await _context.Satuans.AsNoTracking().FirstOrDefaultAsync(a => a.SatuanId == Id);
        }

        public async Task<List<Satuan>> GetSatuans()
        {
            return await _context.Satuans.OrderBy(p => p.CreateDateTime).Select(Satuan => new Satuan()
            {
                SatuanId = Satuan.SatuanId,
                KodeSatuan = Satuan.KodeSatuan,
                NamaSatuan = Satuan.NamaSatuan,                
            }).ToListAsync();
        }

        public IEnumerable<Satuan> GetAllSatuan()
        {
            return _context.Satuans.AsNoTracking();
        }

        public Satuan Update(Satuan update)
        {
            var Satuan = _context.Satuans.Attach(update);
            Satuan.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return update;
        }

        public Satuan Delete(Guid Id)
        {
            var Satuan = _context.Satuans.Find(Id);
            if (Satuan != null)
            {
                _context.Satuans.Remove(Satuan);
                _context.SaveChanges();
            }
            return Satuan;
        }
    }
}
