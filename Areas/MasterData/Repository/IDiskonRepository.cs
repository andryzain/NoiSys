using Microsoft.EntityFrameworkCore;
using NoiSys.Areas.MasterData.Models;
using NoiSys.Data;

namespace NoiSys.Areas.MasterData.Repository
{
    public class IDiskonRepository
    {
        private readonly ApplicationDbContext _context;

        public IDiskonRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Diskon Tambah(Diskon Diskon)
        {
            _context.Diskons.Add(Diskon);
            _context.SaveChanges();
            return Diskon;
        }

        public async Task<Diskon> GetDiskonById(Guid Id)
        {
            var Diskon = await _context.Diskons.FindAsync(Id);

            if (Diskon != null)
            {
                var DiskonDetail = new Diskon()
                {
                    DiskonId = Diskon.DiskonId,
                    KodeDiskon = Diskon.KodeDiskon,
                    Nilai = Diskon.Nilai
                };
                return DiskonDetail;
            }
            return null;
        }

        public async Task<Diskon> GetDiskonByIdNoTracking(Guid Id)
        {
            return await _context.Diskons.AsNoTracking().FirstOrDefaultAsync(a => a.DiskonId == Id);
        }

        public async Task<List<Diskon>> GetDiskons()
        {
            return await _context.Diskons.OrderBy(p => p.CreateDateTime).Select(Diskon => new Diskon()
            {
                DiskonId = Diskon.DiskonId,
                KodeDiskon = Diskon.KodeDiskon,
                Nilai = Diskon.Nilai,
            }).ToListAsync();
        }

        public IEnumerable<Diskon> GetAllDiskon()
        {
            return _context.Diskons.AsNoTracking();
        }

        public Diskon Update(Diskon update)
        {
            var Diskon = _context.Diskons.Attach(update);
            Diskon.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return update;
        }

        public Diskon Delete(Guid Id)
        {
            var Diskon = _context.Diskons.Find(Id);
            if (Diskon != null)
            {
                _context.Diskons.Remove(Diskon);
                _context.SaveChanges();
            }
            return Diskon;
        }
    }
}
