using Microsoft.EntityFrameworkCore;
using NoiSys.Areas.Identity.Data;
using NoiSys.Areas.MasterData.Models;
using NoiSys.Data;
using System.Web.Razor.Parser.SyntaxTree;

namespace NoiSys.Areas.MasterData.Repository
{
    public class IPenggunaRepository
    {
        private readonly ApplicationDbContext _context;

        public IPenggunaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Pengguna Tambah(Pengguna user)
        {
            _context.Penggunas.Add(user);
            _context.SaveChanges();
            return user;
        }

        public async Task<Pengguna> GetPenggunaById(Guid Id)
        {
            var user = await _context.Penggunas
                .Include(l => l.LevelPengguna)
                .SingleOrDefaultAsync(i => i.PenggunaId == Id);

            if (user != null)
            {
                var userDetail = new Pengguna()
                {
                    PenggunaId = user.PenggunaId,
                    KodePengguna = user.KodePengguna,
                    NamaLengkap = user.NamaLengkap,
                    NomorIdentitas = user.NomorIdentitas,
                    LevelId = user.LevelId,
                    TempatLahir = user.TempatLahir,
                    TanggalLahir = user.TanggalLahir,
                    JenisKelamin = user.JenisKelamin,
                    AlamatLengkap = user.AlamatLengkap,
                    NomorHandphone = user.NomorHandphone,
                    Email = user.Email,
                    Foto = user.Foto
                };
                return userDetail;
            }
            return null;
        }

        public async Task<Pengguna> GetPenggunaByIdNoTracking(Guid Id)
        {
            return await _context.Penggunas.AsNoTracking().FirstOrDefaultAsync(a => a.PenggunaId == Id);
        }

        public async Task<List<Pengguna>> GetPenggunas()
        {
            return await _context.Penggunas.OrderBy(p => p.CreateDateTime).Select(user => new Pengguna()
            {
                PenggunaId = user.PenggunaId,
                KodePengguna = user.KodePengguna,
                NamaLengkap = user.NamaLengkap,
                NomorIdentitas = user.NomorIdentitas,
                LevelId = user.LevelId,
                TempatLahir = user.TempatLahir,
                TanggalLahir = user.TanggalLahir,
                JenisKelamin = user.JenisKelamin,
                AlamatLengkap = user.AlamatLengkap,
                NomorHandphone = user.NomorHandphone,
                Email = user.Email,
                Foto = user.Foto
            }).ToListAsync();
        }

        public IEnumerable<Pengguna> GetAllPengguna()
        {
            return _context.Penggunas
                .Include(l => l.LevelPengguna)
                .AsNoTracking();
        }

        public IEnumerable<ApplicationUser> GetAllUserLogin()
        {
            return _context.Users
                .Include(l => l.LevelPengguna)
                .AsNoTracking();
        }

        public Pengguna Update(Pengguna update)
        {
            var user = _context.Penggunas.Attach(update);
            user.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return update;
        }

        public Pengguna Delete(Guid Id)
        {
            var user = _context.Penggunas.Find(Id);
            if (user != null)
            {
                _context.Penggunas.Remove(user);
                _context.SaveChanges();
            }
            return user;
        }
    }
}
