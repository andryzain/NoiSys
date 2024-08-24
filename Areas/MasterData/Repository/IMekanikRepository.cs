using Microsoft.EntityFrameworkCore;
using NoiSys.Areas.MasterData.Models;
using NoiSys.Data;

namespace NoiSys.Areas.MasterData.Repository
{
    public class IMekanikRepository
    {
        private readonly ApplicationDbContext _context;

        public IMekanikRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Mekanik Tambah(Mekanik mekanik)
        {
            _context.Mekaniks.Add(mekanik);
            _context.SaveChanges();
            return mekanik;
        }

        public async Task<Mekanik> GetMekanikById(Guid Id)
        {
            var mekanik = await _context.Mekaniks
                .Include(b => b.Bengkel)
                .SingleOrDefaultAsync(i => i.BengkelId == Id);

            if (mekanik != null)
            {
                var mekanikDetail = new Mekanik()
                {
                    MekanikId = mekanik.MekanikId,
                    KodeMekanik = mekanik.KodeMekanik,
                    NamaMekanik = mekanik.NamaMekanik,
                    BengkelId = mekanik.BengkelId,
                };
                return mekanikDetail;
            }
            return null;
        }

        public async Task<Mekanik> GetMekanikByIdNoTracking(Guid Id)
        {
            return await _context.Mekaniks.AsNoTracking().FirstOrDefaultAsync(a => a.MekanikId == Id);
        }

        public async Task<List<Mekanik>> GetMekaniks()
        {
            return await _context.Mekaniks.OrderBy(p => p.CreateDateTime).Select(mekanik => new Mekanik()
            {
                MekanikId = mekanik.MekanikId,
                KodeMekanik = mekanik.KodeMekanik,
                NamaMekanik = mekanik.NamaMekanik,
                BengkelId = mekanik.BengkelId,
            }).ToListAsync();
        }

        public IEnumerable<Mekanik> GetAllMekanik()
        {
            return _context.Mekaniks
                .Include(b => b.Bengkel)
                .AsNoTracking();
        }

        public Mekanik Update(Mekanik update)
        {
            var mekanik = _context.Mekaniks.Attach(update);
            mekanik.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return update;
        }

        public Mekanik Delete(Guid Id)
        {
            var mekanik = _context.Mekaniks.Find(Id);
            if (mekanik != null)
            {
                _context.Mekaniks.Remove(mekanik);
                _context.SaveChanges();
            }
            return mekanik;
        }
    }
}
