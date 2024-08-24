using Microsoft.EntityFrameworkCore;
using NoiSys.Areas.MasterData.Models;
using NoiSys.Data;

namespace NoiSys.Areas.MasterData.Repository
{
    public class IBengkelRepository
    {
        private readonly ApplicationDbContext _context;

        public IBengkelRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Bengkel Tambah(Bengkel Bengkel)
        {
            _context.Bengkels.Add(Bengkel);
            _context.SaveChanges();
            return Bengkel;
        }

        public async Task<Bengkel> GetBengkelById(Guid Id)
        {
            var Bengkel = await _context.Bengkels.FindAsync(Id);

            if (Bengkel != null)
            {
                var BengkelDetail = new Bengkel()
                {
                    BengkelId = Bengkel.BengkelId,
                    KodeBengkel = Bengkel.KodeBengkel,
                    NamaBengkel = Bengkel.NamaBengkel,
                    PenanggungJawab = Bengkel.PenanggungJawab,
                    NamaPemilik = Bengkel.NamaPemilik,
                    Alamat = Bengkel.Alamat,
                    NomorTelepon = Bengkel.NomorTelepon,
                    Email = Bengkel.Email,
                    Keterangan = Bengkel.Keterangan,
                    Foto = Bengkel.Foto
                };
                return BengkelDetail;
            }
            return null;
        }

        public async Task<Bengkel> GetBengkelByIdNoTracking(Guid Id)
        {
            return await _context.Bengkels.AsNoTracking().FirstOrDefaultAsync(a => a.BengkelId == Id);
        }

        public async Task<List<Bengkel>> GetBengkels()
        {
            return await _context.Bengkels.OrderBy(p => p.CreateDateTime).Select(Bengkel => new Bengkel()
            {
                BengkelId = Bengkel.BengkelId,
                KodeBengkel = Bengkel.KodeBengkel,
                NamaBengkel = Bengkel.NamaBengkel,
                PenanggungJawab = Bengkel.PenanggungJawab,
                NamaPemilik = Bengkel.NamaPemilik,
                Alamat = Bengkel.Alamat,
                NomorTelepon = Bengkel.NomorTelepon,
                Email = Bengkel.Email,
                Keterangan = Bengkel.Keterangan,
                Foto = Bengkel.Foto
            }).ToListAsync();
        }

        public IEnumerable<Bengkel> GetAllBengkel()
        {
            return _context.Bengkels.AsNoTracking();
        }

        public Bengkel Update(Bengkel update)
        {
            var Bengkel = _context.Bengkels.Attach(update);
            Bengkel.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return update;
        }

        public Bengkel Delete(Guid Id)
        {
            var Bengkel = _context.Bengkels.Find(Id);
            if (Bengkel != null)
            {
                _context.Bengkels.Remove(Bengkel);
                _context.SaveChanges();
            }
            return Bengkel;
        }
    }
}
