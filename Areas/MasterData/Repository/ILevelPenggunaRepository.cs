using Microsoft.EntityFrameworkCore;
using NoiSys.Areas.MasterData.Models;
using NoiSys.Data;

namespace NoiSys.Areas.MasterData.Repository
{
    public class ILevelPenggunaRepository
    {
        private readonly ApplicationDbContext _context;

        public ILevelPenggunaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public LevelPengguna Tambah(LevelPengguna LevelPengguna)
        {
            _context.LevelPenggunas.Add(LevelPengguna);
            _context.SaveChanges();
            return LevelPengguna;
        }

        public async Task<LevelPengguna> GetLevelPenggunaById(Guid Id)
        {
            var LevelPengguna = await _context.LevelPenggunas.FindAsync(Id);

            if (LevelPengguna != null)
            {
                var LevelPenggunaDetail = new LevelPengguna()
                {
                    LevelId = LevelPengguna.LevelId,
                    KodeLevel = LevelPengguna.KodeLevel,
                    NamaLevel = LevelPengguna.NamaLevel,
                    Persentase = LevelPengguna.Persentase,
                    Keterangan = LevelPengguna.Keterangan,
                };
                return LevelPenggunaDetail;
            }
            return null;
        }

        public async Task<LevelPengguna> GetLevelPenggunaByIdNoTracking(Guid Id)
        {
            return await _context.LevelPenggunas.AsNoTracking().FirstOrDefaultAsync(a => a.LevelId == Id);
        }

        public async Task<List<LevelPengguna>> GetLevelPenggunas()
        {
            return await _context.LevelPenggunas.OrderBy(p => p.CreateDateTime).Select(LevelPengguna => new LevelPengguna()
            {
                LevelId = LevelPengguna.LevelId,
                KodeLevel = LevelPengguna.KodeLevel,
                NamaLevel = LevelPengguna.NamaLevel,
                Persentase = LevelPengguna.Persentase,
                Keterangan = LevelPengguna.Keterangan,
            }).ToListAsync();
        }

        public IEnumerable<LevelPengguna> GetAllLevelPengguna()
        {
            return _context.LevelPenggunas.AsNoTracking();
        }

        public LevelPengguna Update(LevelPengguna update)
        {
            var LevelPengguna = _context.LevelPenggunas.Attach(update);
            LevelPengguna.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return update;
        }

        public LevelPengguna Delete(Guid Id)
        {
            var LevelPengguna = _context.LevelPenggunas.Find(Id);
            if (LevelPengguna != null)
            {
                _context.LevelPenggunas.Remove(LevelPengguna);
                _context.SaveChanges();
            }
            return LevelPengguna;
        }
    }
}
