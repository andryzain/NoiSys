using Microsoft.EntityFrameworkCore;
using NoiSys.Areas.MasterData.Models;
using NoiSys.Data;

namespace NoiSys.Areas.MasterData.Repository
{
    public class IKategoriRepository
    {
        private readonly ApplicationDbContext _context;

        public IKategoriRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Kategori Tambah(Kategori Kategori)
        {
            _context.Kategoris.Add(Kategori);
            _context.SaveChanges();
            return Kategori;
        }

        public async Task<Kategori> GetKategoriById(Guid Id)
        {
            var Kategori = await _context.Kategoris.FindAsync(Id);

            if (Kategori != null)
            {
                var KategoriDetail = new Kategori()
                {
                    KategoriId = Kategori.KategoriId,
                    KodeKategori = Kategori.KodeKategori,
                    NamaKategori = Kategori.NamaKategori
                };
                return KategoriDetail;
            }
            return null;
        }

        public async Task<Kategori> GetKategoriByIdNoTracking(Guid Id)
        {
            return await _context.Kategoris.AsNoTracking().FirstOrDefaultAsync(a => a.KategoriId == Id);
        }

        public async Task<List<Kategori>> GetKategoris()
        {
            return await _context.Kategoris.OrderBy(p => p.CreateDateTime).Select(Kategori => new Kategori()
            {
                KategoriId = Kategori.KategoriId,
                KodeKategori = Kategori.KodeKategori,
                NamaKategori = Kategori.NamaKategori,
            }).ToListAsync();
        }

        public IEnumerable<Kategori> GetAllKategori()
        {
            return _context.Kategoris.AsNoTracking();
        }

        public Kategori Update(Kategori update)
        {
            var Kategori = _context.Kategoris.Attach(update);
            Kategori.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return update;
        }

        public Kategori Delete(Guid Id)
        {
            var Kategori = _context.Kategoris.Find(Id);
            if (Kategori != null)
            {
                _context.Kategoris.Remove(Kategori);
                _context.SaveChanges();
            }
            return Kategori;
        }
    }
}
