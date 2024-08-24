using Microsoft.EntityFrameworkCore;
using NoiSys.Areas.MasterData.Models;
using NoiSys.Data;

namespace NoiSys.Areas.MasterData.Repository
{
    public class IPrincipalRepository
    {
        private readonly ApplicationDbContext _context;

        public IPrincipalRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Principal Tambah(Principal principal)
        {
            _context.Principals.Add(principal);
            _context.SaveChanges();
            return principal;
        }

        public async Task<Principal> GetPrincipalById(Guid Id)
        {
            var principal = await _context.Principals.FindAsync(Id);

            if (principal != null)
            {
                var principalDetail = new Principal()
                {
                    PrincipalId = principal.PrincipalId,
                    KodePrincipal = principal.KodePrincipal,
                    NamaPrincipal = principal.NamaPrincipal,
                    Alamat = principal.Alamat,
                    NomorTelepon = principal.NomorTelepon,
                    Email = principal.Email,
                    Keterangan = principal.Keterangan                    
                };
                return principalDetail;
            }
            return null;
        }

        public async Task<Principal> GetPrincipalByIdNoTracking(Guid Id)
        {
            return await _context.Principals.AsNoTracking().FirstOrDefaultAsync(a => a.PrincipalId == Id);
        }

        public async Task<List<Principal>> GetPrincipals()
        {
            return await _context.Principals.OrderBy(p => p.CreateDateTime).Select(principal => new Principal()
            {
                PrincipalId = principal.PrincipalId,
                KodePrincipal = principal.KodePrincipal,
                NamaPrincipal = principal.NamaPrincipal,
                Alamat = principal.Alamat,
                NomorTelepon = principal.NomorTelepon,
                Email = principal.Email,
                Keterangan = principal.Keterangan
            }).ToListAsync();
        }

        public IEnumerable<Principal> GetAllPrincipal()
        {
            return _context.Principals.AsNoTracking();
        }

        public Principal Update(Principal update)
        {
            var principal = _context.Principals.Attach(update);
            principal.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return update;
        }

        public Principal Delete(Guid Id)
        {
            var principal = _context.Principals.Find(Id);
            if (principal != null)
            {
                _context.Principals.Remove(principal);
                _context.SaveChanges();
            }
            return principal;
        }
    }
}
