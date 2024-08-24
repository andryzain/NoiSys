using Microsoft.EntityFrameworkCore;
using NoiSys.Areas.MasterData.Models;
using NoiSys.Data;

namespace NoiSys.Areas.MasterData.Repository
{
    public class IMetodePembayaranRepository
    {
        private readonly ApplicationDbContext _context;

        public IMetodePembayaranRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public MetodePembayaran Tambah(MetodePembayaran MetodePembayaran)
        {
            _context.MetodePembayarans.Add(MetodePembayaran);
            _context.SaveChanges();
            return MetodePembayaran;
        }

        public async Task<MetodePembayaran> GetMetodePembayaranById(Guid Id)
        {
            var MetodePembayaran = await _context.MetodePembayarans.FindAsync(Id);

            if (MetodePembayaran != null)
            {
                var MetodePembayaranDetail = new MetodePembayaran()
                {
                    MetodePembayaranId = MetodePembayaran.MetodePembayaranId,
                    KodeMetodePembayaran = MetodePembayaran.KodeMetodePembayaran,
                    NamaMetodePembayaran = MetodePembayaran.NamaMetodePembayaran
                };
                return MetodePembayaranDetail;
            }
            return null;
        }

        public async Task<MetodePembayaran> GetMetodePembayaranByIdNoTracking(Guid Id)
        {
            return await _context.MetodePembayarans.AsNoTracking().FirstOrDefaultAsync(a => a.MetodePembayaranId == Id);
        }

        public async Task<List<MetodePembayaran>> GetMetodePembayarans()
        {
            return await _context.MetodePembayarans.OrderBy(p => p.CreateDateTime).Select(MetodePembayaran => new MetodePembayaran()
            {
                MetodePembayaranId = MetodePembayaran.MetodePembayaranId,
                KodeMetodePembayaran = MetodePembayaran.KodeMetodePembayaran,
                NamaMetodePembayaran = MetodePembayaran.NamaMetodePembayaran,
            }).ToListAsync();
        }

        public IEnumerable<MetodePembayaran> GetAllMetodePembayaran()
        {
            return _context.MetodePembayarans.AsNoTracking();
        }

        public MetodePembayaran Update(MetodePembayaran update)
        {
            var MetodePembayaran = _context.MetodePembayarans.Attach(update);
            MetodePembayaran.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return update;
        }

        public MetodePembayaran Delete(Guid Id)
        {
            var MetodePembayaran = _context.MetodePembayarans.Find(Id);
            if (MetodePembayaran != null)
            {
                _context.MetodePembayarans.Remove(MetodePembayaran);
                _context.SaveChanges();
            }
            return MetodePembayaran;
        }
    }
}
